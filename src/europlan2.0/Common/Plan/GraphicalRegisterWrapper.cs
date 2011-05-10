using System;
using System.Collections.Generic;
using System.Text;
using WW.Math.Geometry;
using WW.Math;

namespace Europlan.Common {
	public abstract class GraphicalRegisterWrapper : IGraphicalWallObject {

		#region IGraphicalWallObject Members

		public abstract void PaintObject(System.Drawing.Graphics g, double xOffset, double yOffset, IGraphicalWallObject selectedObject, double scale);

		public abstract IGraphicalWallObject GetPickedObject(WW.Math.Point2D planPoint, double xOffset, double yOffset);
		public abstract Polygon2D GetObjectBorders(double xOffset, double yOffset);

		public abstract bool HitTest(WW.Math.Point2D planPoint, double xOffset, double yOffset);
		public abstract bool CollisionTest(Polygon2D polygon, double xOffset, double yOffset);

		public abstract List<Anchor> GetAnchors(double scale);

		public abstract bool StartDrag(Anchor anchor, Point2D planPoint, GraphicalWall owningWall);
		public abstract bool MoveDrag(Anchor anchor, Point2D planPoint, GraphicalWall owningWall);
		public abstract bool EndDrag(Anchor anchor, Point2D planPoint, GraphicalWall owningWall);

		public virtual bool IsMoveable {
			get { return true; }
		}
		#endregion
	}
}
