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
		private TreeNode selectNode = null;

		public class UserDefinedConnection {
			private int hk1;
			private int hk2;

			public UserDefinedConnection() {
				this.hk1 = 1;
				this.hk2 = 1;
			}

			public UserDefinedConnection(int hk1, int hk2) {
				this.hk1 = hk1;
				this.hk2 = hk2;
			}

			public int Hk1 {
				get { return this.hk1; }
				set { this.hk1 = value; }
			}

			public int Hk2 {
				get { return this.hk2; }
				set { this.hk2 = value; }
			}
		}

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
				if (selectNode == null && this.product.Product.PlannedConnection != null && this.product.Product.PlannedConnection.Distributor == d) {
					selectNode = distributorNodes[i];
				}
				foreach (PlannedProduct p in d.PlannedConnectedProducts) {
					if (p.Product.PlannedCircuits == null) {
						TreeNode node = new TreeNode(p.Node.Text + " in " + p.Product.AssociatedRoom.ToString());
						node.Tag = p;
						distributorNodes[i].Nodes.Add(node);
						if (selectNode == null && this.product.Product.PlannedConnection != null && this.product.Product.PlannedConnection.OtherProduct == p) {
							selectNode = node;
						}
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
							if (selectNode == null && this.product.Product.PlannedConnection != null &&
								this.product.Product.PlannedConnection.OtherProduct == p &&
								cc != null && cc.OtherProduct == this.product.Product) {
								selectNode = node;
							}
						}
					}
				}
				i++;
			}
			rootNode = new TreeNode("Projekt", distributorNodes);
			this.tvDistributors.Nodes.Add(rootNode);
			this.tvDistributors.ExpandAll();
			this.hk1DataGridViewTextBoxColumn.HeaderText = "Heizkreis in " + product.InternalName + " in " + product.Product.AssociatedRoom.ToString();
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
							this.hk2DataGridViewTextBoxColumn.HeaderText = "Heizkreis in " + selectedCircuit.PlannedProduct.InternalName + " in " + selectedCircuit.PlannedProduct.Product.AssociatedRoom.ToString();
							enable = ok;
						}
					}
				} else {
					lblInfo.Text = "Anschluß nicht möglich";
					ok = false;
				}
			}
			this.grpConnection.Enabled = enable;
			this.grpUserDefinedConnection.Enabled = enable;
			this.cbActivateUserDefinedConnection.Checked = false;
			if (enable) {
				this.hk2DataGridViewTextBoxColumn.Items.Clear();
				Product p = (this.tvDistributors.SelectedNode.Tag as Circuit).PlannedProduct.Product;
				List<UserDefinedConnection> list = new List<UserDefinedConnection>();
				if (this.product.Product.PlannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.OTHER_PRODUCT &&
					this.product.Product.PlannedConnection.OtherProduct.Product == p &&
					this.product.Product.PlannedConnection.UserDefined) {
					for (int i = 0; i < this.product.Product.PlannedCircuits.Count; i++) {
						list.Add(new UserDefinedConnection(i + 1, this.product.Product.InverseConnectedCircuits[i].OtherCircuitId + 1));
					}
					/*foreach (KeyValuePair<int, Circuit.CircuitConnection> kvp in this.product.Product.InverseConnectedCircuits) {
						list.Add(new UserDefinedConnection(kvp.Key + 1, kvp.Value.OtherCircuitId + 1));
					}*/
				} else {
					List<int> tmp = new List<int>();
					for (int i = 0; i < 12; i++) {
						if (p == this.product.Product || !p.ConnectedCircuits.ContainsKey(i) || !p.ConnectedCircuits[i].UserDefined) {
							this.hk2DataGridViewTextBoxColumn.Items.Add(i + 1);
							tmp.Add(i + 1);
						}
					}

					for (int i = 1; i <= this.product.Product.PlannedCircuitCount; i++) {
						list.Add(new UserDefinedConnection(i, tmp.Count > (i - 1) ? tmp[i - 1] : tmp[tmp.Count - 1]));
					}
				}
				this.userDefinedConnectionBindingSource.DataSource = list;
				this.userDefinedConnectionBindingSource.ResetBindings(false);
			} else {
				this.userDefinedConnectionBindingSource.DataSource = new List<UserDefinedConnection>();
				this.userDefinedConnectionBindingSource.ResetBindings(false);
			}
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

			// TODO select old
			if (selectNode != null) {
				this.tvDistributors.SelectedNode = selectNode;
			}
			if (this.product.Product.PlannedConnection != null && this.product.Product.PlannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.OTHER_PRODUCT) {
				this.grpConnection.Enabled = true;
				this.grpUserDefinedConnection.Enabled = true;
				if (this.product.Product.PlannedConnection.CircuitConnectionType == Circuit.CircuitConnectionTypeEnum.VORLAUF) {
					this.rbVorlauf.Checked = true;
				} else {
					this.rbRuecklauf.Checked = true;
				}
				this.cbActivateUserDefinedConnection.Checked = this.product.Product.PlannedConnection.UserDefined;
			}
		}

		private void SelectConnectionForProductForm_FormClosing(object sender, FormClosingEventArgs e) {
			if (this.DialogResult == DialogResult.OK) {
				List<PlannedProduct> wasConnectedTo = new List<PlannedProduct>();
				if (this.product.Product.PlannedConnection != null && this.product.Product.PlannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.OTHER_PRODUCT) {
					// find out to which other product(s) this product was connected
					foreach (Circuit.CircuitConnection cc in this.product.Product.InverseConnectedCircuits.Values) {
						if (!wasConnectedTo.Contains(cc.OtherPlannedProduct)) {
							wasConnectedTo.Add(cc.OtherPlannedProduct);
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
					this.product.Product.PlannedConnection = new ProductConnection(pp, this.rbRuecklauf.Checked ? Circuit.CircuitConnectionTypeEnum.RUECKLAUF : Circuit.CircuitConnectionTypeEnum.VORLAUF);
					if (this.cbActivateUserDefinedConnection.Checked) {
						List<UserDefinedConnection> list = this.userDefinedConnectionBindingSource.DataSource as List<UserDefinedConnection>;
						foreach (UserDefinedConnection udc in list) {
							pp.Product.ConnectedCircuits[udc.Hk2 - 1] = new Circuit.CircuitConnection((this.rbRuecklauf.Checked ? Circuit.CircuitConnectionTypeEnum.RUECKLAUF : Circuit.CircuitConnectionTypeEnum.VORLAUF), this.product, udc.Hk1 - 1, true);
							this.product.Product.InverseConnectedCircuits[udc.Hk1 - 1] = new Circuit.CircuitConnection((this.rbRuecklauf.Checked ? Circuit.CircuitConnectionTypeEnum.RUECKLAUF : Circuit.CircuitConnectionTypeEnum.VORLAUF), pp, udc.Hk2 - 1, true);
						}
						this.product.Product.PlannedConnection.UserDefined = true;
						// TODO
						//foreach (DataGridViewRow row in this.gridUserDefinedConnection.Rows) {
							//pp.Product.ConnectedCircuits[(int)row.Cells[1]] = new Circuit.CircuitConnection((
						//}
					} else {
						int i = 0;
						foreach (Circuit c in this.product.Product.PlannedCircuits) {
							while (pp.Product.ConnectedCircuits.ContainsKey(i)) {
								i++;
							}
							pp.Product.ConnectedCircuits[i] = new Circuit.CircuitConnection((this.rbRuecklauf.Checked ? Circuit.CircuitConnectionTypeEnum.RUECKLAUF : Circuit.CircuitConnectionTypeEnum.VORLAUF), c, false);
							this.product.Product.InverseConnectedCircuits[c.NrOfCircuit] = new Circuit.CircuitConnection((this.rbRuecklauf.Checked ? Circuit.CircuitConnectionTypeEnum.RUECKLAUF : Circuit.CircuitConnectionTypeEnum.VORLAUF), this.tvDistributors.SelectedNode.Tag as Circuit, false);
						}
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

		private void cbActivateUserDefinedConnection_CheckedChanged(object sender, EventArgs e) {
			if (this.cbActivateUserDefinedConnection.Checked) {
				this.hk2DataGridViewTextBoxColumn.Items.Clear();
				for (int i = 0; i < 12; i++) {
					Product p = (this.tvDistributors.SelectedNode.Tag as Circuit).PlannedProduct.Product;
					Product currentConnection = this.product.Product.PlannedConnection != null ? (this.product.Product.PlannedConnection.OtherProduct != null ? this.product.Product.PlannedConnection.OtherProduct.Product : null) : null;
					if (p == currentConnection || !p.ConnectedCircuits.ContainsKey(i) || !p.ConnectedCircuits[i].UserDefined) {
						//if (!(this.tvDistributors.SelectedNode.Tag as Circuit).PlannedProduct.Product.ConnectedCircuits.ContainsKey(i)) {
						this.hk2DataGridViewTextBoxColumn.Items.Add(i + 1);
					}
				}
			}
			this.gridUserDefinedConnection.Visible = this.cbActivateUserDefinedConnection.Checked;
		}

		private void gridUserDefinedConnection_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
			if (this.cbActivateUserDefinedConnection.Checked) {
				List<UserDefinedConnection> list = this.userDefinedConnectionBindingSource.DataSource as List<UserDefinedConnection>;
				List<int> circuitsUsed = new List<int>();
				bool ok = true;
				foreach (UserDefinedConnection udc in list) {
					if (circuitsUsed.Contains(udc.Hk2)) {
						ok = false;
					} else {
						circuitsUsed.Add(udc.Hk2);
					}
					/*if ((this.tvDistributors.SelectedNode.Tag as Circuit).PlannedProduct.Product.ConnectedCircuits.ContainsKey(udc.Hk2 - 1)) {
						ok = false;
					}*/
					/*if (udc.Hk2 > (this.tvDistributors.SelectedNode.Tag as Circuit).PlannedProduct.Product.PlannedCircuitCount) {
						ok = false;
					}*/
				
				}
				this.btnOk.Enabled = ok;
			}
		}

		private void gridUserDefinedConnection_CurrentCellDirtyStateChanged(object sender, EventArgs e) {
			this.gridUserDefinedConnection.CommitEdit(DataGridViewDataErrorContexts.Commit);
		}
	}
}