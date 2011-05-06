using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

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
			throw new Exception("The method or operation is not implemented.");
		}

		public override WW.Math.Geometry.Polygon2D GetObjectBorders(double xOffset, double yOffset) {
			throw new Exception("The method or operation is not implemented.");
		}

		public override void PaintObject(System.Drawing.Graphics g, double xOffset, double yOffset, IGraphicalWallObject selectedObject, double scale) {
			this.PaintObject(g, xOffset, yOffset, (this == selectedObject) ? Color.Red : Color.Black, scale);
		}

		public void PaintObject(System.Drawing.Graphics g, double xOffset, double yOffset, Color color) {
			this.PaintObject(g, xOffset, yOffset, color, 1);
		}

		public void PaintObject(System.Drawing.Graphics g, double xOffset, double yOffset, Color color, double scale) {
			if (register == null) {
				return;
			}
			Pen registerPen = new Pen(color, (float)(1 / scale));
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
				//register.RegisterBreite;
				//register.RegisterHoehe;
			} else {
			}
		}

		public override IGraphicalWallObject GetPickedObject(WW.Math.Point2D planPoint, double xOffset, double yOffset) {
			throw new Exception("The method or operation is not implemented.");
		}
	}
}
