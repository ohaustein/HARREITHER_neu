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
		private RoomType newRoomType;

		private static ILog log = LogManager.GetLogger(typeof(QuickDimensioningFloorGrid));

		public QuickDimensioningFloorGrid() {
			InitializeComponent();
			this.colRoomType.ValueMember = "Value";
			this.colRoomType.DisplayMember = "Name";
			foreach (RoomType roomType in Project.Instance.Config.RoomTypes) {
				this.colRoomType.Items.Add(new RoomTypeItem(roomType));
			}
			this.newRoomType = new RoomType();
			this.newRoomType.Name = "<Neu>";
			this.newRoomType.Id = "<NEW>";
			this.newRoomType.UserDefined = true;
			this.colRoomType.Items.Add(new RoomTypeItem(this.newRoomType));
			this.colRoomController.ValueMember = "Controller";
			this.colRoomController.DisplayMember = "Name";
			this.colRoomController.Items.Add(new RoomControllerItem(Room.RoomController.None, ""));
			this.colRoomController.Items.Add(new RoomControllerItem(Room.RoomController.RC, "RC"));
			this.colRoomController.Items.Add(new RoomControllerItem(Room.RoomController.RCF, "RCF"));
			this.colRoomController.Items.Add(new RoomControllerItem(Room.RoomController.RCRadio, "RC-Funk"));
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
				foreach (DataGridViewRow row in this.dataGridView1.Rows) {
					row.Cells[colProductArea.Index].Value = null;
					row.Cells[colProductCircuits.Index].Value = null;
				}
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
				} else if (e.ColumnIndex == this.colHithermCompact.Index) {
					// Hitherm Compact
					this.ValidateProductArea<HithermCompactProduct>(row);
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
				this.dataGridView1.Invalidate();
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
				row.Cells[colProductArea.Index].ErrorText = "Die geplante Fläche ist größer als die maximal verfügbare Fläche";
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
				if (row.DataBoundItem is Room) {
					/*EurovalProduct euroval = room.GetProductForQuickDimensioning<EurovalProduct>();
					if (this.colEuroval.Visible && euroval != null) {
						row.Cells[this.colEuroval.Index].Value = (decimal)euroval.QuickDimensioningPlannedArea;
						row.Cells[this.colEurovalCircuits.Index].Value = euroval.QuickDimensioningCircuitsAsString;
					}*/
					// Euroval
					this.ShowProduct<EurovalProduct>(row);

					// Concrete Activation
					this.ShowProduct<ConcreteActivationProduct>(row);

					// Hitherm
					this.ShowProduct<HithermProduct>(row);

					// Hitherm Comact
					this.ShowProduct<HithermCompactProduct>(row);

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
					/*// get new Euroval Product
					Product product = Project.Instance.Config.EurovalProduct.Clone(room);

					// get and set default area
					product.QuickDimensioningPlannedArea = product.GetDefaultQuickDimensioningPlannedArea();
					this.dataGridView1.Rows[e.RowIndex].Cells[this.colEuroval.Index].Value = (decimal)product.QuickDimensioningPlannedArea;

					// get and set default circuits
					product.QuickDimensioningCircuits = product.GetDefaultQuickDimensioningCircuits();
					this.dataGridView1.Rows[e.RowIndex].Cells[this.colEurovalCircuits.Index].Value = product.QuickDimensioningCircuitsAsString;*/
					// Euroval
					this.AddProduct<EurovalProduct>(this.dataGridView1.Rows[e.RowIndex]);
				} else if ((e.ColumnIndex == this.colConcreteActivation.Index || e.ColumnIndex == this.colConcreteActivationCircuits.Index) && room.GetProductForQuickDimensioning<ConcreteActivationProduct>() == null) {
					// Concrete Activation
					this.AddProduct<ConcreteActivationProduct>(this.dataGridView1.Rows[e.RowIndex]);
				} else if ((e.ColumnIndex == this.colHitherm.Index || e.ColumnIndex == this.colHithermCircuits.Index) && room.GetProductForQuickDimensioning<HithermProduct>() == null) {
					// Hitherm
					this.AddProduct<HithermProduct>(this.dataGridView1.Rows[e.RowIndex]);
				} else if ((e.ColumnIndex == this.colHithermCompact.Index || e.ColumnIndex == this.colHithermCompactCircuits.Index) && room.GetProductForQuickDimensioning<HithermCompactProduct>() == null) {
					// Hitherm Compact
					this.AddProduct<HithermCompactProduct>(this.dataGridView1.Rows[e.RowIndex]);
				} else if ((e.ColumnIndex == this.colModulKlimaBoden.Index || e.ColumnIndex == this.colModulKlimaBodenCircuits.Index) && room.GetProductForQuickDimensioning<ModulKlimaBodenProduct>() == null) {
					// Modul Klimadecke
					this.AddProduct<ModulKlimaBodenProduct>(this.dataGridView1.Rows[e.RowIndex]);
				} else if ((e.ColumnIndex == this.colModulKlimaDecke.Index || e.ColumnIndex == this.colModulKlimaDeckeCircuits.Index) && room.GetProductForQuickDimensioning<ModulKlimaDeckeProduct>() == null) {
					// Modul Klimaboden
					this.AddProduct<ModulKlimaDeckeProduct>(this.dataGridView1.Rows[e.RowIndex]);
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

		private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e) {
			Console.WriteLine(e.Control);
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
				Console.WriteLine(selectedRoomType.Name);
				if (selectedRoomType.Value == this.newRoomType) {
					NewRoomTypeForm form = new NewRoomTypeForm(Project.Instance.Config);
					form.SelectedRoomType = (this.dataGridView1.Rows[(sender as DataGridViewComboBoxEditingControl).EditingControlRowIndex].DataBoundItem as Room).RoomType;
					form.ShowDialog();
					(sender as DataGridViewComboBoxEditingControl).SelectedValueChanged -= new EventHandler(combo_SelectedValueChanged);
					//combo.SelectedValue = form.SelectedRoomType;
					//this.dataGridView1.EndEdit();
					this.dataGridView1.CancelEdit();
					// TODO
				}
			}
		}
	}
}
