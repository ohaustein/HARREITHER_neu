using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using log4net;

namespace Europlan.Common {
	public partial class QuickDimensioningDistributorGrid : UserControl {

		private QuickDimensioningDistributor distributor = null;

		private Dictionary<Type, DataGridViewColumn> productOpenColumns;
		private Dictionary<Type, DataGridViewColumn> productPlannedColumns;

		private static ILog log = LogManager.GetLogger(typeof(QuickDimensioningDistributorGrid));

		public QuickDimensioningDistributorGrid() {
			InitializeComponent();
		}

		public QuickDimensioningDistributor Distributor {
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

		public bool ModulKlimaBoden {
			get { return this.IsProductVisible<ModulKlimaBodenProduct>(); }
			set { this.SetProductVisible<ModulKlimaBodenProduct>(value); }
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
				openCell.Style.BackColor = (planned ? openCell.InheritedStyle.BackColor : SystemColors.Control);
				plannedCell.ReadOnly = !planned;
				plannedCell.Style.BackColor = (planned ? plannedCell.InheritedStyle.BackColor : SystemColors.Control);
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
				this.EnableProductInRoom<ConcreteActivationProduct>(row);
				this.EnableProductInRoom<HithermProduct>(row);
				this.EnableProductInRoom<HithermCompactProduct>(row);
				this.EnableProductInRoom<ModulKlimaBodenProduct>(row);
				this.EnableProductInRoom<ModulKlimaDeckeProduct>(row);
			}
		}

		private DataGridViewColumn GetProductOpenColumn<P>() where P : Product {
			if (this.productOpenColumns == null) {
				// fill dictionary
				this.productOpenColumns = new Dictionary<Type, DataGridViewColumn>();
				this.productOpenColumns.Add(typeof(EurovalProduct), this.colEurovalOpenCircuits);
				this.productOpenColumns.Add(typeof(ConcreteActivationProduct), this.colConcreteActivationOpenCircuits);
				this.productOpenColumns.Add(typeof(HithermProduct), this.colHithermOpenCircuits);
				this.productOpenColumns.Add(typeof(HithermCompactProduct), this.colHithermCompactOpenCircuits);
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
				this.productPlannedColumns.Add(typeof(ConcreteActivationProduct), this.colConcreteActivationPlannedCircuits);
				this.productPlannedColumns.Add(typeof(HithermProduct), this.colHithermPlannedCircuits);
				this.productPlannedColumns.Add(typeof(HithermCompactProduct), this.colHithermCompactPlannedCircuits);
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
	}
}
