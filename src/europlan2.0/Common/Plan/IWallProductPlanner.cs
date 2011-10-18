using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using WW.Math;
using System.Drawing;

namespace Europlan.Common {
	public interface IWallProductPlanner {
		GraphicalWallPanel ConnectedWallPanel {
			get;
			set;
		}

		Cursor CustomCursor {
			get;
		}

		void PaintAfterPlanPannel(PaintEventArgs e, Point2D mousePositionInPlan, Point mousePositionInControl, double scale);
		void PaintAfterPlanPannel(Graphics g, Point2D mousePositionInPlan, Point mousePositionInControl, double scale);

		bool PlannerClick(Point2D planPoint, Point pointInControl, MouseButtons button);
		bool PlannerMouseMove(Point2D planPoint, Point pointInControl, MouseButtons button);

		bool PlannerDragStart(Point2D planPoint, Point pointInControl, MouseButtons button);
		bool PlannerDragMove(Point2D planPoint, Point pointInControl, MouseButtons button);
		bool PlannerDragEnd(Point2D planPoint, Point pointInControl, MouseButtons button);

		bool PlannerKeyPress(Keys key);

		IGraphicalWallObject PickObject(Point2D mousePosInPlan);

		Product Product {
			get;
		}

		void OnRecalculationNecessary();
	}
}
