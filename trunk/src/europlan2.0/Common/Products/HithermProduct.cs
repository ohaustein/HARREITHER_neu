using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Europlan.Common {

	[Serializable()]
	[ProductName("Hitherm®")]
	public class HithermProduct : Product {

		// quick dimensioning
		private static int quickDimensioningHeatPowerPerSquareMeter = 100;
		private static int quickDimensioningCoolPowerPerSquareMeter = 100;
		private static bool canHeat = true;
		private static bool canCool = false;

		// planning
		// Hitherm(r) Hochleistungs-Klimawandregister (RA 5) Heizleistung qW in W/m²
		private static double[][] hlRegHeizleistung = {
			//  tHm (°C)  30.0  32.5  35.0  37.5  40.0  42.5  45.0  47.5  50.0
			new double[] { 105,  120,  140,  155,  175,  190,  210,  225,  240}, // ti=15°C
			new double[] {  85,  100,  120,  135,  155,  170,  185,  205,  220}, // ti=18°C
			new double[] {  70,   85,  105,  120,  140,  155,  175,  190,  210}, // ti=20°C
			new double[] {  55,   70,   90,  105,  125,  140,  160,  175,  195}, // ti=22°C
			new double[] {  45,   60,   80,   95,  115,  130,  145,  165,  180}  // ti=24°C
		};

		// Hitherm(r) Standard-Klimawandregister (RA 10) Heizleistung qW in W/m²
		private static double[][] stdRegHeizleistung = {
			//  tHm (°C)  30.0  32.5  35.0  37.5  40.0  42.5  45.0  47.5  50.0
			new double[] {  70,   85,   95,  110,  120,  130,  145,  155,  170}, // ti=15°C
			new double[] {  55,   70,   80,   95,  105,  120,  130,  145,  155}, // ti=18°C
			new double[] {  50,   60,   75,   85,  100,  110,  125,  135,  150}, // ti=20°C
			new double[] {  40,   50,   65,   75,   90,  100,  115,  125,  140}, // ti=22°C
			new double[] {  30,   40,   55,   65,   80,   90,  100,  115,  130}  // ti=24°C
		};

		private static double factorSpezialputz = 1.15;
		private static double factorMaschinenputz = 1.0;
		private static double factorLehmputz = 0.95;
		private static double factorGkpHohlraum = 0.69;
		private static double factorHolzHohlraum = 0.62;

		public HithermProduct() {

		}

		protected HithermProduct(HithermProduct product) : base(product) {

		}

		public override void Initialize() {
		}

		public override void StaticInitialize() {
			quickDimensioningHeatPowerPerSquareMeter = 100;
			quickDimensioningCoolPowerPerSquareMeter = 100;
			canHeat = true;
			canCool = false;
		}

		public override Product Clone(Room room) {
			HithermProduct product = new HithermProduct(this);
			product.AssociatedRoom = room;
			return product;
		}

		#region Product Parameters
		[ProductParameter]
		public static bool ConfigQuickDimensioningCanHeat {
			get { return canHeat; }
			set { canHeat = value; }
		}
		public override bool QuickDimensioningCanHeat {
			get { return canCool; }
		}

		[ProductParameter]
		public static bool ConfigQuickDimensioningCanCool {
			get { return canCool; }
			set { canCool = value; }
		}
		public override bool QuickDimensioningCanCool {
			get { return canCool; }
		}

		[ProductParameter]
		public static int ConfigQuickDimensioningHeatPowerPerSquareMeter {
			get { return quickDimensioningHeatPowerPerSquareMeter; }
			set { quickDimensioningHeatPowerPerSquareMeter = value; }
		}
		public override int QuickDimensioningHeatPowerPerSquareMeter {
			get { return quickDimensioningHeatPowerPerSquareMeter; }
		}

		[ProductParameter]
		public static int ConfigQuickDimensioningCoolPowerPerSquareMeter {
			get { return quickDimensioningCoolPowerPerSquareMeter; }
			set { quickDimensioningCoolPowerPerSquareMeter = value; }
		}
		public override int QuickDimensioningCoolPowerPerSquareMeter {
			get { return quickDimensioningCoolPowerPerSquareMeter; }
		}

		private static double[][] ConvertStringToArray(string value) {
			string str = value.Trim();
			if (!str.StartsWith("{") || !str.EndsWith("}")) {
				// log warning
				return null;
			}
			str = str.Substring(1, str.Length - 2).Trim();
			List<List<double>> list = new List<List<double>>();
			while (str.Length > 0) {
				int end = str.IndexOf('}');
				if (str[0] != '{' || end < 0) {
					// log warning
					return null;
				}
				List<double> curList = new List<double>();
				list.Add(curList);
				string[] strValues = str.Substring(1, end - 1).Trim().Split(',');
				foreach (string strValue in strValues) {
					double doubleValue;
					if (!double.TryParse(strValue.Trim(), out doubleValue)) {
						// log warning
						return null;
					}
					curList.Add(doubleValue);
				}
				str = str.Substring(end + 1).Trim();
				if (str.Length != 0) {
					if (str[0] != ',') {
						// log warning
						return null;
					}
					str = str.Substring(1);
				}
			}
			double[][] array = new double[list.Count][];
			int i = 0;
			foreach (List<double> curList in list) {
				array[i] = new double[curList.Count];
				int j = 0;
				foreach (double doubleValue in curList) {
					array[i][j] = doubleValue;
					j++;
				}
				i++;
			}
			return array;
		}

		public static string ConvertArrayToString(double[][] array) {
			string rtn = "";
			foreach (double[] row in array) {
				string rowStr = "";
				foreach (double val in row) {
					rowStr += ", " + val.ToString(System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
				}
				rowStr = rowStr.Substring(2);
				rtn += " ,{" + rowStr + "}";
			}
			rtn = "{" + rtn.Substring(2) + "}";
			return rtn;
		}

		[ProductParameter]
		public static string ConfigHlRegHeizleistungString {
			get {
				return ConvertArrayToString(hlRegHeizleistung);
			}
			set {
				double[][] array = ConvertStringToArray(value);
				if (array != null) {
					hlRegHeizleistung = array;
				}
			}
		}
		public static double[][] ConfigHlRegHeizleistung {
			get { return hlRegHeizleistung; }
			set { hlRegHeizleistung = value; }
		}

		[ProductParameter]
		public static string ConfigStdRegHeizleistungString {
			get {
				return ConvertArrayToString(stdRegHeizleistung);
			}
			set {
				double[][] array = ConvertStringToArray(value);
				if (array != null) {
					stdRegHeizleistung = array;
				}
			}
		}
		public static double[][] ConfigStdRegHeizleistung {
			get { return stdRegHeizleistung; }
			set { stdRegHeizleistung = value; }
		}
		#endregion Product Parameters

		public override int GetDefaultQuickDimensioningCircuits() {
			return (int)Math.Ceiling(quickDimensioningPlannedArea / 10);
		}

		public override float GetDefaultQuickDimensioningPlannedArea() {
			return 0;
		}

		public override float QuickDimensioningMaximumArea {
			get { return Int32.MaxValue; }
		}

		public override string Name {
			get { return "Hitherm®"; }
		}

		public override string QuickDimensioningName {
			get { return "Hitherm®\n(m²)"; }
		}

		/// <summary>
		/// The full name of this product
		/// </summary>
		public override string FullName {
			get { return "Hitherm® Klimawand"; }
		}

		public override ProductType Type {
			get { return ProductType.WH; }
		}

		public override bool ConfigureProduct(double requestedHeatLoad, double requestedCoolLoad, bool calculateHeat, bool calculateCool, out string errorMsg) {
			// TODO
			errorMsg = "Noch nicht implementiert";
			return false;
		}

		public override float PlannedFloorArea {
			get { return 0; }
			set { }
		}

		public override float PlannedWallArea {
			get { return 0; }
			set { }
		}

		public override float PlannedCeilingArea {
			get { return 0; }
			set { }
		}

		public override double PlannedCoolLoad {
			get { return 0; }
		}

		public override double PlannedHeatLoad {
			get { return 0; }
		}

		public override float PlannedNetArea {
			get { return 0; }
		}

		/*public override int GetIndexOfCircuit(Circuit c) {
			return -1;
		}*/

		public override ConnectionPipe.PipeTypeEnum DefaultPipeType {
			get { return ConnectionPipe.PipeTypeEnum.PT_21MM; }
		}

		[XmlIgnore]
		public override float PlannedInsideConstructionRValue {
			get { return 0; /*TODO*/ }
		}

		[XmlIgnore]
		public override bool HasInsideConstruction {
			get { return false; }
		}

		[XmlIgnore]
		public override Construction PlannedInsideConstruction {
			get { return null; }
		}

		[XmlIgnore]
		public override float PlannedOutsideConstructionRValue {
			get { return 0; /*TODO*/ }
		}

		[XmlIgnore]
		public override bool HasOutsideConstruction {
			get { return false; }
		}

		[XmlIgnore]
		public override Construction PlannedOutsideConstruction {
			get { return null; }
		}
	}
	
}
