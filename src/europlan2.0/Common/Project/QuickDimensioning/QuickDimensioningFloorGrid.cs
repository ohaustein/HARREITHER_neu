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

        private event ProjectChangedHandler projectChanged;
        public event ProjectChangedHandler ProjectChanged {
            add { this.projectChanged += value; }
            remove { this.projectChanged -= value; }
        }

		private Floor floor = null;
		private RoomType newRoomType;

		private static ILog log = LogManager.GetLogger(typeof(QuickDimensioningFloorGrid));

		public QuickDimensioningFloorGrid() {
			InitializeComponent();

			this.SetLanguage();

			this.colRoomType.ValueMember = "Value";
			this.colRoomType.DisplayMember = "Name";
			foreach (RoomType roomType in Project.Instance.Config.RoomTypes) {
				this.colRoomType.Items.Add(new RoomTypeItem(roomType));
			}
			this.newRoomType = new RoomType();
			this.newRoomType.Name = EuroplanRes.QuickDimensioningFloorGrid_NeuBearbeiten; //"<Neu/Bearbeiten>";
			this.newRoomType.Id = "<NEW>";
			this.newRoomType.UserDefined = true;
			this.colRoomType.Items.Add(new RoomTypeItem(this.newRoomType));
			this.colRoomController.ValueMember = "Controller";
			this.colRoomController.DisplayMember = "Name";
			this.colRoomController.Items.Add(new RoomControllerItem(Room.RoomController.None, ""));
			this.colRoomController.Items.Add(new RoomControllerItem(Room.RoomController.RC, EuroplanRes.QuickDimensioningFloorGrid_RC/*"RC"*/));
			this.colRoomController.Items.Add(new RoomControllerItem(Room.RoomController.RCF, EuroplanRes.QuickDimensioningFloorGrid_RCF/*"RCF"*/));
			this.colRoomController.Items.Add(new RoomControllerItem(Room.RoomController.RCRadio, EuroplanRes.QuickDimensioningFloorGrid_RCFunk/*"RC-Funk"*/));
			this.colRoomController.Items.Add(new RoomControllerItem(Room.RoomController.RF, EuroplanRes.QuickDimensioningFloorGrid_RF/*"RF"*/));
			//this.colRoomType.
		}

		private void SetLanguage() {
			this.colId.HeaderText = EuroplanRes.QuickDimensioningFloorGrid_Nr; //"Nr.";
			this.colName.HeaderText = EuroplanRes.QuickDimensioningFloorGrid_Bezeichnung; //"Bezeichnung";
			this.colRoomTemperature.HeaderText = EuroplanRes.QuickDimensioningFloorGrid_Temperatur; //"Temp.\n(°C)";
			this.colArea.HeaderText = EuroplanRes.QuickDimensioningFloorGrid_Raumflaeche; //"Raumfl.\n(m²)";
			this.colRoomType.HeaderText = EuroplanRes.QuickDimensioningFloorGrid_Raumtyp; //"Raumtyp";
			this.colHeatLoad.HeaderText = EuroplanRes.QuickDimensioningFloorGrid_Heizlast; //"Heizlast\n(W)";
			this.colCoolLoad.HeaderText = EuroplanRes.QuickDimensioningFloorGrid_Kuehllast; //"Kühllast\n(W)";
			this.colEuroval.HeaderText = EuroplanRes.QuickDimensioningFloorGrid_EurovalFlaeche; //"Euroval®\n(m²)";
			this.colEurovalCircuits.HeaderText = EuroplanRes.QuickDimensioningFloorGrid_EurovalHeizkreise; //"Euroval®\nHeizkreise";
			this.colConcreteActivation.HeaderText = EuroplanRes.QuickDimensioningFloorGrid_BkaFlaeche; //"BKA\n(m²)";
			this.colConcreteActivationCircuits.HeaderText = EuroplanRes.QuickDimensioningFloorGrid_BkaHeizkreise; //"BKA\nHeizkreise";
			this.colHitherm.HeaderText = EuroplanRes.QuickDimensioningFloorGrid_HithermFlaeche; //"Hitherm®\n(m²)";
			this.colHithermCircuits.HeaderText = EuroplanRes.QuickDimensioningFloorGrid_HithermHeizkreise; //"Hitherm®\nHeizkreise";
			this.colHithermCompact.HeaderText = EuroplanRes.QuickDimensioningFloorGrid_HithermCompactFlaeche; //"Hitherm® Co\n(m²)";
			this.colHithermCompactCircuits.HeaderText = EuroplanRes.QuickDimensioningFloorGrid_HithermCompactHeizkreise; //"Hitherm® Co\nHeizkreise";
			this.colHithermCompactRoof.HeaderText = EuroplanRes.QuickDimensioningFloorGrid_HithermCompactDachFlaeche; //"Hitherm® Co\nDach (m²)";
			this.colHithermCompactRoofCircuits.HeaderText = EuroplanRes.QuickDimensioningFloorGrid_HithermCompactDachHeizkreise; //"Hitherm® Co\nDach Hkr.";
			this.colModulKlimaBoden.HeaderText = EuroplanRes.QuickDimensioningFloorGrid_KlimaBodenFlaeche; //"Klima-Boden\n(m²)";
			this.colModulKlimaBodenCircuits.HeaderText = EuroplanRes.QuickDimensioningFloorGrid_KlimaBodenHeizkreise; //"Klima-Boden\nHeizkreise";
			this.colModulKlimaDecke.HeaderText = EuroplanRes.QuickDimensioningFloorGrid_KlimaDeckeFlaeche; //"Klima-Decke\n(m²)";
			this.colModulKlimaDeckeCircuits.HeaderText = EuroplanRes.QuickDimensioningFloorGrid_KlimaDeckeHeizkreise; //"Klima-Decke\nHeizkreise";
			this.colRoomController.HeaderText = EuroplanRes.QuickDimensioningFloorGrid_RaumController; //"Raum-\ncontroller";
			this.colNrOfServos.HeaderText = EuroplanRes.QuickDimensioningFloorGrid_Stellmotore; //"Stell-\nmotore";
			this.colComments.HeaderText = EuroplanRes.QuickDimensioningFloorGrid_Bemerkung; //"Bemerkung";
			this.colRevert.HeaderText = EuroplanRes.QuickDimensioningFloorGrid_Ruecksetzen; //"Rücksetzen";
			this.colRevert.Text = EuroplanRes.QuickDimensioningFloorGrid_Ruecksetzen; //"Rücksetzen";
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
			get { return this.colEuroval.Visible; }
			set {
				/*this.colEuroval.Visible = value;
				this.colEurovalCircuits.Visible = value;
				if (!value) {
					foreach (DataGridViewRow row in this.dataGridView1.Rows) {
						row.Cells[this.colEuroval.Index].Value = null;
						row.Cells[this.colEurovalCircuits.Index].Value = null;
					}
				}*/
				this.SetProductAvailable<EurovalProduct>(value);
			}
		}

		public bool ConcreteActivation {
			// TODO !!!
			get { return this.colConcreteActivation.Visible; }
			set { this.SetProductAvailable<ConcreteActivationProduct>(value); }
		}

		public bool Hitherm {
			get { return this.colHitherm.Visible; }
			set { this.SetProductAvailable<HithermProduct>(value); }
		}

		public bool HithermCompact {
			get { return this.colHithermCompact.Visible; }
			set { this.SetProductAvailable<HithermCompactProduct>(value); }
		}

		public bool HithermCompactRoof {
			get { return this.colHithermCompactRoof.Visible; }
			set { this.SetProductAvailable<HithermCompactRoofProduct>(value); }
		}

		public bool ModulKlimaBoden {
			get { return this.colModulKlimaBoden.Visible; }
			set { this.SetProductAvailable<ModulKlimaBodenProduct>(value); }
		}

		public bool ModulKlimaDecke {
			get { return this.colModulKlimaDecke.Visible; }
			set { this.SetProductAvailable<ModulKlimaDeckeProduct>(value); }
		}

		private void SetProductAvailable<P>(bool available) where P:Product {
			DataGridViewColumn colProductArea = this.GetProductAreaColumn<P>();
			DataGridViewColumn colProductCircuits = this.GetProductCircuitsColumn<P>();
			colProductArea.Visible = available;
			colProductCircuits.Visible = available;
			if (!available) {
				foreach (DataGridViewRow row in this.quickDimensioningGrid.Rows) {
					row.Cells[colProductArea.Index].Value = null;
					row.Cells[colProductCircuits.Index].Value = null;
				}
			}
		}

		private bool heatLoadWasDefault = false;
		private bool coolLoadWasDefault = false;

		private void quickDimensioningGrid_CellValidating(object sender, DataGridViewCellValidatingEventArgs e) {
			if (e.ColumnIndex == this.colRoomType.Index) {
				Room room = this.quickDimensioningGrid.Rows[e.RowIndex].DataBoundItem as Room;
				if (room != null) {
					this.heatLoadWasDefault = (room.QuickDimensioningHeatLoad == room.GetDefaultQuickDimensioningHeatLoad() && room.QuickDimensioningHeatLoad != room.HeatLoad);
					this.coolLoadWasDefault = (room.QuickDimensioningCoolLoad == room.GetDefaultQuickDimensioningCoolLoad() && room.QuickDimensioningCoolLoad != room.CoolLoad);
				}
			}
		}

		private void quickDimensioningGrid_CellValidated(object sender, DataGridViewCellEventArgs e) {
			if (this.roomBindingSource.Count > 0) {
				DataGridViewRow row = this.quickDimensioningGrid.Rows[e.RowIndex];
				Room room = row.DataBoundItem as Room;
				if (room != null) {
					if (e.ColumnIndex == this.colEuroval.Index) {
						/*Product product = room.GetProductForQuickDimensioning<EurovalProduct>();
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
						}*/
						// Euroval
						this.ValidateProductArea<EurovalProduct>(row);
					} else if (e.ColumnIndex == this.colEurovalCircuits.Index) {
						/*Product product = room.GetProductForQuickDimensioning<EurovalProduct>();
						if (product == null) {
							log.Warn("Product for validated cell is null");
							product = Project.Instance.Config.EurovalProduct.Clone(room);
							product.QuickDimensioningPlannedArea = product.GetDefaultQuickDimensioningPlannedArea();
						}
						string circuits = row.Cells[this.colEurovalCircuits.Index].Value as string;
						product.QuickDimensioningCircuitsAsString = circuits;*/
						// Euroval circuits
						this.ValidateProductCircuits<EurovalProduct>(row);
					} else if (e.ColumnIndex == this.colConcreteActivation.Index) {
						// Concrete Activation
						this.ValidateProductArea<ConcreteActivationProduct>(row);
					} else if (e.ColumnIndex == this.colConcreteActivationCircuits.Index) {
						// Concrete Activation circuits
						this.ValidateProductCircuits<ConcreteActivationProduct>(row);
					} else if (e.ColumnIndex == this.colHitherm.Index) {
						// Hitherm
						this.ValidateProductArea<HithermProduct>(row);
					} else if (e.ColumnIndex == this.colHithermCircuits.Index) {
						// Hitherm circuits
						this.ValidateProductCircuits<HithermProduct>(row);
					} else if (e.ColumnIndex == this.colHithermCompact.Index) {
						// Hitherm Compact
						this.ValidateProductArea<HithermCompactProduct>(row);
					} else if (e.ColumnIndex == this.colHithermCompactCircuits.Index) {
						// Hitherm Compact circuits
						this.ValidateProductCircuits<HithermCompactProduct>(row);
					} else if (e.ColumnIndex == this.colHithermCompactRoof.Index) {
						// Hitherm Compact Roof
						this.ValidateProductArea<HithermCompactRoofProduct>(row);
					} else if (e.ColumnIndex == this.colHithermCompactRoofCircuits.Index) {
						// Hitherm Compact Roof circuits
						this.ValidateProductCircuits<HithermCompactRoofProduct>(row);
					} else if (e.ColumnIndex == this.colModulKlimaBoden.Index) {
						// Modul Klimaboden
						this.ValidateProductArea<ModulKlimaBodenProduct>(row);
					} else if (e.ColumnIndex == this.colModulKlimaBodenCircuits.Index) {
						// Modul Klimaboden circuits
						this.ValidateProductCircuits<ModulKlimaBodenProduct>(row);
					} else if (e.ColumnIndex == this.colModulKlimaDecke.Index) {
						// Modul Klimadecke
						this.ValidateProductArea<ModulKlimaDeckeProduct>(row);
					} else if (e.ColumnIndex == this.colModulKlimaDeckeCircuits.Index) {
						// Modul Klimadecke circuits
						this.ValidateProductCircuits<ModulKlimaDeckeProduct>(row);
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
					this.quickDimensioningGrid.Invalidate();
				}
			}
		}

		private void ValidateProductArea<P>(DataGridViewRow row) where P: Product {
			DataGridViewColumn colProductArea = this.GetProductAreaColumn<P>();
			DataGridViewColumn colProductCircuits = this.GetProductCircuitsColumn<P>();
			Room room = row.DataBoundItem as Room;
			P product = room.GetProductForQuickDimensioning<P>();
			object o = row.Cells[colProductArea.Index].Value;
			if (o == null || (decimal)o == 0) {
				if (product != null) {
					room.UsedProductsForQuickDimensioning.Remove(product);
				}
				row.Cells[colProductArea.Index].Value = null;
				row.Cells[colProductCircuits.Index].Value = null;
				product = null;
			} else {
				if (product == null) {
					log.Warn("Product for validated cell is null");
					product = (P)Project.Instance.Config.GetProduct<P>().Clone(room);
				}
				decimal area = (decimal)o;
				bool setCircuits = (product.QuickDimensioningCircuits == product.GetDefaultQuickDimensioningCircuits());
				product.QuickDimensioningPlannedArea = (float)area;
				if (setCircuits) {
					product.QuickDimensioningCircuits = product.GetDefaultQuickDimensioningCircuits();
					row.Cells[colProductCircuits.Index].Value = product.QuickDimensioningCircuitsAsString;
				}
			}

			// check if heat- and coolload are covered
			this.CheckLoadsCovered(row);

			// check if planned area exceeds maximum area
			if (product == null || product.QuickDimensioningPlannedArea <= product.QuickDimensioningMaximumArea) {
				row.Cells[colProductArea.Index].ErrorText = null;
			} else {
				row.Cells[colProductArea.Index].ErrorText = EuroplanRes.QuickDimensioningFloorGridFlaecheFehler; //"Die geplante Fläche ist größer als die maximal verfügbare Fläche";
			}
		}

		private void ValidateProductCircuits<P>(DataGridViewRow row) where P : Product {
			DataGridViewColumn colProductArea = this.GetProductAreaColumn<P>();
			DataGridViewColumn colProductCircuits = this.GetProductCircuitsColumn<P>();
			Room room = row.DataBoundItem as Room;
			P product = room.GetProductForQuickDimensioning<P>();
			object o = row.Cells[colProductArea.Index].Value;
			if (product == null && (o != null && (decimal)o != 0)) {
				log.Warn("Product for validated cell is null");
				product = (P)Project.Instance.Config.GetProduct<P>().Clone(room);
				product.QuickDimensioningPlannedArea = product.GetDefaultQuickDimensioningPlannedArea();
				row.Cells[colProductArea.Index].Value = (decimal)product.QuickDimensioningPlannedArea;
			}
			if (product != null) {
				string circuits = row.Cells[colProductCircuits.Index].Value as string;
				product.QuickDimensioningCircuitsAsString = circuits;
			}
		}

		private void CheckLoadsCovered(DataGridViewRow row) {
			Room room = row.DataBoundItem as Room;
			if (room.QuickDimensioningHeatLoadCovered && (room.QuickDimensioningCoolLoadCovered || !this.Cooling)) {
				row.ErrorText = null;
			} else {
				if (!room.QuickDimensioningHeatLoadCovered && (!room.QuickDimensioningCoolLoadCovered && this.Cooling)) {
					row.ErrorText = EuroplanRes.QuickDimensioningFloorGrid_HeizUndKuehllastFehler; //"Heiz- und Kühllast nicht abgedeckt";
				} else if (!room.QuickDimensioningCoolLoadCovered && this.Cooling) {
					row.ErrorText = EuroplanRes.QuickDimensioningFloorGrid_HeizlastFehler; //"Kühllast nicht abgedeckt";
				} else {
					row.ErrorText = EuroplanRes.QuickDimensioningFloorGrid_KuehllastFehler; //"Heizlast nicht abgedeckt";
				}
			}
		}

		private void quickDimensioningGrid_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e) {
			for (int i = e.RowIndex; i < e.RowIndex + e.RowCount; i++) {
				DataGridViewRow row = this.quickDimensioningGrid.Rows[i];
				if (row.DataBoundItem is Room) {
					// Euroval
					this.ShowProduct<EurovalProduct>(row);

					// Concrete Activation
					this.ShowProduct<ConcreteActivationProduct>(row);

					// Hitherm
					this.ShowProduct<HithermProduct>(row);

					// Hitherm Comact
					this.ShowProduct<HithermCompactProduct>(row);
					
					// Hitherm Comact
					this.ShowProduct<HithermCompactRoofProduct>(row);

					// Module Klimaboden
					this.ShowProduct<ModulKlimaBodenProduct>(row);

					// Module Klimadecke
					this.ShowProduct<ModulKlimaDeckeProduct>(row);

					this.CheckLoadsCovered(row);
				}
			}
		}

		private void ShowProduct<P>(DataGridViewRow row) where P : Product {
			DataGridViewColumn colProductArea = this.GetProductAreaColumn<P>();
			DataGridViewColumn colProductCircuits = this.GetProductCircuitsColumn<P>();
			Room room = row.DataBoundItem as Room;
			P product = room.GetProductForQuickDimensioning<P>();
			if (colProductArea.Visible && product != null) {
				row.Cells[colProductArea.Index].Value = (decimal)product.QuickDimensioningPlannedArea;
				row.Cells[colProductCircuits.Index].Value = product.QuickDimensioningCircuitsAsString;
			}
		}

		private void quickDimensioningGrid_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e) {
			Room room = this.quickDimensioningGrid.Rows[e.RowIndex].DataBoundItem as Room;
			if (room != null) {
				if (e.ColumnIndex == this.colHeatLoad.Index && room.QuickDimensioningHeatLoad == 0) {
					// get and set default heatload (if heatload is already set use it, if it is not set get default heatload for quickdimensioning)
					int heatload = room.NormalizedHeatLoad;
					if (heatload <= 0) {
						heatload = room.GetDefaultQuickDimensioningHeatLoad();
					}
					this.quickDimensioningGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = heatload;
				} else if (e.ColumnIndex == this.colCoolLoad.Index && room.QuickDimensioningCoolLoad == 0) {
					// get and set default coolload (if coolload is already set use it, if it is not set get default coolload for quickdimensioning)
					int coolload = room.NormalizedCoolLoad;
					if (coolload <= 0) {
						coolload = room.GetDefaultQuickDimensioningCoolLoad();
					}
					this.quickDimensioningGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = coolload;
				} else if ((e.ColumnIndex == this.colEuroval.Index || e.ColumnIndex == this.colEurovalCircuits.Index) && room.GetProductForQuickDimensioning<EurovalProduct>() == null) {
					// Euroval
					this.AddProduct<EurovalProduct>(this.quickDimensioningGrid.Rows[e.RowIndex]);
				} else if ((e.ColumnIndex == this.colConcreteActivation.Index || e.ColumnIndex == this.colConcreteActivationCircuits.Index) && room.GetProductForQuickDimensioning<ConcreteActivationProduct>() == null) {
					// Concrete Activation
					this.AddProduct<ConcreteActivationProduct>(this.quickDimensioningGrid.Rows[e.RowIndex]);
				} else if ((e.ColumnIndex == this.colHitherm.Index || e.ColumnIndex == this.colHithermCircuits.Index) && room.GetProductForQuickDimensioning<HithermProduct>() == null) {
					// Hitherm
					this.AddProduct<HithermProduct>(this.quickDimensioningGrid.Rows[e.RowIndex]);
				} else if ((e.ColumnIndex == this.colHithermCompact.Index || e.ColumnIndex == this.colHithermCompactCircuits.Index) && room.GetProductForQuickDimensioning<HithermCompactProduct>() == null) {
					// Hitherm Compact
					this.AddProduct<HithermCompactProduct>(this.quickDimensioningGrid.Rows[e.RowIndex]);
				} else if ((e.ColumnIndex == this.colHithermCompactRoof.Index || e.ColumnIndex == this.colHithermCompactRoofCircuits.Index) && room.GetProductForQuickDimensioning<HithermCompactRoofProduct>() == null) {
					// Hitherm Compact Roof
					this.AddProduct<HithermCompactRoofProduct>(this.quickDimensioningGrid.Rows[e.RowIndex]);
				} else if ((e.ColumnIndex == this.colModulKlimaBoden.Index || e.ColumnIndex == this.colModulKlimaBodenCircuits.Index) && room.GetProductForQuickDimensioning<ModulKlimaBodenProduct>() == null) {
					// Modul Klimadecke
					this.AddProduct<ModulKlimaBodenProduct>(this.quickDimensioningGrid.Rows[e.RowIndex]);
				} else if ((e.ColumnIndex == this.colModulKlimaDecke.Index || e.ColumnIndex == this.colModulKlimaDeckeCircuits.Index) && room.GetProductForQuickDimensioning<ModulKlimaDeckeProduct>() == null) {
					// Modul Klimaboden
					this.AddProduct<ModulKlimaDeckeProduct>(this.quickDimensioningGrid.Rows[e.RowIndex]);
				} else if (e.ColumnIndex == this.colNrOfServos.Index) {
					// TODO
				}
			}
		}

		private void AddProduct<P>(DataGridViewRow row) where P : Product {
			DataGridViewColumn colProductArea = this.GetProductAreaColumn<P>();
			DataGridViewColumn colProductCircuits = this.GetProductCircuitsColumn<P>();
			Room room = row.DataBoundItem as Room;
			// get new Product
			P product = (P)Project.Instance.Config.GetProduct<P>().Clone(room);

			// get and set default area
			product.QuickDimensioningPlannedArea = product.GetDefaultQuickDimensioningPlannedArea();
			row.Cells[colProductArea.Index].Value = (decimal)product.QuickDimensioningPlannedArea;

			// get and set default circuits
			product.QuickDimensioningCircuits = product.GetDefaultQuickDimensioningCircuits();
			row.Cells[colProductCircuits.Index].Value = product.QuickDimensioningCircuitsAsString;
		}

		private Dictionary<Type, DataGridViewColumn> productAreaColumns = null;
		private Dictionary<Type, DataGridViewColumn> productCircuitsColumns = null;

		private DataGridViewColumn GetProductAreaColumn<P>() where P : Product {
			if (this.productAreaColumns == null) {
				// fill dictionary
				this.productAreaColumns = new Dictionary<Type, DataGridViewColumn>();
				this.productAreaColumns.Add(typeof(EurovalProduct), this.colEuroval);
				this.productAreaColumns.Add(typeof(ConcreteActivationProduct), this.colConcreteActivation);
				this.productAreaColumns.Add(typeof(HithermProduct), this.colHitherm);
				this.productAreaColumns.Add(typeof(HithermCompactProduct), this.colHithermCompact);
				this.productAreaColumns.Add(typeof(HithermCompactRoofProduct), this.colHithermCompactRoof);
				this.productAreaColumns.Add(typeof(ModulKlimaBodenProduct), this.colModulKlimaBoden);
				this.productAreaColumns.Add(typeof(ModulKlimaDeckeProduct), this.colModulKlimaDecke);
			}

			if (this.productAreaColumns.ContainsKey(typeof(P))) {
				return this.productAreaColumns[typeof(P)];
			} else {
				log.Warn("unknown product");
				return null;
			}
		}

		private DataGridViewColumn GetProductCircuitsColumn<P>() where P : Product {
			if (this.productCircuitsColumns == null) {
				// fill dictionary
				this.productCircuitsColumns = new Dictionary<Type, DataGridViewColumn>();
				this.productCircuitsColumns.Add(typeof(EurovalProduct), this.colEurovalCircuits);
				this.productCircuitsColumns.Add(typeof(ConcreteActivationProduct), this.colConcreteActivationCircuits);
				this.productCircuitsColumns.Add(typeof(HithermProduct), this.colHithermCircuits);
				this.productCircuitsColumns.Add(typeof(HithermCompactProduct), this.colHithermCompactCircuits);
				this.productCircuitsColumns.Add(typeof(HithermCompactRoofProduct), this.colHithermCompactRoofCircuits);
				this.productCircuitsColumns.Add(typeof(ModulKlimaBodenProduct), this.colModulKlimaBodenCircuits);
				this.productCircuitsColumns.Add(typeof(ModulKlimaDeckeProduct), this.colModulKlimaDeckeCircuits);
			}

			if (this.productCircuitsColumns.ContainsKey(typeof(P))) {
				return this.productCircuitsColumns[typeof(P)];
			} else {
				log.Warn("unknown product");
				return null;
			}
		}

		private void quickDimensioningGrid_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e) {
			if (e.Control is DataGridViewComboBoxEditingControl) {
				DataGridViewComboBoxEditingControl combo = (e.Control as DataGridViewComboBoxEditingControl);
				if (combo.Items.Count > 0 && combo.Items[0] is RoomTypeItem) {
					combo.SelectedValueChanged -= new EventHandler(combo_SelectedValueChanged);
					combo.SelectedValueChanged += new EventHandler(combo_SelectedValueChanged);
				}
			}
		}

		private void combo_SelectedValueChanged(object sender, EventArgs e) {
			DataGridViewComboBoxEditingControl combo = (sender as DataGridViewComboBoxEditingControl);
			RoomTypeItem selectedRoomType = combo.SelectedItem as RoomTypeItem;
			if (selectedRoomType != null) {
				if (selectedRoomType.Value == this.newRoomType) {
					NewRoomTypeForm form = new NewRoomTypeForm(Project.Instance.Config);
					form.SelectedRoomType = (this.quickDimensioningGrid.Rows[(sender as DataGridViewComboBoxEditingControl).EditingControlRowIndex].DataBoundItem as Room).QuickDimensioningRoomType;
					form.ShowDialog();
					(sender as DataGridViewComboBoxEditingControl).SelectedValueChanged -= new EventHandler(combo_SelectedValueChanged);

					// update items in current combobox
					combo.Items.Clear();
					RoomTypeItem selectedItem = null;
					foreach (RoomType roomType in Project.Instance.Config.RoomTypes) {
						RoomTypeItem newItem = new RoomTypeItem(roomType);
						combo.Items.Add(newItem);
						if (roomType == form.SelectedRoomType) {
							selectedItem = newItem;
						}
					}
					this.colRoomType.Items.Add(new RoomTypeItem(this.newRoomType));

					// select new room type
					if (selectedItem != null) {
						combo.SelectedItem = selectedItem;
						this.quickDimensioningGrid.EndEdit();
					} else {
						this.quickDimensioningGrid.CancelEdit();
					}

					// update items in column
					this.colRoomType.Items.Clear();
					foreach (RoomType roomType in Project.Instance.Config.RoomTypes) {
						this.colRoomType.Items.Add(new RoomTypeItem(roomType));
					}
					this.colRoomType.Items.Add(new RoomTypeItem(this.newRoomType));
					this.OnProjectChanged();
				}
			}
		}

		private void quickDimensioningGrid_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e) {
			if (e.KeyCode == Keys.Delete && this.quickDimensioningGrid.SelectedCells.Count == 1 &&
					this.quickDimensioningGrid.SelectedRows.Count == 0 && this.quickDimensioningGrid.SelectedCells[0].Value != null) {
				DataGridViewCell cell = this.quickDimensioningGrid.SelectedCells[0];
				DataGridViewColumn col = cell.OwningColumn;
				DataGridViewRow row = cell.OwningRow;
				if (col == this.colRoomTemperature || col == this.colHeatLoad || col == this.colCoolLoad ||
						col == this.colRoomController || col == this.colComments || col == this.colNrOfServos ||
						col == this.colEuroval || col == this.colEurovalCircuits ||
						col == this.colConcreteActivation || col == this.colConcreteActivationCircuits ||
						col == this.colHitherm || col == this.colHithermCircuits ||
						col == this.colHithermCompact || col == this.colHithermCompactCircuits ||
						col == this.colHithermCompactRoof || col == this.colHithermCompactRoofCircuits ||
						col == this.colModulKlimaBoden || col == this.colModulKlimaBodenCircuits ||
						col == this.colModulKlimaDecke || col == this.colModulKlimaDeckeCircuits) {
					e.IsInputKey = false;
					this.quickDimensioningGrid.BeginEdit(true);
					cell.Value = null;
					this.quickDimensioningGrid.EndEdit();
					if (col == this.colEuroval) {
						this.ValidateProductArea<EurovalProduct>(row);
					} else if (col == this.colConcreteActivation) {
						this.ValidateProductArea<ConcreteActivationProduct>(row);
					} else if (col == this.colHitherm) {
						this.ValidateProductArea<HithermProduct>(row);
					} else if (col == this.colHithermCompact) {
						this.ValidateProductArea<HithermCompactProduct>(row);
					} else if (col == this.colHithermCompactRoof) {
						this.ValidateProductArea<HithermCompactRoofProduct>(row);
					} else if (col == this.colModulKlimaBoden) {
						this.ValidateProductArea<ModulKlimaBodenProduct>(row);
					} else if (col == this.colModulKlimaDecke) {
						this.ValidateProductArea<ModulKlimaDeckeProduct>(row);
					} else if (col == this.colNrOfServos) {
						this.quickDimensioningGrid.BeginEdit(true);
						cell.Value = -1;
						this.quickDimensioningGrid.EndEdit();
					}
					this.CheckLoadsCovered(row);
					this.OnProjectChanged();
				}
			}
		}

		private void OnProjectChanged() {
			if (this.projectChanged != null) {
				this.projectChanged(this);
			}
		}

		private void quickDimensioningGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
			this.OnProjectChanged();
		}

		private void quickDimensioningGrid_CellClick(object sender, DataGridViewCellEventArgs e) {
			if (e.ColumnIndex == this.colRevert.Index) {
				DataGridViewRow row = this.quickDimensioningGrid.Rows[e.RowIndex];
				Room room = row.DataBoundItem as Room;
				if (room != null) {
					room.RevertQuickDimensioning();

					// reset product values;
					row.Cells[this.colEuroval.Index].Value = null;
					row.Cells[this.colEurovalCircuits.Index].Value = null;
					row.Cells[this.colConcreteActivation.Index].Value = null;
					row.Cells[this.colConcreteActivationCircuits.Index].Value = null;
					row.Cells[this.colHitherm.Index].Value = null;
					row.Cells[this.colHithermCircuits.Index].Value = null;
					row.Cells[this.colHithermCompact.Index].Value = null;
					row.Cells[this.colHithermCompactCircuits.Index].Value = null;
					row.Cells[this.colHithermCompactRoof.Index].Value = null;
					row.Cells[this.colHithermCompactRoofCircuits.Index].Value = null;
					row.Cells[this.colModulKlimaBoden.Index].Value = null;
					row.Cells[this.colModulKlimaBodenCircuits.Index].Value = null;
					row.Cells[this.colModulKlimaDecke.Index].Value = null;
					row.Cells[this.colModulKlimaDeckeCircuits.Index].Value = null;

					row.Cells[this.colEuroval.Index].ErrorText = null;
					row.Cells[this.colConcreteActivation.Index].ErrorText = null;
					row.Cells[this.colHitherm.Index].ErrorText = null;
					row.Cells[this.colHithermCompact.Index].ErrorText = null;
					row.Cells[this.colHithermCompactRoof.Index].ErrorText = null;
					row.Cells[this.colModulKlimaBoden.Index].ErrorText = null;
					row.Cells[this.colModulKlimaDecke.Index].ErrorText = null;

					this.CheckLoadsCovered(row);

					this.quickDimensioningGrid.Invalidate();
				}
			}
		}
	}
}
