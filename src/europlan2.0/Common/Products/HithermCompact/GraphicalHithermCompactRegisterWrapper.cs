using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using WW.Math.Geometry;
using WW.Math;
using System.Drawing.Drawing2D;
using System.Xml.Serialization;

namespace Europlan.Common {
	public class GraphicalHithermCompactRegisterWrapper : GraphicalRegisterWrapper, IWallRegisterWrapper<HithermCompactRegister> {
		private HithermCompactRegister register;
		private HithermCompactProduct product;

		public GraphicalHithermCompactRegisterWrapper(HithermCompactProduct product) {
			this.product = product;
		}

		public GraphicalHithermCompactRegisterWrapper(HithermCompactRegister register, HithermCompactProduct product) {
			this.register = register;
			this.product = product;
		}

		public HithermCompactRegister Register {
			get { return this.register; }
			set { this.register = value; }
		}

		public HithermCompactProduct Product {
			get { return this.product; }
		}

		public Room AssociatedRoom {
			get { return this.product != null ? this.product.AssociatedRoom : null; }
		}

		public override bool HitTest(WW.Math.Point2D planPoint, double xOffset, double yOffset) {
			return this.GetObjectBorders(xOffset, yOffset)[0].IsInside(planPoint);
		}

		public override List<WW.Math.Geometry.Polygon2D> GetObjectBorders(double xOffset, double yOffset) {
			try {
				Polygon2D borders = new Polygon2D();
				if (this.register.IsParapet) {
					borders.Add(new Point2D(xOffset + this.register.GraphPosX, yOffset + this.register.GraphPosY));
					borders.Add(new Point2D(xOffset + this.register.GraphPosX + this.Width, yOffset + this.register.GraphPosY));
					borders.Add(new Point2D(xOffset + this.register.GraphPosX + this.Width, yOffset + this.register.GraphPosY + this.Height));
					borders.Add(new Point2D(xOffset + this.register.GraphPosX, yOffset + this.register.GraphPosY + this.Height));
				} else {
					borders.Add(new Point2D(xOffset + this.register.GraphPosX, yOffset + this.register.GraphPosY));
					borders.Add(new Point2D(xOffset + this.register.GraphPosX + this.Width, yOffset + this.register.GraphPosY));
					borders.Add(new Point2D(xOffset + this.register.GraphPosX + this.Width, yOffset + this.register.GraphPosY + this.Height));
					borders.Add(new Point2D(xOffset + this.register.GraphPosX, yOffset + this.register.GraphPosY + this.Height));
				}
				return new List<Polygon2D>(new Polygon2D[] { borders });
			} catch (Exception) {
				return new List<Polygon2D>();
			}
		}

		public override void PaintObject(System.Drawing.Graphics g, double xOffset, double yOffset, IGraphicalWallObject selectedObject, double scale, bool export) {
			this.PaintObject(g, xOffset, yOffset, (this == selectedObject) ? Color.Red : Color.Black, scale, false, export);
		}

		public void PaintObject(System.Drawing.Graphics g, double xOffset, double yOffset, Color color, bool export) {
			this.PaintObject(g, xOffset, yOffset, color, 1, false, export);
		}

		private const float parapet_distLeft = 3.5f;
		private const float parapet_distRight = 3.5f;
		private const float parapet_distPipeToRegister = 7.5f;
		private const float parapet_pipeWidth = 2.0f;
		private const float parapet_registerPipeWidth = 2.0f;
		private const float parapet_distBottomLeftRegisterPipe = 2.5f;
		private const float parapet_distTopPipe = 2.5f;

		private const float standard_distLeft = 3.5f;
		private const float standard_distRight = 3.5f;
		private const float standard_pipeWidth = 2.0f;
		private const float standard_distPipeToRegister = 14.0f - standard_distLeft - standard_pipeWidth;
		private const float standard_platteBottom = 6.5f;
		private const float standard_totalWidth = 62.5f;

		public void PaintObject(System.Drawing.Graphics g, double xOffset, double yOffset, Color color, double scale, bool error, bool export) {
			if (register == null) {
				return;
			}
			Pen registerPen = new Pen(color, (float)(1 / scale));
			Brush bInput = new SolidBrush(Color.FromArgb(127, Color.Red));
			Pen pInput = new Pen(Color.Red, (float)(1.0 / scale));
			Brush bOutput = new SolidBrush(Color.FromArgb(127, Color.Blue));
			Pen pOutput = new Pen(Color.Blue, (float)(1.0 / scale));
			Pen connectionPen = new Pen(registerPen.Color, 2);
			Pen plattePen = new Pen(color, (float)(1 / scale));
			plattePen.DashPattern = new float[] { 5, 5 };
			if (error || this.error) {
				registerPen.DashPattern = new float[] { 1, 2 };
				pInput.DashPattern = new float[] { 1, 2 };
				pOutput.DashPattern = new float[] { 1, 2 };
				bInput = new SolidBrush(Color.FromArgb(63, Color.Red));
				bOutput = new SolidBrush(Color.FromArgb(63, Color.Blue));
			}

			float w = 0.0f;
			float h = 0.0f;

			if (register.IsParapet) {
				w = (float)Width;
				h = (float)Height;

				float x, x1, x2, y, y1, y2;
				x1 = (float)(xOffset + register.GraphPosX + parapet_distLeft + parapet_pipeWidth + parapet_distPipeToRegister);
				x2 = (float)(xOffset + register.GraphPosX + this.Width - parapet_distRight - parapet_registerPipeWidth);
				y = (float)(yOffset + register.GraphPosY);
				float hoehe = 55;
				g.DrawRectangle(registerPen, x1, y + parapet_distBottomLeftRegisterPipe, parapet_registerPipeWidth, hoehe - parapet_distBottomLeftRegisterPipe);
				g.DrawRectangle(registerPen, x2, y, parapet_registerPipeWidth, hoehe);
				x1 = (float)(xOffset + register.GraphPosX + parapet_distLeft + parapet_pipeWidth + parapet_distPipeToRegister + parapet_registerPipeWidth / 2.0f);
				x2 = (float)(xOffset + register.GraphPosX + this.Width - parapet_distRight - parapet_registerPipeWidth / 2.0);
				for (int pos = 5; pos <= 50; pos += 5) {
					y = (float)(yOffset + register.GraphPosY + pos);
					g.DrawLine(registerPen, x1, y, x2, y);
				}
				connectionPen.EndCap = LineCap.Round;
				connectionPen.StartCap = LineCap.Flat;
				if (this.register.GraphRuecklaufHorizontal) {
					x1 = (float)(xOffset + register.GraphPosX + parapet_pipeWidth);
					x2 = (float)(xOffset + register.GraphPosX + parapet_distLeft + parapet_pipeWidth / 2.0);
					y = (float)(yOffset + register.GraphPosY + parapet_pipeWidth / 2.0);
					g.DrawLine(connectionPen, x1, y, x2, y);
					connectionPen.StartCap = LineCap.Round;
					x = x2;
					y1 = y;
					y2 = (float)(yOffset + register.GraphPosY + this.Height - parapet_distTopPipe - parapet_pipeWidth / 2.0);
					g.DrawLine(connectionPen, x, y1, x, y2);
				} else {
					x = (float)(xOffset + register.GraphPosX + parapet_distLeft + parapet_pipeWidth / 2.0);
					y1 = (float)(yOffset + register.GraphPosY + parapet_pipeWidth);
					y2 = (float)(yOffset + register.GraphPosY + this.Height - parapet_distTopPipe - parapet_pipeWidth / 2.0);
					g.DrawLine(connectionPen, x, y1, x, y2);
					connectionPen.StartCap = LineCap.Round;
				}
				x1 = x;
				x2 = x + parapet_distPipeToRegister + parapet_pipeWidth;
				y = y2;
				g.DrawLine(connectionPen, x1, y, x2, y);
				connectionPen.EndCap = LineCap.Flat;
				x = x2;
				y1 = y;
				y2 = (float)(yOffset + register.GraphPosY + 55);
				g.DrawLine(connectionPen, x, y1, x, y2);
				g.DrawRectangle(plattePen, (float)(xOffset + register.GraphPosX), (float)(yOffset + register.GraphPosY), (float)this.Width, (float)this.Height);
				g.FillRectangle(bInput, (float)(xOffset + register.GraphPosX + this.Width - parapet_distRight - parapet_pipeWidth), (float)(yOffset + register.GraphPosY), parapet_pipeWidth, parapet_pipeWidth);
				g.DrawRectangle(pInput, (float)(xOffset + register.GraphPosX + this.Width - parapet_distRight - parapet_pipeWidth), (float)(yOffset + register.GraphPosY), parapet_pipeWidth, parapet_pipeWidth);
				if (this.register.GraphRuecklaufHorizontal) {
					g.FillRectangle(bOutput, (float)(xOffset + register.GraphPosX), (float)(yOffset + register.GraphPosY), parapet_pipeWidth, parapet_pipeWidth);
					g.DrawRectangle(pOutput, (float)(xOffset + register.GraphPosX), (float)(yOffset + register.GraphPosY), parapet_pipeWidth, parapet_pipeWidth);
				} else {
					g.FillRectangle(bOutput, (float)(xOffset + register.GraphPosX + parapet_distLeft), (float)(yOffset + register.GraphPosY), parapet_pipeWidth, parapet_pipeWidth);
					g.DrawRectangle(pOutput, (float)(xOffset + register.GraphPosX + parapet_distLeft), (float)(yOffset + register.GraphPosY), parapet_pipeWidth, parapet_pipeWidth);
				}
			} else {
				h = (float)Height;
				w = (float)Width;
				for (int i = 0; i < register.RegisterCount; i++) {
					float x, x1, x2, y, y1, y2;
					x1 = (float)(xOffset + register.GraphPosX + i * standard_totalWidth + standard_distLeft + standard_pipeWidth + standard_distPipeToRegister);
					y1 = (float)(yOffset + register.GraphPosY);
					float width = (float)(standard_totalWidth - standard_distLeft - standard_pipeWidth - standard_distPipeToRegister - standard_pipeWidth);
					x2 = (float)(x1 - standard_distRight + standard_pipeWidth);
					y2 = (float)(yOffset + register.GraphPosY + this.Height - standard_platteBottom);
					g.DrawRectangle(registerPen, x1, y1, width, standard_pipeWidth);
					g.DrawRectangle(registerPen, x2, y2, width, standard_pipeWidth);
					y1 = y1 + standard_pipeWidth / 2.0f;
					y2 = y2 + standard_pipeWidth / 2.0f;
					float maxX = (float)(xOffset + register.GraphPosX + i * standard_totalWidth + standard_totalWidth - standard_distRight);
					for (x = (float)(xOffset + register.GraphPosX + i * standard_totalWidth + standard_distLeft + standard_pipeWidth + standard_distPipeToRegister + 2.5); x <= maxX; x += 5.0f) {
						g.DrawLine(registerPen, x, y1, x, y2);
					}
					connectionPen.EndCap = LineCap.Round;
					connectionPen.StartCap = LineCap.Flat;
					if (i == 0) {
						if (this.register.GraphRuecklaufHorizontal) {
							x1 = (float)(xOffset + register.GraphPosX + i * standard_totalWidth + standard_pipeWidth);
							x2 = (float)(xOffset + register.GraphPosX + i * standard_totalWidth + standard_distLeft + standard_pipeWidth / 2.0);
							y = (float)(yOffset + register.GraphPosY + standard_pipeWidth / 2.0);
							g.DrawLine(connectionPen, x1, y, x2, y);
							connectionPen.StartCap = LineCap.Round;
							x = x2;
							y1 = y;
							y2 = (float)(y1 + this.Height - standard_platteBottom);
							g.DrawLine(connectionPen, x, y1, x, y2);
						} else {
							x = (float)(xOffset + register.GraphPosX + i * standard_totalWidth + standard_distLeft + standard_pipeWidth / 2.0);
							y1 = (float)(yOffset + register.GraphPosY + standard_pipeWidth);
							y2 = (float)(y1 + this.Height - standard_platteBottom);
							g.DrawLine(connectionPen, x, y1, x, y2);
							connectionPen.StartCap = LineCap.Round;
						}
					} else {
						x1 = (float)(xOffset + register.GraphPosX + i * standard_totalWidth - standard_pipeWidth);
						x2 = (float)(xOffset + register.GraphPosX + i * standard_totalWidth + standard_distLeft + standard_pipeWidth / 2.0);
						y = (float)(yOffset + register.GraphPosY + standard_pipeWidth / 2.0);
						g.DrawLine(connectionPen, x1, y, x2, y);
						connectionPen.StartCap = LineCap.Round;
						x = x2;
						y1 = y;
						y2 = (float)(y1 + this.Height - standard_platteBottom);
						g.DrawLine(connectionPen, x, y1, x, y2);
					}
					connectionPen.EndCap = LineCap.Flat;
					x1 = x;
					x2 = (float)(xOffset + register.GraphPosX + i * standard_totalWidth - standard_distRight + standard_distLeft + standard_pipeWidth + standard_distPipeToRegister + standard_pipeWidth);
					y = y2;
					g.DrawLine(connectionPen, x1, y, x2, y);
					if (i > 0) {
						g.DrawLine(plattePen, (float)(xOffset + register.GraphPosX + i * standard_totalWidth), (float)(yOffset + register.GraphPosY + standard_platteBottom), (float)(xOffset + register.GraphPosX + i * standard_totalWidth), (float)(yOffset + register.GraphPosY + this.Height));
					}
					if (i == register.RegisterCount - 1) {
						x = (float)(xOffset + register.GraphPosX + i * standard_totalWidth + standard_totalWidth - standard_pipeWidth);
						y = (float)(yOffset + register.GraphPosY);
						g.FillRectangle(bInput, x, y, standard_pipeWidth, standard_pipeWidth);
						g.DrawRectangle(pInput, x, y, standard_pipeWidth, standard_pipeWidth);
					}
					if (i == 0) {
						if (this.register.GraphRuecklaufHorizontal) {
							x = (float)(xOffset + register.GraphPosX);
							y = (float)(yOffset + register.GraphPosY);
							g.FillRectangle(bOutput, x, y, standard_pipeWidth, standard_pipeWidth);
							g.DrawRectangle(pOutput, x, y, standard_pipeWidth, standard_pipeWidth);
						} else {
							x = (float)(xOffset + register.GraphPosX + standard_distLeft);
							y = (float)(yOffset + register.GraphPosY);
							g.FillRectangle(bOutput, x, y, standard_pipeWidth, standard_pipeWidth);
							g.DrawRectangle(pOutput, x, y, standard_pipeWidth, standard_pipeWidth);
						}

					}
				}
				g.DrawRectangle(plattePen, (float)(xOffset + register.GraphPosX), (float)(yOffset + register.GraphPosY + standard_platteBottom), (float)(this.Width), (float)(this.Height - standard_platteBottom));
			}
			if (export) {
				Font font = new Font("Arial", 8);
				string type = new HithermRegister.RegisterOrientationEnumConverter().ConvertToString(register.RegisterType);
				SizeF size = g.MeasureString(type, font);
				float x, y;
				x = (float)(xOffset + register.GraphPosX + (w / 2) - (size.Width / 2));
				y = (float)(yOffset + register.GraphPosY + (h / 2) + (size.Height / 2));

				Matrix oldTransform = g.Transform;
				Matrix textTransform = oldTransform.Clone();
				textTransform.Translate(0, -y);
				textTransform.Scale(1, -1);
				textTransform.Translate(0, -y);
				g.Transform = textTransform;
				g.FillRectangle(Brushes.White, x - 1, -(y + 1), size.Width + 1, size.Height + 1);
				g.DrawString(type, font, Brushes.Black, x, -y);
				g.Transform = oldTransform;
			}
		}

		public override IGraphicalWallObject GetPickedObject(WW.Math.Point2D planPoint, double xOffset, double yOffset) {
			if (HitTest(planPoint, xOffset, yOffset)) {
				return this;
			}
			return null;
		}

		public Point2D GetOutputConnectionPoint(double xOffset, double yOffset, double dist) {
			if (this.register.GraphRuecklaufHorizontal) {
				return new Point2D(xOffset + this.register.GraphPosX - dist, yOffset + this.register.GraphPosY + standard_pipeWidth / 2.0);
			} else {
				if (this.register.IsParapet) {
					return new Point2D(xOffset + this.register.GraphPosX + parapet_distLeft + parapet_pipeWidth / 2.0, yOffset + this.register.GraphPosY - dist);
				} else {
					return new Point2D(xOffset + this.register.GraphPosX + standard_distLeft + standard_pipeWidth / 2.0, yOffset + this.register.GraphPosY - dist);
				}
			}
		}

		private double connectionSize = 7;

		public Polygon2D GetOutputConnectionArea(double xOffset, double yOffset) {
			Polygon2D area = new Polygon2D();
			Point2D connectionPoint = this.GetOutputConnectionPoint(xOffset, yOffset, 0);
			connectionPoint = connectionPoint + (this.register.GraphRuecklaufHorizontal ? new Vector2D(parapet_pipeWidth / 2.0, 0) : new Vector2D(0, parapet_pipeWidth / 2.0));
			area.Add(new Point2D(connectionPoint.X - connectionSize / 2.0, connectionPoint.Y - connectionSize / 2.0));
			area.Add(new Point2D(connectionPoint.X - connectionSize / 2.0, connectionPoint.Y + connectionSize / 2.0));
			area.Add(new Point2D(connectionPoint.X + connectionSize / 2.0, connectionPoint.Y + connectionSize / 2.0));
			area.Add(new Point2D(connectionPoint.X + connectionSize / 2.0, connectionPoint.Y - connectionSize / 2.0));
			return area;
		}

		public Point2D GetInputConnectionPoint(double xOffset, double yOffset, double dist) {
			if (this.register.IsParapet) {
				return new Point2D(xOffset + this.register.GraphPosX + this.Width - parapet_distRight - parapet_pipeWidth / 2.0, yOffset + this.register.GraphPosY - dist);
			} else {
				return new Point2D(xOffset + this.register.GraphPosX + this.Width + dist, yOffset + this.register.GraphPosY + standard_pipeWidth / 2.0);
			}
		}

		public Polygon2D GetInputConnectionArea(double xOffset, double yOffset) {
			Polygon2D area = new Polygon2D();
			Point2D connectionPoint = this.GetInputConnectionPoint(xOffset, yOffset, 0);
			connectionPoint = connectionPoint + (this.register.IsParapet ? new Vector2D(0, parapet_pipeWidth / 2.0) : new Vector2D(-parapet_pipeWidth / 2.0, 0));
			area.Add(new Point2D(connectionPoint.X - connectionSize / 2.0, connectionPoint.Y - connectionSize / 2.0));
			area.Add(new Point2D(connectionPoint.X - connectionSize / 2.0, connectionPoint.Y + connectionSize / 2.0));
			area.Add(new Point2D(connectionPoint.X + connectionSize / 2.0, connectionPoint.Y + connectionSize / 2.0));
			area.Add(new Point2D(connectionPoint.X + connectionSize / 2.0, connectionPoint.Y - connectionSize / 2.0));
			return area;
		}

		public PossibleHithermCompactRegisterConnection GetOutputConnection(double xOffset, double yOffset, HithermCompactProduct product, HithermCompactCircuit circuit) {
			return new PossibleHithermCompactRegisterConnection(this.GetOutputConnectionPoint(xOffset, yOffset, 0), GetOutputConnectionArea(xOffset, yOffset), false, true, product, circuit, this.register, false, true, this.register.GraphRuecklaufHorizontal ? new Vector2D(-5, 0) : new Vector2D(0, -5));
		}

		public PossibleHithermCompactRegisterConnection GetInputConnection(double xOffset, double yOffset, HithermCompactProduct product, HithermCompactCircuit circuit) {
			return new PossibleHithermCompactRegisterConnection(this.GetInputConnectionPoint(xOffset, yOffset, 0), GetInputConnectionArea(xOffset, yOffset), true, false, product, circuit, this.register, false, true, this.register.IsParapet ? new Vector2D(0, -5) : new Vector2D(5, 0));
		}

		public override bool CollisionTest(IList<Polygon2D> polygon, double xOffset, double yOffset, bool ignoreBorders) {
			List<Polygon2D> register = GetObjectBorders(xOffset, yOffset);
			/*if (polygon.IsClockwise()) {
				polygon.Reverse();
			}
			if (register.IsClockwise()) {
				register.Reverse();
			}
			List<Polygon2D> list1 = new List<Polygon2D>();
			list1.Add(polygon);
			List<Polygon2D> list2 = new List<Polygon2D>();
			list2.Add(register);*/

			try {
				return Polygon2D.GetIntersection(polygon, register).Count > 0;
			} catch {
				return true;
			}
		}

		public override List<Anchor> GetAnchors(double scale) {
			List<Anchor> anchors = new List<Anchor>();
			double px5 = 4.0 / scale;
			anchors.Add(    new Anchor(this.X - px5,              this.Y - px5,               this.register.IsParapet ? AnchorTypeEnum.ANCHOR_SCALE_LEFT : AnchorTypeEnum.ANCHOR_SCALE_BOTTOM_LEFT,   this));
			anchors.Add(    new Anchor(this.X - px5,              this.Y + this.Height / 2.0, AnchorTypeEnum.ANCHOR_SCALE_LEFT,                                                                       this));
			anchors.Add(    new Anchor(this.X - px5,              this.Y + this.Height + px5, this.register.IsParapet ? AnchorTypeEnum.ANCHOR_SCALE_LEFT : AnchorTypeEnum.ANCHOR_SCALE_TOP_LEFT,      this));
			if (!this.register.IsParapet) {
				anchors.Add(new Anchor(this.X + this.Width / 2.0, this.Y + this.Height + px5, AnchorTypeEnum.ANCHOR_SCALE_TOP,                                                                        this));
			}
			anchors.Add(new Anchor(this.X + this.Width + px5, this.Y + this.Height + px5,     this.register.IsParapet ? AnchorTypeEnum.ANCHOR_SCALE_RIGHT : AnchorTypeEnum.ANCHOR_SCALE_TOP_RIGHT,    this));
			anchors.Add(new Anchor(this.X + this.Width + px5, this.Y + this.Height / 2.0,     AnchorTypeEnum.ANCHOR_SCALE_RIGHT,        this));
			anchors.Add(new Anchor(this.X + this.Width + px5, this.Y - px5,                   this.register.IsParapet ? AnchorTypeEnum.ANCHOR_SCALE_RIGHT : AnchorTypeEnum.ANCHOR_SCALE_BOTTOM_RIGHT, this));
			if (!this.register.IsParapet) {
				anchors.Add(new Anchor(this.X + this.Width / 2.0, this.Y - px5,               AnchorTypeEnum.ANCHOR_SCALE_BOTTOM,                                                                     this));
			}
			return anchors;
		}

		public GraphicalHithermCompactVerbindung GetInputLink() {
			HithermCompactCircuit c = this.product.GetCircuitForRegister(this.register);
			foreach (GraphicalHithermCompactVerbindung link in c.Links) {
				if (link.End == this.register) {
					return link;
				}
			}
			return null;
		}

		public GraphicalHithermCompactVerbindung GetOutputLink() {
			HithermCompactCircuit c = this.product.GetCircuitForRegister(this.register);
			foreach (GraphicalHithermCompactVerbindung link in c.Links) {
				if (link.Start == this.register) {
					return link;
				}
			}
			return null;
		}

		private Nullable<Point2D> startDrag = null;
		private double startDragRegisterX, startDragRegisterY, startDragRegisterWidth, startDragRegisterHeight;
		private int startRegisterCount;
		private List<Point2D> startInputConnectionVertices, startOutputConnectionVertices;
		private GraphicalHithermCompactVerbindung startInputConnection, startOutputConnection;

		public override bool StartDrag(Anchor anchor, Point2D planPoint, GraphicalWall owningWall, Room owningRoom, Product owningProduct, bool useSnap) {
			this.startDrag = planPoint;
			this.startDragRegisterX = this.register.GraphPosX;
			this.startDragRegisterY = this.register.GraphPosY;
			this.startDragRegisterHeight = this.Height;
			this.startDragRegisterWidth = this.Width;
			this.startRegisterCount = this.register.RegisterCount;

			this.startInputConnection = this.GetInputLink();
			this.startOutputConnection = this.GetOutputLink();
			if (this.startInputConnection != null) {
				this.startInputConnectionVertices = new List<Point2D>(this.startInputConnection.Vertices);
			}
			if (this.startOutputConnection != null) {
				this.startOutputConnectionVertices = new List<Point2D>(this.startOutputConnection.Vertices);
			}
			return false;
		}

		public override bool MoveDrag(Anchor anchor, Point2D planPoint, GraphicalWall owningWall, Room owningRoom, Product owningProduct, bool useSnap) {
			return MoveAnchor(anchor, planPoint, owningWall, owningRoom, false, useSnap);
		}

		private bool MoveAnchor(Anchor anchor, Point2D planPoint, GraphicalWall owningWall, Room owningRoom, bool checkLinks, bool useSnap) {
			if (this.startInputConnection != null) {
				this.startInputConnection.Vertices = new List<Point2D>(this.startInputConnectionVertices);
			}
			if (this.startOutputConnection != null) {
				this.startOutputConnection.Vertices = new List<Point2D>(this.startOutputConnectionVertices);
			}
			if (this.register.IsParapet) {
				if (anchor == null) {
					// move
					this.UpdatePosition(owningWall, startDragRegisterX + planPoint.X - startDrag.Value.X, startDragRegisterY + planPoint.Y - startDrag.Value.Y, checkLinks, useSnap);
				} else {
					if ((anchor.AnchorType & AnchorTypeEnum.ANCHOR_SCALE_LEFT) == AnchorTypeEnum.ANCHOR_SCALE_LEFT) {
						Nullable<HithermCompactRegister.HithermCompactRegisterTypeEnum> newType = HithermCompactRegister.GetRegisterTypeForSize(this.startDragRegisterWidth - planPoint.X + startDrag.Value.X + 25, this.startDragRegisterHeight, true, false);
						if (newType.HasValue) {
							this.UpdateType(owningWall, newType.Value, false, checkLinks, useSnap);
						}
					} else if ((anchor.AnchorType & AnchorTypeEnum.ANCHOR_SCALE_RIGHT) == AnchorTypeEnum.ANCHOR_SCALE_RIGHT) {
						Nullable<HithermCompactRegister.HithermCompactRegisterTypeEnum> newType = HithermCompactRegister.GetRegisterTypeForSize(this.startDragRegisterWidth + planPoint.X - startDrag.Value.X + 25, this.startDragRegisterHeight, true, false);
						if (newType.HasValue) {
							this.UpdateType(owningWall, newType.Value, true, checkLinks, useSnap);
						}
					}
				}
			} else {
				if (anchor == null) {
					// move
					this.UpdatePosition(owningWall, startDragRegisterX + planPoint.X - startDrag.Value.X, startDragRegisterY + planPoint.Y - startDrag.Value.Y, checkLinks, useSnap);
				} else {
					if ((anchor.AnchorType & AnchorTypeEnum.ANCHOR_SCALE_LEFT) == AnchorTypeEnum.ANCHOR_SCALE_LEFT) {
						int registerCount = (int)Math.Round((this.startDragRegisterWidth - planPoint.X + startDrag.Value.X) / standard_totalWidth);
						if (registerCount < 1) {
							registerCount = 1;
						}
						this.UpdateRegisterCount(owningWall, registerCount, false, checkLinks, useSnap);
					} else if ((anchor.AnchorType & AnchorTypeEnum.ANCHOR_SCALE_RIGHT) == AnchorTypeEnum.ANCHOR_SCALE_RIGHT) {
						int registerCount = (int)Math.Round((this.startDragRegisterWidth + planPoint.X - startDrag.Value.X) / standard_totalWidth);
						if (registerCount < 1) {
							registerCount = 1;
						}
						this.UpdateRegisterCount(owningWall, registerCount, true, checkLinks, useSnap);
					}

					if ((anchor.AnchorType & AnchorTypeEnum.ANCHOR_SCALE_TOP) == AnchorTypeEnum.ANCHOR_SCALE_TOP) {
						Nullable<HithermCompactRegister.HithermCompactRegisterTypeEnum> newType = HithermCompactRegister.GetRegisterTypeForSize(this.startDragRegisterWidth, this.startDragRegisterHeight + planPoint.Y - this.startDrag.Value.Y + 25, false, this.register.IsDachschraege);
						if (newType.HasValue) {
							this.UpdateType(owningWall, newType.Value, true, checkLinks, useSnap);
						}
					} else if ((anchor.AnchorType & AnchorTypeEnum.ANCHOR_SCALE_BOTTOM) == AnchorTypeEnum.ANCHOR_SCALE_BOTTOM) {
						Nullable<HithermCompactRegister.HithermCompactRegisterTypeEnum> newType = HithermCompactRegister.GetRegisterTypeForSize(this.startDragRegisterWidth, this.startDragRegisterHeight - planPoint.Y + this.startDrag.Value.Y + 25, false, this.register.IsDachschraege);
						if (newType.HasValue) {
							this.UpdateType(owningWall, newType.Value, false, checkLinks, useSnap);
						}
					}
				}
			}
			return true;
		}

		public override bool EndDrag(Anchor anchor, Point2D planPoint, GraphicalWall owningWall, Room owningRoom, Product owningProduct, bool useSnap) {
			owningRoom.MarkErrors(this, owningWall);
			HithermCompactCircuit circuit = product.GetCircuitForRegister(this.register);
			foreach (GraphicalHithermCompactVerbindung link in circuit.Links) {
				if (link.Start == this.register) {
					link.Error = !link.CheckValidity(null, 0, 0);
				}
				if (link.End == this.register) {
					link.Error = !link.CheckValidity(null, 0, 0);
				}
			}

			this.startDrag = null;
			return true;
		}

		public override bool CheckValidity(GraphicalWall owningWall, double offsetX, double offsetY) {
			List<Polygon2D> registerBorders = this.GetObjectBorders(offsetX, offsetY);
			if (owningWall.CollisionTest(registerBorders, offsetX, offsetY, false)) {
				return false;
			} else {
				foreach (GraphicalHithermCompactRegisterWrapper register in owningWall.Registers) {
					if (register != this && register.CollisionTest(registerBorders, offsetX, offsetY, false)) {
						return false;
					}
				}
				foreach (GraphicalWallObstacle obstacle in owningWall.Obstacles) {
					if (obstacle.CollisionTest(registerBorders, offsetX, offsetY, false)) {
						return false;
					}
				}
			}
			return true;
		}

		public bool UpdatePosition(GraphicalWall owningWall, double newPosX, double newPosY, bool updateLinks, bool snapEnabled) {
			return this.UpdatePositionAndSize(owningWall, newPosX, newPosY, null, null, null, null, updateLinks, snapEnabled);
		}

		public bool UpdateRegisterCount(GraphicalWall owningWall, int newRegisterCount, bool anchorStart, bool updateLinks, bool snapEnabled) {
			return this.UpdatePositionAndSize(owningWall, null, null, newRegisterCount, anchorStart, null, null, updateLinks, snapEnabled);
		}

		public bool UpdateType(GraphicalWall owningWall, HithermCompactRegister.HithermCompactRegisterTypeEnum newType, bool anchorStart, bool updateLinks, bool snapEnabled) {
			return this.UpdatePositionAndSize(owningWall, null, null, null, null, newType, anchorStart, updateLinks, snapEnabled);
		}

		public bool UpdatePositionAndSize(GraphicalWall owningWall, Nullable<double> newPosX, Nullable<double> newPosY, Nullable<int> newRegisterCount, Nullable<bool> anchorRohreStart, Nullable<HithermCompactRegister.HithermCompactRegisterTypeEnum> newType, Nullable<bool> anchorTypeStart, bool checkLinks, bool useSnap) {
			double oldPosX = this.register.GraphPosX;
			double oldPosY = this.register.GraphPosY;
			double oldWidth = this.Height;
			double oldHeight = this.Width;
			int oldRegisterCount = this.register.RegisterCount;
			HithermCompactRegister.HithermCompactRegisterTypeEnum oldType = this.register.RegisterType;

			Vector2D offset = this.product.AssociatedRoom.GetWallOffset(owningWall).Value * 100;
			bool ok = false;
			if (newPosX.HasValue && newPosY.HasValue && !newRegisterCount.HasValue && !newType.HasValue) {
				this.register.GraphPosY = newPosY.Value;
				bool retryY = false;
				if (useSnap) {
					this.SnapToHelplines(owningWall.AllHelpLines, true, true);
				}
				if (!this.CheckValidity(owningWall, offset.X, offset.Y)) {
					this.register.GraphPosY = oldPosY;
					retryY = true;
				}
				this.register.GraphPosX = newPosX.Value;
				if (!this.CheckValidity(owningWall, offset.X, offset.Y)) {
					this.register.GraphPosX = oldPosX;
				}
				if (retryY) {
					this.register.GraphPosY = newPosY.Value;
					if (useSnap) {
						this.SnapToHelplines(owningWall.AllHelpLines, true, true);
					}
					if (!this.CheckValidity(owningWall, offset.X, offset.Y)) {
						this.register.GraphPosY = oldPosY;
					}
				}
				ok = this.register.GraphPosX != oldPosX || this.register.GraphPosY != oldPosY;
			} else {
				if (newPosX.HasValue) {
					this.register.GraphPosX = newPosX.Value;
				}
				if (newPosY.HasValue) {
					this.register.GraphPosY = newPosY.Value;
				}
				if (newRegisterCount.HasValue) {
					double oldHoehe = this.Height;
					double oldBreite = this.Width;
					this.register.RegisterCount = newRegisterCount.Value;
					if (anchorRohreStart.HasValue && !anchorRohreStart.Value) {
						if (this.register.IsParapet) {
							this.register.GraphPosY = this.register.GraphPosY + (oldHoehe - this.Height);
						} else {
							this.register.GraphPosX = this.register.GraphPosX + (oldBreite - this.Width);
						}
					}
				}
				if (newType.HasValue) {
					double oldHoehe = this.Height;
					double oldBreite = this.Width;
					this.register.RegisterType = newType.Value;
					if (anchorTypeStart.HasValue && !anchorTypeStart.Value) {
						if (this.register.IsParapet) {
							this.register.GraphPosX = this.register.GraphPosX + (oldBreite - this.Width);
						} else {
							this.register.GraphPosY = this.register.GraphPosY + (oldHoehe - this.Height);
						}
					}
				}

				ok = this.CheckValidity(owningWall, offset.X, offset.Y);
				if (!ok) {
					this.register.GraphPosX = oldPosX;
					this.register.GraphPosY = oldPosY;
					this.register.RegisterCount = oldRegisterCount;
					this.register.RegisterType = oldType;
				}
			}

			HithermCompactCircuit circuit = product.GetCircuitForRegister(this.register);
			foreach (GraphicalHithermCompactVerbindung link in circuit.Links) {
				if (link.Start == this.register) {
					link.RevertState();
					link.UpdateStartPoint(this, owningWall, checkLinks);
				}
				if (link.End == this.register) {
					link.RevertState();
					link.UpdateEndPoint(this, owningWall, checkLinks);
				}
			}

			return ok;
		}

		public double X {
			get { return this.register.GraphPosX; }
		}

		public double Y {
			get { return this.register.GraphPosY; }
		}

		public double Height {
			get { return this.register.RegisterHoehe / 10.0; }
		}

		public double Width {
			get { return this.register.RegisterBreite * this.register.RegisterCount / 10.0; }
		}

		private double bakX, bakY;
		private HithermCompactRegister.HithermCompactRegisterTypeEnum bakType;
		private int bakRegisterCount = -1;

		public override void BackupState() {
			this.bakX = this.register.GraphPosX;
			this.bakY = this.register.GraphPosY;
			this.bakType = this.register.RegisterType;
			this.bakRegisterCount = this.register.RegisterCount;
			HithermCompactCircuit circuit = product.GetCircuitForRegister(this.register);
			foreach (GraphicalHithermCompactVerbindung link in circuit.Links) {
				if (link.Start == this.register) {
					link.BackupState();
				}
				if (link.End == this.register) {
					link.BackupState();
				}
			}
		}

		public override void RevertState() {
			if (bakRegisterCount > 0) {
				this.register.GraphPosX = bakX;
				this.register.GraphPosY = bakY;
				this.register.RegisterType = bakType;
				this.register.RegisterCount = bakRegisterCount;
				HithermCompactCircuit circuit = product.GetCircuitForRegister(this.register);
				if (circuit != null) {
					foreach (GraphicalHithermCompactVerbindung link in circuit.Links) {
						if (link.Start == this.register) {
							link.RevertState();
						}
						if (link.End == this.register) {
							link.RevertState();
						}
					}
				}
			}
		}

		public override bool SnapToHelplines(List<double> helplines, bool snapTop, bool snapBottom) {
#if BLUB
			if (helplines == null) {
				return false;
			}
			double top = this.Y + this.Height;
			double bottom = this.Y;

			double deltaTop = double.MaxValue;
			double deltaBottom = double.MaxValue;

			double newTop = top;
			double newBottom = bottom;

			double newDeltaTop, newDeltaBottom;
			bool snappedTop = false;
			bool snappedBottom = false;

			double moveHelpline = this.register.Orientation == HithermRegister.RegisterOrientationEnum.ORIENTATION_VERTIKAL ? -1.0 : 0;

			foreach (double helpline in helplines) {
				newDeltaTop = Math.Abs((helpline - moveHelpline) - top);
				newDeltaBottom = Math.Abs((helpline + moveHelpline) - bottom);
				if (newDeltaTop < newDeltaBottom) {
					if (snapTop && newDeltaTop <= GraphicalWall.HELPLINE_SNAP_DISTANCE && newDeltaTop < deltaTop) {
						deltaTop = newDeltaTop;
						newTop = (helpline - moveHelpline);
						snappedTop = true;
					}
				} else {
					if (snapBottom && newDeltaBottom <= GraphicalWall.HELPLINE_SNAP_DISTANCE && newDeltaBottom < deltaBottom) {
						deltaBottom = newDeltaBottom;
						newBottom = (helpline + moveHelpline);
						snappedBottom = true;
					}
				}
			}
			if (snapTop && snapBottom) {
				if (snappedTop && deltaTop <= deltaBottom) {
					this.Register.GraphPosY = newTop - this.Height;
				} else if (snappedBottom) {
					this.Register.GraphPosY = newBottom;
				}
			} else if (snapTop && snappedTop) {
				this.Register.GraphPosY = newTop - this.Height;
			} else if (snapBottom && snappedBottom) {
				this.Register.GraphPosY = newBottom;
			}
			return (snappedTop && snapTop) || (snappedBottom && snapBottom);
#else
			return false;
#endif
		}
	}
}
