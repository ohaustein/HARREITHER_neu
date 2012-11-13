using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Europlan.Licensing;

namespace Europlan.AdminApplication {
	public class DateCell : DataGridViewTextBoxCell {
		public override Type EditType {
			get { return typeof(DateEditingControl); }
		}

		public override Type ValueType {
			get { return typeof(DateTime); }
		}

		public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle) {
			base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
			DateEditingControl ctl = (DateEditingControl)DataGridView.EditingControl;

			ctl.Value = (DateTime)this.Value;
		}

		public override object DefaultNewRowValue {
			get { 
				return null;
			}
		}

		public override Type FormattedValueType {
			get {
				return typeof(string);
			}
		}

		protected override object GetFormattedValue(object value, int rowIndex, ref DataGridViewCellStyle cellStyle, System.ComponentModel.TypeConverter valueTypeConverter, System.ComponentModel.TypeConverter formattedValueTypeConverter, DataGridViewDataErrorContexts context) {
			if (value == null) {
				return null;
			}
			return ((DateTime)value).ToShortDateString();
		}
	}
}
