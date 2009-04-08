using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace Europlan.Application {
	public class NumericCell : DataGridViewTextBoxCell {

		private NumericEditBox.NumericEditType numEditType = NumericEditBox.NumericEditType.DEFAULT;

		public NumericCell() {
		}

		public override Type EditType {
			get { return typeof(NumericEditingControl); }
		}

		public override Type ValueType {
			get { return typeof(Decimal); }
		}

		public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle) {
			base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
			NumericEditingControl ctl = (NumericEditingControl)DataGridView.EditingControl;
			ctl.EditType = this.numEditType;
			ctl.ClientBorder = BorderStyle.None;
			/*Console.WriteLine("(" + this.DataGridView.Rows[rowIndex].Height + " - " + ctl.Height + ") / 2"); 
			ctl.Top = (this.DataGridView.Rows[rowIndex].Height - ctl.Height + 1) / 2;*/
			ctl.Value = Convert.ToDecimal(this.Value);
			ctl.SelectAll();
			//ctl.Value = new decimal((int)this.Value);
		}

		public override object DefaultNewRowValue {
			get {
				return null;
			}
		}

		[DefaultValue(NumericEditBox.NumericEditType.DEFAULT)]
		public NumericEditBox.NumericEditType NumEditType {
			get { return this.numEditType; }
			set {
				this.numEditType = value;
				OnCommonChange();
			}
		}

		internal void SetNumEditType(int rowIndex, NumericEditBox.NumericEditType numEditType) {
			this.numEditType = numEditType;
			if (OwnsEditingControl(rowIndex)) {
				this.EditingNumericEditBox.EditType = numEditType;
			}
		}

		private bool OwnsEditingControl(int rowIndex) {
			if (rowIndex == -1 || this.DataGridView == null) {
				return false;
			}
			NumericEditingControl editingControl = this.DataGridView.EditingControl as NumericEditingControl;
			return editingControl != null && rowIndex == editingControl.EditingControlRowIndex;
		}

		private NumericEditingControl EditingNumericEditBox {
			get { return this.DataGridView.EditingControl as NumericEditingControl; }
		}

		private void OnCommonChange() {
			if (this.DataGridView != null && !this.DataGridView.IsDisposed && !this.DataGridView.Disposing) {
				if (this.RowIndex == -1) {
					this.DataGridView.InvalidateColumn(this.ColumnIndex);
				} else {
					this.DataGridView.UpdateCellValue(this.ColumnIndex, this.RowIndex);
				}
			}
		}

		public override object Clone() {
			NumericCell cell = base.Clone() as NumericCell;
			if (cell != null) {
				cell.NumEditType = this.numEditType;
			}
			return cell;
		}

		public string FormatString {
			get { return "F" + NumericEditBox.DecimalPlaces(this.NumEditType); }
		}
	}
}
