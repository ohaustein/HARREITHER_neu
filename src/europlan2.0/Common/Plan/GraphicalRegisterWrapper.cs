using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {
	public abstract class GraphicalRegisterWrapper : IGraphicalWallObject {

		#region IGraphicalWallObject Members
		public abstract bool HitTest(WW.Math.Point2D planPoint, double xOffset, double yOffset);

		public abstract void PaintObject(System.Drawing.Graphics g, double xOffset, double yOffset, IGraphicalWallObject selectedObject, double scale);

		public abstract IGraphicalWallObject GetPickedObject(WW.Math.Point2D planPoint, double xOffset, double yOffset);
		#endregion
	}
}
