using System;
using System.Collections.Generic;
using System.Text;
using WW.Math;
using System.Drawing;
using System.Xml.Serialization;
using WW.Math.Geometry;
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
		private bool[] flexibleStartConnections = null;
		private bool[] flexibleEndConnections = null;
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
			if (this.StartConnectedToAnbindung) {
				c = Color.Red;
			} else if (this.EndConnectedToAnbindung) {
				c = Color.Blue;
			}
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
		}

		public void DrawDxf(WW.Cad.Model.DxfModel model, DxfLayer connectionLayer, Color c) {
			if (this.StartConnectedToAnbindung) {
				c = Color.Red;
			} else if (this.EndConnectedToAnbindung) {
				c = Color.Blue;
			}
			EntityColor ec = EntityColor.CreateFromRgb(c.ToArgb());
			foreach (List<Point2D> v in vertices) {
				Point2D oldVertex = v[0];
				Point2D newVertex;

				for (int i = 1; i < v.Count; i++) {
					newVertex = v[i];

					DxfLine line = new DxfLine(ec, oldVertex, newVertex);
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
						throw new Exception("modul boden does not support teilflächen");
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
                    else if (this.Circuit is ModulKlimaBoden20Circuit)
                    {
                        ModulKlimaBoden20Circuit mbc = this.Circuit as ModulKlimaBoden20Circuit;
                        this.start = new List<KlimaFlaechenModul>();
                        for (int i = 0; i < this.moduleStartIndices.Count; i++)
                        {
                            this.start.Add(mbc.SubAreas[this.subAreaStartIndices[i]].Rows[this.rowStartIndices[i]].List[this.moduleStartIndices[i]]);
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
						throw new Exception("modul boden does not support teilflächen");
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
                    else if (this.Circuit is ModulKlimaBoden20Circuit)
                    {
                        ModulKlimaBoden20Circuit mbc = this.Circuit as ModulKlimaBoden20Circuit;
                        this.end = new List<KlimaFlaechenModul>();
                        for (int i = 0; i < this.moduleEndIndices.Count; i++)
                        {
                            this.end.Add(mbc.SubAreas[this.subAreaEndIndices[i]].Rows[this.rowEndIndices[i]].List[this.moduleEndIndices[i]]);
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
					throw new Exception("modul boden does not support teilflächen");
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
                else if (this.Circuit is ModulKlimaBoden20Circuit)
                {
                    ModulKlimaBoden20Circuit mdc = this.Circuit as ModulKlimaBoden20Circuit;
                    foreach (KlimaFlaechenModul m in this.Start)
                    {
                        bool found = false;
                        foreach (ModulKlimaBoden20SubArea sa in mdc.SubAreas)
                        {
                            foreach (KlimaFlaechenList row in sa.Rows)
                            {
                                int i = 0;
                                foreach (KlimaFlaechenModul m2 in row.List)
                                {
                                    if (m2 == m)
                                    {
                                        indices.Add(i);
                                        found = true;
                                        break;
                                    }
                                    i++;
                                }
                                if (found)
                                {
                                    break;
                                }
                            }
                            if (found)
                            {
                                break;
                            }
                        }
                        if (!found)
                        {
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
					throw new Exception("modul boden does not support teilflächen");
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
                else if (this.Circuit is ModulKlimaBoden20Circuit)
                {
                    ModulKlimaBoden20Circuit mdc = this.Circuit as ModulKlimaBoden20Circuit;
                    foreach (KlimaFlaechenModul m in this.End)
                    {
                        bool found = false;
                        foreach (ModulKlimaBoden20SubArea sa in mdc.SubAreas)
                        {
                            foreach (KlimaFlaechenList row in sa.Rows)
                            {
                                int i = 0;
                                foreach (KlimaFlaechenModul m2 in row.List)
                                {
                                    if (m2 == m)
                                    {
                                        indices.Add(i);
                                        found = true;
                                        break;
                                    }
                                    i++;
                                }
                                if (found)
                                {
                                    break;
                                }
                            }
                            if (found)
                            {
                                break;
                            }
                        }
                        if (!found)
                        {
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
					throw new Exception("modul boden does not support teilflächen");
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
                else if (this.Circuit is ModulKlimaBoden20Circuit)
                {
                    ModulKlimaBoden20Circuit mdc = this.Circuit as ModulKlimaBoden20Circuit;
                    foreach (KlimaFlaechenModul m in this.Start)
                    {
                        bool found = false;
                        foreach (ModulKlimaBoden20SubArea sa in mdc.SubAreas)
                        {
                            int i = 0;
                            foreach (KlimaFlaechenList row in sa.Rows)
                            {
                                foreach (KlimaFlaechenModul m2 in row.List)
                                {
                                    if (m2 == m)
                                    {
                                        indices.Add(i);
                                        found = true;
                                        break;
                                    }
                                }
                                if (found)
                                {
                                    break;
                                }
                                i++;
                            }
                            if (found)
                            {
                                break;
                            }
                        }
                        if (!found)
                        {
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
					throw new Exception("modul boden does not support teilflächen");
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
                else if (this.Circuit is ModulKlimaBoden20Circuit)
                {
                    ModulKlimaBoden20Circuit mdc = this.Circuit as ModulKlimaBoden20Circuit;
                    foreach (KlimaFlaechenModul m in this.End)
                    {
                        bool found = false;
                        foreach (ModulKlimaBoden20SubArea sa in mdc.SubAreas)
                        {
                            int i = 0;
                            foreach (KlimaFlaechenList row in sa.Rows)
                            {
                                foreach (KlimaFlaechenModul m2 in row.List)
                                {
                                    if (m2 == m)
                                    {
                                        indices.Add(i);
                                        found = true;
                                        break;
                                    }
                                }
                                if (found)
                                {
                                    break;
                                }
                                i++;
                            }
                            if (found)
                            {
                                break;
                            }
                        }
                        if (!found)
                        {
                            indices.Add(-1);
                        }
                    }
                } 
				return indices;
			}
			set { this.rowEndIndices = value; }
		}

		public List<KlimaFlaechenList> GetStartRows() {
			List<KlimaFlaechenList> rows = new List<KlimaFlaechenList>();
			if (this.Circuit is ModulBodenCircuit) {
				throw new Exception("modul boden does not support teilflächen");
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
            else if (this.Circuit is ModulKlimaBoden20Circuit)
            {
                ModulKlimaBoden20Circuit mdc = this.Circuit as ModulKlimaBoden20Circuit;
                foreach (KlimaFlaechenModul m in this.Start)
                {
                    bool found = false;
                    foreach (ModulKlimaBoden20SubArea sa in mdc.SubAreas)
                    {
                        foreach (KlimaFlaechenList row in sa.Rows)
                        {
                            if (row.List.Contains(m))
                            {
                                rows.Add(row);
                                found = true;
                                break;
                            }
                        }
                        if (found)
                        {
                            break;
                        }
                    }
                    if (!found)
                    {
                        rows.Add(null);
                    }
                }
            }
			return rows;
		}

		public List<KlimaFlaechenList> GetEndRows() {
			List<KlimaFlaechenList> rows = new List<KlimaFlaechenList>();
			if (this.Circuit is ModulBodenCircuit) {
				throw new Exception("modul boden does not support teilflächen");
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
            else if (this.Circuit is ModulKlimaBoden20Circuit)
            {
                ModulKlimaBoden20Circuit mdc = this.Circuit as ModulKlimaBoden20Circuit;
                foreach (KlimaFlaechenModul m in this.End)
                {
                    bool found = false;
                    foreach (ModulKlimaBoden20SubArea sa in mdc.SubAreas)
                    {
                        foreach (KlimaFlaechenList row in sa.Rows)
                        {
                            if (row.List.Contains(m))
                            {
                                rows.Add(row);
                                found = true;
                                break;
                            }
                        }
                        if (found)
                        {
                            break;
                        }
                    }
                    if (!found)
                    {
                        rows.Add(null);
                    }
                }
            }
			return rows;
		}

		public List<int> SubAreaStartIndices {
			get {
				if (this.subAreaStartIndices != null) {
					return this.subAreaStartIndices;
				}
				List<int> indices = new List<int>();
				if (this.Circuit is ModulBodenCircuit) {
					throw new Exception("modul boden does not support teilflächen");
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
                else if (this.Circuit is ModulKlimaBoden20Circuit)
                {
                    ModulKlimaBoden20Circuit mdc = this.Circuit as ModulKlimaBoden20Circuit;
                    foreach (KlimaFlaechenModul m in this.Start)
                    {
                        bool found = false;
                        int i = 0;
                        foreach (ModulKlimaBoden20SubArea sa in mdc.SubAreas)
                        {
                            foreach (KlimaFlaechenList row in sa.Rows)
                            {
                                foreach (KlimaFlaechenModul m2 in row.List)
                                {
                                    if (m2 == m)
                                    {
                                        indices.Add(i);
                                        found = true;
                                        break;
                                    }
                                }
                                if (found)
                                {
                                    break;
                                }
                            }
                            if (found)
                            {
                                break;
                            }
                            i++;
                        }
                        if (!found)
                        {
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
					throw new Exception("modul boden does not support teilflächen");
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
                else if (this.Circuit is ModulKlimaBoden20Circuit)
                {
                    ModulKlimaBoden20Circuit mdc = this.Circuit as ModulKlimaBoden20Circuit;
                    foreach (KlimaFlaechenModul m in this.End)
                    {
                        bool found = false;
                        int i = 0;
                        foreach (ModulKlimaBoden20SubArea sa in mdc.SubAreas)
                        {
                            foreach (KlimaFlaechenList row in sa.Rows)
                            {
                                foreach (KlimaFlaechenModul m2 in row.List)
                                {
                                    if (m2 == m)
                                    {
                                        indices.Add(i);
                                        found = true;
                                        break;
                                    }
                                }
                                if (found)
                                {
                                    break;
                                }
                            }
                            if (found)
                            {
                                break;
                            }
                            i++;
                        }
                        if (!found)
                        {
                            indices.Add(-1);
                        }
                    }
                }
				return indices;
			}
			set { this.subAreaEndIndices = value; }
		}

		public List<ModulSubArea> GetStartSubAreas() {
			List<ModulSubArea> subAreas = new List<ModulSubArea>();
			if (this.Circuit is ModulBodenCircuit) {
				throw new Exception("modul boden does not support teilflächen");
			} else if (this.Circuit is ModulDeckeCircuit) {
				ModulDeckeCircuit mdc = this.Circuit as ModulDeckeCircuit;
				foreach (KlimaFlaechenModul m in this.Start) {
					bool found = false;
					int i = 0;
					foreach (ModulDeckeSubArea sa in mdc.SubAreas) {
						foreach (KlimaFlaechenList row in sa.Rows) {
							if (row.List.Contains(m)) {
								subAreas.Add(sa);
								found = true;
								break;
							}
						}
						if (found) {
							break;
						}
						i++;
					}
					if (!found) {
						subAreas.Add(null);
					}
				}
            }
            else if (this.Circuit is ModulKlimaBoden20Circuit)
            {
                ModulKlimaBoden20Circuit mdc = this.Circuit as ModulKlimaBoden20Circuit;
                foreach (KlimaFlaechenModul m in this.Start)
                {
                    bool found = false;
                    int i = 0;
                    foreach (ModulKlimaBoden20SubArea sa in mdc.SubAreas)
                    {
                        foreach (KlimaFlaechenList row in sa.Rows)
                        {
                            if (row.List.Contains(m))
                            {
                                subAreas.Add(sa);
                                found = true;
                                break;
                            }
                        }
                        if (found)
                        {
                            break;
                        }
                        i++;
                    }
                    if (!found)
                    {
                        subAreas.Add(null);
                    }
                }
            }
			return subAreas;
		}

		public List<ModulSubArea> GetEndSubAreas() {
			List<ModulSubArea> subAreas = new List<ModulSubArea>();
			if (this.Circuit is ModulBodenCircuit) {
				throw new Exception("modul boden does not support teilflächen");
			} else if (this.Circuit is ModulDeckeCircuit) {
				ModulDeckeCircuit mdc = this.Circuit as ModulDeckeCircuit;
				foreach (KlimaFlaechenModul m in this.End) {
					bool found = false;
					int i = 0;
					foreach (ModulDeckeSubArea sa in mdc.SubAreas) {
						foreach (KlimaFlaechenList row in sa.Rows) {
							if (row.List.Contains(m)) {
								subAreas.Add(sa);
								found = true;
								break;
							}
						}
						if (found) {
							break;
						}
						i++;
					}
					if (!found) {
						subAreas.Add(null);
					}
				}
            }
            else if (this.Circuit is ModulKlimaBoden20Circuit)
            {
                ModulKlimaBoden20Circuit mdc = this.Circuit as ModulKlimaBoden20Circuit;
                foreach (KlimaFlaechenModul m in this.End)
                {
                    bool found = false;
                    int i = 0;
                    foreach (ModulKlimaBoden20SubArea sa in mdc.SubAreas)
                    {
                        foreach (KlimaFlaechenList row in sa.Rows)
                        {
                            if (row.List.Contains(m))
                            {
                                subAreas.Add(sa);
                                found = true;
                                break;
                            }
                        }
                        if (found)
                        {
                            break;
                        }
                        i++;
                    }
                    if (!found)
                    {
                        subAreas.Add(null);
                    }
                }
            }
			return subAreas;
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
				return (this.End != null && this.End.Count > 0 && this.distributorIndex >= 0);
			}
		}

		public bool EndConnectedToAnbindung {
			get {
				return (this.Start != null && this.Start.Count > 0 && this.distributorIndex >= 0);
			}
		}

		public bool[] FlexibleStartConnections {
			get {
				if (this.flexibleStartConnections == null) {
					this.flexibleStartConnections = new bool[this.Start.Count];
					for (int i = 0; i < this.flexibleStartConnections.Length; i++) {
						this.flexibleStartConnections[i] = false;
					}
				} else if (this.flexibleStartConnections.Length != this.Start.Count) {
					bool[] oldFlexibleStart = this.flexibleStartConnections;
					this.flexibleStartConnections = new bool[this.Start.Count];
					for (int i = 0; i < oldFlexibleStart.Length; i++) {
						this.flexibleStartConnections[i] = oldFlexibleStart[i];
					}
					for (int i = oldFlexibleStart.Length; i < this.flexibleStartConnections.Length; i++) {
						this.flexibleStartConnections[i] = false;
					}
				}
				return this.flexibleStartConnections;
			}
			set { this.flexibleStartConnections = value; }
		}

		public bool isStartFlexible(KlimaFlaechenModul modul) {
			int i = this.Start.IndexOf(modul);
			if (i < 0) {
				return false;
			}
			return this.FlexibleStartConnections[i];
		}

		public void SetStartFlexible(KlimaFlaechenModul modul, bool flexible) {
			int i = this.Start.IndexOf(modul);
			if (i >= 0) {
				this.FlexibleStartConnections[i] = flexible;
			}
		}

		public bool[] FlexibleEndConnections {
			get {
				if (this.flexibleEndConnections == null) {
					this.flexibleEndConnections = new bool[this.End.Count];
					for (int i = 0; i < this.flexibleEndConnections.Length; i++) {
						this.flexibleEndConnections[i] = false;
					}
				} else if (this.flexibleEndConnections.Length != this.End.Count) {
					bool[] oldFlexibleEnd = this.flexibleEndConnections;
					this.flexibleEndConnections = new bool[this.End.Count];
					for (int i = 0; i < oldFlexibleEnd.Length; i++) {
						this.flexibleEndConnections[i] = oldFlexibleEnd[i];
					}
					for (int i = oldFlexibleEnd.Length; i < this.flexibleEndConnections.Length; i++) {
						this.flexibleEndConnections[i] = false;
					}
				}
				return this.flexibleEndConnections;
			}
			set { this.flexibleEndConnections = value; }
		}

		public bool isEndFlexible(KlimaFlaechenModul modul) {
			int i = this.End.IndexOf(modul);
			if (i < 0) {
				return false;
			}
			return this.FlexibleEndConnections[i];
		}

		public void SetEndFlexible(KlimaFlaechenModul modul, bool flexible) {
			int i = this.End.IndexOf(modul);
			if (i >= 0) {
				this.FlexibleEndConnections[i] = flexible;
			}
		}

		#region Graphical Materials
		public double GetLength(double measure) {
			if (measure == 0 || this.vertices == null) {
				return 0;
			}
			
			double length = 0;
			foreach (List<Point2D> vs in this.vertices) {
				for (int i = 1; i < vs.Count; i++) {
					length += (vs[i - 1] - vs[i]).GetLength();
				}
			}

			length = length / measure;
			length -= (this.vertices.Count + 1) * KlimaFlaechenModul.CONNECTION_DISTANCE;
			if (length < 0) {
				length = 0;
			}
			return length;
		}

		public int GetRequiredWinkel(double measure) {
			int result = 0;
			if (this.vertices != null) {
				foreach (List<Point2D> vs in this.vertices) {
					if (vs.Count > 2) {
						Vector2D lastVector = vs[1] - vs[0];
						Vector2D curVector;
						double angle;
						for (int i = 1; i < vs.Count - 1; i++) {
							Point2D center = vs[i];
							bool addWinkel = true;
							foreach (List<Point2D> vs2 in this.vertices) {
								if (vs2 != vs && vs2.Count > 0) {
									double startDist = (vs2[0] - center).GetLength() / measure;
									double endDist = (vs2[vs2.Count - 1] - center).GetLength() / measure;
									if (startDist < 0.01 || endDist < 0.01) {
										addWinkel = false;
										break;
									}
								}
							}
							curVector = vs[i + 1] - vs[i];
							if (addWinkel) {
								angle = (Math.Atan2(curVector.Y, curVector.X) - Math.Atan2(-lastVector.Y, -lastVector.X)) * 180.0 / Math.PI;
								if (angle < 0) {
									angle += 360.0;
								}
								if (angle > 180.0) {
									angle = 360.0 - angle;
								}
								if (angle <= 157.5) {
									result++;
								}
							}
							lastVector = curVector;
						}
					}
				}
			}
			return result;
		}

		public int GetRequiredTStuecke() {
			return this.vertices.Count - 1;
		}
		#endregion Graphical Materials
	}
}