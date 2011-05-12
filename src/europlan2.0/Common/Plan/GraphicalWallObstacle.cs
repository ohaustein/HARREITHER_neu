using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {

	
	public abstract class GraphicalWallObstacle : IGraphicalWallObject {

		public abstract bool HitTest(WW.Math.Point2D planPoint, double xOffset, double yOffset);
		public abstract void PaintObject(System.Drawing.Graphics g, double xOffset, double yOffset, IGraphicalWallObject selectedObject, double scale);
		public abstract IGraphicalWallObject GetPickedObject(WW.Math.Point2D planPoint, double xOffset, double yOffset);
		public abstract WW.Math.Geometry.Polygon2D GetObjectBorders(double xOffset, double yOffset);
		public abstract bool CollisionTest(WW.Math.Geometry.Polygon2D polygon, double xOffset, double yOffset);
		public abstract bool StartDrag(Anchor anchor, WW.Math.Point2D planPoint, GraphicalWall owningWall);
		public abstract bool MoveDrag(Anchor anchor, WW.Math.Point2D planPoint, GraphicalWall owningWall);
		public abstract bool EndDrag(Anchor anchor, WW.Math.Point2D planPoint, GraphicalWall owningWall);
		public abstract List<Anchor> GetAnchors(double scale);
		public abstract bool IsMoveable {
			get;
		}

	}

}
