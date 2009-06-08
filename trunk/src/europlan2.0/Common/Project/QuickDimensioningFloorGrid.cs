using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using log4net;

namespace Europlan.Common {
	public partial class QuickDimensioningFloorGrid : UserControl {

		private Floor floor = null;

		private static ILog log = LogManager.GetLogger(typeof(QuickDimensioningFloorGrid));

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
				if (!value) {
					foreach (DataGridViewRow row in this.dataGridView1.Rows) {
						row.Cells[this.colEuroval.Index].Value = null;
						row.Cells[this.colEurovalCircuits.Index].Value = null;
					}
				}
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

		private bool heatLoadWasDefault = false;
		private bool coolLoadWasDefault = false;

		private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e) {
			if (e.ColumnIndex == this.colRoomType.Index) {
				Room room = this.dataGridView1.Rows[e.RowIndex].DataBoundItem as Room;
				if (room != null) {
					this.heatLoadWasDefault = (room.QuickDimensioningHeatLoad == room.GetDefaultQuickDimensioningHeatLoad() && room.QuickDimensioningHeatLoad != room.HeatLoad);
					this.coolLoadWasDefault = (room.QuickDimensioningCoolLoad == room.GetDefaultQuickDimensioningCoolLoad() && room.QuickDimensioningCoolLoad != room.CoolLoad);
				}
			}
		}

		private void dataGridView1_CellValidated(object sender, DataGridViewCellEventArgs e) {
			DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
			Room room = row.DataBoundItem as Room;
			if (room != null) {
				if (e.ColumnIndex == this.colEuroval.Index) {
					Product product = room.GetProductForQuickDimensioning<EurovalProduct>();
					object o = row.Cells[this.colEuroval.Index].Value;
					if (o == null || (decimal)o == 0) {
						if (product != null) {
							room.UsedProductsForQuickDimensioning.Remove(product);
						}
						row.Cells[this.colEuroval.Index].Value = null;
						row.Cells[this.colEurovalCircuits.Index].Value = null;
					} else {
						if (product == null) {
							log.Warn("Product for validated cell is null");
							product = Project.Instance.Config.EurovalProduct.Clone(room);
						}
						decimal area = (decimal)o;
						bool setCircuits = (product.QuickDimensioningCircuits == product.GetDefaultQuickDimensioningCircuits());
						product.QuickDimensioningPlannedArea = (float)area;
						if (setCircuits) {
							product.QuickDimensioningCircuits = product.GetDefaultQuickDimensioningCircuits();
							row.Cells[this.colEurovalCircuits.Index].Value = product.QuickDimensioningCircuitsAsString;
						}
					}

					// check if heat- and coolload are covered
					this.CheckLoadsCovered(row);

					// check if planned area exceeds maximum area
					if (product == null || product.QuickDimensioningPlannedArea <= product.QuickDimensioningMaximumArea) {
						row.Cells[this.colEuroval.Index].ErrorText = null;
					} else {
						row.Cells[this.colEuroval.Index].ErrorText = "Die geplante Fläche ist größer als die maximal verfügbare Fläche";
					}
				} else if (e.ColumnIndex == this.colEurovalCircuits.Index) {
					Product product = room.GetProductForQuickDimensioning<EurovalProduct>();
					if (product == null) {
						log.Warn("Product for validated cell is null");
						product = Project.Instance.Config.EurovalProduct.Clone(room);
						product.QuickDimensioningPlannedArea = product.GetDefaultQuickDimensioningPlannedArea();
					}
					string circuits = row.Cells[this.colEurovalCircuits.Index].Value as string;
					product.QuickDimensioningCircuitsAsString = circuits;
				} else if (e.ColumnIndex == this.colHitherm.Index) {
					Product product = room.GetProductForQuickDimensioning<HithermProduct>();
					object o = row.Cells[this.colHitherm.Index].Value;
					if (o == null || (decimal)o == 0) {
						if (product != null) {
							room.UsedProductsForQuickDimensioning.Remove(product);
						}
						row.Cells[this.colHitherm.Index].Value = null;
						row.Cells[this.colHithermCircuits.Index].Value = null;
					} else {
						if (product == null) {
							log.Warn("Product for validated cell is null");
							product = Project.Instance.Config.HithermProduct.Clone(room);
						}
						decimal area = (decimal)o;
						bool setCircuits = (product.QuickDimensioningCircuits == product.GetDefaultQuickDimensioningCircuits());
						product.QuickDimensioningPlannedArea = (float)area;
						if (setCircuits) {
							product.QuickDimensioningCircuits = product.GetDefaultQuickDimensioningCircuits();
							row.Cells[this.colHithermCircuits.Index].Value = product.QuickDimensioningCircuitsAsString;
						}
					}

					// check if heat- and coolload are covered
					this.CheckLoadsCovered(row);

					// check if planned area exceeds maximum area
					if (product == null || product.QuickDimensioningPlannedArea <= product.QuickDimensioningMaximumArea) {
						row.Cells[this.colHitherm.Index].ErrorText = null;
					} else {
						row.Cells[this.colHitherm.Index].ErrorText = "Die geplante Fläche ist größer als die maximal verfügbare Fläche";
					}
				} else if (e.ColumnIndex == this.colHithermCircuits.Index) {
				} else if (e.ColumnIndex == this.colHithermCompact.Index) {
					Product product = room.GetProductForQuickDimensioning<HithermCompactProduct>();
					object o = row.Cells[this.colHithermCompact.Index].Value;
					if (o == null || (decimal)o == 0) {
						if (product != null) {
							room.UsedProductsForQuickDimensioning.Remove(product);
						}
						row.Cells[this.colHithermCompact.Index].Value = null;
						row.Cells[this.colHithermCompactCircuits.Index].Value = null;
					} else {
						if (product == null) {
							log.Warn("Product for validated cell is null");
							product = Project.Instance.Config.HithermCompactProduct.Clone(room);
						}
						decimal area = (decimal)o;
						bool setCircuits = (product.QuickDimensioningCircuits == product.GetDefaultQuickDimensioningCircuits());
						product.QuickDimensioningPlannedArea = (float)area;
						if (setCircuits) {
							product.QuickDimensioningCircuits = product.GetDefaultQuickDimensioningCircuits();
							row.Cells[this.colHithermCircuits.Index].Value = product.QuickDimensioningCircuitsAsString;
						}
					}

					// check if heat- and coolload are covered
					this.CheckLoadsCovered(row);

					// check if planned area exceeds maximum area
					if (product == null || product.QuickDimensioningPlannedArea <= product.QuickDimensioningMaximumArea) {
						row.Cells[this.colHitherm.Index].ErrorText = null;
					} else {
						row.Cells[this.colHitherm.Index].ErrorText = "Die geplante Fläche ist größer als die maximal verfügbare Fläche";
					}
				} else if (e.ColumnIndex == this.colHithermCompactCircuits.Index) {
				} else if (e.ColumnIndex == this.colModulKlimaBoden.Index) {
				} else if (e.ColumnIndex == this.colModulKlimaBodenCircuits.Index) {
				} else if (e.ColumnIndex == this.colModulKlimaDecke.Index) {
				} else if (e.ColumnIndex == this.colModulKlimaDeckeCircuits.Index) {
				} else if (e.ColumnIndex == this.colHeatLoad.Index) {
					// check if heat- and coolload are covered
					this.CheckLoadsCovered(row);
				} else if (e.ColumnIndex == this.colCoolLoad.Index) {
					// check if heat- and coolload are covered
					this.CheckLoadsCovered(row);
				} else if (e.ColumnIndex == this.colRoomType.Index) {
					// update default heat- and coolload
					if (this.heatLoadWasDefault) {
						row.Cells[this.colHeatLoad.Index].Value = (decimal)room.GetDefaultQuickDimensioningHeatLoad();
					}
					if (this.coolLoadWasDefault) {
						row.Cells[this.colCoolLoad.Index].Value = (decimal)room.GetDefaultQuickDimensioningCoolLoad();
					}
					this.heatLoadWasDefault = false;
					this.coolLoadWasDefault = false;

					// check if heat- and coolload are covered
					this.CheckLoadsCovered(row);
				}

				// Invalidate datagridview to update nr of servos
				this.dataGridView1.Invalidate();
			}
		}

		private void ValidateProductArea(int colAreaIndex, int colCircuitsIndex, Product product, Product productTemplate, DataGridViewRow row) {
			Room room = row.DataBoundItem as Room;
			object o = row.Cells[colAreaIndex].Value;
			if (o == null || (decimal)o == 0) {
				if (product != null) {
					room.UsedProductsForQuickDimensioning.Remove(product);
				}
				row.Cells[colAreaIndex].Value = null;
				row.Cells[colCircuitsIndex].Value = null;
			} else {
				if (product == null) {
					log.Warn("Product for validated cell is null");
					product = productTemplate.Clone(room);
				}
				decimal area = (decimal)o;
				bool setCircuits = (product.QuickDimensioningCircuits == product.GetDefaultQuickDimensioningCircuits());
				product.QuickDimensioningPlannedArea = (float)area;
				if (setCircuits) {
					product.QuickDimensioningCircuits = product.GetDefaultQuickDimensioningCircuits();
					row.Cells[colCircuitsIndex].Value = product.QuickDimensioningCircuitsAsString;
				}
			}

			// check if heat- and coolload are covered
			this.CheckLoadsCovered(row);

			// check if planned area exceeds maximum area
			if (product == null || product.QuickDimensioningPlannedArea <= product.QuickDimensioningMaximumArea) {
				row.Cells[colAreaIndex].ErrorText = null;
			} else {
				row.Cells[colAreaIndex].ErrorText = "Die geplante Fläche ist größer als die maximal verfügbare Fläche";
			}
		}

		private void CheckLoadsCovered(DataGridViewRow row) {
			Room room = row.DataBoundItem as Room;
			if (room.QuickDimensioningHeatLoadCovered && (room.QuickDimensioningCoolLoadCovered || !this.Cooling)) {
				row.ErrorText = null;
			} else {
				if (!room.QuickDimensioningHeatLoadCovered && (!room.QuickDimensioningCoolLoadCovered && this.Cooling)) {
					row.ErrorText = "Heiz- und Kühllast nicht abgedeckt";
				} else if (!room.QuickDimensioningCoolLoadCovered && this.Cooling) {
					row.ErrorText = "Kühllast nicht abgedeckt";
				} else {
					row.ErrorText = "Heizlast nicht abgedeckt";
				}
			}
		}

		private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e) {
			for (int i = e.RowIndex; i < e.RowIndex + e.RowCount; i++) {
				DataGridViewRow row = this.dataGridView1.Rows[i];
				Room room = row.DataBoundItem as Room;
				if (room != null) {
					// Euroval
					EurovalProduct euroval = room.GetProductForQuickDimensioning<EurovalProduct>();
					if (this.colEuroval.Visible && euroval != null) {
						row.Cells[this.colEuroval.Index].Value = (decimal)euroval.QuickDimensioningPlannedArea;
						row.Cells[this.colEurovalCircuits.Index].Value = euroval.QuickDimensioningCircuitsAsString;
					}

					// Hitherm
					HithermProduct hitherm = room.GetProductForQuickDimensioning<HithermProduct>();
					if (this.colHitherm.Visible && hitherm != null) {
						row.Cells[this.colHitherm.Index].Value = (decimal)hitherm.QuickDimensioningPlannedArea;
						row.Cells[this.colHithermCircuits.Index].Value = hitherm.QuickDimensioningCircuitsAsString;
					}

					// Hitherm Comact
					HithermCompactProduct hithermCompact = room.GetProductForQuickDimensioning<HithermCompactProduct>();
					if (this.colHithermCompact.Visible && hithermCompact != null) {
						row.Cells[this.colHithermCompact.Index].Value = (decimal)hithermCompact.QuickDimensioningPlannedArea;
						row.Cells[this.colHithermCompactCircuits.Index].Value = hithermCompact.QuickDimensioningCircuitsAsString;
					}

					// Module Klimaboden
					ModulKlimaBodenProduct klimaBoden = room.GetProductForQuickDimensioning<ModulKlimaBodenProduct>();
					if (this.colModulKlimaBoden.Visible && klimaBoden != null) {
						row.Cells[this.colModulKlimaBoden.Index].Value = (decimal)klimaBoden.QuickDimensioningPlannedArea;
						row.Cells[this.colModulKlimaBodenCircuits.Index].Value = klimaBoden.QuickDimensioningCircuitsAsString;
					}

					// Module Klimadecke
					ModulKlimaDeckeProduct klimaDecke = room.GetProductForQuickDimensioning<ModulKlimaDeckeProduct>();
					if (this.colModulKlimaDecke.Visible && klimaDecke != null) {
						row.Cells[this.colModulKlimaDecke.Index].Value = (decimal)klimaDecke.QuickDimensioningPlannedArea;
						row.Cells[this.colModulKlimaDeckeCircuits.Index].Value = klimaDecke.QuickDimensioningCircuitsAsString;
					}

					this.CheckLoadsCovered(row);
				}
			}
		}

		private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e) {
			Room room = this.dataGridView1.Rows[e.RowIndex].DataBoundItem as Room;
			if (room != null) {
				if (e.ColumnIndex == this.colHeatLoad.Index && room.QuickDimensioningHeatLoad == 0) {
					// get and set default heatload (if heatload is already set use it, if it is not set get default heatload for quickdimensioning)
					int heatload = room.HeatLoad;
					if (heatload <= 0) {
						heatload = room.GetDefaultQuickDimensioningHeatLoad();
					}
					this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = heatload;
				} else if (e.ColumnIndex == this.colCoolLoad.Index && room.QuickDimensioningCoolLoad == 0) {
					// get and set default coolload (if coolload is already set use it, if it is not set get default coolload for quickdimensioning)
					int coolload = room.CoolLoad;
					if (coolload <= 0) {
						coolload = room.GetDefaultQuickDimensioningCoolLoad();
					}
					this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = coolload;
				} else if ((e.ColumnIndex == this.colEuroval.Index || e.ColumnIndex == this.colEurovalCircuits.Index) && room.GetProductForQuickDimensioning<EurovalProduct>() == null) {
					// get new Euroval Product
					Product product = Project.Instance.Config.EurovalProduct.Clone(room);

					// get and set default area
					product.QuickDimensioningPlannedArea = product.GetDefaultQuickDimensioningPlannedArea();
					this.dataGridView1.Rows[e.RowIndex].Cells[this.colEuroval.Index].Value = (decimal)product.QuickDimensioningPlannedArea;

					// get and set default circuits
					product.QuickDimensioningCircuits = product.GetDefaultQuickDimensioningCircuits();
					this.dataGridView1.Rows[e.RowIndex].Cells[this.colEurovalCircuits.Index].Value = product.QuickDimensioningCircuitsAsString;
				} else if ((e.ColumnIndex == this.colHitherm.Index || e.ColumnIndex == this.colHithermCircuits.Index) && room.GetProductForQuickDimensioning<HithermProduct>() == null) {
					// get new Hitherm Product
					Product product = Project.Instance.Config.HithermProduct.Clone(room);

					// get and set default area
					product.QuickDimensioningPlannedArea = product.GetDefaultQuickDimensioningPlannedArea();
					this.dataGridView1.Rows[e.RowIndex].Cells[this.colHitherm.Index].Value = (decimal)product.QuickDimensioningPlannedArea;

					// get and set default circuits
					product.QuickDimensioningCircuits = product.GetDefaultQuickDimensioningCircuits();
					this.dataGridView1.Rows[e.RowIndex].Cells[this.colHithermCircuits.Index].Value = product.QuickDimensioningCircuitsAsString;
				} else if ((e.ColumnIndex == this.colHithermCompact.Index || e.ColumnIndex == this.colHithermCompactCircuits.Index) && room.GetProductForQuickDimensioning<HithermCompactProduct>() == null) {
					// get new Hitherm Compact Product
					Product product = Project.Instance.Config.HithermCompactProduct.Clone(room);

					// get and set default area
					product.QuickDimensioningPlannedArea = product.GetDefaultQuickDimensioningPlannedArea();
					this.dataGridView1.Rows[e.RowIndex].Cells[this.colHithermCompact.Index].Value = (decimal)product.QuickDimensioningPlannedArea;

					// get and set default circuits
					product.QuickDimensioningCircuits = product.GetDefaultQuickDimensioningCircuits();
					this.dataGridView1.Rows[e.RowIndex].Cells[this.colHithermCompactCircuits.Index].Value = product.QuickDimensioningCircuitsAsString;
				} else if ((e.ColumnIndex == this.colModulKlimaBoden.Index || e.ColumnIndex == this.colModulKlimaBodenCircuits.Index) && room.GetProductForQuickDimensioning<ModulKlimaBodenProduct>() == null) {
					// get new Modul Klimaboden Product
					Product product = Project.Instance.Config.ModulKlimaBodenProduct.Clone(room);

					// get and set default area
					product.QuickDimensioningPlannedArea = product.GetDefaultQuickDimensioningPlannedArea();
					this.dataGridView1.Rows[e.RowIndex].Cells[this.colModulKlimaBoden.Index].Value = (decimal)product.QuickDimensioningPlannedArea;

					// get and set default circuits
					product.QuickDimensioningCircuits = product.GetDefaultQuickDimensioningCircuits();
					this.dataGridView1.Rows[e.RowIndex].Cells[this.colModulKlimaBodenCircuits.Index].Value = product.QuickDimensioningCircuitsAsString;
				} else if ((e.ColumnIndex == this.colModulKlimaDecke.Index || e.ColumnIndex == this.colModulKlimaDeckeCircuits.Index) && room.GetProductForQuickDimensioning<ModulKlimaDeckeProduct>() == null) {
					// get new Modul Klimaboden Product
					Product product = Project.Instance.Config.ModulKlimaDeckeProduct.Clone(room);

					// get and set default area
					product.QuickDimensioningPlannedArea = product.GetDefaultQuickDimensioningPlannedArea();
					this.dataGridView1.Rows[e.RowIndex].Cells[this.colModulKlimaDecke.Index].Value = (decimal)product.QuickDimensioningPlannedArea;

					// get and set default circuits
					product.QuickDimensioningCircuits = product.GetDefaultQuickDimensioningCircuits();
					this.dataGridView1.Rows[e.RowIndex].Cells[this.colModulKlimaDeckeCircuits.Index].Value = product.QuickDimensioningCircuitsAsString;
				}
			}
		}
	}
}
