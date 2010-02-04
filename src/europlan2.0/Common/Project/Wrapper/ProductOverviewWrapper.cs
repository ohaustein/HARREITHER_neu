using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {

	public class ProductOverviewWrapper {

		public class LayDistanceEnumConverter : System.ComponentModel.TypeConverter {
			private static readonly string EV5 = "EV5";
			private static readonly string EV10 = "EV10";
			private static readonly string EV15 = "EV15";
			private static readonly string EV20 = "EV20";
			private static readonly string EV25 = "EV25";
			private static readonly string EV30 = "EV30";
			private static readonly string EV35 = "EV35";

			private Dictionary<string, LayDistanceEnum> mappingFromString = new Dictionary<string, LayDistanceEnum>();
			private Dictionary<LayDistanceEnum, string> mappingToString = new Dictionary<LayDistanceEnum, string>();

			public LayDistanceEnumConverter() {
				mappingFromString.Add(EV5, LayDistanceEnum.EV5);
				mappingFromString.Add(EV10, LayDistanceEnum.EV10);
				mappingFromString.Add(EV15, LayDistanceEnum.EV15);
				mappingFromString.Add(EV20, LayDistanceEnum.EV20);
				mappingFromString.Add(EV25, LayDistanceEnum.EV25);
				mappingFromString.Add(EV30, LayDistanceEnum.EV30);
				mappingFromString.Add(EV35, LayDistanceEnum.EV35);
				mappingToString.Add(LayDistanceEnum.EV5, EV5);
				mappingToString.Add(LayDistanceEnum.EV10, EV10);
				mappingToString.Add(LayDistanceEnum.EV15, EV15);
				mappingToString.Add(LayDistanceEnum.EV20, EV20);
				mappingToString.Add(LayDistanceEnum.EV25, EV25);
				mappingToString.Add(LayDistanceEnum.EV30, EV30);
				mappingToString.Add(LayDistanceEnum.EV35, EV35);
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
				if (value is LayDistanceEnum && destinationType == typeof(string)) {
					if (mappingToString.ContainsKey((LayDistanceEnum)value)) {
						return mappingToString[(LayDistanceEnum)value];
					}
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}
		}

		[System.ComponentModel.TypeConverter(typeof(LayDistanceEnumConverter))]

		public enum LayDistanceEnum {
			EV5 = 1,
			EV10 = 2,
			EV15 = 3,
			EV20 = 4,
			EV25 = 5,
			EV30 = 6,
			EV35 = 7,
		}

		public class RimTypeEnumConverter : System.ComponentModel.TypeConverter {
			private static readonly string EV15_60 = "EV15/60";
			private static readonly string EV15_120 = "EV15/120";
			private static readonly string EV15_180 = "EV15/180";
			private static readonly string EV10_55 = "EV10/55";
			private static readonly string EV10_110 = "EV10/110";
			private static readonly string EV10_165 = "EV10/165";
			private static readonly string EV5_40 = "EV5/40";
			private static readonly string EV5_80 = "EV5/80";
			private static readonly string EV5_120 = "EV5/120";

			private Dictionary<string, RimTypeEnum> mappingFromString = new Dictionary<string, RimTypeEnum>();
			private Dictionary<RimTypeEnum, string> mappingToString = new Dictionary<RimTypeEnum, string>();

			public RimTypeEnumConverter() {
				mappingFromString.Add(EV15_60, RimTypeEnum.EV15_60);
				mappingFromString.Add(EV15_120, RimTypeEnum.EV15_120);
				mappingFromString.Add(EV15_180, RimTypeEnum.EV15_180);
				mappingFromString.Add(EV10_55, RimTypeEnum.EV10_55);
				mappingFromString.Add(EV10_110, RimTypeEnum.EV10_110);
				mappingFromString.Add(EV10_165, RimTypeEnum.EV10_165);
				mappingFromString.Add(EV5_40, RimTypeEnum.EV5_40);
				mappingFromString.Add(EV5_80, RimTypeEnum.EV5_80);
				mappingFromString.Add(EV5_120, RimTypeEnum.EV5_120);
				mappingToString.Add(RimTypeEnum.EV15_60, EV15_60);
				mappingToString.Add(RimTypeEnum.EV15_120, EV15_120);
				mappingToString.Add(RimTypeEnum.EV15_180, EV15_180);
				mappingToString.Add(RimTypeEnum.EV10_55, EV10_55);
				mappingToString.Add(RimTypeEnum.EV10_110, EV10_110);
				mappingToString.Add(RimTypeEnum.EV10_165, EV10_165);
				mappingToString.Add(RimTypeEnum.EV5_40, EV5_40);
				mappingToString.Add(RimTypeEnum.EV5_80, EV5_80);
				mappingToString.Add(RimTypeEnum.EV5_120, EV5_120);
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
				if (value is RimTypeEnum && destinationType == typeof(string)) {
					if (mappingToString.ContainsKey((RimTypeEnum)value)) {
						return mappingToString[(RimTypeEnum)value];
					}
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}
		}

		[System.ComponentModel.TypeConverter(typeof(RimTypeEnumConverter))]

		public enum RimTypeEnum {
			EV15_60,
			EV15_120,
			EV15_180,
			EV10_55,
			EV10_110,
			EV10_165,
			EV5_40,
			EV5_80,
			EV5_120
		}

		private Nullable<LayDistanceEnum> GetLayDistance(Nullable<EurovalProduct.EurovalLayDistance> layDistance) {
			switch (layDistance) {
				case null:
					return null;
				case EurovalProduct.EurovalLayDistance.EV5:
					return LayDistanceEnum.EV5;
				case EurovalProduct.EurovalLayDistance.EV10:
					return LayDistanceEnum.EV10;
				case EurovalProduct.EurovalLayDistance.EV15:
					return LayDistanceEnum.EV15;
				case EurovalProduct.EurovalLayDistance.EV20:
					return LayDistanceEnum.EV20;
				case EurovalProduct.EurovalLayDistance.EV25:
					return LayDistanceEnum.EV25;
				case EurovalProduct.EurovalLayDistance.EV30:
					return LayDistanceEnum.EV30;
				case EurovalProduct.EurovalLayDistance.EV35:
					return LayDistanceEnum.EV35;
				default:
					throw new Exception("Unknwon LayDistance");
			}
		}

		private Nullable<LayDistanceEnum> GetLayDistance(Nullable<EcothermProduct.EcothermLayDistance> layDistance) {
			switch (layDistance) {
				case null:
					return null;
				case EcothermProduct.EcothermLayDistance.EV5:
					return LayDistanceEnum.EV5;
				case EcothermProduct.EcothermLayDistance.EV10:
					return LayDistanceEnum.EV10;
				case EcothermProduct.EcothermLayDistance.EV15:
					return LayDistanceEnum.EV15;
				case EcothermProduct.EcothermLayDistance.EV20:
					return LayDistanceEnum.EV20;
				case EcothermProduct.EcothermLayDistance.EV25:
					return LayDistanceEnum.EV25;
				case EcothermProduct.EcothermLayDistance.EV30:
					return LayDistanceEnum.EV30;
				case EcothermProduct.EcothermLayDistance.EV35:
					return LayDistanceEnum.EV35;
				default:
					throw new Exception("Unknwon LayDistance");
			}
		}

		private Nullable<EurovalProduct.EurovalLayDistance> GetEurovalLayDistance(Nullable<LayDistanceEnum> layDistance) {
			switch (layDistance) {
				case null:
					return null;
				case LayDistanceEnum.EV5:
					return EurovalProduct.EurovalLayDistance.EV5;
				case LayDistanceEnum.EV10:
					return EurovalProduct.EurovalLayDistance.EV10;
				case LayDistanceEnum.EV15:
					return EurovalProduct.EurovalLayDistance.EV15;
				case LayDistanceEnum.EV20:
					return EurovalProduct.EurovalLayDistance.EV20;
				case LayDistanceEnum.EV25:
					return EurovalProduct.EurovalLayDistance.EV25;
				case LayDistanceEnum.EV30:
					return EurovalProduct.EurovalLayDistance.EV30;
				case LayDistanceEnum.EV35:
					return EurovalProduct.EurovalLayDistance.EV35;
				default:
					throw new Exception("Unknwon LayDistance");
			}
		}

		private Nullable<EcothermProduct.EcothermLayDistance> GetEcothermLayDistance(Nullable<LayDistanceEnum> layDistance) {
			switch (layDistance) {
				case null:
					return null;
				case LayDistanceEnum.EV5:
					return EcothermProduct.EcothermLayDistance.EV5;
				case LayDistanceEnum.EV10:
					return EcothermProduct.EcothermLayDistance.EV10;
				case LayDistanceEnum.EV15:
					return EcothermProduct.EcothermLayDistance.EV15;
				case LayDistanceEnum.EV20:
					return EcothermProduct.EcothermLayDistance.EV20;
				case LayDistanceEnum.EV25:
					return EcothermProduct.EcothermLayDistance.EV25;
				case LayDistanceEnum.EV30:
					return EcothermProduct.EcothermLayDistance.EV30;
				case LayDistanceEnum.EV35:
					return EcothermProduct.EcothermLayDistance.EV35;
				default:
					throw new Exception("Unknwon LayDistance");
			}
		}

		private Nullable<RimTypeEnum> GetRimType(Nullable<EurovalProduct.EurovalRimType> rimType) {
			switch (rimType) {
				case null:
					return null;
				case EurovalProduct.EurovalRimType.EV5_40:
					return RimTypeEnum.EV5_40;
				case EurovalProduct.EurovalRimType.EV10_55:
					return RimTypeEnum.EV10_55;
				case EurovalProduct.EurovalRimType.EV15_60:
					return RimTypeEnum.EV15_60;
				case EurovalProduct.EurovalRimType.EV5_80:
					return RimTypeEnum.EV5_80;
				case EurovalProduct.EurovalRimType.EV10_110:
					return RimTypeEnum.EV10_110;
				case EurovalProduct.EurovalRimType.EV15_120:
					return RimTypeEnum.EV15_120;
				case EurovalProduct.EurovalRimType.EV5_120:
					return RimTypeEnum.EV5_120;
				case EurovalProduct.EurovalRimType.EV10_165:
					return RimTypeEnum.EV10_165;
				case EurovalProduct.EurovalRimType.EV15_180:
					return RimTypeEnum.EV15_180;
				default:
					throw new Exception("Unknwon RimType");
			}
		}

		private Nullable<RimTypeEnum> GetRimType(Nullable<EcothermProduct.EcothermRimType> rimType) {
			switch (rimType) {
				case null:
					return null;
				case EcothermProduct.EcothermRimType.EV5_40:
					return RimTypeEnum.EV5_40;
				case EcothermProduct.EcothermRimType.EV10_55:
					return RimTypeEnum.EV10_55;
				case EcothermProduct.EcothermRimType.EV15_60:
					return RimTypeEnum.EV15_60;
				case EcothermProduct.EcothermRimType.EV5_80:
					return RimTypeEnum.EV5_80;
				case EcothermProduct.EcothermRimType.EV10_110:
					return RimTypeEnum.EV10_110;
				case EcothermProduct.EcothermRimType.EV15_120:
					return RimTypeEnum.EV15_120;
				case EcothermProduct.EcothermRimType.EV5_120:
					return RimTypeEnum.EV5_120;
				case EcothermProduct.EcothermRimType.EV10_165:
					return RimTypeEnum.EV10_165;
				case EcothermProduct.EcothermRimType.EV15_180:
					return RimTypeEnum.EV15_180;
				default:
					throw new Exception("Unknwon RimType");
			}
		}

		private Nullable<EurovalProduct.EurovalRimType> GetEurovalRimType(Nullable<RimTypeEnum> rimType) {
			switch (rimType) {
				case null:
					return null;
				case RimTypeEnum.EV5_40:
					return EurovalProduct.EurovalRimType.EV5_40;
				case RimTypeEnum.EV10_55:
					return EurovalProduct.EurovalRimType.EV10_55;
				case RimTypeEnum.EV15_60:
					return EurovalProduct.EurovalRimType.EV15_60;
				case RimTypeEnum.EV5_80:
					return EurovalProduct.EurovalRimType.EV5_80;
				case RimTypeEnum.EV10_110:
					return EurovalProduct.EurovalRimType.EV10_110;
				case RimTypeEnum.EV15_120:
					return EurovalProduct.EurovalRimType.EV15_120;
				case RimTypeEnum.EV5_120:
					return EurovalProduct.EurovalRimType.EV5_120;
				case RimTypeEnum.EV10_165:
					return EurovalProduct.EurovalRimType.EV10_165;
				case RimTypeEnum.EV15_180:
					return EurovalProduct.EurovalRimType.EV15_180;
				default:
					throw new Exception("Unknwon RimType");
			}
		}

		private Nullable<EcothermProduct.EcothermRimType> GetEcothermRimType(Nullable<RimTypeEnum> rimType) {
			switch (rimType) {
				case null:
					return null;
				case RimTypeEnum.EV5_40:
					return EcothermProduct.EcothermRimType.EV5_40;
				case RimTypeEnum.EV10_55:
					return EcothermProduct.EcothermRimType.EV10_55;
				case RimTypeEnum.EV15_60:
					return EcothermProduct.EcothermRimType.EV15_60;
				case RimTypeEnum.EV5_80:
					return EcothermProduct.EcothermRimType.EV5_80;
				case RimTypeEnum.EV10_110:
					return EcothermProduct.EcothermRimType.EV10_110;
				case RimTypeEnum.EV15_120:
					return EcothermProduct.EcothermRimType.EV15_120;
				case RimTypeEnum.EV5_120:
					return EcothermProduct.EcothermRimType.EV5_120;
				case RimTypeEnum.EV10_165:
					return EcothermProduct.EcothermRimType.EV10_165;
				case RimTypeEnum.EV15_180:
					return EcothermProduct.EcothermRimType.EV15_180;
				default:
					throw new Exception("Unknwon RimType");
			}
		}

		public class LayDistanceItem {
			public Nullable<LayDistanceEnum> layDistance;
			public string name;

			public LayDistanceItem(Nullable<LayDistanceEnum> layDistance, string name) {
				this.layDistance = layDistance;
				this.name = name;
			}

			public override string ToString() {
				return this.name;
			}

			public override bool Equals(object obj) {
				return obj is LayDistanceItem && (obj as LayDistanceItem).layDistance == this.layDistance;
			}

			public override int GetHashCode() {
				return (this.layDistance == null ? 0 : this.layDistance.GetHashCode());
			}
		}

		public class RimTypeItem {
			public Nullable<RimTypeEnum> rimType;
			public string name;

			public RimTypeItem(Nullable<RimTypeEnum> layDistance, string name) {
				this.rimType = layDistance;
				this.name = name;
			}

			public override string ToString() {
				return this.name;
			}

			public override bool Equals(object obj) {
				return obj is RimTypeItem && (obj as RimTypeItem).rimType == this.rimType;
			}

			public override int GetHashCode() {
				return (this.rimType == null ? 0 : this.rimType.GetHashCode());
			}
		}

		private PlannedProduct plannedProduct;
		
		public ProductOverviewWrapper(PlannedProduct pp) {
			plannedProduct = pp;
		}

		public PlannedProduct PlannedProduct {
			get { return plannedProduct; }
		}

		public bool ManualMode {
			get { return plannedProduct.Product.ManualMode; }
		}
		
		public string RoomId {
			get { return plannedProduct.Product.AssociatedRoom.Id; }
		}

		public string RoomName {
			get { return plannedProduct.Product.AssociatedRoom.Name; }
		}

		public string TeilSystem {
			get { return plannedProduct.InternalName; }
		}

		public string SystemName {
			get { return plannedProduct.Product.FullName; }
		}

		public Nullable<int> NrOfCircuits {
			get {
				if (plannedProduct.Product.PlannedCircuitCount == 0) {
					return null;
				} else {
					return plannedProduct.Product.PlannedCircuitCount;
				}
			}
			set {
				if (plannedProduct.Product is EurovalProduct) {
					(plannedProduct.Product as EurovalProduct).RequestedCircuits = value;
					plannedProduct.ConfigureProduct(false);
				} else if (plannedProduct.Product is EcothermProduct) {
					(plannedProduct.Product as EcothermProduct).RequestedCircuits = value;
					plannedProduct.ConfigureProduct(false);
				} else if (plannedProduct.Product is ModulKlimaBodenProduct) {
					(plannedProduct.Product as ModulKlimaBodenProduct).RequestedCircuits = value;
					plannedProduct.ConfigureProduct(false);
				}
			}
		}

		public bool NrOfCircuitsEditable {
			get {
				if (plannedProduct.Product is EurovalProduct) {
					return !(plannedProduct.Product as EurovalProduct).PlannedProductIsConnection;
				} else if (plannedProduct.Product is EcothermProduct) {
					return !(plannedProduct.Product as EcothermProduct).PlannedProductIsConnection;
				} else if (plannedProduct.Product is ModulKlimaBodenProduct) {
					return true;
				}

				return false;
			}
		}

		public bool NrOfCircuitsModified {
			get {
				if (plannedProduct.Product is EurovalProduct) {
					return (plannedProduct.Product as EurovalProduct).RequestedCircuits != null;
				} else if (plannedProduct.Product is EcothermProduct) {
					return (plannedProduct.Product as EcothermProduct).RequestedCircuits != null;
				} else if (plannedProduct.Product is ModulKlimaBodenProduct) {
					return (plannedProduct.Product as ModulKlimaBodenProduct).RequestedCircuits != null;
				}
				return false;
			}
		}

		public Nullable<double> PipeLength {
			get {
				if (plannedProduct.Product is EurovalProduct) {
					if ((plannedProduct.Product as EurovalProduct).PlannedPipeLengthPerCircuit > 0) {
						return (plannedProduct.Product as EurovalProduct).PlannedPipeLengthPerCircuit;
					} 
				} else if (plannedProduct.Product is EcothermProduct) {
					if ((plannedProduct.Product as EcothermProduct).PlannedPipeLengthPerCircuit > 0) {
						return (plannedProduct.Product as EcothermProduct).PlannedPipeLengthPerCircuit;
					}
				}
				return null;
			}
		}

		public Nullable<RimTypeEnum> RimType {
			get {
				if (plannedProduct.Product is EurovalProduct) {
					if ((plannedProduct.Product as EurovalProduct).PlannedProductIsConnection) {
						return null;
					}
					return GetRimType((plannedProduct.Product as EurovalProduct).PlannedRimType);
				} else if (plannedProduct.Product is EcothermProduct) {
					if ((plannedProduct.Product as EcothermProduct).PlannedProductIsConnection) {
						return null;
					}
					return GetRimType((plannedProduct.Product as EcothermProduct).PlannedRimType);
				}
				return null;
			}
			set {
				if (plannedProduct.Product is EurovalProduct) {
					(plannedProduct.Product as EurovalProduct).RequestedRimType = GetEurovalRimType(value);
					plannedProduct.ConfigureProduct(false);
				} else if (plannedProduct.Product is EcothermProduct) {
					(plannedProduct.Product as EcothermProduct).RequestedRimType = GetEcothermRimType(value);
					plannedProduct.ConfigureProduct(false);
				}
			}
		}

		public bool RimTypeEditable {
			get {
				if (plannedProduct.Product is EurovalProduct && !(plannedProduct.Product as EurovalProduct).PlannedProductIsConnection) {
					return (plannedProduct.Product as EurovalProduct).PlannedRimLength > 0;
				} else if (plannedProduct.Product is EcothermProduct && !(plannedProduct.Product as EcothermProduct).PlannedProductIsConnection) {
					return (plannedProduct.Product as EcothermProduct).PlannedRimLength > 0;
				}
				return false;
			}
		}

		public bool RimTypeModified {
			get {
				if (plannedProduct.Product is EurovalProduct) {
					return (plannedProduct.Product as EurovalProduct).RequestedRimType != null;
				} else if (plannedProduct.Product is EcothermProduct) {
					return (plannedProduct.Product as EcothermProduct).RequestedRimType != null;
				}
				return false;
			}
		}

		public Nullable<LayDistanceEnum> LayDistance {
			get {
				if (plannedProduct.Product is EurovalProduct) {
					if ((plannedProduct.Product as EurovalProduct).PlannedProductIsConnection) {
						return null;
					}
					return GetLayDistance((plannedProduct.Product as EurovalProduct).PlannedLayDistance);
				} else if (plannedProduct.Product is EcothermProduct) {
					if ((plannedProduct.Product as EcothermProduct).PlannedProductIsConnection) {
						return null;
					}
					return GetLayDistance((plannedProduct.Product as EcothermProduct).PlannedLayDistance);
				}
				return null;
			}
			set {
				if (plannedProduct.Product is EurovalProduct) {
					(plannedProduct.Product as EurovalProduct).RequestedLayDistance = GetEurovalLayDistance(value);
					plannedProduct.ConfigureProduct(false);
				} else if (plannedProduct.Product is EcothermProduct) {
					(plannedProduct.Product as EcothermProduct).RequestedLayDistance = GetEcothermLayDistance(value);
					plannedProduct.ConfigureProduct(false);
				}
			}
		}

		public bool LayDistanceEditable {
			get {
				if (plannedProduct.Product is EurovalProduct && !(plannedProduct.Product as EurovalProduct).PlannedProductIsConnection) {
					return true;
				} else if (plannedProduct.Product is EcothermProduct && !(plannedProduct.Product as EcothermProduct).PlannedProductIsConnection) {
					return true;
				}
				return false;
			}
		}

		public bool LayDistanceModified {
			get {
				if (plannedProduct.Product is EurovalProduct) {
					return (plannedProduct.Product as EurovalProduct).RequestedLayDistance != null;
				} else if (plannedProduct.Product is EcothermProduct) {
					return (plannedProduct.Product as EcothermProduct).RequestedLayDistance != null;
				}
				return false;
			}
		}

		public Nullable<double> TotalArea {
			get { return plannedProduct.PlannedArea; }
		}

		public Nullable<double> DruckverlustHeat {
			get {
				if (plannedProduct.Product.PlannedMhHeat > 0) {
					return plannedProduct.Product.PlannedMhHeat; 
				} else {
					return null;
				}
			}
		}

		public double HeatNetLoad {
			get { return plannedProduct.RequestedHeatLoad; }
		}

		public double HeatPower {
			get { return plannedProduct.PlannedHeatLoad; }
		}

		public Nullable<double> HeatRest {
			get {
				if (plannedProduct.Product is EurovalProduct) {
					if ((plannedProduct.Product as EurovalProduct).PlannedProductIsConnection) {
						return null;
					}
				} else if (plannedProduct.Product is EcothermProduct) {
					if ((plannedProduct.Product as EcothermProduct).PlannedProductIsConnection) {
						return null;
					}
				} 
				return -1 * (HeatNetLoad - HeatPower); 
			}
		}

		public Nullable<double> DruckverlustCool {
			get {
				if (plannedProduct.Product.PlannedMhCool > 0) {
					return plannedProduct.Product.PlannedMhCool;
				} else {
					return null;
				}
			}
		}

		public double CoolNetLoad {
			get { return plannedProduct.RequestedCoolLoad; }
		}
		
		public double CoolPower {
			get { return plannedProduct.PlannedCoolLoad; }
		}

		public Nullable<double> CoolRest {
			get {
				if (plannedProduct.Product is EurovalProduct) {
					if ((plannedProduct.Product as EurovalProduct).PlannedProductIsConnection) {
						return null;
					}
				} else if (plannedProduct.Product is EcothermProduct) {
					if ((plannedProduct.Product as EcothermProduct).PlannedProductIsConnection) {
						return null;
					}
				} 
				return -1 * (CoolNetLoad - CoolPower); 
			}
		}

		public bool Ok {
			get { return plannedProduct.Product.LastErrorMessage == null; }
		}

	}

}
