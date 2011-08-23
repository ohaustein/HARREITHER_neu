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
	public class KlimaFlaechenSubAreaVerbindung : IKlimaFlaechenVerbindung {
		private List<KlimaFlaechenModul> start;
		private List<KlimaFlaechenModul> end;
		private List<List<Point2D>> vertices;
		private List<int> subAreaStartIndices = null;
		private List<int> subAreaEndIndices = null;
		private List<int> rowStartIndices = null;
		private List<int> rowEndIndices = null;
		private List<int> moduleStartIndices = null;
		private List<int> moduleEndIndices = null;
		private Circuit circuit;
		private int circuitIndex = -1;
		private PlannedProduct product;
		private string productGuid = null;
		private int distributorIndex = -1;

		public List<List<Point2D>> Vertices {
		  get { return vertices; }
		  set { vertices = value; }
		}

		internal KlimaFlaechenSubAreaVerbindung() {
			this.moduleStartIndices = new List<int>();
			this.moduleEndIndices = new List<int>();
			this.rowStartIndices = new List<int>();
			this.rowEndIndices = new List<int>();
			this.subAreaStartIndices = new List<int>();
			this.subAreaEndIndices = new List<int>();
		}

		public KlimaFlaechenSubAreaVerbindung(IEnumerable<KlimaFlaechenModul> start, IEnumerable<KlimaFlaechenModul> end, IEnumerable<IEnumerable<Point2D>> vertices, Circuit circuit, PlannedProduct product) {
			this.Initialize(start, end, vertices, circuit, product, -1);
		}

		public KlimaFlaechenSubAreaVerbindung(IEnumerable<KlimaFlaechenModul> start, IEnumerable<KlimaFlaechenModul> end, IEnumerable<IEnumerable<Point2D>> vertices, Circuit circuit, PlannedProduct product, int distributorIndex) {
			this.Initialize(start, end, vertices, circuit, product, distributorIndex);
		}

		private void Initialize(IEnumerable<KlimaFlaechenModul> start, IEnumerable<KlimaFlaechenModul> end, IEnumerable<IEnumerable<Point2D>> vertices, Circuit circuit, PlannedProduct product, int distributorIndex) {
			this.distributorIndex = distributorIndex;
			this.start = new List<KlimaFlaechenModul>();
			if (start != null) {
				this.start.AddRange(start);
			}
			this.end = new List<KlimaFlaechenModul>();
			if (end != null) {
				this.end.AddRange(end);
			}
			this.vertices = new List<List<Point2D>>();
			foreach (IEnumerable<Point2D> v in vertices) {
				this.vertices.Add(new List<Point2D>(v));
			}
			Point2D oldVertex = new Point2D();
			Vector2D oldVector = new Vector2D();
			Vector2D newVector = new Vector2D();
			bool first = true;
			bool second = true;
			foreach (List<Point2D> v in this.vertices) {
				List<Point2D> verticesToRemove = new List<Point2D>();
				foreach (Point2D newVertex in v) {
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
					v.Remove(vertex);
				}
			}
			this.circuit = circuit;
			this.product = product;
		}

		public void Draw(Graphics g, Matrix4D additionalTransformation, Color c, double measure) {
			Pen p = new Pen(c, (float)(0.021 * measure * additionalTransformation.M00));
			foreach (List<Point2D> v in vertices) {
				Point2D newVertex2D;
				Point2D oldVertex2D = additionalTransformation.TransformTo2D(v[0]);
				PointF newVertex;
				PointF oldVertex = new PointF((float)oldVertex2D.X, (float)oldVertex2D.Y);
				p.StartCap = System.Drawing.Drawing2D.LineCap.Flat;
				p.EndCap = System.Drawing.Drawing2D.LineCap.Round;
				Point2D vertex;
				for (int i = 1; i < v.Count; i++) {
					vertex = v[i];
					newVertex2D = additionalTransformation.TransformTo2D(vertex);
					newVertex = new PointF((float)newVertex2D.X, (float)newVertex2D.Y);
					if (i == 2) {
						p.StartCap = System.Drawing.Drawing2D.LineCap.Round;
					}
					if (i == v.Count - 1) {
						p.EndCap = System.Drawing.Drawing2D.LineCap.Flat;
					}
					g.DrawLine(p, oldVertex, newVertex);
					oldVertex = newVertex;
				}
			}
			// TODO
		}

		public void DrawDxf(DxfModel model, DxfLayer connectionLayer, Color c) {
			foreach (List<Point2D> v in vertices) {
				Point2D oldVertex = v[0];
				Point2D newVertex;

				for (int i = 1; i < v.Count; i++) {
					newVertex = v[i];

					DxfLine line = new DxfLine(c, oldVertex, newVertex);
					line.Layer = connectionLayer;
					model.Entities.Add(line);

					oldVertex = newVertex;
				}
			}
		}

		public bool HitTest(Point2D planPoint, double maxDist) {
			return this.GetDistance(planPoint) <= maxDist;
		}

		public double GetDistance(Point2D planPoint) {
			Point2D oldVertex = new Point2D();
			double bestDist = double.MaxValue;
			foreach (List<Point2D> v in this.vertices) {
				bool first = true;
				foreach (Point2D newVertex in v) {
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
			}
			return bestDist;
		}

		[XmlIgnore]
		public List<KlimaFlaechenModul> Start {
			get {
				if (this.moduleStartIndices != null) {
					if (this.Circuit is ModulBodenCircuit) {
						throw new Exception("todo");
					} else if (this.Circuit is ModulDeckeCircuit) {
						ModulDeckeCircuit mdc = this.Circuit as ModulDeckeCircuit;
						this.start = new List<KlimaFlaechenModul>();
						for (int i = 0; i < this.moduleStartIndices.Count; i++) {
							this.start.Add(mdc.SubAreas[this.subAreaStartIndices[i]].Rows[this.rowStartIndices[i]].List[this.moduleStartIndices[i]]);
						}
						this.moduleStartIndices = null;
						this.rowStartIndices = null;
						this.subAreaStartIndices = null;
					}

				}
				return this.start;
			}
		}

		[XmlIgnore]
		public List<KlimaFlaechenModul> End {
			get {
				if (this.moduleEndIndices != null) {
					if (this.Circuit is ModulBodenCircuit) {
						throw new Exception("todo");
					} else if (this.Circuit is ModulDeckeCircuit) {
						ModulDeckeCircuit mdc = this.Circuit as ModulDeckeCircuit;
						this.end = new List<KlimaFlaechenModul>();
						for (int i = 0; i < this.moduleEndIndices.Count; i++) {
							this.end.Add(mdc.SubAreas[this.subAreaEndIndices[i]].Rows[this.rowEndIndices[i]].List[this.moduleEndIndices[i]]);
						}
						this.moduleEndIndices = null;
						this.rowEndIndices = null;
						this.subAreaEndIndices = null;
					}
				}
				return this.end;
			}
		}

		public List<int> ModuleStartIndices {
			get {
				if (this.moduleStartIndices != null) {
					return this.moduleStartIndices;
				}
				List<int> indices = new List<int>();
				if (this.Circuit is ModulBodenCircuit) {
					throw new Exception("todo");
				} else if (this.Circuit is ModulDeckeCircuit) {
					ModulDeckeCircuit mdc = this.Circuit as ModulDeckeCircuit;
					foreach (KlimaFlaechenModul m in this.Start) {
						bool found = false;
						foreach (ModulDeckeSubArea sa in mdc.SubAreas) {
							foreach (KlimaFlaechenList row in sa.Rows) {
								int i = 0;
								foreach (KlimaFlaechenModul m2 in row.List) {
									if (m2 == m) {
										indices.Add(i);
										found = true;
										break;
									}
									i++;
								}
								if (found) {
									break;
								}
							}
							if (found) {
								break;
							}
						}
						if (!found) {
							indices.Add(-1);
						}
					}
				}
				return indices;
			}
			set { this.moduleStartIndices = value; }
		}

		public List<int> ModuleEndIndices {
			get {
				if (this.moduleEndIndices != null) {
					return this.moduleEndIndices;
				}
				List<int> indices = new List<int>();
				if (this.Circuit is ModulBodenCircuit) {
					throw new Exception("todo");
				} else if (this.Circuit is ModulDeckeCircuit) {
					ModulDeckeCircuit mdc = this.Circuit as ModulDeckeCircuit;
					foreach (KlimaFlaechenModul m in this.End) {
						bool found = false;
						foreach (ModulDeckeSubArea sa in mdc.SubAreas) {
							foreach (KlimaFlaechenList row in sa.Rows) {
								int i = 0;
								foreach (KlimaFlaechenModul m2 in row.List) {
									if (m2 == m) {
										indices.Add(i);
										found = true;
										break;
									}
									i++;
								}
								if (found) {
									break;
								}
							}
							if (found) {
								break;
							}
						}
						if (!found) {
							indices.Add(-1);
						}
					}
				}
				return indices;
			}
			set { this.moduleEndIndices = value; }
		}

		public List<int> RowStartIndices {
			get {
				if (this.rowStartIndices != null) {
					return this.rowStartIndices;
				}
				List<int> indices = new List<int>();
				if (this.Circuit is ModulBodenCircuit) {
					throw new Exception("todo");
				} else if (this.Circuit is ModulDeckeCircuit) {
					ModulDeckeCircuit mdc = this.Circuit as ModulDeckeCircuit;
					foreach (KlimaFlaechenModul m in this.Start) {
						bool found = false;
						foreach (ModulDeckeSubArea sa in mdc.SubAreas) {
							int i = 0;
							foreach (KlimaFlaechenList row in sa.Rows) {
								foreach (KlimaFlaechenModul m2 in row.List) {
									if (m2 == m) {
										indices.Add(i);
										found = true;
										break;
									}
								}
								if (found) {
									break;
								}
								i++;
							}
							if (found) {
								break;
							}
						}
						if (!found) {
							indices.Add(-1);
						}
					}
				}
				return indices;
			}
			set { this.rowStartIndices = value; }
		}

		public List<int> RowEndIndices {
			get {
				if (this.rowEndIndices != null) {
					return this.rowEndIndices;
				}
				List<int> indices = new List<int>();
				if (this.Circuit is ModulBodenCircuit) {
					throw new Exception("todo");
				} else if (this.Circuit is ModulDeckeCircuit) {
					ModulDeckeCircuit mdc = this.Circuit as ModulDeckeCircuit;
					foreach (KlimaFlaechenModul m in this.End) {
						bool found = false;
						foreach (ModulDeckeSubArea sa in mdc.SubAreas) {
							int i = 0;
							foreach (KlimaFlaechenList row in sa.Rows) {
								foreach (KlimaFlaechenModul m2 in row.List) {
									if (m2 == m) {
										indices.Add(i);
										found = true;
										break;
									}
								}
								if (found) {
									break;
								}
								i++;
							}
							if (found) {
								break;
							}
						}
						if (!found) {
							indices.Add(-1);
						}
					}
				}
				return indices;
			}
			set { this.rowEndIndices = value; }
		}

		[XmlIgnore]
		public List<KlimaFlaechenList> StartRows {
			get {
				List<KlimaFlaechenList> rows = new List<KlimaFlaechenList>();
				if (this.Circuit is ModulBodenCircuit) {
					throw new Exception("todo");
				} else if (this.Circuit is ModulDeckeCircuit) {
					ModulDeckeCircuit mdc = this.Circuit as ModulDeckeCircuit;
					foreach (KlimaFlaechenModul m in this.Start) {
						bool found = false;
						foreach (ModulDeckeSubArea sa in mdc.SubAreas) {
							foreach (KlimaFlaechenList row in sa.Rows) {
								if (row.List.Contains(m)) {
									rows.Add(row);
									found = true;
									break;
								}
							}
							if (found) {
								break;
							}
						}
						if (!found) {
							rows.Add(null);
						}
					}
				}
				return rows;
			}
		}

		[XmlIgnore]
		public List<KlimaFlaechenList> EndRows {
			get {
				List<KlimaFlaechenList> rows = new List<KlimaFlaechenList>();
				if (this.Circuit is ModulBodenCircuit) {
					throw new Exception("todo");
				} else if (this.Circuit is ModulDeckeCircuit) {
					ModulDeckeCircuit mdc = this.Circuit as ModulDeckeCircuit;
					foreach (KlimaFlaechenModul m in this.End) {
						bool found = false;
						foreach (ModulDeckeSubArea sa in mdc.SubAreas) {
							foreach (KlimaFlaechenList row in sa.Rows) {
								if (row.List.Contains(m)) {
									rows.Add(row);
									found = true;
									break;
								}
							}
							if (found) {
								break;
							}
						}
						if (!found) {
							rows.Add(null);
						}
					}
				}
				return rows;
			}
		}

		public List<int> SubAreaStartIndices {
			get {
				if (this.subAreaStartIndices != null) {
					return this.subAreaStartIndices;
				}
				List<int> indices = new List<int>();
				if (this.Circuit is ModulBodenCircuit) {
					throw new Exception("todo");
				} else if (this.Circuit is ModulDeckeCircuit) {
					ModulDeckeCircuit mdc = this.Circuit as ModulDeckeCircuit;
					foreach (KlimaFlaechenModul m in this.Start) {
						bool found = false;
						int i = 0;
						foreach (ModulDeckeSubArea sa in mdc.SubAreas) {
							foreach (KlimaFlaechenList row in sa.Rows) {
								foreach (KlimaFlaechenModul m2 in row.List) {
									if (m2 == m) {
										indices.Add(i);
										found = true;
										break;
									}
								}
								if (found) {
									break;
								}
							}
							if (found) {
								break;
							}
							i++;
						}
						if (!found) {
							indices.Add(-1);
						}
					}
				}
				return indices;
			}
			set { this.subAreaStartIndices = value; }
		}

		public List<int> SubAreaEndIndices {
			get {
				if (this.subAreaEndIndices != null) {
					return this.subAreaEndIndices;
				}
				List<int> indices = new List<int>();
				if (this.Circuit is ModulBodenCircuit) {
					throw new Exception("todo");
				} else if (this.Circuit is ModulDeckeCircuit) {
					ModulDeckeCircuit mdc = this.Circuit as ModulDeckeCircuit;
					foreach (KlimaFlaechenModul m in this.End) {
						bool found = false;
						int i = 0;
						foreach (ModulDeckeSubArea sa in mdc.SubAreas) {
							foreach (KlimaFlaechenList row in sa.Rows) {
								foreach (KlimaFlaechenModul m2 in row.List) {
									if (m2 == m) {
										indices.Add(i);
										found = true;
										break;
									}
								}
								if (found) {
									break;
								}
							}
							if (found) {
								break;
							}
							i++;
						}
						if (!found) {
							indices.Add(-1);
						}
					}
				}
				return indices;
			}
			set { this.subAreaEndIndices = value; }
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

		public int DistributorIndex {
			get { return this.distributorIndex; }
			set { this.distributorIndex = value; }
		}

		public Nullable<Point2D> GetClosestPoint(Point2D planPoint, out double bestDist) {
			Point2D oldVertex = new Point2D();
			bestDist = double.MaxValue;
			Nullable<Point2D> bestPoint = null;
			foreach (List<Point2D> v in this.vertices) {
				bool first = true;
				foreach (Point2D newVertex in v) {
					if (first) {
						first = false;
					} else {
						Segment2D segment = new Segment2D(oldVertex, newVertex);
						double dist = segment.GetDistance(planPoint);
						if (dist <= bestDist) {
							bestDist = dist;
							bestPoint = segment.GetClosestPoint(planPoint);
						}
					}
					oldVertex = newVertex;
				}
			}
			return bestPoint;
		}

		public void InvertDirection() {
			List<KlimaFlaechenModul> tmpM = this.start;
			this.start = this.end;
			this.end = tmpM;

			List<int> tmpI = this.moduleStartIndices;
			this.moduleStartIndices = this.moduleEndIndices;
			this.moduleEndIndices = tmpI;

			tmpI = this.subAreaStartIndices;
			this.subAreaStartIndices = this.subAreaEndIndices;
			this.subAreaEndIndices = tmpI;

			tmpI = this.rowStartIndices;
			this.rowStartIndices = this.rowEndIndices;
			this.rowEndIndices = tmpI;

			foreach (List<Point2D> vs in this.vertices) {
				vs.Reverse();
			}
		}

		public List<KlimaFlaechenModul> GetEnds() {
			return this.End;
		}

		public List<KlimaFlaechenModul> GetStarts() {
			return this.Start;
		}

		public bool StartConnectedToAnbindung {
			get {
				return (this.Start == null || this.Start.Count == 0);
			}
		}

		public bool EndConnectedToAnbindung {
			get {
				return (this.End == null || this.End.Count == 0);
			}
		}
	}
}