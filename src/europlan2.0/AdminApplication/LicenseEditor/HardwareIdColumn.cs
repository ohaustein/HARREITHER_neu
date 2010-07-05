using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Europlan.AdminApplication {
	public class HardwareIdColumn : DataGridViewColumn {

		public HardwareIdColumn() {
			this.CellTemplate = new HardwareIdCell();
		}
	}
}
