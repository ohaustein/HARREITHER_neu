using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using log4net;

namespace Europlan.Common {
	public partial class QuickDimensioningDistributorsGrid : UserControl {

        private event ProjectChangedHandler projectChanged;
        public event ProjectChangedHandler ProjectChanged {
            add { this.projectChanged += value; }
            remove { this.projectChanged -= value; }
        }

		private Distributor distributor = null;

		private Dictionary<Type, DataGridViewColumn> productOpenColumns;
		private Dictionary<Type, DataGridViewColumn> productPlannedColumns;

		private static ILog log = LogManager.GetLogger(typeof(QuickDimensioningDistributorsGrid));

		public QuickDimensioningDistributorsGrid() {
			InitializeComponent();

			this.SetLanguage();
		}

		private void SetLanguage() {
			this.colRoomId.HeaderText = EuroplanRes.QuickDimensioningDistributorsGrid_RaumNr; //"Id"
			this.colRoomName.HeaderText = EuroplanRes.QuickDimensioningDistributorsGrid_Raumname; // "Raumname"
			this.colFloorName.HeaderText = EuroplanRes.QuickDimensioningDistributorsGrid_Geschoss; //"Geschoß"
			this.colEurovalOpenCircuits.HeaderText = EuroplanRes.QuickDimensioningDistributorsGrid_EurovalOffen; //"Euroval®\noffene\nHeizkreise"
			this.colEurovalPlannedCircuits.HeaderText = EuroplanRes.QuickDimensioningDistributorsGrid_EurovalAngeschlossen; //"Euroval®\nangeschl.\nHeizkreise"
			this.colJumbovalOpenCircuits.HeaderText = EuroplanRes.QuickDimensioningDistributorsGrid_JumbovalOffen; //"Jumboval®\noffene\nHeizkreise"
			this.colJumbovalPlannedCircuits.HeaderText = EuroplanRes.QuickDimensioningDistributorsGrid_JumbovalAngeschlossen; //"Jumboval®\nangeschl.\nHeizkreise"
			this.colConcreteActivationOpenCircuits.HeaderText = EuroplanRes.QuickDimensioningDistributorsGrid_BkaOffen; //"BKA\noffene\nHeizkreise"
			this.colConcreteActivationPlannedCircuits.HeaderText = EuroplanRes.QuickDimensioningDistributorsGrid_BkaAngeschlossen; //"BKA\nangeschl.\nHeizkreise"
			this.colHithermOpenCircuits.HeaderText = EuroplanRes.QuickDimensioningDistributorsGrid_HithermOffen; //"Hitherm®\noffene\nHeizkreise"
			this.colHithermPlannedCircuits.HeaderText = EuroplanRes.QuickDimensioningDistributorsGrid_HithermAngschlossen; //"Hitherm®\nangeschl.\nHeizkreise"
			this.colHithermCompactOpenCircuits.HeaderText = EuroplanRes.QuickDimensioningDistributorsGrid_HithermCompactOffen; //"Hitherm® Co\noffene\nHeizkreise"
			this.colHithermCompactPlannedCircuits.HeaderText = EuroplanRes.QuickDimensioningDistributorsGrid_HithermCompactAngeschlossen; //"Hitherm® Co\nangeschl.\nHeizkreise"
			this.colHithermCompactRoofOpenCircuits.HeaderText = EuroplanRes.QuickDimensioningDistributorsGrid_HithermCompactDachOffen; //"Hitherm® Co\nDach offene\nHeizkreise"
			this.colHithermCompactRoofPlannedCircuits.HeaderText = EuroplanRes.QuickDimensioningDistributorsGrid_HithermCompactDachAngeschlossen; //"Hitherm® Co\nDach angeschl.\nHeizkreise"
			this.colModulKlimaBodenOpenCircuits.HeaderText = EuroplanRes.QuickDimensioningDistributorsGrid_KlimaBodenOffen; //"Klima-Boden\noffene\nHeizkreise"
			this.colModulKlimaBodenPlannedCircuits.HeaderText = EuroplanRes.QuickDimensioningDistributorsGrid_KlimaBodenAngschlossen; //"Klima-Boden\nangeschl.\nHeizkreise"
			this.colModulKlimaDeckeOpenCircuits.HeaderText = EuroplanRes.QuickDimensioningDistributorsGrid_KlimaDeckeOffen; //"Klima-Decke\noffene\nHeizkreise"
			this.colModulKlimaDeckePlannedCircuits.HeaderText = EuroplanRes.QuickDimensioningDistributorsGrid_KlimaDeckeAngeschlossen; //"Klima-Decke\nangeschl.\nHeizkreise"
		}

		private void OnProjectChanged() {
			if (this.projectChanged != null) {
				this.projectChanged(this);
			}
		}

		public Distributor Distributor {
			get { return this.distributor; }
			set {
				this.distributor = value;
				List<QuickDimensioningRoomDistributorsWrapper> roomWrappers = new List<QuickDimensioningRoomDistributorsWrapper>();
				if (this.distributor != null) {
					foreach (Floor floor in Project.Instance.Floors) {
						foreach (Room room in floor.Rooms) {
							roomWrappers.Add(new QuickDimensioningRoomDistributorsWrapper(room, floor, this.distributor));
						}
					}
				}
				this.dataSourceRooms.DataSource = roomWrappers;
				this.dataSourceRooms.ResetBindings(false);
			}
		}

		private bool IsProductVisible<P>() where P : Product {
			return this.GetProductOpenColumn<P>().Visible;
		}

		private void SetProductVisible<P>(bool visible) where P : Product {
			this.GetProductOpenColumn<P>().Visible = visible;
			this.GetProductPlannedColumn<P>().Visible = visible;
		}

		public bool Euroval {
			get { return this.IsProductVisible<EurovalProduct>(); }
			set { this.SetProductVisible<EurovalProduct>(value); }
		}

		public bool Jumboval {
			get { return this.IsProductVisible<JumbovalProduct>(); }
			set { this.SetProductVisible<JumbovalProduct>(value); }
		}

		public bool ConcreteActivation {
			get { return this.IsProductVisible<ConcreteActivationProduct>(); }
			set { this.SetProductVisible<ConcreteActivationProduct>(value); }
		}

		public bool Hitherm {
			get { return this.IsProductVisible<HithermProduct>(); }
			set { this.SetProductVisible<HithermProduct>(value); }
		}

		public bool HithermCompact {
			get { return this.IsProductVisible<HithermCompactProduct>(); }
			set { this.SetProductVisible<HithermCompactProduct>(value); }
		}

		public bool HithermCompactRoof {
			get { return this.IsProductVisible<HithermCompactRoofProduct>(); }
			set { this.SetProductVisible<HithermCompactRoofProduct>(value); }
		}

		public bool ModulKlimaBoden {
			get { return this.IsProductVisible<ModulKlimaBodenProduct>(); }
			set { this.SetProductVisible<ModulKlimaBodenProduct>(value); }
		}

        public bool ModulKlimaBoden20
        {
            get { return this.IsProductVisible<ModulKlimaBoden20Product>(); }
            set { this.SetProductVisible<ModulKlimaBoden20Product>(value); }
        }

		public bool ModulKlimaDecke {
			get { return this.IsProductVisible<ModulKlimaDeckeProduct>(); }
			set { this.SetProductVisible<ModulKlimaDeckeProduct>(value); }
		}

		private void EnableProductInRoom<P>(DataGridViewRow row) where P : Product {
			QuickDimensioningRoomDistributorsWrapper wrapper = row.DataBoundItem as QuickDimensioningRoomDistributorsWrapper;
			if (wrapper != null) {
				bool planned = wrapper.IsProductPlanned<P>();
				DataGridViewCell openCell = row.Cells[this.GetProductOpenColumn<P>().Index];
				DataGridViewCell plannedCell = row.Cells[this.GetProductPlannedColumn<P>().Index];
				openCell.Style.BackColor = (planned ? row.DefaultCellStyle.BackColor : SystemColors.Control);
				plannedCell.ReadOnly = !planned;
				plannedCell.Style.BackColor = (planned ? row.DefaultCellStyle.BackColor : SystemColors.Control);
				DataGridViewNumericUpDownCell plannedUpDownCell = plannedCell as DataGridViewNumericUpDownCell;
				if (plannedUpDownCell != null) {
					plannedUpDownCell.Maximum = wrapper.GetMaximumPlannableCircuits<P>();
				}
			}
		}

		private void gridRooms_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e) {
			for (int i = e.RowIndex; i < e.RowIndex + e.RowCount; i++) {
				DataGridViewRow row = this.gridRooms.Rows[i];
				this.EnableProductInRoom<EurovalProduct>(row);
				this.EnableProductInRoom<JumbovalProduct>(row);
				this.EnableProductInRoom<ConcreteActivationProduct>(row);
				this.EnableProductInRoom<HithermProduct>(row);
				this.EnableProductInRoom<HithermCompactProduct>(row);
				this.EnableProductInRoom<HithermCompactRoofProduct>(row);
				this.EnableProductInRoom<ModulKlimaBodenProduct>(row);
				this.EnableProductInRoom<ModulKlimaDeckeProduct>(row);
			}
		}

		private DataGridViewColumn GetProductOpenColumn<P>() where P : Product {
			if (this.productOpenColumns == null) {
				// fill dictionary
				this.productOpenColumns = new Dictionary<Type, DataGridViewColumn>();
				this.productOpenColumns.Add(typeof(EurovalProduct), this.colEurovalOpenCircuits);
				this.productOpenColumns.Add(typeof(JumbovalProduct), this.colJumbovalOpenCircuits);
				this.productOpenColumns.Add(typeof(ConcreteActivationProduct), this.colConcreteActivationOpenCircuits);
				this.productOpenColumns.Add(typeof(HithermProduct), this.colHithermOpenCircuits);
				this.productOpenColumns.Add(typeof(HithermCompactProduct), this.colHithermCompactOpenCircuits);
				this.productOpenColumns.Add(typeof(HithermCompactRoofProduct), this.colHithermCompactRoofOpenCircuits);
				this.productOpenColumns.Add(typeof(ModulKlimaBodenProduct), this.colModulKlimaBodenOpenCircuits);
				this.productOpenColumns.Add(typeof(ModulKlimaDeckeProduct), this.colModulKlimaDeckeOpenCircuits);
			}

			if (this.productOpenColumns.ContainsKey(typeof(P))) {
				return this.productOpenColumns[typeof(P)];
			} else {
				log.Warn("unknown product");
				return null;
			}
		}

		private DataGridViewColumn GetProductPlannedColumn<P>() where P : Product {
			if (this.productPlannedColumns == null) {
				// fill dictionary
				this.productPlannedColumns = new Dictionary<Type, DataGridViewColumn>();
				this.productPlannedColumns.Add(typeof(EurovalProduct), this.colEurovalPlannedCircuits);
				this.productPlannedColumns.Add(typeof(JumbovalProduct), this.colJumbovalPlannedCircuits);
				this.productPlannedColumns.Add(typeof(ConcreteActivationProduct), this.colConcreteActivationPlannedCircuits);
				this.productPlannedColumns.Add(typeof(HithermProduct), this.colHithermPlannedCircuits);
				this.productPlannedColumns.Add(typeof(HithermCompactProduct), this.colHithermCompactPlannedCircuits);
				this.productPlannedColumns.Add(typeof(HithermCompactRoofProduct), this.colHithermCompactRoofPlannedCircuits);
				this.productPlannedColumns.Add(typeof(ModulKlimaBodenProduct), this.colModulKlimaBodenPlannedCircuits);
				this.productPlannedColumns.Add(typeof(ModulKlimaDeckeProduct), this.colModulKlimaDeckePlannedCircuits);
			}

			if (this.productPlannedColumns.ContainsKey(typeof(P))) {
				return this.productPlannedColumns[typeof(P)];
			} else {
				log.Warn("unknown product");
				return null;
			}
		}

		private void gridRooms_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
			this.OnProjectChanged();
		}

		private void gridRooms_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e) {
			if (e.KeyCode == Keys.Delete && this.gridRooms.SelectedCells.Count == 1 &&
					this.gridRooms.SelectedRows.Count == 0 && this.gridRooms.SelectedCells[0].Value != null) {
				DataGridViewCell cell = this.gridRooms.SelectedCells[0];
				if (!cell.ReadOnly) {
					e.IsInputKey = false;
					this.gridRooms.BeginEdit(true);
					cell.Value = null;
					this.gridRooms.EndEdit();
				}
			}
		}

		private void gridRooms_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e) {
			DataGridViewNumericUpDownCell cell = this.gridRooms.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewNumericUpDownCell;
			if (cell != null && cell.Value == null) {
				int count = 0;
				if (this.distributor != null) {
					Project project = Project.Instance;
					if (project != null) {
						foreach (Floor floor in project.Floors) {
							foreach (Room room in floor.Rooms) {
								foreach (Product product in room.UsedProductsForQuickDimensioning) {
									if (product.QuickDimensioningConnectedDistributors.ContainsKey(distributor.Id)) {
										count += product.QuickDimensioningConnectedDistributors[distributor.Id];
									}
								}
							}
						}
					}
                    cell.Value = (count >= this.distributor.MaxCircuits ? 0 : (count + cell.Maximum > this.distributor.MaxCircuits ? this.distributor.MaxCircuits - count : (int)cell.Maximum));
				}
			}
		}
	}
}
