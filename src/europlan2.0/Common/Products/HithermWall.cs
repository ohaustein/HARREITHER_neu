using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {
	public class HithermWall {
		private string id;
		private string name;
		private WallConstruction construction;
		private double deckschicht;
		private double kValue;
		private bool bereinigen;
		private double additionalInsulation;
		private double tempBehindHeat;
		private double tempBehindCool;

		public HithermWall() {
		}

		public string Id {
			get { return id; }
			set { id = value; }
		}

		public string Name {
			get { return name; }
			set { name = value; }
		}

		public WallConstruction Construction {
			get { return construction; }
			set { construction = value; }
		}

		public double Deckschicht {
			get { return deckschicht; }
			set { deckschicht = value; }
		}

		public double KValue {
			get { return kValue; }
			set { kValue = value; }
		}

		public bool Bereinigen {
			get { return bereinigen; }
			set { bereinigen = value; }
		}

		public double AdditionalInsulation {
			get { return additionalInsulation; }
			set { additionalInsulation = value; }
		}

		public double TempBehindHeat {
			get { return tempBehindHeat; }
			set { tempBehindHeat = value; }
		}

		public double TempBehindCool {
			get { return tempBehindCool; }
			set { tempBehindCool = value; }
		}
	}
}
