using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace Europlan.Application {
	public class NumericColumn : DataGridViewColumn {

		//private NumericCell cellTemplate;

		public NumericColumn() {
			//this.cellTemplate = new NumericCell();
			NumericCell cell = new NumericCell();
			base.CellTemplate = cell;
			base.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
			base.DefaultCellStyle.Format = this.NumericCellTemplate.FormatString;
		}

		[Category("Appearance"), DefaultValue(NumericBox.NumericEditType.DEFAULT), Description("The type of the cell")]
		public NumericBox.NumericEditType NumEditType {
			get { return this.NumericCellTemplate.NumEditType; }
			set {
				this.NumericCellTemplate.NumEditType = value;
				base.DefaultCellStyle.Format = this.NumericCellTemplate.FormatString;
				if (this.DataGridView != null) {
					// Update all existing cells in the column
					DataGridViewRowCollection rows = this.DataGridView.Rows;
					int rowCount = rows.Count;
					for (int i = 0; i < rowCount; i++) {
						DataGridViewRow row = rows.SharedRow(i);
						NumericCell cell = row.Cells[this.Index] as NumericCell;
						if (cell != null) {
							cell.SetNumEditType(i, value);
						}
					}
				}
			}
		}

		private NumericCell NumericCellTemplate {
			get {
				NumericCell cell = this.CellTemplate as NumericCell;
				if (cell == null) {
					throw new InvalidOperationException("NumericColumn does not have a CellTemplate");
				}
				return cell;
			}
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override DataGridViewCell CellTemplate {
			get { return base.CellTemplate; }
			set {
				NumericCell cell = value as NumericCell;
				if (value != null && cell == null) {
					throw new InvalidOperationException("Value provided for CellTemplate must be of type NumericCell or derive from it.");
				}
				base.CellTemplate = value;
			}
		}

		public override DataGridViewCellStyle DefaultCellStyle {
			get {
				return base.DefaultCellStyle;
			}
			set {
				value.Format = this.NumericCellTemplate.FormatString;
				base.DefaultCellStyle = value;
			}
		}
	}
}
