using System;
using System.Collections.Generic;
using System.Text;
using WW.Math;
using System.Drawing;

namespace Europlan.Common {
	interface IDragableObject {
		void StartDrag(Point2D planPoint, Point pointInControl);
		void MoveDrag(Point2D planPoint, Point pointInControl);
		void EndDrag(Point2D planPoint, Point pointInControl);
	}
}
