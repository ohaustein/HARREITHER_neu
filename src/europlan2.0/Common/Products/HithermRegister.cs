using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Xml.Serialization;

namespace Europlan.Common {
	public class HithermRegister {
		#region Enums
		public class RegisterTypeEnumConverter : System.ComponentModel.TypeConverter {
			private static readonly string hit_50_10 = "HIT 50/10";
			private static readonly string hit_100_10 = "HIT 100/10";
			private static readonly string hit_150_10 = "HIT 150/10";
			private static readonly string hit_200_10 = "HIT 200/10";
			private static readonly string hit_250_10 = "HIT 250/10";
			private static readonly string hit_300_10 = "HIT 300/10";
			private static readonly string hit_50_5 = "HIT 50/5";
			private static readonly string hit_100_5 = "HIT 100/5";
			private static readonly string hit_150_5 = "HIT 150/5";
			private static readonly string hit_200_5 = "HIT 200/5";
			private static readonly string hit_250_5 = "HIT 250/5";
			private static readonly string hit_300_5 = "HIT 300/5";

			private Dictionary<string, RegisterTypeEnum> mappingFromString = new Dictionary<string, RegisterTypeEnum>();
			private Dictionary<RegisterTypeEnum, string> mappingToString = new Dictionary<RegisterTypeEnum, string>();

			public RegisterTypeEnumConverter() {
				mappingFromString.Add(hit_50_10, RegisterTypeEnum.HIT_50_10);
				mappingFromString.Add(hit_100_10, RegisterTypeEnum.HIT_100_10);
				mappingFromString.Add(hit_150_10, RegisterTypeEnum.HIT_150_10);
				mappingFromString.Add(hit_200_10, RegisterTypeEnum.HIT_200_10);
				mappingFromString.Add(hit_250_10, RegisterTypeEnum.HIT_250_10);
				mappingFromString.Add(hit_300_10, RegisterTypeEnum.HIT_300_10);
				mappingFromString.Add(hit_50_5, RegisterTypeEnum.HIT_50_5);
				mappingFromString.Add(hit_100_5, RegisterTypeEnum.HIT_100_5);
				mappingFromString.Add(hit_150_5, RegisterTypeEnum.HIT_150_5);
				mappingFromString.Add(hit_200_5, RegisterTypeEnum.HIT_200_5);
				mappingFromString.Add(hit_250_5, RegisterTypeEnum.HIT_250_5);
				mappingFromString.Add(hit_300_5, RegisterTypeEnum.HIT_300_5);

				mappingToString.Add(RegisterTypeEnum.HIT_50_10, hit_50_10);
				mappingToString.Add(RegisterTypeEnum.HIT_100_10, hit_100_10);
				mappingToString.Add(RegisterTypeEnum.HIT_150_10, hit_150_10);
				mappingToString.Add(RegisterTypeEnum.HIT_200_10, hit_200_10);
				mappingToString.Add(RegisterTypeEnum.HIT_250_10, hit_250_10);
				mappingToString.Add(RegisterTypeEnum.HIT_300_10, hit_300_10);
				mappingToString.Add(RegisterTypeEnum.HIT_50_5, hit_50_5);
				mappingToString.Add(RegisterTypeEnum.HIT_100_5, hit_100_5);
				mappingToString.Add(RegisterTypeEnum.HIT_150_5, hit_150_5);
				mappingToString.Add(RegisterTypeEnum.HIT_200_5, hit_200_5);
				mappingToString.Add(RegisterTypeEnum.HIT_250_5, hit_250_5);
				mappingToString.Add(RegisterTypeEnum.HIT_300_5, hit_300_5);
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
				if (value is RegisterTypeEnum && destinationType == typeof(string)) {
					if (mappingToString.ContainsKey((RegisterTypeEnum)value)) {
						return mappingToString[(RegisterTypeEnum)value];
					}
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}
		}

		[System.ComponentModel.TypeConverter(typeof(RegisterTypeEnumConverter))]
		public enum RegisterTypeEnum {
			HIT_50_10,
			HIT_100_10,
			HIT_150_10,
			HIT_200_10,
			HIT_250_10,
			HIT_300_10,
			HIT_50_5,
			HIT_100_5,
			HIT_150_5,
			HIT_200_5,
			HIT_250_5,
			HIT_300_5
		}

		public class RegisterOrientationEnumConverter : System.ComponentModel.TypeConverter {
			private static readonly string horizontal = "Horizontal";
			private static readonly string vertikal = "Vertikal";

			private Dictionary<string, RegisterOrientationEnum> mappingFromString = new Dictionary<string, RegisterOrientationEnum>();
			private Dictionary<RegisterOrientationEnum, string> mappingToString = new Dictionary<RegisterOrientationEnum, string>();

			public RegisterOrientationEnumConverter() {
				mappingFromString.Add(horizontal, RegisterOrientationEnum.ORIENTATION_HORIZONTAL);
				mappingFromString.Add(vertikal, RegisterOrientationEnum.ORIENTATION_VERTIKAL);
				mappingToString.Add(RegisterOrientationEnum.ORIENTATION_HORIZONTAL, horizontal);
				mappingToString.Add(RegisterOrientationEnum.ORIENTATION_VERTIKAL, vertikal);
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
				if (value is RegisterOrientationEnum && destinationType == typeof(string)) {
					if (mappingToString.ContainsKey((RegisterOrientationEnum)value)) {
						return mappingToString[(RegisterOrientationEnum)value];
					}
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}
		}

		[System.ComponentModel.TypeConverter(typeof(RegisterOrientationEnumConverter))]
		public enum RegisterOrientationEnum {
			ORIENTATION_HORIZONTAL,
			ORIENTATION_VERTIKAL
		}

		public class RohrabstandEnumConverter : System.ComponentModel.TypeConverter {
			private static readonly string horizontal = "10cm (Standardreg.)";
			private static readonly string vertikal = "5cm (Hochleistungsreg.)";

			private Dictionary<string, RohrabstandEnum> mappingFromString = new Dictionary<string, RohrabstandEnum>();
			private Dictionary<RohrabstandEnum, string> mappingToString = new Dictionary<RohrabstandEnum, string>();

			public RohrabstandEnumConverter() {
				mappingFromString.Add(horizontal, RohrabstandEnum.RC_STANDARD);
				mappingFromString.Add(vertikal, RohrabstandEnum.RC_HOCHLEISTUNG);
				mappingToString.Add(RohrabstandEnum.RC_STANDARD, horizontal);
				mappingToString.Add(RohrabstandEnum.RC_HOCHLEISTUNG, vertikal);
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
				if (value is RohrabstandEnum && destinationType == typeof(string)) {
					if (mappingToString.ContainsKey((RohrabstandEnum)value)) {
						return mappingToString[(RohrabstandEnum)value];
					}
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}
		}

		[System.ComponentModel.TypeConverter(typeof(RohrabstandEnumConverter))]
		public enum RohrabstandEnum {
			RC_STANDARD,
			RC_HOCHLEISTUNG
		}
		#endregion Enums

		#region Static Methods
		public static RohrabstandEnum GetRohrabstandForRegisterType(RegisterTypeEnum registerType) {
			switch (registerType) {
				case RegisterTypeEnum.HIT_50_10:
				case RegisterTypeEnum.HIT_100_10:
				case RegisterTypeEnum.HIT_150_10:
				case RegisterTypeEnum.HIT_200_10:
				case RegisterTypeEnum.HIT_250_10:
				case RegisterTypeEnum.HIT_300_10:
					return RohrabstandEnum.RC_STANDARD;

				case RegisterTypeEnum.HIT_50_5:
				case RegisterTypeEnum.HIT_100_5:
				case RegisterTypeEnum.HIT_150_5:
				case RegisterTypeEnum.HIT_200_5:
				case RegisterTypeEnum.HIT_250_5:
				case RegisterTypeEnum.HIT_300_5:
					return RohrabstandEnum.RC_HOCHLEISTUNG;

				default:
					throw new Exception("Unknown Register Type");
			}
		}

		public static int GetRegisterHoehe(RegisterTypeEnum registerType) {
			switch (registerType) {
				case RegisterTypeEnum.HIT_50_10:
				case RegisterTypeEnum.HIT_50_5:
					return 50;

				case RegisterTypeEnum.HIT_100_10:
				case RegisterTypeEnum.HIT_100_5:
					return 100;

				case RegisterTypeEnum.HIT_150_10:
				case RegisterTypeEnum.HIT_150_5:
					return 150;

				case RegisterTypeEnum.HIT_200_10:
				case RegisterTypeEnum.HIT_200_5:
					return 200;

				case RegisterTypeEnum.HIT_250_10:
				case RegisterTypeEnum.HIT_250_5:
					return 250;

				case RegisterTypeEnum.HIT_300_10:
				case RegisterTypeEnum.HIT_300_5:
					return 300;

				default:
					throw new Exception("Unknown Register Type");
			}
		}
		#endregion Static Methods

		private RegisterTypeEnum registerType = RegisterTypeEnum.HIT_50_10;
		private RegisterOrientationEnum orientation = RegisterOrientationEnum.ORIENTATION_VERTIKAL;
		private int rohre = 1;
		private double pipeHorizontal = 0.25;
		private double pipeVertical = 0.5;
		private HithermWall wall;
		private String wallId = null;

		/*private Nullable<Point> origin = null;*/

		public HithermRegister() {
			this.registerType = RegisterTypeEnum.HIT_50_10;
			this.orientation = RegisterOrientationEnum.ORIENTATION_VERTIKAL;
			this.pipeHorizontal = 0.25;
			this.pipeVertical = 0.5;
			this.rohre = 1;
		}

		public HithermRegister(RegisterTypeEnum registerType, RegisterOrientationEnum orientation, int rohre) {
			this.registerType = registerType;
			this.orientation = orientation;
			this.rohre = rohre;
		}

		public RegisterTypeEnum RegisterType {
			get { return this.registerType; }
			set {
				bool setDefaultPipeVertical = this.pipeVertical == this.DefaultPipeVertical;
				int oldBreite = this.RegisterBreite;
				this.registerType = value;
				this.RegisterBreite = oldBreite;
				if (setDefaultPipeVertical) {
					this.pipeVertical = this.DefaultPipeVertical;
				}
			}
		}

		[XmlIgnore]
		public bool IsHochleistungsRegister {
			get { return this.registerType == RegisterTypeEnum.HIT_50_5 || this.registerType == RegisterTypeEnum.HIT_100_5 || this.registerType == RegisterTypeEnum.HIT_150_5 || this.registerType == RegisterTypeEnum.HIT_200_5 || this.registerType == RegisterTypeEnum.HIT_250_5 || this.registerType == RegisterTypeEnum.HIT_300_5; }
		}

		[XmlIgnore]
		public RohrabstandEnum Rohrabstand {
			get { return GetRohrabstandForRegisterType(this.registerType); }
		}
		
		public int RegisterHoehe {
			get { return GetRegisterHoehe(this.registerType); }
		}
		
		public RegisterOrientationEnum Orientation {
			get { return this.orientation; }
			set {
				bool setDefaultPipeVertical = this.pipeVertical == this.DefaultPipeVertical;
				this.orientation = value;
				if (setDefaultPipeVertical) {
					this.pipeVertical = this.DefaultPipeVertical;
				}
			}
		}

		[XmlIgnore]
		public bool Horizontal {
			get { return this.orientation == RegisterOrientationEnum.ORIENTATION_HORIZONTAL; }
			set { this.Orientation = value ? RegisterOrientationEnum.ORIENTATION_HORIZONTAL : RegisterOrientationEnum.ORIENTATION_VERTIKAL; }
		}
		
		public int Rohre {
			get { return this.rohre; }
			set {
				bool setDefaultPipeVertical = this.pipeVertical == this.DefaultPipeVertical;
				this.rohre = value;
				if (this.Rohrabstand == RohrabstandEnum.RC_HOCHLEISTUNG) {
					if (this.rohre > 27) {
						this.rohre = 27;
					}
				} else {
					if (this.rohre > 30) {
						this.rohre = 30;
					}
				}
				if (this.rohre < 1) {
					this.rohre = 1;
				}
				if (setDefaultPipeVertical) {
					this.pipeVertical = this.DefaultPipeVertical;
				}
			}
		}

		[XmlIgnore]
		public double DefaultPipeVertical {
			get {
				if (this.orientation == RegisterOrientationEnum.ORIENTATION_VERTIKAL) {
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
				}
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
				return this.Area * 10;
				// TODO confirm
			}
		}

		[XmlIgnore]
		public int NrOfRegisters {
			get { return (this.Rohrabstand == RohrabstandEnum.RC_HOCHLEISTUNG ? (int)Math.Ceiling(((float)this.rohre) / 9.0) : (int)Math.Ceiling(((float)this.rohre) / 5.0)); }
		}
		
		[XmlIgnore]
		public int RegisterBreite {
			get {
				if (this.Rohrabstand == RohrabstandEnum.RC_HOCHLEISTUNG) {
					return (this.Rohre * 10 / 9) * 5;
				} else {
					return this.Rohre * 10;
				}
			}
			set {
				if (this.Rohrabstand == RohrabstandEnum.RC_HOCHLEISTUNG) {
					this.Rohre = 9 * (value + 5) / 50;
				} else {
					this.Rohre = value / 10;
				}
			}
		}

		//public Nullable<Point> Origin {
		//	get { return this.origin; }
		//	set { this.origin = value; }
		//}
		
		[XmlIgnore]
		public double Area {
			get {
				return ((double)this.RegisterBreite / 100.0) * ((double)this.RegisterHoehe / 100.0);
			}
		}

		public double Heizleistung(double heizmittelTemp, double roomTemp, double alpha) {
			double faktor = 1;
			if (this.Wall != null) {
				faktor = this.Wall.Construction.Factor * EN1264.Instance.HithermBeplankungsFaktor(HithermProduct.ConfigBeplankungRWerte, HithermProduct.ConfigBeplankungFaktoren, this.Wall.DeckschichtValue);
			}
			faktor = faktor * alpha / HithermProduct.ConfigAlphaWand;
			return EN1264.Instance.WaermestromDichteRegister(heizmittelTemp, roomTemp, this.IsHochleistungsRegister ? HithermProduct.ConfigHlRegHeizleistung : HithermProduct.ConfigStdRegHeizleistung, faktor) * this.Area;
		}

		public double HeizleistungBereinigung(double roomTemp) {
			double leistung = 0;
			if (this.Wall != null && this.Wall.Bereinigen) {
				leistung = this.Wall.UValueValue * this.Area * (roomTemp - this.Wall.TempBehindHeat);
				if (leistung < 0) {
					leistung = 0;
				}
			}
			return leistung;
		}

		public double WaermeverlustAussen(double leistung, double roomTemp, double alphaAussen, double alphaInnen) {
			double verlust = 0;
			if (this.Wall != null) {
				verlust = EN1264.Instance.WaermeverlustAussen(leistung, this.Wall.Construction.RValue + this.Wall.DeckschichtValue + 1.0 / alphaInnen, HithermProduct.ConfigDefaultDaemmung + this.Wall.AdditionalInsulationValue + 1.0 / alphaInnen, roomTemp, this.Wall.TempBehindHeat);
			}
			return verlust;
		}

		public double Kuehlleistung(double kuehlmittelTemp, double roomTemp, double alpha) {
			double faktor = 1;
			if (this.Wall != null) {
				faktor = this.Wall.Construction.Factor * EN1264.Instance.HithermBeplankungsFaktor(HithermProduct.ConfigBeplankungRWerte, HithermProduct.ConfigBeplankungFaktoren, this.Wall.DeckschichtValue);
			}
			faktor = faktor * alpha / HithermProduct.ConfigAlphaWand;
			return EN1264.Instance.KaeltestromDichteRegister(kuehlmittelTemp, roomTemp, this.IsHochleistungsRegister ? HithermProduct.ConfigHlRegKuehlleistung : HithermProduct.ConfigStdRegKuehlleistung, faktor) * this.Area;
		}

		public double KuehlleistungBereinigung(double roomTemp) {
			double leistung = 0;
			if (this.Wall != null && this.Wall.Bereinigen) {
				leistung = this.Wall.UValueValue * this.Area * (this.Wall.TempBehindCool - roomTemp);
				if (leistung < 0) {
					leistung = 0;
				}
			}
			return leistung;
		}

		public double KaelteverlustHinten(double leistung, double roomTemp, double alphaAussen, double alphaInnen) {
			double verlust = 0;
			if (this.Wall != null) {
				verlust = EN1264.Instance.WaermeverlustAussen(leistung, this.Wall.Construction.RValue + this.Wall.DeckschichtValue + 1.0 / alphaInnen, HithermProduct.ConfigDefaultDaemmung + this.Wall.AdditionalInsulationValue + 1.0 / alphaInnen, roomTemp, this.Wall.TempBehindCool);
			}
			return verlust;
		}

		public double Druckverlust(double durchfluss) {
			return EN1264.Instance.DruckverlustRegister(this.registerType, this.RegisterBreite, durchfluss) +
				EN1264.Instance.DruckverlustRohr(durchfluss, HithermProduct.ConfigVerbindeLeitungInnenquerschnitt, HithermProduct.ConfigRho, HithermProduct.ConfigVerbindeLeitungInnendurchmesser, HithermProduct.ConfigV, 0.000004, this.PipeVertical + this.PipeHorizontal);
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
				if (this.product != null && this.product.Product is HithermProduct) {
					int rtn = (this.product.Product as HithermProduct).GetRegisterCircuitId(this);
					if (rtn > 0) {
						return rtn;
					} else {
						return tmpHeizkreis;
					}
				}
				return tmpHeizkreis;
			}
			set {
				if (this.product != null && this.product.Product is HithermProduct) {
					if ((this.product.Product as HithermProduct).GetRegisterCircuitId(this) == 0) {
						this.tmpHeizkreis = value;
					} else {
						(this.product.Product as HithermProduct).MoveRegisterToCircuit(this, value);
					}
				}
				/*if (this.product != null) {
					HithermCircuit hc;
					if (this.heizkreis > 0) {
						hc = this.product.Product.GetCircuit(this.heizkreis - 1) as HithermCircuit;
						if (hc != null) {
							hc.Registers.Remove(this);
						}
						this.heizkreis = 0;
					}
					hc = this.product.Product.GetCircuit(value - 1) as HithermCircuit;
					if (hc != null) {
						hc.Registers.Add(this);
						this.heizkreis = value;
					}
				}*/
			}
		}

		[XmlIgnore]
		public HithermWall Wall {
			get {
				if (this.wallId != null) {
					foreach (HithermWall hw in Project.Instance.HithermWalls) {
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
	}
}
