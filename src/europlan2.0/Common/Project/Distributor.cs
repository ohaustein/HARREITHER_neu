using System;
using System.Collections.Generic;
using System.Text;
using log4net;
using System.Windows.Forms;
using System.Xml.Serialization;
using WW.Math;
using WW.Math.Geometry;
using WW.Cad.Model;
using WW.Cad.Model.Tables;
using WW.Cad.Model.Entities;
using System.Drawing;

namespace Europlan.Common {

	[Serializable()]
	public class Distributor : IGuiRepresentation, IRequiredMaterial {

	public struct GraphicalRepresentation {
		public Point2D position;
		public double rotation;
		public bool isOnThisFloor;
		public string floorId;
	}

#region enums

		//public class DistributorTypeEnumConverter : System.ComponentModel.TypeConverter {
	
		//    private static readonly string durchflussmengenregler = "Harreither Systemverteiler mit Durchflußmengenregler";
		//    private static readonly string ruecklaufventil = "Harreither Systemverteiler mit Rücklaufventil";

		//    private Dictionary<string, DistributorTypeEnum> mappingFromString = new Dictionary<string, DistributorTypeEnum>();
		//    private Dictionary<DistributorTypeEnum, string> mappingToString = new Dictionary<DistributorTypeEnum, string>();

		//    public DistributorTypeEnumConverter() {
		//        mappingFromString.Add(durchflussmengenregler, DistributorTypeEnum.Durchflussmengenregler);
		//        mappingFromString.Add(ruecklaufventil, DistributorTypeEnum.Ruecklaufventil);
		//        mappingToString.Add(DistributorTypeEnum.Durchflussmengenregler, durchflussmengenregler);
		//        mappingToString.Add(DistributorTypeEnum.Ruecklaufventil, ruecklaufventil);
		//    }

		//    public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext context, Type sourceType) {
		//        return sourceType == typeof(string);
		//    }

		//    public override bool CanConvertTo(System.ComponentModel.ITypeDescriptorContext context, Type destinationType) {
		//        return destinationType == typeof(string);
		//    }

		//    public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value) {
		//        if (value is string) {
		//            if (mappingFromString.ContainsKey((string)value)) {
		//                return mappingFromString[(string)value];
		//            }
		//        }
		//        return base.ConvertFrom(context, culture, value);
		//    }

		//    public override object ConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType) {
		//        if (value is DistributorTypeEnum && destinationType == typeof(string)) {
		//            if (mappingToString.ContainsKey((DistributorTypeEnum)value)) {
		//                return mappingToString[(DistributorTypeEnum)value];
		//            }
		//        }
		//        return base.ConvertTo(context, culture, value, destinationType);
		//    }
		//}

		//[System.ComponentModel.TypeConverter(typeof(DistributorTypeEnumConverter))]
		//public enum DistributorTypeEnum {
		//    Durchflussmengenregler,
		//    Ruecklaufventil
		//}

		public class AnschlussHollaenderEnumConverter : System.ComponentModel.TypeConverter {

			private static readonly string kein = EuroplanRes.Distributor_KeinHollaender;
			private static readonly string hollaender32 = EuroplanRes.Distributor_Hollaender32mm;
			private static readonly string hollaenderIG = EuroplanRes.Distributor_HollaenderIg;
			//private static readonly string hollaenderAG = "Anschlußholländer mit Anschlußstück 1\" AG";

			private Dictionary<string,  AnschlussHollaenderEnum> mappingFromString = new Dictionary<string,  AnschlussHollaenderEnum>();
			private Dictionary< AnschlussHollaenderEnum, string> mappingToString = new Dictionary< AnschlussHollaenderEnum, string>();

			public AnschlussHollaenderEnumConverter() {
				mappingFromString.Add(kein, AnschlussHollaenderEnum.Kein);
				mappingFromString.Add(hollaender32, AnschlussHollaenderEnum.hollaender32);
				mappingFromString.Add(hollaenderIG, AnschlussHollaenderEnum.hollaenderIG);
				//mappingFromString.Add(hollaenderAG, AnschlussHollaenderEnum.hollaenderAG);
				mappingToString.Add(AnschlussHollaenderEnum.Kein, kein);
				mappingToString.Add(AnschlussHollaenderEnum.hollaender32, hollaender32);
				mappingToString.Add(AnschlussHollaenderEnum.hollaenderIG, hollaenderIG);
				//mappingToString.Add(AnschlussHollaenderEnum.hollaenderAG, hollaenderAG);
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
				if (value is  AnschlussHollaenderEnum && destinationType == typeof(string)) {
					if (mappingToString.ContainsKey(( AnschlussHollaenderEnum)value)) {
						return mappingToString[( AnschlussHollaenderEnum)value];
					}
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}
		}

		[System.ComponentModel.TypeConverter(typeof(AnschlussHollaenderEnumConverter))]
		public enum AnschlussHollaenderEnum {
			Kein,
			hollaender32,
			hollaenderIG
			//hollaenderAG
		}

#endregion

		private string id;
		private string name;
		private string regulatorCircuitId = null;
		private RegulatorCircuit regulatorCircuit = null;
		//private DistributorTypeEnum distributorType;
		private AnschlussHollaenderEnum anschlussHollaender;
		private bool langeAnschlussboegen;
		private int maxCircuits;
		private int additionalCircuits;
		private int zusaetzlicheStellantriebe;
		private bool flanschKugelHaehne;
		private bool einbauSchrank;
		private bool useForFloor;
		private bool useForWall;
		private bool useForCeiling;

		private List<string> additionalFloors;
		private List<GraphicalRepresentation> graphicalRepresentations;

		[NonSerialized]
		private static readonly ILog log = LogManager.GetLogger(typeof(Distributor));

		private TreeNode distributorNode = new TreeNode();

		public Distributor() {
			InitializeDistributor();
		}

		public override string ToString() {
			return this.id + ": " + this.name;
		}

		private void InitializeDistributor() {
			id = "";
			name = "";
			regulatorCircuitId = "";
			//distributorType = DistributorTypeEnum.Durchflussmengenregler;
			anschlussHollaender = AnschlussHollaenderEnum.Kein;
			langeAnschlussboegen = false;
			maxCircuits = 12;
			additionalCircuits = 0;
			zusaetzlicheStellantriebe = 0;
			flanschKugelHaehne = true;
			einbauSchrank = true;
			additionalFloors = new List<string>();
			graphicalRepresentations = new List<GraphicalRepresentation>();
			distributorNode.Tag = this;
			useForFloor = true;
			useForWall = true;
			useForCeiling = true;
            distributorNode.ImageKey = "Verteiler.png";
            distributorNode.SelectedImageKey = "Verteiler.png";
		}

		public string Id {
			get { return id; }
			set {
				id = value;
				if (distributorNode != null) {
					distributorNode.Text = (String.IsNullOrEmpty(name) ? EuroplanRes.Distributor_Unbenannt : id + ": " + name);
				}
			}
		}

		public string Name {
			get { return name; }
			set {
				name = value;
				if (distributorNode != null) {
					distributorNode.Text = (String.IsNullOrEmpty(name) ? EuroplanRes.Distributor_Unbenannt : id + ": " + name);
				}
			}
		}

		//public DistributorTypeEnum DistributorType {
		//    get { return distributorType; }
		//    set { distributorType = value; }
		//}

		public AnschlussHollaenderEnum AnschlussHollaender {
			get { return anschlussHollaender; }
			set { anschlussHollaender = value; }
		}

		[XmlIgnore]
		public double Width {
			get {
				switch (anschlussHollaender) {
					case AnschlussHollaenderEnum.Kein:
						return 0.349 + ((maxCircuits - 2) * 0.055);
					case AnschlussHollaenderEnum.hollaender32:
						return 0.349 + ((maxCircuits - 2) * 0.055);
					case AnschlussHollaenderEnum.hollaenderIG:
						if (flanschKugelHaehne) {
							return 0.372 + ((maxCircuits - 2) * 0.055);
						} else {
							return 0.326 + ((maxCircuits - 2) * 0.055);
						}
					default:
						return 0;
				}
			}
		}

		[XmlIgnore]
		public double Height {
			get {
				return 0.108;
			}
		}

		[XmlIgnore]
		public List<Floor> AdditionalFloors {
			get {
				List<Floor> floors = new List<Floor>();
				foreach (string floorId in additionalFloors) {
					foreach (Floor floor in Project.Instance.Floors) {
						if (floor.Id == floorId) {
							floors.Add(floor);
							continue;
						}
					}
				}
				return floors;
			}
			set {
				foreach (Floor floor in value) {
					if (!additionalFloors.Contains(floor.Id)) {
						additionalFloors.Add(floor.Id);
					}
				}
			}
		}

		[XmlIgnore]
		public Floor AssociatedFloor {
			get {
				foreach (Floor floor in Project.Instance.Floors) {
					if (floor.Distributors.Contains(this)) {
						return floor;
					}
				}
				return null;
			}
		}

		public int MaxCircuits {
			get { return maxCircuits; }
			set { maxCircuits = value; }
		}

		public int AdditionalCircuits {
			get { return additionalCircuits; }
			set { additionalCircuits = value; }
		}

		public int ZusaetzlicheStellantriebe {
			get { return zusaetzlicheStellantriebe; }
			set { zusaetzlicheStellantriebe = value; }
		}

		public bool FlanschKugelHaehne {
			get { return flanschKugelHaehne; }
			set { flanschKugelHaehne = value; }
		}

		public bool EinbauSchrank {
			get { return einbauSchrank; }
			set { einbauSchrank = value; }
		}

		public bool LangeAnschlussboegen {
			get { return langeAnschlussboegen; }
			set { langeAnschlussboegen = value; }
		}

		public bool UseForFloor {
			get { return useForFloor; }
			set { useForFloor = value; }
		}

		public bool UseForWall {
			get { return useForWall; }
			set { useForWall = value; }
		}

		public bool UseForCeiling {
			get { return useForCeiling; }
			set { useForCeiling = value; }
		}

		internal TreeNode Node {
			get { return this.distributorNode; }
		}

		public List<string> AdditionalFloorIds {
			get { return additionalFloors; }
			set { additionalFloors = value; }
		}

		public List<GraphicalRepresentation> GraphicalRepresentations {
			get { return graphicalRepresentations; }
			set { graphicalRepresentations = value; }
		}

		[XmlIgnore]
		public RegulatorCircuit RegulatorCircuit {
			get {
				if (this.regulatorCircuitId != null) {
					if (Project.Instance != null) {
						foreach (RegulatorCircuit c in Project.Instance.RegulatorCircuits) {
							if (c.Id == regulatorCircuitId) {
								this.regulatorCircuit = c;
								continue;
							}
						}
					}
					this.regulatorCircuitId = null;
				}
				return this.regulatorCircuit;
			}
			set {
				this.regulatorCircuit = value;
				this.regulatorCircuitId = null;
			}
		}

		public string RegulatorCircuitId {
			get {
				if (this.RegulatorCircuit == null) {
					return null;
				}
				return this.regulatorCircuit.Id;
			}
			set { this.regulatorCircuitId = value; }
		}

		public TreeNode FindNode(object element) {
			if (element == this) {
				return distributorNode;
			}
			return null;
		}
		
		public Type AssociatedPanelType {
			get {
				return typeof(DistributorPanel);
			}
		}

		[XmlIgnore]
		public List<PlannedProduct> PlannedConnectedProducts {
			get {
				List<PlannedProduct> products = new List<PlannedProduct>();
				foreach (Floor floor in Project.Instance.Floors) {
					foreach (Room room in floor.Rooms) {
						foreach (PlannedProduct product in room.PlannedProducts) {
							if (product.Product.PlannedConnection != null && product.Product.PlannedConnection.Distributor != null) {
								if (product.Product.PlannedConnection.Distributor == this) {
									products.Add(product);
								}
							}
						}
					}
				}
				return products;
			}
		}

		[XmlIgnore]
		public List<PlannedProduct> PlannedDirectAndIndirectConnectedProducts {
			get {
				List<PlannedProduct> products = new List<PlannedProduct>();
				foreach (Floor floor in Project.Instance.Floors) {
					foreach (Room room in floor.Rooms) {
						foreach (PlannedProduct product in room.PlannedProducts) {
							if (product.Product.PlannedConnection != null && product.Product.PlannedConnection.DirectOrIndirectDistributor == this) {
								products.Add(product);
							}
						}
					}
				}
				return products;
			}
		}

		public int PlannedCircuits {
			get {
				int plannedCircuits = 0;
				foreach (Floor floor in Project.Instance.Floors) {
					foreach (Room room in floor.Rooms) {
						foreach (PlannedProduct pp in room.PlannedProducts) {
							if (pp.Product.PlannedConnection != null && pp.Product.PlannedConnection.DistributorId == this.Id) {
								plannedCircuits += pp.Product.PlannedCircuitCount;
							}
						}
					}
				}
				return plannedCircuits;
			}
		}

		public int PlannedStellAntriebe {
			get {
				int plannedStellAntriebe = 0;
				foreach (Floor floor in Project.Instance.Floors) {
					foreach (Room room in floor.Rooms) {
						foreach (PlannedProduct pp in room.PlannedProducts) {
							if (pp.Product.PlannedConnection != null && pp.Product.PlannedConnection.DistributorId == this.Id) {
								if (pp.Product.StellMotore) {
									plannedStellAntriebe += pp.Product.PlannedCircuitCount;
								}
							}
						}
					}
				}
				return plannedStellAntriebe;
			}
		}

		public double GetDruckverlust(double durchfluss) {
			double ratio = 0;
			switch (PlannedCircuits) {
				case 1:
				case 2:
					ratio = 200.0 / 700.0;
					break;
				case 3:
					ratio = 200.0 / 1000.0;
					break;
				case 4:
					ratio = 200.0 / 1300.0;
					break;
				case 5:
					ratio = 200.0 / 1600.0;
					break;
				case 6:
					ratio = 200.0 / 1900.0;
					break;
				case 7:
					ratio = 180.0 / 2000.0;
					break;
				case 8:
					ratio = 160.0 / 2000.0;
					break;
				case 9:
					ratio = 140.0 / 2000.0;
					break;
				case 10:
					ratio = 120.0 / 2000.0;
					break;
				case 11:
					ratio = 100.0 / 2000.0;
					break;
				case 12:
					ratio = 80.0 / 2000.0;
					break;
			}

			return durchfluss * ratio;
		}

		public double MaxDruckverlustVerteilerHeat {
			get {
				double maxDruckverlustInCircuit = 0;
				double durchfluss = 0;
				foreach (PlannedProduct pp in PlannedConnectedProducts) {
					maxDruckverlustInCircuit = maxDruckverlustInCircuit < pp.Product.PlannedDeltaRhoDistributorHeat ? pp.Product.PlannedDeltaRhoDistributorHeat : maxDruckverlustInCircuit;
					durchfluss += pp.Product.PlannedMhHeat;
				}
				return maxDruckverlustInCircuit + GetDruckverlust(durchfluss);
			}
		}

		public double MaxDruckverlustVerteilerCool {
			get {
				double maxDruckverlustInCircuit = 0;
				double durchfluss = 0;
				foreach (PlannedProduct pp in PlannedConnectedProducts) {
					maxDruckverlustInCircuit = maxDruckverlustInCircuit < pp.Product.PlannedDeltaRhoDistributorCool ? pp.Product.PlannedDeltaRhoDistributorCool : maxDruckverlustInCircuit;
					durchfluss += pp.Product.PlannedMhCool;
				}
				return maxDruckverlustInCircuit + GetDruckverlust(durchfluss);
			}
		}


		public void CalculateRequiredMaterial(SerializableDictionary<string, double> requiredMaterial) {
			int totalCircuits = PlannedCircuits + AdditionalCircuits;
			int totalStellantriebe = PlannedStellAntriebe + ZusaetzlicheStellantriebe;

			string partNr = "VO";
			int circuits = totalCircuits > 2 ? totalCircuits : 2;
			partNr += String.Format("{0:00}", circuits);
			Project.Instance.AddRequiredMaterial(requiredMaterial, partNr, 1);

			if (this.FlanschKugelHaehne) {
				Project.Instance.AddRequiredMaterial(requiredMaterial, "VT35", 2);
			}
			if (this.AnschlussHollaender == AnschlussHollaenderEnum.hollaender32) {
				Project.Instance.AddRequiredMaterial(requiredMaterial, "VT21", 2);
			} else if (this.AnschlussHollaender == AnschlussHollaenderEnum.hollaenderIG) {
				Project.Instance.AddRequiredMaterial(requiredMaterial, "VT20", 2);
			}
			string einbauSchrank = "";
			if (this.EinbauSchrank) {
				if (totalCircuits <= 4) {
					einbauSchrank = "VO60";
				} else if (totalCircuits > 4 && totalCircuits <= 9) {
					einbauSchrank = "VO61";
				} else {
					einbauSchrank = "VO62";
				}
				Project.Instance.AddRequiredMaterial(requiredMaterial, einbauSchrank, 1);
			}
			if (totalStellantriebe > 0) {
				Project.Instance.AddRequiredMaterial(requiredMaterial, "VO44", totalStellantriebe);
			}
		}

		public string ErrorMessage {
			get {
				string message = null;
				foreach (string err in this.ErrorMessageArray) {
					if (message != null) {
						message += "\n" + err;
					} else {
						message = err;
					}
				}
				return message;
			}
		}

		public string[] ErrorMessageArray {
			get {
				List<string> errors = new List<string>();
				if (this.MaxCircuits - this.AdditionalCircuits < this.PlannedCircuits) {
					string err = this.AdditionalCircuits > 0 ? EuroplanRes.Distributor_ZuVieleHeizkreiseZus : EuroplanRes.Distributor_ZuVieleHeizkreise;
					/*err = err.Replace("%VERTEILER%", this.Name);
					err = err.Replace("%GESCHOSS%", this.AssociatedFloor.Name);*/
					err = err.Replace("%VALUE%", this.PlannedCircuits.ToString());
					err = err.Replace("%VALUEZUS%", this.AdditionalCircuits.ToString());
					err = err.Replace("%MAXIMUM%", MaxCircuits.ToString());
					//An den Verteiler %VERTEILER% (%GESCHOSS%) sind zu viele Heizkreise angeschlossen (%VALUE% + %VALUEZUS% > %MAXIMUM%)
					//An den Verteiler %VERTEILER% (%GESCHOSS%) sind zu viele Heizkreise angeschlossen (%VALUE% > %MAXIMUM%)
					errors.Add(err);
				}
				string[] errs = new string[errors.Count];
				errors.CopyTo(errs);
				return errs;
			}
		}

		public List<PossibleConnection> GetPossibleConnections(bool input, bool output, double measure, bool invertYAxis, Point2D currentMousePoint, Product product, Circuit circuit, Floor floor) {
			// TODO
			double width = this.Width * measure;
			double height = this.Height * measure;
			double connectionWidth = 0.055 * measure;
			double border = (width - this.maxCircuits * connectionWidth) / 2.0;

			Point2D leftBottom = Point2D.Zero;
			Point2D leftTop = Point2D.Zero;
			Point2D rightTop = Point2D.Zero;
			Point2D rightBottom = Point2D.Zero;
			Matrix3D transformation = Matrix3D.Identity;

			List<PossibleConnection> possibleConnections = new List<PossibleConnection>();

			Nullable<GraphicalRepresentation> representation = null;
			foreach (GraphicalRepresentation gr in this.graphicalRepresentations) {
				if (gr.floorId == floor.Id) {
					representation = gr;
					break;
				}
			}

			if (representation == null) {
				return possibleConnections;
			}

			List<int> openInputs = this.GetOpenInputs();
			List<int> openOutputs = this.GetOpenOutputs();

			if (invertYAxis) {
				transformation = transformation * Transformation3D.Translation(representation.Value.position.X, representation.Value.position.Y);
				transformation = transformation * Transformation3D.Rotate(-representation.Value.rotation * Math.PI / 180.0);
				transformation = transformation * Transformation3D.Translation(-representation.Value.position.X, -representation.Value.position.Y);
				leftBottom = transformation.Transform(new Point2D(representation.Value.position.X, representation.Value.position.Y));
				leftTop = transformation.Transform(new Point2D(representation.Value.position.X, representation.Value.position.Y + height));
				rightTop = transformation.Transform(new Point2D(representation.Value.position.X + width, representation.Value.position.Y + height));
				rightBottom = transformation.Transform(new Point2D(representation.Value.position.X + width, representation.Value.position.Y));
			} else {
				transformation = transformation * Transformation3D.Translation(representation.Value.position.X, representation.Value.position.Y);
				transformation = transformation * Transformation3D.Rotate(representation.Value.rotation * Math.PI / 180.0);
				transformation = transformation * Transformation3D.Translation(-representation.Value.position.X, -representation.Value.position.Y);
				leftBottom = transformation.Transform(new Point2D(representation.Value.position.X, representation.Value.position.Y));
				leftTop = transformation.Transform(new Point2D(representation.Value.position.X, representation.Value.position.Y - height));
				rightTop = transformation.Transform(new Point2D(representation.Value.position.X + width, representation.Value.position.Y - height));
				rightBottom = transformation.Transform(new Point2D(representation.Value.position.X + width, representation.Value.position.Y));
			}
			Polygon2D distPoly = new Polygon2D(new Point2D[] { leftBottom, leftTop, rightTop, rightBottom });
			if (!distPoly.IsInside(currentMousePoint)) {
				return possibleConnections;
			}

			if (invertYAxis) {
				for (int i = 0; i < this.maxCircuits; i++) {
					if (output && openOutputs.Contains(i)) {
						leftBottom = transformation.Transform(new Point2D(representation.Value.position.X + border + i * connectionWidth, representation.Value.position.Y));
						leftTop = transformation.Transform(new Point2D(representation.Value.position.X + border + i * connectionWidth, representation.Value.position.Y + height));
						rightTop = transformation.Transform(new Point2D(representation.Value.position.X + border + (i + 0.5) * connectionWidth, representation.Value.position.Y + height));
						rightBottom = transformation.Transform(new Point2D(representation.Value.position.X + border + (i + 0.5) * connectionWidth, representation.Value.position.Y));
						possibleConnections.Add(new PossibleConnection(transformation.Transform(new Point2D(representation.Value.position.X + border + (i + 0.25) * connectionWidth, representation.Value.position.Y + height * 0.7)), new Polygon2D(new Point2D[] { leftBottom, leftTop, rightTop, rightBottom }), false, true, this, i, representation.Value.rotation, 0));
					}

					if (input && openInputs.Contains(i)) {
						leftBottom = transformation.Transform(new Point2D(representation.Value.position.X + border + (i + 0.5) * connectionWidth, representation.Value.position.Y)); ;
						leftTop = transformation.Transform(new Point2D(representation.Value.position.X + border + (i + 0.5) * connectionWidth, representation.Value.position.Y + height));
						rightTop = transformation.Transform(new Point2D(representation.Value.position.X + border + (i + 1.0) * connectionWidth, representation.Value.position.Y + height));
						rightBottom = transformation.Transform(new Point2D(representation.Value.position.X + border + (i + 1.0) * connectionWidth, representation.Value.position.Y));
						possibleConnections.Add(new PossibleConnection(transformation.Transform(new Point2D(representation.Value.position.X + border + (i + 0.75) * connectionWidth, representation.Value.position.Y + height * 0.3)), new Polygon2D(new Point2D[] { leftBottom, leftTop, rightTop, rightBottom }), true, false, this, i, representation.Value.rotation, 0));
					}
				}
			} else {
				for (int i = 0; i < this.maxCircuits; i++) {
					if (output && openOutputs.Contains(i)) {
						leftBottom = transformation.Transform(new Point2D(representation.Value.position.X + border + i * connectionWidth, representation.Value.position.Y));
						leftTop = transformation.Transform(new Point2D(representation.Value.position.X + border + i * connectionWidth, representation.Value.position.Y - height));
						rightTop = transformation.Transform(new Point2D(representation.Value.position.X + border + (i + 0.5) * connectionWidth, representation.Value.position.Y - height));
						rightBottom = transformation.Transform(new Point2D(representation.Value.position.X + border + (i + 0.5) * connectionWidth, representation.Value.position.Y));
						possibleConnections.Add(new PossibleConnection(transformation.Transform(new Point2D(representation.Value.position.X + border + (i + 0.25) * connectionWidth, representation.Value.position.Y - height * 0.7)), new Polygon2D(new Point2D[] { leftBottom, leftTop, rightTop, rightBottom }), false, true, this, i, representation.Value.rotation, 0));
					}

					if (input && openInputs.Contains(i)) {
						leftBottom = transformation.Transform(new Point2D(representation.Value.position.X + border + (i + 0.5) * connectionWidth, representation.Value.position.Y));
						leftTop = transformation.Transform(new Point2D(representation.Value.position.X + border + (i + 0.5) * connectionWidth, representation.Value.position.Y - height));
						rightTop = transformation.Transform(new Point2D(representation.Value.position.X + border + (i + 1) * connectionWidth, representation.Value.position.Y - height));
						rightBottom = transformation.Transform(new Point2D(representation.Value.position.X + border + (i + 1) * connectionWidth, representation.Value.position.Y));
						possibleConnections.Add(new PossibleConnection(transformation.Transform(new Point2D(representation.Value.position.X + border + (i + 0.75) * connectionWidth, representation.Value.position.Y - height * 0.3)), new Polygon2D(new Point2D[] { leftBottom, leftTop, rightTop, rightBottom }), true, false, this, i, representation.Value.rotation, 0));
					}
				}
			}

			return possibleConnections;
		}

		public List<int> GetOpenInputs() {
			List<int> openInputs = new List<int>();
			for (int i = 0; i < this.maxCircuits; i++) {
				openInputs.Add(i);
			}
			foreach (Floor floor in Project.Instance.Floors) {
				foreach (Room room in floor.Rooms) {
					foreach (PlannedProduct pp in room.PlannedProducts) {
						foreach (GraphicalProductConnection conn in pp.Product.Connections) {
							if (!conn.Vorlauf && conn.Distributor == this) {
								for (int index = conn.DistributorStartIndex; index < conn.DistributorStartIndex + conn.NrOfCircuits; index++) {
									openInputs.Remove(index);
								}
							}
						}
					}
				}
			}
			return openInputs;
		}

		public List<int> GetOpenOutputs() {
			List<int> openOutputs = new List<int>();
			for (int i = 0; i < this.maxCircuits; i++) {
				openOutputs.Add(i);
			}
			foreach (Floor floor in Project.Instance.Floors) {
				foreach (Room room in floor.Rooms) {
					foreach (PlannedProduct pp in room.PlannedProducts) {
						foreach (GraphicalProductConnection conn in pp.Product.Connections) {
							if (conn.Vorlauf && conn.Distributor == this) {
								for (int index = conn.DistributorStartIndex; index < conn.DistributorStartIndex + conn.NrOfCircuits; index++) {
									openOutputs.Remove(index);
								}
							}
						}
					}
				}
			}
			return openOutputs;
		}

		public PossibleProductConnection GetPossibleProductConnections(bool input, bool output, double measure, bool invertYAxis, Point2D currentMousePoint1, Product product, Floor floor, int nrOfCircuits, bool useFirstConnection, double moveMousePoint) {
			double width = this.Width * measure;
			double height = this.Height * measure;
			double connectionWidth = 0.055 * measure;
			double border = (width - this.maxCircuits * connectionWidth) / 2.0;

			Point2D leftBottom = Point2D.Zero;
			Point2D leftTop = Point2D.Zero;
			Point2D rightTop = Point2D.Zero;
			Point2D rightBottom = Point2D.Zero;
			Matrix3D transformation = Matrix3D.Identity;

			PossibleProductConnection possibleConnection = null;

			Nullable<GraphicalRepresentation> representation = null;
			foreach (GraphicalRepresentation gr in this.graphicalRepresentations) {
				if (gr.floorId == floor.Id) {
					representation = gr;
					break;
				}
			}

			if (representation == null) {
				return possibleConnection;
			}

			List<int> openInputs = this.GetOpenInputs();
			List<int> openOutputs = this.GetOpenOutputs();

			Point2D mousePoint;

			if (invertYAxis) {
				transformation = transformation * Transformation3D.Translation(representation.Value.position.X, representation.Value.position.Y);
				transformation = transformation * Transformation3D.Rotate(-representation.Value.rotation * Math.PI / 180.0);
				transformation = transformation * Transformation3D.Translation(-representation.Value.position.X, -representation.Value.position.Y);
				leftBottom = transformation.Transform(new Point2D(representation.Value.position.X, representation.Value.position.Y));
				leftTop = transformation.Transform(new Point2D(representation.Value.position.X, representation.Value.position.Y + height));
				rightTop = transformation.Transform(new Point2D(representation.Value.position.X + width, representation.Value.position.Y + height));
				rightBottom = transformation.Transform(new Point2D(representation.Value.position.X + width, representation.Value.position.Y));
				mousePoint = currentMousePoint1 + new Vector2D(moveMousePoint * measure * Math.Sin((representation.Value.rotation + 90.0) * Math.PI / 180.0), moveMousePoint * measure * Math.Cos((representation.Value.rotation + 90.0) * Math.PI / 180.0));
			} else {
				transformation = transformation * Transformation3D.Translation(representation.Value.position.X, representation.Value.position.Y);
				transformation = transformation * Transformation3D.Rotate(representation.Value.rotation * Math.PI / 180.0);
				transformation = transformation * Transformation3D.Translation(-representation.Value.position.X, -representation.Value.position.Y);
				leftBottom = transformation.Transform(new Point2D(representation.Value.position.X, representation.Value.position.Y));
				leftTop = transformation.Transform(new Point2D(representation.Value.position.X, representation.Value.position.Y - height));
				rightTop = transformation.Transform(new Point2D(representation.Value.position.X + width, representation.Value.position.Y - height));
				rightBottom = transformation.Transform(new Point2D(representation.Value.position.X + width, representation.Value.position.Y));
				mousePoint = currentMousePoint1 + new Vector2D(moveMousePoint * measure * Math.Sin((representation.Value.rotation - 90.0) * Math.PI / 180.0), moveMousePoint * measure * Math.Cos((representation.Value.rotation + 90.0) * Math.PI / 180.0));
			}
			Polygon2D distPoly = new Polygon2D(new Point2D[] { leftBottom, leftTop, rightTop, rightBottom });
			if (!distPoly.IsInside(currentMousePoint1)) {
				return possibleConnection;
			}

			Dictionary<int, Point2D> possibleStarts = new Dictionary<int, Point2D>();
			for (int i = 0; i < this.maxCircuits - nrOfCircuits; i++) {
				bool ok = true;
				for (int j = i; j < i + nrOfCircuits; j++) {
					if (output && !openOutputs.Contains(j)) {
						ok = false;
						break;
					}
					if (input && !openInputs.Contains(j)) {
						ok = false;
						break;
					}
				}
				if (ok) {
					if (invertYAxis) {
						possibleStarts.Add(i, transformation.Transform(new Point2D(representation.Value.position.X + border + (i + (useFirstConnection ? 1 : nrOfCircuits) / 2.0) * connectionWidth, representation.Value.position.Y + height / 2.0)));
					} else {
						possibleStarts.Add(i, transformation.Transform(new Point2D(representation.Value.position.X + border + (i + (useFirstConnection ? 1 : nrOfCircuits) / 2.0) * connectionWidth, representation.Value.position.Y - height / 2.0)));
					}
				}
			}
			if (possibleStarts.Count == 0) {
				return possibleConnection;
			}

			double bestDist = double.MaxValue;
			int bestStart = -1;
			foreach (KeyValuePair<int, Point2D> start in possibleStarts) {
				double dist = (start.Value - mousePoint).GetLength();
				if (dist < bestDist) {
					bestStart = start.Key;
					bestDist = dist;
				}
			}
			if (invertYAxis) {
				leftBottom = transformation.Transform(new Point2D(representation.Value.position.X + border + bestStart * connectionWidth, representation.Value.position.Y));
				leftTop = transformation.Transform(new Point2D(representation.Value.position.X + border + bestStart * connectionWidth, representation.Value.position.Y + height));
				rightTop = transformation.Transform(new Point2D(representation.Value.position.X + border + (bestStart + nrOfCircuits) * connectionWidth, representation.Value.position.Y + height));
				rightBottom = transformation.Transform(new Point2D(representation.Value.position.X + border + (bestStart + nrOfCircuits) * connectionWidth, representation.Value.position.Y));
				possibleConnection = new PossibleProductConnection(transformation.Transform(new Point2D(representation.Value.position.X + border + (bestStart + nrOfCircuits / 2.0) * connectionWidth, representation.Value.position.Y + height * 0.5)), new Polygon2D(new Point2D[] { leftBottom, leftTop, rightTop, rightBottom }), input, output, representation.Value.rotation, this, bestStart, nrOfCircuits);
			} else {
				leftBottom = transformation.Transform(new Point2D(representation.Value.position.X + border + bestStart * connectionWidth, representation.Value.position.Y));
				leftTop = transformation.Transform(new Point2D(representation.Value.position.X + border + bestStart * connectionWidth, representation.Value.position.Y - height));
				rightTop = transformation.Transform(new Point2D(representation.Value.position.X + border + (bestStart + nrOfCircuits) * connectionWidth, representation.Value.position.Y - height));
				rightBottom = transformation.Transform(new Point2D(representation.Value.position.X + border + (bestStart + nrOfCircuits) * connectionWidth, representation.Value.position.Y));
				possibleConnection = new PossibleProductConnection(transformation.Transform(new Point2D(representation.Value.position.X + border + (bestStart + nrOfCircuits / 2.0) * connectionWidth, representation.Value.position.Y - height * 0.5)), new Polygon2D(new Point2D[] { leftBottom, leftTop, rightTop, rightBottom }), input, output, representation.Value.rotation, this, bestStart, nrOfCircuits);
			}

			return possibleConnection;
		}

		[XmlIgnore]
		public bool AreProductsConnected {
			get {
				foreach (Floor floor in Project.Instance.Floors) {
					foreach (Room room in floor.Rooms) {
						foreach (PlannedProduct pp in room.PlannedProducts) {
							foreach (GraphicalProductConnection conn in pp.Product.Connections) {
								if (conn.Distributor == this) {
									return true;
								}
							}
						}
					}
				}
				return false;
			}
		}

		public bool IsInsideProduct(Product product) {
			if (product.GraphicalArea == null || product.GraphicalArea.Count < 3) {
				return false;
			}
			Nullable<GraphicalRepresentation> representation = null;
			Floor floor = product.AssociatedRoom.AssociatedFloor;
			foreach (GraphicalRepresentation gr in this.graphicalRepresentations) {
				if (gr.floorId == floor.Id) {
					representation = gr;
					break;
				}
			}

			return representation.HasValue && product.GraphicalArea.IsInside(representation.Value.position);
		}

		public bool IsPointInside(Point2D point, Floor floor, double measure, bool invertYAxis) {
			double width = this.Width * measure;
			double height = this.Height * measure;
			double connectionWidth = 0.055 * measure;
			double border = (width - this.maxCircuits * connectionWidth) / 2.0;

			Point2D leftBottom = Point2D.Zero;
			Point2D leftTop = Point2D.Zero;
			Point2D rightTop = Point2D.Zero;
			Point2D rightBottom = Point2D.Zero;
			Matrix3D transformation = Matrix3D.Identity;

			Nullable<GraphicalRepresentation> representation = null;
			foreach (GraphicalRepresentation gr in this.graphicalRepresentations) {
				if (gr.floorId == floor.Id) {
					representation = gr;
					break;
				}
			}

			if (representation == null) {
				return false;
			}

			List<int> openInputs = this.GetOpenInputs();
			List<int> openOutputs = this.GetOpenOutputs();

			if (invertYAxis) {
				transformation = transformation * Transformation3D.Translation(representation.Value.position.X, representation.Value.position.Y);
				transformation = transformation * Transformation3D.Rotate(-representation.Value.rotation * Math.PI / 180.0);
				transformation = transformation * Transformation3D.Translation(-representation.Value.position.X, -representation.Value.position.Y);
				leftBottom = transformation.Transform(new Point2D(representation.Value.position.X, representation.Value.position.Y));
				leftTop = transformation.Transform(new Point2D(representation.Value.position.X, representation.Value.position.Y + height));
				rightTop = transformation.Transform(new Point2D(representation.Value.position.X + width, representation.Value.position.Y + height));
				rightBottom = transformation.Transform(new Point2D(representation.Value.position.X + width, representation.Value.position.Y));
			} else {
				transformation = transformation * Transformation3D.Translation(representation.Value.position.X, representation.Value.position.Y);
				transformation = transformation * Transformation3D.Rotate(representation.Value.rotation * Math.PI / 180.0);
				transformation = transformation * Transformation3D.Translation(-representation.Value.position.X, -representation.Value.position.Y);
				leftBottom = transformation.Transform(new Point2D(representation.Value.position.X, representation.Value.position.Y));
				leftTop = transformation.Transform(new Point2D(representation.Value.position.X, representation.Value.position.Y - height));
				rightTop = transformation.Transform(new Point2D(representation.Value.position.X + width, representation.Value.position.Y - height));
				rightBottom = transformation.Transform(new Point2D(representation.Value.position.X + width, representation.Value.position.Y));
			}
			Polygon2D distPoly = new Polygon2D(new Point2D[] { leftBottom, leftTop, rightTop, rightBottom });
			return distPoly.IsInside(point);
		}

		/*public List<GraphicalConnectionAnbindungsPunkt> GetPossibleAnbindungsPunkte(Product product, double measure, bool invertYAxis, bool input, int distributorIndex) {
			double width = this.Width * measure;
			double height = this.Height * measure;
			double connectionWidth = 0.055 * measure;
			double border = (width - this.maxCircuits * connectionWidth) / 2.0;

			Point2D leftBottom = Point2D.Zero;
			Point2D leftTop = Point2D.Zero;
			Point2D rightTop = Point2D.Zero;
			Point2D rightBottom = Point2D.Zero;
			Matrix3D transformation = Matrix3D.Identity;

			Nullable<GraphicalRepresentation> representation = null;
			Floor floor = product.AssociatedRoom.AssociatedFloor;
			foreach (GraphicalRepresentation gr in this.graphicalRepresentations) {
				if (gr.floorId == floor.Id) {
					representation = gr;
					break;
				}
			}

			if (representation == null) {
				return new List<GraphicalConnectionAnbindungsPunkt>();
			}

			List<int> openInputs = this.GetOpenInputs();
			List<int> openOutputs = this.GetOpenOutputs();

			if (invertYAxis) {
				transformation = transformation * Transformation3D.Translation(representation.Value.position.X, representation.Value.position.Y);
				transformation = transformation * Transformation3D.Rotate(-representation.Value.rotation * Math.PI / 180.0);
				transformation = transformation * Transformation3D.Translation(-representation.Value.position.X, -representation.Value.position.Y);
				//leftBottom = transformation.Transform(new Point2D(representation.Value.position.X, representation.Value.position.Y));
				//leftTop = transformation.Transform(new Point2D(representation.Value.position.X, representation.Value.position.Y + height));
				//rightTop = transformation.Transform(new Point2D(representation.Value.position.X + width, representation.Value.position.Y + height));
				//rightBottom = transformation.Transform(new Point2D(representation.Value.position.X + width, representation.Value.position.Y));
			} else {
				transformation = transformation * Transformation3D.Translation(representation.Value.position.X, representation.Value.position.Y);
				transformation = transformation * Transformation3D.Rotate(representation.Value.rotation * Math.PI / 180.0);
				transformation = transformation * Transformation3D.Translation(-representation.Value.position.X, -representation.Value.position.Y);
				//leftBottom = transformation.Transform(new Point2D(representation.Value.position.X, representation.Value.position.Y));
				//leftTop = transformation.Transform(new Point2D(representation.Value.position.X, representation.Value.position.Y - height));
				//rightTop = transformation.Transform(new Point2D(representation.Value.position.X + width, representation.Value.position.Y - height));
				//rightBottom = transformation.Transform(new Point2D(representation.Value.position.X + width, representation.Value.position.Y));
			}
			//Polygon2D distPoly = new Polygon2D(new Point2D[] { leftBottom, leftTop, rightTop, rightBottom });
			//if (!distPoly.IsInside(currentMousePoint)) {
			//	return possibleConnection;
			//}

			Dictionary<int, Point2D> possibleStarts = new Dictionary<int, Point2D>();
			List<GraphicalConnectionAnbindungsPunkt> anbindungsPunkte = new List<GraphicalConnectionAnbindungsPunkt>();
			for (int i = 0; i < this.maxCircuits; i++) {
				if ((distributorIndex < 0 || distributorIndex == i) && ((!input && !openOutputs.Contains(i)) || (input && !openInputs.Contains(i)))) {
					if (invertYAxis) {
						Point2D connectionPoint = transformation.Transform(new Point2D(representation.Value.position.X + border + (i + 0.5) * connectionWidth, representation.Value.position.Y + height / 2.0));
						leftBottom = transformation.Transform(new Point2D(representation.Value.position.X + border + i * connectionWidth, representation.Value.position.Y));
						leftTop = transformation.Transform(new Point2D(representation.Value.position.X + border + i * connectionWidth, representation.Value.position.Y + height));
						rightTop = transformation.Transform(new Point2D(representation.Value.position.X + border + (i + 1) * connectionWidth, representation.Value.position.Y + height));
						rightBottom = transformation.Transform(new Point2D(representation.Value.position.X + border + (i + 1) * connectionWidth, representation.Value.position.Y));
						Polygon2D connectionArea = new Polygon2D(new Point2D[] { leftBottom, leftTop, rightTop, rightBottom });
						GraphicalConnectionAnbindungsPunkt anbindungsPunkt = new GraphicalConnectionAnbindungsPunkt(connectionPoint, connectionArea, i);
						anbindungsPunkte.Add(anbindungsPunkt);
					} else {
						Point2D connectionPoint = transformation.Transform(new Point2D(representation.Value.position.X + border + (i + 0.5) * connectionWidth, representation.Value.position.Y - height / 2.0));
						leftBottom = transformation.Transform(new Point2D(representation.Value.position.X + border + i * connectionWidth, representation.Value.position.Y));
						leftTop = transformation.Transform(new Point2D(representation.Value.position.X + border + i * connectionWidth, representation.Value.position.Y - height));
						rightTop = transformation.Transform(new Point2D(representation.Value.position.X + border + (i + 1) * connectionWidth, representation.Value.position.Y - height));
						rightBottom = transformation.Transform(new Point2D(representation.Value.position.X + border + (i + 1) * connectionWidth, representation.Value.position.Y));
						Polygon2D connectionArea = new Polygon2D(new Point2D[] { leftBottom, leftTop, rightTop, rightBottom });
						GraphicalConnectionAnbindungsPunkt anbindungsPunkt = new GraphicalConnectionAnbindungsPunkt(connectionPoint, connectionArea, i);
						anbindungsPunkte.Add(anbindungsPunkt);
					}
				}
			}
			return anbindungsPunkte;
		}*/

		public void Draw(System.Drawing.Graphics g, Matrix4D additionalTransformation, double measure, bool invertYAxis, Floor floor) {
			g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
			System.Drawing.Pen pen = System.Drawing.Pens.Red;
			double width = this.Width * measure;
			double height = this.Height * measure;

			Point2D leftBottom = Point2D.Zero;
			Point2D leftTop = Point2D.Zero;
			Point2D rightTop = Point2D.Zero;
			Point2D rightBottom = Point2D.Zero;

			foreach (Distributor.GraphicalRepresentation gp in this.GraphicalRepresentations) {
				if (gp.floorId == floor.Id) {
					if (invertYAxis) {
						Matrix4D transformation = additionalTransformation * Transformation4D.Translation(gp.position.X, gp.position.Y, 0);
						transformation = transformation * Transformation4D.RotateZ(-gp.rotation * Math.PI / 180.0);
						transformation = transformation * Transformation4D.Translation(-gp.position.X, -gp.position.Y, 0);

						leftBottom = transformation.TransformTo2D(gp.position);
						leftTop = transformation.TransformTo2D(new Point2D(gp.position.X, gp.position.Y + height));
						rightTop = transformation.TransformTo2D(new Point2D(gp.position.X + width, gp.position.Y + height));
						rightBottom = transformation.TransformTo2D(new Point2D(gp.position.X + width, gp.position.Y));
					} else {
						Matrix4D transformation = additionalTransformation * Transformation4D.Translation(gp.position.X, gp.position.Y, 0);
						transformation = transformation * Transformation4D.RotateZ(gp.rotation * Math.PI / 180.0);
						transformation = transformation * Transformation4D.Translation(-gp.position.X, -gp.position.Y, 0);

						leftBottom = transformation.TransformTo2D(gp.position);
						leftTop = transformation.TransformTo2D(new Point2D(gp.position.X, gp.position.Y - height));
						rightTop = transformation.TransformTo2D(new Point2D(gp.position.X + width, gp.position.Y - height));
						rightBottom = transformation.TransformTo2D(new Point2D(gp.position.X + width, gp.position.Y));
					}
					g.DrawLine(pen, (float)leftBottom.X, (float)leftBottom.Y, (float)rightBottom.X, (float)rightBottom.Y);
					g.DrawLine(pen, (float)rightBottom.X, (float)rightBottom.Y, (float)rightTop.X, (float)rightTop.Y);
					g.DrawLine(pen, (float)rightTop.X, (float)rightTop.Y, (float)leftTop.X, (float)leftTop.Y);
					g.DrawLine(pen, (float)leftTop.X, (float)leftTop.Y, (float)leftBottom.X, (float)leftBottom.Y);
					g.FillPolygon(System.Drawing.Brushes.Red, new System.Drawing.PointF[] { new System.Drawing.PointF((float)leftBottom.X, (float)leftBottom.Y), new System.Drawing.PointF((float)rightBottom.X, (float)rightBottom.Y), new System.Drawing.PointF((float)rightTop.X, (float)rightTop.Y) });
					break;
				}
			}
		}

		public void DrawDxf(DxfModel model, DxfLayer distributorLayer, Floor floor, double measure) {
			Matrix4D additionalTransformation = Matrix4D.Identity;

			double width = this.Width * measure;
			double height = this.Height * measure;

			Point2D leftBottom = Point2D.Zero;
			Point2D leftTop = Point2D.Zero;
			Point2D rightTop = Point2D.Zero;
			Point2D rightBottom = Point2D.Zero;

			EntityColor red = EntityColor.CreateFromRgb(System.Drawing.Color.Red.ToArgb());

			foreach (Distributor.GraphicalRepresentation gp in this.GraphicalRepresentations) {
				if (gp.floorId == floor.Id) {
				
					Matrix4D transformation = additionalTransformation * Transformation4D.Translation(gp.position.X, gp.position.Y, 0);
					transformation = transformation * Transformation4D.RotateZ(-gp.rotation * Math.PI / 180.0);
					transformation = transformation * Transformation4D.Translation(-gp.position.X, -gp.position.Y, 0);

					leftBottom = transformation.TransformTo2D(gp.position);
					leftTop = transformation.TransformTo2D(new Point2D(gp.position.X, gp.position.Y + height));
					rightTop = transformation.TransformTo2D(new Point2D(gp.position.X + width, gp.position.Y + height));
					rightBottom = transformation.TransformTo2D(new Point2D(gp.position.X + width, gp.position.Y));
					

					DxfLine line = new DxfLine(red, leftBottom, rightBottom);
					line.Layer = distributorLayer;
					model.Entities.Add(line);
					line = new DxfLine(red, rightBottom, rightTop);
					line.Layer = distributorLayer;
					model.Entities.Add(line);
					line = new DxfLine(red, rightTop, leftTop);
					line.Layer = distributorLayer;
					model.Entities.Add(line);
					line = new DxfLine(red, leftTop, leftBottom);
					line.Layer = distributorLayer;
					model.Entities.Add(line);

					DxfHatch hatch = new DxfHatch();
					hatch.Color = red;
					DxfHatch.BoundaryPath boundaryPath = new DxfHatch.BoundaryPath();
					boundaryPath.Type = BoundaryPathType.Polyline;
					boundaryPath.PolylineData = new DxfHatch.BoundaryPath.Polyline(new Point2D[] { leftBottom, rightBottom, rightTop});
					boundaryPath.PolylineData.Closed = true;
					hatch.BoundaryPaths.Add(boundaryPath);

					hatch.Layer = distributorLayer;
					model.Entities.Add(hatch);

					//g.FillPolygon(System.Drawing.Brushes.Red, new System.Drawing.PointF[] { new System.Drawing.PointF((float)leftBottom.X, (float)leftBottom.Y), new System.Drawing.PointF((float)rightBottom.X, (float)rightBottom.Y), new System.Drawing.PointF((float)rightTop.X, (float)rightTop.Y) });
					break;
				}
			}
		}

	}
}
