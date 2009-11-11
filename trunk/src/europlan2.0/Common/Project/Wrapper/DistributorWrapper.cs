using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {

	public class DistributorWrapper {

		private string heatOrCool;
		private string id;
		private string name;
		private int groups;
		private string regulatorCircuit;
		private int vorlaufTemp;
		private int ruecklaufTemp;
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

		public int Groups {
			get { return groups; }
			set { groups = value; }
		}

		public string RegulatorCircuit {
			get { return regulatorCircuit; }
			set { regulatorCircuit = value; }
		}

		public int VorlaufTemp {
			get { return vorlaufTemp; }
			set { vorlaufTemp = value; }
		}

		public int RuecklaufTemp {
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
