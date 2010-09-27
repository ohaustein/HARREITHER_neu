using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using WW.Math;
using System.Drawing;

namespace Europlan.Common {
	public interface IProductPlanner {
		IPlanPanel ConnectedPlanPanel {
			get;
			set;
		}

		void PaintAfterPlanPannel(PaintEventArgs e, Matrix4D additionalTransformation);

		bool PlannerClick(Point2D planPoint, PointF screenPoint, MouseButtons button);
		bool PlannerMouseMove(Point2D planPoint, PointF screenPoint, MouseButtons button);

		bool PlannerDragStart(Point2D planPoint, PointF screenPoint, MouseButtons button);
		bool PlannerDragMove(Point2D planPoint, PointF screenPoint, Point2D lastPlanPoint, PointF lastScreenPoint, MouseButtons button);
		bool PlannerDragEnd(Point2D planPoint, PointF screenPoint, MouseButtons button);
	}
}
