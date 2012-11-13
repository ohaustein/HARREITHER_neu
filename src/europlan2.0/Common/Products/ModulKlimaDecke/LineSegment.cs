using System;
using System.Collections.Generic;
using System.Text;
using WW.Math.Geometry;
using WW.Math;

namespace Europlan.Common.Products.ModulKlimaDecke {
	public class LineSegment : IComparable<LineSegment> {
		private double start;
		private double end;

		public LineSegment() {
			this.start = 0;
			this.end = 0;
		}

		public LineSegment(double start, double end) {
			this.start = start;
			this.end = end;
		}

		public double Start {
			get { return this.start; }
			set { this.start = value; }
		}

		public double End {
			get { return this.end; }
			set { this.end = value; }
		}

		#region IComparable<LineSegment> Members
		public int CompareTo(LineSegment other) {
			int rtn = this.start.CompareTo(other.start);
			if (rtn == 0) {
				rtn = this.end.CompareTo(other.end);
			}
			return rtn;
		}
		#endregion

		private List<LineSegment> MergeSegments(List<LineSegment> segments1, List<LineSegment> segments2) {
			List<LineSegment> mergedSegments = new List<LineSegment>(segments1);
			mergedSegments.AddRange(segments2);
			NormalizeSegments(mergedSegments);
			return mergedSegments;
		}

		public static void NormalizeSegments(List<LineSegment> segments) {
			segments.Sort();
			int i = 1;
			while (i < segments.Count) {
				if (segments[i - 1].End >= segments[i].Start) {
					segments[i - 1].End = Math.Max(segments[i - 1].End, segments[i].End);
					segments.RemoveAt(i);
				} else {
					i++;
				}
			}
		}

		public static List<LineSegment> InvertSegments(List<LineSegment> segments) {
			List<LineSegment> invertedSegments = new List<LineSegment>();
			if (segments.Count == 0) {
				invertedSegments.Add(new LineSegment(double.MinValue, double.MaxValue));
			} else {
				if (segments[0].Start > double.MinValue) {
					invertedSegments.Add(new LineSegment(double.MinValue, segments[0].Start));
				}
				for (int i = 1; i < segments.Count; i++) {
					invertedSegments.Add(new LineSegment(segments[i - 1].End, segments[i].Start));
				}
				if (segments[segments.Count - 1].End < double.MaxValue) {
					invertedSegments.Add(new LineSegment(segments[segments.Count - 1].End, double.MaxValue));
				}
			}
			return invertedSegments;
		}

		public static void CheckLeftBorder(Line2D borderLeft, Segment2D roomBorder, ref bool inside, List<double> bordersTop, List<CompareablePair<double>> removes, ref List<double> possiblePoints, ref bool enteredLeft) {
			Nullable<Point2D> intersection = Line2D.GetIntersection(borderLeft, roomBorder);
			if (intersection.HasValue) {
				possiblePoints.Add(intersection.Value.Y);
				if (inside) {
					if (enteredLeft) {
						removes.Add(new CompareablePair<double>(GetMax(possiblePoints), GetMin(possiblePoints)));
					} else {
						bordersTop.Add(GetMin(possiblePoints));
					}
					possiblePoints = new List<double>();
					inside = false;
				} else {
					inside = true;
					enteredLeft = true;
				}
			}
		}

		public static double GetMin(List<double> values) {
			double min = double.MaxValue;
			foreach (double val in values) {
				if (val < min) {
					min = val;
				}
			}
			return min;
		}

		public static double GetMax(List<double> values) {
			double max = double.MinValue;
			foreach (double val in values) {
				if (val > max) {
					max = val;
				}
			}
			return max;
		}
	}
}
