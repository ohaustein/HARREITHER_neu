using System;
using System.Collections.Generic;
using System.Text;
using log4net;
using System.Windows.Forms;
using System.Threading;
using Europlan.Common;
using System.Xml.Serialization;
using System.Drawing;
using WW.Math;

namespace Europlan.Common {

	[Serializable()]
	public class Room : IGuiRepresentation, IClipboard {

		public enum RoomController {
			None,
			RC,
			RCF,
			RCRadio,
			RF
		}

		private string id;
		private string internalId;
		private string name;
		private int roomHeatTemperature;
		private int roomCoolTemperature;
		private int roomRelativeHumidity;
		private float area;
		private int heatLoad;
		private int coolLoad;
		private int floorHeatingLoss;
		private bool nassraum = false;
		private int quickDimensioningRoomTemperature;
		private int quickDimensioningHeatLoad;
		private int quickDimensioningCoolLoad;
		private int quickDimensioningNrOfServos;
		private RoomController quickDimensioningRoomController = RoomController.None;
		private string quickDimensioningComments;
		private int additionalHeatLoad;
		private string quickDimensioningRoomTypeId;
		private bool quickDimensioningInitialized = false;

		private List<Product> usedProductsForQuickDimensioning;
		private List<PlannedProduct> plannedProducts;
		private List<Point2D> roomCoordinates;
		private List<List<Point2D>> roomUnusedAreaCoordinates;
		private List<Point2D> ceilingCoordinates;
		private List<List<Point2D>> ceilingUnusedAreaCoordinates;
		private List<GraphicalWall> walls;
		private List<double> helpLines;
		private Nullable<double> planSettingX;
		private Nullable<double> planSettingY;
		private Nullable<double> planSettingScale;
		private Nullable<double> planSettingAngle;

		private TreeNode roomNode = new TreeNode();

		[NonSerialized]
		private static readonly ILog log = LogManager.GetLogger(typeof(Room));

		public Room() {
			InitializeRoom();
			this.Name = "";
		}

		public Room(string name) {
			InitializeRoom();
			this.Name = name;
		}

		public Room(Room room) {
			InitializeRoom();
			string copyOf = EuroplanRes.General_KopieVon;
			this.Name = copyOf + " " + room.Name;
			this.roomHeatTemperature = room.RoomHeatTemperature;
			this.roomCoolTemperature = room.RoomCoolTemperature;
			this.roomRelativeHumidity = room.RoomRelativeHumidity;
			this.area = room.Area;
			this.heatLoad = room.HeatLoad;
			this.coolLoad = room.CoolLoad;
			this.floorHeatingLoss = room.FloorHeatingLoss;
			this.quickDimensioningHeatLoad = room.QuickDimensioningHeatLoad;
			this.quickDimensioningCoolLoad = room.QuickDimensioningCoolLoad;
			this.quickDimensioningNrOfServos = room.quickDimensioningNrOfServos;
			this.quickDimensioningRoomController = room.QuickDimensioningRoomController;
			this.quickDimensioningComments = room.QuickDimensioningComments;
			this.quickDimensioningRoomTypeId = room.quickDimensioningRoomTypeId; // used the member instead of the public property on purpose here!
			this.usedProductsForQuickDimensioning = new List<Product>();
			this.plannedProducts = new List<PlannedProduct>();
			//foreach (Product product in room.UsedProductsForQuickDimensioning) {
			//    this.usedProductsForQuickDimensioning.Add(product.Clone(this));
			//}
			this.roomCoordinates.Clear();
			this.roomCoordinates.AddRange(room.roomCoordinates);
			this.ceilingCoordinates.Clear();
			this.ceilingCoordinates.AddRange(room.ceilingCoordinates);
			this.roomUnusedAreaCoordinates.Clear();
			this.roomUnusedAreaCoordinates.AddRange(room.roomUnusedAreaCoordinates);
			this.ceilingUnusedAreaCoordinates.Clear();
			this.ceilingUnusedAreaCoordinates.AddRange(room.ceilingUnusedAreaCoordinates);
			this.walls.Clear();
			this.walls.AddRange(room.walls);
			this.helpLines.Clear();
			this.helpLines.AddRange(room.helpLines);
		}

		private void InitializeRoom() {
			id = "";
			this.internalId = System.Guid.NewGuid().ToString();
			name = "";
			roomHeatTemperature = 0;
			roomCoolTemperature = Project.Instance.InsideTemperatureForCooling;
			roomRelativeHumidity = Project.Instance.RelativeHumidity;
			area = 0;
			heatLoad = 0;
			coolLoad = 0;
			floorHeatingLoss = 0;
			roomNode.Tag = this;
			quickDimensioningRoomTypeId = "";
			this.usedProductsForQuickDimensioning = new List<Product>();
			this.plannedProducts = new List<PlannedProduct>();
			this.quickDimensioningHeatLoad = 0;
			this.quickDimensioningCoolLoad = 0;
			this.quickDimensioningNrOfServos = -1;
			this.quickDimensioningRoomController = RoomController.None;
			this.quickDimensioningComments = "";
            this.roomNode.ImageKey = "Raum.png";
            this.roomNode.SelectedImageKey = "Raum.png";
			this.roomCoordinates = new List<Point2D>();
			this.ceilingCoordinates = new List<Point2D>();
			this.roomUnusedAreaCoordinates = new List<List<Point2D>>();
			this.ceilingUnusedAreaCoordinates = new List<List<Point2D>>();
			this.walls = new List<GraphicalWall>();
			this.helpLines = new List<double>();
		}

		internal void Synchronize(Room room) {
			this.Name = room.Name;
			this.RoomHeatTemperature = room.RoomHeatTemperature;
			this.RoomCoolTemperature = room.RoomCoolTemperature;
			this.RoomRelativeHumidity = room.RoomRelativeHumidity;
			this.Area = room.Area;
			this.HeatLoad = room.HeatLoad;
			this.CoolLoad = room.CoolLoad;
			this.NormalizedHeatLoad = room.NormalizedHeatLoad;
			this.NormalizedCoolLoad = room.NormalizedCoolLoad;
			this.quickDimensioningHeatLoad = room.QuickDimensioningHeatLoad;
			this.quickDimensioningCoolLoad = room.QuickDimensioningCoolLoad;
			this.quickDimensioningNrOfServos = room.quickDimensioningNrOfServos;
			this.quickDimensioningRoomController = room.QuickDimensioningRoomController;
			this.quickDimensioningComments = room.QuickDimensioningComments;
			this.quickDimensioningRoomTypeId = room.QuickDimensioningRoomTypeId;
			this.usedProductsForQuickDimensioning = new List<Product>();
			foreach (Product product in room.UsedProductsForQuickDimensioning) {
				this.usedProductsForQuickDimensioning.Add(product.Clone(this));
			}
			this.plannedProducts = new List<PlannedProduct>();
			foreach (PlannedProduct product in room.PlannedProducts) {
				this.plannedProducts.Add(new PlannedProduct(product.Product.Clone(this)));
			}
			this.roomCoordinates.Clear();
			this.roomCoordinates.AddRange(room.roomCoordinates);
			this.ceilingCoordinates.Clear();
			this.ceilingCoordinates.AddRange(room.ceilingCoordinates);
			this.roomUnusedAreaCoordinates.Clear();
			this.roomUnusedAreaCoordinates.AddRange(room.roomUnusedAreaCoordinates);
			this.ceilingUnusedAreaCoordinates.Clear();
			this.ceilingUnusedAreaCoordinates.AddRange(room.ceilingUnusedAreaCoordinates);
			this.walls.Clear();
			this.walls.AddRange(room.walls);
			this.helpLines.Clear();
			this.helpLines.AddRange(room.helpLines);
		}

		[XmlIgnore]
		public Floor AssociatedFloor {
			get {
				foreach (Floor f in Project.Instance.Floors) {
					foreach (Room room in f.Rooms) {
						if (room == this) {
							return f;
						}
					}
				}
				return null;
			}			
		}

		[XmlIgnore]
		public Plan AssociatedPlan {
			get {
				if (this.AssociatedFloor == null || this.AssociatedFloor.AssociatedPlanId == null) {
					return null;
				}
				foreach (Plan plan in Project.Instance.ImportedPlans) {
					if (plan.Id.Equals(this.AssociatedFloor.AssociatedPlanId)) {
						return plan;
					}
				}
				return null;
			}
		}

		public string Id {
			get { return id; }
			set { 
				id = value;
				if (roomNode != null) {
					roomNode.Text = (String.IsNullOrEmpty(name) ? EuroplanRes.Room_Unbenannt : id + ": " + name);
				}		
			}
		}

		public string InternalId {
			get { return internalId; }
			set { internalId = value; }
		}
		
		public string Name {
			get { return name; }
			set { 
				name = value;
				if (roomNode != null) {
					roomNode.Text = (String.IsNullOrEmpty(name) ? EuroplanRes.Room_Unbenannt : id + ": " + name);
				}
			}
		}

		public string QuickDimensioningRoomTypeId {
			get { return quickDimensioningRoomTypeId; }
			set { quickDimensioningRoomTypeId = value; }
		}

		[XmlIgnore]
		public RoomType QuickDimensioningRoomType{
			get {
				if (this.QuickDimensioningRoomTypeId != "") {
					foreach (RoomType type in Project.Instance.Config.RoomTypes) {
						if (this.QuickDimensioningRoomTypeId == type.Id) {
							return type;
						}
					}
				} else if (Project.Instance.Config.RoomTypes.Count > 0) {
					this.QuickDimensioningRoomTypeId = Project.Instance.Config.RoomTypes[0].Id;
					return Project.Instance.Config.RoomTypes[0];
				}
				return null;
			}
			set { this.QuickDimensioningRoomTypeId = value.Id;}
		}

		public int RoomHeatTemperature {
			get { return roomHeatTemperature; }
			set { roomHeatTemperature = value; }
		}

		public int RoomCoolTemperature {
			get { return roomCoolTemperature; }
			set { roomCoolTemperature = value; }
		}

		public int RoomRelativeHumidity {
			get { return roomRelativeHumidity; }
			set { roomRelativeHumidity = value; }
		}

		public float Area {
			get { return area; }
			set { area = value; }
		}

		public int HeatLoad {
			get { return heatLoad; }
			set {
				heatLoad = value;
				this.CorrectPlanning();
			}
		}

		public bool IsNassraum {
			get { return nassraum; }
			set { nassraum = value; }
		}

		[XmlIgnore]
		public double OpenHeatLoad {
			get {
				double covered = 0;
				foreach (PlannedProduct pp in this.plannedProducts) {
					if (!pp.PlannedHeatLoad.Equals(double.NaN)) {
						covered += pp.PlannedHeatLoad;
					}
					covered += pp.Product.PlannedHeizlastBereinigung;
				}
				return covered - this.NormalizedHeatLoad;
			}
		}

		public int CoolLoad {
			get { return coolLoad; }
			set { coolLoad = value; }
		}

		[XmlIgnore]
		public double OpenCoolLoad {
			get {
				double covered = 0;
				foreach (PlannedProduct pp in this.plannedProducts) {
					if (!pp.PlannedCoolLoad.Equals(double.NaN)) {
						covered += pp.PlannedCoolLoad;
					}
					covered += pp.Product.PlannedKuehllastBereinigung;
				}
				return covered - this.NormalizedCoolLoad;
			}
		}

		[XmlIgnore]
		public int NormalizedHeatLoad {
			get { return heatLoad - floorHeatingLoss - additionalHeatLoad; }
			set { floorHeatingLoss = heatLoad - additionalHeatLoad - value; }
		}

		[XmlIgnore]
		public int NormalizedCoolLoad {
			get { return coolLoad; }
			set { /*TODO*/; }
		}

		public int AdditionalHeatLoad {
			get { return additionalHeatLoad; }
			set { additionalHeatLoad = value; }
		}

		public int FloorHeatingLoss {
			get { return floorHeatingLoss; }
			set { floorHeatingLoss = value; }
		}

		private void CorrectPlanning() {
			foreach (PlannedProduct pp in this.PlannedProducts) {
				pp.Product.ConfigureProduct(pp.RequestedHeatLoad, pp.RequestedCoolLoad, pp.CalculateHeat, pp.CalculateCool, false);
			}
		}

		public int QuickDimensioningHeatLoad {
			get { return quickDimensioningHeatLoad; }
			set { quickDimensioningHeatLoad = value; }
		}

		public int GetDefaultQuickDimensioningHeatLoad() {
			if (this.QuickDimensioningRoomType != null) {
				return (int)(this.area * this.QuickDimensioningRoomType.HeatLoadPerSquareMeter);
			}
			return 0;
		}

		public int QuickDimensioningCoolLoad {
			get { return quickDimensioningCoolLoad; }
			set { quickDimensioningCoolLoad = value; }
		}

		public int GetDefaultQuickDimensioningCoolLoad() {
			if (this.QuickDimensioningRoomType != null) {
				return (int)(this.area * this.QuickDimensioningRoomType.CoolLoadPerSquareMeter);
			}
			return 0;
		}

		private int GetDefaultNrOfServos() {
			if (this.QuickDimensioningRoomController != RoomController.None) {
				int nr = 0;
				foreach (Product product in usedProductsForQuickDimensioning) {
					nr += product.QuickDimensioningCircuits;
				}
				return nr;
			}
			return 0;
		}

		public int QuickDimensioningNrOfServos {
			get {
				if (this.quickDimensioningNrOfServos >= 0) {
					return this.quickDimensioningNrOfServos;
				}
				return this.GetDefaultNrOfServos();
			}
			set {
				if (value == this.GetDefaultNrOfServos()) {
					this.quickDimensioningNrOfServos = -1;
				} else {
					this.quickDimensioningNrOfServos = value;
				}
			}
		}

		public string QuickDimensioningComments {
			get { return quickDimensioningComments; }
			set { quickDimensioningComments = value; }
		}

		public RoomController QuickDimensioningRoomController {
			get { return quickDimensioningRoomController; }
			set { quickDimensioningRoomController = value; }
		}

		public bool QuickDimensioningHeatLoadCovered {
			get { return this.QuickDimensioningCoveredHeatLoad >= this.quickDimensioningHeatLoad; }
		}

		public bool QuickDimensioningCoolLoadCovered {
			get { return this.QuickDimensioningCoveredCoolLoad >= this.quickDimensioningCoolLoad; }
		}

		public int QuickDimensioningCoveredHeatLoad {
			get {
				int coveredLoad = 0;
				foreach (Product product in this.UsedProductsForQuickDimensioning) {
					coveredLoad += product.QuickDimensioningHeatPower;
				}
				return coveredLoad;
			}
		}

		public int QuickDimensioningCoveredCoolLoad {
			get {
				int coveredLoad = 0;
				foreach (Product product in this.UsedProductsForQuickDimensioning) {
					coveredLoad += product.QuickDimensioningCoolPower;
				}
				return coveredLoad;
			}
		}

		public int QuickDimensioningRoomTemperature {
			get { return quickDimensioningRoomTemperature; }
			set { quickDimensioningRoomTemperature = value; }
		}

		public List<Product> UsedProductsForQuickDimensioning {
			get { return usedProductsForQuickDimensioning; }
			set { usedProductsForQuickDimensioning = value; }
		}

		public List<PlannedProduct> PlannedProducts {
			get { return plannedProducts; }
			set {
				plannedProducts = value;
				foreach (PlannedProduct p in plannedProducts) {
					p.Product.AssociatedRoom = this;
				}
			}
		}

		public P GetProductForQuickDimensioning<P>() where P : Product {
			foreach (Product product in this.usedProductsForQuickDimensioning) {
				if (product.GetType() == typeof(P)) {
					return (P)product;
				}
			}
			return null;
		}
		
		/*internal void InitializeTree(TreeNode floor) {
			roomNode.Tag = this;
			floor.Nodes.Add(roomNode);
		}*/

		public Type AssociatedPanelType {
			get {
				return typeof(RoomSummaryPanel);
			}
		}

		public TreeNode FindNode(object element) {
			if (element == this) {
				return roomNode;
			} else {
				foreach (PlannedProduct pp in this.PlannedProducts) {
					TreeNode node = pp.FindNode(element);
					if (node != null) {
						return node;
					}
				}
			}
			return null;
		}

		public bool SupportsCut {
			get { return false; }
		}

		public bool SupportsCopy {
			get { return true; }
		}

		public string DataFormat {
			get { return this.GetType().ToString(); }
		}

		public bool SupportsPaste(string data) {
			return false;
		}

		public string SupportedPasteFormat {
			get { return null; }
		}

		public object Copy() {
			return new Room(this);
		}

		public void Paste(object o) {
			throw new Exception("Paste not supported");
		}

		internal TreeNode Node {
			get { return this.roomNode; }
		}

		public bool QuickDimensioningInitialized {
			get { return this.quickDimensioningInitialized; }
			set { this.quickDimensioningInitialized = value; }
		}

		public void InitializeQuickDimensioning() {
			if (this.quickDimensioningInitialized) {
				log.Warn("trying to initialize quick dimensioning, but it was already initialized before");
			} else {
				int heatload = this.HeatLoad;
				if (heatload <= 0) {
					heatload = this.GetDefaultQuickDimensioningHeatLoad();
				}
				this.QuickDimensioningHeatLoad = heatLoad;
				this.QuickDimensioningRoomTemperature = this.RoomHeatTemperature;
				this.quickDimensioningInitialized = true;
			}
		}

		public void RevertQuickDimensioning() {
			this.usedProductsForQuickDimensioning.Clear();
			this.QuickDimensioningRoomType = Project.Instance.Config.RoomTypes[0];
			this.quickDimensioningComments = "";
			this.quickDimensioningCoolLoad = 0;
			this.quickDimensioningHeatLoad = 0;
			this.quickDimensioningRoomController = RoomController.None;
			this.quickDimensioningNrOfServos = -1;
			this.quickDimensioningRoomTemperature = 0;
			this.quickDimensioningInitialized = false;
			this.InitializeQuickDimensioning();
		}

		internal void UpdateTree() {
			int i = 0;
			bool expand = this.Node.Nodes.Count == 0;
			foreach (PlannedProduct product in this.PlannedProducts) {
				int index = this.Node.Nodes.IndexOf(product.Node);
				if (index < 0) {
					this.Node.Nodes.Insert(i, product.Node);
				} else if (index > i) {
					if (product.Node.IsSelected) {
						for (int j = i; j < index; j++) {
							this.Node.Nodes.RemoveAt(i);
						}
					} else {
						this.Node.Nodes.RemoveAt(index);
						this.Node.Nodes.Insert(i, product.Node);
					}
				}
				i++;
				product.UpdateTree();
			}
			while (this.Node.Nodes.Count > i) {
				this.Node.Nodes.RemoveAt(i);
			}
			if (expand) {
				this.Node.Expand();
			}
		}
	
		internal void FinalizeLoading() {
 			foreach (PlannedProduct pp in this.plannedProducts) {
				pp.FinalizeLoading();
			}
			foreach (Product p in this.usedProductsForQuickDimensioning) {
				p.AssociatedRoom = this;
				p.FinalizeLoading(null);
			}
		}

		public Floor GetFloor() {
			foreach (Floor f in Project.Instance.Floors) {
				foreach (Room r in f.Rooms) {
					if (r == this) {
						return f;
					}
				}
			}
			return null;
		}

		public override string ToString() {
			return this.Id + ": " + this.Name;
		}

		public List<WW.Math.Point2D> RoomCoordinates {
			get { return this.roomCoordinates; }
			set { this.roomCoordinates = value; }
		}

		public List<Point2D> CeilingCoordinates {
			get { return this.ceilingCoordinates; }
			set { this.ceilingCoordinates = value; }
		}

		[XmlIgnore]
		public List<Point2D> CeilingCoordinatesToUse {
			get { return (this.ceilingCoordinates != null && this.ceilingCoordinates.Count > 0 ? this.ceilingCoordinates : this.roomCoordinates); }
		}

		public List<List<Point2D>> RoomUnusedAreaCoordinates {
			get { return this.roomUnusedAreaCoordinates; }
			set { this.roomUnusedAreaCoordinates = value; }
		}

		public List<List<Point2D>> CeilingUnusedAreaCoordinates {
			get { return this.ceilingUnusedAreaCoordinates; }
			set { this.ceilingUnusedAreaCoordinates = value; }
		}

		public List<GraphicalWall> Walls {
			get { return this.walls; }
			set { this.walls = value; }
		}

		public List<double> HelpLines {
			get { return this.helpLines; }
			set { this.helpLines = value; }
		}

		public Nullable<double> PlanSettingX {
			get { return planSettingX; }
			set { planSettingX = value; }
		}

		public Nullable<double> PlanSettingY {
			get { return planSettingY; }
			set { planSettingY = value; }
		}

		public Nullable<double> PlanSettingScale {
			get { return planSettingScale; }
			set { planSettingScale = value; }
		}

		public Nullable<double> PlanSettingAngle {
			get { return planSettingAngle; }
			set { planSettingAngle = value; }
		}


		public GraphicalWall GetWallForId(string wallId) {
			if (this.Walls == null || wallId == null) {
				return null;
			}
			GraphicalWall foundWall = null;
			foreach (GraphicalWall wall in this.Walls) {
				foundWall = wall.GetWallForId(wallId);
				if (foundWall != null) {
					break;
				}
			}
			return foundWall;
		}

		public Nullable<Vector2D> GetWallOffset(GraphicalWall wall) {
			if (this.walls == null) {
				return null;
			}
			double xOffset = 0;
			Nullable<double> yOffset;
			GraphicalWall oldWall = null;
			foreach (GraphicalWall curWall in this.walls) {
				if (oldWall != null && oldWall.PlanEndPoint.HasValue && curWall.PlanStartPoint.HasValue &&
					(oldWall.PlanEndPoint.Value - curWall.PlanStartPoint.Value).GetLength() > 0.000001) {
					xOffset += 0.2;
				}
				yOffset = curWall.GetWallYOffset(wall, 0);
				Nullable<double> dachschraegeXOffset = curWall.GetDachschraegeXOffset(wall, 0);
				if (yOffset.HasValue) {
					return new Vector2D(xOffset + (dachschraegeXOffset.HasValue ? dachschraegeXOffset.Value : 0), yOffset.Value);
				}
				xOffset += curWall.GetWallWidth();
				oldWall = curWall;
			}
			return null;
		}

		public List<WW.Math.Geometry.Polygon2D> GetTotalWallsArea() {
			List<WW.Math.Geometry.Polygon2D> result = new List<WW.Math.Geometry.Polygon2D>();
			if (this.walls == null) {
				return result;
			}
			foreach (GraphicalWall w in this.walls) {
				GraphicalWall wall = w;
				while (wall != null) {
					Vector2D offset = this.GetWallOffset(wall).Value * 100;
					WW.Math.Geometry.Polygon2D wallBorder = wall.GetObjectBorders(offset.X, offset.Y);
					if (wallBorder.IsClockwise()) {
						wallBorder.Reverse();
					}
					if (result.Count == 0) {
						result.Add(wallBorder);
					} else {
						List<WW.Math.Geometry.Polygon2D> list = new List<WW.Math.Geometry.Polygon2D>();
						list.Add(wallBorder);
						result = WW.Math.Geometry.Polygon2D.GetUnion(result, list);
					}
					wall = wall.DachSchraege;
				}
			}
			return result;
		}

		public bool CollisionTest(WW.Math.Geometry.Polygon2D polygon) {
			if (polygon == null || polygon.Count < 1) {
				return false;
			}
			List<WW.Math.Geometry.Polygon2D> walls = this.GetTotalWallsArea();
			List<WW.Math.Geometry.Polygon2D> polyList = new List<WW.Math.Geometry.Polygon2D>();
			if (polygon.IsClockwise()) {
				polygon.Reverse();
			}
			polyList.Add(polygon);
			try {
				List<WW.Math.Geometry.Polygon2D> result = WW.Math.Geometry.Polygon2D.GetDifference(polyList, walls);
				return result != null && result.Count > 0;
			} catch (Exception e) {
				return true;
			}
			/*bool outside = false;
			Point2D firstPoint = polygon[0];
			WW.Math.Geometry.Polygon2D wall = null;
			foreach (WW.Math.Geometry.Polygon2D poly in walls) {
				if (poly.IsInside(firstPoint)) {
					wall = poly;
					break;
				}

			}
			foreach (Point2D point in polygon) {
				if (!WW.Math.Geometry.Polygon2D.IsInside(point, wall)) {
					IList<WW.Math.Geometry.Segment2D> segments = new List<WW.Math.Geometry.Segment2D>();
					WW.Math.Geometry.Polygon2D.GetSegments(wall, segments);
					outside = true;
					foreach (WW.Math.Geometry.Segment2D segment in segments) {
						if (segment.GetDistance(point) < 0.01) {
							outside = false;
						}
					}
					if (outside) {
						break;
					}
				}
			}
			return outside;*/
		}

		public GraphicalWall GetWallForPoint(Point2D planPoint, out double xOffset, out double yOffset) {
			GraphicalWall pickedWall = null;
			foreach (GraphicalWall wall in this.Walls) {
				pickedWall = wall.GetPickedWall(planPoint, this.GetWallOffset(wall).Value.X * 100, 0);
				if (pickedWall != null) {
					break;
				}
			}
			if (pickedWall == null) {
				xOffset = 0;
				yOffset = 0;
			} else {
				Nullable<Vector2D> offset = this.GetWallOffset(pickedWall);
				if (offset.HasValue) {
					xOffset = offset.Value.X * 100;
					yOffset = offset.Value.Y * 100;
				} else {
					xOffset = 0;
					yOffset = 0;
				}
			}
			return pickedWall;
		}

		public GraphicalWall GetOwningWall(IGraphicalWallObject obj) {
			GraphicalWall owner = null;
			foreach (GraphicalWall wall in this.Walls) {
				owner = wall.GetOwningWall(obj);
				if (owner != null) {
					break;
				}
			}
			return owner;
		}

		public List<GraphicalWall> GetAllWalls() {
			List<GraphicalWall> allWalls = new List<GraphicalWall>();
			foreach (GraphicalWall baseWall in this.Walls) {
				GraphicalWall wall = baseWall;
				while (wall != null) {
					allWalls.Add(wall);
					wall = wall.DachSchraege;
				}
			}
			return allWalls;
		}

		public bool MarkErrors(IGraphicalWallObject obj, GraphicalWall owningWall) {
			return this.MarkErrors(obj, owningWall, true);
		}

#region Generic MarkErrors Methods
		private void MarkProductErrors<Product, Circuit, Register, Wrapper, Verbindung>(GraphicalWall wall, GraphicalWall owningWall, bool resetLinkErrors, Product product)
				where Circuit: Europlan.Common.Circuit, IWallCircuit<Circuit, Verbindung, Register>
				where Product: Europlan.Common.Product, IWallProduct<Circuit, Register>
				where Register : IWallRegister
				where Wrapper : Europlan.Common.GraphicalRegisterWrapper, IWallRegisterWrapper<Register>
				where Verbindung : Europlan.Common.GraphicalWallVerbindung {

			if (resetLinkErrors) {
				foreach (Circuit c in product.PlannedCircuits) {
					foreach (Verbindung link in c.Links) {
						link.Error = false;
					}
				}
			}
			Vector2D offset = this.GetWallOffset(owningWall).Value * 100;
			foreach (Wrapper register in owningWall.Registers) {
				WW.Math.Geometry.Polygon2D registerBorders = register.GetObjectBorders(offset.X, offset.Y);
				register.Error = owningWall.CollisionTest(registerBorders, offset.X, offset.Y, false);
				if (register.Error) {
					Circuit c = product.GetCircuitForRegister(register.Register);
					Verbindung link = c.GetInputLink(register.Register);
					if (link != null) {
						link.Error = true;
					}
					link = c.GetOutputLink(register.Register);
					if (link != null) {
						link.Error = true;
					}
				}
			}
			foreach (Circuit c in product.PlannedCircuits) {
				foreach (Verbindung link in c.Links) {
					link.Error = link.Error || !link.CheckValidity(owningWall, 0, 0);
				}
			}
		}

		private void MarkProductErrors<Product, Circuit, Register, Wrapper, Verbindung>(GraphicalWallObstacle obstacle, GraphicalWall owningWall, bool resetLinkErrors, Product product)
				where Circuit : Europlan.Common.Circuit, IWallCircuit<Circuit, Verbindung, Register>
				where Product : Europlan.Common.Product, IWallProduct<Circuit, Register>
				where Register : IWallRegister
				where Wrapper : Europlan.Common.GraphicalRegisterWrapper, IWallRegisterWrapper<Register>
				where Verbindung : Europlan.Common.GraphicalWallVerbindung {

			if (resetLinkErrors) {
				foreach (Circuit c in product.PlannedCircuits) {
					foreach (Verbindung link in c.Links) {
						link.Error = false;
					}
				}
			}
			Vector2D offset = this.GetWallOffset(owningWall).Value * 100;
			WW.Math.Geometry.Polygon2D objBorder = obstacle.GetObjectBorders(offset.X, offset.Y);
			WW.Math.Geometry.Polygon2D unsableBorder = obstacle.GetOutsideBorder(offset.X, offset.Y);
			foreach (Wrapper register in owningWall.Registers) {
				register.Error = register.CollisionTest(unsableBorder, offset.X, offset.Y, false);
				if (register.Error) {
					Circuit c = product.GetCircuitForRegister(register.Register);
					Verbindung link = c.GetInputLink(register.Register);
					if (link != null) {
						link.Error = true;
					}
					link = c.GetOutputLink(register.Register);
					if (link != null) {
						link.Error = true;
					}
				}
			}
			foreach (Circuit c in product.PlannedCircuits) {
				foreach (Verbindung link in c.Links) {
					link.Error = link.Error || link.CollisionTest(objBorder, 0, 0, true);
				}
			}
		}


		private void MarkProductErrors<Product, Circuit, Register, Wrapper, Verbindung>(GraphicalRegisterWrapper register, GraphicalWall owningWall, bool resetLinkErrors, Product product)
				where Circuit : Europlan.Common.Circuit, IWallCircuit<Circuit, Verbindung, Register>
				where Product : Europlan.Common.Product, IWallProduct<Circuit, Register>
				where Register : IWallRegister
				where Wrapper : Europlan.Common.GraphicalRegisterWrapper, IWallRegisterWrapper<Register>
				where Verbindung : Europlan.Common.GraphicalWallVerbindung {

			Vector2D offset = this.GetWallOffset(owningWall).Value * 100.0;
			WW.Math.Geometry.Polygon2D poly = register.GetObjectBorders(offset.X, offset.Y);
			foreach (Circuit c in product.PlannedCircuits) {
				foreach (Verbindung link in c.Links) {
					link.Error = link.CollisionTest(poly, 0, 0, false);
				}
			}
		}

		private void MarkProductErrors<Product, Circuit, Register, Wrapper, Verbindung>(GraphicalWallSchraege schraege, GraphicalWall owningWall, bool resetLinkErrors, Product product)
				where Circuit : Europlan.Common.Circuit, IWallCircuit<Circuit, Verbindung, Register>
				where Product : Europlan.Common.Product, IWallProduct<Circuit, Register>
				where Register : IWallRegister
				where Wrapper : Europlan.Common.GraphicalRegisterWrapper, IWallRegisterWrapper<Register>
				where Verbindung : Europlan.Common.GraphicalWallVerbindung {

			if (resetLinkErrors) {
				foreach (Circuit c in product.PlannedCircuits) {
					foreach (Verbindung link in c.Links) {
						link.Error = false;
					}
				}
			}
			Vector2D offset = this.GetWallOffset(owningWall).Value * 100;
			foreach (Wrapper register in owningWall.Registers) {
				WW.Math.Geometry.Polygon2D registerBorders = register.GetObjectBorders(offset.X, offset.Y);
				register.Error = owningWall.CollisionTest(registerBorders, offset.X, offset.Y, false);
				if (register.Error) {
					Circuit c = product.GetCircuitForRegister(register.Register);
					Verbindung link = c.GetInputLink(register.Register);
					if (link != null) {
						link.Error = true;
					}
					link = c.GetOutputLink(register.Register);
					if (link != null) {
						link.Error = true;
					}
				}
			}
			foreach (Circuit c in product.PlannedCircuits) {
				foreach (Verbindung link in c.Links) {
					link.Error = link.Error || !link.CheckValidity(owningWall, 0, 0);
				}
			}
		}
#endregion Generic MarkErrors Methods

		private bool MarkErrors(IGraphicalWallObject obj, GraphicalWall owningWall, bool resetLinkErrors) {
			if (obj is GraphicalWall) {
				foreach (PlannedProduct pp in this.PlannedProducts) {
					if (pp.Product is HithermProduct) {
						this.MarkProductErrors<HithermProduct, HithermCircuit, HithermRegister, GraphicalHithermRegisterWrapper, GraphicalHithermVerbindung>(obj as GraphicalWall, owningWall, resetLinkErrors, pp.Product as HithermProduct);
					} else if (pp.Product is HithermCompactProduct) {
						this.MarkProductErrors<HithermCompactProduct, HithermCompactCircuit, HithermCompactRegister, GraphicalHithermCompactRegisterWrapper, GraphicalHithermCompactVerbindung>(obj as GraphicalWall, owningWall, resetLinkErrors, pp.Product as HithermCompactProduct);
					}
				}
				foreach (GraphicalWallObstacle obstacle in owningWall.Obstacles) {
					Vector2D offset = this.GetWallOffset(owningWall).Value * 100;
					WW.Math.Geometry.Polygon2D obstacleBorders = obstacle.GetObjectBorders(offset.X, offset.Y);
					obstacle.Error = owningWall.CollisionTest(obstacleBorders, offset.X, offset.Y, true);
				}
				if (owningWall.DachSchraege != null) {
					this.MarkErrors(obj, owningWall.DachSchraege, false);
				}
			} else if (obj is GraphicalWallObstacle) {
				foreach (PlannedProduct pp in this.PlannedProducts) {
					if (pp.Product is HithermProduct) {
						this.MarkProductErrors<HithermProduct, HithermCircuit, HithermRegister, GraphicalHithermRegisterWrapper, GraphicalHithermVerbindung>(obj as GraphicalWallObstacle, owningWall, resetLinkErrors, pp.Product as HithermProduct);
					} else if (pp.Product is HithermCompactProduct) {
						this.MarkProductErrors<HithermCompactProduct, HithermCompactCircuit, HithermCompactRegister, GraphicalHithermCompactRegisterWrapper, GraphicalHithermCompactVerbindung>(obj as GraphicalWallObstacle, owningWall, resetLinkErrors, pp.Product as HithermCompactProduct);
					}
				}
			} else if (obj is GraphicalHithermVerbindung) {
				// nothing to do
			} else if (obj is GraphicalRegisterWrapper) {
				foreach (PlannedProduct pp in this.PlannedProducts) {
					if (pp.Product is HithermProduct) {
						this.MarkProductErrors<HithermProduct, HithermCircuit, HithermRegister, GraphicalHithermRegisterWrapper, GraphicalHithermVerbindung>(obj as GraphicalRegisterWrapper, owningWall, resetLinkErrors, pp.Product as HithermProduct);
					} else if (pp.Product is HithermCompactProduct) {
						this.MarkProductErrors<HithermCompactProduct, HithermCompactCircuit, HithermCompactRegister, GraphicalHithermCompactRegisterWrapper, GraphicalHithermCompactVerbindung>(obj as GraphicalRegisterWrapper, owningWall, resetLinkErrors, pp.Product as HithermCompactProduct);
					}
				}
			} else if (obj is GraphicalWallSchraege) {
				foreach (PlannedProduct pp in this.PlannedProducts) {
					if (pp.Product is HithermProduct) {
						this.MarkProductErrors<HithermProduct, HithermCircuit, HithermRegister, GraphicalHithermRegisterWrapper, GraphicalHithermVerbindung>(obj as GraphicalWallSchraege, owningWall, resetLinkErrors, pp.Product as HithermProduct);
					} else if (pp.Product is HithermCompactProduct) {
						this.MarkProductErrors<HithermCompactProduct, HithermCompactCircuit, HithermCompactRegister, GraphicalHithermCompactRegisterWrapper, GraphicalHithermCompactVerbindung>(obj as GraphicalWallSchraege, owningWall, resetLinkErrors, pp.Product as HithermCompactProduct);
					}
				}
				foreach (GraphicalWallObstacle obstacle in owningWall.Obstacles) {
					Vector2D offset = this.GetWallOffset(owningWall).Value * 100;
					WW.Math.Geometry.Polygon2D obstacleBorders = obstacle.GetObjectBorders(offset.X, offset.Y);
					obstacle.Error = owningWall.CollisionTest(obstacleBorders, offset.X, offset.Y, true);
				}
				if (owningWall.DachSchraege != null) {
					this.MarkErrors(obj, owningWall.DachSchraege);
				}
			}
			return true;
		}

		public bool ClearErrors() {
			foreach (PlannedProduct pp in this.PlannedProducts) {
				if (pp.Product is HithermProduct) {
					HithermProduct hp = pp.Product as HithermProduct;
					foreach (HithermCircuit hc in hp.PlannedCircuits) {
						foreach (GraphicalHithermVerbindung link in hc.Links) {
							link.Error = false;
						}
					}
				} else if (pp.Product is HithermCompactProduct) {
					HithermCompactProduct hp = pp.Product as HithermCompactProduct;
					foreach (HithermCompactCircuit hc in hp.PlannedCircuits) {
						foreach (GraphicalHithermCompactVerbindung link in hc.Links) {
							link.Error = false;
						}
					}
				}
			}
			foreach (GraphicalWall baseWall in this.Walls) {
				GraphicalWall wall = baseWall;
				while (wall != null) {
					wall.Error = false;
					foreach (GraphicalWallObstacle obstacle in wall.Obstacles) {
						obstacle.Error = false;
					}
					foreach (GraphicalRegisterWrapper register in wall.Registers) {
						register.Error = false;
					}
					wall = wall.DachSchraege;
				}
			}
			return true;
		}

		public bool DeleteErroneousObjects() {
			List<HithermRegister> hithermRegistersToDelete = new List<HithermRegister>();
			List<HithermCompactRegister> hithermCompactRegistersToDelete = new List<HithermCompactRegister>();
			foreach (GraphicalWall baseWall in this.Walls) {
				GraphicalWall wall = baseWall;
				while (wall != null) {
					wall.Error = false;
					List<GraphicalWallObstacle> obstaclesToDelete = new List<GraphicalWallObstacle>();
					foreach (GraphicalWallObstacle obstacle in wall.Obstacles) {
						if (obstacle.Error) {
							obstaclesToDelete.Add(obstacle);
						}
					}
					foreach (GraphicalWallObstacle obstacle in obstaclesToDelete) {
						wall.Obstacles.Remove(obstacle);
					}
					List<GraphicalRegisterWrapper> registersToDelete = new List<GraphicalRegisterWrapper>();
					foreach (GraphicalRegisterWrapper register in wall.Registers) {
						if (register.Error) {
							registersToDelete.Add(register);
							if (register is GraphicalHithermRegisterWrapper) {
								hithermRegistersToDelete.Add((register as GraphicalHithermRegisterWrapper).Register);
							} else if (register is GraphicalHithermCompactRegisterWrapper) {
								hithermCompactRegistersToDelete.Add((register as GraphicalHithermCompactRegisterWrapper).Register);
							}
						}
					}
					foreach (GraphicalRegisterWrapper register in registersToDelete) {
						wall.Registers.Remove(register);
					}
					wall = wall.DachSchraege;
				}
			}
			foreach (PlannedProduct pp in this.PlannedProducts) {
				if (pp.Product is HithermProduct) {
					HithermProduct hp = pp.Product as HithermProduct;
					foreach (HithermRegister register in hithermRegistersToDelete) {
						hp.RemoveRegisterFromCircuit(register);
					}
					Dictionary<HithermCircuit, List<GraphicalHithermVerbindung>> linksToDelete = new Dictionary<HithermCircuit,List<GraphicalHithermVerbindung>>();
					foreach (HithermCircuit hc in hp.PlannedCircuits) {
						List<GraphicalHithermVerbindung> linksToDeleteInCircuit = new List<GraphicalHithermVerbindung>();
						foreach (GraphicalHithermVerbindung link in hc.Links) {
							if (link.Error) {
								linksToDeleteInCircuit.Add(link);
							}
						}
						foreach (GraphicalHithermVerbindung link in linksToDeleteInCircuit) {
							hc.Links.Remove(link);
						}
						if (linksToDeleteInCircuit.Count > 0) {
							linksToDelete.Add(hc, linksToDeleteInCircuit);
						}
					}
					foreach (List<GraphicalHithermVerbindung> links in linksToDelete.Values) {
						foreach (GraphicalHithermVerbindung link in links) {
							if (link.Start != null && link.End != null) {
								hp.MoveRegisterToCircuit(link.End, hp.GetNewHkId());
								hp.CorrectCircuitIds();
							}
						}
					}
				} else if (pp.Product is HithermCompactProduct) {
					HithermCompactProduct hp = pp.Product as HithermCompactProduct;
					foreach (HithermCompactRegister register in hithermCompactRegistersToDelete) {
						hp.RemoveRegisterFromCircuit(register);
					}
					Dictionary<HithermCompactCircuit, List<GraphicalHithermCompactVerbindung>> linksToDelete = new Dictionary<HithermCompactCircuit, List<GraphicalHithermCompactVerbindung>>();
					foreach (HithermCompactCircuit hc in hp.PlannedCircuits) {
						List<GraphicalHithermCompactVerbindung> linksToDeleteInCircuit = new List<GraphicalHithermCompactVerbindung>();
						foreach (GraphicalHithermCompactVerbindung link in hc.Links) {
							if (link.Error) {
								linksToDeleteInCircuit.Add(link);
							}
						}
						foreach (GraphicalHithermCompactVerbindung link in linksToDeleteInCircuit) {
							hc.Links.Remove(link);
						}
						if (linksToDeleteInCircuit.Count > 0) {
							linksToDelete.Add(hc, linksToDeleteInCircuit);
						}
					}
					foreach (List<GraphicalHithermCompactVerbindung> links in linksToDelete.Values) {
						foreach (GraphicalHithermCompactVerbindung link in links) {
							if (link.Start != null && link.End != null) {
								hp.MoveRegisterToCircuit(link.End, hp.GetNewHkId());
								hp.CorrectCircuitIds();
							}
						}
					}
				}
			}
			return true;
		}
	}
}