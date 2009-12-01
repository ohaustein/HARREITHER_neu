using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

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

		private void ConfigureColumnVisibility() {
			if (showPipesThroughProduct) {
				// TODO: this is just a quick workaround - room column could not be made invisible if it
				//       was the first column
				if (!dgvConnectionPipes.Columns.Contains(Room)) {
					dgvConnectionPipes.Columns.Insert(0, Room);
				}
				Room.Visible = true;
				roomDataGridViewComboBoxColumn.Visible = false;
				Area.Visible = true;
				HeatLoad.Visible = true;
				CoolLoad.Visible = true;
				PlannedCircuits.Visible = true;
				PipeType.Visible = false;
				PipeTypeText.Visible = true;
				productDataGridViewComboBoxColumn.DefaultCellStyle.BackColor = SystemColors.Control;
				onlyFirstDataGridViewCheckBoxColumn.DefaultCellStyle.BackColor = SystemColors.Control;
				onlyFirstDataGridViewCheckBoxColumn.ReadOnly = true;
				printDataGridViewCheckBoxColumn.Visible = false;
				roomDataGridViewComboBoxColumn.DefaultCellStyle.BackColor = SystemColors.Control;
				dgvConnectionPipes.AllowUserToAddRows = false;
			} else {
				// TODO: this is just a quick workaround - room column could not be made invisible if it
				//       was the first column
				if (dgvConnectionPipes.Columns.Contains(Room)) {
					dgvConnectionPipes.Columns.Remove(Room);
				}
				Room.Visible = false;
				roomDataGridViewComboBoxColumn.Visible = true;
				Area.Visible = false;
				HeatLoad.Visible = false;
				CoolLoad.Visible = false;
				PlannedCircuits.Visible = false;
				PipeType.Visible = true;
				PipeTypeText.Visible = false;
				productDataGridViewComboBoxColumn.DefaultCellStyle.BackColor = SystemColors.ControlLightLight;
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
			// pipe type items
			/*this.PipeType.Items.Clear();
			this.PipeType.Items.Add(ConnectionPipe.PipeTypeEnum.PT_EUROVAL);
			if (this.product != null && this.product.Product != null &&
				this.product.Product.PlannedConnection != null) {
				PlannedProduct connectedProduct = this.product.Product.PlannedConnection.OtherProduct;
				if (connectedProduct != null && connectedProduct.Product.DefaultPipeType == ConnectionPipe.PipeTypeEnum.PT_21MM) {
					this.PipeType.Items.Add(ConnectionPipe.PipeTypeEnum.PT_21MM);
				}
			}*/
			//this.PipeType.Items.Add(ConnectionPipe.PipeTypeEnum.PT_21MM);

			// verlegeart items
			/*this.Verlegeart.Items.Clear();
			this.Verlegeart.Items.Add(ConnectionPipe.VerlegeartEnum.VA_UNTER_ESTRICH);
			this.Verlegeart.Items.Add(ConnectionPipe.VerlegeartEnum.VA_EV35);
			this.Verlegeart.Items.Add(ConnectionPipe.VerlegeartEnum.VA_EV30);
			this.Verlegeart.Items.Add(ConnectionPipe.VerlegeartEnum.VA_EV25);
			this.Verlegeart.Items.Add(ConnectionPipe.VerlegeartEnum.VA_EV20);
			this.Verlegeart.Items.Add(ConnectionPipe.VerlegeartEnum.VA_EV15);
			this.Verlegeart.Items.Add(ConnectionPipe.VerlegeartEnum.VA_EV10);
			this.Verlegeart.Items.Add(ConnectionPipe.VerlegeartEnum.VA_EV5);
			this.Verlegeart.Items.Add(ConnectionPipe.VerlegeartEnum.VA_A5);*/

			// insulation items
			/*this.Insulation.Items.Clear();
			this.Insulation.Items.Add(ConnectionPipe.InsulationEnum.IN_NONE);
			this.Insulation.Items.Add(ConnectionPipe.InsulationEnum.IN_VL);
			this.Insulation.Items.Add(ConnectionPipe.InsulationEnum.IN_VL_RL);*/

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
			if (!showPipesThroughProduct) {
				if (((e.ColumnIndex == roomDataGridViewComboBoxColumn.DisplayIndex) || (e.ColumnIndex == productDataGridViewComboBoxColumn.DisplayIndex)) && e.RowIndex >= 0) {
					Rectangle rect = dgvConnectionPipes.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
					int width = dgvConnectionPipes.CurrentCell.Size.Width;
					roomSelectionButton.Location = new Point(rect.X + width - roomSelectionButton.Width, rect.Y);
					roomSelectionButton.Height = dgvConnectionPipes.Rows[e.RowIndex].Height;
					roomSelectionButton.Show();
				} else if (e.ColumnIndex == PipeType.DisplayIndex && e.RowIndex >= 0) {
					this.pipeTypeCombo.SelectedValueChanged -= new EventHandler(pipeTypeCombo_SelectedValueChanged);
					DataGridViewCell pipeTypeCell = dgvConnectionPipes.Rows[e.RowIndex].Cells[PipeType.DisplayIndex];
					pipeTypeCombo.Items.Clear();
					ConnectionPipe.PipeTypeEnumConverter conv = new ConnectionPipe.PipeTypeEnumConverter();
					pipeTypeCombo.Items.Add(conv.ConvertToString(ConnectionPipe.PipeTypeEnum.PT_EUROVAL));
					DataGridViewRow selectedRow = dgvConnectionPipes.Rows[e.RowIndex];
					if ((this.showPipesThroughProduct && selectedRow.DataBoundItem != null && (selectedRow.DataBoundItem as ConnectionPipe).ConnectionOf != null && (selectedRow.DataBoundItem as ConnectionPipe).ConnectionOf.Product != null && (selectedRow.DataBoundItem as ConnectionPipe).ConnectionOf.Product.DefaultPipeType == ConnectionPipe.PipeTypeEnum.PT_21MM) ||
						(!this.showPipesThroughProduct && this.product != null && this.product.Product != null && this.product.Product.DefaultPipeType == ConnectionPipe.PipeTypeEnum.PT_21MM)) {
						//(dgvConnectionPipes.Rows[e.RowIndex].DataBoundItem as ConnectionPipe).ConnectionOf.Product.DefaultPipeType == ConnectionPipe.PipeTypeEnum.PT_21MM) {
						pipeTypeCombo.Items.Add(conv.ConvertToString(ConnectionPipe.PipeTypeEnum.PT_21MM));
					}
					pipeTypeCombo.SelectedItem = conv.ConvertToString(pipeTypeCell.Value);
					this.pipeTypeCombo.SelectedValueChanged += new EventHandler(pipeTypeCombo_SelectedValueChanged);

					Rectangle rect = dgvConnectionPipes.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
					pipeTypeCombo.Location = new Point(rect.X, rect.Y);
					pipeTypeCombo.Size = new Size(rect.Width, rect.Height);
					//pipeTypeCombo.Height = dgvConnectionPipes.Rows[e.RowIndex].Height;
					//pipeTypeCombo.Width = dgvConnectionPipes.CurrentCell.Size.Width;
					pipeTypeCombo.Show();
				} else if (e.ColumnIndex == Verlegeart.DisplayIndex && e.RowIndex >= 0) {
					this.verlegeartCombo.SelectedValueChanged -= new EventHandler(verlegeartCombo_SelectedValueChanged);
					DataGridViewCell pipeTypeCell = dgvConnectionPipes.Rows[e.RowIndex].Cells[PipeType.DisplayIndex];
					DataGridViewCell verlegeartCell = dgvConnectionPipes.Rows[e.RowIndex].Cells[Verlegeart.DisplayIndex];
					verlegeartCombo.Items.Clear();
					ConnectionPipe.VerlegeartEnumConverter conv = new ConnectionPipe.VerlegeartEnumConverter();
					verlegeartCombo.Items.Add(conv.ConvertToString(ConnectionPipe.VerlegeartEnum.VA_UNTER_ESTRICH));
					bool verlegeart = false;
					DataGridViewRow selectedRow = dgvConnectionPipes.Rows[e.RowIndex];
					if ((this.showPipesThroughProduct && this.product != null && (this.product.Product is EurovalProduct || this.product.Product is ModulKlimaBodenProduct)) ||
						(!this.showPipesThroughProduct && selectedRow.DataBoundItem != null && (selectedRow.DataBoundItem as ConnectionPipe).ConnectionThrough != null && (selectedRow.DataBoundItem as ConnectionPipe).ConnectionThrough.Product is EurovalProduct)) {
						verlegeart = true;
					}
					if (ConnectionPipe.PipeTypeEnum.PT_EUROVAL.Equals(pipeTypeCell.Value) && verlegeart) {
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
				} else if (e.ColumnIndex == Insulation.DisplayIndex && e.RowIndex >= 0) {
					this.insulationCombo.SelectedValueChanged -= new EventHandler(insulationCombo_SelectedValueChanged);
					DataGridViewCell verlegeartCell = dgvConnectionPipes.Rows[e.RowIndex].Cells[Verlegeart.DisplayIndex];
					DataGridViewCell insulationCell = dgvConnectionPipes.Rows[e.RowIndex].Cells[Insulation.DisplayIndex];
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

		private void dgvConnectionPipes_CellParsing(object sender, DataGridViewCellParsingEventArgs e) {
			if (e.ColumnIndex == roomDataGridViewComboBoxColumn.DisplayIndex) {
				this.product = this.Tag as PlannedProduct;
				foreach (Floor f in Project.Instance.Floors) {
					if (f.Rooms.Contains(this.product.Product.AssociatedRoom)) {
						foreach (Room r in f.Rooms) {
							if (r.ToString().Equals(e.Value)) {
								e.Value = r;
								e.ParsingApplied = true;
								return;
							}
						}
					}
				}
			} else if (e.ColumnIndex == productDataGridViewComboBoxColumn.DisplayIndex) {
				this.product = this.Tag as PlannedProduct;
				foreach (Floor f in Project.Instance.Floors) {
					if (f.Rooms.Contains(this.product.Product.AssociatedRoom)) {
						foreach (Room r in f.Rooms) {
							foreach (PlannedProduct p in r.PlannedProducts) {
								if (p.ToString().Equals(e.Value)) {
									e.Value = p;
									e.ParsingApplied = true;
									return;
								}
							}
						}
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
		}

		private void dgvConnectionPipes_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e) {
			if (e.KeyCode == Keys.Delete && this.dgvConnectionPipes.SelectedCells.Count == 1 &&
				this.dgvConnectionPipes.SelectedRows.Count == 0 && this.dgvConnectionPipes.SelectedCells[0].Value != null) {
				DataGridViewCell cell = this.dgvConnectionPipes.SelectedCells[0];
				cell.Value = null;
				if (cell.ColumnIndex == roomDataGridViewComboBoxColumn.DisplayIndex) {
					this.dgvConnectionPipes.Rows[cell.RowIndex].Cells[productDataGridViewComboBoxColumn.DisplayIndex].Value = null;
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
			if (dgvConnectionPipes.CurrentCell.ColumnIndex == roomDataGridViewComboBoxColumn.DisplayIndex) {
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
			} else if (dgvConnectionPipes.CurrentCell.ColumnIndex == productDataGridViewComboBoxColumn.DisplayIndex) {
				List<PlannedProduct> products = new List<PlannedProduct>();

				Room r = this.dgvConnectionPipes.Rows[dgvConnectionPipes.CurrentCell.RowIndex].Cells[roomDataGridViewComboBoxColumn.DisplayIndex].Value as Room;
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
			this.dgvConnectionPipes.Rows[dgvConnectionPipes.CurrentCell.RowIndex].Cells[PipeType.DisplayIndex].Value = pipeType;
			if (pipeType == ConnectionPipe.PipeTypeEnum.PT_21MM) {
				this.dgvConnectionPipes.Rows[dgvConnectionPipes.CurrentCell.RowIndex].Cells[Verlegeart.DisplayIndex].Value = ConnectionPipe.VerlegeartEnum.VA_UNTER_ESTRICH;
				this.dgvConnectionPipes.Rows[dgvConnectionPipes.CurrentCell.RowIndex].Cells[Insulation.DisplayIndex].Value = ConnectionPipe.InsulationEnum.IN_VL_RL;
			}
			ReloadGrid();
		}

		private void verlegeartCombo_SelectedValueChanged(object sender, EventArgs e) {
			ConnectionPipe.VerlegeartEnum verlegeart = (ConnectionPipe.VerlegeartEnum)new ConnectionPipe.VerlegeartEnumConverter().ConvertFromString(verlegeartCombo.SelectedItem as String);
			this.dgvConnectionPipes.Rows[dgvConnectionPipes.CurrentCell.RowIndex].Cells[Verlegeart.DisplayIndex].Value = verlegeart;
			if (verlegeart == ConnectionPipe.VerlegeartEnum.VA_UNTER_ESTRICH) {
				this.dgvConnectionPipes.Rows[dgvConnectionPipes.CurrentCell.RowIndex].Cells[Insulation.DisplayIndex].Value = ConnectionPipe.InsulationEnum.IN_VL_RL;
			}
			ReloadGrid();
		}

		private void insulationCombo_SelectedValueChanged(object sender, EventArgs e) {
			ConnectionPipe.InsulationEnum insulation = (ConnectionPipe.InsulationEnum)new ConnectionPipe.InsulationEnumConverter().ConvertFromString(insulationCombo.SelectedItem as String);
			this.dgvConnectionPipes.Rows[dgvConnectionPipes.CurrentCell.RowIndex].Cells[Insulation.DisplayIndex].Value = insulation;
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
			dgvConnectionPipes.AllowUserToAddRows = false;
		}

		private void dgvConnectionPipes_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e) {
			if (this.product != null && this.product.Product.DefaultPipeType == ConnectionPipe.PipeTypeEnum.PT_21MM) {
				e.Row.Cells[PipeType.DisplayIndex].Value = ConnectionPipe.PipeTypeEnum.PT_21MM;
				e.Row.Cells[Verlegeart.DisplayIndex].Value = ConnectionPipe.VerlegeartEnum.VA_UNTER_ESTRICH;
				e.Row.Cells[Insulation.DisplayIndex].Value = ConnectionPipe.InsulationEnum.IN_VL_RL;
			} else {
				e.Row.Cells[PipeType.DisplayIndex].Value = ConnectionPipe.PipeTypeEnum.PT_EUROVAL;
				e.Row.Cells[Verlegeart.DisplayIndex].Value = ConnectionPipe.VerlegeartEnum.VA_EV5;
				e.Row.Cells[Insulation.DisplayIndex].Value = ConnectionPipe.InsulationEnum.IN_NONE;
			}
		}

	}
}
