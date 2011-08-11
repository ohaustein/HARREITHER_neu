using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using WW.Math.Geometry;
using WW.Math;
using System.Drawing.Drawing2D;
using System.Xml.Serialization;

namespace Europlan.Common {
	public class GraphicalHithermRegisterWrapper : GraphicalRegisterWrapper {
		public class RegisterGapAnchor : Anchor {
			private int gapNr = 0;
			private bool left = true;

			public RegisterGapAnchor(double x, double y, AnchorTypeEnum anchorType, IGraphicalWallObject owner, int gapNr, bool left)
				: base(x, y, anchorType, owner) {
				this.gapNr = gapNr;
				this.left = left;
			}

			public RegisterGapAnchor(Point2D position, AnchorTypeEnum anchorType, IGraphicalWallObject owner, int gapNr, bool left)
				: base(position, anchorType, owner) {
				this.gapNr = gapNr;
				this.left = left;
			}

			public int GapNr {
				get { return this.gapNr; }
			}

			public bool Left {
				get { return this.left; }
			}

			public override void PaintAnchor(Graphics g, double xOffset, double yOffset, double scale) {
				//Brush b = new SolidBrush(Color.DarkBlue);
				Brush b = new SolidBrush(Color.Gray);
				g.FillRectangle(b, (float)(this.position.X + xOffset - 3.0 / scale), (float)(this.position.Y + yOffset - 3.0 / scale), (float)(2.0 / scale), (float)(6.0 / scale));
				g.FillRectangle(b, (float)(this.position.X + xOffset + 1.0 / scale), (float)(this.position.Y + yOffset - 3.0 / scale), (float)(2.0 / scale), (float)(6.0 / scale));
			}
		}


		private HithermRegister register;
		//private List<HithermRegister> registers = new List<HithermRegister>();
		private HithermProduct product;

		public GraphicalHithermRegisterWrapper(HithermProduct product) {
			this.product = product;
		}

		public GraphicalHithermRegisterWrapper(HithermRegister register, HithermProduct product) {
			this.register = register;
			//this.registers.Add(register);
			this.product = product;
		}

		/*public List<HithermRegister> Registers {
			get { return this.registers; }
			set { this.registers = value; }
		}*/

		public HithermRegister Register {
			get { 
				return this.register;
			}
			set {
				this.register = value;
				if (this.register != null) {
					this.register.OnlyCompleteRegisters = this.tmpOnlyCompleteRegisters;
				}
			}
		}

		/*public int RegisterCount {
			get {
				int count = 0;
				foreach (HithermRegister register in hithermRegister.Registers) {
					count += register.RegisterCount;
				}
				return count;
			}
		}

		public bool IsHochleistungsRegister {
			get { return (this.registers != null && this.registers.Count > 0) ? this.registers[0].IsHochleistungsRegister : false; }
		}*/

		public HithermProduct Product {
			get { return this.product; }
		}

		public override bool HitTest(WW.Math.Point2D planPoint, double xOffset, double yOffset) {
			return this.GetObjectBorders(xOffset, yOffset).IsInside(planPoint);
		}

		public override WW.Math.Geometry.Polygon2D GetObjectBorders(double xOffset, double yOffset) {
			try {
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
			} catch (Exception e) {
				return new Polygon2D();
			}
		}

		public override void PaintObject(System.Drawing.Graphics g, double xOffset, double yOffset, IGraphicalWallObject selectedObject, double scale, bool export) {
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
			Brush bInput = new SolidBrush(Color.FromArgb(127, Color.Red));
			Pen pInput = new Pen(Color.Red, (float)(1.0 / scale));
			Brush bOutput = new SolidBrush(Color.FromArgb(127, Color.Blue));
			Pen pOutput = new Pen(Color.Blue, (float)(1.0 / scale));
			if (error || this.error) {
				registerPen.DashPattern = new float[] { 1, 2 };
				pInput.DashPattern = new float[] { 1, 2 };
				pOutput.DashPattern = new float[] { 1, 2 };
				bInput = new SolidBrush(Color.FromArgb(63, Color.Red));
				bOutput = new SolidBrush(Color.FromArgb(63, Color.Blue));
			}
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
		}

		public double GetRohrOffset(int rohrNr) {
			double offset = 5;
			for (int i = 1; i < rohrNr; i++) {
				if (register.Gaps.ContainsKey(i)) {
					offset += register.Gaps[i];
				}
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
		}

		public override IGraphicalWallObject GetPickedObject(WW.Math.Point2D planPoint, double xOffset, double yOffset) {
			if (HitTest(planPoint, xOffset, yOffset)) {
				return this;
			}
			return null;
		}

		public Point2D GetOutputConnectionPoint(double xOffset, double yOffset, double dist) {
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
		}

		private double connectionSize = 7;

		public Polygon2D GetOutputConnectionArea(double xOffset, double yOffset) {
			Polygon2D area = new Polygon2D();
			if (this.register.GraphVorlaufRight) {
				area.Add(new Point2D(xOffset + this.register.GraphPosX - connectionSize / 2.0 + 1.0, yOffset + register.GraphPosY + this.Height - connectionSize / 2.0 - 1.0));
				area.Add(new Point2D(xOffset + this.register.GraphPosX - connectionSize / 2.0 + 1.0, yOffset + register.GraphPosY + this.Height + connectionSize / 2.0 - 1.0));
				area.Add(new Point2D(xOffset + this.register.GraphPosX + connectionSize / 2.0 + 1.0, yOffset + register.GraphPosY + this.Height + connectionSize / 2.0 - 1.0));
				area.Add(new Point2D(xOffset + this.register.GraphPosX + connectionSize / 2.0 + 1.0, yOffset + register.GraphPosY + this.Height - connectionSize / 2.0 - 1.0));
			} else {
				area.Add(new Point2D(xOffset + this.register.GraphPosX + this.Width - connectionSize / 2.0 - 1.0, yOffset + register.GraphPosY + this.Height - connectionSize / 2.0 - 1.0));
				area.Add(new Point2D(xOffset + this.register.GraphPosX + this.Width - connectionSize / 2.0 - 1.0, yOffset + register.GraphPosY + this.Height + connectionSize / 2.0 - 1.0));
				area.Add(new Point2D(xOffset + this.register.GraphPosX + this.Width + connectionSize / 2.0 - 1.0, yOffset + register.GraphPosY + this.Height + connectionSize / 2.0 - 1.0));
				area.Add(new Point2D(xOffset + this.register.GraphPosX + this.Width + connectionSize / 2.0 - 1.0, yOffset + register.GraphPosY + this.Height - connectionSize / 2.0 - 1.0));
			}
			return area;
		}

		public Point2D GetInputConnectionPoint(double xOffset, double yOffset, double dist) {
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
		}

		public Polygon2D GetInputConnectionArea(double xOffset, double yOffset) {
			Polygon2D area = new Polygon2D();
			if (this.register.GraphVorlaufRight) {
				area.Add(new Point2D(xOffset + this.register.GraphPosX + this.Width - connectionSize / 2 - 1.0, yOffset + register.GraphPosY - connectionSize / 2 + 1.0));
				area.Add(new Point2D(xOffset + this.register.GraphPosX + this.Width - connectionSize / 2 - 1.0, yOffset + register.GraphPosY + connectionSize / 2 + 1.0));
				area.Add(new Point2D(xOffset + this.register.GraphPosX + this.Width + connectionSize / 2 - 1.0, yOffset + register.GraphPosY + connectionSize / 2 + 1.0));
				area.Add(new Point2D(xOffset + this.register.GraphPosX + this.Width + connectionSize / 2 - 1.0, yOffset + register.GraphPosY - connectionSize / 2 + 1.0));
			} else {
				area.Add(new Point2D(xOffset + this.register.GraphPosX - connectionSize / 2 + 1.0, yOffset + register.GraphPosY - connectionSize / 2 + 1.0));
				area.Add(new Point2D(xOffset + this.register.GraphPosX - connectionSize / 2 + 1.0, yOffset + register.GraphPosY + connectionSize / 2 + 1.0));
				area.Add(new Point2D(xOffset + this.register.GraphPosX + connectionSize / 2 + 1.0, yOffset + register.GraphPosY + connectionSize / 2 + 1.0));
				area.Add(new Point2D(xOffset + this.register.GraphPosX + connectionSize / 2 + 1.0, yOffset + register.GraphPosY - connectionSize / 2 + 1.0));
			}
			return area;
		}

		public PossibleHithermRegisterConnection GetOutputConnection(double xOffset, double yOffset, HithermProduct product, HithermCircuit circuit) {
			return new PossibleHithermRegisterConnection(this.GetOutputConnectionPoint(xOffset, yOffset, 0), GetOutputConnectionArea(xOffset, yOffset), false, true, product, circuit, this.register, this.register.Orientation == HithermRegister.RegisterOrientationEnum.ORIENTATION_VERTIKAL, this.register.Orientation == HithermRegister.RegisterOrientationEnum.ORIENTATION_HORIZONTAL, this.register.Orientation == HithermRegister.RegisterOrientationEnum.ORIENTATION_VERTIKAL ? new Vector2D(this.register.GraphVorlaufRight ? -5 : 5, 0) : new Vector2D(0, 5));
		}

		public PossibleHithermRegisterConnection GetInputConnection(double xOffset, double yOffset, HithermProduct product, HithermCircuit circuit) {
			return new PossibleHithermRegisterConnection(this.GetInputConnectionPoint(xOffset, yOffset, 0), GetInputConnectionArea(xOffset, yOffset), true, false, product, circuit, this.register, this.register.Orientation == HithermRegister.RegisterOrientationEnum.ORIENTATION_VERTIKAL, this.register.Orientation == HithermRegister.RegisterOrientationEnum.ORIENTATION_HORIZONTAL, this.register.Orientation == HithermRegister.RegisterOrientationEnum.ORIENTATION_VERTIKAL ? new Vector2D(this.register.GraphVorlaufRight ? 5 : -5, 0) : new Vector2D(0, -5));
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
			foreach (KeyValuePair<int, double> gap in this.register.Gaps) {
				anchors.Add(new RegisterGapAnchor(this.X + this.GetRohrOffset(gap.Key), this.Y + this.Height / 2.0, AnchorTypeEnum.ANCHOR_SCALE_LEFT | AnchorTypeEnum.ANCHOR_SCALE_RIGHT, this, gap.Key, true));
				anchors.Add(new RegisterGapAnchor(this.X + this.GetRohrOffset(gap.Key + 1), this.Y + this.Height / 2.0, AnchorTypeEnum.ANCHOR_SCALE_LEFT | AnchorTypeEnum.ANCHOR_SCALE_RIGHT, this, gap.Key, false));
			}
			return anchors;
		}

		private int GetBestRohrCount(double width) {
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
		}

		public GraphicalHithermVerbindung GetInputLink() {
			HithermCircuit c = this.product.GetCircuitForRegister(this.register);
			foreach (GraphicalHithermVerbindung link in c.Links) {
				if (link.End == this.register) {
					return link;
				}
			}
			return null;
		}

		public GraphicalHithermVerbindung GetOutputLink() {
			HithermCircuit c = this.product.GetCircuitForRegister(this.register);
			foreach (GraphicalHithermVerbindung link in c.Links) {
				if (link.Start == this.register) {
					return link;
				}
			}
			return null;
		}

		private Nullable<Point2D> startDrag = null;
		private double startDragRegisterX, startDragRegisterY, startDragRegisterWidth;
		private int startDragRegisterHeight, startRegisterRohre;
		private List<Point2D> startInputConnectionVertices, startOutputConnectionVertices;
		private GraphicalHithermVerbindung startInputConnection, startOutputConnection;
		private Dictionary<int, double> startGaps;

		public override bool StartDrag(Anchor anchor, Point2D planPoint, GraphicalWall owningWall, Room owningRoom, Product owningProduct, bool useSnap) {
			this.startDrag = planPoint;
			this.startDragRegisterX = this.register.GraphPosX;
			this.startDragRegisterY = this.register.GraphPosY;
			this.startDragRegisterHeight = this.register.RegisterHoehe;
			this.startDragRegisterWidth = this.register.RegisterBreiteForDrawing;
			this.startRegisterRohre = this.register.Rohre;
			this.startGaps = new Dictionary<int, double>(this.register.Gaps);

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
			return true;
		}

		public override bool EndDrag(Anchor anchor, Point2D planPoint, GraphicalWall owningWall, Room owningRoom, Product owningProduct, bool useSnap) {
			owningRoom.MarkErrors(this, owningWall);
			HithermCircuit circuit = product.GetCircuitForRegister(this.register);
			foreach (GraphicalHithermVerbindung link in circuit.Links) {
				if (link.Start == this.register) {
					link.Error = !link.CheckValidity(null, 0, 0);
				}
				if (link.End == this.register) {
					link.Error = !link.CheckValidity(null, 0, 0);
				}
			}

			this.startDrag = null;
			return true;

			//this.MoveAnchor(anchor, planPoint, owningWall, owningRoom, true);
			/*HithermCircuit circuit = product.GetCircuitForRegister(this.register);
			List<GraphicalHithermVerbindung> linksToDel = new List<GraphicalHithermVerbindung>();
			foreach (GraphicalHithermVerbindung link in circuit.Links) {
				if (link.Start == this.register && link.Vertices.Count == 0) {
					linksToDel.Add(link);
				} else if (link.End == this.register && link.Vertices.Count == 0) {
					linksToDel.Add(link);
				}
			}
			foreach (GraphicalHithermVerbindung link in linksToDel) {
				circuit.Links.Remove(link);
			}

			this.startDrag = null;
			return linksToDel.Count > 0;*/
		}

		public override bool CheckValidity(GraphicalWall owningWall, double offsetX, double offsetY) {
			Polygon2D registerBorders = this.GetObjectBorders(offsetX, offsetY);
			if (owningWall.CollisionTest(registerBorders, offsetX, offsetY, false)) {
				return false;
			} else {
				foreach (GraphicalHithermRegisterWrapper register in owningWall.Registers) {
					if (register != this && register.CollisionTest(registerBorders, offsetX, offsetY, false)) {
						return false;
					}
				}
				foreach (GraphicalWallObstacle obstacle in owningWall.Obstacles) {
					if (obstacle.CollisionTest(registerBorders, offsetX, offsetY, false)) {
						return false;
					}
				}
				/*foreach (HithermCircuit hc in this.product.PlannedCircuits) {
					foreach (GraphicalHithermVerbindung link in hc.Links) {
						if (link.Start != this.Register && link.End != this.Register && link.CollisionTest(registerBorders, 0, 0, false)) {
							return false;
						}
					}
				}*/
			}
			return true;
		}

		public bool UpdatePosition(GraphicalWall owningWall, double newPosX, double newPosY, bool updateLinks, bool snapEnabled) {
			return this.UpdatePositionAndSizeAndGap(owningWall, newPosX, newPosY, null, null, null, null, null, null, updateLinks, snapEnabled);
		}

		public bool UpdateRohre(GraphicalWall owningWall, int newRohre, bool anchorStart, bool updateLinks, bool snapEnabled) {
			return this.UpdatePositionAndSizeAndGap(owningWall, null, null, newRohre, anchorStart, null, null, null, null, updateLinks, snapEnabled);
		}

		public bool UpdateType(GraphicalWall owningWall, HithermRegister.HithermRegisterTypeEnum newType, bool anchorStart, bool updateLinks, bool snapEnabled) {
			return this.UpdatePositionAndSizeAndGap(owningWall, null, null, null, null, newType, null, null, anchorStart, updateLinks, snapEnabled);
		}

		public bool UpdateGap(GraphicalWall owningWall, int gapNr, double gapWidth, double newXPos, double newYPos, bool updateLinks, bool snapEnabled) {
			return this.UpdatePositionAndSizeAndGap(owningWall, newXPos, newYPos, null, null, null, gapNr, gapWidth, null, updateLinks, snapEnabled);
		}

		public bool UpdatePositionAndSizeAndGap(GraphicalWall owningWall, Nullable<double> newPosX, Nullable<double> newPosY, Nullable<int> newRohre, Nullable<bool> anchorRohreStart, Nullable<HithermRegister.HithermRegisterTypeEnum> newType, Nullable<int> setGap, Nullable<double> setGapTo, Nullable<bool> anchorTypeStart, bool checkLinks, bool useSnap) {
			double oldPosX = this.register.GraphPosX;
			double oldPosY = this.register.GraphPosY;
			double oldWidth = this.register.RegisterBreiteForDrawing;
			double oldHeight = this.register.RegisterHoehe;
			int oldRohre = this.register.Rohre;
			Dictionary<int, double> oldGaps = new Dictionary<int,double>(this.register.Gaps);
			HithermRegister.HithermRegisterTypeEnum oldType = this.register.RegisterType;

			Vector2D offset = this.product.AssociatedRoom.GetWallOffset(owningWall).Value * 100;
			bool ok = false;
			if (setGap.HasValue) {
				this.register.Gaps[setGap.Value] = setGapTo.Value;
				if (newPosX.HasValue) {
					this.register.GraphPosX = newPosX.Value;
				}
				if (newPosY.HasValue) {
					this.register.GraphPosY = newPosY.Value;
				}
				ok = true;
				if (!this.CheckValidity(owningWall, offset.X, offset.Y)) {
					this.register.GraphPosX = oldPosX;
					this.register.GraphPosY = oldPosY;
					this.register.Gaps = oldGaps;
					ok = false;
				}
			} else if (newPosX.HasValue && newPosY.HasValue && !newRohre.HasValue && !newType.HasValue) {
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
				if (newRohre.HasValue) {
					double oldBreite = this.register.RegisterBreiteForDrawing;
					this.register.Rohre = newRohre.Value;
					if (anchorRohreStart.HasValue && !anchorRohreStart.Value) {
						/*foreach (int i in this.register.Gaps.Keys) {
							this.register.Gaps[i] += this.register.Rohre - oldRohre;
						}*/
						this.register.Gaps = new Dictionary<int, double>();
						foreach (KeyValuePair<int, double> kvp in oldGaps) {
							this.register.Gaps.Add(kvp.Key + this.register.Rohre - oldRohre, kvp.Value);
						}
						if (this.register.Orientation == HithermRegister.RegisterOrientationEnum.ORIENTATION_VERTIKAL) {
							this.register.GraphPosX = this.register.GraphPosX + oldBreite - this.register.RegisterBreiteForDrawing;
						} else {
							this.register.GraphPosY = this.register.GraphPosY + oldBreite - this.register.RegisterBreiteForDrawing;
						}
					}
				}
				if (newType.HasValue) {
					double oldHoehe = this.register.RegisterHoehe;
					this.register.RegisterType = newType.Value;
					if (anchorTypeStart.HasValue && !anchorTypeStart.Value) {
						if (this.register.Orientation == HithermRegister.RegisterOrientationEnum.ORIENTATION_VERTIKAL) {
							this.register.GraphPosY = this.register.GraphPosY + oldHoehe - this.register.RegisterHoehe;
						} else {
							this.register.GraphPosX = this.register.GraphPosX + oldHoehe - this.register.RegisterHoehe;
						}
					}
				}

				ok = this.CheckValidity(owningWall, offset.X, offset.Y);
				if (!ok) {
					this.register.GraphPosX = oldPosX;
					this.register.GraphPosY = oldPosY;
					this.register.Rohre = oldRohre;
					this.register.RegisterType = oldType;
					this.register.Gaps = oldGaps;
				} else {
					// TODO move Verbindeleitungen
				}
			}

			HithermCircuit circuit = product.GetCircuitForRegister(this.register);
			foreach (GraphicalHithermVerbindung link in circuit.Links) {
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
			get { return this.register.Orientation == HithermRegister.RegisterOrientationEnum.ORIENTATION_VERTIKAL ? this.register.RegisterHoehe : this.register.RegisterBreiteForDrawing; }
		}

		public double Width {
			get { return this.register.Orientation == HithermRegister.RegisterOrientationEnum.ORIENTATION_VERTIKAL ? this.register.RegisterBreiteForDrawing : this.register.RegisterHoehe; }
		}

		private double bakX, bakY;
		private HithermRegister.HithermRegisterTypeEnum bakType;
		private int bakRohre = -1;

		public override void BackupState() {
			this.bakX = this.register.GraphPosX;
			this.bakY = this.register.GraphPosY;
			this.bakType = this.register.RegisterType;
			this.bakRohre = this.register.Rohre;
			HithermCircuit circuit = product.GetCircuitForRegister(this.register);
			foreach (GraphicalHithermVerbindung link in circuit.Links) {
				if (link.Start == this.register) {
					link.BackupState();
				}
				if (link.End == this.register) {
					link.BackupState();
				}
			}
		}

		public override void RevertState() {
			if (bakRohre > 0) {
				this.register.GraphPosX = bakX;
				this.register.GraphPosY = bakY;
				this.register.RegisterType = bakType;
				this.register.Rohre = bakRohre;
				HithermCircuit circuit = product.GetCircuitForRegister(this.register);
				if (circuit != null) {
					foreach (GraphicalHithermVerbindung link in circuit.Links) {
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
		}

		private bool tmpOnlyCompleteRegisters = false;
		public bool OnlyCompleteRegisters {
			get { return this.register == null ? this.tmpOnlyCompleteRegisters : this.register.OnlyCompleteRegisters; }
			set {
				this.tmpOnlyCompleteRegisters = value;
				if (this.register != null) {
					this.register.OnlyCompleteRegisters = value;
				}
			}
		}
	}
}
