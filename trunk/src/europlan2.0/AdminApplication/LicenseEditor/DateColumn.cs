using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Europlan.AdminApplication {
	public class DateColumn : DataGridViewColumn {

		public DateColumn() {
			this.CellTemplate = new DateCell();
		}
	}
}
