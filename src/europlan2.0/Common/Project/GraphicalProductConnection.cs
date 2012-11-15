using System;
using System.Collections.Generic;
using System.Text;
using WW.Math;
using System.Drawing;
using System.Xml.Serialization;
using WW.Math.Geometry;
using System.Windows.Forms;
using WW.Cad.Model.Tables;
using WW.Cad.Model.Entities;

namespace Europlan.Common {
	[XmlInclude(typeof(GraphicalProductToProductConnection))]
	public class GraphicalProductConnection {


		private PlannedProduct product;
		private string productGuid = null;
		private Distributor distributor;
		private string distributorId;
		private List<Point2D> vertices;
		private bool firstCircuit;
		private bool otherCircuits;
		private int distributorStartIndex;
		private bool vorlauf = true;
		private bool ruecklauf = true;
		private Product.ProductType connectionType;
		private bool finishedConnection = true;
		private bool automatic = false;

		private List<List<Point2D>> vorlaufVerticesForDrawing = null;
		private List<List<Point2D>> ruecklaufVerticesForDrawing = null;

		private PlannedProduct connectedProduct;
		private string connectedProductGuid = null;
		private int productConnectedAtSegment = -1;
		private double productConnectedPoint = -1;
		private bool productConnectedVorlaufseitig = true;
		private bool productConnectionRight = true;

		public virtual List<Point2D> Vertices {
			get { return vertices; }
			set { vertices = value; }
		}

		[XmlIgnore]
		public virtual bool FinishedConnection {
			get { return this.finishedConnection; }
			set { this.finishedConnection = value; }
		}

		internal GraphicalProductConnection() {
			connectionType = Europlan.Common.Product.ProductType.REST;
		}

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
			this.distributorStartIndex = distributorStartIndex;
			this.vorlauf = vorlauf;
			this.ruecklauf = ruecklauf;
			this.connectionType = connectionType;
		}

		public virtual double ConnectionWidth {
			get { return this.NrOfCircuits * 0.05 * 2 - 0.029; }
		}

		public virtual double ConnectionDistributorWidth {
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
				Line2D curLineVl = new Line2D(), curLineRl = new Line2D();
				Nullable<Line2D> lastLineVl = null, lastLineRl = null;
				Point2D startPoint, endPoint;
				Vector2D curVector = new Vector2D();
				Vector2D moveVector = new Vector2D();
				for (int j = 0; j < this.vertices.Count; j++) {
					if (j < this.vertices.Count - 1) {
						startPoint = this.vertices[j];
						endPoint = this.vertices[j + 1];
						curVector = endPoint - startPoint;
						curVector.Normalize();
						moveVector = new Vector2D(curVector.Y, -curVector.X);
						curLineVl = new Line2D(startPoint + moveVector * ((j < this.vertices.Count - 2 ? distVl : distVlFirst) * measure * factor), curVector);
						curLineRl = new Line2D(startPoint + moveVector * ((j < this.vertices.Count - 2 ? distRl : distRlFirst) * measure * factor), curVector);
					}
					if (lastLineVl == null) {
						vl.Add(curLineVl.Origin);
					} else {
						Nullable<Point2D> curPoint;
						if (j < this.vertices.Count - 1) {
							curPoint = Line2D.GetIntersection(curLineVl, lastLineVl.Value);
						} else {
							curPoint = this.vertices[j] + moveVector * (distVlFirst * measure * factor);
						}
						if (curPoint.HasValue) {
							if (this.ConnectedProduct != null && j == this.ProductConnectedAtSegment + 1) {
								if (this.ProductConnectedVorlaufseitig && i < this.ConnectedProduct.Product.PlannedCircuitCount) {
									Vector2D lastVector = lastLineVl.Value.Direction;
									Vector2D lastMoveVector = new Vector2D(lastVector.Y, -lastVector.X);
									int rightFactor = -1;
									if (this.productConnectionRight) {
										if (lastVector.X > 0) {
											rightFactor = 1;
										} else if (lastVector.X == 0 && lastVector.Y >= 0) {
											rightFactor = 1;
										}
									} else {
										if (lastVector.X < 0) {
											rightFactor = 1;
										} else if (lastVector.X == 0 && lastVector.Y <= 0) {
											rightFactor = 1;
										}
									}
									Point2D tmp = this.vertices[j - 1];
									List<Point2D> tmpRl = new List<Point2D>();

									double distVlTmp = 0.05 * ((2 * this.NrOfCircuits - 1) / 2.0 - (this.NrOfCircuits - 1) * 2) - 0.1;
									double distVlFirstTmp = finishedConnection ? 0.055 / 2 * ((2 * this.NrOfCircuits - 1) / 2.0 - (this.NrOfCircuits - 1) * 2) - 1 : distVlTmp;

									Point2D connPoint1 = tmp + (lastMoveVector * ((j < this.vertices.Count - 1 ? distVl : distVlFirst) * measure * factor)) + (lastVector * (this.productConnectedPoint + (0.1 * i - 0.025) * measure));
									Point2D connPoint0 = connPoint1 - lastVector * (0.015 * measure);
									tmpRl.Add(connPoint0);
									tmpRl.Add(connPoint1);
									vl.Add(connPoint1);
									Point2D connPoint2 = tmp + (lastMoveVector * ((j < this.vertices.Count - 1 ? distVlTmp : distVlFirstTmp) * measure * factor * rightFactor)) + (lastVector * (this.productConnectedPoint + (0.1 * i - 0.025) * measure));
									tmpRl.Add(connPoint2);
									this.ruecklaufVerticesForDrawing.Add(tmpRl);
									this.vorlaufVerticesForDrawing.Add(vl);
									vl = new List<Point2D>();
									Point2D connPoint3 = connPoint2 + (lastVector * (0.05 * measure));
									vl.Add(connPoint3);
									Point2D connPoint4 = connPoint1 + (lastVector * (0.05 * measure));
									vl.Add(connPoint4);
								}
							}
							vl.Add(curPoint.Value);
						}
					}
					if (lastLineRl == null) {
						rl.Add(curLineRl.Origin);
					} else {
						Nullable<Point2D> curPoint;
						if (j < this.vertices.Count - 1) {
							curPoint = Line2D.GetIntersection(curLineRl, lastLineRl.Value);
						} else {
							curPoint = this.vertices[j] + moveVector * (distRlFirst * measure * factor);
						}
						if (curPoint.HasValue) {
							if (this.ConnectedProduct != null && j == this.ProductConnectedAtSegment + 1) {
								if (!this.ProductConnectedVorlaufseitig && i < this.ConnectedProduct.Product.PlannedCircuitCount) {
									Vector2D lastVector = lastLineRl.Value.Direction;
									Vector2D lastMoveVector = new Vector2D(lastVector.Y, -lastVector.X);
									int rightFactor = -1;
									if (this.productConnectionRight) {
										if (lastVector.X > 0) {
											rightFactor = 1;
										} else if (lastVector.X == 0 && lastVector.Y >= 0) {
											rightFactor = 1;
										}
									} else {
										if (lastVector.X < 0) {
											rightFactor = 1;
										} else if (lastVector.X == 0 && lastVector.Y <= 0) {
											rightFactor = 1;
										}
									}
									Point2D tmp = this.vertices[j - 1];
									List<Point2D> tmpVl = new List<Point2D>();

									double distRlTmp = 0.05 * ((2 * this.NrOfCircuits - 1) / 2.0 - (this.NrOfCircuits - 1) * 2) - 0.1;
									double distRlFirstTmp = finishedConnection ? 0.055 / 2 * ((2 * this.NrOfCircuits - 1) / 2.0 - (this.NrOfCircuits - 1) * 2) : distRlTmp;


									Point2D connPoint1 = tmp + (lastMoveVector * ((j < this.vertices.Count - 1 ? distRl : distRlFirst) * measure * factor)) + (lastVector * (this.productConnectedPoint + (0.1 * i - 0.025) * measure));
									Point2D connPoint0 = connPoint1 - lastVector * (0.015 * measure);
									tmpVl.Add(connPoint0);
									tmpVl.Add(connPoint1);
									rl.Add(connPoint1);
									Point2D connPoint2 = tmp + (lastMoveVector * ((j < this.vertices.Count - 1 ? distRlTmp : distRlFirstTmp) * measure * factor * rightFactor)) + (lastVector * (this.productConnectedPoint + (0.1 * i - 0.025) * measure));
									tmpVl.Add(connPoint2);
									this.vorlaufVerticesForDrawing.Add(tmpVl);
									this.ruecklaufVerticesForDrawing.Add(rl);
									rl = new List<Point2D>();
									Point2D connPoint3 = connPoint2 + (lastVector * (0.05 * measure));
									rl.Add(connPoint3);
									Point2D connPoint4 = connPoint1 + (lastVector * (0.05 * measure));
									rl.Add(connPoint4);
								}
							}
							rl.Add(curPoint.Value);
						}
					}
					lastLineVl = curLineVl;
					lastLineRl = curLineRl;
				}
				this.vorlaufVerticesForDrawing.Add(vl);
				this.ruecklaufVerticesForDrawing.Add(rl);
			}
		}

		private double CalculateFactor() {
			return 1;
		}

		public virtual void ResetCachedVerticesForDrawing() {
			this.vorlaufVerticesForDrawing = null;
			this.ruecklaufVerticesForDrawing = null;
		}

		public virtual void Draw(Graphics g, Matrix4D additionalTransformation, double measure, bool selected, bool gray) {
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

		public virtual void DrawDxf(WW.Cad.Model.DxfModel model, DxfLayer connectionLayer, double measure) {
			if (this.vertices.Count < 2) {
				return;
			}

			if (this.vorlaufVerticesForDrawing == null || this.ruecklaufVerticesForDrawing == null) {
				this.CalculateVerticesForDrawing(measure);
			}
			foreach (List<Point2D> singleConnection in this.vorlaufVerticesForDrawing) {
				this.DrawDxfSingleConnection(model, connectionLayer, singleConnection, Color.Red);
			}
			foreach (List<Point2D> singleConnection in this.ruecklaufVerticesForDrawing) {
				this.DrawDxfSingleConnection(model, connectionLayer, singleConnection, Color.Blue);
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
			if (singleConnection.Count > 2) {
				p.EndCap = System.Drawing.Drawing2D.LineCap.Round;
			}
			for (int i = 1; i < singleConnection.Count; i++) {
				newVertex2D = additionalTransformation.TransformTo2D(singleConnection[i]);
				newVertex = new PointF((float)newVertex2D.X, (float)newVertex2D.Y);
				if (i == 2) {
					p.StartCap = System.Drawing.Drawing2D.LineCap.Round;
				}
				if (i == singleConnection.Count - 1) {
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

		private void DrawDxfSingleConnection(WW.Cad.Model.DxfModel model, DxfLayer connectionLayer, List<Point2D> singleConnection, Color c) {
			if (singleConnection.Count < 2) {
				return;
			}
			Point2D oldVertex = singleConnection[0];
			Point2D newVertex;

			EntityColor ec = EntityColor.CreateFromRgb(c.ToArgb());

			for (int i = 1; i < singleConnection.Count; i++) {
				newVertex = singleConnection[i];

				DxfLine line = new DxfLine(ec, oldVertex, newVertex);
				line.Layer = connectionLayer;
				model.Entities.Add(line);

				oldVertex = newVertex;
			}
		}

		public virtual bool HitTest(Point2D planPoint, double measure) {
			return this.GetDistance(planPoint) <= measure * (this.NrOfCircuits * 0.05 - 0.01);
		}

		public virtual double GetDistance(Point2D planPoint) {
			int tmp;
			Point2D tmp2;
			return GetDistance(planPoint, out tmp, out tmp2);
		}

		private double GetDistance(Point2D planPoint, out int segmentId, out Point2D closestPoint) {
			Point2D oldVertex = new Point2D();
			bool first = true;
			double bestDist = double.MaxValue;
			segmentId = -1;
			closestPoint = new Point2D();
			int i = 0;
			foreach (Point2D newVertex in this.vertices) {
				if (first) {
					first = false;
				} else {
					Segment2D segment = new Segment2D(oldVertex, newVertex);
					double dist = segment.GetDistance(planPoint);
					if (dist <= bestDist) {
						segmentId = i;
						bestDist = dist;
						closestPoint = segment.GetClosestPoint(planPoint);
					}
					i++;
				}
				oldVertex = newVertex;
			}
			return bestDist;
		}

		[XmlIgnore]
		public virtual PlannedProduct Product {
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

		public virtual string ProductGuid {
			set { this.productGuid = value; }
			get { return (this.productGuid != null || this.product == null) ? this.productGuid : this.product.Id; }
		}

		[XmlIgnore]
		public virtual PlannedProduct ConnectedProduct {
			get {
				if (this.connectedProductGuid != null) {
					this.connectedProduct = null;
					foreach (Floor f in Project.Instance.Floors) {
						foreach (Room r in f.Rooms) {
							foreach (PlannedProduct pp in r.PlannedProducts) {
								if (pp.Id == this.connectedProductGuid) {
									this.connectedProduct = pp;
									this.connectedProductGuid = null;
									break;
								}
							}
							if (this.connectedProductGuid == null) {
								break;
							}
						}
						if (this.connectedProductGuid == null) {
							break;
						}
					}
					this.connectedProductGuid = null;
				}
				return this.connectedProduct;
			}
			set {
				this.connectedProduct = value;
				this.connectedProductGuid = null;
			}
		}

		public virtual string ConnectedProductGuid {
			set { this.connectedProductGuid = value; }
			get { return (this.connectedProductGuid != null || this.connectedProduct == null) ? this.connectedProductGuid : this.connectedProduct.Id; }
		}

		[XmlIgnore]
		public virtual Distributor Distributor {
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

		public virtual string DistributorId {
			get { return (this.distributorId != null && this.distributor == null) ? this.distributorId : this.distributor.Id; }
			set { this.distributorId = value; }
		}

		public virtual int DistributorStartIndex {
			get { return this.distributorStartIndex; }
			set { this.distributorStartIndex = value; }
		}

		public virtual bool Automatic {
			get { return this.automatic; }
			set { this.automatic = value; }
		}

		public virtual bool Vorlauf {
			get { return this.vorlauf; }
			set { this.vorlauf = value; }
		}

		public virtual bool Ruecklauf {
			get { return this.ruecklauf; }
			set { this.ruecklauf = value; }
		}

		public virtual bool FirstCircuit {
			get { return this.firstCircuit; }
			set { this.firstCircuit = value; }
		}

		public virtual bool OtherCircuits {
			get { return this.otherCircuits; }
			set { this.otherCircuits = value; }
		}

		public virtual int NrOfCircuits {
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

		public virtual Product.ProductType ConnectionType {
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

		public virtual double GetPartInsidePolygon(Polygon2D polygon) {
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

		public virtual double GetLength(double measure) {
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
			if (length < 0) {
				length = 0;
			}
			return length;
		}

		public virtual List<GraphicalConnectionAnchor> GetAnchors(double scale) {
			List<GraphicalConnectionAnchor> anchors = new List<GraphicalConnectionAnchor>();
			if (this.vertices == null || this.vertices.Count < 2) {
				return anchors;
			}
			Nullable<Point2D> prev = null;
			int i = 0;
			foreach (Point2D vertex in this.vertices) {
				if (prev.HasValue) {
					if (i > 0 && i < this.vertices.Count - 2 && i != this.productConnectedAtSegment && i != this.productConnectedAtSegment - 1) {
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

		public virtual bool StartDrag(GraphicalConnectionAnchor anchor, Point2D planPoint) {
			this.startDrag = planPoint;
			this.startVertices = new List<Point2D>(this.vertices);
			return false;
		}

		public virtual bool MoveDrag(GraphicalConnectionAnchor anchor, Point2D planPoint) {
			this.MoveSegment(anchor.SegmentId, planPoint - startDrag);
			return true;
		}

		public virtual bool EndDrag(GraphicalConnectionAnchor anchor, Point2D planPoint) {
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

			Point2D oldVertex1 = this.vertices[segmentId];
			Point2D oldVertex2 = this.vertices[segmentId + 1];

			if (newSegmentStart.HasValue && newSegmentEnd.HasValue) {
				this.vertices[segmentId] = newSegmentStart.Value;
				this.vertices[segmentId + 1] = newSegmentEnd.Value;
			}

			if (this.ConnectedProduct != null) {
				double segmentLength = new Segment2D(this.vertices[this.productConnectedAtSegment], this.vertices[this.productConnectedAtSegment + 1]).GetLength();
				if (segmentLength < this.productConnectedPoint) {
					this.vertices[segmentId] = oldVertex1;
					this.vertices[segmentId + 1] = oldVertex2;
				}
			}
		}

		private void Simplify() {
		}

		public virtual List<GraphicalConnectionAnbindungsPunkt> GetConnectedProductAnbindungsPunkte(double measure, bool input, int distributorIndex, List<int> ignoreDistributorIndices, bool newProductConnection) {
			if (this.ConnectedProduct == null || this.Vertices == null || this.Vertices.Count < 2) {
				return new List<GraphicalConnectionAnbindungsPunkt>();
			}

			List<GraphicalConnectionAnbindungsPunkt> result = new List<GraphicalConnectionAnbindungsPunkt>();
			double factor = CalculateFactor();
			for (int i = 0; i < this.ConnectedProduct.Product.PlannedCircuitCount; i++) {
				int j = this.ProductConnectedAtSegment + 1;
				Vector2D lastVector = this.vertices[j] - this.vertices[j - 1];
				lastVector.Normalize();
				Vector2D lastMoveVector = new Vector2D(lastVector.Y, -lastVector.X);
				int rightFactor = -1;
				if (this.productConnectionRight) {
					if (lastVector.X > 0) {
						rightFactor = 1;
					} else if (lastVector.X == 0 && lastVector.Y >= 0) {
						rightFactor = 1;
					}
				} else {
					if (lastVector.X < 0) {
						rightFactor = 1;
					} else if (lastVector.X == 0 && lastVector.Y <= 0) {
						rightFactor = 1;
					}
				}
				Point2D tmp = this.vertices[j - 1];

				double distVlTmp = 0.05 * ((2 * this.NrOfCircuits - 1) / 2.0 - (this.NrOfCircuits - 1) * 2) - 0.1;
				double distVlFirstTmp = finishedConnection ? 0.055 / 2 * ((2 * this.NrOfCircuits - 1) / 2.0 - (this.NrOfCircuits - 1) * 2) : distVlTmp - 0.1;

				Point2D connPoint1 = tmp + (lastMoveVector * ((j < this.vertices.Count - 1 ? distVlTmp : distVlFirstTmp) * measure * factor * rightFactor)) + (lastVector * (this.productConnectedPoint + (0.1 * i - 0.025) * measure));
				Point2D connPoint2 = connPoint1 + (lastVector * (0.05 * measure));

				if ((distributorIndex < 0 || this.distributorStartIndex + i == distributorIndex) &&
					(ignoreDistributorIndices == null || !ignoreDistributorIndices.Contains(this.distributorStartIndex + i))) {
					if (this.ProductConnectedVorlaufseitig == !input) {
						result.Add(new GraphicalConnectionAnbindungsPunkt(connPoint1, new Polygon2D(new Point2D[] { connPoint1 - lastVector * (0.025 * measure) - lastMoveVector * (0.05 * measure), connPoint1 + lastVector * (0.025 * measure) - lastMoveVector * (0.05 * measure), connPoint1 + lastVector * (0.025 * measure) + lastMoveVector * (0.05 * measure), connPoint1 - lastVector * (0.025 * measure) + lastMoveVector * (0.05 * measure) }), this.distributorStartIndex + i));
					} else {
						result.Add(new GraphicalConnectionAnbindungsPunkt(connPoint2, new Polygon2D(new Point2D[] { connPoint2 - lastVector * (0.025 * measure) - lastMoveVector * (0.05 * measure), connPoint2 + lastVector * (0.025 * measure) - lastMoveVector * (0.05 * measure), connPoint2 + lastVector * (0.025 * measure) + lastMoveVector * (0.05 * measure), connPoint2 - lastVector * (0.025 * measure) + lastMoveVector * (0.05 * measure) }), this.distributorStartIndex + i));
					}
				}
			}

			return result;
		}

		public virtual List<GraphicalConnectionAnbindungsPunkt> GetAnbindungsPunkte(double measure, bool input, int distributorIndex, List<int> ignoreDistributorIndices, bool newProductConnection) {
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
					pi1 = connectionPoint - v * ((this.NrOfCircuits) / 2.0 - i) * connectionWidth * 2;
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

		public virtual int ProductConnectedAtSegment {
			get { return this.productConnectedAtSegment; }
			set { this.productConnectedAtSegment = value; }
		}

		public virtual double ProductConnectedPoint {
			get { return this.productConnectedPoint; }
			set { this.productConnectedPoint = value; }
		}

		public virtual bool ProductConnectedVorlaufseitig {
			get { return this.productConnectedVorlaufseitig; }
			set { this.productConnectedVorlaufseitig = value; }
		}

		public virtual bool ProductConnectionRight {
			get { return this.productConnectionRight; }
			set { this.productConnectionRight = value; }
		}

		public virtual PossibleProductConnection GetPossibleProductConnection(float measure, bool invertYAxis, Point2D planPoint, Product productToUse, Floor floor, int circuitCount) {
			if (this.ConnectedProduct != null) {
				return null;
			}
			int segmentId;
			Point2D closestPoint;
			double dist = this.GetDistance(planPoint, out segmentId, out closestPoint);

			PossibleProductConnection possibleConnection = null;
			if (dist < 0.025 * measure) {
				double size = 0.05 * measure;
				Point2D leftBottom = new Point2D(closestPoint.X - size, closestPoint.Y - size);
				Point2D leftTop = new Point2D(closestPoint.X - size, closestPoint.Y + size);
				Point2D rightTop = new Point2D(closestPoint.X + size, closestPoint.Y + size);
				Point2D rightBottom = new Point2D(closestPoint.X + size, closestPoint.Y - size);
				possibleConnection = new PossibleProductConnection(closestPoint, new Polygon2D(new Point2D[] { leftBottom, leftTop, rightTop, rightBottom }), true, true, 0, this, segmentId, new Segment2D(closestPoint, this.vertices[segmentId]).GetLength());
			}
			return possibleConnection;
		}
	}

	public class GraphicalProductToProductConnection : GraphicalProductConnection {
		private PlannedProduct product;
		private string productGuid = null;

		private PlannedProduct connectedToProduct;
		private string connectedToProductGuid = null;

		public override List<Point2D> Vertices {
			get { return null; }
			set { }
		}

		[XmlIgnore]
		public override bool FinishedConnection {
			get { return true; }
			set { }
		}

		internal GraphicalProductToProductConnection() {
		}

		public GraphicalProductToProductConnection(PlannedProduct product, PlannedProduct connectedToProduct) {
			this.product = product;
			this.connectedToProduct = connectedToProduct;
		}

		public override double ConnectionDistributorWidth {
			get { return this.NrOfCircuits * 0.05 * 2 - 0.029; }
		}

		public override void ResetCachedVerticesForDrawing() {
		}

		public override void Draw(Graphics g, Matrix4D additionalTransformation, double measure, bool selected, bool gray) {
		}

		public override void DrawDxf(WW.Cad.Model.DxfModel model, DxfLayer connectionLayer, double measure) {
		}

		public override bool HitTest(Point2D planPoint, double measure) {
			return false;
		}

		public override double GetDistance(Point2D planPoint) {
			return double.MaxValue;
		}

		[XmlIgnore]
		public override PlannedProduct Product {
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

		public override string ProductGuid {
			set { this.productGuid = value; }
			get { return (this.productGuid != null || this.product == null) ? this.productGuid : this.product.Id; }
		}

		[XmlIgnore]
		public PlannedProduct ConnectedToProduct {
			get {
				if (this.connectedToProductGuid != null) {
					this.connectedToProduct = null;
					foreach (Floor f in Project.Instance.Floors) {
						foreach (Room r in f.Rooms) {
							foreach (PlannedProduct pp in r.PlannedProducts) {
								if (pp.Id == this.connectedToProductGuid) {
									this.connectedToProduct = pp;
									this.connectedToProductGuid = null;
									break;
								}
							}
							if (this.connectedToProductGuid == null) {
								break;
							}
						}
						if (this.connectedToProductGuid == null) {
							break;
						}
					}
					this.connectedToProductGuid = null;
				}
				return this.connectedToProduct;
			}
			set {
				this.connectedToProduct = value;
				this.connectedToProductGuid = null;
			}
		}

		public string ConnectedToProductGuid {
			set { this.connectedToProductGuid = value; }
			get { return (this.connectedToProductGuid != null || this.connectedToProduct == null) ? this.connectedToProductGuid : this.connectedToProduct.Id; }
		}

		[XmlIgnore]
		public override Distributor Distributor {
			get { return null; }
			set { }
		}

		[XmlIgnore]
		public override string DistributorId {
			get { return null; }
			set { }
		}

		[XmlIgnore]
		public override int DistributorStartIndex {
			get { return -1; }
			set { }
		}

		[XmlIgnore]
		public override bool Automatic {
			get { return false; }
			set { }
		}

		[XmlIgnore]
		public override bool Vorlauf {
			get { return true; }
			set { }
		}

		[XmlIgnore]
		public override bool Ruecklauf {
			get { return true; }
			set { }
		}

		[XmlIgnore]
		public override bool FirstCircuit {
			get { return true; }
			set { }
		}

		[XmlIgnore]
		public override bool OtherCircuits {
			get { return true; }
			set { }
		}

		[XmlIgnore]
		public override int NrOfCircuits {
			get { return this.Product.Product.PlannedCircuitCount; }
		}

		[XmlIgnore]
		public override Product.ProductType ConnectionType {
			get {
				if (this.ConnectedToProduct != null && this.ConnectedToProduct.Product.Connections != null && this.ConnectedToProduct.Product.Connections.Count > 0) {
					return this.ConnectedToProduct.Product.Connections[0].ConnectionType;
				}
				return Europlan.Common.Product.ProductType.FBH;
			}
			set { }
		}

		public override double GetPartInsidePolygon(Polygon2D polygon) {
			return 0;
		}

		public override double GetLength(double measure) {
			return 0;
		}

		public override List<GraphicalConnectionAnchor> GetAnchors(double scale) {
			return new List<GraphicalConnectionAnchor>();
		}

		public override bool StartDrag(GraphicalConnectionAnchor anchor, Point2D planPoint) {
			return false;
		}

		public override bool MoveDrag(GraphicalConnectionAnchor anchor, Point2D planPoint) {
			return false;
		}

		public override bool EndDrag(GraphicalConnectionAnchor anchor, Point2D planPoint) {
			return false;
		}

		public override List<GraphicalConnectionAnbindungsPunkt> GetAnbindungsPunkte(double measure, bool input, int distributorIndex, List<int> ignoreDistributorIndices, bool newProductConnection) {
			if (this.ConnectedToProduct.Product.Connections != null && this.ConnectedToProduct.Product.Connections.Count > 0) {
				return this.ConnectedToProduct.Product.Connections[0].GetConnectedProductAnbindungsPunkte(measure, input, distributorIndex, ignoreDistributorIndices, newProductConnection);
			}
			return new List<GraphicalConnectionAnbindungsPunkt>();
		}

		public override List<GraphicalConnectionAnbindungsPunkt> GetConnectedProductAnbindungsPunkte(double measure, bool input, int distributorIndex, List<int> ignoreDistributorIndices, bool newProductConnection) {
			return new List<GraphicalConnectionAnbindungsPunkt>();
		}

		[XmlIgnore]
		public override int ProductConnectedAtSegment {
			get { return -1; }
			set { }
		}

		[XmlIgnore]
		public override double ProductConnectedPoint {
			get { return -1; }
			set { }
		}

		[XmlIgnore]
		public override bool ProductConnectedVorlaufseitig {
			get { return true; }
			set { }
		}

		public override PossibleProductConnection GetPossibleProductConnection(float measure, bool invertYAxis, Point2D planPoint, Product productToUse, Floor floor, int circuitCount) {
			return null;
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
		protected Segment2D segment = new Segment2D();
		protected GraphicalProductConnection connection = null;
		protected int segmentId = 0;

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
