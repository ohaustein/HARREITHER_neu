using System;
using System.Collections.Generic;
using System.Text;
using WW.Math;
using System.Drawing;
using System.Xml.Serialization;
using WW.Math.Geometry;

namespace Europlan.Common {
	public class HithermRegisterVerbindung : IGraphicalWallObject {
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
			foreach (Point2D vertex in vertices) {
				newVertex = new PointF((float)vertex.X, (float)vertex.Y);
				if (first) {
					first = false;
				} else {
					g.DrawLine(p, oldVertex, newVertex);
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
			throw new Exception("The method or operation is not implemented.");
		}

		public bool CollisionTest(Polygon2D polygon, double xOffset, double yOffset) {
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
	}
}
