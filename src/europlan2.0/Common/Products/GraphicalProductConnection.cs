using System;
using System.Collections.Generic;
using System.Text;
using WW.Math;
using System.Drawing;
using System.Xml.Serialization;
using WW.Math.Geometry;

namespace Europlan.Common {
	public class GraphicalProductConnection : IPickableObject {
		private PlannedProduct product;
		private string productGuid = null;
		private Distributor distributor;
		private string distributorId;
		private List<Point2D> vertices;
		private Circuit productCircuit;
		private int productCircuitIndex = -1;
		private int distributorIndex = -1;
		private bool vorlauf = true;

		public List<Point2D> Vertices {
		  get { return vertices; }
		  set { vertices = value; }
		}

		internal GraphicalProductConnection() {
		}

		public GraphicalProductConnection(PlannedProduct product, Distributor distributor, IEnumerable<Point2D> vertices, Circuit productCircuit, int distributorIndex, bool vorlauf) {
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
		}

		public void Draw(Graphics g, Matrix4D additionalTransformation, Color c, double measure) {
			//Point2D oldVertex2D;
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

		#region IPickableObject Members
		public bool HitTest(Point2D planPoint, Point pointInControl) {
			// TODO
			throw new Exception("The method or operation is not implemented.");
		}

		public System.Windows.Forms.Cursor PickCursor {
			get {
				// TODO
				throw new Exception("The method or operation is not implemented.");
			}
		}

		public bool HitTest(Point2D planPoint, double maxDist) {
			Point2D oldVertex = new Point2D();
			bool first = false;
			foreach (Point2D newVertex in this.vertices) {
				if (first) {
					first = false;
				} else {
					Segment2D segment = new Segment2D(oldVertex, newVertex);
					double dist = segment.GetDistance(planPoint);
					if (dist <= maxDist) {
						return true;
					}
				}
				oldVertex = newVertex;
			}
			return false;
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
			get { return this.product.Id; }
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
			get { return this.Distributor == null ? null : this.Distributor.Id; }
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
				int index = -1;
				int i = 0;
				foreach (Circuit c in this.Product.Product.PlannedCircuits) {
					if (c == this.productCircuit) {
						index = i;
						break;
					}
					i++;
				}
				return index;
			}
			set { this.productCircuitIndex = value; }
		}

		public bool Vorlauf {
			get { return this.vorlauf; }
			set { this.vorlauf = value; }
		}

		#endregion
	}
}
