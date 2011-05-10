using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using WW.Math.Geometry;
using WW.Math;
using System.Drawing.Drawing2D;

namespace Europlan.Common {
	public class GraphicalHithermRegisterWrapper : GraphicalRegisterWrapper {
		private HithermRegister register;

		public GraphicalHithermRegisterWrapper() {
		}

		public GraphicalHithermRegisterWrapper(HithermRegister register) {
			this.register = register;
		}

		public HithermRegister Register {
			get { return this.register; }
			set { this.register = value; }
		}

		public override bool HitTest(WW.Math.Point2D planPoint, double xOffset, double yOffset) {
			return this.GetObjectBorders(xOffset, yOffset).IsInside(planPoint);
		}

		public override WW.Math.Geometry.Polygon2D GetObjectBorders(double xOffset, double yOffset) {
			Polygon2D borders = new Polygon2D();
			if (this.register.Orientation == HithermRegister.RegisterOrientationEnum.ORIENTATION_VERTIKAL) {
				borders.Add(new Point2D(xOffset + this.register.GraphPosX, yOffset + this.register.GraphPosY));
				borders.Add(new Point2D(xOffset + this.register.GraphPosX, yOffset + this.register.GraphPosY + this.register.RegisterHoehe));
				borders.Add(new Point2D(xOffset + this.register.GraphPosX + this.register.RegisterBreiteForDrawing, yOffset + this.register.GraphPosY + this.register.RegisterHoehe));
				borders.Add(new Point2D(xOffset + this.register.GraphPosX + this.register.RegisterBreiteForDrawing, yOffset + this.register.GraphPosY));
			} else {
				borders.Add(new Point2D(xOffset + this.register.GraphPosX, yOffset + this.register.GraphPosY));
				borders.Add(new Point2D(xOffset + this.register.GraphPosX, yOffset + this.register.GraphPosY + this.register.RegisterBreiteForDrawing));
				borders.Add(new Point2D(xOffset + this.register.GraphPosX + this.register.RegisterHoehe, yOffset + this.register.GraphPosY + this.register.RegisterBreiteForDrawing));
				borders.Add(new Point2D(xOffset + this.register.GraphPosX + this.register.RegisterHoehe, yOffset + this.register.GraphPosY));
			}
			return borders;
		}

		public override void PaintObject(System.Drawing.Graphics g, double xOffset, double yOffset, IGraphicalWallObject selectedObject, double scale) {
			this.PaintObject(g, xOffset, yOffset, (this == selectedObject) ? Color.Red : Color.Black, scale, (this == selectedObject), false/*, this == selectedObject*/);
		}

		public void PaintObject(System.Drawing.Graphics g, double xOffset, double yOffset, Color color) {
			this.PaintObject(g, xOffset, yOffset, color, 1, false, false/*, false*/);
		}

		public void PaintObject(System.Drawing.Graphics g, double xOffset, double yOffset, Color color, double scale, bool highlightConnections, bool error/*, bool drawAnchors*/) {
			highlightConnections = false;
			if (register == null) {
				return;
			}
			Pen registerPen = new Pen(color, (float)(1 / scale));
			if (error) {
				registerPen.DashStyle = DashStyle.DashDotDot;
			}
			Brush bInput = new SolidBrush(Color.FromArgb(127, Color.Red));
			Pen pInput = new Pen(Color.Red, (float)(1.0 / scale));
			Brush bOutput = new SolidBrush(Color.FromArgb(127, Color.Blue));
			Pen pOutput = new Pen(Color.Blue, (float)(1.0 / scale));
			if (register.Orientation == HithermRegister.RegisterOrientationEnum.ORIENTATION_VERTIKAL) {
				float x = (float)(xOffset + register.GraphPosX);
				float y1 = (float)(yOffset + register.GraphPosY);
				float y2 = (float)(yOffset + register.GraphPosY + register.RegisterHoehe - 2);
				float breite = (float)register.RegisterBreiteForDrawing;
				g.DrawRectangle(registerPen, x, y1, breite, 2);
				g.DrawRectangle(registerPen, x, y2, breite, 2);
				double pos = 5;
				for (int i = 0; i < register.Rohre; i++) {
					x = (float)(xOffset + register.GraphPosX + pos);
					y1 = (float)(yOffset + register.GraphPosY + 1);
					y2 = (float)(yOffset + register.GraphPosY + register.RegisterHoehe - 1);
					g.DrawLine(registerPen, x, y1, x, y2);
					if (register.Rohrabstand == HithermRegister.RohrabstandEnum.RC_HOCHLEISTUNG) {
						if (HithermProduct.ConfigUsePlus) {
							if (i % 14 == 13) {
								pos += 10;
							} else {
								pos += 5;
							}
						} else {
							if (i % 9 == 8) {
								pos += 10;
							} else {
								pos += 5;
							}
						}
					} else {
						pos += 10;
					}
				}
				if (highlightConnections) {
					if (register.GraphVorlaufRight) {
						g.FillRectangle(bInput, (float)(xOffset + register.GraphPosX + register.RegisterBreiteForDrawing - 3.5), (float)(yOffset + register.GraphPosY - 1.5), 5, 5);
						g.DrawRectangle(pInput, (float)(xOffset + register.GraphPosX + register.RegisterBreiteForDrawing - 3.5), (float)(yOffset + register.GraphPosY - 1.5), 5, 5);
						g.FillRectangle(bOutput, (float)(xOffset + register.GraphPosX - 1.5), (float)(yOffset + register.GraphPosY + register.RegisterHoehe - 3.5), 5, 5);
						g.DrawRectangle(pOutput, (float)(xOffset + register.GraphPosX - 1.5), (float)(yOffset + register.GraphPosY + register.RegisterHoehe - 3.5), 5, 5);
					} else {
						g.FillRectangle(bInput, (float)(xOffset + register.GraphPosX - 1.5), (float)(yOffset + register.GraphPosY - 1.5), 5, 5);
						g.DrawRectangle(pInput, (float)(xOffset + register.GraphPosX - 1.5), (float)(yOffset + register.GraphPosY - 1.5), 5, 5);
						g.FillRectangle(bOutput, (float)(xOffset + register.GraphPosX + register.RegisterBreiteForDrawing - 3.5), (float)(yOffset + register.GraphPosY + register.RegisterHoehe - 3.5), 5, 5);
						g.DrawRectangle(pOutput, (float)(xOffset + register.GraphPosX + register.RegisterBreiteForDrawing - 3.5), (float)(yOffset + register.GraphPosY + register.RegisterHoehe - 3.5), 5, 5);
					}
				} else {
					if (register.GraphVorlaufRight) {
						g.FillRectangle(bInput, (float)(xOffset + register.GraphPosX + register.RegisterBreiteForDrawing - 2), (float)(yOffset + register.GraphPosY - 0), 2, 2);
						g.DrawRectangle(pInput, (float)(xOffset + register.GraphPosX + register.RegisterBreiteForDrawing - 2), (float)(yOffset + register.GraphPosY - 0), 2, 2);
						g.FillRectangle(bOutput, (float)(xOffset + register.GraphPosX - 0), (float)(yOffset + register.GraphPosY + register.RegisterHoehe - 2), 2, 2);
						g.DrawRectangle(pOutput, (float)(xOffset + register.GraphPosX - 0), (float)(yOffset + register.GraphPosY + register.RegisterHoehe - 2), 2, 2);
					} else {
						g.FillRectangle(bInput, (float)(xOffset + register.GraphPosX - 0), (float)(yOffset + register.GraphPosY - 0), 2, 2);
						g.DrawRectangle(pInput, (float)(xOffset + register.GraphPosX - 0), (float)(yOffset + register.GraphPosY - 0), 2, 2);
						g.FillRectangle(bOutput, (float)(xOffset + register.GraphPosX + register.RegisterBreiteForDrawing - 2), (float)(yOffset + register.GraphPosY + register.RegisterHoehe - 2), 2, 2);
						g.DrawRectangle(pOutput, (float)(xOffset + register.GraphPosX + register.RegisterBreiteForDrawing - 2), (float)(yOffset + register.GraphPosY + register.RegisterHoehe - 2), 2, 2);
					}
				}
			} else {
				float x1 = (float)(xOffset + register.GraphPosX);
				float x2 = (float)(xOffset + register.GraphPosX + register.RegisterHoehe - 2);
				float y = (float)(yOffset + register.GraphPosY);
				float breite = (float)register.RegisterBreiteForDrawing;
				g.DrawRectangle(registerPen, x1, y, 2, breite);
				g.DrawRectangle(registerPen, x2, y, 2, breite);
				double pos = 5;
				for (int i = 0; i < register.Rohre; i++) {
					x1 = (float)(xOffset + register.GraphPosX + 1);
					x2 = (float)(xOffset + register.GraphPosX + register.RegisterHoehe - 1);
					y = (float)(yOffset + register.GraphPosY + pos);

					g.DrawLine(registerPen, x1, y, x2, y);
					if (register.Rohrabstand == HithermRegister.RohrabstandEnum.RC_HOCHLEISTUNG) {
						if (HithermProduct.ConfigUsePlus) {
							if (i % 14 == 13) {
								pos += 10;
							} else {
								pos += 5;
							}
						} else {
							if (i % 9 == 8) {
								pos += 10;
							} else {
								pos += 5;
							}
						}
					} else {
						pos += 10;
					}
				}
				if (highlightConnections) {
					if (register.GraphVorlaufRight) {
						g.FillRectangle(bInput, (float)(xOffset + register.GraphPosX + register.RegisterHoehe - 3.5), (float)(yOffset + register.GraphPosY - 1.5), 5, 5);
						g.DrawRectangle(pInput, (float)(xOffset + register.GraphPosX + register.RegisterHoehe - 3.5), (float)(yOffset + register.GraphPosY - 1.5), 5, 5);
						g.FillRectangle(bOutput, (float)(xOffset + register.GraphPosX - 1.5), (float)(yOffset + register.GraphPosY + register.RegisterBreiteForDrawing - 3.5), 5, 5);
						g.DrawRectangle(pOutput, (float)(xOffset + register.GraphPosX - 1.5), (float)(yOffset + register.GraphPosY + register.RegisterBreiteForDrawing - 3.5), 5, 5);
					} else {
						g.FillRectangle(bInput, (float)(xOffset + register.GraphPosX - 1.5), (float)(yOffset + register.GraphPosY - 1.5), 5, 5);
						g.DrawRectangle(pInput, (float)(xOffset + register.GraphPosX - 1.5), (float)(yOffset + register.GraphPosY - 1.5), 5, 5);
						g.FillRectangle(bOutput, (float)(xOffset + register.GraphPosX + register.RegisterHoehe - 3.5), (float)(yOffset + register.GraphPosY + register.RegisterBreiteForDrawing - 3.5), 5, 5);
						g.DrawRectangle(pOutput, (float)(xOffset + register.GraphPosX + register.RegisterHoehe - 3.5), (float)(yOffset + register.GraphPosY + register.RegisterBreiteForDrawing - 3.5), 5, 5);
					}
				} else {
					if (register.GraphVorlaufRight) {
						g.FillRectangle(bInput, (float)(xOffset + register.GraphPosX + register.RegisterHoehe - 2), (float)(yOffset + register.GraphPosY - 0), 2, 2);
						g.DrawRectangle(pInput, (float)(xOffset + register.GraphPosX + register.RegisterHoehe - 2), (float)(yOffset + register.GraphPosY - 0), 2, 2);
						g.FillRectangle(bOutput, (float)(xOffset + register.GraphPosX - 0), (float)(yOffset + register.GraphPosY + register.RegisterBreiteForDrawing - 2), 2, 2);
						g.DrawRectangle(pOutput, (float)(xOffset + register.GraphPosX - 0), (float)(yOffset + register.GraphPosY + register.RegisterBreiteForDrawing - 2), 2, 2);
					} else {
						g.FillRectangle(bInput, (float)(xOffset + register.GraphPosX - 0), (float)(yOffset + register.GraphPosY - 0), 2, 2);
						g.DrawRectangle(pInput, (float)(xOffset + register.GraphPosX - 0), (float)(yOffset + register.GraphPosY - 0), 2, 2);
						g.FillRectangle(bOutput, (float)(xOffset + register.GraphPosX + register.RegisterHoehe - 2), (float)(yOffset + register.GraphPosY + register.RegisterBreiteForDrawing - 2), 2, 2);
						g.DrawRectangle(pOutput, (float)(xOffset + register.GraphPosX + register.RegisterHoehe - 2), (float)(yOffset + register.GraphPosY + register.RegisterBreiteForDrawing - 2), 2, 2);
					}
				}
			}
			/*if (drawAnchors) {
				Region oldClip = g.Clip;
				g.ResetClip();
				foreach (Anchor a in this.GetAnchors(scale)) {
					a.PaintAnchor(g, xOffset, yOffset, scale);
				}
				g.Clip = oldClip;
			}*/
		}

		public override IGraphicalWallObject GetPickedObject(WW.Math.Point2D planPoint, double xOffset, double yOffset) {
			if (HitTest(planPoint, xOffset, yOffset)) {
				return this;
			}
			return null;
		}

		public Point2D GetOutputConnectionPoint(double xOffset, double yOffset) {
			if (this.register.GraphVorlaufRight) {
				return new Point2D(xOffset + this.register.GraphPosX, yOffset + this.register.GraphPosY + this.register.RegisterHoehe - 1);
			} else {
				return new Point2D(xOffset + this.register.GraphPosX + this.register.RegisterBreiteForDrawing, yOffset + this.register.GraphPosY + this.register.RegisterHoehe - 1);
			}
		}

		public Polygon2D GetOutputConnectionArea(double xOffset, double yOffset) {
			Polygon2D area = new Polygon2D();
			if (this.register.GraphVorlaufRight) {
				area.Add(new Point2D(xOffset + this.register.GraphPosX - 1.5, yOffset + register.GraphPosY + register.RegisterHoehe - 3.5));
				area.Add(new Point2D(xOffset + this.register.GraphPosX - 1.5, yOffset + register.GraphPosY + register.RegisterHoehe + 1.5));
				area.Add(new Point2D(xOffset + this.register.GraphPosX + 3.5, yOffset + register.GraphPosY + register.RegisterHoehe + 1.5));
				area.Add(new Point2D(xOffset + this.register.GraphPosX + 3.5, yOffset + register.GraphPosY + register.RegisterHoehe - 3.5));
			} else {
				area.Add(new Point2D(xOffset + this.register.GraphPosX + this.register.RegisterBreiteForDrawing - 3.5, yOffset + register.GraphPosY + register.RegisterHoehe - 3.5));
				area.Add(new Point2D(xOffset + this.register.GraphPosX + this.register.RegisterBreiteForDrawing - 3.5, yOffset + register.GraphPosY + register.RegisterHoehe + 1.5));
				area.Add(new Point2D(xOffset + this.register.GraphPosX + this.register.RegisterBreiteForDrawing + 1.5, yOffset + register.GraphPosY + register.RegisterHoehe + 1.5));
				area.Add(new Point2D(xOffset + this.register.GraphPosX + this.register.RegisterBreiteForDrawing + 1.5, yOffset + register.GraphPosY + register.RegisterHoehe - 3.5));
			}
			return area;
		}

		public Point2D GetInputConnectionPoint(double xOffset, double yOffset) {
			if (this.register.GraphVorlaufRight) {
				return new Point2D(xOffset + this.register.GraphPosX + this.register.RegisterBreiteForDrawing, yOffset + this.register.GraphPosY + 1);
			} else {
				return new Point2D(xOffset + this.register.GraphPosX, yOffset + this.register.GraphPosY + 1);
			}
		}

		public Polygon2D GetInputConnectionArea(double xOffset, double yOffset) {
			Polygon2D area = new Polygon2D();
			if (this.register.GraphVorlaufRight) {
				area.Add(new Point2D(xOffset + this.register.GraphPosX + this.register.RegisterBreiteForDrawing - 3.5, yOffset + register.GraphPosY - 1.5));
				area.Add(new Point2D(xOffset + this.register.GraphPosX + this.register.RegisterBreiteForDrawing - 3.5, yOffset + register.GraphPosY + 3.5));
				area.Add(new Point2D(xOffset + this.register.GraphPosX + this.register.RegisterBreiteForDrawing + 1.5, yOffset + register.GraphPosY + 3.5));
				area.Add(new Point2D(xOffset + this.register.GraphPosX + this.register.RegisterBreiteForDrawing + 1.5, yOffset + register.GraphPosY - 1.5));
			} else {
				area.Add(new Point2D(xOffset + this.register.GraphPosX - 1.5, yOffset + register.GraphPosY - 1.5));
				area.Add(new Point2D(xOffset + this.register.GraphPosX - 1.5, yOffset + register.GraphPosY + 3.5));
				area.Add(new Point2D(xOffset + this.register.GraphPosX + 3.5, yOffset + register.GraphPosY + 3.5));
				area.Add(new Point2D(xOffset + this.register.GraphPosX + 3.5, yOffset + register.GraphPosY - 1.5));
			}
			return area;
		}

		public PossibleHithermRegisterConnection GetOutputConnection(double xOffset, double yOffset, HithermProduct product, HithermCircuit circuit) {
			return new PossibleHithermRegisterConnection(this.GetOutputConnectionPoint(xOffset, yOffset), GetOutputConnectionArea(xOffset, yOffset), false, true, product, circuit, this.register);
		}

		public PossibleHithermRegisterConnection GetInputConnection(double xOffset, double yOffset, HithermProduct product, HithermCircuit circuit) {
			return new PossibleHithermRegisterConnection(this.GetInputConnectionPoint(xOffset, yOffset), GetInputConnectionArea(xOffset, yOffset), true, false, product, circuit, this.register);
		}

		public override bool CollisionTest(Polygon2D polygon, double xOffset, double yOffset) {
			Polygon2D register = GetObjectBorders(xOffset, yOffset);
			if (polygon.IsClockwise()) {
				polygon.Reverse();
			}
			if (register.IsClockwise()) {
				register.Reverse();
			}
			List<Polygon2D> list1 = new List<Polygon2D>();
			list1.Add(polygon);
			List<Polygon2D> list2 = new List<Polygon2D>();
			list2.Add(register);

			return Polygon2D.GetIntersection(list1, list2).Count > 0;
		}

		public override List<Anchor> GetAnchors(double scale) {
			List<Anchor> anchors = new List<Anchor>();
			double px5 = 4.0 / scale;
			anchors.Add(new Anchor(this.X - px5,              this.Y - px5,               AnchorTypeEnum.ANCHOR_SCALE_BOTTOM_LEFT,  this));
			anchors.Add(new Anchor(this.X - px5,              this.Y + this.Height / 2.0, AnchorTypeEnum.ANCHOR_SCALE_LEFT,         this));
			anchors.Add(new Anchor(this.X - px5,              this.Y + this.Height + px5, AnchorTypeEnum.ANCHOR_SCALE_TOP_LEFT,     this));
			anchors.Add(new Anchor(this.X + this.Width / 2.0, this.Y + this.Height + px5, AnchorTypeEnum.ANCHOR_SCALE_TOP,          this));
			anchors.Add(new Anchor(this.X + this.Width + px5, this.Y + this.Height + px5, AnchorTypeEnum.ANCHOR_SCALE_TOP_RIGHT,    this));
			anchors.Add(new Anchor(this.X + this.Width + px5, this.Y + this.Height / 2.0, AnchorTypeEnum.ANCHOR_SCALE_RIGHT,        this));
			anchors.Add(new Anchor(this.X + this.Width + px5, this.Y - px5,               AnchorTypeEnum.ANCHOR_SCALE_BOTTOM_RIGHT, this));
			anchors.Add(new Anchor(this.X + this.Width / 2.0, this.Y - px5,               AnchorTypeEnum.ANCHOR_SCALE_BOTTOM,       this));
			return anchors;
		}

		private int GetBestRohrCount(double width) {
			int bestRohre = 3;
			double bestDelta = double.MaxValue;
			foreach (KeyValuePair<int, double> kvp in this.register.PossibleWidths) {
				if (Math.Abs(kvp.Value - width) < bestDelta) {
					bestDelta = Math.Abs(kvp.Value - width);
					bestRohre = kvp.Key;
				}
			}
			return bestRohre;
		}

		private Nullable<Point2D> startDrag = null;
		private double startDragRegisterX, startDragRegisterY, startDragRegisterWidth;
		private int startDragRegisterHeight, startRegisterRohre;

		public override bool StartDrag(Anchor anchor, Point2D planPoint, GraphicalWall owningWall) {
			this.startDrag = planPoint;
			this.startDragRegisterX = this.register.GraphPosX;
			this.startDragRegisterY = this.register.GraphPosY;
			this.startDragRegisterHeight = this.register.RegisterHoehe;
			this.startDragRegisterWidth = this.register.RegisterBreiteForDrawing;
			this.startRegisterRohre = this.register.Rohre;
			// TODO
			return false;
		}

		public override bool MoveDrag(Anchor anchor, Point2D planPoint, GraphicalWall owningWall) {
			if (this.register.Orientation == HithermRegister.RegisterOrientationEnum.ORIENTATION_VERTIKAL) {
				if (anchor == null) {
					// move
					double tmpX = this.register.GraphPosX;
					double tmpY = this.register.GraphPosY;
					bool retryY = false;
					this.register.GraphPosY = startDragRegisterY + planPoint.Y - startDrag.Value.Y;
					if (!this.PositionAndSizeOk(owningWall, 0, 0)) {
						this.register.GraphPosY = tmpY;
						retryY = true;
					}
					this.register.GraphPosX = startDragRegisterX + planPoint.X - startDrag.Value.X;
					if (!this.PositionAndSizeOk(owningWall, 0, 0)) {
						this.register.GraphPosX = tmpX;
					}
					if (retryY) {
						this.register.GraphPosY = startDragRegisterY + planPoint.Y - startDrag.Value.Y;
						if (!this.PositionAndSizeOk(owningWall, 0, 0)) {
							this.register.GraphPosY = tmpY;
						}
					}
				} else {
					if ((anchor.AnchorType & AnchorTypeEnum.ANCHOR_SCALE_LEFT) == AnchorTypeEnum.ANCHOR_SCALE_LEFT) {
						int tmpRohre = this.register.Rohre;
						double tmpX = this.register.GraphPosX;
						this.register.Rohre = GetBestRohrCount(this.startDragRegisterWidth - planPoint.X + startDrag.Value.X);
						this.register.GraphPosX = this.startDragRegisterX + this.startDragRegisterWidth - this.register.RegisterBreiteForDrawing;
						if (!this.PositionAndSizeOk(owningWall, 0, 0)) {
							this.register.Rohre = tmpRohre;
							this.register.GraphPosX = tmpX;
						}
					} else if ((anchor.AnchorType & AnchorTypeEnum.ANCHOR_SCALE_RIGHT) == AnchorTypeEnum.ANCHOR_SCALE_RIGHT) {
						int tmpRohre = this.register.Rohre;
						this.register.Rohre = GetBestRohrCount(this.startDragRegisterWidth + planPoint.X - startDrag.Value.X);
						if (!this.PositionAndSizeOk(owningWall, 0, 0)) {
							this.register.Rohre = tmpRohre;
						}
					}

					if ((anchor.AnchorType & AnchorTypeEnum.ANCHOR_SCALE_TOP) == AnchorTypeEnum.ANCHOR_SCALE_TOP) {
						HithermRegister.HithermRegisterTypeEnum tmpType = this.register.RegisterType;
						Nullable<HithermRegister.HithermRegisterTypeEnum> newType = HithermRegister.GetRegisterTypeForHoehe((int)(this.startDragRegisterHeight + planPoint.Y - this.startDrag.Value.Y + 25), this.register.IsHochleistungsRegister);
						this.register.RegisterType = newType.HasValue ? newType.Value : HithermRegister.GetRegisterTypeForHoehe(50, this.register.IsHochleistungsRegister).Value;
						if (!this.PositionAndSizeOk(owningWall, 0, 0)) {
							this.register.RegisterType = tmpType;
						}
					} else if ((anchor.AnchorType & AnchorTypeEnum.ANCHOR_SCALE_BOTTOM) == AnchorTypeEnum.ANCHOR_SCALE_BOTTOM) {
						HithermRegister.HithermRegisterTypeEnum tmpType = this.register.RegisterType;
						double tmpY = this.register.GraphPosY;
						Nullable<HithermRegister.HithermRegisterTypeEnum> newType = HithermRegister.GetRegisterTypeForHoehe((int)(this.startDragRegisterHeight - planPoint.Y + this.startDrag.Value.Y + 25), this.register.IsHochleistungsRegister);
						this.register.RegisterType = newType.HasValue ? newType.Value : HithermRegister.GetRegisterTypeForHoehe(50, this.register.IsHochleistungsRegister).Value;
						this.register.GraphPosY = this.startDragRegisterY + this.startDragRegisterHeight - this.register.RegisterHoehe;
						if (!this.PositionAndSizeOk(owningWall, 0, 0)) {
							this.register.RegisterType = tmpType;
							this.register.GraphPosY = tmpY;
						}
					}
				}
			} else {
				// TODO
			}

			// TODO
			return true;
		}

		public override bool EndDrag(Anchor anchor, Point2D planPoint, GraphicalWall owningWall) {
			// TODO
			this.startDrag = null;
			return false;
		}

		public bool PositionAndSizeOk(GraphicalWall owningWall, double offsetX, double offsetY) {
			Polygon2D registerBorders = this.GetObjectBorders(offsetX, offsetY);
			if (owningWall.CollisionTest(registerBorders, offsetX, offsetY)) {
				return false;
			} else {
				foreach (GraphicalHithermRegisterWrapper register in owningWall.Registers) {
					if (register != this && register.CollisionTest(registerBorders, offsetX, offsetY)) {
						return false;
					}
				}
			}
			return true;
		}

		public double X {
			get { return this.register.GraphPosX; }
		}

		public double Y {
			get { return this.register.GraphPosY; }
		}

		public double Height {
			get { return this.register.Orientation == HithermRegister.RegisterOrientationEnum.ORIENTATION_VERTIKAL ? this.register.RegisterHoehe : this.register.RegisterBreiteForDrawing; }
		}

		public double Width {
			get { return this.register.Orientation == HithermRegister.RegisterOrientationEnum.ORIENTATION_VERTIKAL ? this.register.RegisterBreiteForDrawing : this.register.RegisterHoehe; }
		}
	}
}
