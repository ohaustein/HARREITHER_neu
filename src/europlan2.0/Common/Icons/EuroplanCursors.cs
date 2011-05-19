using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Europlan.Common.Icons {
	public class EuroplanCursors {
		private static Cursor movePlan = null;
		private static Cursor movePlanActive = null;

		public static Cursor MOVE_PLAN {
			get {
				if (movePlan == null) {
					try {
						movePlan = new Cursor(typeof(EuroplanCursors), "hand.cur");
					} catch {
						movePlan = Cursors.SizeAll;
					}
				}
				return movePlan;
			}
		}

		public static Cursor MOVE_PLAN_ACTIVE {
			get {
				if (movePlanActive == null) {
					try {
						movePlanActive = new Cursor(typeof(EuroplanCursors), "handGrabbed.cur");
					} catch {
						movePlanActive = Cursors.SizeAll;
					}
				}
				return movePlanActive;
			}
		}
	}
}
