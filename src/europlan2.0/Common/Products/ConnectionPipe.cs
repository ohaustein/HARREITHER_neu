using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Threading;

namespace Europlan.Common {
	public class ConnectionPipe {

		public class PipeTypeEnumConverter : System.ComponentModel.TypeConverter {
			private static readonly string euroval = EuroplanRes.ConnectionPipe_Euroval; //"Euroval FBH 24/17"
			private static readonly string rundrohr = EuroplanRes.ConnectionPipe_Rundrohr; //"21mm Rundrohr"
			private static readonly string ecotherm = EuroplanRes.ConnectionPipe_Ecotherm; //"Ecotherm FBH"
			private static readonly string jumboval = EuroplanRes.ConnectionPipe_Jumboval; //"Jumboval FBH"

			private Dictionary<string, PipeTypeEnum> mappingFromString = new Dictionary<string, PipeTypeEnum>();
			private Dictionary<PipeTypeEnum, string> mappingToString = new Dictionary<PipeTypeEnum, string>();

			public PipeTypeEnumConverter() {
				mappingFromString.Add(euroval, PipeTypeEnum.PT_EUROVAL);
				mappingFromString.Add(rundrohr, PipeTypeEnum.PT_21MM);
				mappingFromString.Add(ecotherm, PipeTypeEnum.PT_ECOTHERM);
				mappingFromString.Add(jumboval, PipeTypeEnum.PT_JUMBOVAL);
				mappingToString.Add(PipeTypeEnum.PT_EUROVAL, euroval);
				mappingToString.Add(PipeTypeEnum.PT_21MM, rundrohr);
				mappingToString.Add(PipeTypeEnum.PT_ECOTHERM, ecotherm);
				mappingToString.Add(PipeTypeEnum.PT_JUMBOVAL, jumboval);
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
			PT_EUROVAL,
			PT_21MM,
			PT_ECOTHERM,
			PT_JUMBOVAL
		}

		public class VerlegeartEnumConverter : System.ComponentModel.TypeConverter {
			private static readonly string unterEstrich = EuroplanRes.ConnectionPipe_UnterEstrich; //"unter Estrich"
			private static readonly string ev35 = EuroplanRes.EurovalProduct_EV35; //"EV35"
			private static readonly string ev30 = EuroplanRes.EurovalProduct_EV30; //"EV30"
			private static readonly string ev25 = EuroplanRes.EurovalProduct_EV25; //"EV25"
			private static readonly string ev20 = EuroplanRes.EurovalProduct_EV20; //"EV20"
			private static readonly string ev15 = EuroplanRes.EurovalProduct_EV15; //"EV15"
			private static readonly string ev10 = EuroplanRes.EurovalProduct_EV10; //"EV10"
			private static readonly string ev5 = EuroplanRes.EurovalProduct_EV5; //"EV5"
			private static readonly string a5 = EuroplanRes.EurovalProduct_A5; //"A5"
            private static readonly string jv50 = EuroplanRes.JumbovalProduct_JV50;
            private static readonly string jv40 = EuroplanRes.JumbovalProduct_JV40;
            private static readonly string jv30 = EuroplanRes.JumbovalProduct_JV30;
            private static readonly string jv20 = EuroplanRes.JumbovalProduct_JV20;

			private Dictionary<string, VerlegeartEnum> mappingFromString = new Dictionary<string, VerlegeartEnum>();
			private Dictionary<VerlegeartEnum, string> mappingToString = new Dictionary<VerlegeartEnum, string>();

			public VerlegeartEnumConverter() {
				mappingFromString.Add(unterEstrich, VerlegeartEnum.VA_UNTER_ESTRICH);
				mappingFromString.Add(ev35, VerlegeartEnum.VA_EV35);
				mappingFromString.Add(ev30, VerlegeartEnum.VA_EV30);
				mappingFromString.Add(ev25, VerlegeartEnum.VA_EV25);
				mappingFromString.Add(ev20, VerlegeartEnum.VA_EV20);
				mappingFromString.Add(ev15, VerlegeartEnum.VA_EV15);
				mappingFromString.Add(ev10, VerlegeartEnum.VA_EV10);
				mappingFromString.Add(ev5, VerlegeartEnum.VA_EV5);
				mappingFromString.Add(a5, VerlegeartEnum.VA_A5);
                mappingFromString.Add(jv50, VerlegeartEnum.VA_JV50);
                mappingFromString.Add(jv40, VerlegeartEnum.VA_JV40);
                mappingFromString.Add(jv30, VerlegeartEnum.VA_JV30);
                mappingFromString.Add(jv20, VerlegeartEnum.VA_JV20);
                mappingToString.Add(VerlegeartEnum.VA_UNTER_ESTRICH, unterEstrich);
				mappingToString.Add(VerlegeartEnum.VA_EV35, ev35);
				mappingToString.Add(VerlegeartEnum.VA_EV30, ev30);
				mappingToString.Add(VerlegeartEnum.VA_EV25, ev25);
				mappingToString.Add(VerlegeartEnum.VA_EV20, ev20);
				mappingToString.Add(VerlegeartEnum.VA_EV15, ev15);
				mappingToString.Add(VerlegeartEnum.VA_EV10, ev10);
				mappingToString.Add(VerlegeartEnum.VA_EV5, ev5);
				mappingToString.Add(VerlegeartEnum.VA_A5, a5);
                mappingToString.Add(VerlegeartEnum.VA_JV50, jv50);
                mappingToString.Add(VerlegeartEnum.VA_JV40, jv40);
                mappingToString.Add(VerlegeartEnum.VA_JV30, jv30);
                mappingToString.Add(VerlegeartEnum.VA_JV20, jv20);
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
			VA_UNTER_ESTRICH,
			VA_EV35,
			VA_EV30,
			VA_EV25,
			VA_EV20,
			VA_EV15,
			VA_EV10,
			VA_EV5,
			VA_A5,
            VA_JV50,
            VA_JV40,
            VA_JV30,
            VA_JV20
		}

		public class InsulationEnumConverter : System.ComponentModel.TypeConverter {
			private static readonly string none = EuroplanRes.ConnectionPipe_KeineDaemmung; //"keine"
			private static readonly string vl = EuroplanRes.ConnectionPipe_VorlaufGedaemmt; //"VL"
			private static readonly string vlrl = EuroplanRes.ConnectionPipe_Gedaemmt; //"VL+RL"

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
		private PlannedProduct product;
		private string productId = null;
		private bool print;
		private bool onlyFirst;
		private PipeTypeEnum pipeType;
		private VerlegeartEnum verlegeart;
		private InsulationEnum insulation;
		private bool generated = false;

		public ConnectionPipe() {
			this.vorlauf = 0;
			this.ruecklauf = 0;
			this.room = null;
			this.product = null;
			this.print = false;
			this.onlyFirst = false;
			this.pipeType = PipeTypeEnum.PT_EUROVAL;
			this.verlegeart = VerlegeartEnum.VA_EV5;
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

		public bool IsGenerated {
			get { return this.generated; }
			set { this.generated = value; }
		}

		[XmlIgnore]
		public Room Room {
			get {
				if (this.roomId != null) {
					foreach (Floor f in Project.Instance.Floors) {
						foreach (Room r in f.Rooms) {
							if (r.InternalId == this.roomId) {
								this.room = r;
								return this.room;
							}
						}
					}
				}
				return this.room;
			}
			set {
				bool roomChanged = this.Room != value;
				this.roomId = null;
				this.room = value;
				if (roomChanged) {
					this.ConnectionThrough = null;
				}
			}
		}

		public string RoomId {
			get { return this.room == null ? null : this.room.InternalId; }
			set {
				if (value == null) {
					this.room = null;
				}
				this.roomId = value;
			}
		}

		[XmlIgnore]
		public Room DestinationRoom {
			get {
				if (this.ConnectionOf != null) {
					return this.ConnectionOf.Product.AssociatedRoom;
				}
				return null;
			}
		}

		[XmlIgnore]
		public PlannedProduct ConnectionThrough {
			get {
				if (this.productId != null) {
					foreach (Floor f in Project.Instance.Floors) {
						foreach (Room r in f.Rooms) {
							foreach (PlannedProduct p in r.PlannedProducts) {
								if (p.Id == this.productId) {
									this.product = p;
									return this.product;
								}
							}
						}
					}
				} else if (this.product == null && !this.IsGenerated) {
					bool found = false;
					if (this.room != null) {
						int i = 0;
						while (!found && i < this.room.PlannedProducts.Count) {
							if (this.room.PlannedProducts[i] != this.ConnectionOf) {
								this.product = this.room.PlannedProducts[i];
								found = true;
							}
							i++;
						}
					}
				}
				return this.product;
			}
			set {
				this.productId = null;
				this.product = value;
			}
		}

		public string ProductId {
			get { return this.product == null ? null : this.product.Id; }
			set {
				if (value == null) {
					this.product = null;
				}
				this.productId = value;
			}
		}

		[XmlIgnore]
		public PlannedProduct ConnectionOf {
			get {
				foreach (Floor f in Project.Instance.Floors) {
					foreach (Room r in f.Rooms) {
						foreach (PlannedProduct pp in r.PlannedProducts) {
							foreach (ConnectionPipe cp in pp.Product.PlannedConnectionPipes) {
								if (cp == this) {
									return pp;
								}
							}
						}
					}
				}
				return null;
			}
		}

		[XmlIgnore]
		public int PlannedCircuits {
			get {
				if (this.onlyFirst) {
					return 1;
				}
				PlannedProduct connectionOf = this.ConnectionOf;
				if (connectionOf != null) {
					return connectionOf.Product.PlannedCircuitCount;
				}
				return 0;
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
			get {
				return this.verlegeart;
			}
			set { this.verlegeart = value; }
		}

		public InsulationEnum Insulation {
			get {
				if (this.Verlegeart == VerlegeartEnum.VA_UNTER_ESTRICH) {
					return InsulationEnum.IN_VL_RL;
				}
				return this.insulation;
			}
			set { this.insulation = value; }
		}

		[XmlIgnore]
		public InsulationEnum InsulationForCalculation {
			get { return this.ConnectionThrough == null ? InsulationEnum.IN_VL_RL : this.Insulation; }
		}

		[XmlIgnore]
		public double AreaTotal {
			get {
				double area = 0;
				switch (verlegeart) {
					case VerlegeartEnum.VA_UNTER_ESTRICH:
						area = 0;
						break;

					case VerlegeartEnum.VA_EV35:
						area = (this.vorlauf + this.ruecklauf) / EurovalProduct.GetPipeLengthPerSqm(EurovalProduct.EurovalLayDistance.EV35);
						break;

					case VerlegeartEnum.VA_EV30:
						area = (this.vorlauf + this.ruecklauf) / EurovalProduct.GetPipeLengthPerSqm(EurovalProduct.EurovalLayDistance.EV30);
						break;

					case VerlegeartEnum.VA_EV25:
						area = (this.vorlauf + this.ruecklauf) / EurovalProduct.GetPipeLengthPerSqm(EurovalProduct.EurovalLayDistance.EV25);
						break;

					case VerlegeartEnum.VA_EV20:
						area = (this.vorlauf + this.ruecklauf) / EurovalProduct.GetPipeLengthPerSqm(EurovalProduct.EurovalLayDistance.EV20);
						break;

					case VerlegeartEnum.VA_EV15:
						area = (this.vorlauf + this.ruecklauf) / EurovalProduct.GetPipeLengthPerSqm(EurovalProduct.EurovalLayDistance.EV15);
						break;

					case VerlegeartEnum.VA_EV10:
						area = (this.vorlauf + this.ruecklauf) / EurovalProduct.GetPipeLengthPerSqm(EurovalProduct.EurovalLayDistance.EV10);
						break;

					case VerlegeartEnum.VA_EV5:
						area = (this.vorlauf + this.ruecklauf) / EurovalProduct.GetPipeLengthPerSqm(EurovalProduct.EurovalLayDistance.EV5);
						break;

					case VerlegeartEnum.VA_A5:
						area = (this.vorlauf + this.ruecklauf) / EurovalProduct.GetPipeLengthPerSqm(EurovalProduct.EurovalLayDistance.A5);
						break;

                    case VerlegeartEnum.VA_JV50:
                        area = (this.vorlauf + this.ruecklauf) / JumbovalProduct.GetPipeLengthPerSqm(JumbovalProduct.JumbovalLayDistance.JV50);
                        break;

                    case VerlegeartEnum.VA_JV40:
                        area = (this.vorlauf + this.ruecklauf) / JumbovalProduct.GetPipeLengthPerSqm(JumbovalProduct.JumbovalLayDistance.JV40);
                        break;

                    case VerlegeartEnum.VA_JV30:
                        area = (this.vorlauf + this.ruecklauf) / JumbovalProduct.GetPipeLengthPerSqm(JumbovalProduct.JumbovalLayDistance.JV30);
                        break;

                    case VerlegeartEnum.VA_JV20:
                        area = (this.vorlauf + this.ruecklauf) / JumbovalProduct.GetPipeLengthPerSqm(JumbovalProduct.JumbovalLayDistance.JV20);
                        break;

                    default:
						area = 0;
						break;
				}
				return area * this.PlannedCircuits;
			}
		}

		public static EurovalProduct.EurovalLayDistance GetEurovalLayDistance(VerlegeartEnum verlegeart) {
			switch (verlegeart) {
				case VerlegeartEnum.VA_EV35:
					return EurovalProduct.EurovalLayDistance.EV35;
				case VerlegeartEnum.VA_EV30:
					return EurovalProduct.EurovalLayDistance.EV30;
				case VerlegeartEnum.VA_EV25:
					return EurovalProduct.EurovalLayDistance.EV25;
				case VerlegeartEnum.VA_EV20:
					return EurovalProduct.EurovalLayDistance.EV20;
				case VerlegeartEnum.VA_EV15:
					return EurovalProduct.EurovalLayDistance.EV15;
				case VerlegeartEnum.VA_EV10:
					return EurovalProduct.EurovalLayDistance.EV10;
				case VerlegeartEnum.VA_EV5:
					return EurovalProduct.EurovalLayDistance.EV5;
				case VerlegeartEnum.VA_A5:
					return EurovalProduct.EurovalLayDistance.A5;
				default:
					return EurovalProduct.EurovalLayDistance.EV5;
			}
		}

        public static EcothermProduct.EcothermLayDistance GetEcothermLayDistance(VerlegeartEnum verlegeart) {
            switch (verlegeart) {
                case VerlegeartEnum.VA_EV35:
                    return EcothermProduct.EcothermLayDistance.EV35;
                case VerlegeartEnum.VA_EV30:
                    return EcothermProduct.EcothermLayDistance.EV30;
                case VerlegeartEnum.VA_EV25:
                    return EcothermProduct.EcothermLayDistance.EV25;
                case VerlegeartEnum.VA_EV20:
                    return EcothermProduct.EcothermLayDistance.EV20;
                case VerlegeartEnum.VA_EV15:
                    return EcothermProduct.EcothermLayDistance.EV15;
                case VerlegeartEnum.VA_EV10:
                    return EcothermProduct.EcothermLayDistance.EV10;
                case VerlegeartEnum.VA_EV5:
                    return EcothermProduct.EcothermLayDistance.EV5;
                case VerlegeartEnum.VA_A5:
                    return EcothermProduct.EcothermLayDistance.A5;
                default:
                    return EcothermProduct.EcothermLayDistance.EV5;
            }
        }

        public static JumbovalProduct.JumbovalLayDistance GetJumbovalLayDistance(VerlegeartEnum verlegeart) {
            switch (verlegeart) {
                case VerlegeartEnum.VA_JV50:
                    return JumbovalProduct.JumbovalLayDistance.JV50;
                case VerlegeartEnum.VA_JV40:
                    return JumbovalProduct.JumbovalLayDistance.JV40;
                case VerlegeartEnum.VA_JV30:
                    return JumbovalProduct.JumbovalLayDistance.JV30;
                case VerlegeartEnum.VA_JV20:
                    return JumbovalProduct.JumbovalLayDistance.JV20;
                default:
                    return JumbovalProduct.JumbovalLayDistance.JV20;
            }
        }

        public static bool IsVerlegeartPossible(VerlegeartEnum verlegeart, PipeTypeEnum pipeType) {
            switch (pipeType) {
                case PipeTypeEnum.PT_EUROVAL:
                case PipeTypeEnum.PT_ECOTHERM:
                    return (verlegeart == VerlegeartEnum.VA_UNTER_ESTRICH ||
                        verlegeart == VerlegeartEnum.VA_EV35 ||
                        verlegeart == VerlegeartEnum.VA_EV30 ||
                        verlegeart == VerlegeartEnum.VA_EV25 ||
                        verlegeart == VerlegeartEnum.VA_EV20 ||
                        verlegeart == VerlegeartEnum.VA_EV15 ||
                        verlegeart == VerlegeartEnum.VA_EV10 ||
                        verlegeart == VerlegeartEnum.VA_EV5 ||
                        verlegeart == VerlegeartEnum.VA_A5
                    );
                case PipeTypeEnum.PT_JUMBOVAL:
                    return (verlegeart == VerlegeartEnum.VA_UNTER_ESTRICH ||
                        verlegeart == VerlegeartEnum.VA_JV50 ||
                        verlegeart == VerlegeartEnum.VA_JV40 ||
                        verlegeart == VerlegeartEnum.VA_JV30 ||
                        verlegeart == VerlegeartEnum.VA_JV20);
                case PipeTypeEnum.PT_21MM:
                default:
                    return verlegeart == VerlegeartEnum.VA_UNTER_ESTRICH;
            }
        }

        public static bool IsInsulationPossible(InsulationEnum insulation, VerlegeartEnum verlegeart) {
            switch (verlegeart) {
                case VerlegeartEnum.VA_EV35:
                case VerlegeartEnum.VA_EV30:
                case VerlegeartEnum.VA_EV25:
                case VerlegeartEnum.VA_EV20:
                case VerlegeartEnum.VA_EV15:
                case VerlegeartEnum.VA_EV10:
                case VerlegeartEnum.VA_EV5:
                case VerlegeartEnum.VA_A5:
                case VerlegeartEnum.VA_JV50:
                case VerlegeartEnum.VA_JV40:
                case VerlegeartEnum.VA_JV30:
                case VerlegeartEnum.VA_JV20:
                    return true;
                case VerlegeartEnum.VA_UNTER_ESTRICH:
                default:
                    return insulation == InsulationEnum.IN_VL_RL;
            }
        }

        public static VerlegeartEnum GetDefaultVerlegeart(PipeTypeEnum pipeType) {
            switch (pipeType) {
                case PipeTypeEnum.PT_EUROVAL:
                case PipeTypeEnum.PT_ECOTHERM:
                    return VerlegeartEnum.VA_EV5;
                case PipeTypeEnum.PT_JUMBOVAL:
                    return VerlegeartEnum.VA_JV20;
                case PipeTypeEnum.PT_21MM:
                default:
                    return VerlegeartEnum.VA_UNTER_ESTRICH;
            }
        }

        public static InsulationEnum GetDefaultInsulation(PipeTypeEnum pipeType) {
            switch (pipeType) {
                case PipeTypeEnum.PT_EUROVAL:
                case PipeTypeEnum.PT_ECOTHERM:
                    return InsulationEnum.IN_NONE;
                case PipeTypeEnum.PT_JUMBOVAL:
                    return InsulationEnum.IN_NONE;
                case PipeTypeEnum.PT_21MM:
                default:
                    return InsulationEnum.IN_VL_RL;
            }
        }

		[XmlIgnore]
		private double RohrAussenD {
			get {
				if (this.pipeType == PipeTypeEnum.PT_EUROVAL) {
					return EurovalProduct.ConfigRohrAussenD;
				} else if (this.pipeType == PipeTypeEnum.PT_ECOTHERM) {
					return EcothermProduct.ConfigRohrAussenD;
				} else if (this.pipeType == PipeTypeEnum.PT_JUMBOVAL) {
					return JumbovalProduct.ConfigRohrAussenD;
				} else {
					return Product.rundrohr21mmAussenD;
				}
			}
		}

		[XmlIgnore]
		private double RohrInnenD {
			get {
				if (this.pipeType == PipeTypeEnum.PT_EUROVAL) {
					return EurovalProduct.ConfigRohrInnenD;
				} else if (this.pipeType == PipeTypeEnum.PT_ECOTHERM) {
					return EcothermProduct.ConfigRohrInnenD;
				} else if (this.pipeType == PipeTypeEnum.PT_JUMBOVAL) {
					return JumbovalProduct.ConfigRohrInnenD;
				} else {
					return Product.rundrohr21mmInnenD;
				}
			}
		}

		[XmlIgnore]
		private double RohrInnenA {
			get {
				if (this.pipeType == PipeTypeEnum.PT_EUROVAL) {
					return EurovalProduct.ConfigRohrInnenA;
				} else if (this.pipeType == PipeTypeEnum.PT_ECOTHERM) {
					return EcothermProduct.ConfigRohrInnenA;
				} else if (this.pipeType == PipeTypeEnum.PT_JUMBOVAL) {
					return JumbovalProduct.ConfigRohrInnenA;
				} else {
					return Product.rundrohr21mmInnenA;
				}
			}
		}

		[XmlIgnore]
		private double Geometriefaktor {
			get {
				if (this.pipeType == PipeTypeEnum.PT_EUROVAL && EurovalProduct.ConfigAgActivated) {
					return EurovalProduct.ConfigAg;
                } else if (this.pipeType == PipeTypeEnum.PT_JUMBOVAL && JumbovalProduct.ConfigAgActivated) {
                    return JumbovalProduct.ConfigAg;
				} else {
					return 1;
				}
			}
		}

		public void CalculateHeatLoad(out double heatLoadRoom, out double qH2o, Nullable<int> circuitNr) {
			heatLoadRoom = 0;
			qH2o = 0;

			if (this.verlegeart == VerlegeartEnum.VA_UNTER_ESTRICH) {
				return;
			}
			if (this.Room == null || this.ConnectionThrough == null || this.ConnectionThrough.Product == null || !this.ConnectionThrough.Product.HasInsideConstruction || !this.ConnectionThrough.Product.HasOutsideConstruction) {
				return;
			}

			if (this.ConnectionThrough.RequestedHeatLoad <= 0) {
				return;
			}

			PlannedProduct originalProduct = null;
			foreach (Floor f in Project.Instance.Floors) {
				foreach (Room r in f.Rooms) {
					foreach (PlannedProduct pp in r.PlannedProducts) {
						foreach (ConnectionPipe cp in pp.Product.PlannedConnectionPipes) {
							if (cp == this) {
								originalProduct = pp;
								break;
							}
						}
						if (originalProduct != null) {
							break;
						}
					}
					if (originalProduct != null) {
						break;
					}
				}
				if (originalProduct != null) {
					break;
				}
			}

			if (originalProduct == null || originalProduct.Product == null || originalProduct.Product.PlannedConnection == null || originalProduct.Product.PlannedConnection.Distributor == null || originalProduct.Product.PlannedConnection.Distributor.RegulatorCircuit == null) {
				return;
			}
			double distributorTempOut = 0;
			double distributorTempIn = 0;
			originalProduct.Product.GetHeatFlow(out distributorTempOut, out distributorTempIn);
			double distributorSpreizung = distributorTempOut - distributorTempIn;

			int iterations = this.onlyFirst ? 1 : originalProduct.Product.PlannedCircuitCount;
			int startI = circuitNr.HasValue ? circuitNr.Value : 0;
			int endI = circuitNr.HasValue ? circuitNr.Value + 1 : iterations;
			for (int i = startI; i < endI; i++) {

				double totalPipeLength = originalProduct.Product.GetCircuit(i).PipeLengthWithoutConnections;
				double pipeBeforeVorlauf = 0;
				double pipeAfterVorlauf = totalPipeLength;
				double pipeBeforeRuecklauf = totalPipeLength;
				double pipeAfterRuecklauf = 0;
				bool found = false;

				foreach (ConnectionPipe cp in originalProduct.Product.PlannedConnectionPipes) {
					if (cp == this) {
						found = true;
					} else {
						if (!found) {
							if (i == 0 || !cp.OnlyFirst) {
								if (cp.InsulationForCalculation == InsulationEnum.IN_NONE) {
									pipeBeforeVorlauf += cp.Vorlauf;
								}
								if (cp.InsulationForCalculation != InsulationEnum.IN_VL_RL) {
									pipeAfterRuecklauf += cp.Ruecklauf;
								}
							}
						} else {
							if (i == 0 || !cp.OnlyFirst) {
								if (cp.InsulationForCalculation == InsulationEnum.IN_NONE) {
									pipeAfterVorlauf += cp.Vorlauf;
								}
								if (cp.InsulationForCalculation != InsulationEnum.IN_VL_RL) {
									pipeBeforeRuecklauf += cp.Ruecklauf;
								}
							}
						}
						if (i == 0 || !cp.OnlyFirst) {
							if (cp.InsulationForCalculation == InsulationEnum.IN_NONE) {
								pipeBeforeRuecklauf += cp.Vorlauf;
							}
							if (cp.InsulationForCalculation != InsulationEnum.IN_VL_RL) {
								pipeAfterVorlauf += cp.Ruecklauf;
							}
						}
					}
				}

				Circuit.CircuitConnection connectedCircuit = originalProduct.Product.GetCircuitConnected(i);
				if (connectedCircuit != null) {
					pipeAfterVorlauf += connectedCircuit.OtherCircuit.PipeLengthWithUnisolatedConnections;
					pipeBeforeRuecklauf += connectedCircuit.OtherCircuit.PipeLengthWithUnisolatedConnections;
				}

				connectedCircuit = originalProduct.Product.GetCircuitInverseConnected(i);
				if (connectedCircuit != null) {
					if (connectedCircuit.CircuitConnectionType == Circuit.CircuitConnectionTypeEnum.VORLAUF) {
						pipeAfterVorlauf += connectedCircuit.OtherCircuit.PipeLengthWithoutConnections + connectedCircuit.OtherCircuit.PipeLengthRuecklaufNotIsolated;
						pipeAfterRuecklauf += connectedCircuit.OtherCircuit.PipeLengthWithoutConnections + connectedCircuit.OtherCircuit.PipeLengthRuecklaufNotIsolated;
						pipeBeforeVorlauf += connectedCircuit.OtherCircuit.PipeLengthVorlaufNotIsolated;
						pipeBeforeRuecklauf += connectedCircuit.OtherCircuit.PipeLengthVorlaufNotIsolated;
					} else {
						pipeBeforeVorlauf += connectedCircuit.OtherCircuit.PipeLengthWithoutConnections + connectedCircuit.OtherCircuit.PipeLengthVorlaufNotIsolated;
						pipeBeforeRuecklauf += connectedCircuit.OtherCircuit.PipeLengthWithoutConnections + connectedCircuit.OtherCircuit.PipeLengthVorlaufNotIsolated;
						pipeAfterVorlauf += connectedCircuit.OtherCircuit.PipeLengthRuecklaufNotIsolated;
						pipeAfterRuecklauf += connectedCircuit.OtherCircuit.PipeLengthRuecklaufNotIsolated;
					}
				}

				if (this.Insulation != InsulationEnum.IN_VL_RL) {
					pipeAfterVorlauf += this.ruecklauf;
				}
				if (this.Insulation == InsulationEnum.IN_NONE) {
					pipeBeforeRuecklauf += this.vorlauf;
				}
				totalPipeLength = pipeBeforeVorlauf + pipeAfterVorlauf;
				if (this.Insulation == InsulationEnum.IN_NONE) {
					totalPipeLength += this.vorlauf;
				}

                double su;
                double lambdaU;
                double faktorTrockenkonstruktion;
                double rAlphaDeckeFbh;
                double teilung;
                double alpha0;
                double alphaFbh;
                double su0;
                double lambdaU0;
                double lambdaE;
                double sr;
                double sr0;
                double lambdaR;
                double lambdaR0;
                double pipeLengthPerSqm;
                double rLambdaDecke;
                double rLambdaPutz;

                if (this.pipeType == PipeTypeEnum.PT_JUMBOVAL) {
                    su = JumbovalProduct.ConfigSu; /* Estrichüberdeckung; Annahme ECO30; durch echte Konstruktion ersetzen! */
                    lambdaU = JumbovalProduct.ConfigLambdaU;  /* Estrich??? */
                    faktorTrockenkonstruktion = JumbovalProduct.ConfigFaktorTrockenkonstruktion;
                    rAlphaDeckeFbh = 1 / JumbovalProduct.ConfigAlphaFbk; /* Wärmeübergang Decke bei Heizung */
                    teilung = JumbovalProduct.GetTeilung(ConnectionPipe.GetJumbovalLayDistance(this.verlegeart));
                    alpha0 = JumbovalProduct.ConfigAlpha0;
                    alphaFbh = JumbovalProduct.ConfigAlphaFbh;
                    su0 = JumbovalProduct.ConfigSu0;
                    lambdaU0 = JumbovalProduct.ConfigLambdaU0;
                    lambdaE = JumbovalProduct.ConfigLambdaE;
                    sr = JumbovalProduct.ConfigSr;
                    sr0 = JumbovalProduct.ConfigSr0;
                    lambdaR = JumbovalProduct.ConfigLambdaR;
                    lambdaR0  = JumbovalProduct.ConfigLambdaR0;
                    pipeLengthPerSqm = JumbovalProduct.GetPipeLengthPerSqm(ConnectionPipe.GetJumbovalLayDistance(this.verlegeart));
                    rLambdaDecke = JumbovalProduct.ConfigRLambdaDecke;
                    rLambdaPutz = JumbovalProduct.ConfigRLambdaPutz;
                } else if (this.pipeType == PipeTypeEnum.PT_ECOTHERM) {
                    su = EcothermProduct.ConfigSu; /* Estrichüberdeckung; Annahme ECO30; durch echte Konstruktion ersetzen! */
                    lambdaU = EcothermProduct.ConfigLambdaU;  /* Estrich??? */
                    faktorTrockenkonstruktion = EcothermProduct.ConfigFaktorTrockenkonstruktion;
                    rAlphaDeckeFbh = 1 / EcothermProduct.ConfigAlphaFbk; /* Wärmeübergang Decke bei Heizung */
                    teilung = EcothermProduct.GetTeilung(ConnectionPipe.GetEcothermLayDistance(this.verlegeart));
                    alpha0 = EcothermProduct.ConfigAlpha0;
                    alphaFbh = EcothermProduct.ConfigAlphaFbh;
                    su0 = EcothermProduct.ConfigSu0;
                    lambdaU0 = EcothermProduct.ConfigLambdaU0;
                    lambdaE = EcothermProduct.ConfigLambdaE;
                    sr = EcothermProduct.ConfigSr;
                    sr0 = EcothermProduct.ConfigSr0;
                    lambdaR = EcothermProduct.ConfigLambdaR;
                    lambdaR0 = EcothermProduct.ConfigLambdaR0;
                    pipeLengthPerSqm = EcothermProduct.GetPipeLengthPerSqm(ConnectionPipe.GetEcothermLayDistance(this.verlegeart));
                    rLambdaDecke = EurovalProduct.ConfigRLambdaDecke;
                    rLambdaPutz = EurovalProduct.ConfigRLambdaPutz;
                } else {
                    su = EurovalProduct.ConfigSu; /* Estrichüberdeckung; Annahme ECO30; durch echte Konstruktion ersetzen! */
                    lambdaU = EurovalProduct.ConfigLambdaU;  /* Estrich??? */
                    faktorTrockenkonstruktion = EurovalProduct.ConfigFaktorTrockenkonstruktion;
                    rAlphaDeckeFbh = 1 / EurovalProduct.ConfigAlphaFbk; /* Wärmeübergang Decke bei Heizung */
                    teilung = EurovalProduct.GetTeilung(ConnectionPipe.GetEurovalLayDistance(this.verlegeart));
                    alpha0 = EurovalProduct.ConfigAlpha0;
                    alphaFbh = EurovalProduct.ConfigAlphaFbh;
                    su0 = EurovalProduct.ConfigSu0;
                    lambdaU0 = EurovalProduct.ConfigLambdaU0;
                    lambdaE = EurovalProduct.ConfigLambdaE;
                    sr = EurovalProduct.ConfigSr;
                    sr0 = EurovalProduct.ConfigSr0;
                    lambdaR = EurovalProduct.ConfigLambdaR;
                    lambdaR0 = EurovalProduct.ConfigLambdaR0;
                    pipeLengthPerSqm = EurovalProduct.GetPipeLengthPerSqm(ConnectionPipe.GetEurovalLayDistance(this.verlegeart));
                    rLambdaDecke = EurovalProduct.ConfigRLambdaDecke;
                    rLambdaPutz = EurovalProduct.ConfigRLambdaPutz;
                }

                double rLambdaB = this.ConnectionThrough.Product.PlannedInsideConstructionRValue;
                double rLambdaIns = this.ConnectionThrough.Product.PlannedOutsideConstructionRValue;
                double factor = (this.ConnectionThrough.Product.PlannedInsideConstruction != null && (this.ConnectionThrough.Product.PlannedInsideConstruction.Type == ConstructionTypeManager.Instance.GetConstructionTypeById(ConstructionTypeManager.CT_STD_TROCKEN) || this.ConnectionThrough.Product.PlannedInsideConstruction.Type == ConstructionTypeManager.Instance.GetConstructionTypeById(ConstructionTypeManager.CT_USER_TROCKEN))) ? faktorTrockenkonstruktion : 1;

                double vorlaufTempIn = distributorTempOut - (distributorSpreizung * pipeBeforeVorlauf / totalPipeLength);
                double vorlaufTempOut = distributorTempOut - distributorSpreizung + (distributorSpreizung * pipeAfterVorlauf / totalPipeLength);
                double ruecklaufTempIn = distributorTempOut - (distributorSpreizung * pipeBeforeRuecklauf / totalPipeLength);
                double ruecklaufTempOut = distributorTempOut - distributorSpreizung + (distributorSpreizung * pipeAfterRuecklauf / totalPipeLength);

                double vorlaufHeizmitteluebertemperatur = EN1264.Instance.Heizmitteluebertemperatur(vorlaufTempIn, vorlaufTempOut, this.room.RoomHeatTemperature);
                double vorlaufPotenzProdukt = EN1264.Instance.PotenzProduktFussbodenGeometrie(alpha0, alphaFbh, su0, lambdaU0, lambdaE, rLambdaB, teilung, su, this.RohrAussenD, this.Geometriefaktor);
                double vorlaufSystemabhaengigerKoeffizient = EN1264.Instance.SystemabhaengigerKoeffizientGeometrie(6.7, alpha0, alphaFbh, su0, lambdaU0, lambdaE, rLambdaB, teilung, su, this.RohrAussenD, this.Geometriefaktor, sr, sr0, lambdaR, lambdaR0);
                double vorlaufWaermedurchgangsKoeffizient = EN1264.Instance.WaermedurchgangsKoeffizientRohr(vorlaufSystemabhaengigerKoeffizient, vorlaufPotenzProdukt);
                double vorlaufWaermestromDichte = EN1264.Instance.WaermestromDichteRohr(vorlaufWaermedurchgangsKoeffizient, vorlaufHeizmitteluebertemperatur) * factor;
                double vorlaufHeatLoad = vorlaufWaermestromDichte * this.vorlauf / pipeLengthPerSqm;
                double vorlaufQU = EN1264.Instance.WaermeverlustAussen(alphaFbh, rLambdaB, su, lambdaU, rAlphaDeckeFbh, rLambdaIns, rLambdaDecke, rLambdaPutz, vorlaufWaermestromDichte, this.room.RoomHeatTemperature, this.ConnectionThrough.Product.PlannedRoomTemperatureBelowHeat) * this.vorlauf / pipeLengthPerSqm;

                double ruecklaufHeizmitteluebertemperatur = EN1264.Instance.Heizmitteluebertemperatur(ruecklaufTempIn, ruecklaufTempOut, this.room.RoomHeatTemperature);
                double ruecklaufPotenzProdukt = EN1264.Instance.PotenzProduktFussbodenGeometrie(alpha0, alphaFbh, su0, lambdaU0, lambdaE, rLambdaB, teilung, su, this.RohrAussenD, this.Geometriefaktor);
                double ruecklaufSystemabhaengigerKoeffizient = EN1264.Instance.SystemabhaengigerKoeffizientGeometrie(6.7, alpha0, alphaFbh, su0, lambdaU0, lambdaE, rLambdaB, teilung, su, this.RohrAussenD, this.Geometriefaktor, sr, sr0, lambdaR, lambdaR0);
                double ruecklaufWaermedurchgangsKoeffizient = EN1264.Instance.WaermedurchgangsKoeffizientRohr(ruecklaufSystemabhaengigerKoeffizient, ruecklaufPotenzProdukt);
                double ruecklaufWaermestromDichte = EN1264.Instance.WaermestromDichteRohr(ruecklaufWaermedurchgangsKoeffizient, ruecklaufHeizmitteluebertemperatur) * factor;
                double ruecklaufHeatLoad = ruecklaufWaermestromDichte * this.ruecklauf / pipeLengthPerSqm;
                double ruecklaufQU = EN1264.Instance.WaermeverlustAussen(alphaFbh, rLambdaB, su, lambdaU, rAlphaDeckeFbh, rLambdaIns, rLambdaDecke, rLambdaPutz, ruecklaufWaermestromDichte, this.room.RoomHeatTemperature, this.ConnectionThrough.Product.PlannedRoomTemperatureBelowHeat) * this.ruecklauf / pipeLengthPerSqm;

				if (!double.IsNaN(vorlaufHeatLoad)) {
					heatLoadRoom += vorlaufHeatLoad;
					qH2o += vorlaufHeatLoad;
					if (!double.IsNaN(vorlaufQU)) {
						qH2o += vorlaufQU;
					}
				}
				if (!double.IsNaN(ruecklaufHeatLoad)) {
					heatLoadRoom += ruecklaufHeatLoad;
					qH2o += ruecklaufHeatLoad;
					if (!double.IsNaN(vorlaufQU)) {
						qH2o += ruecklaufQU;
					}
				}
			}
			if (heatLoadRoom.Equals(double.NaN)) {
				heatLoadRoom = 0;
			}
			if (qH2o.Equals(double.NaN)) {
				qH2o = 0;
			}
		}

		public void CalculateCoolLoad(out double coolLoadRoom, out double qH2o, Nullable<int> circuitNr) {
			coolLoadRoom = 0;
			qH2o = 0;
			if (this.verlegeart == VerlegeartEnum.VA_UNTER_ESTRICH) {
				return;
			}
			if (this.Room == null || this.ConnectionThrough == null || this.ConnectionThrough.Product == null || !this.ConnectionThrough.Product.HasInsideConstruction || !this.ConnectionThrough.Product.HasOutsideConstruction) {
				return;
			}

			if (this.ConnectionThrough.RequestedHeatLoad <= 0) {
				return;
			}

			PlannedProduct originalProduct = null;
			foreach (Floor f in Project.Instance.Floors) {
				foreach (Room r in f.Rooms) {
					foreach (PlannedProduct pp in r.PlannedProducts) {
						foreach (ConnectionPipe cp in pp.Product.PlannedConnectionPipes) {
							if (cp == this) {
								originalProduct = pp;
								break;
							}
						}
						if (originalProduct != null) {
							break;
						}
					}
					if (originalProduct != null) {
						break;
					}
				}
				if (originalProduct != null) {
					break;
				}
			}

			if (originalProduct.Product.PlannedConnection == null || originalProduct.Product.PlannedConnection.Distributor == null || originalProduct.Product.PlannedConnection.Distributor.RegulatorCircuit == null) {
				return;
			}
			double distributorTempOut = 0;
			double distributorTempIn = 0;
			originalProduct.Product.GetCoolFlow(out distributorTempOut, out distributorTempIn);
			double distributorSpreizung = distributorTempOut - distributorTempIn;

			int iterations = this.onlyFirst ? 1 : originalProduct.Product.PlannedCircuitCount;
			int startI = circuitNr.HasValue ? circuitNr.Value : 0;
			int endI = circuitNr.HasValue ? circuitNr.Value + 1 : iterations;
			for (int i = startI; i < endI; i++) {

				double totalPipeLength = originalProduct.Product.GetCircuit(i).PipeLengthWithoutConnections;
				double pipeBeforeVorlauf = 0;
				double pipeAfterVorlauf = totalPipeLength;
				double pipeBeforeRuecklauf = totalPipeLength;
				double pipeAfterRuecklauf = 0;
				bool found = false;

				foreach (ConnectionPipe cp in originalProduct.Product.PlannedConnectionPipes) {
					if (cp == this) {
						found = true;
					} else {
						if (!found) {
							if (i == 0 || !cp.OnlyFirst) {
								if (cp.InsulationForCalculation == InsulationEnum.IN_NONE) {
									pipeBeforeVorlauf += cp.Vorlauf;
								}
								if (cp.InsulationForCalculation != InsulationEnum.IN_VL_RL) {
									pipeAfterRuecklauf += cp.Ruecklauf;
								}
							}
						} else {
							if (i == 0 || !cp.OnlyFirst) {
								if (cp.InsulationForCalculation == InsulationEnum.IN_NONE) {
									pipeAfterVorlauf += cp.Vorlauf;
								}
								if (cp.InsulationForCalculation != InsulationEnum.IN_VL_RL) {
									pipeBeforeRuecklauf += cp.Ruecklauf;
								}
							}
						}
						if (i == 0 || !cp.OnlyFirst) {
							if (cp.InsulationForCalculation == InsulationEnum.IN_NONE) {
								pipeBeforeRuecklauf += cp.Vorlauf;
							}
							if (cp.InsulationForCalculation != InsulationEnum.IN_VL_RL) {
								pipeAfterVorlauf += cp.Ruecklauf;
							}
						}
					}
				}

				Circuit.CircuitConnection connectedCircuit = originalProduct.Product.GetCircuitConnected(i);
				if (connectedCircuit != null) {
					pipeAfterVorlauf += connectedCircuit.OtherCircuit.PipeLengthWithUnisolatedConnections;
					pipeBeforeRuecklauf += connectedCircuit.OtherCircuit.PipeLengthWithUnisolatedConnections;
				}

				connectedCircuit = originalProduct.Product.GetCircuitInverseConnected(i);
				if (connectedCircuit != null) {
					if (connectedCircuit.CircuitConnectionType == Circuit.CircuitConnectionTypeEnum.VORLAUF) {
						pipeAfterVorlauf += connectedCircuit.OtherCircuit.PipeLengthWithoutConnections + connectedCircuit.OtherCircuit.PipeLengthRuecklaufNotIsolated;
						pipeAfterRuecklauf += connectedCircuit.OtherCircuit.PipeLengthWithoutConnections + connectedCircuit.OtherCircuit.PipeLengthRuecklaufNotIsolated;
						pipeBeforeVorlauf += connectedCircuit.OtherCircuit.PipeLengthVorlaufNotIsolated;
						pipeBeforeRuecklauf += connectedCircuit.OtherCircuit.PipeLengthVorlaufNotIsolated;
					} else {
						pipeBeforeVorlauf += connectedCircuit.OtherCircuit.PipeLengthWithoutConnections + connectedCircuit.OtherCircuit.PipeLengthVorlaufNotIsolated;
						pipeBeforeRuecklauf += connectedCircuit.OtherCircuit.PipeLengthWithoutConnections + connectedCircuit.OtherCircuit.PipeLengthVorlaufNotIsolated;
						pipeAfterVorlauf += connectedCircuit.OtherCircuit.PipeLengthRuecklaufNotIsolated;
						pipeAfterRuecklauf += connectedCircuit.OtherCircuit.PipeLengthRuecklaufNotIsolated;
					}
				}

				if (this.Insulation != InsulationEnum.IN_VL_RL) {
					pipeAfterVorlauf += this.ruecklauf;
				}
				if (this.Insulation == InsulationEnum.IN_NONE) {
					pipeBeforeRuecklauf += this.vorlauf;
				}
				totalPipeLength = pipeBeforeVorlauf + pipeAfterVorlauf;
				if (this.Insulation == InsulationEnum.IN_NONE) {
					totalPipeLength += this.vorlauf;
				}

                double su;
                double lambdaU;
                double faktorTrockenkonstruktion;
                double rAlphaDeckeFbk;
                double teilung;
                double alpha0;
                double alphaFbk;
                double su0;
                double lambdaU0;
                double lambdaE;
                double sr;
                double sr0;
                double lambdaR;
                double lambdaR0;
                double pipeLengthPerSqm;
                double rLambdaDecke;
                double rLambdaPutz;

                if (this.pipeType == PipeTypeEnum.PT_JUMBOVAL) {
                    su = JumbovalProduct.ConfigSu; /* Estrichüberdeckung; Annahme ECO30; durch echte Konstruktion ersetzen! */
                    lambdaU = JumbovalProduct.ConfigLambdaU;  /* Estrich??? */
                    faktorTrockenkonstruktion = JumbovalProduct.ConfigFaktorTrockenkonstruktion;
                    rAlphaDeckeFbk = 1 / JumbovalProduct.ConfigAlphaFbh; /* Wärmeübergang Decke bei Heizung */
                    teilung = JumbovalProduct.GetTeilung(ConnectionPipe.GetJumbovalLayDistance(this.verlegeart));
                    alpha0 = JumbovalProduct.ConfigAlpha0;
                    alphaFbk = JumbovalProduct.ConfigAlphaFbk;
                    su0 = JumbovalProduct.ConfigSu0;
                    lambdaU0 = JumbovalProduct.ConfigLambdaU0;
                    lambdaE = JumbovalProduct.ConfigLambdaE;
                    sr = JumbovalProduct.ConfigSr;
                    sr0 = JumbovalProduct.ConfigSr0;
                    lambdaR = JumbovalProduct.ConfigLambdaR;
                    lambdaR0 = JumbovalProduct.ConfigLambdaR0;
                    pipeLengthPerSqm = JumbovalProduct.GetPipeLengthPerSqm(ConnectionPipe.GetJumbovalLayDistance(this.verlegeart));
                    rLambdaDecke = JumbovalProduct.ConfigRLambdaDecke;
                    rLambdaPutz = JumbovalProduct.ConfigRLambdaPutz;
                } else if (this.pipeType == PipeTypeEnum.PT_ECOTHERM) {
                    su = EcothermProduct.ConfigSu; /* Estrichüberdeckung; Annahme ECO30; durch echte Konstruktion ersetzen! */
                    lambdaU = EcothermProduct.ConfigLambdaU;  /* Estrich??? */
                    faktorTrockenkonstruktion = EcothermProduct.ConfigFaktorTrockenkonstruktion;
                    rAlphaDeckeFbk = 1 / EcothermProduct.ConfigAlphaFbh; /* Wärmeübergang Decke bei Heizung */
                    teilung = EcothermProduct.GetTeilung(ConnectionPipe.GetEcothermLayDistance(this.verlegeart));
                    alpha0 = EcothermProduct.ConfigAlpha0;
                    alphaFbk = EcothermProduct.ConfigAlphaFbk;
                    su0 = EcothermProduct.ConfigSu0;
                    lambdaU0 = EcothermProduct.ConfigLambdaU0;
                    lambdaE = EcothermProduct.ConfigLambdaE;
                    sr = EcothermProduct.ConfigSr;
                    sr0 = EcothermProduct.ConfigSr0;
                    lambdaR = EcothermProduct.ConfigLambdaR;
                    lambdaR0 = EcothermProduct.ConfigLambdaR0;
                    pipeLengthPerSqm = EcothermProduct.GetPipeLengthPerSqm(ConnectionPipe.GetEcothermLayDistance(this.verlegeart));
                    rLambdaDecke = EurovalProduct.ConfigRLambdaDecke;
                    rLambdaPutz = EurovalProduct.ConfigRLambdaPutz;
                } else {
                    su = EurovalProduct.ConfigSu; /* Estrichüberdeckung; Annahme ECO30; durch echte Konstruktion ersetzen! */
                    lambdaU = EurovalProduct.ConfigLambdaU;  /* Estrich??? */
                    faktorTrockenkonstruktion = EurovalProduct.ConfigFaktorTrockenkonstruktion;
                    rAlphaDeckeFbk = 1 / EurovalProduct.ConfigAlphaFbh; /* Wärmeübergang Decke bei Heizung */
                    teilung = EurovalProduct.GetTeilung(ConnectionPipe.GetEurovalLayDistance(this.verlegeart));
                    alpha0 = EurovalProduct.ConfigAlpha0;
                    alphaFbk = EurovalProduct.ConfigAlphaFbk;
                    su0 = EurovalProduct.ConfigSu0;
                    lambdaU0 = EurovalProduct.ConfigLambdaU0;
                    lambdaE = EurovalProduct.ConfigLambdaE;
                    sr = EurovalProduct.ConfigSr;
                    sr0 = EurovalProduct.ConfigSr0;
                    lambdaR = EurovalProduct.ConfigLambdaR;
                    lambdaR0 = EurovalProduct.ConfigLambdaR0;
                    pipeLengthPerSqm = EurovalProduct.GetPipeLengthPerSqm(ConnectionPipe.GetEurovalLayDistance(this.verlegeart));
                    rLambdaDecke = EurovalProduct.ConfigRLambdaDecke;
                    rLambdaPutz = EurovalProduct.ConfigRLambdaPutz;
                }
                
				double rLambdaB = this.ConnectionThrough.Product.PlannedInsideConstructionRValue;
				double rLambdaIns = this.ConnectionThrough.Product.PlannedOutsideConstructionRValue;

				double vorlaufTempIn = distributorTempOut - (distributorSpreizung * pipeBeforeVorlauf / totalPipeLength);
				double vorlaufTempOut = distributorTempOut - distributorSpreizung + (distributorSpreizung * pipeAfterVorlauf / totalPipeLength);
				double ruecklaufTempIn = distributorTempOut - (distributorSpreizung * pipeBeforeRuecklauf / totalPipeLength);
				double ruecklaufTempOut = distributorTempOut - distributorSpreizung + (distributorSpreizung * pipeAfterRuecklauf / totalPipeLength);

				double vorlaufHeizmitteluebertemperatur = EN1264.Instance.Heizmitteluebertemperatur(vorlaufTempIn, vorlaufTempOut, this.room.RoomCoolTemperature);
				double vorlaufPotenzProdukt = EN1264.Instance.PotenzProduktFussbodenGeometrie(alpha0, alphaFbk, su0, lambdaU0, lambdaE, rLambdaB, teilung, su, this.RohrAussenD, this.Geometriefaktor);
				double vorlaufSystemabhaengigerKoeffizient = EN1264.Instance.SystemabhaengigerKoeffizientGeometrie(6.7, alpha0, alphaFbk, su0, lambdaU0, lambdaE, rLambdaB, teilung, su, this.RohrAussenD, this.Geometriefaktor, sr, sr0, lambdaR, lambdaR0);
				double vorlaufWaermedurchgangsKoeffizient = EN1264.Instance.WaermedurchgangsKoeffizientRohr(vorlaufSystemabhaengigerKoeffizient, vorlaufPotenzProdukt);
				double vorlaufWaermestromDichte = EN1264.Instance.WaermestromDichteRohr(vorlaufWaermedurchgangsKoeffizient, vorlaufHeizmitteluebertemperatur);
				double vorlaufHeatLoad = vorlaufWaermestromDichte * this.vorlauf / pipeLengthPerSqm;
				double vorlaufQU = EN1264.Instance.WaermeverlustAussen(alphaFbk, rLambdaB, su, lambdaU, rAlphaDeckeFbk, rLambdaIns, rLambdaDecke, rLambdaPutz, vorlaufWaermestromDichte, this.room.RoomCoolTemperature, this.ConnectionThrough.Product.PlannedRoomTemperatureBelowCool) * this.vorlauf / pipeLengthPerSqm;

				double ruecklaufHeizmitteluebertemperatur = EN1264.Instance.Heizmitteluebertemperatur(ruecklaufTempIn, ruecklaufTempOut, this.room.RoomCoolTemperature);
				double ruecklaufPotenzProdukt = EN1264.Instance.PotenzProduktFussbodenGeometrie(alpha0, alphaFbk, su0, lambdaU0, lambdaE, rLambdaB, teilung, su, this.RohrAussenD, this.Geometriefaktor);
				double ruecklaufSystemabhaengigerKoeffizient = EN1264.Instance.SystemabhaengigerKoeffizientGeometrie(6.7, alpha0, alphaFbk, su0, lambdaU0, lambdaE, rLambdaB, teilung, su, this.RohrAussenD, this.Geometriefaktor, sr, sr0, lambdaR, lambdaR0);
				double ruecklaufWaermedurchgangsKoeffizient = EN1264.Instance.WaermedurchgangsKoeffizientRohr(ruecklaufSystemabhaengigerKoeffizient, ruecklaufPotenzProdukt);
				double ruecklaufWaermestromDichte = EN1264.Instance.WaermestromDichteRohr(ruecklaufWaermedurchgangsKoeffizient, ruecklaufHeizmitteluebertemperatur);
				double ruecklaufHeatLoad = ruecklaufWaermestromDichte * this.ruecklauf / pipeLengthPerSqm;
				double ruecklaufQU = EN1264.Instance.WaermeverlustAussen(alphaFbk, rLambdaB, su, lambdaU, rAlphaDeckeFbk, rLambdaIns, rLambdaDecke, rLambdaPutz, ruecklaufWaermestromDichte, this.room.RoomCoolTemperature, this.ConnectionThrough.Product.PlannedRoomTemperatureBelowCool) * this.ruecklauf / pipeLengthPerSqm;

				if (!double.IsNaN(vorlaufHeatLoad)) {
					coolLoadRoom += vorlaufHeatLoad;
					qH2o += vorlaufHeatLoad;
					if (!double.IsNaN(vorlaufQU)) {
						qH2o += vorlaufQU;
					}
				}
				if (!double.IsNaN(ruecklaufHeatLoad)) {
					coolLoadRoom += ruecklaufHeatLoad;
					qH2o += ruecklaufHeatLoad;
					if (!double.IsNaN(vorlaufQU)) {
						qH2o += ruecklaufQU;
					}
				}

			}
			if (coolLoadRoom.Equals(double.NaN)) {
				coolLoadRoom = 0;
			} else {
				coolLoadRoom = -coolLoadRoom;
			}
			if (qH2o.Equals(double.NaN)) {
				qH2o = 0;
			} else {
				qH2o = -qH2o;
			}
			return;
		}

		[XmlIgnore]
		public double HeatLoadTotal {
			get {
				double heatLoad;
				double qH2o;
				this.CalculateHeatLoad(out heatLoad, out qH2o, null);
				return heatLoad;
			}
		}

		[XmlIgnore]
		public double CoolLoadTotal {
			get {
				double coolLoad;
				double qH2o;
				this.CalculateCoolLoad(out coolLoad, out qH2o, null);
				return coolLoad;
			}
		}

		public double CalculateDruckverlust(double durchfluss) {
			if (this.ConnectionOf == null) {
				return 0;
			}
			return EN1264.Instance.DruckverlustRohr(durchfluss, this.RohrInnenA, this.ConnectionOf.Product.Dichte, this.RohrInnenD, this.ConnectionOf.Product.Viskositaet, 0.000004, this.Vorlauf + this.Ruecklauf);
		}
	}
}
