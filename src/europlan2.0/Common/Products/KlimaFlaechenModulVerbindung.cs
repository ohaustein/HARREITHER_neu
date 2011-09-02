using System;
using System.Collections.Generic;
using System.Text;
using WW.Math;
using System.Drawing;
using System.Xml.Serialization;
using WW.Math.Geometry;
using WW.Cad.Model;
using WW.Cad.Model.Tables;
using WW.Cad.Model.Entities;

namespace Europlan.Common {
	public interface IKlimaFlaechenVerbindung {
		void Draw(Graphics g, Matrix4D additionalTransformation, Color c, double measure);
		bool HitTest(Point2D planPoint, double maxDist);
		double GetDistance(Point2D planPoint);

		Circuit Circuit {
			get;
		}

		int CircuitIndex {
			get;
			set;
		}

		[XmlIgnore]
		PlannedProduct Product {
			get;
		}

		string ProductGuid {
			set;
			get;
		}

		//double GetLength(double measure);

		void InvertDirection();

		List<KlimaFlaechenModul> GetEnds();
		List<KlimaFlaechenModul> GetStarts();

		bool StartConnectedToAnbindung {
			get;
		}

		bool EndConnectedToAnbindung {
			get;
		}

		int DistributorIndex {
			get;
			set;
		}
	}

	public class KlimaFlaechenModulVerbindung : IKlimaFlaechenVerbindung {
		private KlimaFlaechenModul start;
		private KlimaFlaechenModul end;
		private List<Point2D> vertices;
		private int startIndex = -1;
		private int endIndex = -1;
		private int startRow = -1;
		private int endRow = -1;
		private int startSubarea = -1;
		private int endSubarea = -1;
		private Circuit circuit;
		private int circuitIndex = -1;
		private PlannedProduct product;
		private string productGuid = null;
		private int distributorIndex = -1;
		private bool flexible = false;

		public List<Point2D> Vertices {
		  get { return vertices; }
		  set { vertices = value; }
		}

		internal KlimaFlaechenModulVerbindung() {
		}

		public KlimaFlaechenModulVerbindung(KlimaFlaechenModul start, KlimaFlaechenModul end, IEnumerable<Point2D> vertices, Circuit circuit, PlannedProduct product, int distributorIndex) {
			this.Initialize(start, end, vertices, circuit, product, distributorIndex);
		}

		public KlimaFlaechenModulVerbindung(KlimaFlaechenModul start, KlimaFlaechenModul end, IEnumerable<Point2D> vertices, Circuit circuit, PlannedProduct product) {
			this.Initialize(start, end, vertices, circuit, product, -1);
		}

		public KlimaFlaechenModulVerbindung(KlimaFlaechenModul modul, bool isStart, IEnumerable<Point2D> vertices, Circuit circuit, PlannedProduct product, int distributorIndex) {
			this.Initialize(isStart ? modul : null, isStart ? null : modul, vertices, circuit, product, distributorIndex);
		}

		private void Initialize(KlimaFlaechenModul start, KlimaFlaechenModul end, IEnumerable<Point2D> vertices, Circuit circuit, PlannedProduct product, int distributorIndex) {
			this.distributorIndex = distributorIndex;
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
			if (this.StartConnectedToAnbindung) {
				c = Color.Red;
			} else if (this.EndConnectedToAnbindung) {
				c = Color.Blue;
			}
			Point2D newVertex2D;
			Point2D oldVertex2D = additionalTransformation.TransformTo2D(this.vertices[0]);
			PointF newVertex;
			PointF oldVertex = new PointF((float)oldVertex2D.X, (float)oldVertex2D.Y);
			//bool first = true;
			Pen p = new Pen(c, (float)(0.021 * measure * additionalTransformation.M00));
			if (this.flexible) {
				p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
				//p.DashPattern = new float[] { 3, 1 };
			}
			p.EndCap = System.Drawing.Drawing2D.LineCap.Round;
			Point2D vertex;
			for (int i = 1; i < this.vertices.Count; i++) {
			//foreach (Point2D vertex in vertices) {
				vertex = this.vertices[i];
				newVertex2D = additionalTransformation.TransformTo2D(vertex);
				newVertex = new PointF((float)newVertex2D.X, (float)newVertex2D.Y);
				if (i == 2) {
					p.StartCap = System.Drawing.Drawing2D.LineCap.Round;
				}
				if (i == this.vertices.Count - 1) {
					p.EndCap = System.Drawing.Drawing2D.LineCap.Flat;
				}
				g.DrawLine(p, oldVertex, newVertex);
				/*if (first) {
					first = false;
				} else {
					g.DrawLine(p, oldVertex, newVertex);
				}*/
				oldVertex = newVertex;
			}
			// TODO
		}

		public void DrawDxf(DxfModel model, DxfLayer connectionLayer, Color c) {
			if (this.StartConnectedToAnbindung) {
				c = Color.Red;
			} else if (this.EndConnectedToAnbindung) {
				c = Color.Blue;
			}
			if (this.vertices.Count < 2) {
				return;
			}
			Point2D oldVertex = this.vertices[0];
			Point2D newVertex;

			for (int i = 1; i < this.vertices.Count; i++) {
				newVertex = this.vertices[i];

				DxfLine line = new DxfLine(c, oldVertex, newVertex);
				line.Layer = connectionLayer;
				model.Entities.Add(line);

				oldVertex = newVertex;
			}
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
		public KlimaFlaechenModul Start {
			get {
				if (this.startIndex >= 0) {
					if (this.Circuit is ModulBodenCircuit) {
						ModulBodenCircuit mbc = this.Circuit as ModulBodenCircuit;
						this.start = mbc.Row.List[this.startIndex];
						this.startIndex = -1;
					} else if (this.Circuit is ModulDeckeCircuit) {
						if (this.startRow >= 0 && this.startSubarea >= 0) {
							ModulDeckeCircuit mdc = this.Circuit as ModulDeckeCircuit;
							this.start = mdc.SubAreas[this.startSubarea].Rows[this.startRow].List[this.startIndex];
							this.startIndex = -1;
							this.startRow = -1;
							this.startSubarea = -1;
						}
					}
				}
				return this.start;
			}
			set {
				this.startIndex = -1;
				this.start = value;
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
						if (this.endRow >= 0 && this.endSubarea >= 0) {
							ModulDeckeCircuit mdc = this.Circuit as ModulDeckeCircuit;
							this.end = mdc.SubAreas[this.endSubarea].Rows[this.endRow].List[this.endIndex];
							this.endIndex = -1;
							this.endRow = -1;
							this.endSubarea = -1;
						}
					}
				}
				return this.end;
			}
			set {
				this.endIndex = -1;
				this.end = value;
			}
		}

		public int StartIndex {
			get {
				if (this.startIndex >= 0) {
					return this.startIndex;
				}
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
					ModulDeckeCircuit mdc = this.Circuit as ModulDeckeCircuit;
					foreach (ModulDeckeSubArea sa in mdc.SubAreas) {
						foreach (KlimaFlaechenList row in sa.Rows) {
							int i = 0;
							foreach (KlimaFlaechenModul m in row.List) {
								if (m == this.start) {
									index = i;
									break;
								}
								i++;
							}
							if (index >= 0) {
								break;
							}
						}
						if (index >= 0) {
							break;
						}
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
					ModulDeckeCircuit mdc = this.Circuit as ModulDeckeCircuit;
					foreach (ModulDeckeSubArea sa in mdc.SubAreas) {
						foreach (KlimaFlaechenList row in sa.Rows) {
							int i = 0;
							foreach (KlimaFlaechenModul m in row.List) {
								if (m == this.end) {
									index = i;
									break;
								}
								i++;
							}
							if (index >= 0) {
								break;
							}
						}
						if (index >= 0) {
							break;
						}
					}
				}
				return index;
			}
			set { this.endIndex = value; }
		}

		public int StartRow {
			get {
				if (this.startRow >= 0) {
					return this.startRow;
				}
				if (this.start == null) {
					return -1;
				}
				int index = -1;
				if (this.Circuit is ModulBodenCircuit) {
					// klimaboden doesn't have multiple rows
					index = 0;
				} else if (this.Circuit is ModulDeckeCircuit) {
					ModulDeckeCircuit mdc = this.Circuit as ModulDeckeCircuit;
					foreach (ModulDeckeSubArea sa in mdc.SubAreas) {
						int i = 0;
						foreach (KlimaFlaechenList row in sa.Rows) {
							if (row.List.Contains(this.start)) {
								index = i;
								break;
							}
							i++;
						}
						if (index >= 0) {
							break;
						}
					}
				}
				return index;
			}
			set { this.startRow = value; }
		}

		public int EndRow {
			get {
				if (this.endRow >= 0) {
					return this.endRow;
				}
				if (this.end == null) {
					return -1;
				}
				int index = -1;
				if (this.Circuit is ModulBodenCircuit) {
					// klimaboden doesn't have multiple rows
					index = 0;
				} else if (this.Circuit is ModulDeckeCircuit) {
					ModulDeckeCircuit mdc = this.Circuit as ModulDeckeCircuit;
					foreach (ModulDeckeSubArea sa in mdc.SubAreas) {
						int i = 0;
						foreach (KlimaFlaechenList row in sa.Rows) {
							if (row.List.Contains(this.end)) {
								index = i;
								break;
							}
							i++;
						}
						if (index >= 0) {
							break;
						}
					}
				}
				return index;
			}
			set { this.endRow = value; }
		}

		public int StartSubarea {
			get {
				if (this.startSubarea >= 0) {
					return this.startSubarea;
				}
				if (this.start == null) {
					return -1;
				}
				int index = -1;
				if (this.Circuit is ModulBodenCircuit) {
					// klimaboden doesn't have multiple rows
					index = 0;
				} else if (this.Circuit is ModulDeckeCircuit) {
					ModulDeckeCircuit mdc = this.Circuit as ModulDeckeCircuit;
					int i = 0;
					foreach (ModulDeckeSubArea sa in mdc.SubAreas) {
						foreach (KlimaFlaechenList row in sa.Rows) {
							if (row.List.Contains(this.start)) {
								index = i;
								break;
							}
						}
						if (index >= 0) {
							break;
						}
						i++;
					}
				}
				return index;
			}
			set { this.startSubarea = value; }
		}

		public int EndSubarea {
			get {
				if (this.endSubarea >= 0) {
					return this.endSubarea;
				}
				if (this.end == null) {
					return -1;
				}
				int index = -1;
				if (this.Circuit is ModulBodenCircuit) {
					// klimaboden doesn't have multiple rows
					index = 0;
				} else if (this.Circuit is ModulDeckeCircuit) {
					ModulDeckeCircuit mdc = this.Circuit as ModulDeckeCircuit;
					int i = 0;
					foreach (ModulDeckeSubArea sa in mdc.SubAreas) {
						foreach (KlimaFlaechenList row in sa.Rows) {
							if (row.List.Contains(this.end)) {
								index = i;
								break;
							}
						}
						if (index >= 0) {
							break;
						}
						i++;
					}
				}
				return index;
			}
			set { this.endSubarea = value; }
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

		public bool IsLangerFitting(double measure) {
			return this.vertices.Count == 2 && this.Start != null && this.End != null && Math.Abs((this.vertices[0] - this.vertices[1]).GetLength() / measure - (0.1 + 2 * KlimaFlaechenModul.CONNECTION_DISTANCE)) < 0.0001;
		}

		public int DistributorIndex {
			get { return this.distributorIndex; }
			set { this.distributorIndex = value; }
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
			length -= 2 * KlimaFlaechenModul.CONNECTION_DISTANCE;
			if (length < 0) {
				length = 0;
			}
			return length;
		}

		public void InvertDirection() {
			KlimaFlaechenModul tmpM = this.start;
			this.start = this.end;
			this.end = tmpM;

			int tmpI = this.startIndex;
			this.startIndex = this.endIndex;
			this.endIndex = tmpI;

			tmpI = this.startSubarea;
			this.startSubarea = this.endSubarea;
			this.endSubarea = tmpI;

			tmpI = this.startRow;
			this.startRow = this.endRow;
			this.endRow = tmpI;

			this.vertices.Reverse();
		}

		public List<KlimaFlaechenModul> GetEnds() {
			List < KlimaFlaechenModul > ends = new List<KlimaFlaechenModul>();
			ends.Add(this.End);
			return ends;
		}

		public List<KlimaFlaechenModul> GetStarts() {
			List<KlimaFlaechenModul> start = new List<KlimaFlaechenModul>();
			start.Add(this.Start);
			return start;
		}

		public bool StartConnectedToAnbindung {
			get {
				return (this.Start == null);
			}
		}

		public bool EndConnectedToAnbindung {
			get {
				return (this.End == null);
			}
		}

		public bool IsFlexible {
			get { return this.flexible; }
			set { this.flexible = value; }
		}
	}
}
