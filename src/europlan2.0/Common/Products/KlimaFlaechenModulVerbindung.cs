using System;
using System.Collections.Generic;
using System.Text;
using WW.Math;
using System.Drawing;
using System.Xml.Serialization;
using WW.Math.Geometry;

namespace Europlan.Common {
	public class KlimaFlaechenModulVerbindung : IPickableObject {
		private KlimaFlaechenModul start;
		private KlimaFlaechenModul end;
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

		internal KlimaFlaechenModulVerbindung() {
		}

		public KlimaFlaechenModulVerbindung(KlimaFlaechenModul start, KlimaFlaechenModul end, IEnumerable<Point2D> vertices, Circuit circuit, PlannedProduct product) {
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
		public KlimaFlaechenModul Start {
			get {
				if (this.startIndex >= 0) {
					if (this.Circuit is ModulBodenCircuit) {
						ModulBodenCircuit mbc = this.Circuit as ModulBodenCircuit;
						this.start = mbc.Row.List[this.startIndex];
						this.startIndex = -1;
					} else if (this.Circuit is ModulDeckeCircuit) {
						throw new Exception("todo");
					}
				}
				return this.start;
			}
		}

		[XmlIgnore]
		public KlimaFlaechenModul End {
			get {
				if (this.endIndex >= 0) {
					if (this.Circuit is ModulBodenCircuit) {
						ModulBodenCircuit mbc = this.Circuit as ModulBodenCircuit;
						this.end = mbc.Row.List[this.endIndex];
						this.endIndex = -1;
					} else if (this.Circuit is ModulDeckeCircuit) {
						throw new Exception("todo");
					}
				}
				return this.end;
			}
		}

		public int StartIndex {
			get {
				int index = -1;
				if (this.Circuit is ModulBodenCircuit) {
					ModulBodenCircuit mbc = this.Circuit as ModulBodenCircuit;
					int i = 0;
					foreach (KlimaFlaechenModul m in mbc.Row.List) {
						if (m == this.start) {
							index = i;
							break;
						}
						i++;
					}
				} else if (this.Circuit is ModulDeckeCircuit) {
					throw new Exception("todo");
				}
				return index;
			}
			set { this.startIndex = value; }
		}

		public int EndIndex {
			get {
				int index = -1;
				if (this.Circuit is ModulBodenCircuit) {
					ModulBodenCircuit mbc = this.Circuit as ModulBodenCircuit;
					int i = 0;
					foreach (KlimaFlaechenModul m in mbc.Row.List) {
						if (m == this.end) {
							index = i;
							break;
						}
						i++;
					}
				} else if (this.Circuit is ModulDeckeCircuit) {
					throw new Exception("todo");
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
			get { return this.product.Id; }
		}
		#endregion
	}
}
