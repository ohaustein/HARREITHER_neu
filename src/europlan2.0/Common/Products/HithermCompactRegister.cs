using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Xml.Serialization;

namespace Europlan.Common {
	public class HithermCompactRegister {
		#region Enums
		public class RegisterTypeEnumConverter : System.ComponentModel.TypeConverter {
			private static readonly string hitc_620_std = "HIT 620 Std";
			private static readonly string hitc_1000_std = "HIT 1000 Std";
			private static readonly string hitc_1500_std = "HIT 1500 Std";
			private static readonly string hitc_2000_std = "HIT 2000 Std";
			private static readonly string hitc_2500_std = "HIT 2500 Std";
			private static readonly string hitc_1000_par = "HIT 1000 Par";
			private static readonly string hitc_1500_par = "HIT 1500 Par";
			private static readonly string hitc_2000_par = "HIT 2000 Par";

			private Dictionary<string, HithermCompactRegisterTypeEnum> mappingFromString = new Dictionary<string, HithermCompactRegisterTypeEnum>();
			private Dictionary<HithermCompactRegisterTypeEnum, string> mappingToString = new Dictionary<HithermCompactRegisterTypeEnum, string>();

			public RegisterTypeEnumConverter() {
				mappingFromString.Add(hitc_620_std, HithermCompactRegisterTypeEnum.HITC_620_Std);
				mappingFromString.Add(hitc_1000_std, HithermCompactRegisterTypeEnum.HITC_1000_Std);
				mappingFromString.Add(hitc_1500_std, HithermCompactRegisterTypeEnum.HITC_1500_Std);
				mappingFromString.Add(hitc_2000_std, HithermCompactRegisterTypeEnum.HITC_2000_Std);
				mappingFromString.Add(hitc_2500_std, HithermCompactRegisterTypeEnum.HITC_2500_Std);
				mappingFromString.Add(hitc_1000_par, HithermCompactRegisterTypeEnum.HITC_1000_Par);
				mappingFromString.Add(hitc_1500_par, HithermCompactRegisterTypeEnum.HITC_1500_Par);
				mappingFromString.Add(hitc_2000_par, HithermCompactRegisterTypeEnum.HITC_2000_Par);

				mappingToString.Add(HithermCompactRegisterTypeEnum.HITC_620_Std, hitc_620_std);
				mappingToString.Add(HithermCompactRegisterTypeEnum.HITC_1000_Std, hitc_1000_std);
				mappingToString.Add(HithermCompactRegisterTypeEnum.HITC_1500_Std, hitc_1500_std);
				mappingToString.Add(HithermCompactRegisterTypeEnum.HITC_2000_Std, hitc_2000_std);
				mappingToString.Add(HithermCompactRegisterTypeEnum.HITC_2500_Std, hitc_2500_std);
				mappingToString.Add(HithermCompactRegisterTypeEnum.HITC_1000_Par, hitc_1000_par);
				mappingToString.Add(HithermCompactRegisterTypeEnum.HITC_1500_Par, hitc_1500_par);
				mappingToString.Add(HithermCompactRegisterTypeEnum.HITC_2000_Par, hitc_2000_par);
			}

			public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext context, Type sourceType) {
				return sourceType == typeof(string);
			}

			public override bool CanConvertTo(System.ComponentModel.ITypeDescriptorContext context, Type destinationType) {
				return destinationType == typeof(string);
			}

			public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value) {
				if (value is string) {
					if (mappingFromString.ContainsKey((string)value)) {
						return mappingFromString[(string)value];
					}
				}
				return base.ConvertFrom(context, culture, value);
			}

			public override object ConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType) {
				if (value is HithermCompactRegisterTypeEnum && destinationType == typeof(string)) {
					if (mappingToString.ContainsKey((HithermCompactRegisterTypeEnum)value)) {
						return mappingToString[(HithermCompactRegisterTypeEnum)value];
					}
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}
		}

		[System.ComponentModel.TypeConverter(typeof(RegisterTypeEnumConverter))]
		public enum HithermCompactRegisterTypeEnum {
			HITC_620_Std,
			HITC_1000_Std,
			//HIT_1250_Std,
			HITC_1500_Std,
			//HIT_1750_Std,
			HITC_2000_Std,
			HITC_2500_Std,
			//HIT_3000_Std,
			HITC_1000_Par,
			//HIT_1250_Par,
			HITC_1500_Par,
			//HIT_1750_Par,
			HITC_2000_Par
		}
		#endregion Enums

		#region Static Methods
		// Register Height in mm
		public static int GetRegisterHoehe(HithermCompactRegisterTypeEnum registerType) {
			switch (registerType) {
				case HithermCompactRegisterTypeEnum.HITC_620_Std:
					return 620;

				case HithermCompactRegisterTypeEnum.HITC_1000_Std:
					return 1000;

				case HithermCompactRegisterTypeEnum.HITC_1500_Std:
					return 1500;

				case HithermCompactRegisterTypeEnum.HITC_2000_Std:
					return 2000;

				case HithermCompactRegisterTypeEnum.HITC_2500_Std:
					return 2500;

				case HithermCompactRegisterTypeEnum.HITC_1000_Par:
				case HithermCompactRegisterTypeEnum.HITC_1500_Par:
				case HithermCompactRegisterTypeEnum.HITC_2000_Par:
					return 625;

				default:
					throw new Exception("Unknown Register Type");
			}
		}

		// Register Width in mm
		public static int GetRegisterBreite(HithermCompactRegisterTypeEnum registerType) {
			switch (registerType) {
				case HithermCompactRegisterTypeEnum.HITC_620_Std:
				case HithermCompactRegisterTypeEnum.HITC_1000_Std:
				case HithermCompactRegisterTypeEnum.HITC_1500_Std:
				case HithermCompactRegisterTypeEnum.HITC_2000_Std:
				case HithermCompactRegisterTypeEnum.HITC_2500_Std:
					return 625;

				case HithermCompactRegisterTypeEnum.HITC_1000_Par:
					return 1000;

				case HithermCompactRegisterTypeEnum.HITC_1500_Par:
					return 1500;

				case HithermCompactRegisterTypeEnum.HITC_2000_Par:
					return 2000;

				default:
					throw new Exception("Unknown Register Type");
			}
		}

		public static double GetHeatArea(HithermCompactRegisterTypeEnum registerType) {
			switch (registerType) {
				case HithermCompactRegisterTypeEnum.HITC_620_Std:
					return 0.62 * 0.5;

				case HithermCompactRegisterTypeEnum.HITC_1000_Std:
				case HithermCompactRegisterTypeEnum.HITC_1000_Par:
					return 1.0 * 0.5;

				case HithermCompactRegisterTypeEnum.HITC_1500_Std:
				case HithermCompactRegisterTypeEnum.HITC_1500_Par:
					return 1.5 * 0.5;

				case HithermCompactRegisterTypeEnum.HITC_2000_Std:
				case HithermCompactRegisterTypeEnum.HITC_2000_Par:
					return 2.0 * 0.5;

				case HithermCompactRegisterTypeEnum.HITC_2500_Std:
					return 2.5 * 0.5;

				default:
					throw new Exception("Unknown Register Type");
			}
		}
		#endregion Static Methods

		private HithermCompactRegisterTypeEnum registerType = HithermCompactRegisterTypeEnum.HITC_620_Std;
		private int registerCount = 1;
		private double pipeHorizontal = 0.25;
		private double pipeVertical = 0;
		private HithermWall wall;
		private String wallId = null;

		/*private Nullable<Point> origin = null;*/

		public HithermCompactRegister() {
			this.registerType = HithermCompactRegisterTypeEnum.HITC_620_Std;
			this.pipeHorizontal = 0.25;
			this.pipeVertical = 0.5;
			this.registerCount = 1;
		}

		public HithermCompactRegister(HithermCompactRegisterTypeEnum registerType, int registerCount) {
			this.registerType = registerType;
			this.registerCount = registerCount;
		}

		public HithermCompactRegisterTypeEnum RegisterType {
			get { return this.registerType; }
			set {
				bool setDefaultPipeVertical = this.pipeVertical == this.DefaultPipeVertical;
				this.registerType = value;
				if (setDefaultPipeVertical) {
					this.pipeVertical = this.DefaultPipeVertical;
				}
			}
		}

		[XmlIgnore]
		public bool IsParapet {
			get { return this.registerType == HithermCompactRegisterTypeEnum.HITC_1000_Par || this.registerType == HithermCompactRegisterTypeEnum.HITC_1500_Par || this.registerType == HithermCompactRegisterTypeEnum.HITC_2000_Par; }
		}

		[XmlIgnore]
		public int RegisterHoehe {
			get { return GetRegisterHoehe(this.registerType); }
		}

		[XmlIgnore]
		public int RegisterBreite {
			get { return GetRegisterBreite(this.registerType); }
		}

		[XmlIgnore]
		public double DefaultPipeVertical {
			get {
				/*if (this.orientation == RegisterOrientationEnum.ORIENTATION_VERTIKAL) {
					switch (this.registerType) {
						case RegisterTypeEnum.HIT_50_5:
						case RegisterTypeEnum.HIT_50_10:
							return 0.5;
						case RegisterTypeEnum.HIT_100_5:
						case RegisterTypeEnum.HIT_100_10:
							return 1;
						case RegisterTypeEnum.HIT_150_5:
						case RegisterTypeEnum.HIT_150_10:
							return 1.5;
						case RegisterTypeEnum.HIT_200_5:
						case RegisterTypeEnum.HIT_200_10:
							return 2.0;
						case RegisterTypeEnum.HIT_250_5:
						case RegisterTypeEnum.HIT_250_10:
							return 2.5;
						case RegisterTypeEnum.HIT_300_5:
						case RegisterTypeEnum.HIT_300_10:
							return 3.0;
						default:
							return 0;
					}
				} else {
					return ((double)this.RegisterBreite) / 100.0;
				}*/
				return 0;
			}
		}

		public double PipeHorizontal {
			get { return this.pipeHorizontal; }
			set { this.pipeHorizontal = value; }
		}

		public double PipeVertical {
			get { return this.pipeVertical; }
			set { this.pipeVertical = value; }
		}

		[XmlIgnore]
		public double EquivalentPipeLength {
			get { return this.EquivalentPipeLengthUnisolated + this.pipeVertical + this.pipeHorizontal; }
		}

		[XmlIgnore]
		public double EquivalentPipeLengthUnisolated {
			get {
				return this.HeatArea * 10;
				// TODO confirm
			}
		}

		public int RegisterCount {
			get { return this.registerCount; }
			set { this.registerCount = value; }
		}
		
		//public Nullable<Point> Origin {
		//	get { return this.origin; }
		//	set { this.origin = value; }
		//}

		[XmlIgnore]
		public double RegisterArea {
			get {
				return ((double)this.RegisterBreite / 1000.0) * ((double)this.RegisterHoehe / 1000.0) * this.registerCount;
			}
		}

		[XmlIgnore]
		public double HeatArea {
			get { return GetHeatArea(this.registerType) * this.registerCount; }
		}

		public double Heizleistung(double heizmittelTemp, double roomTemp, double alpha) {
			double faktor = 1;
			if (this.Wall != null) {
				faktor = this.Wall.Construction.Factor * EN1264.Instance.HithermBeplankungsFaktor(HithermCompactProduct.ConfigBeplankungRWerte, HithermCompactProduct.ConfigBeplankungFaktoren, this.Wall.DeckschichtValue);
			}
			faktor = faktor * alpha / HithermCompactProduct.ConfigAlphaWand;
			double[][] table;
			switch (this.registerType) {
				case HithermCompactRegisterTypeEnum.HITC_2500_Std:
					table = HithermCompactProduct.ConfigHlRegHeizleistung2500Std;
					break;
				case HithermCompactRegisterTypeEnum.HITC_2000_Std:
					table = HithermCompactProduct.ConfigHlRegHeizleistung2000Std;
					break;
				case HithermCompactRegisterTypeEnum.HITC_1500_Std:
					table = HithermCompactProduct.ConfigHlRegHeizleistung1500Std;
					break;
				case HithermCompactRegisterTypeEnum.HITC_1000_Std:
					table = HithermCompactProduct.ConfigHlRegHeizleistung1000Std;
					break;
				/*case HithermCompactRegisterTypeEnum.HITC_620_Std:
					table = HithermCompactProduct.ConfigHlRegHeizleistung620Std;
					break;*/
				case HithermCompactRegisterTypeEnum.HITC_2000_Par:
					table = HithermCompactProduct.ConfigHlRegHeizleistung2000Par;
					break;
				case HithermCompactRegisterTypeEnum.HITC_1500_Par:
					table = HithermCompactProduct.ConfigHlRegHeizleistung1500Par;
					break;
				case HithermCompactRegisterTypeEnum.HITC_1000_Par:
					table = HithermCompactProduct.ConfigHlRegHeizleistung1000Par;
					break;
				default:
					return 0;
			}
			return EN1264.Instance.WaermestromDichteRegister(heizmittelTemp, roomTemp, table, faktor, true) * this.registerCount;
		}

		public double HeizleistungBereinigung(double roomTemp) {
			double leistung = 0;
			if (this.Wall != null && this.Wall.Bereinigen) {
				leistung = this.Wall.UValueValue * this.HeatArea * (roomTemp - this.Wall.TempBehindHeat);
				if (leistung < 0) {
					leistung = 0;
				}
			}
			return leistung;
		}

		public double WaermeverlustAussen(double leistung, double roomTemp, double alphaAussen, double alphaInnen) {
			double verlust = 0;
			if (this.Wall != null) {
				verlust = EN1264.Instance.WaermeverlustAussen(leistung, this.Wall.Construction.RValue + this.Wall.DeckschichtValue + 1.0 / alphaInnen, HithermCompactProduct.ConfigDefaultDaemmung + this.Wall.AdditionalInsulationValue + 1.0 / alphaInnen, roomTemp, this.Wall.TempBehindHeat);
			}
			return verlust;
		}

		public double Kuehlleistung(double kuehlmittelTemp, double roomTemp, double alpha) {
			double faktor = 1;
			if (this.Wall != null) {
				faktor = this.Wall.Construction.Factor * EN1264.Instance.HithermBeplankungsFaktor(HithermCompactProduct.ConfigBeplankungRWerte, HithermCompactProduct.ConfigBeplankungFaktoren, this.Wall.DeckschichtValue);
			}
			faktor = faktor * alpha / HithermCompactProduct.ConfigAlphaWand;
			return 0;
			// TODO
			//return EN1264.Instance.KaeltestromDichteRegister(kuehlmittelTemp, roomTemp, HithermCompactProduct.ConfigHlRegKuehlleistung, faktor) * this.registerCount;
		}

		public double KuehlleistungBereinigung(double roomTemp) {
			double leistung = 0;
			if (this.Wall != null && this.Wall.Bereinigen) {
				leistung = this.Wall.UValueValue * this.HeatArea * (this.Wall.TempBehindCool - roomTemp);
				if (leistung < 0) {
					leistung = 0;
				}
			}
			return leistung;
		}

		public double KaelteverlustHinten(double leistung, double roomTemp, double alphaAussen, double alphaInnen) {
			double verlust = 0;
			if (this.Wall != null) {
				verlust = EN1264.Instance.WaermeverlustAussen(leistung, this.Wall.Construction.RValue + this.Wall.DeckschichtValue + 1.0 / alphaInnen, HithermCompactProduct.ConfigDefaultDaemmung + this.Wall.AdditionalInsulationValue + 1.0 / alphaInnen, roomTemp, this.Wall.TempBehindCool);
			}
			return verlust;
		}

		public double Druckverlust(double durchfluss) {
			return EN1264.Instance.DruckverlustRegister(this.registerType, durchfluss) * this.registerCount +
				EN1264.Instance.DruckverlustRohr(durchfluss, HithermCompactProduct.ConfigVerbindeLeitungInnenquerschnitt, HithermCompactProduct.ConfigRho, HithermCompactProduct.ConfigVerbindeLeitungInnendurchmesser, HithermCompactProduct.ConfigV, 0.000004, this.PipeVertical + this.PipeHorizontal);
		}

		private PlannedProduct product;
		[XmlIgnore]
		public PlannedProduct PlannedProduct {
			get { return this.product; }
			set { this.product = value; }
		}

		private int tmpHeizkreis = 0;
		[XmlIgnore]
		public int Heizkreis {
			get {
				if (this.product != null && this.product.Product is HithermCompactProduct) {
					int rtn = (this.product.Product as HithermCompactProduct).GetRegisterCircuitId(this);
					if (rtn > 0) {
						return rtn;
					} else {
						return tmpHeizkreis;
					}
				}
				return tmpHeizkreis;
			}
			set {
				if (this.product != null && this.product.Product is HithermCompactProduct) {
					if ((this.product.Product as HithermCompactProduct).GetRegisterCircuitId(this) == 0) {
						this.tmpHeizkreis = value;
					} else {
						(this.product.Product as HithermCompactProduct).MoveRegisterToCircuit(this, value);
					}
				}
			}
		}

		[XmlIgnore]
		public HithermWall Wall {
			get {
				if (this.wallId != null) {
					foreach (HithermWall hw in Project.Instance.HithermCompactWalls) {
						if (hw.Id == this.wallId) {
							this.wall = hw;
						}
					}
					this.wallId = null;
				}
				return this.wall; 
			}
			set {
				this.wallId = null;
				this.wall = value; 
			}
		}

		public string WallId {
			get { return this.Wall != null ? this.Wall.Id : null; }
			set { this.wallId = value; }
		}

		[XmlIgnore]
		public string PartNumber {
			get {
				switch (registerType) {
					case HithermCompactRegisterTypeEnum.HITC_620_Std:
						if (HithermCompactProduct.ConfigUsePlus) {
							return "";
						} else {
							return "";
						}
					default:
						throw new Exception("Unknown Register Type");
				}
			}
		}
	}
}
