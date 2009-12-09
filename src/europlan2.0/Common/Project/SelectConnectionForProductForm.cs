using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Star.SettingsXpress;

namespace Europlan.Common {
	public partial class SelectConnectionForProductForm : Form {

		private TreeNode rootNode;
		private Dictionary<object, TreeNode> nodes = new Dictionary<object, TreeNode>();
		private PlannedProduct product = null;

		public SelectConnectionForProductForm(PlannedProduct product, Floor floor) {
			InitializeComponent();
			this.tvDistributors.Nodes.Clear();
			this.product = product;
			DistributorList distributors = floor.GetAllAvailableDistributors();
			TreeNode[] distributorNodes = new TreeNode[distributors.Count];
			int i = 0;
			foreach (Distributor d in distributors) {
				distributorNodes[i] = new TreeNode(d.Id + ": " + d.Name);
				distributorNodes[i].Tag = d;
				this.nodes.Add(d, distributorNodes[i]);
				foreach (PlannedProduct p in d.PlannedConnectedProducts) {
					if (p.Product.PlannedCircuits == null) {
						TreeNode node = new TreeNode(p.Node.Text + " in " + p.Product.AssociatedRoom.ToString());
						node.Tag = p;
						distributorNodes[i].Nodes.Add(node);
					} else {
						int j = 0;
						foreach (Circuit c in p.Product.PlannedCircuits) {
							Circuit.CircuitConnection cc = p.Product.GetCircuitConnected(j);
							j++;
							string label = p.Node.Text + (p.Product.PlannedCircuits.Count > 1 ? " (HK" + j.ToString() + ")" : "") + " in " + p.Product.AssociatedRoom.ToString();
							if (cc != null) {
								label += ", " + cc.OtherCircuit.PlannedProduct.Node.Text + (cc.OtherCircuit.PlannedProduct.Product.PlannedCircuits.Count > 1 ? " (HK" + cc.OtherCircuit.NrOfCircuit + ")" : "") + " in " + cc.OtherCircuit.PlannedProduct.Product.AssociatedRoom.ToString();
							}
							TreeNode node = new TreeNode(label);
							node.Tag = c;
							distributorNodes[i].Nodes.Add(node);
							node.Checked = true;
						}
					}
				}
				i++;
			}
			rootNode = new TreeNode("Projekt", distributorNodes);
			this.tvDistributors.Nodes.Add(rootNode);
			this.tvDistributors.ExpandAll();
			// TODO select old
		}

		private void tvDistributors_AfterSelect(object sender, TreeViewEventArgs e) {
			lblInfo.Text = "";
			bool ok = tvDistributors.SelectedNode != null && tvDistributors.SelectedNode.Tag != null;
			bool enable = false;
			if (ok) {
				if (tvDistributors.SelectedNode.Tag is Distributor) {
					lblInfo.Text = "Anschluß an " + (tvDistributors.SelectedNode.Tag as Distributor).Id + ": " + (tvDistributors.SelectedNode.Tag as Distributor).Name;
					ok = true;
				} else if (tvDistributors.SelectedNode.Tag is Circuit) {
					if (this.product.Product.ConnectedCircuits.Count > 0) {
						lblInfo.Text = "Anschluß nicht möglich. An das Heizsystem ist mindestens ein anderes Teilsystem angeschloßen. Es kann daher nur an einen Verteiler angeschloßen werden.";
						ok = false;
					} else {
						Circuit selectedCircuit = tvDistributors.SelectedNode.Tag as Circuit;
						Product selectedProduct = selectedCircuit.PlannedProduct.Product;
						if (selectedProduct == this.product.Product) {
							lblInfo.Text = "Anschluß nicht möglich. Das Heizsystem kann nicht an sich selbst angeschlossen werden.";
							ok = false;
						} else {
							/*int free = 0;
							foreach (Circuit c in selectedProduct.PlannedCircuits) {
								if (selectedProduct.GetCircuitConnected(c.NrOfCircuit) == null) {
									free++;
								}
							}

							ok = free >= this.product.Product.PlannedCircuits.Count;*/
							ok = true;
							lblInfo.Text = ok ? "Anschluß an " + selectedCircuit.PlannedProduct.Node.Text + " in " + selectedCircuit.PlannedProduct.Product.AssociatedRoom.ToString() : "Anschluß nicht möglich. Bei diesem Heizsystem sind nicht genug Heizkreise verfügbar";
							enable = ok;
						}
					}
				} else {
					lblInfo.Text = "Anschluß nicht möglich";
					ok = false;
				}
			}
			this.grpConnection.Enabled = enable;
			this.btnOk.Enabled = ok;
		}

		/*public ProductConnection SelectedConnection {
			get {
				if (tvDistributors.SelectedNode == null) {
					return null;
				}
				if (tvDistributors.SelectedNode.Tag is Distributor) {
					return new ProductConnection(tvDistributors.SelectedNode.Tag as Distributor, (Distributor)null);
				}
				if (tvDistributors.SelectedNode.Tag is Product) {
					return new ProductConnection(tvDistributors.SelectedNode.Tag as PlannedProduct, (Distributor)null);
				}
				return null;
			}
			set {
				if (value == null) {
					this.tvDistributors.SelectedNode = null;
				} else {
					object sel = value.Connection;
					if (sel != null && this.nodes.ContainsKey(sel)) {
						this.tvDistributors.SelectedNode = this.nodes[sel];
					} else {
						this.tvDistributors.SelectedNode = null;
					}
				}
			}
		}*/

		private void SelectConnectionForProductForm_Load(object sender, EventArgs e) {
			SettingsKey settings = SettingsFile.Settings["SelectConnectionForProductForm"];
			this.Location = settings.GetPoint("Location", this.Location);
		}

		private void SelectConnectionForProductForm_FormClosing(object sender, FormClosingEventArgs e) {
			if (this.DialogResult == DialogResult.OK) {
				List<PlannedProduct> wasConnectedTo = new List<PlannedProduct>();
				if (this.product.Product.PlannedConnection != null && this.product.Product.PlannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.OTHER_PRODUCT) {
					// find out to which other product(s) this product was connected
					foreach (Circuit.CircuitConnection cc in this.product.Product.InverseConnectedCircuits.Values) {
						if (!wasConnectedTo.Contains(cc.OtherCircuit.PlannedProduct)) {
							wasConnectedTo.Add(cc.OtherCircuit.PlannedProduct);
						}
					}

					// remove the connection in the other product(s)
					foreach (PlannedProduct pp in wasConnectedTo) {
						List<int> delete = new List<int>();
						foreach (KeyValuePair<int, Circuit.CircuitConnection> kvp in pp.Product.ConnectedCircuits) {
							if (kvp.Value.OtherCircuit.PlannedProduct == this.product) {
								delete.Add(kvp.Key);
							}
						}
						foreach (int i in delete) {
							pp.Product.ConnectedCircuits.Remove(i);
						}
					}

					// remove the connection in this product
					this.product.Product.InverseConnectedCircuits.Clear();
				}

				if (this.tvDistributors.SelectedNode.Tag is Distributor) {
					Distributor dist = this.tvDistributors.SelectedNode.Tag as Distributor;
					this.product.Product.PlannedConnection = new ProductConnection(dist);
					foreach (PlannedProduct pp in wasConnectedTo) {
						pp.ConfigureProductDefault();
					}
				} else if (this.tvDistributors.SelectedNode.Tag is Circuit) {
					PlannedProduct pp = (this.tvDistributors.SelectedNode.Tag as Circuit).PlannedProduct;
					this.product.Product.PlannedConnection = new ProductConnection(pp, this.rbRuecklauf.Checked ?  Circuit.CircuitConnectionTypeEnum.RUECKLAUF : Circuit.CircuitConnectionTypeEnum.VORLAUF);
					int i = 0;
					foreach (Circuit c in this.product.Product.PlannedCircuits) {
						while (pp.Product.ConnectedCircuits.ContainsKey(i)) {
							i++;
						}
						pp.Product.ConnectedCircuits[i] = new Circuit.CircuitConnection((this.rbRuecklauf.Checked ?  Circuit.CircuitConnectionTypeEnum.RUECKLAUF : Circuit.CircuitConnectionTypeEnum.VORLAUF), c);
						this.product.Product.InverseConnectedCircuits[c.NrOfCircuit] = new Circuit.CircuitConnection((this.rbRuecklauf.Checked ? Circuit.CircuitConnectionTypeEnum.RUECKLAUF : Circuit.CircuitConnectionTypeEnum.VORLAUF), this.tvDistributors.SelectedNode.Tag as Circuit);
					}
					this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
					pp.Product.ConfigureProduct(pp.RequestedHeatLoad, pp.RequestedCoolLoad, pp.CalculateHeat, pp.CalculateCool, false);
				}
				// TODO
			}
			SettingsKey settings = SettingsFile.Settings["SelectConnectionForProductForm"];
			settings.StorePoint("Location", this.Location);
			SettingsFile.Update();
		}
	}
}