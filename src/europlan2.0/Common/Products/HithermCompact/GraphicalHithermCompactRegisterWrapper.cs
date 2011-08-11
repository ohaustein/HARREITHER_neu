using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using WW.Math.Geometry;
using WW.Math;
using System.Drawing.Drawing2D;
using System.Xml.Serialization;

namespace Europlan.Common {
	public class GraphicalHithermCompactRegisterWrapper : GraphicalRegisterWrapper {
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

		public override bool HitTest(WW.Math.Point2D planPoint, double xOffset, double yOffset) {
			return this.GetObjectBorders(xOffset, yOffset).IsInside(planPoint);
		}

		public override WW.Math.Geometry.Polygon2D GetObjectBorders(double xOffset, double yOffset) {
			try {
				Polygon2D borders = new Polygon2D();
				if (this.register.IsParapet) {
					borders.Add(new Point2D(xOffset + this.register.GraphPosX, yOffset + this.register.GraphPosY));
					borders.Add(new Point2D(xOffset + this.register.GraphPosX, yOffset + this.register.GraphPosY + this.Height));
					borders.Add(new Point2D(xOffset + this.register.GraphPosY + this.Width, yOffset + this.register.GraphPosY + this.Height));
					borders.Add(new Point2D(xOffset + this.register.GraphPosY + this.Width, yOffset + this.register.GraphPosY));
				} else {
					// TODO: check if dachschraegen register are different!
					borders.Add(new Point2D(xOffset + this.register.GraphPosX, yOffset + this.register.GraphPosY));
					borders.Add(new Point2D(xOffset + this.register.GraphPosX, yOffset + this.register.GraphPosY + this.Height + 6.5 + 1));
					borders.Add(new Point2D(xOffset + this.register.GraphPosX + this.Width, yOffset + this.register.GraphPosY + this.Height + 6.5 + 1));
					borders.Add(new Point2D(xOffset + this.register.GraphPosX + this.Width, yOffset + this.register.GraphPosY));
				}
				return borders;
			} catch (Exception e) {
				return new Polygon2D();
			}
		}

		public override void PaintObject(System.Drawing.Graphics g, double xOffset, double yOffset, IGraphicalWallObject selectedObject, double scale, bool export) {
			this.PaintObject(g, xOffset, yOffset, (this == selectedObject) ? Color.Red : Color.Black, scale, false/*, this == selectedObject*/);
		}

		public void PaintObject(System.Drawing.Graphics g, double xOffset, double yOffset, Color color) {
			this.PaintObject(g, xOffset, yOffset, color, 1, false/*, false*/);
		}

		private const float parapet_distLeft = 3.5f;
		private const float parapet_distRight = 3.5f;
		private const float parapet_distPipeToRegister = 7.5f;
		private const float parapet_pipeWidth = 2.0f;
		private const float parapet_registerPipeWidth = 2.0f;
		private const float parapet_distBottomLeftRegisterPipe = 2.5f;
		private const float parapet_distTopPipe = 2.5f;

		public void PaintObject(System.Drawing.Graphics g, double xOffset, double yOffset, Color color, double scale, bool error/*, bool drawAnchors*/) {
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
			if (register.IsParapet) {
				float x1 = (float)(xOffset + register.GraphPosX + parapet_distLeft + parapet_pipeWidth + parapet_distPipeToRegister);
				float x2 = (float)(xOffset + register.GraphPosX + this.Width - parapet_distRight - parapet_registerPipeWidth);
				float y = (float)(yOffset + register.GraphPosY);
				float hoehe = 55;
				g.DrawRectangle(registerPen, x1, y + parapet_distBottomLeftRegisterPipe, parapet_registerPipeWidth, hoehe - parapet_distBottomLeftRegisterPipe);
				g.DrawRectangle(registerPen, x2, y, parapet_registerPipeWidth, hoehe);
				x1 = (float)(xOffset + register.GraphPosX + parapet_distLeft + parapet_pipeWidth + parapet_distPipeToRegister + parapet_registerPipeWidth / 2.0f);
				x2 = (float)(xOffset + register.GraphPosX + this.Width - parapet_distRight - parapet_registerPipeWidth / 2.0);
				for (int pos = 5; pos <= 50; pos += 5) {
					y = (float)(yOffset + register.GraphPosY + pos);
					g.DrawLine(registerPen, x1, y, x2, y);
				}
				connectionPen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
				connectionPen.StartCap = LineCap.Flat;
				float x = (float)(xOffset + register.GraphPosX + parapet_distLeft + parapet_pipeWidth / 2.0);
				float y1 = (float)(yOffset + register.GraphPosY + parapet_pipeWidth);
				float y2 = (float)(yOffset + register.GraphPosY + this.Height - parapet_distTopPipe - parapet_pipeWidth / 2.0);
				g.DrawLine(connectionPen, x, y1, x, y2);
				connectionPen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
				x1 = x;
				x2 = x + parapet_distPipeToRegister + parapet_pipeWidth;
				y = y2;
				g.DrawLine(connectionPen, x1, y, x2, y);
				connectionPen.EndCap = System.Drawing.Drawing2D.LineCap.Flat;
				x = x2;
				y1 = y;
				y2 = (float)(yOffset + register.GraphPosY + 55);
				g.DrawLine(connectionPen, x, y1, x, y2);
				g.DrawRectangle(plattePen, (float)(xOffset + register.GraphPosX), (float)(yOffset + register.GraphPosY), (float)this.Width, (float)this.Height);
				g.FillRectangle(bInput, (float)(xOffset + register.GraphPosX + this.Width - parapet_distRight - parapet_pipeWidth), (float)(yOffset + register.GraphPosY), parapet_pipeWidth, parapet_pipeWidth);
				g.DrawRectangle(pInput, (float)(xOffset + register.GraphPosX + this.Width - parapet_distRight - parapet_pipeWidth), (float)(yOffset + register.GraphPosY), parapet_pipeWidth, parapet_pipeWidth);
				g.FillRectangle(bOutput, (float)(xOffset + register.GraphPosX + parapet_distLeft), (float)(yOffset + register.GraphPosY), parapet_pipeWidth, parapet_pipeWidth);
				g.DrawRectangle(pOutput, (float)(xOffset + register.GraphPosX + parapet_distLeft), (float)(yOffset + register.GraphPosY), parapet_pipeWidth, parapet_pipeWidth);
			} else {
				throw new Exception("TODO");
			}
#if BLUB
			if (register.Orientation == HithermRegister.RegisterOrientationEnum.ORIENTATION_VERTIKAL) {
				float x = (float)(xOffset + register.GraphPosX);
				float y1 = (float)(yOffset + register.GraphPosY);
				float y2 = (float)(yOffset + register.GraphPosY + register.RegisterHoehe - 2);
				float breite = (float)register.RegisterBreiteForDrawing;
				g.DrawRectangle(registerPen, x, y1, breite, 2);
				g.DrawRectangle(registerPen, x, y2, breite, 2);
				double pos = 5;
				for (int i = 0; i < register.Rohre; i++) {
					if (register.Gaps.ContainsKey(i)) {
						pos += register.Gaps[i];
					}
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
#endif
		}

		/*public double GetRohrOffset(int rohrNr) {
			double offset = 5;
			for (int i = 1; i < rohrNr; i++) {
				if (register.Rohrabstand == HithermRegister.RohrabstandEnum.RC_HOCHLEISTUNG) {
					if (HithermProduct.ConfigUsePlus) {
						if (i % 14 == 13) {
							offset += 10;
						} else {
							offset += 5;
						}
					} else {
						if (i % 9 == 8) {
							offset += 10;
						} else {
							offset += 5;
						}
					}
				} else {
					offset += 10;
				}
			}
			return offset;
		}*/

		public override IGraphicalWallObject GetPickedObject(WW.Math.Point2D planPoint, double xOffset, double yOffset) {
			if (HitTest(planPoint, xOffset, yOffset)) {
				return this;
			}
			return null;
		}

		public Point2D GetOutputConnectionPoint(double xOffset, double yOffset, double dist) {
			if (this.register.IsParapet) {
				return new Point2D(xOffset + this.register.GraphPosX + parapet_distLeft + parapet_pipeWidth / 2.0, yOffset + this.register.GraphPosY - dist);
			} else {
				throw new Exception("TODO");
			}
#if BLUB
			if (this.register.Orientation == HithermRegister.RegisterOrientationEnum.ORIENTATION_VERTIKAL) {
				if (this.register.GraphVorlaufRight) {
					return new Point2D(xOffset + this.register.GraphPosX - dist, yOffset + this.register.GraphPosY + this.Height - 1);
				} else {
					return new Point2D(xOffset + this.register.GraphPosX + this.Width + dist, yOffset + this.register.GraphPosY + this.Height - 1);
				}
			} else {
				if (this.register.GraphVorlaufRight) {
					return new Point2D(xOffset + this.register.GraphPosX + 1, yOffset + this.register.GraphPosY + this.Height + dist);
				} else {
					return new Point2D(xOffset + this.register.GraphPosX + this.Width - 1, yOffset + this.register.GraphPosY + this.Height + dist);
				}
			}
#endif
		}

		private double connectionSize = 7;

		public Polygon2D GetOutputConnectionArea(double xOffset, double yOffset) {
			Polygon2D area = new Polygon2D();
			Point2D connectionPoint = this.GetOutputConnectionPoint(xOffset, yOffset, 0);
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
				throw new Exception("TODO");
			}
#if BLUB
			if (this.register.Orientation == HithermRegister.RegisterOrientationEnum.ORIENTATION_VERTIKAL) {
				if (this.register.GraphVorlaufRight) {
					return new Point2D(xOffset + this.register.GraphPosX + this.Width + dist, yOffset + this.register.GraphPosY + 1);
				} else {
					return new Point2D(xOffset + this.register.GraphPosX - dist, yOffset + this.register.GraphPosY + 1);
				}
			} else {
				if (this.register.GraphVorlaufRight) {
					return new Point2D(xOffset + this.register.GraphPosX + this.Width - 1, yOffset + this.register.GraphPosY - dist);
				} else {
					return new Point2D(xOffset + this.register.GraphPosX + 1, yOffset + this.register.GraphPosY - dist);
				}
			}
#endif
		}

		public Polygon2D GetInputConnectionArea(double xOffset, double yOffset) {
			Polygon2D area = new Polygon2D();
			Point2D connectionPoint = this.GetInputConnectionPoint(xOffset, yOffset, 0);
			area.Add(new Point2D(connectionPoint.X - connectionSize / 2.0, connectionPoint.Y - connectionSize / 2.0));
			area.Add(new Point2D(connectionPoint.X - connectionSize / 2.0, connectionPoint.Y + connectionSize / 2.0));
			area.Add(new Point2D(connectionPoint.X + connectionSize / 2.0, connectionPoint.Y + connectionSize / 2.0));
			area.Add(new Point2D(connectionPoint.X + connectionSize / 2.0, connectionPoint.Y - connectionSize / 2.0));
			return area;
		}

		public PossibleHithermCompactRegisterConnection GetOutputConnection(double xOffset, double yOffset, HithermCompactProduct product, HithermCompactCircuit circuit) {
			return new PossibleHithermCompactRegisterConnection(this.GetOutputConnectionPoint(xOffset, yOffset, 0), GetOutputConnectionArea(xOffset, yOffset), false, true, product, circuit, this.register, false, true, new Vector2D(0, -5));
		}

		public PossibleHithermCompactRegisterConnection GetInputConnection(double xOffset, double yOffset, HithermCompactProduct product, HithermCompactCircuit circuit) {
			return new PossibleHithermCompactRegisterConnection(this.GetInputConnectionPoint(xOffset, yOffset, 0), GetInputConnectionArea(xOffset, yOffset), true, false, product, circuit, this.register, false, true, new Vector2D(0, -5));
		}

		public override bool CollisionTest(Polygon2D polygon, double xOffset, double yOffset, bool ignoreBorders) {
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

			try {
				return Polygon2D.GetIntersection(list1, list2).Count > 0;
			} catch {
				return true;
			}
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

		/*private int GetBestRohrCount(double width) {
			int bestRohre = 3;
			double bestDelta = double.MaxValue;
			foreach (KeyValuePair<int, double> kvp in this.register.PossibleWidths) {
				if (Math.Abs(kvp.Value - width + register.GapsSum) < bestDelta) {
					bestDelta = Math.Abs(kvp.Value - width + register.GapsSum);
					bestRohre = kvp.Key;
				}
			}
			if (bestRohre < register.LastGap + 1) {
				bestRohre = register.LastGap + 1;
			}
			return bestRohre;
		}*/

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
			// TODO
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
#if BLUB
			if (this.register.Orientation == HithermRegister.RegisterOrientationEnum.ORIENTATION_VERTIKAL) {
				if (anchor == null) {
					// move
					this.UpdatePosition(owningWall, startDragRegisterX + planPoint.X - startDrag.Value.X, startDragRegisterY + planPoint.Y - startDrag.Value.Y, checkLinks, useSnap);
				} else if (anchor is RegisterGapAnchor) {
					RegisterGapAnchor rga = anchor as RegisterGapAnchor;
					if (rga.Left) {
						double newGap = this.startGaps[rga.GapNr] - planPoint.X + startDrag.Value.X;
						if (newGap < 0) {
							newGap = 0;
						}
						this.UpdateGap(owningWall, rga.GapNr, newGap, startDragRegisterX + planPoint.X - startDrag.Value.X, startDragRegisterY, true, useSnap);
						//this.register.Gaps[rga.GapNr] = newGap;
						//this.UpdatePosition(owningWall, startDragRegisterX + planPoint.X - startDrag.Value.X, startDragRegisterY, true, useSnap);
					} else {
						double newGap = this.startGaps[rga.GapNr] + planPoint.X - startDrag.Value.X;
						if (newGap < 0) {
							newGap = 0;
						}
						this.UpdateGap(owningWall, rga.GapNr, newGap, startDragRegisterX, startDragRegisterY, true, useSnap);
						//this.register.Gaps[rga.GapNr] = newGap;
						//this.UpdatePosition(owningWall, startDragRegisterX, startDragRegisterY, true, useSnap);
					}
					// TODO
				} else if (anchor is Anchor) {
					if ((anchor.AnchorType & AnchorTypeEnum.ANCHOR_SCALE_LEFT) == AnchorTypeEnum.ANCHOR_SCALE_LEFT) {
						this.UpdateRohre(owningWall, GetBestRohrCount(this.startDragRegisterWidth - planPoint.X + startDrag.Value.X), false, checkLinks, useSnap);
					} else if ((anchor.AnchorType & AnchorTypeEnum.ANCHOR_SCALE_RIGHT) == AnchorTypeEnum.ANCHOR_SCALE_RIGHT) {
						this.UpdateRohre(owningWall, GetBestRohrCount(this.startDragRegisterWidth + planPoint.X - startDrag.Value.X), true, checkLinks, useSnap);
					}

					if ((anchor.AnchorType & AnchorTypeEnum.ANCHOR_SCALE_TOP) == AnchorTypeEnum.ANCHOR_SCALE_TOP) {
						this.UpdateType(owningWall, HithermRegister.GetRegisterTypeForHoehe((int)(this.startDragRegisterHeight + planPoint.Y - this.startDrag.Value.Y + 25), this.register.IsHochleistungsRegister, false).Value, true, checkLinks, useSnap);
					} else if ((anchor.AnchorType & AnchorTypeEnum.ANCHOR_SCALE_BOTTOM) == AnchorTypeEnum.ANCHOR_SCALE_BOTTOM) {
						this.UpdateType(owningWall, HithermRegister.GetRegisterTypeForHoehe((int)(this.startDragRegisterHeight - planPoint.Y + this.startDrag.Value.Y + 25), this.register.IsHochleistungsRegister, false).Value, false, checkLinks, useSnap);
					}
				}
			} else {
				if (anchor == null) {
					// move
					this.UpdatePosition(owningWall, startDragRegisterX + planPoint.X - startDrag.Value.X, startDragRegisterY + planPoint.Y - startDrag.Value.Y, true, useSnap);
				} else if (anchor is RegisterGapAnchor) {
					// TODO
				} else if (anchor is Anchor) {
					if ((anchor.AnchorType & AnchorTypeEnum.ANCHOR_SCALE_LEFT) == AnchorTypeEnum.ANCHOR_SCALE_LEFT) {
						this.UpdateType(owningWall, HithermRegister.GetRegisterTypeForHoehe((int)(this.startDragRegisterHeight - planPoint.X + this.startDrag.Value.X + 25), this.register.IsHochleistungsRegister, false).Value, false, checkLinks, useSnap);
					} else if ((anchor.AnchorType & AnchorTypeEnum.ANCHOR_SCALE_RIGHT) == AnchorTypeEnum.ANCHOR_SCALE_RIGHT) {
						this.UpdateType(owningWall, HithermRegister.GetRegisterTypeForHoehe((int)(this.startDragRegisterHeight + planPoint.X - this.startDrag.Value.X + 25), this.register.IsHochleistungsRegister, false).Value, true, checkLinks, useSnap);
					}

					if ((anchor.AnchorType & AnchorTypeEnum.ANCHOR_SCALE_TOP) == AnchorTypeEnum.ANCHOR_SCALE_TOP) {
						this.UpdateRohre(owningWall, GetBestRohrCount(this.startDragRegisterWidth + planPoint.Y - startDrag.Value.Y), true, checkLinks, useSnap);
					} else if ((anchor.AnchorType & AnchorTypeEnum.ANCHOR_SCALE_BOTTOM) == AnchorTypeEnum.ANCHOR_SCALE_BOTTOM) {
						this.UpdateRohre(owningWall, GetBestRohrCount(this.startDragRegisterWidth - planPoint.Y + startDrag.Value.Y), false, checkLinks, useSnap);
					}
				}
			}
#endif
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
			Polygon2D registerBorders = this.GetObjectBorders(offsetX, offsetY);
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

		public bool UpdateRohre(GraphicalWall owningWall, int newRohre, bool anchorStart, bool updateLinks, bool snapEnabled) {
			return this.UpdatePositionAndSize(owningWall, null, null, newRohre, anchorStart, null, null, updateLinks, snapEnabled);
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
							this.register.GraphPosY = this.register.GraphPosY + (oldHoehe - this.Width);
						}
					}
				}

				ok = this.CheckValidity(owningWall, offset.X, offset.Y);
				if (!ok) {
					this.register.GraphPosX = oldPosX;
					this.register.GraphPosY = oldPosY;
					this.register.RegisterCount = oldRegisterCount;
					this.register.RegisterType = oldType;
				} else {
					// TODO move Verbindeleitungen
				}
			}

			HithermCompactCircuit circuit = product.GetCircuitForRegister(this.register);
			foreach (GraphicalHithermCompactVerbindung link in circuit.Links) {
				if (link.Start == this.register) {
					link.RevertState();
					link.UpdateStartPoint(this, owningWall, checkLinks); // TODO
				}
				if (link.End == this.register) {
					link.RevertState();
					link.UpdateEndPoint(this, owningWall, checkLinks); // TODO
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
			get { return this.register.RegisterBreite / 10.0; }
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
