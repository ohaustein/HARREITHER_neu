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

		private DataGridViewColumn colEuroval = null;
		private DataGridViewColumn colEurovalCircuits = null;
		private DataGridViewColumn colHitherm = null;
		private DataGridViewColumn colHithermCircuits = null;
		private DataGridViewColumn colHithermCompact = null;
		private DataGridViewColumn colHithermCompactCircuits = null;
		private DataGridViewColumn colModulKlimaBoden = null;
		private DataGridViewColumn colModulKlimaBodenCircuits = null;
		private DataGridViewColumn colModulKlimaDecke = null;
		private DataGridViewColumn colModulKlimaDeckeCircuits = null;

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
			get { return this.colEuroval != null; }
			set {
				if (value != this.Euroval) {
					if (value) {
						this.colEuroval = new DataGridViewTextBoxColumn();
						this.colEuroval.HeaderText = "Euroval\n(m²)";
						this.colEuroval.Width = 70;
						this.dataGridView1.Columns.Insert(this.colNrOfServos.Index, this.colEuroval);
						this.colEurovalCircuits = new DataGridViewTextBoxColumn();
						this.colEurovalCircuits.HeaderText = "Euroval\nHeizkreise";
						this.colEurovalCircuits.Width = 70;
						this.dataGridView1.Columns.Insert(this.colNrOfServos.Index, this.colEurovalCircuits);
					} else {
						this.dataGridView1.Columns.Remove(this.colEuroval);
						this.dataGridView1.Columns.Remove(this.colEurovalCircuits);
						this.colEuroval = null;
						this.colEurovalCircuits = null;
					}
				}
			}
		}

		public bool Hitherm {
			get { return this.colHitherm != null; }
			set {
				if (value != this.Hitherm) {
					if (value) {
						this.colHitherm = new DataGridViewTextBoxColumn();
						this.colHitherm.HeaderText = "Hitherm\n(m²)";
						this.colHitherm.Width = 70;
						this.dataGridView1.Columns.Insert(this.colNrOfServos.Index, this.colHitherm);
						this.colHithermCircuits = new DataGridViewTextBoxColumn();
						this.colHithermCircuits.HeaderText = "Hitherm\nHeizkreise";
						this.colHithermCircuits.Width = 70;
						this.dataGridView1.Columns.Insert(this.colNrOfServos.Index, this.colHithermCircuits);
					} else {
						this.dataGridView1.Columns.Remove(this.colHitherm);
						this.dataGridView1.Columns.Remove(this.colHithermCircuits);
						this.colHitherm = null;
						this.colHithermCircuits = null;
					}
				}
			}
		}

		public bool HithermCompact {
			get { return this.colHithermCompact != null; }
			set {
				if (value != this.HithermCompact) {
					if (value) {
						this.colHithermCompact = new DataGridViewTextBoxColumn();
						this.colHithermCompact.Width = 70;
						this.colHithermCompact.HeaderText = "Hitherm Co\n(m²)";
						this.dataGridView1.Columns.Insert(this.colNrOfServos.Index, this.colHithermCompact);
						this.colHithermCompactCircuits = new DataGridViewTextBoxColumn();
						this.colHithermCompactCircuits.HeaderText = "Hitherm Co\nHeizkreise";
						this.colHithermCompactCircuits.Width = 70;
						this.dataGridView1.Columns.Insert(this.colNrOfServos.Index, this.colHithermCompactCircuits);
					} else {
						this.dataGridView1.Columns.Remove(this.colHithermCompact);
						this.dataGridView1.Columns.Remove(this.colHithermCompactCircuits);
						this.colHithermCompact = null;
						this.colHithermCompactCircuits = null;
					}
				}
			}
		}

		public bool ModulKlimaBoden {
			get { return this.colModulKlimaBoden != null; }
			set {
				if (value != this.ModulKlimaBoden) {
					if (value) {
						this.colModulKlimaBoden = new DataGridViewTextBoxColumn();
						this.colModulKlimaBoden.HeaderText = "Klimaboden\n(m²)";
						this.colModulKlimaBoden.Width = 70;
						this.dataGridView1.Columns.Insert(this.colNrOfServos.Index, this.colModulKlimaBoden);
						this.colModulKlimaBodenCircuits = new DataGridViewTextBoxColumn();
						this.colModulKlimaBodenCircuits.HeaderText = "Klimaboden\nHeizkreise";
						this.colModulKlimaBodenCircuits.Width = 70;
						this.dataGridView1.Columns.Insert(this.colNrOfServos.Index, this.colModulKlimaBodenCircuits);
					} else {
						this.dataGridView1.Columns.Remove(this.colModulKlimaBoden);
						this.dataGridView1.Columns.Remove(this.colModulKlimaBodenCircuits);
						this.colModulKlimaBoden = null;
						this.colModulKlimaBodenCircuits = null;
					}
				}
			}
		}

		public bool ModulKlimaDecke {
			get { return this.colModulKlimaDecke != null; }
			set {
				if (value != this.ModulKlimaDecke) {
					if (value) {
						this.colModulKlimaDecke = new DataGridViewTextBoxColumn();
						this.colModulKlimaDecke.HeaderText = "Klimadecke\n(m²)";
						this.colModulKlimaDecke.Width = 70;
						this.dataGridView1.Columns.Insert(this.colNrOfServos.Index, this.colModulKlimaDecke);
						this.colModulKlimaDeckeCircuits = new DataGridViewTextBoxColumn();
						this.colModulKlimaDeckeCircuits.HeaderText = "Klimadecke\nHeizkreise";
						this.colModulKlimaDeckeCircuits.Width = 70;
						this.dataGridView1.Columns.Insert(this.colNrOfServos.Index, this.colModulKlimaDeckeCircuits);
					} else {
						this.dataGridView1.Columns.Remove(this.colModulKlimaDecke);
						this.dataGridView1.Columns.Remove(this.colModulKlimaDeckeCircuits);
						this.colModulKlimaDecke = null;
						this.colModulKlimaDeckeCircuits = null;
					}
				}
			}
		}
	}
}
