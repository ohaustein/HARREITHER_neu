using System;
using System.Collections.Generic;
using System.Text;
using WW.Math;
using System.Drawing;
using System.Xml.Serialization;
using WW.Math.Geometry;

namespace Europlan.Common {
	public class GraphicalProductConnection {


		private PlannedProduct product;
		private string productGuid = null;
		private Distributor distributor;
		private string distributorId;
		private List<Point2D> vertices;
		private Circuit productCircuit;
		private int productCircuitIndex = -1;
		private int distributorIndex = -1;
		private bool vorlauf = true;
		private Product.ProductType connectionType;

		public List<Point2D> Vertices {
		  get { return vertices; }
		  set { vertices = value; }
		}

		internal GraphicalProductConnection() {
			connectionType = Europlan.Common.Product.ProductType.REST;
		}

		public GraphicalProductConnection(PlannedProduct product, Distributor distributor, IEnumerable<Point2D> vertices, Circuit productCircuit, int distributorIndex, bool vorlauf, Product.ProductType connectionType) {
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
			this.productCircuit = productCircuit;
			this.distributorIndex = distributorIndex;
			this.vorlauf = vorlauf;
			this.connectionType = connectionType;
		}

		public void Draw(Graphics g, Matrix4D additionalTransformation, Color c, double measure) {
			Point2D newVertex2D;
			PointF oldVertex = PointF.Empty;
			PointF newVertex;
			bool first = true;
			Pen p = new Pen(c, (float)(0.021 * measure * additionalTransformation.M00));
			foreach (Point2D vertex in vertices) {
				newVertex2D = additionalTransformation.TransformTo2D(vertex);
				newVertex = new PointF((float)newVertex2D.X, (float)newVertex2D.Y);
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

		public int DistributorIndex {
			get { return this.distributorIndex; }
			set { this.distributorIndex = value; }
		}

		[XmlIgnore]
		public Circuit ProductCircuit {
			get {
				if (this.productCircuitIndex >= 0) {
					this.productCircuit = this.Product.Product.PlannedCircuits[this.productCircuitIndex];
					this.productCircuitIndex = -1;
				}
				return this.productCircuit;
			}
		}

		public int ProductCircuitIndex {
			get {
				if (this.productCircuitIndex >= 0) {
					return this.productCircuitIndex;
				}
				int index = -1;
				int i = 0;
				if (this.Product != null && this.Product.Product != null && this.Product.Product.PlannedCircuits != null) {
					foreach (Circuit c in this.Product.Product.PlannedCircuits) {
						if (c == this.productCircuit) {
							index = i;
							break;
						}
						i++;
					}
				}
				return index;
			}
			set { this.productCircuitIndex = value; }
		}

		public bool Vorlauf {
			get { return this.vorlauf; }
			set { this.vorlauf = value; }
		}

		public Product.ProductType ConnectionType {
			get { return this.connectionType; }
			set { this.connectionType = value; }
		}

		//     Input: a 2D segment S from point P0 to point P1
        //     a 2D convex polygon W with n vertices V0,...,Vn-1,Vn=V0
		private List<Segment2D> GetSegmentsInPolygon(Segment2D s, Polygon2D w) {
			List<Segment2D> result = new List<Segment2D>();
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

			Nullable<Point2D> lastPoint = null;
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
			if (this.vertices == null || this.vertices.Count < 2) {
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
	}
}
