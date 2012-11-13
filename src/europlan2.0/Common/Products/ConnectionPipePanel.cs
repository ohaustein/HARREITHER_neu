using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Europlan.Common {

	public delegate void ConnectionPipePanelContentChangedHandler(object sender);

	public partial class ConnectionPipePanel : UserControl {

		private Button roomSelectionButton;
		private ComboBox pipeTypeCombo;
		private ComboBox verlegeartCombo;
		private ComboBox insulationCombo;
		private PlannedProduct product;
		private bool showPipesThroughProduct = false;

		public event ConnectionPipePanelContentChangedHandler GridContentChanged;

		public ConnectionPipePanel() {
			InitializeComponent();
			this.SetLanguage();

			roomSelectionButton = new Button();
			roomSelectionButton.Size = new Size(30, 20);
			roomSelectionButton.Text = "...";
			dgvConnectionPipes.Controls.Add(roomSelectionButton);
			roomSelectionButton.Hide();
			roomSelectionButton.Click += new EventHandler(roomSelectionButton_Click);

			pipeTypeCombo = new ComboBox();
			pipeTypeCombo.Size = new Size(30, 20);
			pipeTypeCombo.DropDownStyle = ComboBoxStyle.DropDownList;
			dgvConnectionPipes.Controls.Add(pipeTypeCombo);
			pipeTypeCombo.Hide();
			pipeTypeCombo.SelectedValueChanged += new EventHandler(pipeTypeCombo_SelectedValueChanged);

			verlegeartCombo = new ComboBox();
			verlegeartCombo.Size = new Size(30, 20);
			verlegeartCombo.DropDownStyle = ComboBoxStyle.DropDownList;
			dgvConnectionPipes.Controls.Add(verlegeartCombo);
			verlegeartCombo.Hide();
			verlegeartCombo.SelectedValueChanged += new EventHandler(verlegeartCombo_SelectedValueChanged);

			insulationCombo = new ComboBox();
			insulationCombo.Size = new Size(30, 20);
			insulationCombo.DropDownStyle = ComboBoxStyle.DropDownList;
			dgvConnectionPipes.Controls.Add(insulationCombo);
			insulationCombo.Hide();
			insulationCombo.SelectedValueChanged += new EventHandler(insulationCombo_SelectedValueChanged);

			ConfigureColumnVisibility();
		}

		private void SetLanguage() {
			this.Room.HeaderText = EuroplanRes.ConnectionPipePanel_RaumCol; //"Raum"
			this.vorlaufDataGridViewTextBoxColumn.HeaderText = EuroplanRes.ConnectionPipePanel_VorlaufCol; //"Länge\nVorlauf\n(m)"
			this.ruecklaufDataGridViewTextBoxColumn.HeaderText = EuroplanRes.ConnectionPipePanel_RuecklaufCol; //"Länge\nRücklauf\n(m)"
			this.roomDataGridViewComboBoxColumn.HeaderText = EuroplanRes.ConnectionPipePanel_DurchRaumCol; //"durch\nRaum\nNr."
			this.ConnectionThrough.HeaderText = EuroplanRes.ConnectionPipePanel_TeilsystemCol; //"Teilsystem"
			this.PlannedCircuits.HeaderText = EuroplanRes.ConnectionPipePanel_HkAnzahlCol; //"Anz."
			this.onlyFirstDataGridViewCheckBoxColumn.HeaderText = EuroplanRes.ConnectionPipePanel_NurErsterHkCol; //"nur\nerster\nHK"
			this.printDataGridViewCheckBoxColumn.HeaderText = EuroplanRes.ConnectionPipePanel_VerlegedatenDruckenCol; //"Verlege-\ndaten\ndrucken"
			this.PipeType.HeaderText = EuroplanRes.ConnectionPipePanel_RohrsystemCol; //"Rohr-\nsystem"
			this.PipeTypeText.HeaderText = EuroplanRes.ConnectionPipePanel_RohrsystemCol; //"Rohr-\nsystem"
			this.Verlegeart.HeaderText = EuroplanRes.ConnectionPipePanel_VerlegeartCol; //"Verlege-\nart"
			this.Insulation.HeaderText = EuroplanRes.ConnectionPipePanel_DaemmungCol; //"Dämmung"
			this.Area.HeaderText = EuroplanRes.ConnectionPipePanel_FlaecheCol; //"Fläche"
			this.HeatLoad.HeaderText = EuroplanRes.ConnectionPipePanel_HeizleistungCol; //"Heiz-\nleistung"
			this.CoolLoad.HeaderText = EuroplanRes.ConnectionPipePanel_KuehlleistungCol; //"Kühl-\nleistung"
			this.ConnectionOf.HeaderText = EuroplanRes.ConnectionPipePanel_TeilsystemCol;
		}

		private void ConfigureColumnVisibility() {
			this.dgvConnectionPipes.AllowUserToDeleteRows = !showPipesThroughProduct;
			if (showPipesThroughProduct) {
				// this is a workaround as room column cannot be made invisible if it is the first column
				if (!dgvConnectionPipes.Columns.Contains(Room)) {
					dgvConnectionPipes.Columns.Insert(0, Room);
				}
				if (!dgvConnectionPipes.Columns.Contains(ConnectionOf)) {
					dgvConnectionPipes.Columns.Insert(1, ConnectionOf);
				}
				Room.DisplayIndex = 0;
				ConnectionOf.DisplayIndex = 1;
				Room.Visible = true;
				roomDataGridViewComboBoxColumn.Visible = false;
				ConnectionOf.Visible = true;
				ConnectionThrough.Visible = false;
				Area.Visible = true;
				HeatLoad.Visible = true;
				CoolLoad.Visible = true;
				PlannedCircuits.Visible = true;
				PipeType.Visible = false;
				PipeTypeText.Visible = true;
				ConnectionOf.DefaultCellStyle.BackColor = SystemColors.Control;
				onlyFirstDataGridViewCheckBoxColumn.DefaultCellStyle.BackColor = SystemColors.Control;
				onlyFirstDataGridViewCheckBoxColumn.ReadOnly = true;
				printDataGridViewCheckBoxColumn.Visible = false;
				roomDataGridViewComboBoxColumn.DefaultCellStyle.BackColor = SystemColors.Control;
				dgvConnectionPipes.AllowUserToAddRows = false;
			} else {
				// this is a workaround as room column cannot be made invisible if it is the first column
				if (dgvConnectionPipes.Columns.Contains(Room)) {
					dgvConnectionPipes.Columns.Remove(Room);
				}
				if (dgvConnectionPipes.Columns.Contains(ConnectionOf)) {
					dgvConnectionPipes.Columns.Remove(ConnectionOf);
				}
				Room.Visible = false;
				roomDataGridViewComboBoxColumn.Visible = true;
				ConnectionOf.Visible = false;
				ConnectionThrough.Visible = true;
				Area.Visible = false;
				HeatLoad.Visible = false;
				CoolLoad.Visible = false;
				PlannedCircuits.Visible = false;
				PipeType.Visible = true;
				PipeTypeText.Visible = false;
				ConnectionThrough.DefaultCellStyle.BackColor = SystemColors.ControlLightLight;
				onlyFirstDataGridViewCheckBoxColumn.DefaultCellStyle.BackColor = SystemColors.ControlLightLight;
				onlyFirstDataGridViewCheckBoxColumn.ReadOnly = false;
				printDataGridViewCheckBoxColumn.Visible = true;
				roomDataGridViewComboBoxColumn.DefaultCellStyle.BackColor = SystemColors.ControlLightLight;
				dgvConnectionPipes.AllowUserToAddRows = true;
			}
		}

		public bool ShowPipesThroughProduct {
			get { return showPipesThroughProduct; }
			set {
				this.showPipesThroughProduct = value;
				ConfigureColumnVisibility();
			}
		}

		public void ReloadGrid() {
			int col = dgvConnectionPipes.SelectedCells.Count > 0 ? dgvConnectionPipes.SelectedCells[0].ColumnIndex : -1;
			int row = dgvConnectionPipes.SelectedCells.Count > 0 ? dgvConnectionPipes.SelectedCells[0].RowIndex : -1;
			connectionPipeBindingSource.ResetBindings(false);
			if (col > -1) {
				dgvConnectionPipes.Rows[row].Cells[col].Selected = true;
			}
		}

		public void Update(PlannedProduct product) {
			this.product = product;

			if (this.product != null) {
				if (showPipesThroughProduct) {
					this.connectionPipeBindingSource.DataSource = this.product.Product.PlannedConnectionPipesThroughThisProduct;
				} else {
					this.connectionPipeBindingSource.DataSource = this.product.Product.PlannedConnectionPipes;
				}
				this.connectionPipeBindingSource.ResetBindings(false);
			} else {
				this.connectionPipeBindingSource.DataSource = null;
			}
		}

		private void dgvConnectionPipes_CellEnter(object sender, DataGridViewCellEventArgs e) {
			if (e.RowIndex >= 0) {
				DataGridViewRow selectedRow = dgvConnectionPipes.Rows[e.RowIndex];
				bool generatedPipe = selectedRow.DataBoundItem is ConnectionPipe ? (selectedRow.DataBoundItem as ConnectionPipe).IsGenerated : false;
				if (((e.ColumnIndex == roomDataGridViewComboBoxColumn.Index) || (e.ColumnIndex == ConnectionThrough.Index)) && e.RowIndex >= 0 && !showPipesThroughProduct && !generatedPipe) {
					Rectangle rect = dgvConnectionPipes.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
					int width = dgvConnectionPipes.CurrentCell.Size.Width;
					roomSelectionButton.Location = new Point(rect.X + width - roomSelectionButton.Width, rect.Y);
					roomSelectionButton.Height = dgvConnectionPipes.Rows[e.RowIndex].Height;
					roomSelectionButton.Show();
				} else if (e.ColumnIndex == PipeType.Index && e.RowIndex >= 0) {
					this.pipeTypeCombo.SelectedValueChanged -= new EventHandler(pipeTypeCombo_SelectedValueChanged);
					DataGridViewCell pipeTypeCell = dgvConnectionPipes.Rows[e.RowIndex].Cells[PipeType.Index];
					pipeTypeCombo.Items.Clear();
					ConnectionPipe.PipeTypeEnumConverter conv = new ConnectionPipe.PipeTypeEnumConverter();

					PlannedProduct connectionOf = null;
					if (this.showPipesThroughProduct) {
						if (selectedRow.DataBoundItem is ConnectionPipe) {
							connectionOf = (selectedRow.DataBoundItem as ConnectionPipe).ConnectionOf;
						}
					} else {
						connectionOf = this.product;
					}
					if (connectionOf != null && connectionOf.Product != null) {
						if (connectionOf.Product.DefaultPipeType == ConnectionPipe.PipeTypeEnum.PT_ECOTHERM) {
							pipeTypeCombo.Items.Add(conv.ConvertToString(ConnectionPipe.PipeTypeEnum.PT_ECOTHERM));
						} else if (connectionOf.Product.DefaultPipeType == ConnectionPipe.PipeTypeEnum.PT_EUROVAL) {
							pipeTypeCombo.Items.Add(conv.ConvertToString(ConnectionPipe.PipeTypeEnum.PT_EUROVAL));
						} else {
							pipeTypeCombo.Items.Add(conv.ConvertToString(ConnectionPipe.PipeTypeEnum.PT_ECOTHERM));
							pipeTypeCombo.Items.Add(conv.ConvertToString(ConnectionPipe.PipeTypeEnum.PT_EUROVAL));
							pipeTypeCombo.Items.Add(conv.ConvertToString(ConnectionPipe.PipeTypeEnum.PT_21MM));
						}
					}

					pipeTypeCombo.SelectedItem = conv.ConvertToString(pipeTypeCell.Value);
					this.pipeTypeCombo.SelectedValueChanged += new EventHandler(pipeTypeCombo_SelectedValueChanged);

					Rectangle rect = dgvConnectionPipes.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
					pipeTypeCombo.Location = new Point(rect.X, rect.Y);
					pipeTypeCombo.Size = new Size(rect.Width, rect.Height);
					pipeTypeCombo.Show();
				} else if (e.ColumnIndex == Verlegeart.Index && e.RowIndex >= 0) {
					this.verlegeartCombo.SelectedValueChanged -= new EventHandler(verlegeartCombo_SelectedValueChanged);
					DataGridViewCell pipeTypeCell = dgvConnectionPipes.Rows[e.RowIndex].Cells[PipeType.Index];
					DataGridViewCell verlegeartCell = dgvConnectionPipes.Rows[e.RowIndex].Cells[Verlegeart.Index];
					verlegeartCombo.Items.Clear();
					ConnectionPipe.VerlegeartEnumConverter conv = new ConnectionPipe.VerlegeartEnumConverter();
					verlegeartCombo.Items.Add(conv.ConvertToString(ConnectionPipe.VerlegeartEnum.VA_UNTER_ESTRICH));
					if ((ConnectionPipe.PipeTypeEnum.PT_EUROVAL.Equals(pipeTypeCell.Value) ||
						ConnectionPipe.PipeTypeEnum.PT_ECOTHERM.Equals(pipeTypeCell.Value))) {
						verlegeartCombo.Items.Add(conv.ConvertToString(ConnectionPipe.VerlegeartEnum.VA_EV35));
						verlegeartCombo.Items.Add(conv.ConvertToString(ConnectionPipe.VerlegeartEnum.VA_EV30));
						verlegeartCombo.Items.Add(conv.ConvertToString(ConnectionPipe.VerlegeartEnum.VA_EV25));
						verlegeartCombo.Items.Add(conv.ConvertToString(ConnectionPipe.VerlegeartEnum.VA_EV20));
						verlegeartCombo.Items.Add(conv.ConvertToString(ConnectionPipe.VerlegeartEnum.VA_EV15));
						verlegeartCombo.Items.Add(conv.ConvertToString(ConnectionPipe.VerlegeartEnum.VA_EV10));
						verlegeartCombo.Items.Add(conv.ConvertToString(ConnectionPipe.VerlegeartEnum.VA_EV5));
						verlegeartCombo.Items.Add(conv.ConvertToString(ConnectionPipe.VerlegeartEnum.VA_A5));
					}
					verlegeartCombo.SelectedItem = conv.ConvertToString(verlegeartCell.Value);
					this.verlegeartCombo.SelectedValueChanged += new EventHandler(verlegeartCombo_SelectedValueChanged);

					Rectangle rect = dgvConnectionPipes.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
					verlegeartCombo.Location = new Point(rect.X, rect.Y);
					verlegeartCombo.Size = new Size(rect.Width, rect.Height);
					verlegeartCombo.Show();
				} else if (e.ColumnIndex == Insulation.Index && e.RowIndex >= 0) {
					this.insulationCombo.SelectedValueChanged -= new EventHandler(insulationCombo_SelectedValueChanged);
					DataGridViewCell verlegeartCell = dgvConnectionPipes.Rows[e.RowIndex].Cells[Verlegeart.Index];
					DataGridViewCell insulationCell = dgvConnectionPipes.Rows[e.RowIndex].Cells[Insulation.Index];
					insulationCombo.Items.Clear();
					ConnectionPipe.InsulationEnumConverter conv = new ConnectionPipe.InsulationEnumConverter();
					if (!ConnectionPipe.VerlegeartEnum.VA_UNTER_ESTRICH.Equals(verlegeartCell.Value)) {
						insulationCombo.Items.Add(conv.ConvertToString(ConnectionPipe.InsulationEnum.IN_NONE));
						insulationCombo.Items.Add(conv.ConvertToString(ConnectionPipe.InsulationEnum.IN_VL));
					}
					insulationCombo.Items.Add(conv.ConvertToString(ConnectionPipe.InsulationEnum.IN_VL_RL));
					insulationCombo.SelectedItem = conv.ConvertToString(insulationCell.Value);
					this.insulationCombo.SelectedValueChanged += new EventHandler(insulationCombo_SelectedValueChanged);

					Rectangle rect = dgvConnectionPipes.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
					insulationCombo.Location = new Point(rect.X, rect.Y);
					insulationCombo.Size = new Size(rect.Width, rect.Height);
					insulationCombo.Show();
				}
			}
		}

		private void dgvConnectionPipes_CellLeave(object sender, DataGridViewCellEventArgs e) {
			roomSelectionButton.Hide();
			pipeTypeCombo.Hide();
			verlegeartCombo.Hide();
			insulationCombo.Hide();
		}

		bool lastRoomOk = false;
		bool lastProductOk = false;

		private void dgvConnectionPipes_CellParsing(object sender, DataGridViewCellParsingEventArgs e) {
			if (e.ColumnIndex == roomDataGridViewComboBoxColumn.Index) {
				lastRoomOk = false;
				if (String.IsNullOrEmpty(e.Value as string)) {
					e.Value = null;
					e.ParsingApplied = true;
					(this.dgvConnectionPipes.Rows[e.RowIndex].DataBoundItem as ConnectionPipe).Room = null;
					lastRoomOk = true;
					return;
				}
				foreach (Floor f in Project.Instance.Floors) {
					if (f.Rooms.Contains(this.product.Product.AssociatedRoom)) {
						foreach (Room r in f.Rooms) {
							if (r.Id.Equals(e.Value)) {
								e.Value = r;
								e.ParsingApplied = true;
								lastRoomOk = true;
								return;
							}
						}
					}
				}
				if (!e.ParsingApplied) {
					if (MessageBox.Show(EuroplanRes.ConnectionPipePanel_UngueltigeRaumNrText, EuroplanRes.ConnectionPipePanel_UngueltigeRaumNrTitel, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel) {
						e.Value = null;
						e.ParsingApplied = true;
						lastRoomOk = true;
						return;
					}
				}
			} else if (e.ColumnIndex == ConnectionThrough.Index) {
				lastProductOk = false;
				if (String.IsNullOrEmpty(e.Value as string)) {
					e.Value = null;
					e.ParsingApplied = true;
					(this.dgvConnectionPipes.Rows[e.RowIndex].DataBoundItem as ConnectionPipe).ConnectionThrough = null;
					lastProductOk = true;
					return;
				}
				ConnectionPipe pipe = this.dgvConnectionPipes.Rows[e.RowIndex].DataBoundItem as ConnectionPipe;
				foreach (PlannedProduct p in pipe.Room.PlannedProducts) {
					if (p.InternalName.Equals(e.Value as String, StringComparison.InvariantCultureIgnoreCase) && p != this.product) {
						e.Value = p;
						e.ParsingApplied = true;
						lastProductOk = true;
						return;
					}
				}
				if (!e.ParsingApplied) {
					if (MessageBox.Show(EuroplanRes.ConnectionPipePanel_UngueltigeProduktNrText, EuroplanRes.ConnectionPipePanel_UngueltigeProduktNrTitel, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel) {
						e.Value = null;
						e.ParsingApplied = true;
						lastProductOk = true;
						return;
					}
				}
			}
		}

		private void dgvConnectionPipes_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
			if (this.GridContentChanged != null) {
				this.GridContentChanged(this);
			}
		}

		private void dgvConnectionPipes_DataError(object sender, DataGridViewDataErrorEventArgs e) {
			string test = e.Exception.ToString();
			if (e.ColumnIndex == this.roomDataGridViewComboBoxColumn.Index && lastRoomOk) {
				e.Cancel = false;
			}
			if (e.ColumnIndex == this.ConnectionThrough.Index && lastProductOk) {
				e.Cancel = false;
			}
		}

		private void dgvConnectionPipes_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e) {
			if (e.KeyCode == Keys.Delete && this.dgvConnectionPipes.SelectedCells.Count == 1 &&
				this.dgvConnectionPipes.SelectedRows.Count == 0 && this.dgvConnectionPipes.SelectedCells[0].Value != null) {
				DataGridViewCell cell = this.dgvConnectionPipes.SelectedCells[0];
				cell.Value = null;
				if (cell.ColumnIndex == roomDataGridViewComboBoxColumn.Index) {
					this.dgvConnectionPipes.Rows[cell.RowIndex].Cells[ConnectionThrough.Index].Value = null;
				}
			}
		}

		private void dgvConnectionPipes_UserDeletedRow(object sender, DataGridViewRowEventArgs e) {
			// the following line is part of the workaraound to make the UserDeletedRow event work in case the last remaining row is deleted
			dgvConnectionPipes.AllowUserToAddRows = true;
			if (this.GridContentChanged != null) {
				this.GridContentChanged(this);
			}
		}


		void roomSelectionButton_Click(object sender, EventArgs e) {
			if (dgvConnectionPipes.CurrentCell.ColumnIndex == roomDataGridViewComboBoxColumn.Index) {
				List<Room> rooms = new List<Room>();
				foreach (Floor f in Project.Instance.Floors) {
					if (f.Rooms.Contains(this.product.Product.AssociatedRoom)) {
						foreach (Room r in f.Rooms) {
							rooms.Add(r);
						}
					}
				}
				SelectRoomForm form = new SelectRoomForm(rooms);
				if (form.ShowDialog().Equals(DialogResult.OK)) {
					Room room = form.SelectedRoom;
					if (dgvConnectionPipes.CurrentCell.Value != room) {
						dgvConnectionPipes.CurrentCell.Value = room;
					}
				}
				form.Dispose();
			} else if (dgvConnectionPipes.CurrentCell.ColumnIndex == ConnectionThrough.Index) {
				List<PlannedProduct> products = new List<PlannedProduct>();

				Room r = this.dgvConnectionPipes.Rows[dgvConnectionPipes.CurrentCell.RowIndex].Cells[roomDataGridViewComboBoxColumn.Index].Value as Room;
				// product items
				if (r != null) {
					foreach (PlannedProduct p in r.PlannedProducts) {
						if (this.product != p) {
							products.Add(p);
						}
					}

					SelectPlannedProduct form = new SelectPlannedProduct(products);
					if (form.ShowDialog().Equals(DialogResult.OK)) {
						PlannedProduct product = form.SelectedPlannedProduct;
						dgvConnectionPipes.CurrentCell.Value = product;
					}
					form.Dispose();
				}
			}
			ReloadGrid();
		}

		private void pipeTypeCombo_SelectedValueChanged(object sender, EventArgs e) {
			ConnectionPipe.PipeTypeEnum pipeType = (ConnectionPipe.PipeTypeEnum)new ConnectionPipe.PipeTypeEnumConverter().ConvertFromString(pipeTypeCombo.SelectedItem as String);
			this.dgvConnectionPipes.Rows[dgvConnectionPipes.CurrentCell.RowIndex].Cells[PipeType.Index].Value = pipeType;
			if (pipeType == ConnectionPipe.PipeTypeEnum.PT_21MM) {
				this.dgvConnectionPipes.Rows[dgvConnectionPipes.CurrentCell.RowIndex].Cells[Verlegeart.Index].Value = ConnectionPipe.VerlegeartEnum.VA_UNTER_ESTRICH;
				this.dgvConnectionPipes.Rows[dgvConnectionPipes.CurrentCell.RowIndex].Cells[Insulation.Index].Value = ConnectionPipe.InsulationEnum.IN_VL_RL;
			}
			ReloadGrid();
		}

		private void verlegeartCombo_SelectedValueChanged(object sender, EventArgs e) {
			ConnectionPipe.VerlegeartEnum verlegeart = (ConnectionPipe.VerlegeartEnum)new ConnectionPipe.VerlegeartEnumConverter().ConvertFromString(verlegeartCombo.SelectedItem as String);
			this.dgvConnectionPipes.Rows[dgvConnectionPipes.CurrentCell.RowIndex].Cells[Verlegeart.Index].Value = verlegeart;
			if (verlegeart == ConnectionPipe.VerlegeartEnum.VA_UNTER_ESTRICH) {
				this.dgvConnectionPipes.Rows[dgvConnectionPipes.CurrentCell.RowIndex].Cells[Insulation.Index].Value = ConnectionPipe.InsulationEnum.IN_VL_RL;
			}
			ReloadGrid();
		}

		private void insulationCombo_SelectedValueChanged(object sender, EventArgs e) {
			ConnectionPipe.InsulationEnum insulation = (ConnectionPipe.InsulationEnum)new ConnectionPipe.InsulationEnumConverter().ConvertFromString(insulationCombo.SelectedItem as String);
			this.dgvConnectionPipes.Rows[dgvConnectionPipes.CurrentCell.RowIndex].Cells[Insulation.Index].Value = insulation;
			ReloadGrid();
		}

		public ConnectionPipe SelectedConnectionPipe {
			get {
				if (dgvConnectionPipes.CurrentCell != null && dgvConnectionPipes.CurrentCell.RowIndex >= 0) {
					DataGridViewRow row = dgvConnectionPipes.Rows[dgvConnectionPipes.CurrentCell.RowIndex];
					return row.DataBoundItem as ConnectionPipe;
				}
				return null;
			}
		}

		private void dgvConnectionPipes_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e) {
			// the following line is part of the workaraound to make the UserDeletedRow event work in case the last remaining row is deleted
			if (e.Row.DataBoundItem is ConnectionPipe && (e.Row.DataBoundItem as ConnectionPipe).IsGenerated) {
				e.Cancel = true;
			} else {
				dgvConnectionPipes.AllowUserToAddRows = false;
			}
		}

		private void dgvConnectionPipes_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e) {
			if (this.product != null && this.product.Product.DefaultPipeType == ConnectionPipe.PipeTypeEnum.PT_21MM) {
				e.Row.Cells[PipeType.Index].Value = ConnectionPipe.PipeTypeEnum.PT_21MM;
				e.Row.Cells[Verlegeart.Index].Value = ConnectionPipe.VerlegeartEnum.VA_UNTER_ESTRICH;
				e.Row.Cells[Insulation.Index].Value = ConnectionPipe.InsulationEnum.IN_VL_RL;
			} else if (this.product != null && this.product.Product.DefaultPipeType == ConnectionPipe.PipeTypeEnum.PT_ECOTHERM) {
				e.Row.Cells[PipeType.Index].Value = ConnectionPipe.PipeTypeEnum.PT_ECOTHERM;
				e.Row.Cells[Verlegeart.Index].Value = ConnectionPipe.VerlegeartEnum.VA_EV5;
				e.Row.Cells[Insulation.Index].Value = ConnectionPipe.InsulationEnum.IN_NONE;
			} else {
				e.Row.Cells[PipeType.Index].Value = ConnectionPipe.PipeTypeEnum.PT_EUROVAL;
				e.Row.Cells[Verlegeart.Index].Value = ConnectionPipe.VerlegeartEnum.VA_EV5;
				e.Row.Cells[Insulation.Index].Value = ConnectionPipe.InsulationEnum.IN_NONE;
			}
		}

		private void dgvConnectionPipes_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e) {
			if (e.RowIndex >= 0 && this.dgvConnectionPipes.Rows[e.RowIndex].DataBoundItem is ConnectionPipe && (this.dgvConnectionPipes.Rows[e.RowIndex].DataBoundItem as ConnectionPipe).IsGenerated && e.ColumnIndex != PipeType.Index && e.ColumnIndex != Verlegeart.Index && e.ColumnIndex != Insulation.Index && e.ColumnIndex != printDataGridViewCheckBoxColumn.Index) {
				e.Cancel = true;
			} else {
				if (e.ColumnIndex == roomDataGridViewComboBoxColumn.Index || e.ColumnIndex == ConnectionThrough.Index) {
					roomSelectionButton.Hide();
				}
			}
		}

		private void dgvConnectionPipes_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e) {
			if (e.Control is DataGridViewTextBoxEditingControl) {
				DataGridViewTextBoxEditingControl txtBox = (e.Control as DataGridViewTextBoxEditingControl);
				if (txtBox.EditingControlRowIndex >= 0 && txtBox.EditingControlRowIndex < this.dgvConnectionPipes.Rows.Count) {
					ConnectionPipe pipe = this.dgvConnectionPipes.Rows[txtBox.EditingControlRowIndex].DataBoundItem as ConnectionPipe;
					if (pipe != null) {
						if (this.dgvConnectionPipes.SelectedCells.Count == 1) {
							if (this.dgvConnectionPipes.SelectedCells[0].ColumnIndex == this.roomDataGridViewComboBoxColumn.Index) {
								if (pipe.Room != null) {
									txtBox.Text = pipe.Room.Id;
								} else {
									txtBox.Text = "";
								}
							} else if (this.dgvConnectionPipes.SelectedCells[0].ColumnIndex == this.ConnectionThrough.Index) {
								if (pipe.ConnectionThrough != null) {
									txtBox.Text = pipe.ConnectionThrough.InternalName;
								} else {
									txtBox.Text = "";
								}
							}
						}
					}
				}
			}
		}
	}
}
