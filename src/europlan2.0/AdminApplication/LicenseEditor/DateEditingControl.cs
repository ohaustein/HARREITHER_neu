using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Europlan.Licensing;

namespace Europlan.AdminApplication {
	public class DateEditingControl : DateTimePicker, IDataGridViewEditingControl {
		#region IDataGridViewEditingControl Members

		private DataGridView dataGridView;
		private int rowIndex;
		private bool valueChanged = false;

		public DateEditingControl() {
			this.Format = DateTimePickerFormat.Short;
		}

		public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle) {
			this.Font = dataGridViewCellStyle.Font;
			this.ForeColor = dataGridViewCellStyle.ForeColor;
			this.BackColor = dataGridViewCellStyle.BackColor;
		}

		public DataGridView EditingControlDataGridView {
			get { return this.dataGridView; }
			set { this.dataGridView = value; }
		}

		public object EditingControlFormattedValue {
			get { return this.Value.ToShortDateString(); }
			set {
				string newValue = value as string;
				if (newValue != null) {
					this.Value = DateTime.Parse(newValue);
				}
			}
		}

		public int EditingControlRowIndex {
			get { return this.rowIndex; }
			set { this.rowIndex = value; }
		}

		public bool EditingControlValueChanged {
			get { return this.valueChanged; }
			set { this.valueChanged = value; }
		}

		public bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey) {
			switch (keyData & Keys.KeyCode) {
				case Keys.Tab:
				case Keys.Up:
				case Keys.Down:
				case Keys.Enter:
					return false;
				default:
					return true;
			}
		}

		public Cursor EditingPanelCursor {
			get { return base.Cursor; }
		}

		public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context) {
			return EditingControlFormattedValue;
		}

		public void PrepareEditingControlForEdit(bool selectAll) { }

		public bool RepositionEditingControlOnValueChange {
			get { return false; }
		}

		protected override void OnValueChanged(EventArgs args) {
			this.valueChanged = true;
			this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
			base.OnValueChanged(args);
		}
		#endregion
	}
}
