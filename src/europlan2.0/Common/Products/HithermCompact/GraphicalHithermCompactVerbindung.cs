using System;
using System.Collections.Generic;
using System.Text;
using WW.Math;
using System.Drawing;
using System.Xml.Serialization;
using WW.Math.Geometry;
using System.Drawing.Drawing2D;

namespace Europlan.Common {
	public class GraphicalHithermCompactVerbindung : GenericGraphicalWallVerbindungImplementation<HithermCompactProduct, HithermCompactCircuit, HithermCompactRegister,  GraphicalHithermCompactRegisterWrapper, GraphicalHithermCompactVerbindung> {

		internal GraphicalHithermCompactVerbindung()
			: base() {
		}

		internal GraphicalHithermCompactVerbindung(bool finished)
			: base(finished) {
		}

		public GraphicalHithermCompactVerbindung(HithermCompactRegister start, HithermCompactRegister end, IEnumerable<Point2D> vertices, HithermCompactCircuit circuit, PlannedProduct product)
			: base(start, end, vertices, circuit, product) {
		}
	}
	
	public class GraphicalHithermCompactUnderfloorVerbindung : GraphicalHithermCompactVerbindung, IWallVerbindungCompound<GraphicalHithermCompactVerbindung> {

		private GraphicalHithermCompactVerbindung startLink = null;
		private GraphicalHithermCompactVerbindung endLink = null;

		public GraphicalHithermCompactUnderfloorVerbindung(GraphicalHithermCompactVerbindung startLink, GraphicalHithermCompactVerbindung endLink) {
			this.startLink = startLink;
			this.endLink = endLink;
			startLink.IsPartOfCompound = true;
			endLink.IsPartOfCompound = true;
		}

		internal GraphicalHithermCompactUnderfloorVerbindung() {
		}

		public override void FinalizeLoading() {
			this.startLink.FinalizeLoading();
			this.endLink.FinalizeLoading();
		}

		public override List<Point2D> Vertices {
			get {
				List<Point2D> vertices = new List<Point2D>();
				vertices.AddRange(this.startLink.Vertices);
				vertices[vertices.Count - 1] = new Point2D(vertices[vertices.Count - 1].X, -10);
				vertices.AddRange(this.endLink.Vertices);
				vertices[this.startLink.Vertices.Count] = new Point2D(vertices[this.startLink.Vertices.Count].X, -10);
				return vertices;
			}
			set { }
		}

		/*public override void InitializeVertices(IEnumerable<Point2D> vertices) {
			// TODO
		}*/

		public override bool IsMoveable {
			get { return false; }
		}

		public override void PaintObject(Graphics g, Color c, bool error, double scale, bool export) {
			Pen p = new Pen(c, 2);
			if (error || this.error) {
				//p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
				p = new Pen(new HatchBrush(HatchStyle.DarkDownwardDiagonal, c, Color.Transparent));
			}
			p.EndCap = LineCap.Round;
			float x1 = (float)(this.startLink.Vertices[this.startLink.Vertices.Count - 1].X);
			float y1 = (float)(this.startLink.Vertices[this.startLink.Vertices.Count - 1].Y);
			float x2 = x1;
			float y2 = -10;
			g.DrawLine(p, x1, y1, x2, y2);
			p.StartCap = LineCap.Round;
			y1 = -10;
			x2 = (float)(this.endLink.Vertices[0].X);
			g.DrawLine(p, x1, y1, x2, y2);
			p.EndCap = LineCap.Flat;
			x1 = x2;
			y2 = (float)(this.endLink.Vertices[0].Y);
			g.DrawLine(p, x1, y1, x2, y2);
			this.startLink.PaintObject(g, Color.Black, error, scale, export);
			this.endLink.PaintObject(g, Color.Black, error, scale, export);
		}

		public override bool HitTest(Point2D planPoint, double maxDist) {
			return this.GetDistance(planPoint) <= maxDist;
		}

		public override double GetDistance(Point2D planPoint) {
			Point2D oldVertex = new Point2D();
			bool first = false;
			double bestDist = double.MaxValue;
			foreach (Point2D newVertex in this.Vertices) {
				if (first) {
					first = false;
				} else {
					Segment2D segment = new Segment2D(oldVertex, newVertex);
					double dist = segment.GetDistance(planPoint);
					if (dist <= bestDist) {
						bestDist = dist;
					}
				}
				oldVertex = newVertex;
			}
			return bestDist;
		}

		[XmlIgnore]
		public override PlannedProduct Product {
			get {
				return this.startLink.Product;
			}
		}

		public override string ProductGuid {
			set {
				this.startLink.ProductGuid = value;
				this.endLink.ProductGuid = value;
			}
			get {
				return this.startLink.ProductGuid;
			}
		}

		public override double GetLength() {
			double length = 0;
			if (this.Vertices.Count > 1) {
				for (int i = 1; i < this.Vertices.Count; i++) {
					length += (this.Vertices[i - 1] - this.Vertices[i]).GetLength();
				}
			}
			if (length < 0) {
				length = 0;
			}
			return length;
		}

		#region IGraphicalWallObject Members
		public override bool HitTest(Point2D planPoint, double xOffset, double yOffset) {
			return this.HitTest(planPoint, 2);
		}

		public override void PaintObject(Graphics g, double xOffset, double yOffset, IGraphicalWallObject selectedObject, double scale, bool export) {
			this.PaintObject(g, (this == selectedObject) ? Color.Red : Color.Black, false, scale, export);
		}

		public override IGraphicalWallObject GetPickedObject(Point2D planPoint, double xOffset, double yOffset) {
			IGraphicalWallObject obj = this.startLink.GetPickedObject(planPoint, xOffset, yOffset);
			if (obj != null) {
				return obj;
			}
			obj = this.endLink.GetPickedObject(planPoint, xOffset, yOffset);
			if (obj != null) {
				return obj;
			}
			if (HitTest(planPoint, xOffset, yOffset)) {
				return this;
			}
			return null;
		}

		public override List<Polygon2D> GetObjectBorders(double xOffset, double yOffset) {
			List<Polygon2D> border = new List<Polygon2D>();
			border.AddRange(this.startLink.GetObjectBorders(xOffset, yOffset));
			border.AddRange(this.endLink.GetObjectBorders(xOffset, yOffset));
			return border;
		}

		public override bool CollisionTest(IList<Polygon2D> polygon, double xOffset, double yOffset, bool ignoreBorders) {
			if (this.startLink.CollisionTest(polygon, xOffset, yOffset, ignoreBorders)) {
				return true;
			}
			return this.endLink.CollisionTest(polygon, xOffset, yOffset, ignoreBorders);
		}

		public override List<Anchor> GetAnchors(double scale) {
			List<Anchor> anchors = new List<Anchor>();
			return anchors;
		}

		public override bool CheckValidity(GraphicalWall owningWall, double offsetX, double offsetY) {
			if (!this.startLink.CheckValidity(owningWall, offsetX, offsetY)) {
				return false;
			}
			return this.startLink.CheckValidity(owningWall, offsetX, offsetY);
		}

		public override bool StartDrag(Anchor anchor, Point2D planPoint, GraphicalWall owningWall, Room owningRoom, Product owningProduct, bool useSnap) {
			return false;
		}

		public override bool MoveDrag(Anchor anchor, Point2D planPoint, GraphicalWall owningWall, Room owningRoom, Product owningProduct, bool useSnap) {
			return false;
		}

		public override bool EndDrag(Anchor anchor, Point2D planPoint, GraphicalWall owningWall, Room owningRoom, Product owningProduct, bool useSnap) {
			return false;
		}
		#endregion

		public override void UpdateStartPoint(GraphicalHithermCompactRegisterWrapper register, GraphicalWall owningWall, bool checkValidity) {
			this.startLink.UpdateStartPoint(register, owningWall, checkValidity);
		}

		public override void UpdateEndPoint(GraphicalHithermCompactRegisterWrapper register, GraphicalWall owningWall, bool checkValidity) {
			this.endLink.UpdateEndPoint(register, owningWall, checkValidity);
		}

		public override void MoveVerticalSegment(int segmentIndex, double delta) {
			if (segmentIndex < this.startLink.Vertices.Count - 1) {
				this.startLink.MoveVerticalSegment(segmentIndex, delta);
			} else {
				this.endLink.MoveVerticalSegment(segmentIndex - this.startLink.Vertices.Count, delta);
			}
			/*if (this.vertices.Count < segmentIndex + 2) {
				return;
			}
			Vector2D deltaVector = new Vector2D(delta, 0);
			this.vertices[segmentIndex] += deltaVector;
			this.vertices[segmentIndex + 1] += deltaVector;*/
		}

		public override void MoveHorizontalSegment(int segmentIndex, double delta) {
			if (segmentIndex < this.startLink.Vertices.Count - 1) {
				this.startLink.MoveHorizontalSegment(segmentIndex, delta);
			} else {
				this.endLink.MoveHorizontalSegment(segmentIndex - this.startLink.Vertices.Count, delta);
			}
			/*if (this.vertices.Count < segmentIndex + 2) {
				return;
			}
			Vector2D deltaVector = new Vector2D(0, delta);
			this.vertices[segmentIndex] += deltaVector;
			this.vertices[segmentIndex + 1] += deltaVector;*/
		}

		public override void Simplify() {
			this.startLink.Simplify();
			this.endLink.Simplify();
		}

		[XmlIgnore]
		public override bool Finished {
			get { return true; }
			set { }
		}

		[XmlIgnore]
		public override bool Error {
			get { return this.error; }
			set { this.error = value; }
		}

		public override void BackupState() {
			this.startLink.BackupState();
			this.endLink.BackupState();
		}

		public override void RevertState() {
			this.startLink.RevertState();
			this.endLink.RevertState();
		}

		private bool isNew = false;
		[XmlIgnore]
		public override bool IsNew {
			get { return this.isNew; }
			set { this.isNew = value; }
		}

		public override bool SnapToHelplines(List<double> helplines, bool snapTop, bool snapBottom) {
			// nothing to do
			return false;
		}

		[XmlIgnore]
		public override HithermCompactRegister Start {
			get { return this.startLink.Start; }
		}

		[XmlIgnore]
		public override HithermCompactRegister End {
			get { return this.endLink.End; }
		}

		public override int StartIndex {
			get { return this.startLink.StartIndex; }
			set { this.startLink.StartIndex = value; }
		}

		public override int EndIndex {
			get { return this.endLink.EndIndex; }
			set { this.endLink.EndIndex = value; }
		}

		[XmlIgnore]
		public override HithermCompactCircuit Circuit {
			get { return this.startLink.Circuit; }
			set {
				this.startLink.Circuit = value;
				this.endLink.Circuit = value;
			}
		}

		public override int CircuitIndex {
			get { return this.startLink.CircuitIndex; }
			set {
				this.startLink.CircuitIndex = value;
				this.endLink.CircuitIndex = value;
			}
		}

		[XmlIgnore]
		public override bool HasStart {
			get { return this.startLink.HasStart; }
		}

		[XmlIgnore]
		public override bool HasEnd {
			get { return this.endLink.HasEnd; }
		}

		[XmlIgnore]
		public override int HkId {
			get { return this.startLink.HkId; }
		}

		public GraphicalHithermCompactVerbindung StartLink {
			get { return this.startLink; }
			set { this.startLink = value; }
		}

		public GraphicalHithermCompactVerbindung EndLink {
			get { return this.endLink; }
			set { this.endLink = value; }
		}

		public override bool EqualsOrIsPart(object obj) {
			return base.EqualsOrIsPart(obj) || this.startLink.EqualsOrIsPart(obj) || this.endLink.EqualsOrIsPart(obj);
		}
	}
}
