using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using WW.Math;
using System.Drawing;

namespace Europlan.Common {
	public interface IPlanner {
		IPlanPanel ConnectedPlanPanel {
			get;
			set;
		}

		Cursor CustomCursor {
			get;
		}

		void PaintAfterPlanPannel(PaintEventArgs e, Matrix4D additionalTransformation, Point2D mousePositionInPlan, Point mousePositionInControl);

		bool PlannerClick(Point2D planPoint, Point pointInControl, MouseButtons button);
		bool PlannerMouseMove(Point2D planPoint, Point pointInControl, MouseButtons button);

		bool PlannerDragStart(Point2D planPoint, Point pointInControl, MouseButtons button);
		bool PlannerDragMove(Point2D planPoint, Point pointInControl, MouseButtons button);
		bool PlannerDragEnd(Point2D planPoint, Point pointInControl, MouseButtons button);

		bool PlannerKeyPress(Keys key);
	}

	public interface IProductPlanner : IPlanner {
		void PaintAfterPlanPannel(Graphics g, Matrix4D additionalTransformation, Point2D mousePositionInPlan, Point mousePositionInControl);
	}
}
