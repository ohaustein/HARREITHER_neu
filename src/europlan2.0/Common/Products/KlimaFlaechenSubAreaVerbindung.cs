using System;
using System.Collections.Generic;
using System.Text;
using WW.Math;
using System.Drawing;
using System.Xml.Serialization;
using WW.Math.Geometry;

namespace Europlan.Common {
	public class KlimaFlaechenSubAreaVerbindung {
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
			//Point2D oldVertex2D;
			Pen p = new Pen(c, (float)(0.021 * measure * additionalTransformation.M00));
			foreach (List<Point2D> v in vertices) {
				Point2D newVertex2D;
				PointF oldVertex = PointF.Empty;
				PointF newVertex;
				bool first = true;
				foreach (Point2D vertex in v) {
					newVertex2D = additionalTransformation.TransformTo2D(vertex);
					newVertex = new PointF((float)newVertex2D.X, (float)newVertex2D.Y);
					if (first) {
						first = false;
					} else {
						g.DrawLine(p, oldVertex, newVertex);
					}
					oldVertex = newVertex;
				}
			}
			// TODO
		}

		public bool HitTest(Point2D planPoint, double maxDist) {
			return this.GetDistance(planPoint) <= maxDist;
		}

		public double GetDistance(Point2D planPoint) {
			Point2D oldVertex = new Point2D();
			double bestDist = double.MaxValue;
			foreach (List<Point2D> v in this.vertices) {
				bool first = false;
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

		public Nullable<Point2D> GetClosestPoint(Point2D planPoint, out double bestDist) {
			Point2D oldVertex = new Point2D();
			bestDist = double.MaxValue;
			Nullable<Point2D> bestPoint = null;
			foreach (List<Point2D> v in this.vertices) {
				bool first = false;
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
	}
}
