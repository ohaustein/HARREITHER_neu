using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Europlan.Common {

	[Serializable()]
	public class ImagePlan : Plan {

		private float angle = 0;
		private float xPos = 0;
		private float yPos = 0;
		private Nullable<float> scale = null;

		public float Angle {
			get { return angle; }
			set { angle = value; }
		}

		public float XPos {
			get { return xPos; }
			set { xPos = value; }
		}

		public float YPos {
			get { return yPos; }
			set { yPos = value; }
		}

		public Nullable<float> Scale {
			get { return scale; }
			set { scale = value; }
		}

		public float PolygonArea(PointF[] coordinates) {
			// Return the absolute value of the signed area.
			// The signed area is negative if the polyogn is
			// oriented clockwise.
			return Math.Abs(SignedPolygonArea(coordinates));
		}

		private float SignedPolygonArea(PointF[] coordinates) {
			// Add the first point to the end.
			int num_points = coordinates.Length;
			PointF[] pts = new PointF[num_points + 1];
			coordinates.CopyTo(pts, 0);
			pts[num_points] = coordinates[0];

			// Get the areas.
			float area = 0;
			for (int i = 0; i < num_points; i++) {
				area +=
					(pts[i + 1].X - pts[i].X) *
					(pts[i + 1].Y + pts[i].Y) / 2;
			}

			// Return the result.
			return area;
		}
	}

}
