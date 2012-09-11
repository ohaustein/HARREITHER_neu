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

			this.SetLanguage();

			this.tvDistributors.Nodes.Clear();
			this.product = product;
			DistributorList distributors = floor.GetAllAvailableDistributors(true);
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
						string label = EuroplanRes.SelectConnectionForProductForm_SystemInRaum;
						label = label.Replace("%SYSTEM%", p.Node.Text);
						label = label.Replace("%RAUM%", p.Product.AssociatedRoom.ToString());
						TreeNode node = new TreeNode(label);
						node.Tag = p;
						distributorNodes[i].Nodes.Add(node);
						if (selectNode == null && this.product.Product.PlannedConnection != null && this.product.Product.PlannedConnection.OtherProduct == p) {
							selectNode = node;
						}
					} else {
						int j = 0;
						foreach (Circuit c in p.Product.PlannedCircuits) {
							if (c.PlannedProduct != null) {
								Circuit.CircuitConnection cc = p.Product.GetCircuitConnected(j);
								j++;
								string label = "";
								if (p.Product.PlannedCircuits.Count <= 1) {
									label = EuroplanRes.SelectConnectionForProductForm_SystemInRaum;
									label = label.Replace("%SYSTEM%", p.Node.Text);
									label = label.Replace("%RAUM%", p.Product.AssociatedRoom.ToString());
								} else {
									label = EuroplanRes.SelectConnectionForProductForm_SystemInRaum2;
									label = label.Replace("%SYSTEM%", p.Node.Text);
									label = label.Replace("%HK%", j.ToString());
									label = label.Replace("%RAUM%", p.Product.AssociatedRoom.ToString());
								}
								if (cc != null) {
									string tmp = "";
									if (cc.OtherCircuit.PlannedProduct.Product.PlannedCircuits.Count <= 1) {
										tmp = EuroplanRes.SelectConnectionForProductForm_SystemInRaum;
										tmp = tmp.Replace("%SYSTEM%", cc.OtherCircuit.PlannedProduct.Node.Text);
										tmp = tmp.Replace("%RAUM%", cc.OtherCircuit.PlannedProduct.Product.AssociatedRoom.ToString());
									} else {
										tmp = EuroplanRes.SelectConnectionForProductForm_SystemInRaum2;
										tmp = tmp.Replace("%SYSTEM%", cc.OtherCircuit.PlannedProduct.Node.Text);
										tmp = tmp.Replace("%HK%", cc.OtherCircuit.NrOfCircuit.ToString());
										tmp = tmp.Replace("%RAUM%", cc.OtherCircuit.PlannedProduct.Product.AssociatedRoom.ToString());
									}
									label += ", " + tmp;
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
				}
				i++;
			}
			rootNode = new TreeNode(EuroplanRes.General_Projekt, distributorNodes);
			this.tvDistributors.Nodes.Add(rootNode);
			this.tvDistributors.ExpandAll();
			string hkIn = EuroplanRes.SelectConnectionForProductForm_HeizkreisIn;
			hkIn = hkIn.Replace("%SYSTEM%", product.InternalName);
			hkIn = hkIn.Replace("%RAUM%", product.Product.AssociatedRoom.ToString());
			this.hk1DataGridViewTextBoxColumn.HeaderText = hkIn;
		}

		private void SetLanguage() {
			this.btnCancel.Text = EuroplanRes.General_Abbrechen; //"Cancel";
			this.btnOk.Text = EuroplanRes.General_Ok; //"OK";

			this.grpInfo.Text = EuroplanRes.SelectConnectionForProductForm_Information; //"Information";
			this.grpConnection.Text = EuroplanRes.SelectConnectionForProductForm_Heizkreisanschluss; //"Heizkreisanschluß (nur bei Anschluß an anderen Heizkreis)";
			this.rbRuecklauf.Text = EuroplanRes.SelectConnectionForProductForm_Ruecklaufseitig; //"rücklaufseitig";
			this.rbVorlauf.Text = EuroplanRes.SelectConnectionForProductForm_Vorlaufseitig; //"vorlaufseitig";
			this.grpUserDefinedConnection.Text = EuroplanRes.SelectConnectionForProductForm_Benutzerdefiniert; //"Benuzerdefinierte Zuordnung (nur bei Anschluß an anderen Heizkreis)";
			this.cbActivateUserDefinedConnection.Text = EuroplanRes.SelectConnectionForProductForm_BenutzerdefiniertAktivieren; //"benutzerdefinierte Heizkreiszuordnung aktivieren";
			this.Text = EuroplanRes.SelectConnectionForProductForm_Verteileranschluss; //"Verteileranschluß";
		}

		private void tvDistributors_AfterSelect(object sender, TreeViewEventArgs e) {
			lblInfo.Text = "";
			bool ok = tvDistributors.SelectedNode != null && tvDistributors.SelectedNode.Tag != null;
			bool enable = false;
			if (ok) {
				if (tvDistributors.SelectedNode.Tag is Distributor) {
					string anschluss = EuroplanRes.SelectConnectionForProductForm_AnschlussAnVerteiler;
					anschluss = anschluss.Replace("%ID%", (tvDistributors.SelectedNode.Tag as Distributor).Id);
					anschluss = anschluss.Replace("%NAME%", (tvDistributors.SelectedNode.Tag as Distributor).Name);
					lblInfo.Text = anschluss;
					ok = true;
				} else if (tvDistributors.SelectedNode.Tag is Circuit) {
					if (this.product.Product.ConnectedCircuits.Count > 0) {
						lblInfo.Text = EuroplanRes.SelectConnectionForProductForm_AnschlussNurAnVerteiler; //"Anschluß nicht möglich. An das Heizsystem ist mindestens ein anderes Teilsystem angeschloßen. Es kann daher nur an einen Verteiler angeschloßen werden.";
						ok = false;
					} else {
						Circuit selectedCircuit = tvDistributors.SelectedNode.Tag as Circuit;
						Product selectedProduct = selectedCircuit.PlannedProduct.Product;
						if (selectedProduct == this.product.Product) {
							lblInfo.Text = EuroplanRes.SelectConnectionForProductForm_AnschlussAnSichSelbst; //"Anschluß nicht möglich. Das Heizsystem kann nicht an sich selbst angeschlossen werden.";
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
							if (ok) {
								string anschluss = EuroplanRes.SelectConnectionForProductForm_AnschlussAnSystem;
								anschluss = anschluss.Replace("%SYSTEM%", selectedCircuit.PlannedProduct.Node.Text);
								anschluss = anschluss.Replace("%RAUM%", selectedCircuit.PlannedProduct.Product.AssociatedRoom.ToString());
								lblInfo.Text = anschluss;
							} else {
								lblInfo.Text = EuroplanRes.SelectConnectionForProductForm_AnschlussAnSystemNichtMoeglich; //"Anschluß nicht möglich. Bei diesem Heizsystem sind nicht genug Heizkreise verfügbar";
							}
							string hkIn = EuroplanRes.SelectConnectionForProductForm_HeizkreisIn;
							hkIn = hkIn.Replace("%SYSTEM%", selectedCircuit.PlannedProduct.InternalName);
							hkIn = hkIn.Replace("%RAUM%", selectedCircuit.PlannedProduct.Product.AssociatedRoom.ToString());
							this.hk2DataGridViewTextBoxColumn.HeaderText = hkIn;
							enable = ok;
						}
					}
				} else {
					lblInfo.Text = EuroplanRes.SelectConnectionForProductForm_AnschlussNichtMoeglich; //"Anschluß nicht möglich";
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
				if (this.product.Product.PlannedConnection != null && 
					this.product.Product.PlannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.OTHER_PRODUCT &&
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

		private void SelectConnectionForProductForm_Load(object sender, EventArgs e) {
			SettingsKey settings = SettingsFile.Settings["SelectConnectionForProductForm"];
			this.Location = settings.GetPoint("Location", this.Location);

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
				UnconnectProduct(this.product);
				
				if (this.tvDistributors.SelectedNode.Tag is Distributor) {
					Distributor dist = this.tvDistributors.SelectedNode.Tag as Distributor;
					ConnectProduct(this.product, dist);
				} else if (this.tvDistributors.SelectedNode.Tag is Circuit) {
					PlannedProduct pp = (this.tvDistributors.SelectedNode.Tag as Circuit).PlannedProduct;
					ConnectProduct(this.product, pp, this.rbRuecklauf.Checked,
						this.cbActivateUserDefinedConnection.Checked ? this.userDefinedConnectionBindingSource.DataSource as List<UserDefinedConnection> : null);
				}
			}
			SettingsKey settings = SettingsFile.Settings["SelectConnectionForProductForm"];
			settings.StorePoint("Location", this.Location);
			SettingsFile.Update();
		}

		public static void UnconnectProduct(PlannedProduct product) {
			List<PlannedProduct> wasConnectedTo = new List<PlannedProduct>();
			if (product.Product.PlannedConnection != null && product.Product.PlannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.OTHER_PRODUCT) {
				// find out to which other product(s) this product was connected
				foreach (Circuit.CircuitConnection cc in product.Product.InverseConnectedCircuits.Values) {
					if (!wasConnectedTo.Contains(cc.OtherPlannedProduct)) {
						wasConnectedTo.Add(cc.OtherPlannedProduct);
					}
				}

				// remove the connection in the other product(s)
				foreach (PlannedProduct pp in wasConnectedTo) {
					List<int> delete = new List<int>();
					foreach (KeyValuePair<int, Circuit.CircuitConnection> kvp in pp.Product.ConnectedCircuits) {
						if (kvp.Value.OtherCircuit != null && kvp.Value.OtherCircuit.PlannedProduct == product) {
							delete.Add(kvp.Key);
						}
					}
					foreach (int i in delete) {
						pp.Product.ConnectedCircuits.Remove(i);
					}
				}

				// remove the connection in this product
				product.Product.InverseConnectedCircuits.Clear();
			}
			foreach (PlannedProduct pp in wasConnectedTo) {
				pp.ConfigureProduct(false);
			}
		}

		public static void ConnectProduct(PlannedProduct product, Distributor dist) {
			product.Product.PlannedConnection = new ProductConnection(dist);
		}

		public static void ConnectProduct(PlannedProduct product, PlannedProduct otherProduct, bool ruecklauf, List<UserDefinedConnection> list) {
			product.Product.PlannedConnection = new ProductConnection(otherProduct, ruecklauf ? Circuit.CircuitConnectionTypeEnum.RUECKLAUF : Circuit.CircuitConnectionTypeEnum.VORLAUF);
			if (list != null) {
				foreach (UserDefinedConnection udc in list) {
					otherProduct.Product.ConnectedCircuits[udc.Hk2 - 1] = new Circuit.CircuitConnection((ruecklauf ? Circuit.CircuitConnectionTypeEnum.RUECKLAUF : Circuit.CircuitConnectionTypeEnum.VORLAUF), product, udc.Hk1 - 1, true);
					product.Product.InverseConnectedCircuits[udc.Hk1 - 1] = new Circuit.CircuitConnection((ruecklauf ? Circuit.CircuitConnectionTypeEnum.RUECKLAUF : Circuit.CircuitConnectionTypeEnum.VORLAUF), otherProduct, udc.Hk2 - 1, true);
				}
				product.Product.PlannedConnection.UserDefined = true;
			} else {
				int i = 0;
				foreach (Circuit c in product.Product.PlannedCircuits) {
					while (otherProduct.Product.ConnectedCircuits.ContainsKey(i)) {
						i++;
					}
					if (otherProduct.Product.PlannedCircuits.Count > i) {
						otherProduct.Product.ConnectedCircuits[i] = new Circuit.CircuitConnection((ruecklauf ? Circuit.CircuitConnectionTypeEnum.RUECKLAUF : Circuit.CircuitConnectionTypeEnum.VORLAUF), c, false);
						product.Product.InverseConnectedCircuits[c.NrOfCircuit] = new Circuit.CircuitConnection((ruecklauf ? Circuit.CircuitConnectionTypeEnum.RUECKLAUF : Circuit.CircuitConnectionTypeEnum.VORLAUF), otherProduct.Product.PlannedCircuits[i], false);
					}
				}
			}
			product.Product.ConfigureProduct(product.RequestedHeatLoad, product.RequestedCoolLoad, product.CalculateHeat, product.CalculateCool, false);
			otherProduct.Product.ConfigureProduct(otherProduct.RequestedHeatLoad, otherProduct.RequestedCoolLoad, otherProduct.CalculateHeat, otherProduct.CalculateCool, false);
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