using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Xml.Serialization;

namespace Europlan.Common {
	public class HithermRegister {
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
				mappingFromString.Add(hit_50_10, RegisterTypeEnum.HIT_50_5);
				mappingFromString.Add(hit_100_10, RegisterTypeEnum.HIT_100_5);
				mappingFromString.Add(hit_150_10, RegisterTypeEnum.HIT_150_5);
				mappingFromString.Add(hit_200_10, RegisterTypeEnum.HIT_200_5);
				mappingFromString.Add(hit_250_10, RegisterTypeEnum.HIT_250_5);
				mappingFromString.Add(hit_300_10, RegisterTypeEnum.HIT_300_5);

				mappingToString.Add(RegisterTypeEnum.HIT_50_10, hit_50_10);
				mappingToString.Add(RegisterTypeEnum.HIT_100_10, hit_100_10);
				mappingToString.Add(RegisterTypeEnum.HIT_150_10, hit_150_10);
				mappingToString.Add(RegisterTypeEnum.HIT_200_10, hit_200_10);
				mappingToString.Add(RegisterTypeEnum.HIT_250_10, hit_250_10);
				mappingToString.Add(RegisterTypeEnum.HIT_300_10, hit_300_10);
				mappingToString.Add(RegisterTypeEnum.HIT_50_10, hit_50_10);
				mappingToString.Add(RegisterTypeEnum.HIT_100_10, hit_100_10);
				mappingToString.Add(RegisterTypeEnum.HIT_150_10, hit_150_10);
				mappingToString.Add(RegisterTypeEnum.HIT_200_10, hit_200_10);
				mappingToString.Add(RegisterTypeEnum.HIT_250_10, hit_250_10);
				mappingToString.Add(RegisterTypeEnum.HIT_300_10, hit_300_10);
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

		private RegisterTypeEnum registerType;
		private RegisterOrientationEnum orientation;
		private int rohre;

		/*private Nullable<Point> origin = null;*/

		public HithermRegister() {
			this.registerType = RegisterTypeEnum.HIT_50_10;
			this.orientation = RegisterOrientationEnum.ORIENTATION_HORIZONTAL;
			this.rohre = 1;
		}

		public HithermRegister(RegisterTypeEnum registerType, RegisterOrientationEnum orientation, int rohre) {
			this.registerType = registerType;
			this.orientation = orientation;
			this.rohre = rohre;
		}

		public RegisterTypeEnum RegisterType {
			get { return this.registerType; }
			set { this.registerType = value; }
		}

		public RegisterOrientationEnum Orientation {
			get { return this.orientation; }
			set { this.orientation = value; }
		}

		public int Rohre {
			get { return this.rohre; }
			set { this.rohre = value; }
		}

		[XmlIgnore]
		public int RegisterBreite {
			get { return 0; /* TODO */ }
		}

		/*public Nullable<Point> Origin {
			get { return this.origin; }
			set { this.origin = value; }
		}*/

		[XmlIgnore]
		public double Area {
			get {
				//switch (this.modulType) {
				//    case ModulTypeEnum.MODUL_100_40:
				//        return KlimaFlaechenModul.module_100_40_area;

				//    case ModulTypeEnum.MODUL_80_30:
				//        return KlimaFlaechenModul.module_80_30_area;

				//    case ModulTypeEnum.MODUL_100_30:
				//        return KlimaFlaechenModul.module_100_30_area;

				//    case ModulTypeEnum.MODUL_120_30:
				//        return KlimaFlaechenModul.module_120_30_area;

				//    default:
				//        return 0;
				//}
				// TODO
				return 0;
			}
		}

		public double Druckverlust(double durchfluss) {
			//switch (this.modulType) {
			//    case ModulTypeEnum.MODUL_100_40:
			//        return EN1264.Instance.DruckverlustModul_100_40(1, durchfluss);

			//    case ModulTypeEnum.MODUL_80_30:
			//        return EN1264.Instance.DruckverlustModul_80_30(1, durchfluss);

			//    case ModulTypeEnum.MODUL_100_30:
			//        return EN1264.Instance.DruckverlustModul_100_30(1, durchfluss);

			//    case ModulTypeEnum.MODUL_120_30:
			//        return EN1264.Instance.DruckverlustModul_120_30(1, durchfluss);

			//    default:
			//        return 0;
			//}
			// TODO
			return 0;
		}
	}
}
