using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Europlan.Common {
	public class HithermCompactWall {
		private string id;
		private string name;
		private WallConstruction construction;
		private Nullable<double> deckschicht;
		private Nullable<double> uValue;
		private bool bereinigen;
		private Nullable<double> additionalInsulation;
		private double tempBehindHeat = -16;
		private double tempBehindCool = 30;
		private bool defaultWall = false;

		public HithermCompactWall() {
		}

		public HithermCompactWall(string id, string name, WallConstruction construction, Nullable<double> deckschicht, Nullable<double> uValue, bool bereinigen, Nullable<double> additionalInsulation, double tempBehindHeat, double tempBehindCool, bool defaultWall) {
			this.id = id;
			this.name = name;
			this.construction = construction;
			this.deckschicht = deckschicht;
			this.uValue = uValue;
			this.bereinigen = bereinigen;
			this.additionalInsulation = additionalInsulation;
			this.tempBehindHeat = tempBehindHeat;
			this.tempBehindCool = tempBehindCool;
			this.defaultWall = defaultWall;
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

		[XmlIgnore]
		public string ConstructionName {
			get {
				if (defaultWall || construction == null) {
					return null;
				}
				return construction.Id;
			}
		}

		public Nullable<double> Deckschicht {
			get { return deckschicht; }
			set { deckschicht = value; }
		}

		[XmlIgnore]
		public double DeckschichtValue {
			get { return deckschicht.HasValue ? deckschicht.Value : 0; }
		}

		public Nullable<double> UValue {
			get { return uValue; }
			set { uValue = value; }
		}

		[XmlIgnore]
		public double UValueValue {
			get { return uValue.HasValue ? uValue.Value : 0; }
		}

		public bool Bereinigen {
			get { return bereinigen; }
			set { bereinigen = value; }
		}

		public Nullable<double> AdditionalInsulation {
			get { return additionalInsulation; }
			set { additionalInsulation = value; }
		}

		[XmlIgnore]
		public double AdditionalInsulationValue {
			get { return additionalInsulation.HasValue ? additionalInsulation.Value : 0; }
		}

		public double TempBehindHeat {
			get { return tempBehindHeat; }
			set { tempBehindHeat = value; }
		}

		public double TempBehindCool {
			get { return tempBehindCool; }
			set { tempBehindCool = value; }
		}

		[XmlIgnore]
		public bool DefaultWall {
			get { return this.defaultWall; }
			set { this.defaultWall = value; }
		}

		public override string ToString() {
			return this.Id;
		}
	}
}
