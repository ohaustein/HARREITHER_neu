using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Europlan.Common {

	public delegate void GridContentChangedHandler(object sender);

	public partial class ConnectionPipePanel : UserControl {

		private Button roomSelectionButton;
		private PlannedProduct product;
		private bool showPipesThroughProduct = false;

		public event GridContentChangedHandler GridContentChanged;

		public ConnectionPipePanel() {
			InitializeComponent();

			roomSelectionButton = new Button();
			roomSelectionButton.Size = new Size(30, 20);
			roomSelectionButton.Text = "...";

			dgvConnectionPipes.Controls.Add(roomSelectionButton);
			roomSelectionButton.Hide();
			roomSelectionButton.Click += new EventHandler(roomSelectionButton_Click);
			ConfigureColumnVisibility();
		}

		private void ConfigureColumnVisibility() {
			if (showPipesThroughProduct) {
				Room.Visible = true;
				Area.Visible = true;
				HeatLoad.Visible = true;
				CoolLoad.Visible = true;
			}
		}

		public bool ShowPipesThroughProduct {
			get { return showPipesThroughProduct; }
			set {
				this.showPipesThroughProduct = value;
				ConfigureColumnVisibility();
			}
		}

		public void Update(PlannedProduct product) {
			this.product = product;
			// pipe type items
			this.PipeType.Items.Clear();
			this.PipeType.Items.Add(ConnectionPipe.PipeTypeEnum.PT_EUROVAL);
			if (this.product != null && this.product.Product != null &&
				this.product.Product.PlannedConnection != null) {
				PlannedProduct connectedProduct = this.product.Product.PlannedConnection.OtherProduct;
				if (connectedProduct != null && connectedProduct.Product.DefaultPipeType == ConnectionPipe.PipeTypeEnum.PT_21MM) {
					this.PipeType.Items.Add(ConnectionPipe.PipeTypeEnum.PT_21MM);
				}
			}

			// verlegeart items
			this.Verlegeart.Items.Clear();
			this.Verlegeart.Items.Add(ConnectionPipe.VerlegeartEnum.VA_UNTER_ESTRICH);
			this.Verlegeart.Items.Add(ConnectionPipe.VerlegeartEnum.VA_EV35);
			this.Verlegeart.Items.Add(ConnectionPipe.VerlegeartEnum.VA_EV30);
			this.Verlegeart.Items.Add(ConnectionPipe.VerlegeartEnum.VA_EV25);
			this.Verlegeart.Items.Add(ConnectionPipe.VerlegeartEnum.VA_EV20);
			this.Verlegeart.Items.Add(ConnectionPipe.VerlegeartEnum.VA_EV15);
			this.Verlegeart.Items.Add(ConnectionPipe.VerlegeartEnum.VA_EV10);
			this.Verlegeart.Items.Add(ConnectionPipe.VerlegeartEnum.VA_EV5);
			this.Verlegeart.Items.Add(ConnectionPipe.VerlegeartEnum.VA_A5);

			// insulation items
			this.Insulation.Items.Clear();
			this.Insulation.Items.Add(ConnectionPipe.InsulationEnum.IN_NONE);
			this.Insulation.Items.Add(ConnectionPipe.InsulationEnum.IN_VL);
			this.Insulation.Items.Add(ConnectionPipe.InsulationEnum.IN_VL_RL);

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
			if (((e.ColumnIndex == roomDataGridViewComboBoxColumn.DisplayIndex) || (e.ColumnIndex == productDataGridViewComboBoxColumn.DisplayIndex)) && e.RowIndex >= 0) {
				Rectangle rect = dgvConnectionPipes.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
				int width = dgvConnectionPipes.CurrentCell.Size.Width;
				roomSelectionButton.Location = new Point(rect.X + width - roomSelectionButton.Width, rect.Y);
				roomSelectionButton.Height = dgvConnectionPipes.Rows[e.RowIndex].Height;
				roomSelectionButton.Show();
			}
		}

		private void dgvConnectionPipes_CellLeave(object sender, DataGridViewCellEventArgs e) {
			roomSelectionButton.Hide();
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
							if (!this.product.Product.AssociatedRoom.Equals(r)) {
								rooms.Add(r);
							}
						}
					}
				}
				SelectRoomForm form = new SelectRoomForm(rooms);
				if (form.ShowDialog().Equals(DialogResult.OK)) {
					Room room = form.SelectedRoom;
					if (dgvConnectionPipes.CurrentCell.Value != room) {
						dgvConnectionPipes.CurrentCell.Value = room;
						this.dgvConnectionPipes.Rows[dgvConnectionPipes.CurrentCell.RowIndex].Cells[productDataGridViewComboBoxColumn.DisplayIndex].Value = null;
					}
				}
				form.Dispose();
			} else if (dgvConnectionPipes.CurrentCell.ColumnIndex == productDataGridViewComboBoxColumn.DisplayIndex) {
				List<PlannedProduct> products = new List<PlannedProduct>();

				Room r = this.dgvConnectionPipes.Rows[dgvConnectionPipes.CurrentCell.RowIndex].Cells[roomDataGridViewComboBoxColumn.DisplayIndex].Value as Room;
				// product items
				if (r != null) {
					foreach (PlannedProduct p in r.PlannedProducts) {
						products.Add(p);
					}

					SelectPlannedProduct form = new SelectPlannedProduct(products);
					if (form.ShowDialog().Equals(DialogResult.OK)) {
						PlannedProduct product = form.SelectedPlannedProduct;
						dgvConnectionPipes.CurrentCell.Value = product;
					}
					form.Dispose();
				}
			}
		}



	}
}
