using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Europlan.Licensing;

namespace Europlan.Application {
	public class NumericEditingControl : NumericBox, IDataGridViewEditingControl {
		#region IDataGridViewEditingControl Members

		private DataGridView dataGridView;
		private int rowIndex;
		private bool valueChanged = false;

		public NumericEditingControl() : base() {
			this.TabStop = false;
			this.TextAlign = HorizontalAlignment.Right;
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
			get { return this.Value.ToString(); }
			set {
				string newValue = value as string;
				if (newValue != null) {
					this.Value = Decimal.Parse(newValue);
				/*} else {
					this.Value = value as HardwareId;*/
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
				case Keys.Escape:
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

		public void PrepareEditingControlForEdit(bool selectAll) {
			if (selectAll) {
				this.SelectionStart = 0;
				this.SelectionLength = this.Text.Length;
			} else {
				this.SelectionStart = this.Text.Length;
				this.SelectionLength = 0;
			}
		}

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
