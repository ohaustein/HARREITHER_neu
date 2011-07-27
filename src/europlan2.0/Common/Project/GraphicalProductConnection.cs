using System;
using System.Collections.Generic;
using System.Text;
using WW.Math;
using System.Drawing;
using System.Xml.Serialization;
using WW.Math.Geometry;
using System.Windows.Forms;

namespace Europlan.Common {
	public class GraphicalProductConnection {


		private PlannedProduct product;
		private string productGuid = null;
		private Distributor distributor;
		private string distributorId;
		private List<Point2D> vertices;
		private bool firstCircuit;
		private bool otherCircuits;
		//private List<Circuit> productCircuits;
		//private List<int> productCircuitIndices;
		private int distributorStartIndex;
		//private int distributorIndicesCount;
		private bool vorlauf = true;
		private bool ruecklauf = true;
		private Product.ProductType connectionType;
		private bool finishedConnection = true;
		private bool automatic = false;

		private List<List<Point2D>> vorlaufVerticesForDrawing = null;
		private List<List<Point2D>> ruecklaufVerticesForDrawing = null;

		public List<Point2D> Vertices {
		  get { return vertices; }
		  set { vertices = value; }
		}

		[XmlIgnore]
		public bool FinishedConnection {
			get { return this.finishedConnection; }
			set { this.finishedConnection = value; }
		}

		internal GraphicalProductConnection() {
			connectionType = Europlan.Common.Product.ProductType.REST;
		}

		/*public GraphicalProductConnection(PlannedProduct product, Distributor distributor, IEnumerable<Point2D> vertices, Circuit productCircuit, int distributorIndex, bool vorlauf, Product.ProductType connectionType) {
			this.product = product;
			this.distributor = distributor;
			this.vertices = new List<Point2D>(vertices);
			Point2D oldVertex = new Point2D();
			Vector2D oldVector = new Vector2D();
			Vector2D newVector = new Vector2D();
			bool first = true;
			bool second = true;
			List<Point2D> verticesToRemove = new List<Point2D>();
			foreach (Point2D newVertex in this.vertices) {
				if (first) {
					first = false;
				} else {
					newVector = (oldVertex - newVertex);
					newVector.Normalize();
					if (newVector.X < 0) {
						newVector.X = -newVector.X;
						newVector.Y = -newVector.Y;
					}
					if (second) {
						second = false;
					} else {
						if (Math.Abs(newVector.X - oldVector.X) < 0.001 && Math.Abs(newVector.Y - oldVector.Y) < 0.001) {
							verticesToRemove.Add(oldVertex);
						}
					}
				}
				oldVertex = newVertex;
				oldVector = newVector;
			}
			foreach (Point2D vertex in verticesToRemove) {
				this.vertices.Remove(vertex);
			}
			this.productCircuits = new List<Circuit>();
			this.productCircuits.Add(productCircuit);
			this.distributorStartIndex = distributorIndex;
			this.distributorIndicesCount = 1;
			//this.distributorIndices = new List<int>();
			//this.distributorIndices.Add(distributorIndex);
			this.vorlauf = vorlauf;
			this.connectionType = connectionType;
		}*/

		/*public GraphicalProductConnection(PlannedProduct product, Distributor distributor, IEnumerable<Point2D> vertices, List<Circuit> productCircuits, int distributorStartIndex, int distributorIndicesCount, bool vorlauf, Product.ProductType connectionType) {
			this.product = product;
			this.distributor = distributor;
			this.vertices = new List<Point2D>(vertices);
			Point2D oldVertex = new Point2D();
			Vector2D oldVector = new Vector2D();
			Vector2D newVector = new Vector2D();
			bool first = true;
			bool second = true;
			List<Point2D> verticesToRemove = new List<Point2D>();
			foreach (Point2D newVertex in this.vertices) {
				if (first) {
					first = false;
				} else {
					newVector = (oldVertex - newVertex);
					newVector.Normalize();
					if (newVector.X < 0) {
						newVector.X = -newVector.X;
						newVector.Y = -newVector.Y;
					}
					if (second) {
						second = false;
					} else {
						if (Math.Abs(newVector.X - oldVector.X) < 0.001 && Math.Abs(newVector.Y - oldVector.Y) < 0.001) {
							verticesToRemove.Add(oldVertex);
						}
					}
				}
				oldVertex = newVertex;
				oldVector = newVector;
			}
			foreach (Point2D vertex in verticesToRemove) {
				this.vertices.Remove(vertex);
			}
			this.productCircuits = productCircuits;
			this.distributorStartIndex = distributorStartIndex;
			this.distributorIndicesCount = distributorIndicesCount;
			this.vorlauf = vorlauf;
			this.connectionType = connectionType;
		}*/


		public GraphicalProductConnection(PlannedProduct product, Distributor distributor, IEnumerable<Point2D> vertices, bool firstCircuit, bool otherCircuits, int distributorStartIndex, bool vorlauf, bool ruecklauf, Product.ProductType connectionType) {
			this.product = product;
			this.distributor = distributor;
			if (vertices != null) {
				this.vertices = new List<Point2D>(vertices);
			} else {
				this.vertices = new List<Point2D>();
			}
			Point2D oldVertex = new Point2D();
			Vector2D oldVector = new Vector2D();
			Vector2D newVector = new Vector2D();
			bool first = true;
			bool second = true;
			List<Point2D> verticesToRemove = new List<Point2D>();
			foreach (Point2D newVertex in this.vertices) {
				if (first) {
					first = false;
				} else {
					newVector = (oldVertex - newVertex);
					newVector.Normalize();
					if (newVector.X < 0 || (newVector.X == 0 && newVector.Y < 0)) {
						newVector.X = -newVector.X;
						newVector.Y = -newVector.Y;
					}
					if (second) {
						second = false;
					} else {
						if (Math.Abs(newVector.X - oldVector.X) < 0.001 && Math.Abs(newVector.Y - oldVector.Y) < 0.001) {
							verticesToRemove.Add(oldVertex);
						}
					}
				}
				oldVertex = newVertex;
				oldVector = newVector;
			}
			foreach (Point2D vertex in verticesToRemove) {
				this.vertices.Remove(vertex);
			}
			this.firstCircuit = firstCircuit;
			this.otherCircuits = otherCircuits;
			//this.productCircuits = productCircuits;
			this.distributorStartIndex = distributorStartIndex;
			//this.distributorIndicesCount = distributorIndicesCount;
			this.vorlauf = vorlauf;
			this.ruecklauf = ruecklauf;
			this.connectionType = connectionType;
		}

		public double ConnectionWidth {
			get { return this.NrOfCircuits * 0.05 * 2 - 0.029; }
		}

		public double ConnectionDistributorWidth {
			get { return this.NrOfCircuits * 0.055 - 0.005; }
		}

		private void CalculateVerticesForDrawing(double measure) {
			this.vorlaufVerticesForDrawing = new List<List<Point2D>>();
			this.ruecklaufVerticesForDrawing = new List<List<Point2D>>();
			double factor = CalculateFactor();
			for (int i = 0; i < this.NrOfCircuits; i++) {
				List<Point2D> vl = new List<Point2D>(), rl = new List<Point2D>();
				double distVl = 0.05 * ((2 * this.NrOfCircuits - 1) / 2.0 - i * 2);
				double distVlFirst = finishedConnection ? 0.055 / 2 * ((2 * this.NrOfCircuits - 1) / 2.0 - i * 2) : distVl;
				double distRl = 0.05 * ((2 * this.NrOfCircuits - 1) / 2.0 - i * 2 - 1);
				double distRlFirst = finishedConnection ? 0.055 / 2 * ((2 * this.NrOfCircuits - 1) / 2.0 - i * 2 - 1) : distRl;
				Line2D curLineVl, curLineRl;
				Nullable<Line2D> lastLineVl = null, lastLineRl = null;
				Point2D startPoint, endPoint;
				Vector2D curVector;
				Vector2D moveVector = new Vector2D();
				for (int j = 0; j < this.vertices.Count - 1; j++) {
					startPoint = this.vertices[j];
					endPoint = this.vertices[j + 1];
					curVector = endPoint - startPoint;
					curVector.Normalize();
					moveVector = new Vector2D(curVector.Y, -curVector.X);
					curLineVl = new Line2D(startPoint + moveVector * ((j < this.vertices.Count - 2 ? distVl : distVlFirst) * measure * factor), curVector);
					curLineRl = new Line2D(startPoint + moveVector * ((j < this.vertices.Count - 2 ? distRl : distRlFirst) * measure * factor), curVector);
					if (lastLineVl == null) {
						vl.Add(curLineVl.Origin);
					} else {
						Nullable<Point2D> curPoint = Line2D.GetIntersection(curLineVl, lastLineVl.Value);
						if (curPoint.HasValue) {
							vl.Add(curPoint.Value);
						}
					}
					if (lastLineRl == null) {
						rl.Add(curLineRl.Origin);
					} else {
						Nullable<Point2D> curPoint = Line2D.GetIntersection(curLineRl, lastLineRl.Value);
						if (curPoint.HasValue) {
							rl.Add(curPoint.Value);
						}
					}
					lastLineVl = curLineVl;
					lastLineRl = curLineRl;
				}
				vl.Add(this.vertices[this.vertices.Count - 1] + moveVector * (distVlFirst * measure * factor));
				rl.Add(this.vertices[this.vertices.Count - 1] + moveVector * (distRlFirst * measure * factor));
				this.vorlaufVerticesForDrawing.Add(vl);
				this.ruecklaufVerticesForDrawing.Add(rl);
			}
		}

		private double CalculateFactor() {
			return 1;
			// TODO
			double factor = 1;
			if (this.Distributor != null && this.vertices != null && this.vertices.Count > 1 && this.Product != null) {
				Nullable<Distributor.GraphicalRepresentation> distRep = null;
				foreach (Distributor.GraphicalRepresentation gr in this.Distributor.GraphicalRepresentations) {
					if (gr.floorId == this.Product.Product.AssociatedRoom.AssociatedFloor.Id) {
						distRep = gr;
						break;
					}
				}
				if (distRep != null) {
					Vector2D startVector = this.vertices[this.vertices.Count - 2] - this.vertices[this.vertices.Count - 1];
					startVector.Normalize();
					double angle = Math.Atan2(startVector.Y, startVector.X) * 180 / Math.PI;
					angle = angle - distRep.Value.rotation;
					if (angle >= 0 && angle < 180) {
						factor = 1;
					} else {
						factor = -1;
					}
				}
			}
			return factor;
		}

		public void ResetCachedVerticesForDrawing() {
			this.vorlaufVerticesForDrawing = null;
			this.ruecklaufVerticesForDrawing = null;
		}

		public void Draw(Graphics g, Matrix4D additionalTransformation, double measure, bool selected, bool gray) {
			if (this.vertices.Count < 2) {
				return;
			}

			if (this.vorlaufVerticesForDrawing == null || this.ruecklaufVerticesForDrawing == null) {
				this.CalculateVerticesForDrawing(measure);
			}
			Color c = selected ? Color.Green : (gray ? Color.FromArgb(100, 0, 0) : Color.Red);
			foreach (List<Point2D> singleConnection in this.vorlaufVerticesForDrawing) {
				this.DrawSingleConnection(g, additionalTransformation, singleConnection, c, measure);
			}
			c = selected ? Color.Green : (gray ? Color.FromArgb(0, 0, 100) : Color.Blue);
			foreach (List<Point2D> singleConnection in this.ruecklaufVerticesForDrawing) {
				this.DrawSingleConnection(g, additionalTransformation, singleConnection, c, measure);
			}
		}

		private void DrawSingleConnection(Graphics g, Matrix4D additionalTransformation, List<Point2D> singleConnection, Color c, double measure) {
			if (singleConnection.Count < 2) {
				return;
			}
			Point2D oldVertex2D = additionalTransformation.TransformTo2D(singleConnection[0]);
			Point2D newVertex2D;
			PointF oldVertex = new PointF((float)oldVertex2D.X, (float)oldVertex2D.Y);
			PointF newVertex;
			Pen p = new Pen(c, (float)(0.021 * measure * additionalTransformation.M00));
			p.EndCap = System.Drawing.Drawing2D.LineCap.Round;
			for (int i = 1; i < singleConnection.Count; i++) {
				newVertex2D = additionalTransformation.TransformTo2D(singleConnection[i]);
				newVertex = new PointF((float)newVertex2D.X, (float)newVertex2D.Y);
				if (i == 2) {
					p.StartCap = System.Drawing.Drawing2D.LineCap.Round;
				}
				if (i == this.vertices.Count - 1) {
					p.EndCap = System.Drawing.Drawing2D.LineCap.Flat;
				}
				try {
					g.DrawLine(p, oldVertex, newVertex);
				} catch (Exception e) {
					Console.WriteLine(e);
				}
				oldVertex = newVertex;
			}
		}

		public bool HitTest(Point2D planPoint, double measure) {
			return this.GetDistance(planPoint) <= measure * (this.NrOfCircuits * 0.05 - 0.01);
		}

		public double GetDistance(Point2D planPoint) {
			Point2D oldVertex = new Point2D();
			bool first = false;
			double bestDist = double.MaxValue;
			foreach (Point2D newVertex in this.vertices) {
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
		public PlannedProduct Product {
			get {
				if (this.productGuid != null) {
					this.product = null;
					foreach (Floor f in Project.Instance.Floors) {
						foreach (Room r in f.Rooms) {
							foreach (PlannedProduct pp in r.PlannedProducts) {
								if (pp.Id == this.productGuid) {
									this.product = pp;
									this.productGuid = null;
									break;
								}
							}
							if (this.productGuid == null) {
								break;
							}
						}
						if (this.productGuid == null) {
							break;
						}
					}
					this.productGuid = null;
				}
				return this.product;
			}
		}

		public string ProductGuid {
			set { this.productGuid = value; }
			get { return (this.productGuid != null || this.product == null) ? this.productGuid : this.product.Id; }
		}

		[XmlIgnore]
		public Distributor Distributor {
			get {
				if (this.distributorId != null) {
					foreach (Floor f in Project.Instance.Floors) {
						foreach (Distributor d in f.Distributors) {
							if (d.Id == this.distributorId) {
								this.distributor = d;
							}
						}
					}
					this.distributorId = null;
				}
				return this.distributor;
			}
			set {
				this.distributor = value;
				this.distributorId = null;
			}
		}

		public string DistributorId {
			get { return (this.distributorId != null && this.distributor == null) ? this.distributorId : this.distributor.Id; }
			set { this.distributorId = value; }
		}

		public int DistributorStartIndex {
			get { return this.distributorStartIndex; }
			set { this.distributorStartIndex = value; }
		}

		public bool Automatic {
			get { return this.automatic; }
			set { this.automatic = true; }
		}

		/*public int DistributorIndicesCount {
			get { return this.distributorIndicesCount; }
			set { this.distributorIndicesCount = value; }
		}

		[XmlIgnore]
		public List<Circuit> ProductCircuits {
			get {
				if (this.productCircuitIndices != null) {
					this.productCircuits = new List<Circuit>();
					foreach (int i in this.productCircuitIndices) {
						this.productCircuits.Add(this.Product.Product.PlannedCircuits[i]);
					}
					this.productCircuitIndices = null;
				}
				if (this.productCircuits == null) {
					this.productCircuits = new List<Circuit>();
				}
				return this.productCircuits;
			}
		}

		public List<int> ProductCircuitIndices {
			get {
				if (this.productCircuitIndices != null) {
					return this.productCircuitIndices;
				}
				List<int> indices = new List<int>();
				foreach (Circuit circuit in this.ProductCircuits) {
					int i = 0;
					if (this.Product != null && this.Product.Product != null && this.Product.Product.PlannedCircuits != null) {
						foreach (Circuit c in this.Product.Product.PlannedCircuits) {
							if (c == circuit) {
								indices.Add(i);
								break;
							}
							i++;
						}
					}
				}
				return indices;
			}
			set { this.productCircuitIndices = value; }
		}*/

		public bool Vorlauf {
			get { return this.vorlauf; }
			set { this.vorlauf = value; }
		}

		public bool Ruecklauf {
			get { return this.ruecklauf; }
			set { this.ruecklauf = value; }
		}

		public bool FirstCircuit {
			get { return this.firstCircuit; }
			set { this.firstCircuit = value; }
		}

		public bool OtherCircuits {
			get { return this.otherCircuits; }
			set { this.otherCircuits = value; }
		}

		public int NrOfCircuits {
			get {
				int nrOfCircuits = 0;
				if (this.firstCircuit) {
					nrOfCircuits++;
				}
				if (this.otherCircuits) {
					nrOfCircuits += this.Product.Product.PlannedCircuits.Count - 1;
				}
				return nrOfCircuits;
			}
		}

		public Product.ProductType ConnectionType {
			get { return this.connectionType; }
			set { this.connectionType = value; }
		}

		//     Input: a 2D segment S from point P0 to point P1
        //     a 2D convex polygon W with n vertices V0,...,Vn-1,Vn=V0
		private List<Segment2D> GetSegmentsInPolygon(Segment2D s, Polygon2D w) {
			List<Segment2D> result = new List<Segment2D>();
			if (w == null || w.Count < 3) {
				return result;
			}
			if (s.Start == s.End) { // S is a single point
				if (Polygon2D.IsInside(s.Start, w)) {
					result.Add(s);
				}
				return result;
 			}

			Segment2D polySegment;
			double[] pArray;
			double[] qArray;
			List<double> intersections = new List<double>();

			Nullable<Point2D> lastPoint = w[w.Count - 1];
			foreach (Point2D curPoint in w) {
				if (lastPoint.HasValue) {
					polySegment = new Segment2D(lastPoint.Value, curPoint);
					if (Segment2D.GetIntersectionParameters(s, polySegment, out pArray, out qArray)) {
						intersections.Add(pArray[0]);
					}
				}
				lastPoint = curPoint;
			}

			if (Polygon2D.IsInside(s.Start, w)) {
				intersections.Add(0);
			}
			if (Polygon2D.IsInside(s.End, w)) {
				intersections.Add(1);
			}

			intersections.Sort();

			Vector2D delta = s.GetDelta();
			for (int i = 1; i < intersections.Count; i += 2) {
				result.Add(new Segment2D(s.Start + (delta * intersections[i - 1]), s.Start + (delta * intersections[i])));
			}
			return result;
		}

		public double GetPartInsidePolygon(Polygon2D polygon) {
			if (this.vertices == null || this.vertices.Count < 2 || polygon == null || polygon.Count < 3) {
				return 0;
			}

			List<Segment2D> segments;
			Nullable<Point2D> lastPoint = null;
			double length = 0;
			foreach (Point2D curPoint in this.vertices) {
				if (lastPoint.HasValue) {
					segments = this.GetSegmentsInPolygon(new Segment2D(lastPoint.Value, curPoint), polygon);
					foreach (Segment2D seg in segments) {
						length += seg.GetLength();
					}
				}
				lastPoint = curPoint;
			}
			return length;
		}

		public double GetLength(double measure) {
			if (measure == 0) {
				return 0;
			}
			double length = 0;
			if (this.vertices.Count > 1) {
				for (int i = 1; i < this.vertices.Count; i++) {
					length += (this.vertices[i - 1] - this.vertices[i]).GetLength();
				}
			}
			length = length / measure;
			// TODO remove part that is inside product
			if (length < 0) {
				length = 0;
			}
			return length;
		}

		public List<GraphicalConnectionAnchor> GetAnchors(double scale) {
			List<GraphicalConnectionAnchor> anchors = new List<GraphicalConnectionAnchor>();
			if (this.vertices == null || this.vertices.Count < 2) {
				return anchors;
			}
			Nullable<Point2D> prev = null;
			//bool lastHorizontal = this.vertices[0].Y != 0;
			int i = 0;
			foreach (Point2D vertex in this.vertices) {
				if (prev.HasValue) {
					if (i > 0 && i < this.vertices.Count - 2) {
						anchors.Add(new GraphicalConnectionAnchor(new Segment2D(prev.Value, vertex), this, i));
					}
					i++;
				}
				prev = vertex;
			}
			return anchors;
		}

		private Point2D startDrag;
		private List<Point2D> startVertices;

		public bool StartDrag(GraphicalConnectionAnchor anchor, Point2D planPoint) {
			this.startDrag = planPoint;
			this.startVertices = new List<Point2D>(this.vertices);
			return false;
		}

		public bool MoveDrag(GraphicalConnectionAnchor anchor, Point2D planPoint) {
			this.MoveSegment(anchor.SegmentId, planPoint - startDrag);
			return true;
		}

		public bool EndDrag(GraphicalConnectionAnchor anchor, Point2D planPoint) {
			this.Simplify();
			this.startDrag = new Point2D();
			this.startVertices = null;
			return true;
		}

		private void MoveSegment(int segmentId, Vector2D vector) {
			this.ResetCachedVerticesForDrawing();
			Line2D prevLine = new Line2D(this.startVertices[segmentId - 1], this.startVertices[segmentId - 1] - this.startVertices[segmentId]);
			Line2D nextLine = new Line2D(this.startVertices[segmentId + 1], this.startVertices[segmentId + 1] - this.startVertices[segmentId + 2]);

			Vector2D movedLineDirection = this.startVertices[segmentId] - this.startVertices[segmentId + 1];
			Vector2D realMoveDirection = new Vector2D(movedLineDirection.Y, -movedLineDirection.X);
			realMoveDirection.Normalize();

			Vector2D realMove = Vector2D.DotProduct(realMoveDirection, vector) * realMoveDirection;

			Line2D movedLine = new Line2D(this.startVertices[segmentId] + realMove, this.startVertices[segmentId] - this.startVertices[segmentId + 1]);

			Nullable<Point2D> newSegmentStart = Line2D.GetIntersection(prevLine, movedLine);
			Nullable<Point2D> newSegmentEnd = Line2D.GetIntersection(nextLine, movedLine);

			if (newSegmentStart.HasValue && newSegmentEnd.HasValue) {
				this.vertices[segmentId] = newSegmentStart.Value;
				this.vertices[segmentId + 1] = newSegmentEnd.Value;
			}
		}

		private void Simplify() {
			// TODO
			//throw new Exception("The method or operation is not implemented.");
		}

		public List<GraphicalConnectionAnbindungsPunkt> GetAnbindungsPunkte(double measure, bool input, int distributorIndex, List<int> ignoreDistributorIndices, bool newProductConnection) {
			if (this.Vertices == null || this.Vertices.Count < 2) {
				return new List<GraphicalConnectionAnbindungsPunkt>();
			}
			if (this.CalculateFactor() < 0) {
				input = !input;
			}
			List<GraphicalConnectionAnbindungsPunkt> anbindungsPunkte = new List<GraphicalConnectionAnbindungsPunkt>();
			if ((input && this.vorlauf) || (!input && this.ruecklauf)) {
				Vector2D startVector = this.Vertices[1] - this.Vertices[0];
				startVector.Normalize();
				Vector2D v = new Vector2D(-startVector.Y, startVector.X);
				Point2D po1, po2, po3, po4, pi1, pi2, pi3, pi4, po, pi;
				Point2D connectionPoint;
				if (this.automatic) {
					connectionPoint = this.Vertices[0] + ((this.Vertices[1] - this.Vertices[0]) / 2.0);
				} else {
					connectionPoint = this.Vertices[0];
				}
				double connectionWidth = (this.Vertices.Count > 2 ? 0.05 : 0.055 / 2.0) * measure;
				double connectionDepth = 0.1 * measure;
				for (int i = 0; i < this.NrOfCircuits; i++) {
					pi1 = connectionPoint - v * ((this.NrOfCircuits) / 2.0 - i /*+ 0.5*/) * connectionWidth * 2;
					if (this.automatic) {
						pi1 -= (startVector * connectionDepth / 2.0);
					}
					pi2 = pi1 + v * connectionWidth;
					pi3 = pi2 + startVector * connectionDepth;
					pi4 = pi1 + startVector * connectionDepth;
					po1 = pi2;
					po2 = po1 + v * connectionWidth;
					po3 = po2 + startVector * connectionDepth;
					po4 = pi3;
					po = po1 + (((automatic ? po3 : po2) - po1) / 2);
					pi = pi1 + (((automatic ? pi3 : pi2) - pi1) / 2);
					if ((distributorIndex < 0 || this.distributorStartIndex + i == distributorIndex) &&
						(ignoreDistributorIndices == null || !ignoreDistributorIndices.Contains(this.distributorStartIndex + i))) {
						if (input) {
							anbindungsPunkte.Add(new GraphicalConnectionAnbindungsPunkt(pi, new Polygon2D(new Point2D[] { pi1, pi2, pi3, pi4 }), this.distributorStartIndex + i, newProductConnection ? this : null));
						} else {
							anbindungsPunkte.Add(new GraphicalConnectionAnbindungsPunkt(po, new Polygon2D(new Point2D[] { po1, po2, po3, po4 }), this.distributorStartIndex + i, newProductConnection ? this : null));
						}
					}
				}
			}
			return anbindungsPunkte;
		}
	}

	public class GraphicalConnectionAnbindungsPunkt {
		private Point2D point;
		private Polygon2D area;
		private int index;
		public GraphicalProductConnection newProductConnection = null;

		public GraphicalConnectionAnbindungsPunkt(Point2D point, Polygon2D area, int index) {
			this.point = point;
			this.area = area;
			this.index = index;
		}

		public GraphicalConnectionAnbindungsPunkt(Point2D point, Polygon2D area, int index, GraphicalProductConnection newProductConnection) {
			this.point = point;
			this.area = area;
			this.index = index;
			this.newProductConnection = newProductConnection;
		}

		public Point2D Point {
			get { return this.point; }
		}

		public Polygon2D Area {
			get { return this.area; }
		}

		public int Index {
			get { return this.index; }
		}

		public GraphicalProductConnection NewProductConnection {
			get { return this.newProductConnection; }
		}
	}

	public class GraphicalConnectionAnchor {
		//protected Point2D position = Point2D.Zero;
		protected Segment2D segment = new Segment2D();
		protected GraphicalProductConnection connection = null;
		protected int segmentId = 0;

		/*public GraphicalConnectionAnchor(double x, double y, GraphicalProductConnection connection, int segmentId) {
			this.position = new Point2D(x, y); ;
			this.connection = connection;
			this.segmentId = segmentId;
		}

		public GraphicalConnectionAnchor(Point2D position, GraphicalProductConnection connection, int segmentId) {
			this.position = position;
			this.connection = connection;
			this.segmentId = segmentId;
		}*/

		public GraphicalConnectionAnchor(Segment2D segment, GraphicalProductConnection connection, int segmentId) {
			this.segment = segment;
			this.connection = connection;
			this.segmentId = segmentId;
		}

		public Segment2D Segment {
			get { return this.segment; }
		}

		public GraphicalProductConnection Connection {
			get { return this.connection; }
		}

		public Cursor Cursor {
			get {
				return Cursors.SizeAll;
			}
		}

		public int SegmentId {
			get {
				return this.segmentId;
			}
		}

		public bool HitTest(Point2D planPoint, double scale) {
			return this.segment.GetDistance(planPoint) <= (this.connection.NrOfCircuits * 0.05 - 0.01) * scale;
		}
	}
}
