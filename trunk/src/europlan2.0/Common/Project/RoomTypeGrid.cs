using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Europlan.Common {
	public partial class RoomTypeGrid : UserControl {

		private bool adminMode = false;

		public RoomTypeGrid() {
			InitializeComponent();
		}

		public Configuration Config {
			set {
				if (!(value.Type == Configuration.ConfigurationType.AdminConfiguration || value.Type == Configuration.ConfigurationType.ProjectConfiguration)) {
					throw new Exception("Type must either be AdminConfiguration or ProjectConfiguration");
				}
				this.adminMode = value.Type == Configuration.ConfigurationType.AdminConfiguration;
				this.roomTypeBindingSource.DataSource = value.RoomTypes;
				this.roomTypeBindingSource.AddingNew += new AddingNewEventHandler(roomTypeBindingSource_AddingNew);
				this.roomTypeBindingSource.ResetBindings(false);
			}
		}

		void roomTypeBindingSource_AddingNew(object sender, AddingNewEventArgs e) {
			RoomType type = new RoomType();
			type.UserDefined = !adminMode;
			e.NewObject = type;
		}

		private void gridRoomTypes_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e) {
			if (!adminMode) {
				for (int i = e.RowIndex; i < e.RowIndex + e.RowCount; i++) {
					DataGridViewRow row = this.gridRoomTypes.Rows[i];
					if (row.DataBoundItem != null) {
						row.ReadOnly = !(row.DataBoundItem as RoomType).UserDefined;
						if (row.ReadOnly) {
							row.DefaultCellStyle.ForeColor = SystemColors.GrayText;
						} else {
							row.DefaultCellStyle.ForeColor = SystemColors.ControlText;
						}
					}
				}
			}
		}

		private void gridRoomTypes_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e) {
			if (!adminMode) {
				if (e.Row.DataBoundItem is RoomType && !(e.Row.DataBoundItem as RoomType).UserDefined) {
					e.Cancel = true;
				}
			}
		}


		public void Cleanup() {
			if (this.gridRoomTypes.SelectedCells.Count > 0) {
				if (this.gridRoomTypes.SelectedCells[0].OwningRow.DataBoundItem == null) {
					this.gridRoomTypes.CancelEdit();
				} else {
					this.gridRoomTypes.EndEdit();
				}
			}
		}

	}
}
