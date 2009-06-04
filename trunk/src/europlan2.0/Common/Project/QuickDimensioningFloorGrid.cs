using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Europlan.Common {
	public partial class QuickDimensioningFloorGrid : UserControl {

		private Floor floor = null;

		/*private DataGridViewColumn colEurovalOld = null;
		private DataGridViewColumn colEurovalCircuitsOld = null;
		private DataGridViewColumn colHithermOld = null;
		private DataGridViewColumn colHithermCircuitsOld = null;
		private DataGridViewColumn colHithermCompactOld = null;
		private DataGridViewColumn colHithermCompactCircuitsOld = null;
		private DataGridViewColumn colModulKlimaBodenOld = null;
		private DataGridViewColumn colModulKlimaBodenCircuitsOld = null;
		private DataGridViewColumn colModulKlimaDeckeOld = null;
		private DataGridViewColumn colModulKlimaDeckeCircuitsOld = null;*/

		public QuickDimensioningFloorGrid() {
			InitializeComponent();
			this.colRoomType.ValueMember = "Value";
			this.colRoomType.DisplayMember = "Name";
			foreach (RoomType roomType in Project.Instance.Config.RoomTypes) {
				this.colRoomType.Items.Add(new RoomTypeItem(roomType));
			}
			this.colRoomController.ValueMember = "Controller";
			this.colRoomController.DisplayMember = "Name";
			this.colRoomController.Items.Add(new RoomControllerItem(Room.RoomController.None, ""));
			this.colRoomController.Items.Add(new RoomControllerItem(Room.RoomController.RC, "RC"));
			this.colRoomController.Items.Add(new RoomControllerItem(Room.RoomController.RCF, "RCF"));
			this.colRoomController.Items.Add(new RoomControllerItem(Room.RoomController.RCRadio, "RCRadio"));
			this.colRoomController.Items.Add(new RoomControllerItem(Room.RoomController.RF, "RF"));

		}

		private class RoomControllerItem {
			private Room.RoomController controller;
			private string name;

			public RoomControllerItem(Room.RoomController controller, string name) {
				this.controller = controller;
				this.name = name;
			}

			public Room.RoomController Controller {
				get { return this.controller; }
			}

			public string Name {
				get { return this.name; }
			}
		}

		private class RoomTypeItem {
			private RoomType roomType;

			public RoomTypeItem(RoomType roomType) {
				this.roomType = roomType;
			}

			public RoomType Value {
				get { return this.roomType; }
			}

			public string Name {
				get { return this.roomType.Name; }
			}
		}

		public Floor Floor {
			get { return this.floor; }
			set {
				this.floor = value;
				this.roomBindingSource.DataSource = this.floor.Rooms;
				this.roomBindingSource.ResetBindings(false);
				foreach (DataGridViewRow row in this.dataGridView1.Rows) {
					Room room = row.DataBoundItem as Room;
					if (room != null) {
						EurovalProduct euroval = room.GetProductForQuickDimensioning<EurovalProduct>();
						if (euroval != null) {
							row.Cells[this.colEuroval.Index].Value = euroval.QuickDimensioningPlannedArea;
							row.Cells[this.colEurovalCircuits.Index].Value = euroval.QuickDimensioningCircuits;
						}
					}
				}
			}
		}

		public bool Heating {
			get { return this.colHeatLoad.Visible; }
			/*set { this.colHeatLoad.Visible = value; }*/
		}

		public bool Cooling {
			get { return this.colCoolLoad.Visible; }
			set { this.colCoolLoad.Visible = value; }
		}

		public bool Euroval {
			get { return this.colEuroval.Visible; }
			set {
				this.colEuroval.Visible = value;
				this.colEurovalCircuits.Visible = value;
			}
		}

		public bool Hitherm {
			get { return this.colHitherm.Visible; }
			set {
				this.colHitherm.Visible = value;
				this.colHithermCircuits.Visible = value;
			}
		}

		public bool HithermCompact {
			get { return this.colHithermCompact.Visible; }
			set {
				this.colHithermCompact.Visible = value;
				this.colHithermCompactCircuits.Visible = value;
			}
		}

		public bool ModulKlimaBoden {
			get { return this.colModulKlimaBoden.Visible; }
			set {
				this.colModulKlimaBoden.Visible = value;
				this.colModulKlimaBodenCircuits.Visible = value;
			}
		}

		public bool ModulKlimaDecke {
			get { return this.colModulKlimaDecke.Visible; }
			set {
				this.colModulKlimaDecke.Visible = value;
				this.colModulKlimaDeckeCircuits.Visible = value;
			}
		}

		private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e) {
			/*Room room = this.dataGridView1.Rows[e.RowIndex].DataBoundItem as Room;
			if (room != null) {
				if (this.colEuroval != null && e.ColumnIndex == this.colEuroval.Index) {
					Console.WriteLine(e.FormattedValue);
					Product product = room.GetProductForQuickDimensioning(typeof(EurovalProduct));
					if (product == null) {
					}
					product.QuickDimensioningPlannedArea = 
				} else if (this.colHitherm != null && e.ColumnIndex == this.colHitherm.Index) {
				} else if (this.colHithermCompact != null && e.ColumnIndex == this.colHithermCompact.Index) {
				} else if (this.colModulKlimaBoden != null && e.ColumnIndex == this.colModulKlimaBoden.Index) {
				} else if (this.colModulKlimaDecke != null && e.ColumnIndex == this.colModulKlimaDecke.Index) {
				}
			}*/
		}

		private void dataGridView1_CellValidated(object sender, DataGridViewCellEventArgs e) {
			Room room = this.dataGridView1.Rows[e.RowIndex].DataBoundItem as Room;
			if (room != null) {
				if (e.ColumnIndex == this.colEuroval.Index) {
					Product product = room.GetProductForQuickDimensioning<EurovalProduct>();
					if (product == null) {
						product = Project.Instance.Config.EurovalProduct.Clone(room);
					}
					decimal area = 0;
					object o = this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
					if (o != null) {
						area = (decimal)o;
					}
					bool setCircuits = (product.QuickDimensioningCircuits == product.GetDefaultQuickDimensioningCircuits());
					product.QuickDimensioningPlannedArea = (float)area;
					if (setCircuits) {
						product.QuickDimensioningCircuits = product.GetDefaultQuickDimensioningCircuits();
					}
				} else if (e.ColumnIndex == this.colEurovalCircuits.Index) {
				} else if (e.ColumnIndex == this.colHitherm.Index) {
				} else if (e.ColumnIndex == this.colHithermCircuits.Index) {
				} else if (e.ColumnIndex == this.colHithermCompact.Index) {
				} else if (e.ColumnIndex == this.colHithermCompactCircuits.Index) {
				} else if (e.ColumnIndex == this.colModulKlimaBoden.Index) {
				} else if (e.ColumnIndex == this.colModulKlimaBodenCircuits.Index) {
				} else if (e.ColumnIndex == this.colModulKlimaDecke.Index) {
				} else if (e.ColumnIndex == this.colModulKlimaDeckeCircuits.Index) {
				}
			}
		}

		private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e) {
			for (int i = e.RowIndex; i < e.RowIndex + e.RowCount; i++) {
				DataGridViewRow row = this.dataGridView1.Rows[i];
				Room room = row.DataBoundItem as Room;
				if (room != null) {
					EurovalProduct euroval = room.GetProductForQuickDimensioning<EurovalProduct>();
					if (this.colEuroval.Visible && euroval != null) {
						row.Cells[this.colEuroval.Index].Value = (decimal)euroval.QuickDimensioningPlannedArea;
						row.Cells[this.colEurovalCircuits.Index].Value = euroval.QuickDimensioningCircuits;
					}
				}
			}

		}

		private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e) {
			Room room = this.dataGridView1.Rows[e.RowIndex].DataBoundItem as Room;
			if (room != null) {
				if (e.ColumnIndex == this.colHeatLoad.Index && room.QuickDimensioningHeatLoad == 0) {
					this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = room.GetDefaultQuickDimensioningHeatLoad();
				} else if (e.ColumnIndex == this.colCoolLoad.Index && room.QuickDimensioningCoolLoad == 0) {
					this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = room.GetDefaultQuickDimensioningCoolLoad();
				} else if (e.ColumnIndex == this.colEuroval.Index || e.ColumnIndex == this.colEurovalCircuits.Index && room.GetProductForQuickDimensioning<EurovalProduct>() == null) {
					Product product = Project.Instance.Config.EurovalProduct.Clone(room);
					product.QuickDimensioningPlannedArea = product.GetDefaultQuickDimensioningPlannedArea();
					this.dataGridView1.Rows[e.RowIndex].Cells[this.colEuroval.Index].Value = (decimal)product.QuickDimensioningPlannedArea;
					product.QuickDimensioningCircuits = product.GetDefaultQuickDimensioningCircuits();
					this.dataGridView1.Rows[e.RowIndex].Cells[this.colEurovalCircuits.Index].Value = product.QuickDimensioningCircuitsAsString;
				}
			}
		}
	}
}
