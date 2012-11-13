using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Europlan.Licensing;

namespace Europlan.AdminApplication {
	public class HardwareIdCell : DataGridViewTextBoxCell {
		public override Type EditType {
			get { return typeof(HardwareIdEditingControl); }
		}

		public override Type ValueType {
			get { return typeof(HardwareId); }
		}

		public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle) {
			base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
			HardwareIdEditingControl ctl = (HardwareIdEditingControl)DataGridView.EditingControl;

			ctl.Value = new HardwareId((string)this.Value);
		}

		public override object DefaultNewRowValue {
			get { 
				return null;
			}
		}
	}
}
