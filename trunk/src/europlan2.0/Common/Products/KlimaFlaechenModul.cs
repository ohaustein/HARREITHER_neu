using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Xml.Serialization;

namespace Europlan.Common {
	public class KlimaFlaechenModul {
		private static double module_100_40_area = 1.0 * 0.4;
		private static double module_100_30_area = 1.0 * 0.3;
		private static double module_120_30_area = 1.2 * 0.3;
		private static double module_80_30_area = 0.8 * 0.3;
		private static double module_60_60_area = 0.6 * 0.6;

		public class ModulTypeEnumConverter : System.ComponentModel.TypeConverter {
			private static readonly string modul_100_40 = "Modul 100/40";
			private static readonly string modul_100_30 = "Modul 100/30";
			private static readonly string modul_120_30 = "Modul 120/30";
			private static readonly string modul_80_30 = "Modul 80/30";
			private static readonly string modul_60_60 = "Modul 60/60";
			
			private Dictionary<string, ModulTypeEnum> mappingFromString = new Dictionary<string, ModulTypeEnum>();
			private Dictionary<ModulTypeEnum, string> mappingToString = new Dictionary<ModulTypeEnum, string>();

			public ModulTypeEnumConverter() {
				mappingFromString.Add(modul_100_40, ModulTypeEnum.MODUL_100_40);
				mappingFromString.Add(modul_100_30, ModulTypeEnum.MODUL_100_30);
				mappingFromString.Add(modul_120_30, ModulTypeEnum.MODUL_120_30);
				mappingFromString.Add(modul_80_30, ModulTypeEnum.MODUL_80_30);
				mappingFromString.Add(modul_60_60, ModulTypeEnum.MODUL_60_60);
				mappingToString.Add(ModulTypeEnum.MODUL_100_40, modul_100_40);
				mappingToString.Add(ModulTypeEnum.MODUL_100_30, modul_100_30);
				mappingToString.Add(ModulTypeEnum.MODUL_120_30, modul_120_30);
				mappingToString.Add(ModulTypeEnum.MODUL_80_30, modul_80_30);
				mappingToString.Add(ModulTypeEnum.MODUL_60_60, modul_60_60);
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
				if (value is ModulTypeEnum && destinationType == typeof(string)) {
					if (mappingToString.ContainsKey((ModulTypeEnum)value)) {
						return mappingToString[(ModulTypeEnum)value];
					}
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}
		}

		[System.ComponentModel.TypeConverter(typeof(ModulTypeEnumConverter))]
		public enum ModulTypeEnum {
			MODUL_100_40,
			MODUL_100_30,
			MODUL_120_30,
			MODUL_80_30,
			MODUL_60_60
		}

		public class ModulOrientationEnumConverter : System.ComponentModel.TypeConverter {
			private static readonly string left = "Links";
			private static readonly string right = "Rechts";
			
			private Dictionary<string, ModulOrientationEnum> mappingFromString = new Dictionary<string, ModulOrientationEnum>();
			private Dictionary<ModulOrientationEnum, string> mappingToString = new Dictionary<ModulOrientationEnum, string>();

			public ModulOrientationEnumConverter() {
				mappingFromString.Add(left, ModulOrientationEnum.ORIENTATION_LEFT);
				mappingFromString.Add(right, ModulOrientationEnum.ORIENTATION_RIGHT);
				mappingToString.Add(ModulOrientationEnum.ORIENTATION_LEFT, left);
				mappingToString.Add(ModulOrientationEnum.ORIENTATION_RIGHT, right);
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
				if (value is ModulOrientationEnum && destinationType == typeof(string)) {
					if (mappingToString.ContainsKey((ModulOrientationEnum)value)) {
						return mappingToString[(ModulOrientationEnum)value];
					}
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}
		}

		[System.ComponentModel.TypeConverter(typeof(ModulOrientationEnumConverter))]
		public enum ModulOrientationEnum {
			ORIENTATION_LEFT,
			ORIENTATION_RIGHT
		}

		private ModulTypeEnum modulType;
		private ModulOrientationEnum orientation;

		/*private Nullable<Point> origin = null;*/

		public KlimaFlaechenModul() {
			this.modulType = ModulTypeEnum.MODUL_100_40;
			this.orientation = ModulOrientationEnum.ORIENTATION_LEFT;
		}

		public KlimaFlaechenModul(ModulTypeEnum modulType, ModulOrientationEnum orientation)  {
			this.modulType = modulType;
			this.orientation = orientation;
		}

		public ModulTypeEnum ModulType {
			get { return this.modulType; }
			set { this.modulType = value; }
		}

		public ModulOrientationEnum Orientation {
			get { return this.orientation; }
			set { this.orientation = value; }
		}

		public string PartNumber {
			get {
				switch (this.modulType) {
					case ModulTypeEnum.MODUL_100_40:
						if (orientation == ModulOrientationEnum.ORIENTATION_RIGHT) {
							return "MK01";
						} else {
							return "MK02";
						}

					case ModulTypeEnum.MODUL_80_30:
						if (orientation == ModulOrientationEnum.ORIENTATION_RIGHT) {
							return "MK34";
						} else {
							return "MK35";
						}

					case ModulTypeEnum.MODUL_60_60:
						if (orientation == ModulOrientationEnum.ORIENTATION_RIGHT) {
							return "MK40";
						} else {
							return "MK40";
						}

					case ModulTypeEnum.MODUL_100_30:
						if (orientation == ModulOrientationEnum.ORIENTATION_RIGHT) {
							return "MK32";
						} else {
							return "MK33";
						}

					case ModulTypeEnum.MODUL_120_30:
						if (orientation == ModulOrientationEnum.ORIENTATION_RIGHT) {
							return "MK30";
						} else {
							return "MK31";
						}

					default:
						return "";
				}
			}
		}

		[XmlIgnore]
		public double WasserInhalt {
			get {
				switch (this.modulType) {
					case ModulTypeEnum.MODUL_100_40:
						return 1.0;

					case ModulTypeEnum.MODUL_80_30:
						return 0.6;

					case ModulTypeEnum.MODUL_120_30:
					case ModulTypeEnum.MODUL_60_60:
						return 0.9;

					case ModulTypeEnum.MODUL_100_30:
						return 0.75;

					default:
						throw new Exception("Unknown Register Type");
				}
			}
		}

		/*public Nullable<Point> Origin {
			get { return this.origin; }
			set { this.origin = value; }
		}*/

		[XmlIgnore]
		public double Area {
			get {
				switch (this.modulType) {
					case ModulTypeEnum.MODUL_100_40:
						return KlimaFlaechenModul.module_100_40_area;

					case ModulTypeEnum.MODUL_80_30:
						return KlimaFlaechenModul.module_80_30_area;

					case ModulTypeEnum.MODUL_60_60:
						return KlimaFlaechenModul.module_60_60_area;

					case ModulTypeEnum.MODUL_100_30:
						return KlimaFlaechenModul.module_100_30_area;

					case ModulTypeEnum.MODUL_120_30:
						return KlimaFlaechenModul.module_120_30_area;

					default:
						return 0;
				}
			}
		}

		public double Druckverlust(double durchfluss) {
			switch (this.modulType) {
				case ModulTypeEnum.MODUL_100_40:
					return EN1264.Instance.DruckverlustModul_100_40(1, durchfluss);

				case ModulTypeEnum.MODUL_80_30:
					return EN1264.Instance.DruckverlustModul_80_30(1, durchfluss);

				case ModulTypeEnum.MODUL_100_30:
					return EN1264.Instance.DruckverlustModul_100_30(1, durchfluss);

				case ModulTypeEnum.MODUL_120_30:
					return EN1264.Instance.DruckverlustModul_120_30(1, durchfluss);

				case ModulTypeEnum.MODUL_60_60:
					return EN1264.Instance.DruckverlustModul_120_30(1, durchfluss);

				default:
					return 0;
			}
		}
	}
}
