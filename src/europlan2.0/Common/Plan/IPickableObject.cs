using System;
using System.Collections.Generic;
using System.Text;
using WW.Math;
using System.Drawing;
using System.Windows.Forms;

namespace Europlan.Common {
	interface IPickableObject {
		bool HitTest(Point2D planPoint, Point pointInControl);
		Cursor PickCursor {
			get;
		}
	}
}
