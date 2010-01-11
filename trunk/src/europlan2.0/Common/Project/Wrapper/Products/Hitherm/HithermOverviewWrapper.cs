using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {

	public class HithermOverviewWrapper {

		private string text;
		private double amount;
		private string unit;

		public string Text {
			get { return text; }
			set { text = value; }
		}
		
		public double Amount {
			get { return amount; }
			set { amount = value; }
		}

		public string Unit {
			get { return unit; }
			set { unit = value; }
		}

	}

}
