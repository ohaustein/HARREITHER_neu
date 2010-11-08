using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Xml.Serialization;
using System.Threading;

namespace Europlan.Common {
	public class KlimaFlaechenModul {
		private static double module_100_40_height = 1.0;
		private static double module_100_30_height = 1.0;
		private static double module_120_30_height = 1.2;
		private static double module_80_30_height = 0.8;
		private static double module_60_60_height = 0.6;

		private static double module_100_40_width = 0.4;
		private static double module_100_30_width = 0.3;
		private static double module_120_30_width = 0.3;
		private static double module_80_30_width = 0.3;
		private static double module_60_60_width = 0.6;

		private static double module_additional_width = 0.03 * 2;

		private static double module_100_40_floor_heatarea = module_100_40_height * module_100_40_width;
		private static double module_100_40_roof_heatarea = module_100_40_height * (module_100_40_width + module_additional_width);
		private static double module_100_30_heatarea = module_100_30_height * (module_100_30_width + module_additional_width);
		private static double module_120_30_heatarea = module_120_30_height * (module_120_30_width + module_additional_width);
		private static double module_80_30_heatarea = module_80_30_height * (module_80_30_width + module_additional_width);
		private static double module_60_60_heatarea = module_60_60_height * module_60_60_width;

		/*private static double module_100_40_area = 1.0 * 0.4;
		private static double module_100_30_area = 1.0 * 0.3;
		private static double module_120_30_area = 1.2 * 0.3;
		private static double module_80_30_area = 0.8 * 0.3;
		private static double module_60_60_area = 0.6 * 0.6;*/
		private static double module_100_40_floor_area = module_100_40_floor_heatarea;
		private static double module_100_40_roof_area = module_100_40_roof_heatarea;
		private static double module_100_30_area = module_100_30_heatarea;
		private static double module_120_30_area = module_120_30_heatarea;
		private static double module_80_30_area = module_80_30_heatarea;
		private static double module_60_60_area = module_60_60_heatarea;

		public class ModulTypeEnumConverter : System.ComponentModel.TypeConverter {
			private static readonly string modul_100_40 = EuroplanRes.KlimaFlaechenModul_100_40; //"Modul 100/40";
			private static readonly string modul_100_30 = EuroplanRes.KlimaFlaechenModul_100_30; //"Modul 100/30";
			private static readonly string modul_120_30 = EuroplanRes.KlimaFlaechenModul_120_30; //"Modul 120/30";
			private static readonly string modul_80_30 = EuroplanRes.KlimaFlaechenModul_80_30; //"Modul 80/30";
			private static readonly string modul_60_60 = EuroplanRes.KlimaFlaechenModul_60_60; //"Modul 60/60";
			private static readonly string modul_100_40_short = EuroplanRes.KlimaFlaechenModul_100_40_Short; //"100/40";
			private static readonly string modul_100_30_short = EuroplanRes.KlimaFlaechenModul_100_30_Short; //"100/30";
			private static readonly string modul_120_30_short = EuroplanRes.KlimaFlaechenModul_120_30_Short; //"120/30";
			private static readonly string modul_80_30_short = EuroplanRes.KlimaFlaechenModul_80_30_Short; //"80/30";
			private static readonly string modul_60_60_short = EuroplanRes.KlimaFlaechenModul_60_60_Short; //"60/60";

			private Dictionary<string, ModulTypeEnum> mappingFromString = new Dictionary<string, ModulTypeEnum>();
			private Dictionary<ModulTypeEnum, string> mappingToString = new Dictionary<ModulTypeEnum, string>();
			private Dictionary<string, ModulTypeEnum> mappingFromShortString = new Dictionary<string, ModulTypeEnum>();
			private Dictionary<ModulTypeEnum, string> mappingToShortString = new Dictionary<ModulTypeEnum, string>();

			private bool shortNames = false;

			public ModulTypeEnumConverter() {
				this.Initialize();
			}

			public ModulTypeEnumConverter(bool shortNames) {
				this.shortNames = shortNames;
				this.Initialize();
			}

			private void Initialize() {
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
				mappingFromShortString.Add(modul_100_40_short, ModulTypeEnum.MODUL_100_40);
				mappingFromShortString.Add(modul_100_30_short, ModulTypeEnum.MODUL_100_30);
				mappingFromShortString.Add(modul_120_30_short, ModulTypeEnum.MODUL_120_30);
				mappingFromShortString.Add(modul_80_30_short, ModulTypeEnum.MODUL_80_30);
				mappingFromShortString.Add(modul_60_60_short, ModulTypeEnum.MODUL_60_60);
				mappingToShortString.Add(ModulTypeEnum.MODUL_100_40, modul_100_40_short);
				mappingToShortString.Add(ModulTypeEnum.MODUL_100_30, modul_100_30_short);
				mappingToShortString.Add(ModulTypeEnum.MODUL_120_30, modul_120_30_short);
				mappingToShortString.Add(ModulTypeEnum.MODUL_80_30, modul_80_30_short);
				mappingToShortString.Add(ModulTypeEnum.MODUL_60_60, modul_60_60_short);
			}

			public bool ShortNames {
				get { return this.shortNames; }
				set { this.shortNames = value; }
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
						return mappingFromShortString[(string)value];
					}
				}
				return base.ConvertFrom(context, culture, value);
			}

			public override object ConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType) {
				if (value is ModulTypeEnum && destinationType == typeof(string)) {
					if (this.shortNames) {
						if (mappingToShortString.ContainsKey((ModulTypeEnum)value)) {
							return mappingToShortString[(ModulTypeEnum)value];
						}
					} else {
						if (mappingToString.ContainsKey((ModulTypeEnum)value)) {
							return mappingToString[(ModulTypeEnum)value];
						}
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
			private static readonly string left = EuroplanRes.KlimaFlaechenModul_AusrichtungLinks; //"Links";
			private static readonly string right = EuroplanRes.KlimaFlaechenModul_AusrichtungRechts; //"Rechts";
			
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

		// position in graphical mode
		private int graphLane = -1;
		private double graphPositionInLane = double.NaN;
		private bool graphBottomUp = false;

		public KlimaFlaechenModul() {
			this.modulType = ModulTypeEnum.MODUL_100_40;
			this.orientation = ModulOrientationEnum.ORIENTATION_LEFT;
		}

		public KlimaFlaechenModul(ModulTypeEnum modulType, ModulOrientationEnum orientation)  {
			this.modulType = modulType;
			this.orientation = orientation;
		}

		public KlimaFlaechenModul(KlimaFlaechenModul other) {
			this.modulType = other.modulType;
			this.orientation = other.orientation;
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

		//[XmlIgnore]
		public double GetHeatArea(bool floor) {
			//get {
				switch (this.modulType) {
					case ModulTypeEnum.MODUL_100_40:
						return floor ? KlimaFlaechenModul.module_100_40_floor_heatarea : KlimaFlaechenModul.module_100_40_roof_heatarea;

					case ModulTypeEnum.MODUL_80_30:
						return KlimaFlaechenModul.module_80_30_heatarea;

					case ModulTypeEnum.MODUL_60_60:
						return KlimaFlaechenModul.module_60_60_heatarea;

					case ModulTypeEnum.MODUL_100_30:
						return KlimaFlaechenModul.module_100_30_heatarea;

					case ModulTypeEnum.MODUL_120_30:
						return KlimaFlaechenModul.module_120_30_heatarea;

					default:
						return 0;
				}
			//}
			}

		//[XmlIgnore]
		public double GetCoveredArea(bool floor) {
			//get {
				switch (this.modulType) {
					case ModulTypeEnum.MODUL_100_40:
						return floor ? KlimaFlaechenModul.module_100_40_floor_area : KlimaFlaechenModul.module_100_40_roof_area;

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
			//}
			}

		public double Druckverlust(double massenstrom) {
			switch (this.modulType) {
				case ModulTypeEnum.MODUL_100_40:
					return EN1264.Instance.DruckverlustModul_100_40(1, massenstrom);

				case ModulTypeEnum.MODUL_80_30:
					return EN1264.Instance.DruckverlustModul_80_30(1, massenstrom);

				case ModulTypeEnum.MODUL_100_30:
					return EN1264.Instance.DruckverlustModul_100_30(1, massenstrom);

				case ModulTypeEnum.MODUL_120_30:
					return EN1264.Instance.DruckverlustModul_120_30(1, massenstrom);

				case ModulTypeEnum.MODUL_60_60:
					return EN1264.Instance.DruckverlustModul_120_30(1, massenstrom);

				default:
					return 0;
			}
		}

		public static double GetModuleHeight(ModulTypeEnum type) {
			switch (type) {
				case ModulTypeEnum.MODUL_100_40:
					return module_100_40_height;

				case ModulTypeEnum.MODUL_100_30:
					return module_100_30_height;

				case ModulTypeEnum.MODUL_120_30:
					return module_120_30_height;

				case ModulTypeEnum.MODUL_80_30:
					return module_80_30_height;

				case ModulTypeEnum.MODUL_60_60:
					return module_60_60_height;

				default:
					return 0;
			}
		}

		public static double GetModuleWidth(ModulTypeEnum type) {
			switch (type) {
				case ModulTypeEnum.MODUL_100_40:
					return module_100_40_width;

				case ModulTypeEnum.MODUL_100_30:
					return module_100_30_width;

				case ModulTypeEnum.MODUL_120_30:
					return module_120_30_width;

				case ModulTypeEnum.MODUL_80_30:
					return module_80_30_width;

				case ModulTypeEnum.MODUL_60_60:
					return module_60_60_width;

				default:
					return 0;
			}
		}

		public int GraphLane {
			get { return this.graphLane; }
			set { this.graphLane = value; }
		}

		public double GraphPositionInLan {
			get { return this.graphPositionInLane; }
			set { this.graphPositionInLane = value; }
		}

		public bool GraphBottomUp {
			get { return this.graphBottomUp; }
			set { this.graphBottomUp = value; }
		}

		public double GraphBottomPositionInLane(double measure) {
			return this.graphPositionInLane + measure * KlimaFlaechenModul.GetModuleHeight(this.modulType);
		}
	}
}
