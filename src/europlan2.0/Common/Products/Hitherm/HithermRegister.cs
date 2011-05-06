using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Xml.Serialization;
using System.Threading;

namespace Europlan.Common {
	public class HithermRegister {
		#region Enums
		public class RegisterTypeEnumConverter : System.ComponentModel.TypeConverter {
			private static readonly string hit_50_10 = EuroplanRes.HithermRegister_Hit50L; //"HIT 50/10";
			private static readonly string hit_100_10 = EuroplanRes.HithermRegister_Hit100L; //"HIT 100/10";
			private static readonly string hit_150_10 = EuroplanRes.HithermRegister_Hit150L; //"HIT 150/10";
			private static readonly string hit_200_10 = EuroplanRes.HithermRegister_Hit200L; //"HIT 200/10";
			private static readonly string hit_250_10 = EuroplanRes.HithermRegister_Hit250L; //"HIT 250/10";
			private static readonly string hit_300_10 = EuroplanRes.HithermRegister_Hit300L; //"HIT 300/10";
			private static readonly string hit_50_5 = EuroplanRes.HithermRegister_Hit50HL; //"HIT 50/5";
			private static readonly string hit_100_5 = EuroplanRes.HithermRegister_Hit100HL; //"HIT 100/5";
			private static readonly string hit_150_5 = EuroplanRes.HithermRegister_Hit150HL; //"HIT 150/5";
			private static readonly string hit_200_5 = EuroplanRes.HithermRegister_Hit200HL; //"HIT 200/5";
			private static readonly string hit_250_5 = EuroplanRes.HithermRegister_Hit250HL; //"HIT 250/5";
			private static readonly string hit_300_5 = EuroplanRes.HithermRegister_Hit300HL; //"HIT 300/5";

			private static readonly string hit_50_10_Short = EuroplanRes.HithermRegister_Hit50L_Short; //"50/10";
			private static readonly string hit_100_10_Short = EuroplanRes.HithermRegister_Hit100L_Short; //"100/10";
			private static readonly string hit_150_10_Short = EuroplanRes.HithermRegister_Hit150L_Short; //"150/10";
			private static readonly string hit_200_10_Short = EuroplanRes.HithermRegister_Hit200L_Short; //"200/10";
			private static readonly string hit_250_10_Short = EuroplanRes.HithermRegister_Hit250L_Short; //"250/10";
			private static readonly string hit_300_10_Short = EuroplanRes.HithermRegister_Hit300L_Short; //"300/10";
			private static readonly string hit_50_5_Short = EuroplanRes.HithermRegister_Hit50HL_Short; //"50/5";
			private static readonly string hit_100_5_Short = EuroplanRes.HithermRegister_Hit100HL_Short; //"100/5";
			private static readonly string hit_150_5_Short = EuroplanRes.HithermRegister_Hit150HL_Short; //"150/5";
			private static readonly string hit_200_5_Short = EuroplanRes.HithermRegister_Hit200HL_Short; //"200/5";
			private static readonly string hit_250_5_Short = EuroplanRes.HithermRegister_Hit250HL_Short; //"250/5";
			private static readonly string hit_300_5_Short = EuroplanRes.HithermRegister_Hit300HL_Short; //"300/5";

			private Dictionary<string, HithermRegisterTypeEnum> mappingFromString = new Dictionary<string, HithermRegisterTypeEnum>();
			private Dictionary<HithermRegisterTypeEnum, string> mappingToString = new Dictionary<HithermRegisterTypeEnum, string>();

			private Dictionary<string, HithermRegisterTypeEnum> mappingFromShortString = new Dictionary<string, HithermRegisterTypeEnum>();
			private Dictionary<HithermRegisterTypeEnum, string> mappingToShortString = new Dictionary<HithermRegisterTypeEnum, string>();

			private bool shortNames = false;

			public RegisterTypeEnumConverter() {
				this.Initialize();
			}

			public RegisterTypeEnumConverter(bool shortNames) {
				this.shortNames = shortNames;
				this.Initialize();
			}

			public bool ShortNames {
				get { return this.shortNames; }
				set { this.shortNames = value; }
			}

			private void Initialize() {
				mappingFromString.Add(hit_50_10, HithermRegisterTypeEnum.HIT_50_10);
				mappingFromString.Add(hit_100_10, HithermRegisterTypeEnum.HIT_100_10);
				mappingFromString.Add(hit_150_10, HithermRegisterTypeEnum.HIT_150_10);
				mappingFromString.Add(hit_200_10, HithermRegisterTypeEnum.HIT_200_10);
				mappingFromString.Add(hit_250_10, HithermRegisterTypeEnum.HIT_250_10);
				mappingFromString.Add(hit_300_10, HithermRegisterTypeEnum.HIT_300_10);
				mappingFromString.Add(hit_50_5, HithermRegisterTypeEnum.HIT_50_5);
				mappingFromString.Add(hit_100_5, HithermRegisterTypeEnum.HIT_100_5);
				mappingFromString.Add(hit_150_5, HithermRegisterTypeEnum.HIT_150_5);
				mappingFromString.Add(hit_200_5, HithermRegisterTypeEnum.HIT_200_5);
				mappingFromString.Add(hit_250_5, HithermRegisterTypeEnum.HIT_250_5);
				mappingFromString.Add(hit_300_5, HithermRegisterTypeEnum.HIT_300_5);

				mappingToString.Add(HithermRegisterTypeEnum.HIT_50_10, hit_50_10);
				mappingToString.Add(HithermRegisterTypeEnum.HIT_100_10, hit_100_10);
				mappingToString.Add(HithermRegisterTypeEnum.HIT_150_10, hit_150_10);
				mappingToString.Add(HithermRegisterTypeEnum.HIT_200_10, hit_200_10);
				mappingToString.Add(HithermRegisterTypeEnum.HIT_250_10, hit_250_10);
				mappingToString.Add(HithermRegisterTypeEnum.HIT_300_10, hit_300_10);
				mappingToString.Add(HithermRegisterTypeEnum.HIT_50_5, hit_50_5);
				mappingToString.Add(HithermRegisterTypeEnum.HIT_100_5, hit_100_5);
				mappingToString.Add(HithermRegisterTypeEnum.HIT_150_5, hit_150_5);
				mappingToString.Add(HithermRegisterTypeEnum.HIT_200_5, hit_200_5);
				mappingToString.Add(HithermRegisterTypeEnum.HIT_250_5, hit_250_5);
				mappingToString.Add(HithermRegisterTypeEnum.HIT_300_5, hit_300_5);

				mappingFromShortString.Add(hit_50_10_Short, HithermRegisterTypeEnum.HIT_50_10);
				mappingFromShortString.Add(hit_100_10_Short, HithermRegisterTypeEnum.HIT_100_10);
				mappingFromShortString.Add(hit_150_10_Short, HithermRegisterTypeEnum.HIT_150_10);
				mappingFromShortString.Add(hit_200_10_Short, HithermRegisterTypeEnum.HIT_200_10);
				mappingFromShortString.Add(hit_250_10_Short, HithermRegisterTypeEnum.HIT_250_10);
				mappingFromShortString.Add(hit_300_10_Short, HithermRegisterTypeEnum.HIT_300_10);
				mappingFromShortString.Add(hit_50_5_Short, HithermRegisterTypeEnum.HIT_50_5);
				mappingFromShortString.Add(hit_100_5_Short, HithermRegisterTypeEnum.HIT_100_5);
				mappingFromShortString.Add(hit_150_5_Short, HithermRegisterTypeEnum.HIT_150_5);
				mappingFromShortString.Add(hit_200_5_Short, HithermRegisterTypeEnum.HIT_200_5);
				mappingFromShortString.Add(hit_250_5_Short, HithermRegisterTypeEnum.HIT_250_5);
				mappingFromShortString.Add(hit_300_5_Short, HithermRegisterTypeEnum.HIT_300_5);

				mappingToShortString.Add(HithermRegisterTypeEnum.HIT_50_10, hit_50_10_Short);
				mappingToShortString.Add(HithermRegisterTypeEnum.HIT_100_10, hit_100_10_Short);
				mappingToShortString.Add(HithermRegisterTypeEnum.HIT_150_10, hit_150_10_Short);
				mappingToShortString.Add(HithermRegisterTypeEnum.HIT_200_10, hit_200_10_Short);
				mappingToShortString.Add(HithermRegisterTypeEnum.HIT_250_10, hit_250_10_Short);
				mappingToShortString.Add(HithermRegisterTypeEnum.HIT_300_10, hit_300_10_Short);
				mappingToShortString.Add(HithermRegisterTypeEnum.HIT_50_5, hit_50_5_Short);
				mappingToShortString.Add(HithermRegisterTypeEnum.HIT_100_5, hit_100_5_Short);
				mappingToShortString.Add(HithermRegisterTypeEnum.HIT_150_5, hit_150_5_Short);
				mappingToShortString.Add(HithermRegisterTypeEnum.HIT_200_5, hit_200_5_Short);
				mappingToShortString.Add(HithermRegisterTypeEnum.HIT_250_5, hit_250_5_Short);
				mappingToShortString.Add(HithermRegisterTypeEnum.HIT_300_5, hit_300_5_Short);
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
					if (mappingFromShortString.ContainsKey((string)value)) {
						return mappingFromString[(string)value];
					}
				}
				return base.ConvertFrom(context, culture, value);
			}

			public override object ConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType) {
				if (value is HithermRegisterTypeEnum && destinationType == typeof(string)) {
					if (this.shortNames) {
						if (mappingToShortString.ContainsKey((HithermRegisterTypeEnum)value)) {
							return mappingToShortString[(HithermRegisterTypeEnum)value];
						}
					} else {
						if (mappingToString.ContainsKey((HithermRegisterTypeEnum)value)) {
							return mappingToString[(HithermRegisterTypeEnum)value];
						}
					}
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}
		}

		[System.ComponentModel.TypeConverter(typeof(RegisterTypeEnumConverter))]
		// if extended - consider also the part number getter
		public enum HithermRegisterTypeEnum {
			HIT_50_5,
			HIT_100_5,
			HIT_150_5,
			HIT_200_5,
			HIT_250_5,
			HIT_300_5,
			HIT_50_10,
			HIT_100_10,
			HIT_150_10,
			HIT_200_10,
			HIT_250_10,
			HIT_300_10,
		}

		public class RegisterOrientationEnumConverter : System.ComponentModel.TypeConverter {
			private static readonly string horizontal = EuroplanRes.HithermRegister_Horizontal; //"Horizontal";
			private static readonly string vertikal = EuroplanRes.HithermRegister_Vertikal; //"Vertikal";

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
			private static readonly string horizontal = EuroplanRes.HithermRegister_Leistungsregister; //"10cm (Standardreg.)";
			private static readonly string vertikal = EuroplanRes.HithermRegister_Hochleistungsregister; //"5cm (Hochleistungsreg.)";

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
		public static RohrabstandEnum GetRohrabstandForRegisterType(HithermRegisterTypeEnum registerType) {
			switch (registerType) {
				case HithermRegisterTypeEnum.HIT_50_10:
				case HithermRegisterTypeEnum.HIT_100_10:
				case HithermRegisterTypeEnum.HIT_150_10:
				case HithermRegisterTypeEnum.HIT_200_10:
				case HithermRegisterTypeEnum.HIT_250_10:
				case HithermRegisterTypeEnum.HIT_300_10:
					return RohrabstandEnum.RC_STANDARD;

				case HithermRegisterTypeEnum.HIT_50_5:
				case HithermRegisterTypeEnum.HIT_100_5:
				case HithermRegisterTypeEnum.HIT_150_5:
				case HithermRegisterTypeEnum.HIT_200_5:
				case HithermRegisterTypeEnum.HIT_250_5:
				case HithermRegisterTypeEnum.HIT_300_5:
					return RohrabstandEnum.RC_HOCHLEISTUNG;

				default:
					throw new Exception("Unknown Register Type");
			}
		}

		public static int GetRegisterHoehe(HithermRegisterTypeEnum registerType) {
			switch (registerType) {
				case HithermRegisterTypeEnum.HIT_50_10:
				case HithermRegisterTypeEnum.HIT_50_5:
					return 50;

				case HithermRegisterTypeEnum.HIT_100_10:
				case HithermRegisterTypeEnum.HIT_100_5:
					return 100;

				case HithermRegisterTypeEnum.HIT_150_10:
				case HithermRegisterTypeEnum.HIT_150_5:
					return 150;

				case HithermRegisterTypeEnum.HIT_200_10:
				case HithermRegisterTypeEnum.HIT_200_5:
					return 200;

				case HithermRegisterTypeEnum.HIT_250_10:
				case HithermRegisterTypeEnum.HIT_250_5:
					return 250;

				case HithermRegisterTypeEnum.HIT_300_10:
				case HithermRegisterTypeEnum.HIT_300_5:
					return 300;

				default:
					throw new Exception("Unknown Register Type");
			}
		}

		public static Nullable<HithermRegisterTypeEnum> GetRegisterTypeForHoehe(int hoehe, bool hochleistungsRegister) {
			if (hoehe >= GetRegisterHoehe(HithermRegisterTypeEnum.HIT_300_5)) {
				return hochleistungsRegister ? HithermRegisterTypeEnum.HIT_300_5 : HithermRegisterTypeEnum.HIT_300_10;
			} else if (hoehe >= GetRegisterHoehe(HithermRegisterTypeEnum.HIT_250_5)) {
				return hochleistungsRegister ? HithermRegisterTypeEnum.HIT_250_5 : HithermRegisterTypeEnum.HIT_250_10;
			} else if (hoehe >= GetRegisterHoehe(HithermRegisterTypeEnum.HIT_200_5)) {
				return hochleistungsRegister ? HithermRegisterTypeEnum.HIT_200_5 : HithermRegisterTypeEnum.HIT_200_10;
			} else if (hoehe >= GetRegisterHoehe(HithermRegisterTypeEnum.HIT_150_5)) {
				return hochleistungsRegister ? HithermRegisterTypeEnum.HIT_150_5 : HithermRegisterTypeEnum.HIT_150_10;
			} else if (hoehe >= GetRegisterHoehe(HithermRegisterTypeEnum.HIT_100_5)) {
				return hochleistungsRegister ? HithermRegisterTypeEnum.HIT_100_5 : HithermRegisterTypeEnum.HIT_100_10;
			} else if (hoehe >= GetRegisterHoehe(HithermRegisterTypeEnum.HIT_50_5)) {
				return hochleistungsRegister ? HithermRegisterTypeEnum.HIT_50_5 : HithermRegisterTypeEnum.HIT_50_10;
			}
			return null;
		}
		#endregion Static Methods

		private HithermRegisterTypeEnum registerType = HithermRegisterTypeEnum.HIT_50_5;
		private RegisterOrientationEnum orientation = RegisterOrientationEnum.ORIENTATION_VERTIKAL;
		private int rohre = 1;
		private double pipeHorizontal = 0.25;
		private double pipeVertical = 0.5;
		private HithermWall wall;
		private String wallId = null;

		private String graphWallId = null;
		private double graphPosX = 0;
		private double graphPosY = 0;
		private bool graphVorlaufRight = true;

		/*private Nullable<Point> origin = null;*/

		public HithermRegister() {
			this.registerType = HithermRegisterTypeEnum.HIT_50_5;
			this.orientation = RegisterOrientationEnum.ORIENTATION_VERTIKAL;
			this.pipeHorizontal = 0.25;
			this.pipeVertical = 0.5;
			this.rohre = 1;
		}

		public HithermRegister(HithermRegisterTypeEnum registerType, RegisterOrientationEnum orientation, int rohre) {
			this.registerType = registerType;
			this.orientation = orientation;
			this.rohre = rohre;
		}

		public HithermRegisterTypeEnum RegisterType {
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
			get { return this.registerType == HithermRegisterTypeEnum.HIT_50_5 || this.registerType == HithermRegisterTypeEnum.HIT_100_5 || this.registerType == HithermRegisterTypeEnum.HIT_150_5 || this.registerType == HithermRegisterTypeEnum.HIT_200_5 || this.registerType == HithermRegisterTypeEnum.HIT_250_5 || this.registerType == HithermRegisterTypeEnum.HIT_300_5; }
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
					if (HithermProduct.ConfigUsePlus) {
						if (this.rohre > 28) {
							this.rohre = 28;
						}
					} else {
						if (this.rohre > 27) {
							this.rohre = 27;
						}
					}
				} else {
					if (this.rohre > 30) {
						this.rohre = 30;
					}
				}
				if (this.rohre < 3) {
					this.rohre = 3;
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
						case HithermRegisterTypeEnum.HIT_50_5:
						case HithermRegisterTypeEnum.HIT_50_10:
							return 0.5;
						case HithermRegisterTypeEnum.HIT_100_5:
						case HithermRegisterTypeEnum.HIT_100_10:
							return 1;
						case HithermRegisterTypeEnum.HIT_150_5:
						case HithermRegisterTypeEnum.HIT_150_10:
							return 1.5;
						case HithermRegisterTypeEnum.HIT_200_5:
						case HithermRegisterTypeEnum.HIT_200_10:
							return 2.0;
						case HithermRegisterTypeEnum.HIT_250_5:
						case HithermRegisterTypeEnum.HIT_250_10:
							return 2.5;
						case HithermRegisterTypeEnum.HIT_300_5:
						case HithermRegisterTypeEnum.HIT_300_10:
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
			get { return this.HeatArea * 10; }
		}

		[XmlIgnore]
		public int RegisterCount {
			get {
				if (HithermProduct.ConfigUsePlus) {
					return (this.Rohrabstand == RohrabstandEnum.RC_HOCHLEISTUNG ? (int)Math.Ceiling(((float)this.rohre) / 14.0) : (int)Math.Ceiling(((float)this.rohre) / 10.0));
				} else {
					return (this.Rohrabstand == RohrabstandEnum.RC_HOCHLEISTUNG ? (int)Math.Ceiling(((float)this.rohre) / 9.0) : (int)Math.Ceiling(((float)this.rohre) / 5.0));
				}
			}
		}
		
		[XmlIgnore]
		public int RegisterBreite {
			get {
				if (this.Rohrabstand == RohrabstandEnum.RC_HOCHLEISTUNG) {
					if (HithermProduct.ConfigUsePlus) {
						return (this.Rohre * 15 / 14) * 5;
					} else {
						return (this.Rohre * 10 / 9) * 5;
					}
				} else {
					return this.Rohre * 10;
				}
			}
			set {
				if (this.Rohrabstand == RohrabstandEnum.RC_HOCHLEISTUNG) {
					if (HithermProduct.ConfigUsePlus) {
						this.Rohre = 14 * (value + 5) / 5 / 15;
					} else {
						this.Rohre = 9 * (value + 5) / 50;
					}
				} else {
					this.Rohre = value / 10;
				}
			}
		}

		[XmlIgnore]
		public double RegisterBreiteForDrawing {
			get {
				if (this.Rohrabstand == RohrabstandEnum.RC_HOCHLEISTUNG) {
					if (HithermProduct.ConfigUsePlus) {
						return (this.Rohre % 14 == 0) ? this.RegisterBreite : this.RegisterBreite + 2.5;
					} else {
						return (this.Rohre % 9 == 0) ? this.RegisterBreite : this.RegisterBreite + 2.5;
					}
				} else {
					return this.RegisterBreite;
				}
			}
			set {
				this.Rohre = 30;
				while (this.RegisterBreiteForDrawing > value && this.Rohre > 3) {
					this.Rohre--;
				}
				/*if (this.Rohrabstand == RohrabstandEnum.RC_HOCHLEISTUNG) {
					value = value - 2.5;
					if (HithermProduct.ConfigUsePlus) {
						int rohre = (int)Math.Floor(value / 5);
						rohre -= rohre / 15;
						this.Rohre = rohre;
					} else {
						int rohre = (int)Math.Floor(value / 5);
						rohre -= rohre / 10;
						this.Rohre = rohre;
					}
				} else {
					this.Rohre = (int)Math.Floor(value / 10);
				}*/
			}
		}

		//public Nullable<Point> Origin {
		//	get { return this.origin; }
		//	set { this.origin = value; }
		//}
		
		[XmlIgnore]
		public double CoveredArea {
			get {
				return this.HeatArea;
				//return ((double)this.RegisterBreite / 100.0) * ((double)this.RegisterHoehe / 100.0);
			}
		}

		private double RealRegisterArea {
			get { return ((double)this.RegisterBreite / 100.0) * ((double)this.RegisterHoehe / 100.0); }
		}

		[XmlIgnore]
		public double HeatArea {
			get {
				return this.RealRegisterArea + ((double)this.RegisterBreite / 100.0) * 0.1 + ((double)this.RegisterHoehe / 100.0) * 0.1;
			}
		}

		public double Heizleistung(double heizmittelTemp, double roomTemp, double alpha) {
			double faktor = 1;
			if (this.Wall != null) {
				faktor = this.Wall.Construction.Factor * EN1264.Instance.HithermBeplankungsFaktor(HithermProduct.ConfigBeplankungRWerte, HithermProduct.ConfigBeplankungFaktoren, this.Wall.DeckschichtValue);
			}
			faktor = faktor * alpha / Product.ConfigAlphaWandHeat * HithermProduct.ConfigLeistungsFaktorHeizen;
			return EN1264.Instance.WaermestromDichteRegister(heizmittelTemp, roomTemp, this.IsHochleistungsRegister ? HithermProduct.ConfigHlRegHeizleistung : HithermProduct.ConfigStdRegHeizleistung, faktor, false) * this.HeatArea;
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
				verlust = EN1264.Instance.WaermeverlustAussen(leistung, this.Wall.Construction.RValue + this.Wall.DeckschichtValue + 1.0 / alphaInnen, HithermProduct.ConfigDefaultDaemmung + this.Wall.AdditionalInsulationValue + 1.0 / alphaAussen, roomTemp, this.Wall.TempBehindHeat);
			}
			return verlust;
		}

		public double Kuehlleistung(double kuehlmittelTemp, double roomTemp, double alpha) {
			double faktor = 1;
			if (this.Wall != null) {
				faktor = this.Wall.Construction.Factor * EN1264.Instance.HithermBeplankungsFaktor(HithermProduct.ConfigBeplankungRWerte, HithermProduct.ConfigBeplankungFaktoren, this.Wall.DeckschichtValue);
			}
			faktor = faktor * alpha / Product.ConfigAlphaWandCool * HithermProduct.ConfigLeistungsFaktorKuehlen;
			return EN1264.Instance.KaeltestromDichteRegister(kuehlmittelTemp, roomTemp, this.IsHochleistungsRegister ? HithermProduct.ConfigHlRegKuehlleistung : HithermProduct.ConfigStdRegKuehlleistung, faktor) * this.HeatArea;
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
				verlust = EN1264.Instance.WaermeverlustAussen(leistung, this.Wall.Construction.RValue + this.Wall.DeckschichtValue + 1.0 / alphaInnen, HithermProduct.ConfigDefaultDaemmung + this.Wall.AdditionalInsulationValue + 1.0 / alphaAussen, roomTemp, this.Wall.TempBehindCool);
			}
			return verlust;
		}

		public double Druckverlust(double massenstrom) {
			return EN1264.Instance.DruckverlustRegister(this.registerType, this.RegisterBreite, massenstrom) +
				EN1264.Instance.DruckverlustRohr(massenstrom, HithermProduct.ConfigVerbindeLeitungInnenquerschnitt, HithermProduct.ConfigRho, HithermProduct.ConfigVerbindeLeitungInnendurchmesser, HithermProduct.ConfigV, 0.000004, this.PipeVertical + this.PipeHorizontal);
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

		[XmlIgnore]
		public string PartNumber {
			get {
				switch (registerType) {
					case HithermRegisterTypeEnum.HIT_50_10:
						if (HithermProduct.ConfigUsePlus) {
							return "";
						} else {
							return "HI06";
						}
					case HithermRegisterTypeEnum.HIT_50_5:
						if (HithermProduct.ConfigUsePlus) {
							return "HP05";
						} else {
							return "HI05";
						}
					case HithermRegisterTypeEnum.HIT_100_10:
						if (HithermProduct.ConfigUsePlus) {
							return "";
						} else {
							return "HI11";
						}
					case HithermRegisterTypeEnum.HIT_100_5:
						if (HithermProduct.ConfigUsePlus) {
							return "HP10";
						} else {
							return "HI10";
						}
					case HithermRegisterTypeEnum.HIT_150_10:
						if (HithermProduct.ConfigUsePlus) {
							return "";
						} else {
							return "HI16";
						}
					case HithermRegisterTypeEnum.HIT_150_5:
						if (HithermProduct.ConfigUsePlus) {
							return "HP15";
						} else {
							return "HI15";
						}
					case HithermRegisterTypeEnum.HIT_200_10:
						if (HithermProduct.ConfigUsePlus) {
							return "";
						} else {
							return "HI21";
						}
					case HithermRegisterTypeEnum.HIT_200_5:
						if (HithermProduct.ConfigUsePlus) {
							return "HP20";
						} else {
							return "HI20";
						}
					case HithermRegisterTypeEnum.HIT_250_10:
						if (HithermProduct.ConfigUsePlus) {
							return "";
						} else {
							return "HI26";
						}
					case HithermRegisterTypeEnum.HIT_250_5:
						if (HithermProduct.ConfigUsePlus) {
							return "HP25";
						} else {
							return "HI25";
						}
					case HithermRegisterTypeEnum.HIT_300_10:
						if (HithermProduct.ConfigUsePlus) {
							return "";
						} else {
							return "HI31";
						}
					case HithermRegisterTypeEnum.HIT_300_5:
						if (HithermProduct.ConfigUsePlus) {
							return "HP30";
						} else {
							return "HI30";
						}

					default:
						throw new Exception("Unknown Register Type");
				}
			}
		}

		[XmlIgnore]
		public double WasserInhaltProRohr {
			get {
				switch (registerType) {
					case HithermRegisterTypeEnum.HIT_50_10:
							return 0.25 / 5;
					case HithermRegisterTypeEnum.HIT_50_5:
							return 0.34 / 9;
					case HithermRegisterTypeEnum.HIT_100_10:
							return 0.35 / 5;
					case HithermRegisterTypeEnum.HIT_100_5:
							return 0.52 / 9;
					case HithermRegisterTypeEnum.HIT_150_10:
							return 0.45 / 5;
					case HithermRegisterTypeEnum.HIT_150_5:
							return 0.69 / 9;
					case HithermRegisterTypeEnum.HIT_200_10:
							return 0.55 / 5;
					case HithermRegisterTypeEnum.HIT_200_5:
							return 0.86 / 9;
					case HithermRegisterTypeEnum.HIT_250_10:
							return 0.65 / 5;
					case HithermRegisterTypeEnum.HIT_250_5:
							return 1.04 / 9;
					case HithermRegisterTypeEnum.HIT_300_10:
							return 0.75 / 5;
					case HithermRegisterTypeEnum.HIT_300_5:
							return 1.21 / 9;
					default:
						throw new Exception("Unknown Register Type");
				}
			}
		}

		public String GraphWallId {
			get { return this.graphWallId; }
			set { this.graphWallId = value; }
		}

		public double GraphPosX {
			get { return this.graphPosX; }
			set { this.graphPosX = value; }
		}

		public double GraphPosY {
			get { return this.graphPosY; }
			set { this.graphPosY = value; }
		}

		public bool GraphVorlaufRight {
			get { return this.graphVorlaufRight; }
			set { this.graphVorlaufRight = value; }
		}
	}
}
