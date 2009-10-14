using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Europlan.Common {
	public class ConnectionPipe {

		public class PipeTypeEnumConverter : System.ComponentModel.TypeConverter {
			private static readonly string none = "";
			private static readonly string euroval = "Euroval FBH 24/17";
			private static readonly string rundrohr = "21mm Rundrohr";

			private Dictionary<string, PipeTypeEnum> mappingFromString = new Dictionary<string, PipeTypeEnum>();
			private Dictionary<PipeTypeEnum, string> mappingToString = new Dictionary<PipeTypeEnum, string>();

			public PipeTypeEnumConverter() {
				mappingFromString.Add(none, PipeTypeEnum.PT_NONE);
				mappingFromString.Add(euroval, PipeTypeEnum.PT_EUROVAL);
				mappingFromString.Add(rundrohr, PipeTypeEnum.PT_21MM);
				mappingToString.Add(PipeTypeEnum.PT_NONE, none);
				mappingToString.Add(PipeTypeEnum.PT_EUROVAL, euroval);
				mappingToString.Add(PipeTypeEnum.PT_21MM, rundrohr);
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
				if (value is PipeTypeEnum && destinationType == typeof(string)) {
					if (mappingToString.ContainsKey((PipeTypeEnum)value)) {
						return mappingToString[(PipeTypeEnum)value];
					}
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}
		}

		[System.ComponentModel.TypeConverter(typeof(PipeTypeEnumConverter))]
		public enum PipeTypeEnum {
			PT_NONE,
			PT_EUROVAL,
			PT_21MM
		}

		public class VerlegeartEnumConverter : System.ComponentModel.TypeConverter {
			private static readonly string none = "";
			private static readonly string unterEstrich = "unter Estrich";
			private static readonly string ev35 = "EV35";
			private static readonly string ev30 = "EV30";
			private static readonly string ev25 = "EV25";
			private static readonly string ev20 = "EV20";
			private static readonly string ev15 = "EV15";
			private static readonly string ev10 = "EV10";
			private static readonly string ev5 = "EV5";
			private static readonly string a5 = "A5";

			private Dictionary<string, VerlegeartEnum> mappingFromString = new Dictionary<string, VerlegeartEnum>();
			private Dictionary<VerlegeartEnum, string> mappingToString = new Dictionary<VerlegeartEnum, string>();

			public VerlegeartEnumConverter() {
				mappingFromString.Add(none, VerlegeartEnum.VA_NONE);
				mappingFromString.Add(unterEstrich, VerlegeartEnum.VA_UNTER_ESTRICH);
				mappingFromString.Add(ev35, VerlegeartEnum.VA_EV35);
				mappingFromString.Add(ev30, VerlegeartEnum.VA_EV30);
				mappingFromString.Add(ev25, VerlegeartEnum.VA_EV25);
				mappingFromString.Add(ev20, VerlegeartEnum.VA_EV20);
				mappingFromString.Add(ev15, VerlegeartEnum.VA_EV15);
				mappingFromString.Add(ev10, VerlegeartEnum.VA_EV10);
				mappingFromString.Add(ev5, VerlegeartEnum.VA_EV5);
				mappingFromString.Add(a5, VerlegeartEnum.VA_A5);
				mappingToString.Add(VerlegeartEnum.VA_NONE, none);
				mappingToString.Add(VerlegeartEnum.VA_UNTER_ESTRICH, unterEstrich);
				mappingToString.Add(VerlegeartEnum.VA_EV35, ev35);
				mappingToString.Add(VerlegeartEnum.VA_EV30, ev30);
				mappingToString.Add(VerlegeartEnum.VA_EV25, ev25);
				mappingToString.Add(VerlegeartEnum.VA_EV20, ev20);
				mappingToString.Add(VerlegeartEnum.VA_EV15, ev15);
				mappingToString.Add(VerlegeartEnum.VA_EV10, ev10);
				mappingToString.Add(VerlegeartEnum.VA_EV5, ev5);
				mappingToString.Add(VerlegeartEnum.VA_A5, a5);
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
				if (value is VerlegeartEnum && destinationType == typeof(string)) {
					if (mappingToString.ContainsKey((VerlegeartEnum)value)) {
						return mappingToString[(VerlegeartEnum)value];
					}
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}
		}

		[System.ComponentModel.TypeConverter(typeof(VerlegeartEnumConverter))]
		public enum VerlegeartEnum {
			VA_NONE,
			VA_UNTER_ESTRICH,
			VA_EV35,
			VA_EV30,
			VA_EV25,
			VA_EV20,
			VA_EV15,
			VA_EV10,
			VA_EV5,
			VA_A5
		}

		public class InsulationEnumConverter : System.ComponentModel.TypeConverter {
			private static readonly string none = "keine";
			private static readonly string vl = "VL";
			private static readonly string vlrl = "VL+RL";

			private Dictionary<string, InsulationEnum> mappingFromString = new Dictionary<string, InsulationEnum>();
			private Dictionary<InsulationEnum, string> mappingToString = new Dictionary<InsulationEnum, string>();

			public InsulationEnumConverter() {
				mappingFromString.Add(none, InsulationEnum.IN_NONE);
				mappingFromString.Add(vl, InsulationEnum.IN_VL);
				mappingFromString.Add(vlrl, InsulationEnum.IN_VL_RL);
				mappingToString.Add(InsulationEnum.IN_NONE, none);
				mappingToString.Add(InsulationEnum.IN_VL, vl);
				mappingToString.Add(InsulationEnum.IN_VL_RL, vlrl);
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
				if (value is InsulationEnum && destinationType == typeof(string)) {
					if (mappingToString.ContainsKey((InsulationEnum)value)) {
						return mappingToString[(InsulationEnum)value];
					}
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}
		}

		[System.ComponentModel.TypeConverter(typeof(InsulationEnumConverter))]
		public enum InsulationEnum {
			IN_NONE,
			IN_VL,
			IN_VL_RL
		}

		private double vorlauf;
		private double ruecklauf;
		private Room room;
		private string roomId = null;
		private bool print;
		private bool onlyFirst;
		private PipeTypeEnum pipeType;
		private VerlegeartEnum verlegeart;
		private InsulationEnum insulation;

		public ConnectionPipe() {
			this.vorlauf = 0;
			this.ruecklauf = 0;
			this.room = null;
			this.print = false;
			this.onlyFirst = false;
			this.pipeType = PipeTypeEnum.PT_NONE;
			this.verlegeart = VerlegeartEnum.VA_NONE;
			this.insulation = InsulationEnum.IN_NONE;
		}

		public double Vorlauf {
			get { return this.vorlauf; }
			set { this.vorlauf = value; }
		}

		public double Ruecklauf {
			get { return this.ruecklauf; }
			set { this.ruecklauf = value; }
		}

		[XmlIgnore]
		public Room Room {
			get {
				if (this.roomId != null) {
					foreach (Floor f in Project.Instance.Floors) {
						foreach (Room r in f.Rooms) {
							if (r.Id == this.roomId) {
								this.room = r;
							}
						}
					}
				}
				return this.room;
			}
			set {
				this.roomId = null;
				this.room = value;
			}
		}

		public string RoomId {
			get { return this.room == null ? null : this.room.Id; }
			set {
				if (value == null) {
					this.room = null;
				}
				this.roomId = value;
			}
		}

		public bool Print {
			get { return this.print; }
			set { this.print = value; }
		}

		public bool OnlyFirst {
			get { return this.onlyFirst; }
			set { this.onlyFirst = value; }
		}

		public PipeTypeEnum PipeType {
			get { return this.pipeType; }
			set { this.pipeType = value; }
		}

		public VerlegeartEnum Verlegeart {
			get { return this.verlegeart; }
			set { this.verlegeart = value; }
		}

		public InsulationEnum Insulation {
			get { return this.insulation; }
			set { this.insulation = value; }
		}
	}
}
