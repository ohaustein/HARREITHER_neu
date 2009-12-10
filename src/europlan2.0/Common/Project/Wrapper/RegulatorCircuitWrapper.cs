using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {

	public class RegulatorCircuitWrapper {

		private string heatOrCool;
		private string id;
		private string name;
		private string medium;
		private double vorlaufTemp;
		private double ruecklaufTemp;
		private double durchfluss;
		private double druckverlust;
		private double inhalt;

		public string HeatOrCool {
			get { return heatOrCool; }
			set { heatOrCool = value; }
		}

		public string Id {
			get { return id; }
			set { id = value; }
		}

		public string Name {
			get { return name; }
			set { name = value; }
		}

		public string Medium {
			get { return medium; }
			set { medium = value; }
		}

		public double VorlaufTemp {
			get { return vorlaufTemp; }
			set { vorlaufTemp = value; }
		}

		public double RuecklaufTemp {
			get { return ruecklaufTemp; }
			set { ruecklaufTemp = value; }
		}

		public double Durchfluss {
			get { return durchfluss; }
			set { durchfluss = value; }
		}

		public double Druckverlust {
			get { return druckverlust; }
			set { druckverlust = value; }
		}

		public double Inhalt {
			get { return inhalt; }
			set { inhalt = value; }
		}

	}

}
