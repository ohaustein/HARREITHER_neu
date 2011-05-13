using System;
using System.Collections.Generic;
using System.Text;
using WW.Math;
using System.Drawing;
using System.Xml.Serialization;
using WW.Math.Geometry;

namespace Europlan.Common {
	public class HithermRegisterVerbindung : IGraphicalWallObject {
		private static double WIDTH = 2.0;

		private HithermRegister start;
		private HithermRegister end;
		private List<Point2D> vertices;
		private int startIndex = -1;
		private int endIndex = -1;
		private Circuit circuit;
		private int circuitIndex = -1;
		private PlannedProduct product;
		private string productGuid = null;

		public List<Point2D> Vertices {
		  get { return vertices; }
		  set { vertices = value; }
		}

		internal HithermRegisterVerbindung() {
			this.vertices = new List<Point2D>();
		}

		public HithermRegisterVerbindung(HithermRegister start, HithermRegister end, IEnumerable<Point2D> vertices, Circuit circuit, PlannedProduct product) {
			this.start = start;
			this.end = end;
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
			this.circuit = circuit;
			this.product = product;
		}

		public bool IsMoveable {
			get { return false; }
		}

		private void Draw(Graphics g, Color c) {
			PointF oldVertex = PointF.Empty;
			PointF newVertex;
			bool first = true;
			Pen p = new Pen(c, 2);
			p.EndCap = System.Drawing.Drawing2D.LineCap.Round;
			foreach (Point2D vertex in vertices) {
				newVertex = new PointF((float)vertex.X, (float)vertex.Y);
				if (first) {
					first = false;
				} else {
					g.DrawLine(p, oldVertex, newVertex);
					p.EndCap = System.Drawing.Drawing2D.LineCap.Flat;
					p.StartCap = System.Drawing.Drawing2D.LineCap.Round;
				}
				oldVertex = newVertex;
			}
			// TODO
		}

		public bool HitTest(Point2D planPoint, double maxDist) {
			return this.GetDistance(planPoint) <= maxDist;
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
		public HithermRegister Start {
			get {
				if (this.startIndex >= 0) {
					if (this.Circuit is HithermCircuit) {
						HithermCircuit hc = this.Circuit as HithermCircuit;
						this.start = hc.Registers[this.startIndex];
						this.startIndex = -1;
					}
				}
				return this.start;
			}
		}

		[XmlIgnore]
		public HithermRegister End {
			get {
				if (this.endIndex >= 0) {
					if (this.Circuit is HithermCircuit) {
						HithermCircuit hc = this.Circuit as HithermCircuit;
						this.end = hc.Registers[this.endIndex];
						this.endIndex = -1;
					}
				}
				return this.end;
			}
		}

		public int StartIndex {
			get {
				if (this.startIndex >= 0) {
					return this.startIndex;
				}
				int index = -1;
				if (this.Circuit is HithermCircuit) {
					HithermCircuit hc = this.Circuit as HithermCircuit;
					int i = 0;
					foreach (HithermRegister r in hc.Registers) {
						if (r == this.start) {
							index = i;
							break;
						}
						i++;
					}
				}
				return index;
			}
			set { this.startIndex = value; }
		}

		public int EndIndex {
			get {
				if (this.endIndex >= 0) {
					return this.endIndex;
				}
				int index = -1;
				if (this.Circuit is HithermCircuit) {
					HithermCircuit hc = this.Circuit as HithermCircuit;
					int i = 0;
					foreach (HithermRegister r in hc.Registers) {
						if (r == this.end) {
							index = i;
							break;
						}
						i++;
					}
				}
				return index;
			}
			set { this.endIndex = value; }
		}

		[XmlIgnore]
		public Circuit Circuit {
			get {
				if (this.circuitIndex >= 0) {
					this.circuit = this.Product.Product.PlannedCircuits[this.circuitIndex];
					this.circuitIndex = -1;
				}
				return this.circuit;
			}
		}

		public int CircuitIndex {
			get {
				if (this.circuitIndex >= 0) {
					return this.circuitIndex;
				}
				int index = -1;
				int i = 0;
				foreach (Circuit c in this.Product.Product.PlannedCircuits) {
					if (c == this.circuit) {
						index = i;
						break;
					}
					i++;
				}
				return index;
			}
			set { this.circuitIndex = value; }
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

		public double GetLength() {
			double length = 0;
			if (this.vertices.Count > 1) {
				for (int i = 1; i < this.vertices.Count; i++) {
					length += (this.vertices[i - 1] - this.vertices[i]).GetLength();
				}
			}
			if (length < 0) {
				length = 0;
			}
			return length;
		}

		#region IGraphicalWallObject Members
		public bool HitTest(Point2D planPoint, double xOffset, double yOffset) {
			return this.HitTest(planPoint, 2);
		}

		public void PaintObject(Graphics g, double xOffset, double yOffset, IGraphicalWallObject selectedObject, double scale) {
			this.Draw(g, (this == selectedObject) ? Color.Red : Color.Black);
		}

		public IGraphicalWallObject GetPickedObject(Point2D planPoint, double xOffset, double yOffset) {
			if (HitTest(planPoint, xOffset, yOffset)) {
				return this;
			}
			return null;
		}

		public Polygon2D GetObjectBorders(double xOffset, double yOffset) {
			Nullable<Point2D> prevVertex = null;
			Nullable<Line2D> prevBorderLeft = null;
			Nullable<Line2D> prevBorderRight = null;
			Line2D borderLeft, borderRight;
			Segment2D segment;
			Vector2D vector = new Vector2D(0, 0);
			Vector2D vLeft = new Vector2D();
			Vector2D vRight = new Vector2D();
			List<Point2D> leftPoints = new List<Point2D>();
			List<Point2D> rightPoints = new List<Point2D>();
			Nullable<Point2D> leftPoint, rightPoint;
			foreach (Point2D vertex in this.vertices) {
				if (prevVertex.HasValue && vertex != prevVertex) {
					vector = (vertex - prevVertex.Value);
					vector.Normalize();
					vector = vector * WIDTH / 2.0;
					vLeft = new Vector2D(-vector.Y, vector.X);
					vRight = new Vector2D(vector.Y, -vector.X);
					borderLeft = new Line2D(prevVertex.Value + vLeft, vector);
					borderRight = new Line2D(prevVertex.Value + vRight, vector);
					if (prevBorderLeft.HasValue) {
						leftPoint = Line2D.GetIntersection(borderLeft, prevBorderLeft.Value);
						rightPoint = Line2D.GetIntersection(borderRight, prevBorderRight.Value);
					} else {
						leftPoint = null;
						rightPoint = null;
					}
					if (leftPoint.HasValue) {
						leftPoints.Add(leftPoint.Value);
					} else {
						leftPoints.Add(prevVertex.Value + vLeft);
					}
					if (rightPoint.HasValue) {
						rightPoints.Add(rightPoint.Value);
					} else {
						rightPoints.Add(prevVertex.Value + vRight);
					}
					prevBorderLeft = borderLeft;
					prevBorderRight = borderRight;
				}
				prevVertex = vertex;
			}
			if (vector != new Vector2D(0,0)) {
				rightPoints.Add(prevVertex.Value + vRight);
				leftPoints.Add(prevVertex.Value + vLeft);
			}
			leftPoints.Reverse();
			Polygon2D border = new Polygon2D(leftPoints);
			border.AddRange(rightPoints);
			return border;
		}

		public bool CollisionTest(Polygon2D polygon, double xOffset, double yOffset, bool ignoreBorders) {
			throw new Exception("The method or operation is not implemented.");
		}

		public List<Anchor> GetAnchors(double scale) {
			return new List<Anchor>();
		}

		public bool StartDrag(Anchor anchor, Point2D planPoint, GraphicalWall owningWall) {
			// nothing to do here as the verbindung doesn't have any anchors
			return false;
		}

		public bool MoveDrag(Anchor anchor, Point2D planPoint, GraphicalWall owningWall) {
			// nothing to do here as the verbindung doesn't have any anchors
			return false;
		}

		public bool EndDrag(Anchor anchor, Point2D planPoint, GraphicalWall owningWall) {
			// nothing to do here as the verbindung doesn't have any anchors
			return false;
		}
		#endregion

		public void UpdateStartPoint(GraphicalHithermRegisterWrapper register, GraphicalWall owningWall) {
			if (this.vertices == null || this.vertices.Count < 2) {
				return;
			}
			bool vertexAdded = false;
			if (this.vertices.Count == 2) {
				this.vertices.Insert(0, new Point2D(this.vertices[0]));
				vertexAdded = true;
			}
			Nullable<Vector2D> offset = register.Product.AssociatedRoom.GetWallOffset(owningWall);
			if (!offset.HasValue) {
				return;
			}
			Point2D newStartPoint = register.GetOutputConnectionPoint(offset.Value.X, offset.Value.Y, 0);
			Point2D newStartPoint5 = register.GetOutputConnectionPoint(offset.Value.X, offset.Value.Y, 5);
			Point2D firstPoint = this.vertices[0];
			Point2D secondPoint = this.vertices[1];
			Vector2D delta = new Segment2D(secondPoint, firstPoint).GetDelta();
			bool moved = false;
			if (Math.Abs(delta.X) < 0.00001 && Math.Abs(delta.Y) >= 0.00001 && !vertexAdded) {
				this.vertices.Insert(0, new Point2D(this.vertices[0]));
			}
			if (Math.Abs(delta.Y) <= 0.00001) {
				firstPoint = this.vertices[0];
				secondPoint = this.vertices[1];
				Point2D thirdPoint = this.vertices[2];
				bool upside = secondPoint.Y < thirdPoint.Y;
				this.MoveHorizontalSegment(0, -firstPoint.Y + newStartPoint.Y);
				firstPoint = this.vertices[0];
				secondPoint = this.vertices[1];
				thirdPoint = this.vertices[2];
				if ((!register.Register.GraphVorlaufRight && newStartPoint5.X > secondPoint.X) ||
					(register.Register.GraphVorlaufRight && newStartPoint5.X < secondPoint.X)) {
					double tmpY;
					if (upside) {
						tmpY = secondPoint.Y + 10;
					} else {
						tmpY = secondPoint.Y - register.Height - 10;
					}
					this.vertices.RemoveAt(1);
					// oder 1
					this.vertices.Insert(1, new Point2D(secondPoint.X, tmpY));
					this.vertices.Insert(1, new Point2D(newStartPoint5.X, tmpY));
					this.vertices.Insert(1, new Point2D(newStartPoint5.X, newStartPoint5.Y));
					this.vertices[0] = newStartPoint;
				} else {
					this.vertices[0] = newStartPoint;
				}
				moved = true;
			}
			if (!moved) {
				this.vertices[0] = newStartPoint;
			}
			this.Simplify();
			Polygon2D borders = this.GetObjectBorders(0, 0);
			try {
				foreach (IGraphicalWallObject obj in owningWall.Registers) {
					if (obj.CollisionTest(borders, offset.Value.X, offset.Value.Y, true)) {
						this.vertices.Clear();
						break;
					}
				}
			} catch {
				// the borders-polygon is invalid as it intersects itself
				this.vertices.Clear();
			}
		}

		public void UpdateEndPoint(GraphicalHithermRegisterWrapper register, GraphicalWall owningWall) {
			if (this.vertices == null || this.vertices.Count < 2) {
				return;
			}
			bool vertexAdded = false;
			if (this.vertices.Count == 2) {
				this.vertices.Insert(this.vertices.Count - 1, new Point2D(this.vertices[this.vertices.Count - 1]));
				vertexAdded = true;
			}
			Nullable<Vector2D> offset = register.Product.AssociatedRoom.GetWallOffset(owningWall);
			if (!offset.HasValue) {
				return;
			}
			Point2D newEndPoint = register.GetInputConnectionPoint(offset.Value.X, offset.Value.Y, 0);
			Point2D newEndPoint5 = register.GetInputConnectionPoint(offset.Value.X, offset.Value.Y, 5);
			Point2D lastPoint = this.vertices[this.vertices.Count - 1];
			Point2D prevLastPoint = this.vertices[this.vertices.Count - 2];
			Vector2D delta = new Segment2D(prevLastPoint, lastPoint).GetDelta();
			bool moved = false;
			if (Math.Abs(delta.X) < 0.00001 && Math.Abs(delta.Y) >= 0.00001 && !vertexAdded) {
				this.vertices.Insert(this.vertices.Count - 1, new Point2D(this.vertices[this.vertices.Count - 1]));
			}
			if (Math.Abs(delta.Y) <= 0.00001) {
				lastPoint = this.vertices[this.vertices.Count - 1];
				prevLastPoint = this.vertices[this.vertices.Count - 2];
				Point2D prevPrevLastPoint = this.vertices[this.vertices.Count - 3];
				bool upside = prevLastPoint.Y < prevPrevLastPoint.Y;
				this.MoveHorizontalSegment(this.vertices.Count - 2, -lastPoint.Y + newEndPoint.Y);
				lastPoint = this.vertices[this.vertices.Count - 1];
				prevLastPoint = this.vertices[this.vertices.Count - 2];
				prevPrevLastPoint = this.vertices[this.vertices.Count - 3];
				if ((register.Register.GraphVorlaufRight && newEndPoint5.X > prevLastPoint.X) ||
					(!register.Register.GraphVorlaufRight && newEndPoint5.X < prevLastPoint.X)) {
					double tmpY;
					if (upside) {
						tmpY = prevLastPoint.Y + register.Height + 10;
					} else {
						tmpY = prevLastPoint.Y - 10;
					}
					this.vertices.RemoveAt(this.vertices.Count - 2);
					this.vertices.Insert(this.vertices.Count - 1, new Point2D(prevLastPoint.X, tmpY));
					this.vertices.Insert(this.vertices.Count - 1, new Point2D(newEndPoint5.X, tmpY));
					this.vertices.Insert(this.vertices.Count - 1, new Point2D(newEndPoint5.X, lastPoint.Y));
					this.vertices[this.vertices.Count - 1] = newEndPoint;
				} else {
					this.vertices[this.vertices.Count - 1] = newEndPoint;
				}
				moved = true;
			}
			if (!moved) {
				this.vertices[this.vertices.Count - 1] = newEndPoint;
			}
			this.Simplify();
			Polygon2D borders = this.GetObjectBorders(0, 0);
			try {
				foreach (IGraphicalWallObject obj in owningWall.Registers) {
					if (obj.CollisionTest(borders, offset.Value.X, offset.Value.Y, true)) {
						this.vertices.Clear();
						break;
					}
				}
			} catch {
				// the borders-polygon is invalid as it intersects itself
				this.vertices.Clear();
			}
		}

		public void MoveVerticalSegment(int segmentIndex, double delta) {
			if (this.vertices.Count < segmentIndex + 2) {
				return;
			}
			Vector2D deltaVector = new Vector2D(delta, 0);
			this.vertices[segmentIndex] += deltaVector;
			this.vertices[segmentIndex + 1] += deltaVector;
		}

		public void MoveHorizontalSegment(int segmentIndex, double delta) {
			if (this.vertices.Count < segmentIndex + 2) {
				return;
			}
			Vector2D deltaVector = new Vector2D(0, delta);
			this.vertices[segmentIndex] += deltaVector;
			this.vertices[segmentIndex + 1] += deltaVector;
		}

		public void Simplify() {
			Point2D vertex, prevVertex, nextVertex;
			for (int i = 1; i < this.vertices.Count - 1; i++) {
				prevVertex = this.vertices[i - 1];
				vertex = this.vertices[i];
				if (vertex == prevVertex) {
					this.vertices.RemoveAt(i);
					i--;
				} else {
					nextVertex = this.vertices[i + 1];
					if (new Segment2D(prevVertex, nextVertex).GetDistance(vertex) < 0.0001) {
						this.vertices.RemoveAt(i);
						i--;
					}
				}
			}
			if (this.vertices[this.vertices.Count - 2] == this.vertices[this.vertices.Count - 1]) {
				this.vertices.RemoveAt(this.vertices.Count - 1);
			}
			for (int i = 1; i < this.vertices.Count; i++) {
				Segment2D seg1 = new Segment2D(this.vertices[i - 1], this.vertices[i]);
				for (int j = i + 2; j < this.vertices.Count; j++) {
					Segment2D seg2 = new Segment2D(this.vertices[j - 1], this.vertices[j]);
					double[] pArr, qArr;
					if (Segment2D.GetIntersectionParameters(seg1, seg2, out pArr, out qArr) && pArr.Length == 1) {
						Point2D intersection = seg1.Start + seg1.GetDelta() * pArr[0];
						int count = j - i;
						for (int k = 0; k < count; k++) {
						//for (int k = i; k < j; k++) {
							j--;
							this.vertices.RemoveAt(i);
						}
						j++;
						this.vertices.Insert(i, intersection);
						if (j < i + 1) {
							j = i + 1;
						}
					}
				}
			}
		}
	}
}
