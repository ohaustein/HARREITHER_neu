using System;
using System.Collections.Generic;
using System.Text;
using WW.Math;
using System.Drawing;
using WW.Math.Geometry;

namespace Europlan.Common {
	public interface IGraphicalWallObject {
		bool HitTest(Point2D planPoint, double xOffset, double yOffset);
		void PaintObject(Graphics g, double xOffset, double yOffset, IGraphicalWallObject selectedObject);
		IGraphicalWallObject GetPickedObject(Point2D planPoint, double xOffset, double yOffset);
	}
}
