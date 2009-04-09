using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;

namespace Europlan.Application {
	public class NumericCell : DataGridViewTextBoxCell {

		private NumericBox.NumericEditType numEditType = NumericBox.NumericEditType.DEFAULT;

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
			ctl.BorderStyle = BorderStyle.None;
			ctl.Value = Convert.ToDecimal(initialFormattedValue);
			/*if (!this.lastKeyEnteredEditMode) {
				ctl.SelectionStart = ctl.Text.Length;
				ctl.SelectionLength = 0;
			}*/
		}

		public override object DefaultNewRowValue {
			get {
				return null;
			}
		}

		[DefaultValue(NumericBox.NumericEditType.DEFAULT)]
		public NumericBox.NumericEditType NumEditType {
			get { return this.numEditType; }
			set {
				this.numEditType = value;
				OnCommonChange();
			}
		}

		internal void SetNumEditType(int rowIndex, NumericBox.NumericEditType numEditType) {
			this.numEditType = numEditType;
			if (OwnsEditingControl(rowIndex)) {
				this.EditingNumericBox.EditType = numEditType;
			}
		}

		private bool OwnsEditingControl(int rowIndex) {
			if (rowIndex == -1 || this.DataGridView == null) {
				return false;
			}
			NumericEditingControl editingControl = this.DataGridView.EditingControl as NumericEditingControl;
			return editingControl != null && rowIndex == editingControl.EditingControlRowIndex;
		}

		private NumericEditingControl EditingNumericBox {
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
			get { return "F" + NumericBox.DecimalPlaces(this.NumEditType); }
		}

		public override System.Drawing.Rectangle PositionEditingPanel(System.Drawing.Rectangle cellBounds, System.Drawing.Rectangle cellClip, DataGridViewCellStyle cellStyle, bool singleVerticalBorderAdded, bool singleHorizontalBorderAdded, bool isFirstDisplayedColumn, bool isFirstDisplayedRow) {
			int height = DataGridView.EditingControl.Height;
			Rectangle rect = base.PositionEditingPanel(cellBounds, cellClip, cellStyle, singleVerticalBorderAdded, singleHorizontalBorderAdded, isFirstDisplayedColumn, isFirstDisplayedRow);
			Console.WriteLine("height: " + height + "/" + rect.Height);
			rect = new Rectangle(rect.X , rect.Y + (rect.Height - height) / 2, rect.Width , height);
			return rect;
		}
	}
}
