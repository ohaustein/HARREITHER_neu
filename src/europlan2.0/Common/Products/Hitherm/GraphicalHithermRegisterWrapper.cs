using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using WW.Math.Geometry;
using WW.Math;

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
			borders.Add(new Point2D(xOffset + this.register.GraphPosX, yOffset + this.register.GraphPosY));
			borders.Add(new Point2D(xOffset + this.register.GraphPosX, yOffset + this.register.GraphPosY + this.register.RegisterHoehe));
			borders.Add(new Point2D(xOffset + this.register.GraphPosX + this.register.RegisterBreiteForDrawing, yOffset + this.register.GraphPosY + this.register.RegisterHoehe));
			borders.Add(new Point2D(xOffset + this.register.GraphPosX + this.register.RegisterBreiteForDrawing, yOffset + this.register.GraphPosY));
			return borders;
		}

		public override void PaintObject(System.Drawing.Graphics g, double xOffset, double yOffset, IGraphicalWallObject selectedObject, double scale) {
			this.PaintObject(g, xOffset, yOffset, (this == selectedObject) ? Color.Red : Color.Black, scale, (this == selectedObject));
		}

		public void PaintObject(System.Drawing.Graphics g, double xOffset, double yOffset, Color color) {
			this.PaintObject(g, xOffset, yOffset, color, 1, false);
		}

		public void PaintObject(System.Drawing.Graphics g, double xOffset, double yOffset, Color color, double scale, bool highlightConnections) {
			if (register == null) {
				return;
			}
			Pen registerPen = new Pen(color, (float)(1 / scale));
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
							if (i % 15 == 14) {
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
						g.FillRectangle(bInput, (float)(xOffset + register.GraphPosX - 1.5), (float)(yOffset + register.GraphPosY - 1.5), 10, 10);
						g.DrawRectangle(pInput, (float)(xOffset + register.GraphPosX - 1.5), (float)(yOffset + register.GraphPosY - 1.5), 10, 10);
						g.FillRectangle(bInput, (float)(xOffset + register.GraphPosX + register.RegisterBreiteForDrawing - 3.5), (float)(yOffset + register.GraphPosY + register.RegisterHoehe - 3.5), 5, 5);
						g.DrawRectangle(pInput, (float)(xOffset + register.GraphPosX + register.RegisterBreiteForDrawing - 3.5), (float)(yOffset + register.GraphPosY + register.RegisterHoehe - 3.5), 5, 5);
					}
				}
			} else {
			}
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
				return new Point2D(xOffset + this.register.GraphPosX + this.register.RegisterBreiteForDrawing, yOffset + this.register.GraphPosY + this.register.RegisterHoehe - 1);
			} else {
				return new Point2D(xOffset + this.register.GraphPosX, yOffset + this.register.GraphPosY + this.register.RegisterHoehe - 1);
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
				area.Add(new Point2D(xOffset + this.register.GraphPosX + this.register.RegisterBreiteForDrawing - 3.5, yOffset + register.GraphPosY - 1.5));
				area.Add(new Point2D(xOffset + this.register.GraphPosX + this.register.RegisterBreiteForDrawing - 3.5, yOffset + register.GraphPosY + 3.5));
				area.Add(new Point2D(xOffset + this.register.GraphPosX + this.register.RegisterBreiteForDrawing + 1.5, yOffset + register.GraphPosY + 3.5));
				area.Add(new Point2D(xOffset + this.register.GraphPosX + this.register.RegisterBreiteForDrawing + 1.5, yOffset + register.GraphPosY - 1.5));
			}
			return area;
		}

		public PossibleHithermRegisterConnection GetOutputConnection(double xOffset, double yOffset, HithermProduct product, HithermCircuit circuit) {
			return new PossibleHithermRegisterConnection(this.GetOutputConnectionPoint(xOffset, yOffset), GetOutputConnectionArea(xOffset, yOffset), false, true, product, circuit, this.register);
		}

		public PossibleHithermRegisterConnection GetInputConnection(double xOffset, double yOffset, HithermProduct product, HithermCircuit circuit) {
			return new PossibleHithermRegisterConnection(this.GetInputConnectionPoint(xOffset, yOffset), GetInputConnectionArea(xOffset, yOffset), false, true, product, circuit, this.register);
		}

		public override bool CollisionTest(Polygon2D polygon, double xOffset, double yOffset) {
			Polygon2D register = GetObjectBorders(xOffset, yOffset);
			bool inside = false;
			foreach (Point2D point in polygon) {
				if (Polygon2D.IsInside(point, register)) {
					inside = true;
					break;
				}
			}
			return inside;
		}
	}
}
