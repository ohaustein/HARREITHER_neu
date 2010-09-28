using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using Europlan.Common;
using System.Drawing;
using WW.Math;

namespace Europlan.Common {

	[Serializable()]
	[XmlInclude(typeof(ImagePlan))]
	[XmlInclude(typeof(CadPlan))]
	public abstract class Plan {

		private string id = Guid.NewGuid().ToString();
		private string name;
		private string relativeFileName;
		private Nullable<float> measure = null;
		
		public string Name {
			get { return name; }
			set { name = value; }
		}

		public string RelativeFileName {
			get { return relativeFileName; }
			set { relativeFileName = value; }
		}

		[XmlIgnore]
		public string AbsoluteFileName {
			get {
				string projectDir = Path.GetDirectoryName(Project.Instance.ProjectFileName);
				return Path.Combine(projectDir, RelativeFileName);
			}
		}

		public Nullable<float> Measure {
			get { return measure; }
			set { measure = value; }
		}

		public string Id {
			get { return id; }
			set { id = value; }
		}


		public static double PolygonArea(Point2D[] coordinates) {
			// Return the absolute value of the signed area.
			// The signed area is negative if the polyogn is
			// oriented clockwise.
			return Math.Abs(SignedPolygonArea(coordinates));
		}

		private static double SignedPolygonArea(Point2D[] coordinates) {
			// Add the first point to the end.
			int num_points = coordinates.Length;
			if (num_points < 3) {
				return 0;
			}
			Point2D[] pts = new Point2D[num_points + 1];
			coordinates.CopyTo(pts, 0);
			pts[num_points] = coordinates[0];

			// Get the areas.
			double area = 0;
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
