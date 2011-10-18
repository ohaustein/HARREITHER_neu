using System;
using System.Collections.Generic;
using System.Text;
using WW.Math;
using System.Drawing;
using System.Xml.Serialization;
using WW.Math.Geometry;
using System.Drawing.Drawing2D;

namespace Europlan.Common {
	[XmlInclude(typeof(GraphicalHithermVerbindung))]
	[XmlInclude(typeof(GraphicalHithermCompactVerbindung))]
	public abstract class GraphicalWallVerbindung : IGraphicalWallObject {

		public abstract List<Point2D> Vertices {
		  get;
		  set;
		}

		public abstract void InitializeVertices(IEnumerable<Point2D> vertices);

		public abstract bool IsMoveable {
			get;
		}

		[XmlIgnore]
		public abstract bool HasStart {
			get;
		}

		[XmlIgnore]
		public abstract bool HasEnd {
			get;
		}

		[XmlIgnore]
		public abstract int HkId {
			get;
		}

		public abstract void PaintObject(Graphics g, Color c, bool error, double scale, bool export);

		public abstract bool HitTest(Point2D planPoint, double maxDist);

		public abstract double GetDistance(Point2D planPoint);

		public abstract int StartIndex {
			get;
			set;
		}

		public abstract int EndIndex {
			get;
			set;
		}

		public abstract int CircuitIndex {
			get;
			set;
		}

		[XmlIgnore]
		public abstract PlannedProduct Product {
			get;
		}

		public abstract string ProductGuid {
			set;
			get;
		}

		public abstract double GetLength();

		#region IGraphicalWallObject Members
		public abstract bool HitTest(Point2D planPoint, double xOffset, double yOffset);

		public abstract void PaintObject(Graphics g, double xOffset, double yOffset, IGraphicalWallObject selectedObject, double scale, bool export);

		public abstract IGraphicalWallObject GetPickedObject(Point2D planPoint, double xOffset, double yOffset);

		public abstract List<Polygon2D> GetObjectBorders(double xOffset, double yOffset);

		public abstract bool CollisionTest(IList<Polygon2D> polygon, double xOffset, double yOffset, bool ignoreBorders);

		public abstract List<Anchor> GetAnchors(double scale);

		public abstract bool CheckValidity(GraphicalWall owningWall, double offsetX, double offsetY);

		public abstract bool StartDrag(Anchor anchor, Point2D planPoint, GraphicalWall owningWall, Room owningRoom, Product owningProduct, bool useSnap);

		public abstract bool MoveDrag(Anchor anchor, Point2D planPoint, GraphicalWall owningWall, Room owningRoom, Product owningProduct, bool useSnap);

		public abstract bool EndDrag(Anchor anchor, Point2D planPoint, GraphicalWall owningWall, Room owningRoom, Product owningProduct, bool useSnap);
		#endregion

		public abstract void MoveVerticalSegment(int segmentIndex, double delta);

		public abstract void MoveHorizontalSegment(int segmentIndex, double delta);

		public abstract void Simplify();

		[XmlIgnore]
		public abstract bool Finished {
			get;
			set;
		}

		[XmlIgnore]
		public abstract bool Error {
			get;
			set;
		}

		public abstract void BackupState();

		public abstract void RevertState();

		[XmlIgnore]
		public abstract bool IsNew {
			get;
			set;
		}

		public abstract bool SnapToHelplines(List<double> helplines, bool snapTop, bool snapBottom);
	}

	public abstract class GenericGraphicalWallVerbindungImplementation<ProductType, CircuitType, RegisterType, WrapperType, VerbindungType> : GraphicalWallVerbindung
			where ProductType : Europlan.Common.Product, Europlan.Common.IWallProduct<CircuitType, RegisterType>
			where CircuitType : Europlan.Common.Circuit, Europlan.Common.IWallCircuit<CircuitType, VerbindungType, RegisterType>
			where RegisterType : Europlan.Common.IWallRegister
			where WrapperType : Europlan.Common.GraphicalRegisterWrapper, Europlan.Common.IWallRegisterWrapper<RegisterType>
			where VerbindungType: Europlan.Common.GraphicalWallVerbindung {

		protected RegisterType start;
		protected RegisterType end;
		protected CircuitType circuit;

		protected List<Point2D> vertices;
		protected int startIndex = -1;
		protected int endIndex = -1;
		protected int circuitIndex = -1;
		protected PlannedProduct product;
		protected string productGuid = null;
		protected bool finished = true;
		protected bool error = false;
		protected bool isPartOfCompound = false;

		internal GenericGraphicalWallVerbindungImplementation() {
			this.vertices = new List<Point2D>();
		}

		internal GenericGraphicalWallVerbindungImplementation(bool finished) {
			this.vertices = new List<Point2D>();
			this.finished = finished;
		}

		public GenericGraphicalWallVerbindungImplementation(RegisterType start, RegisterType end, IEnumerable<Point2D> vertices, CircuitType circuit, PlannedProduct product) {
			this.start = start;
			this.end = end;
			this.InitializeVertices(vertices);
			this.circuit = circuit;
			this.product = product;
		}

		public virtual void FinalizeLoading() {
			PlannedProduct tmpProduct = this.Product;
			CircuitType tmpCircuit = this.Circuit;
			RegisterType tmpRegister = this.End;
			tmpRegister = this.Start;
		}

		protected const double WIDTH = 2.0;

		public override List<Point2D> Vertices {
		  get { return this.vertices; }
		  set { this.vertices = value; }
		}

		public override void InitializeVertices(IEnumerable<Point2D> vertices) {
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
		}

		public override bool IsMoveable {
			get { return false; }
		}

		protected const double arrowWidth = 5.0;
		protected const double arrowHeight = arrowWidth * 1.118;

		public override void PaintObject(Graphics g, Color c, bool error, double scale, bool export) {
			PointF oldVertex = PointF.Empty;
			PointF newVertex;
			bool first = true;
			Pen p = new Pen(c, 2);
			if (error || this.error) {
				//p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
				p = new Pen(new HatchBrush(HatchStyle.DarkDownwardDiagonal, c, Color.Transparent));
			}
			p.EndCap = System.Drawing.Drawing2D.LineCap.Round;
			foreach (Point2D vertex in this.Vertices) {
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
			if (!isPartOfCompound && finished && this.Vertices != null && this.Vertices.Count > 0) {
				if (!this.HasStart && this.Vertices[0].Y == 0) {
					PointF[] arrow = new PointF[3];
					arrow[0] = new PointF((float)(this.Vertices[0].X - arrowWidth / 2.0), (float)(arrowHeight / 2.0));
					arrow[1] = new PointF((float)(this.Vertices[0].X + arrowWidth / 2.0), (float)(arrowHeight / 2.0));
					arrow[2] = new PointF((float)(this.Vertices[0].X), (float)(-arrowHeight / 2.0));
					g.FillPolygon(Brushes.Red, arrow);
					g.DrawPolygon(new Pen(Color.DarkRed, (float)(1 / scale)), arrow);

					if (this.HkId >= 0) {
						Font font = new Font("Arial", (float)(10.0 / scale));
						string label = this.HkId.ToString();
						SizeF size = g.MeasureString(label, font);
						Matrix oldTransform = g.Transform;
						Matrix textTransform = oldTransform.Clone();
						float x = (float)(this.Vertices[0].X - size.Width / 2.0);
						float y = (float)(-arrowHeight * 0.75);
						textTransform.Translate(0, -y);
						textTransform.Scale(1, -1);
						textTransform.Translate(0, -y);
						g.Transform = textTransform;
						g.DrawString(label, font, Brushes.DarkRed, x, -y);
						g.Transform = oldTransform;
					}
				} else if (!this.HasEnd && this.Vertices[this.Vertices.Count - 1].Y == 0) {
					PointF[] arrow = new PointF[3];
					arrow[0] = new PointF((float)(this.Vertices[this.Vertices.Count - 1].X - arrowWidth / 2.0), (float)(arrowHeight / 2.0));
					arrow[1] = new PointF((float)(this.Vertices[this.Vertices.Count - 1].X + arrowWidth / 2.0), (float)(arrowHeight / 2.0));
					arrow[2] = new PointF((float)(this.Vertices[this.Vertices.Count - 1].X), (float)(-arrowHeight / 2.0));
					g.FillPolygon(Brushes.Blue, arrow);
					g.DrawPolygon(new Pen(Color.DarkBlue, (float)(1 / scale)), arrow);

					if (this.HkId >= 0) {
						Font font = new Font("Arial", (float)(10.0 / scale));
						string label = this.HkId.ToString();
						SizeF size = g.MeasureString(label, font);
						Matrix oldTransform = g.Transform;
						Matrix textTransform = oldTransform.Clone();
						float x = (float)(this.Vertices[this.Vertices.Count - 1].X - size.Width / 2.0);
						float y = (float)(-arrowHeight * 0.75);
						textTransform.Translate(0, -y);
						textTransform.Scale(1, -1);
						textTransform.Translate(0, -y);
						g.Transform = textTransform;
						g.DrawString(label, font, Brushes.DarkBlue, x, -y);
						g.Transform = oldTransform;
					}
				}
			}
			// TODO
		}

		public override bool HitTest(Point2D planPoint, double maxDist) {
			return this.GetDistance(planPoint) <= maxDist;
		}

		public override double GetDistance(Point2D planPoint) {
			Point2D oldVertex = new Point2D();
			bool first = false;
			double bestDist = double.MaxValue;
			foreach (Point2D newVertex in this.Vertices) {
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

		public override double GetLength() {
			double length = 0;
			if (this.Vertices.Count > 1) {
				for (int i = 1; i < this.Vertices.Count; i++) {
					length += (this.Vertices[i - 1] - this.Vertices[i]).GetLength();
				}
			}
			if (length < 0) {
				length = 0;
			}
			return length;
		}

		#region IGraphicalWallObject Members
		public override bool HitTest(Point2D planPoint, double xOffset, double yOffset) {
			return this.HitTest(planPoint, 2);
		}

		public override void PaintObject(Graphics g, double xOffset, double yOffset, IGraphicalWallObject selectedObject, double scale, bool export) {
			this.PaintObject(g, (this == selectedObject) ? Color.Red : Color.Black, false, scale, export);
		}

		public override IGraphicalWallObject GetPickedObject(Point2D planPoint, double xOffset, double yOffset) {
			if (HitTest(planPoint, xOffset, yOffset)) {
				return this;
			}
			return null;
		}

		public override List<Polygon2D> GetObjectBorders(double xOffset, double yOffset) {
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
			foreach (Point2D vertex in this.Vertices) {
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
			if (border.IsClockwise()) {
				border.Reverse();
			}
			return new List<Polygon2D>(new Polygon2D[] { border });
		}

		public override bool CollisionTest(IList<Polygon2D> polygon, double xOffset, double yOffset, bool ignoreBorders) {
			List<Polygon2D> linkBorders = this.GetObjectBorders(0, 0);
			/*if (linkBorders.IsClockwise()) {
				linkBorders.Reverse();
			}
			if (polygon.IsClockwise()) {
				polygon.Reverse();
			}
			List<Polygon2D> list1 = new List<Polygon2D>();
			list1.Add(polygon);
			List<Polygon2D> list2 = new List<Polygon2D>();
			list2.Add(linkBorders);*/
			try {
				return Polygon2D.GetIntersection(polygon, linkBorders).Count > 0;
			} catch {
				return false;
			}
		}

		public override List<Anchor> GetAnchors(double scale) {
			List<Anchor> anchors = new List<Anchor>();
			if (this.vertices == null || this.vertices.Count < 2) {
				return anchors;
			}
			Nullable<Point2D> prev = null;
			bool lastHorizontal = this.vertices[0].Y != 0;
			int i = 0;
			foreach (Point2D vertex in this.vertices) {
				if (prev.HasValue) {
					AnchorTypeEnum type = AnchorTypeEnum.ANCHOR_NONE;
					if (prev.Value == vertex) {
						type = lastHorizontal ? AnchorTypeEnum.ANCHOR_MOVE_LEFT_RIGHT : AnchorTypeEnum.ANCHOR_MOVE_UP_DOWN;
					} else if (Math.Abs(prev.Value.X - vertex.X) < 0.00001) {
						type = AnchorTypeEnum.ANCHOR_MOVE_LEFT_RIGHT;
					} else if (Math.Abs(prev.Value.Y - vertex.Y) < 0.00001) {
						type = AnchorTypeEnum.ANCHOR_MOVE_UP_DOWN;
					} else {
						// TODO
					}
					if (type != AnchorTypeEnum.ANCHOR_NONE && (i > 0 || this.Start == null) && (i < this.vertices.Count - 2 || this.End == null)) {
						anchors.Add(new InvisibleSegmentAnchor(prev.Value, vertex, i, 4.0, type, this));
					}
					i++;
				}
				prev = vertex;
			}
			return anchors;
		}

		public override bool CheckValidity(GraphicalWall owningWall, double offsetX, double offsetY) {
			Room room = this.Product.Product.AssociatedRoom;
			List<Polygon2D> linkBorders = this.GetObjectBorders(offsetX, offsetY);
			if (room.CollisionTest(linkBorders)) {
				return false;
			}
			foreach (GraphicalWall baseWall in room.Walls) {
				GraphicalWall wall = baseWall;
				while (wall != null) {
					Nullable<Vector2D> offset = room.GetWallOffset(wall) * 100;
					if (!offset.HasValue) {
						offset = new Vector2D(0, 0);
					}
					foreach (GraphicalRegisterWrapper register in wall.Registers) {
						if (!register.Error && register.CollisionTest(linkBorders, offset.Value.X, offset.Value.Y, true)) {
							return false;
						}
					}
					foreach (GraphicalWallObstacle obstacle in wall.Obstacles) {
						if (!obstacle.Error && obstacle.CollisionTest(linkBorders, offset.Value.X, offset.Value.Y, false)) {
							return false;
						}
					}
					wall = wall.DachSchraege;
				}
			}
			if (this.Product.Product is HithermProduct) {
				foreach (HithermCircuit hc in this.Product.Product.PlannedCircuits) {
					foreach (GraphicalHithermVerbindung link in hc.Links) {
						if (!link.Error && !link.EqualsOrIsPart(this) && link.CollisionTest(linkBorders, offsetX, offsetY, true)) {
							return false;
						}
					}
				}
			} else if (this.Product.Product is HithermCompactProduct) {
				foreach (HithermCompactCircuit hc in this.Product.Product.PlannedCircuits) {
					foreach (GraphicalHithermCompactVerbindung link in hc.Links) {
						if (!link.Error && !link.EqualsOrIsPart(this) && link.CollisionTest(linkBorders, offsetX, offsetY, true)) {
							return false;
						}
					}
				}
			} else {
				throw new Exception("Dieses Produkt wird noch nicht unterstützt");
			}
			return true;
		}

		public virtual bool EqualsOrIsPart(object obj) {
			return this.Equals(obj);
		}

		private Nullable<Point2D> startDrag = null;
		private List<Point2D> startVertices;

		public override bool StartDrag(Anchor anchor, Point2D planPoint, GraphicalWall owningWall, Room owningRoom, Product owningProduct, bool useSnap) {
			this.startDrag = planPoint;
			this.startVertices = new List<Point2D>(this.vertices);
			// nothing to do here as the verbindung doesn't have any anchors
			return false;
		}

		public override bool MoveDrag(Anchor anchor, Point2D planPoint, GraphicalWall owningWall, Room owningRoom, Product owningProduct, bool useSnap) {
			// nothing to do here as the verbindung doesn't have any anchors
			if (anchor != null && anchor is InvisibleSegmentAnchor) {
				List<Point2D> oldVertices = this.vertices;
				this.vertices = new List<Point2D>(this.startVertices);
				bool moved = false;
				if (anchor.AnchorType == AnchorTypeEnum.ANCHOR_MOVE_LEFT_RIGHT) {
					this.MoveVerticalSegment((anchor as InvisibleSegmentAnchor).SegmentIndex, planPoint.X - startDrag.Value.X);
					moved = true;
				} else if (anchor.AnchorType == AnchorTypeEnum.ANCHOR_MOVE_UP_DOWN) {
					this.MoveHorizontalSegment((anchor as InvisibleSegmentAnchor).SegmentIndex, planPoint.Y - startDrag.Value.Y);
					moved = true;
				}
				if (moved) {
					if (!this.CheckValidity(owningWall, 0, 0)) {
						this.vertices = oldVertices;
						return false;
					}
					return true;
				}
			}
			return false;
		}

		public override bool EndDrag(Anchor anchor, Point2D planPoint, GraphicalWall owningWall, Room owningRoom, Product owningProduct, bool useSnap) {
			// nothing to do here as the verbindung doesn't have any anchors
			this.Simplify();
			return true;
		}
		#endregion

		public virtual void UpdateStartPoint(WrapperType register, GraphicalWall owningWall, bool checkValidity) {
			if (this.vertices == null || this.vertices.Count < 2) {
				return;
			}
			bool vertexAdded = false;
			if (this.vertices.Count == 2) {
				this.vertices.Insert(0, new Point2D(this.vertices[0]));
				vertexAdded = true;
			}
			Nullable<Vector2D> offset = register.AssociatedRoom.GetWallOffset(owningWall) * 100;
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
				//this.vertices.Insert(0, new Point2D(this.vertices[0]));
				firstPoint = this.vertices[0];
				secondPoint = this.vertices[1];
				Point2D thirdPoint = this.vertices[2];
				bool left = secondPoint.X < thirdPoint.X;
				this.MoveVerticalSegment(0, -firstPoint.X + newStartPoint.X);
				firstPoint = this.vertices[0];
				secondPoint = this.vertices[1];
				thirdPoint = this.vertices[2];
				if ((register.Register.GraphVorlaufRight && newStartPoint5.Y > secondPoint.Y) ||
					(!register.Register.GraphVorlaufRight && newStartPoint5.Y > secondPoint.Y)) {
					double tmpX;
					if (left && register.Register.GraphVorlaufRight) {
						tmpX = secondPoint.X + register.Width + 10;
					} else if (!left && register.Register.GraphVorlaufRight) {
						tmpX = secondPoint.X - 10;
					} else if (left && !register.Register.GraphVorlaufRight) {
						tmpX = secondPoint.X + 10;
					} else {
						tmpX = secondPoint.X - register.Width - 10;
					}
					this.vertices.RemoveAt(1);
					this.vertices.Insert(1, new Point2D(tmpX, secondPoint.Y));
					this.vertices.Insert(1, new Point2D(tmpX, newStartPoint5.Y));
					this.vertices.Insert(1, new Point2D(newStartPoint5.X, newStartPoint5.Y));
					this.vertices[0] = newStartPoint;
				} else {
					this.vertices[0] = newStartPoint;
				}
				moved = true;
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
					if (register is GraphicalHithermRegisterWrapper) {
						if (upside) {
							tmpY = secondPoint.Y + 10;
						} else {
							tmpY = secondPoint.Y - register.Height - 10;
						}
					} else if (register is GraphicalHithermCompactRegisterWrapper) {
						if (upside) {
							tmpY = secondPoint.Y + register.Height + 10;
						} else {
							tmpY = secondPoint.Y - 10;
						}
					} else {
						throw new Exception("Invalid Register Type");
					}
					this.vertices.RemoveAt(1);
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
			if (checkValidity) {
				if (!this.CheckValidity(owningWall, 0, 0)) {
					this.vertices.Clear();
				}
			}
		}

/*		public void UpdateStartPoint(GraphicalHithermCompactRegisterWrapper register, GraphicalWall owningWall, bool checkValidity) {
#if BLUB
			if (this.vertices == null || this.vertices.Count < 2) {
				return;
			}
			bool vertexAdded = false;
			if (this.vertices.Count == 2) {
				this.vertices.Insert(0, new Point2D(this.vertices[0]));
				vertexAdded = true;
			}
			Nullable<Vector2D> offset = register.Product.AssociatedRoom.GetWallOffset(owningWall) * 100;
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
				//this.vertices.Insert(0, new Point2D(this.vertices[0]));
				firstPoint = this.vertices[0];
				secondPoint = this.vertices[1];
				Point2D thirdPoint = this.vertices[2];
				bool left = secondPoint.X < thirdPoint.X;
				this.MoveVerticalSegment(0, -firstPoint.X + newStartPoint.X);
				firstPoint = this.vertices[0];
				secondPoint = this.vertices[1];
				thirdPoint = this.vertices[2];
				if ((register.Register.GraphVorlaufRight && newStartPoint5.Y > secondPoint.Y) ||
					(!register.Register.GraphVorlaufRight && newStartPoint5.Y > secondPoint.Y)) {
					double tmpX;
					if (left && register.Register.GraphVorlaufRight) {
						tmpX = secondPoint.X + register.Width + 10;
					} else if (!left && register.Register.GraphVorlaufRight) {
						tmpX = secondPoint.X - 10;
					} else if (left && !register.Register.GraphVorlaufRight) {
						tmpX = secondPoint.X + 10;
					} else {
						tmpX = secondPoint.X - register.Width - 10;
					}
					this.vertices.RemoveAt(1);
					this.vertices.Insert(1, new Point2D(tmpX, secondPoint.Y));
					this.vertices.Insert(1, new Point2D(tmpX, newStartPoint5.Y));
					this.vertices.Insert(1, new Point2D(newStartPoint5.X, newStartPoint5.Y));
					this.vertices[0] = newStartPoint;
				} else {
					this.vertices[0] = newStartPoint;
				}
				moved = true;
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
			if (checkValidity) {
				if (!this.CheckValidity(owningWall, 0, 0)) {
					this.vertices.Clear();
				}
			}
#endif
		}*/

		public virtual void UpdateEndPoint(WrapperType register, GraphicalWall owningWall, bool checkValidity) {
			if (this.vertices == null || this.vertices.Count < 2) {
				return;
			}
			bool vertexAdded = false;
			if (this.vertices.Count == 2) {
				this.vertices.Insert(this.vertices.Count - 1, new Point2D(this.vertices[this.vertices.Count - 1]));
				vertexAdded = true;
			}
			Nullable<Vector2D> offset = register.AssociatedRoom.GetWallOffset(owningWall) * 100;

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
				//this.vertices.Insert(this.vertices.Count - 1, new Point2D(this.vertices[this.vertices.Count - 1]));
				lastPoint = this.vertices[this.vertices.Count - 1];
				prevLastPoint = this.vertices[this.vertices.Count - 2];
				Point2D prevPrevLastPoint = this.vertices[this.vertices.Count - 3];
				bool left = prevLastPoint.X < prevPrevLastPoint.X;
				this.MoveVerticalSegment(this.vertices.Count - 2, -lastPoint.X + newEndPoint.X);
				lastPoint = this.vertices[this.vertices.Count - 1];
				prevLastPoint = this.vertices[this.vertices.Count - 2];
				prevPrevLastPoint = this.vertices[this.vertices.Count - 3];
				if ((!register.Register.GraphVorlaufRight && newEndPoint5.Y < prevLastPoint.Y) ||
					(register.Register.GraphVorlaufRight && newEndPoint5.Y < prevLastPoint.Y)) {
					double tmpX;
					if (left && register.Register.GraphVorlaufRight) {
						tmpX = prevLastPoint.X + 10;
					} else if (!left && register.Register.GraphVorlaufRight) {
						tmpX = prevLastPoint.X - register.Width - 10;
					} else if (left && !register.Register.GraphVorlaufRight) {
						tmpX = prevLastPoint.X + register.Width + 10;
					} else {
						tmpX = prevLastPoint.X - 10;
					}
					this.vertices.RemoveAt(this.vertices.Count - 2);
					this.vertices.Insert(this.vertices.Count - 1, new Point2D(tmpX, prevLastPoint.Y));
					this.vertices.Insert(this.vertices.Count - 1, new Point2D(tmpX, newEndPoint5.Y));
					this.vertices.Insert(this.vertices.Count - 1, new Point2D(lastPoint.X, newEndPoint5.Y));
					this.vertices[this.vertices.Count - 1] = newEndPoint;
				} else {
					this.vertices[this.vertices.Count - 1] = newEndPoint;
				}
				moved = true;
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
			if (checkValidity) {
				if (!this.CheckValidity(owningWall, 0, 0)) {
					this.vertices.Clear();
				}
			}
		}

		/*public void UpdateEndPoint(GraphicalHithermCompactRegisterWrapper register, GraphicalWall owningWall, bool checkValidity) {
#if BLUB
			if (this.vertices == null || this.vertices.Count < 2) {
				return;
			}
			bool vertexAdded = false;
			if (this.vertices.Count == 2) {
				this.vertices.Insert(this.vertices.Count - 1, new Point2D(this.vertices[this.vertices.Count - 1]));
				vertexAdded = true;
			}
			Nullable<Vector2D> offset = register.Product.AssociatedRoom.GetWallOffset(owningWall) * 100;
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
				//this.vertices.Insert(this.vertices.Count - 1, new Point2D(this.vertices[this.vertices.Count - 1]));
				lastPoint = this.vertices[this.vertices.Count - 1];
				prevLastPoint = this.vertices[this.vertices.Count - 2];
				Point2D prevPrevLastPoint = this.vertices[this.vertices.Count - 3];
				bool left = prevLastPoint.X < prevPrevLastPoint.X;
				this.MoveVerticalSegment(this.vertices.Count - 2, -lastPoint.X + newEndPoint.X);
				lastPoint = this.vertices[this.vertices.Count - 1];
				prevLastPoint = this.vertices[this.vertices.Count - 2];
				prevPrevLastPoint = this.vertices[this.vertices.Count - 3];
				if ((!register.Register.GraphVorlaufRight && newEndPoint5.Y < prevLastPoint.Y) ||
					(register.Register.GraphVorlaufRight && newEndPoint5.Y < prevLastPoint.Y)) {
					double tmpX;
					if (left && register.Register.GraphVorlaufRight) {
						tmpX = prevLastPoint.X + 10;
					} else if (!left && register.Register.GraphVorlaufRight) {
						tmpX = prevLastPoint.X - register.Width - 10;
					} else if (left && !register.Register.GraphVorlaufRight) {
						tmpX = prevLastPoint.X + register.Width + 10;
					} else {
						tmpX = prevLastPoint.X - 10;
					}
					this.vertices.RemoveAt(this.vertices.Count - 2);
					this.vertices.Insert(this.vertices.Count - 1, new Point2D(tmpX, prevLastPoint.Y));
					this.vertices.Insert(this.vertices.Count - 1, new Point2D(tmpX, newEndPoint5.Y));
					this.vertices.Insert(this.vertices.Count - 1, new Point2D(lastPoint.X, newEndPoint5.Y));
					this.vertices[this.vertices.Count - 1] = newEndPoint;
				} else {
					this.vertices[this.vertices.Count - 1] = newEndPoint;
				}
				moved = true;
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
			if (checkValidity) {
				if (!this.CheckValidity(owningWall, 0, 0)) {
					this.vertices.Clear();
				}
			}
#endif
		}*/

		public override void MoveVerticalSegment(int segmentIndex, double delta) {
			if (this.vertices.Count < segmentIndex + 2) {
				return;
			}
			Vector2D deltaVector = new Vector2D(delta, 0);
			this.vertices[segmentIndex] += deltaVector;
			this.vertices[segmentIndex + 1] += deltaVector;
		}

		public override void MoveHorizontalSegment(int segmentIndex, double delta) {
			if (this.vertices.Count < segmentIndex + 2) {
				return;
			}
			Vector2D deltaVector = new Vector2D(0, delta);
			this.vertices[segmentIndex] += deltaVector;
			this.vertices[segmentIndex + 1] += deltaVector;
		}

		public override void Simplify() {
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

		[XmlIgnore]
		public override bool Finished {
			get { return this.finished; }
			set { this.finished = value; }
		}

		[XmlIgnore]
		public bool IsPartOfCompound {
			get { return this.isPartOfCompound; }
			set { this.isPartOfCompound = value; }
		}

		[XmlIgnore]
		public override bool Error {
			get { return this.error; }
			set { this.error = value; }
		}

		private List<Point2D> bakVertices = null;

		public override void BackupState() {
			bakVertices = new List<Point2D>(this.vertices);
		}

		public override void RevertState() {
			if (bakVertices != null) {
				this.vertices = new List<Point2D>(bakVertices);
			}
		}

		private bool isNew = false;
		[XmlIgnore]
		public override bool IsNew {
			get { return this.isNew; }
			set { this.isNew = value; }
		}

		public override bool SnapToHelplines(List<double> helplines, bool snapTop, bool snapBottom) {
			// nothing to do
			return false;
		}

		[XmlIgnore]
		public virtual RegisterType Start {
			get {
				if (this.startIndex >= 0) {
					if (this.Circuit is CircuitType) {
						CircuitType hc = this.Circuit as CircuitType;
						this.start = hc.Registers[this.startIndex];
						this.startIndex = -1;
					}
				}
				return this.start;
			}
		}

		[XmlIgnore]
		public virtual RegisterType End {
			get {
				if (this.endIndex >= 0) {
					if (this.Circuit is CircuitType) {
						CircuitType hc = this.Circuit as CircuitType;
						this.end = hc.Registers[this.endIndex];
						this.endIndex = -1;
					}
				}
				return this.end;
			}
		}

		public override int StartIndex {
			get {
				if (this.startIndex >= 0) {
					return this.startIndex;
				}
				int index = -1;
				if (this.Circuit is CircuitType) {
					CircuitType hc = this.Circuit as CircuitType;
					int i = 0;
					foreach (RegisterType r in hc.Registers) {
						if (r.Equals(this.start)) {
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

		public override int EndIndex {
			get {
				if (this.endIndex >= 0) {
					return this.endIndex;
				}
				int index = -1;
				if (this.Circuit is CircuitType) {
					CircuitType hc = this.Circuit as CircuitType;
					int i = 0;
					foreach (RegisterType r in hc.Registers) {
						if (r.Equals(this.end)) {
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
		public virtual CircuitType Circuit {
			get {
				if (this.circuitIndex >= 0) {
					this.circuit = this.Product.Product.PlannedCircuits[this.circuitIndex] as CircuitType;
					this.circuitIndex = -1;
				}
				return this.circuit;
			}
			set {
				this.circuit = value;
				this.circuitIndex = -1;
			}
		}

		public override int CircuitIndex {
			get {
				if (this.circuitIndex >= 0) {
					return this.circuitIndex;
				}
				int index = -1;
				int i = 0;
				foreach (CircuitType c in this.Product.Product.PlannedCircuits) {
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
		public override bool HasStart {
			get {
				return this.start != null;
			}
		}

		[XmlIgnore]
		public override bool HasEnd {
			get {
				return this.end != null;
			}
		}

		/*[XmlIgnore]
		public override bool CircuitHasRegister {
			get {
				return this.circuit != null && this.circuit.Registers.Count > 0;
			}
		}*/

		[XmlIgnore]
		public override int HkId {
			get {
				if (this.circuit != null && this.circuit.Registers.Count > 0) {
					return this.circuit.Registers[0].Heizkreis;
				}
				return -1;
			}
		}

		public virtual int CalculateRequiredWandwinkel() {
			int result = 0;
			if (this.vertices != null && this.vertices.Count > 2) {
				Vector2D lastVector = this.vertices[1] - this.vertices[0];
				Vector2D curVector;
				double angle;
				for (int i = 1; i < this.vertices.Count - 1; i++) {
					curVector = this.vertices[i + 1] - this.vertices[i];
					angle = (Math.Atan2(curVector.Y, curVector.X) - Math.Atan2(lastVector.Y, lastVector.X)) * 180.0 / Math.PI;
					if (angle < 0) {
						angle += 360.0;
					}
					if (angle > 180.0) {
						angle = 360.0 - angle;
					}
					/*if (angle <= 112.5) {
						result[0]++;
					} else*/
					if (angle <= 157.5) {
						//result[1]++;
						result++;
					}
					lastVector = curVector;
				}
			}
			return result;
		}

		public virtual int CalculateRequiredEckwinkel(List<double> wallBorders) {
			int result = 0;
			if (this.vertices != null && this.vertices.Count > 2) {
				Point2D lastPoint = this.vertices[0];
				Point2D curPoint;
				for (int i = 1; i < this.vertices.Count; i++) {
					curPoint = this.vertices[i];
					if (curPoint.X != lastPoint.X) {
						foreach (double border in wallBorders) {
							if (curPoint.X > lastPoint.X) {
								if (curPoint.X >= border) {
									if (lastPoint.X < border) {
										result++;
									}
									break;
								}
							} else {
								if (lastPoint.X >= border) {
									if (curPoint.X < border) {
										result++;
									}
									break;
								}
							}
						}
					}
					lastPoint = curPoint;
				}
			}
			return result;
		}

		public virtual double CalculateLength() {
			double length = 0;
			if (this.vertices != null && this.vertices.Count > 1) {
				Point2D lastPoint = this.vertices[0];
				Point2D curPoint;
				for (int i = 1; i < this.vertices.Count; i++) {
					curPoint = this.vertices[i];
					length += (curPoint - lastPoint).GetLength();
					lastPoint = curPoint;
				}
			}
			return length / 100.0;
		}
	}


	public class InvisibleSegmentAnchor : Anchor {
		private Segment2D segment;
		private int segmentIndex;
		private double thickness;

		public InvisibleSegmentAnchor(Segment2D segment, int segmentIndex, double thickness, AnchorTypeEnum anchorType, IGraphicalWallObject owner)
			: base(segment.Start, anchorType, owner) {
			this.segment = segment;
			this.segmentIndex = segmentIndex;
			this.thickness = thickness;
		}

		public InvisibleSegmentAnchor(Point2D start, Point2D end, int segmentIndex, double thickness, AnchorTypeEnum anchorType, IGraphicalWallObject owner)
			: base(start, anchorType, owner) {
			this.segment = new Segment2D(start, end);
			this.segmentIndex = segmentIndex;
			this.thickness = thickness;
		}

		public Segment2D Segment {
			get { return this.segment; }
		}

		public double Thickness {
			get { return this.thickness; }
		}

		public int SegmentIndex {
			get { return this.segmentIndex; }
		}

		public override void PaintAnchor(Graphics g, double xOffset, double yOffset, double scale) {
			// nothing to do as this anchor is invisible
		}

		public override bool HitTest(Point2D planPoint, double xOffset, double yOffset, double scale) {
			return segment.GetDistance(planPoint - new Vector2D(xOffset, yOffset)) <= thickness / 2.0;
		}

		private bool isNew = false;
		[XmlIgnore]
		public bool IsNew {
			get { return this.isNew; }
			set { this.isNew = value; }
		}
	}
}
