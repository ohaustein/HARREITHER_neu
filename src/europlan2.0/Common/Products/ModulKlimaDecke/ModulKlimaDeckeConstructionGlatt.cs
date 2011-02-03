using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using WW.Math;
using System.Drawing;
using WW.Math.Geometry;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Europlan.Common {
	public class ModulKlimaDeckeConstructionGlatt : ModulKlimaDeckeConstruction {
		private double schienenBreite = 0.045; // meter
		private double schienenAbstand = 0.3; // meter
		private double offset = 0; // meter
		private double offsetY = 0; // meter (only used for beplankung)
		//private Nullable<Size2D> beplankung = null; // meter
		private Nullable<Size2D> beplankung = new Size2D(2.0, 1.25);

		private Nullable<Size2D> beplankungStart = null;
		private Nullable<Size2D> beplankungEnd = null;

		public ModulKlimaDeckeConstructionGlatt() {
		}

		[XmlIgnore]
		public override List<Point2D> CeilingCoordinates {
			get {
				if (this.Planner == null || this.Planner.Product == null || this.Planner.Product.AssociatedRoom == null || this.Planner.Product.AssociatedRoom.CeilingCoordinatesToUse == null) {
					return null;
				}
				return this.Planner.Product.AssociatedRoom.CeilingCoordinatesToUse;
			}
		}

		public override void RecalculateSchienen() {
			if (this.Planner == null || this.Planner.Product == null ||
				this.Planner.Product.AssociatedRoom == null ||
				this.CeilingCoordinates == null ||
				this.Planner.Product.AssociatedRoom.AssociatedPlan == null) {
				return;
			}
			Matrix3D matrix = Transformation3D.Rotate(-this.Rotation * Math.PI / 180.0);

			Point2D tmp;
			double maxX = double.MinValue;
			double minX = double.MaxValue;
			double maxY = double.MinValue;
			double minY = double.MaxValue;
			foreach (Point2D point in this.CeilingCoordinates) {
				tmp = matrix.Transform(point);
				if (tmp.X > maxX) {
					maxX = tmp.X;
				}
				if (tmp.X < minX) {
					minX = tmp.X;
				}
				if (tmp.Y > maxY) {
					maxY = tmp.Y;
				}
				if (tmp.Y < minY) {
					minY = tmp.Y;
				}
			}
			matrix = matrix.GetInverse();

			this.schienen.Clear();
			this.possibleLanes.Clear();

			double measure = this.Planner.Product.AssociatedRoom.AssociatedPlan.Measure.Value;
			double increment = (schienenBreite + schienenAbstand) * measure;
			double curPos = (minX + maxX - schienenBreite * measure) / 2.0 + (offset * measure);
			double curYPos = (minY + maxY) / 2.0 + (offsetY * measure);
			double curBeplankungsPos = 0;
			if (beplankung.HasValue) {
				double beplankungsIncrement = beplankung.Value.X * measure;
				while (curPos > minX) {
					curPos -= beplankungsIncrement;
				}
				double beplankungsYIncrement = beplankung.Value.Y * measure;
				while (curYPos > minY) {
					curYPos -= beplankungsYIncrement;
				}
				this.beplankungStart = new Size2D(curPos + schienenBreite / 2.0 * measure, curYPos);
				this.beplankungEnd = new Size2D(maxX, maxY);
				while (curPos + increment < minX) {
					curPos += increment;
					curBeplankungsPos += schienenBreite + schienenAbstand;
				}
			} else {
				while (curPos > minX) {
					curPos -= increment;
				}
				this.beplankungStart = null;
				this.beplankungEnd = null;
			}
			//curPos = curPos - increment;
			List<int> ignoreLanes = new List<int>();
			while (curPos < maxX) {
				Polygon2D schiene = new Polygon2D();
				schiene.Add(matrix.Transform(new Point2D(curPos, minY)));
				schiene.Add(matrix.Transform(new Point2D(curPos + schienenBreite * measure, minY)));
				schiene.Add(matrix.Transform(new Point2D(curPos + schienenBreite * measure, maxY)));
				schiene.Add(matrix.Transform(new Point2D(curPos, maxY)));
				this.schienen.Add(schiene);
				curBeplankungsPos += (schienenBreite + schienenAbstand);
				curPos += increment;
				if (beplankung.HasValue && curBeplankungsPos >= beplankung.Value.X) {
					if (curBeplankungsPos > beplankung.Value.X) {
						curPos -= (curBeplankungsPos - beplankung.Value.X) * measure;
						ignoreLanes.Add(this.schienen.Count - 1);
					}
					curBeplankungsPos = 0;
				}
			}

			Polygon2D room = new Polygon2D(this.CeilingCoordinates);
			if (room.IsClockwise()) {
				room = room.GetReverse();
			}

			for (int i = 0; i < this.schienen.Count - 1; i++) {
				if (!ignoreLanes.Contains(i)) {
					Polygon2D schieneLeft = this.schienen[i];
					Polygon2D schieneRight = this.schienen[i + 1];
					Polygon2D lane = new Polygon2D();
					lane.Add(schieneLeft[1]);
					lane.Add(schieneLeft[2]);
					lane.Add(schieneRight[3]);
					lane.Add(schieneRight[0]);
					if (lane.IsClockwise()) {
						lane = lane.GetReverse();
					}

					this.possibleLanes.Add(new PossibleModulLane(GetPossibleModuleAreasInLane(lane), this.possibleLanes.Count));
				}
			}
		}

		private List<PossibleModulLaneArea> GetPossibleModuleAreasInLane(Polygon2D lane) {
			Line2D rightBorder = new Line2D(lane[0], lane[0] - lane[1]);
			Line2D leftBorder = new Line2D(lane[3], lane[3] - lane[2]);
			List<LineSegment> unusableSegments = GetUnusableSegments(leftBorder, rightBorder, this.CeilingCoordinates, false, 0.15);
			if (this.Planner.Product.AssociatedRoom.CeilingUnusedAreaCoordinates != null) {
				List<LineSegment> tmp;
				foreach (List<Point2D> unusedArea in this.Planner.Product.AssociatedRoom.CeilingUnusedAreaCoordinates) {
					tmp = GetUnusableSegments(leftBorder, rightBorder, unusedArea, true, 0);
					unusableSegments.AddRange(tmp);
					NormalizeSegments(unusableSegments);
				}
			}

			List<LineSegment> usableSegments = InvertSegments(unusableSegments);
			List<PossibleModulLaneArea> possibleAreas = new List<PossibleModulLaneArea>();
			Matrix3D matrix = Transformation3D.Rotate(-this.Rotation * Math.PI / 180.0);
			Point2D tmpPoint = matrix.Transform(leftBorder.Origin);
			double leftBorderX = tmpPoint.X;
			tmpPoint = matrix.Transform(rightBorder.Origin);
			double rightBorderX = tmpPoint.X;
			matrix = matrix.GetInverse();
			foreach (LineSegment segment in usableSegments) {
				possibleAreas.Add(new PossibleModulLaneArea(
					matrix.Transform(new Point2D(leftBorderX, segment.Start)),
					matrix.Transform(new Point2D(leftBorderX, segment.End)),
					matrix.Transform(new Point2D(rightBorderX, segment.End)),
					matrix.Transform(new Point2D(rightBorderX, segment.Start)),
					segment.Start, segment.End));
			}

			return possibleAreas;


			/*Matrix3D matrix = Transformation3D.Rotate(-this.Rotation * Math.PI / 180.0);

			Polygon2D tmp = new Polygon2D();
			double startPointX = double.MaxValue;
			double endPointX = double.MinValue;
			int start = -1;
			int i = 0;
			foreach (Point2D point in this.CeilingCoordinates) {
				Point2D pointTf = matrix.Transform(point);
				if (pointTf.X < startPointX) {
					startPointX = pointTf.X;
					start = i;
				}
				if (pointTf.X > endPointX) {
					endPointX = pointTf.X;
				}
				tmp.Add(matrix.Transform(point));
				i++;
			}
			Polygon2D room = new Polygon2D();
			if (tmp.IsClockwise()) {
				for (i = tmp.Count; i > 0; i--) {
					room.Add(tmp[(start + i) % tmp.Count]);
				}
			} else {
				for (i = 0; i < tmp.Count; i++) {
					room.Add(tmp[(start + i) % tmp.Count]);
				}
			}

			Point2D p = matrix.Transform(lane[0]);
			Line2D borderRight = new Line2D(p, p - matrix.Transform(lane[1]));
			p = matrix.Transform(lane[3]);
			Line2D borderLeft = new Line2D(p, p - matrix.Transform(lane[2]));

			Segment2D roomBorder;
			Nullable<Point2D> intersection = null;
			if (startPointX > borderLeft.Origin.X || endPointX < borderRight.Origin.X) {
				// no possible module areas in lane found
				return new List<PossibleModulLaneArea>();
			}

			bool inside = false;
			List<double> bordersTop = new List<double>();
			List<double> bordersBottom = new List<double>();
			List<CompareablePair<double>> removes = new List<CompareablePair<double>>();
			List<double> lp = new List<double>();
			bool enteredLeft = true;
			for (i = 0; i < room.Count; i++) {
				roomBorder = new Segment2D(room[i], room[(i + 1) % room.Count]);
				if (roomBorder.Start.X < roomBorder.End.X) {
					this.CheckLeftBorder(borderLeft, roomBorder, ref inside, bordersBottom, removes, ref lp, ref enteredLeft);
					this.CheckRightBorder(borderRight, roomBorder, ref inside, bordersTop, removes, ref lp, ref enteredLeft);
				} else {
					this.CheckRightBorder(borderRight, roomBorder, ref inside, bordersTop, removes, ref lp, ref enteredLeft);
					this.CheckLeftBorder(borderLeft, roomBorder, ref inside, bordersBottom, removes, ref lp, ref enteredLeft);
				}
				if (inside) {
					lp.Add(room[i == room.Count - 1 ? 0 : i + 1].Y);
				}
			}

			if (this.Planner.Product.AssociatedRoom.CeilingUnusedAreaCoordinates != null) {
				foreach (List<Point2D> unusedArea in this.Planner.Product.AssociatedRoom.CeilingUnusedAreaCoordinates) {
					tmp = new Polygon2D();
					startPointX = double.MaxValue;
					endPointX = double.MinValue;
					i = 0;
					foreach (Point2D unusedPoint in unusedArea) {
						Point2D pointTf = matrix.Transform(unusedPoint);
						if (pointTf.X < startPointX) {
							startPointX = pointTf.X;
							start = i;
						}
						if (pointTf.X > endPointX) {
							endPointX = pointTf.X;
						}
						tmp.Add(matrix.Transform(unusedPoint));
						i++;
					}

					if (startPointX <= borderRight.Origin.X && endPointX >= borderLeft.Origin.X) {
						Polygon2D unused = new Polygon2D();
						if (!tmp.IsClockwise()) {
							for (i = tmp.Count; i > 0; i--) {
								unused.Add(tmp[(start + i) % tmp.Count]);
							}
						} else {
							for (i = 0; i < tmp.Count; i++) {
								unused.Add(tmp[(start + i) % tmp.Count]);
							}
						}

						Segment2D unusedBorder;
						inside = false;
						for (i = 0; i < unused.Count; i++) {
							unusedBorder = new Segment2D(unused[i], unused[(i + 1) % unused.Count]);
							if (unusedBorder.Start.X < unusedBorder.End.X) {
								this.CheckLeftBorder(borderLeft, unusedBorder, ref inside, bordersBottom, removes, ref lp, ref enteredLeft);
								this.CheckRightBorder(borderRight, unusedBorder, ref inside, bordersTop, removes, ref lp, ref enteredLeft);
							} else {
								this.CheckRightBorder(borderRight, unusedBorder, ref inside, bordersTop, removes, ref lp, ref enteredLeft);
								this.CheckLeftBorder(borderLeft, unusedBorder, ref inside, bordersBottom, removes, ref lp, ref enteredLeft);
							}
							if (inside) {
								lp.Add(unused[i == unused.Count - 1 ? 0 : i + 1].Y);
							}
						}
					}
				}
			}

			bordersTop.Sort();
			bordersBottom.Sort();
			removes.Sort();

			List<PossibleModulLaneArea> possibleAreas = new List<PossibleModulLaneArea>();
			matrix = matrix.GetInverse();
			for (i = 0; i < bordersTop.Count; i++) {
				double top = bordersTop[i];
				double bottom;
				//Polygon2D area;
				foreach (CompareablePair<double> remove in removes) {
					if (remove.value1 > bordersTop[i] && remove.value1 < bordersBottom[i]) {
						bottom = remove.value2;
						possibleAreas.Add(new PossibleModulLaneArea(
							matrix.Transform(new Point2D(borderLeft.Origin.X, top)),
							matrix.Transform(new Point2D(borderLeft.Origin.X, bottom)),
							matrix.Transform(new Point2D(borderRight.Origin.X, bottom)),
							matrix.Transform(new Point2D(borderRight.Origin.X, top)),
							top, bottom));
						top = remove.value1;
					}
				}
				bottom = bordersBottom[i];
				possibleAreas.Add(new PossibleModulLaneArea(
					matrix.Transform(new Point2D(borderLeft.Origin.X, top)),
					matrix.Transform(new Point2D(borderLeft.Origin.X, bottom)),
					matrix.Transform(new Point2D(borderRight.Origin.X, bottom)),
					matrix.Transform(new Point2D(borderRight.Origin.X, top)),
					top, bottom));

			}

			return possibleAreas;*/
		}

		private List<LineSegment> GetUnusableSegments(Line2D borderLeft, Line2D borderRight, List<Point2D> polygon, bool unused, double wallDist) {
			if (polygon.Count < 3) {
				return new List<LineSegment>();
			}
			Matrix3D matrix = Transformation3D.Rotate(-this.Rotation * Math.PI / 180.0);

			Polygon2D alignedPolygon = new Polygon2D();
			double startPointX = double.MaxValue;
			double endPointX = double.MinValue;
			int start = -1;
			//int i = 0;
			//Vector2D normalizedBorderDirection = matrix.Transform(borderLeft.Direction);
			Vector2D normalizedBorderDirection = new Vector2D(0, 1);
			// todo replace by new Vector2D(0, 1)
			Line2D normalizedBorderLeft = new Line2D(matrix.Transform(borderLeft.Origin), normalizedBorderDirection);
			Line2D normalizedBorderRight = new Line2D(matrix.Transform(borderRight.Origin), normalizedBorderDirection);
			List<Line2D> normalizedLines = new List<Line2D>();
			normalizedLines.Add(normalizedBorderLeft);
			for (int i = 0; i < polygon.Count; i++) {
				Point2D pointTf = matrix.Transform(polygon[i]);
				if (pointTf.X < startPointX) {
					startPointX = pointTf.X;
					start = i;
				}
				if (pointTf.X > endPointX) {
					endPointX = pointTf.X;
				}
				alignedPolygon.Add(pointTf);
				if (pointTf.X > normalizedBorderLeft.Origin.X && pointTf.X < normalizedBorderRight.Origin.X) {
					normalizedLines.Add(new Line2D(pointTf, normalizedBorderDirection));
				}
			}
			normalizedLines.Add(normalizedBorderRight);

			/*if (startPointX > normalizedBorderRight.Origin.X || endPointX < normalizedBorderLeft.Origin.X) {
				return new List<LineSegment>();
			}*/

			List<LineSegment> segmentsUnusable = new List<LineSegment>();
			for (int i = 0; i < normalizedLines.Count; i++) {
				Line2D line = normalizedLines[i];
				List<double> lineIntersections = new List<double>();
				int k;
				if (!unused) {
					lineIntersections.Add(double.MinValue);
				}
				for (int j = 0; j < alignedPolygon.Count; j++) {
					k = (j + 1) % alignedPolygon.Count;
					if ((alignedPolygon[j].X < line.Origin.X) != (alignedPolygon[k].X < line.Origin.X)) {
						if (alignedPolygon[j].X == line.Origin.X) {
							lineIntersections.Add(alignedPolygon[j].Y);
						} else if (alignedPolygon[k].X == line.Origin.X) {
							lineIntersections.Add(alignedPolygon[k].Y);
						} else {
							Segment2D segment = new Segment2D(alignedPolygon[j], alignedPolygon[k]);
							Nullable<Point2D> intersection = Line2D.GetIntersection(line, segment);
							lineIntersections.Add(intersection.Value.Y);
						}
					}
				}
				if (!unused) {
					lineIntersections.Add(double.MaxValue);
				}
				lineIntersections.Sort();
				for (int j = 0; j < lineIntersections.Count; j += 2) {
					segmentsUnusable.Add(new LineSegment(lineIntersections[j], lineIntersections[j + 1]));
				}
			}
			NormalizeSegments(segmentsUnusable);
			if (wallDist != 0) {
				List<LineSegment> segmentsUsable = InvertSegments(segmentsUnusable);
				int i = 0;
				double wallDistAdd = wallDist * this.Planner.Product.AssociatedRoom.AssociatedPlan.Measure.Value;
				while (i < segmentsUsable.Count) {
					segmentsUsable[i].Start += wallDistAdd;
					segmentsUsable[i].End -= wallDistAdd;
					if (segmentsUsable[i].Start > segmentsUsable[i].End) {
						segmentsUsable.RemoveAt(i);
					} else {
						i++;
					}
				}
				segmentsUnusable = InvertSegments(segmentsUsable);
			}
			return segmentsUnusable;
		}

		private class LineSegment : IComparable<LineSegment> {
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
		}

		private List<LineSegment> MergeSegments(List<LineSegment> segments1, List<LineSegment> segments2) {
			List<LineSegment> mergedSegments = new List<LineSegment>(segments1);
			mergedSegments.AddRange(segments2);
			NormalizeSegments(mergedSegments);
			return mergedSegments;
		}

		private void NormalizeSegments(List<LineSegment> segments) {
			segments.Sort();
			int i = 1;
			while (i < segments.Count) {
			//for (int i = 1; i < segments.Count; i++) {
				if (segments[i - 1].End >= segments[i].Start) {
					segments[i - 1].End = Math.Max(segments[i - 1].End, segments[i].End);
					segments.RemoveAt(i);
				} else {
					i++;
				}
			}
		}

		private List<LineSegment> InvertSegments(List<LineSegment> segments) {
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

		private void CheckLeftBorder(Line2D borderLeft, Segment2D roomBorder, ref bool inside, List<double> bordersTop, List<CompareablePair<double>> removes, ref List<double> possiblePoints, ref bool enteredLeft) {
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

		private void CheckRightBorder(Line2D borderRight, Segment2D roomBorder, ref bool inside, List<double> bordersBottom, List<CompareablePair<double>> removes, ref List<double> possiblePoints, ref bool enteredLeft) {
			Nullable<Point2D> intersection = Line2D.GetIntersection(borderRight, roomBorder);
			if (intersection.HasValue) {
				possiblePoints.Add(intersection.Value.Y);
				if (inside) {
					if (enteredLeft) {
						bordersBottom.Add(GetMax(possiblePoints));
					} else {
						removes.Add(new CompareablePair<double>(GetMax(possiblePoints), GetMin(possiblePoints)));
					}
					possiblePoints = new List<double>();
					inside = false;
				} else {
					inside = true;
					enteredLeft = false;
				}
			}
		}

		private double GetMin(List<double> values) {
			double min = double.MaxValue;
			foreach (double val in values) {
				if (val < min) {
					min = val;
				}
			}
			return min;
		}

		private double GetMax(List<double> values) {
			double max = double.MinValue;
			foreach (double val in values) {
				if (val > max) {
					max = val;
				}
			}
			return max;
		}

		[XmlIgnore]
		public double SchienenBreite {
			get { return this.schienenBreite; }
			set {
				this.schienenBreite = value;
				this.RecalculateSchienen();
			}
		}

		public double SchienenAbstand {
			get { return this.schienenAbstand; }
			set {
				this.schienenAbstand = value;
				this.RecalculateSchienen();
			}
		}

		public double Offset {
			get { return this.offset; }
			set {
				this.offset = value;
				double increment = this.beplankung.HasValue ? this.beplankung.Value.X : this.schienenBreite + this.schienenAbstand;
				while (this.offset >= increment) {
					this.offset -= increment;
				}
				while (this.offset < 0) {
					this.offset += increment;
				}
				this.RecalculateSchienen();
			}
		}

		public double OffsetY {
			get { return this.offsetY; }
			set {
				if (!this.beplankung.HasValue) {
					this.OffsetY = 0;
					return;
				}
				this.offsetY = value;
				double increment = this.beplankung.Value.Y;
				while (this.offset >= increment) {
					this.offset -= increment;
				}
				while (this.offset < 0) {
					this.offset += increment;
				}
			}
		}

		public Nullable<Size2D> Beplankung {
			get { return this.beplankung; }
			set {
				this.beplankung = value;
				this.RecalculateSchienen();
			}
		}

		protected GraphicsPath GetProductAreaPath() {
			List<PointF> transformedPoints = new List<PointF>();
			Matrix4D additionalTransformation = this.AdditionalTransformation;
			foreach (Point2D point in this.CeilingCoordinates) {
				Point2D tmp = additionalTransformation.TransformTo2D(point);
				transformedPoints.Add(new PointF((float)tmp.X, (float)tmp.Y));
			}
			GraphicsPath path = new GraphicsPath();
			path.AddPolygon(transformedPoints.ToArray());
			return path;
		}

		protected GraphicsPath GetCeilingPath() {
			List<PointF> transformedPoints = new List<PointF>();
			Matrix4D additionalTransformation = this.AdditionalTransformation;
			foreach (Point2D point in this.Planner.Product.AssociatedRoom.CeilingCoordinatesToUse) {
				Point2D tmp = additionalTransformation.TransformTo2D(point);
				transformedPoints.Add(new PointF((float)tmp.X, (float)tmp.Y));
			}
			GraphicsPath path = new GraphicsPath();
			path.AddPolygon(transformedPoints.ToArray());
			return path;
		}

		private List<Polygon2D> GetSchienen(bool forDrawing) {
			if (!forDrawing) {
				return this.schienen;
			}

			Polygon2D schieneForDrawing;
			List<Polygon2D> schienenForDrawing = new List<Polygon2D>();
			Matrix4D additionalTransformation = this.AdditionalTransformation;
			foreach (Polygon2D schiene in this.schienen) {
				schieneForDrawing = new Polygon2D();
				schieneForDrawing.Add(additionalTransformation.TransformTo2D((Point3D)schiene[0]));
				schieneForDrawing.Add(additionalTransformation.TransformTo2D((Point3D)schiene[1]));
				schieneForDrawing.Add(additionalTransformation.TransformTo2D((Point3D)schiene[2]));
				schieneForDrawing.Add(additionalTransformation.TransformTo2D((Point3D)schiene[3]));
				schienenForDrawing.Add(schieneForDrawing);
			}
			return schienenForDrawing;
		}

		private List<Polygon2D> GetPossibleAreas(bool forDrawing) {
			List<Polygon2D> possibleAreas = new List<Polygon2D>();
			Matrix4D additionalTransformation = this.AdditionalTransformation;
			foreach (PossibleModulLane possibleLane in this.possibleLanes) {
				foreach (PossibleModulLaneArea possibleArea in possibleLane.Areas) {
					if (forDrawing) {
						Polygon2D a = new Polygon2D();
						a.Add(additionalTransformation.TransformTo2D((Point3D)possibleArea.Area[0]));
						a.Add(additionalTransformation.TransformTo2D((Point3D)possibleArea.Area[1]));
						a.Add(additionalTransformation.TransformTo2D((Point3D)possibleArea.Area[2]));
						a.Add(additionalTransformation.TransformTo2D((Point3D)possibleArea.Area[3]));
						possibleAreas.Add(a);
					} else {
						possibleAreas.Add(possibleArea.Area);
					}
				}
			}
			return possibleAreas;
		}

		public override void Paint(Graphics g, ModulKlimaDeckePlanner.KlimaDeckeMode mode, bool drawBeplankung) {
			if (this.Planner == null ||
				this.Planner.Product == null ||
				this.Planner.Product.AssociatedRoom == null ||
				this.CeilingCoordinates == null ||
				this.CeilingCoordinates.Count < 3 ||
				this.Planner.Product.AssociatedRoom.AssociatedPlan == null ||
				this.Planner.Product.AssociatedRoom.AssociatedPlan.Measure == null) {
				return;
			}
			double measure = this.Planner.Product.AssociatedRoom.AssociatedPlan.Measure.Value;
			Matrix4D additionalTransformation = this.AdditionalTransformation;

			/*double minX, maxX, minY, maxY;*/
			GraphicsPath roomPath = this.GetProductAreaPath(/*out minX, out maxX, out minY, out maxY*/);
			g.Clip = new Region(roomPath);

			Color c = Color.Gray;
			Pen p = new Pen(c);
			Brush b = new HatchBrush(HatchStyle.DiagonalCross, c, Color.FromArgb(0, c));

			//g.FillPath(new SolidBrush(Color.FromArgb(128, Color.Yellow)), roomPath);

			foreach (Polygon2D schiene in this.GetSchienen(true)) {
				PointF[] poly = new PointF[schiene.Count];
				int i = 0;
				foreach (Point2D point in schiene) {
					poly[i++] = new PointF((float)point.X, (float)point.Y);
				}
				g.DrawPolygon(p, poly);
				g.FillPolygon(b, poly);
			}

			if (drawBeplankung && this.beplankung.HasValue && this.beplankungStart.HasValue && this.beplankungEnd.HasValue) {
				Matrix3D matrix = Transformation3D.Rotate(this.Rotation * Math.PI / 180.0);
				for (double beplankungX = this.beplankungStart.Value.X; beplankungX < this.beplankungEnd.Value.X; beplankungX += beplankung.Value.X * measure) {
					for (double beplankungY = this.beplankungStart.Value.Y; beplankungY < this.beplankungEnd.Value.Y; beplankungY += beplankung.Value.Y * measure) {
						Point2D p1 = additionalTransformation.TransformTo2D(matrix.Transform(new Point3D(beplankungX, beplankungY, 0)));
						Point2D p2 = additionalTransformation.TransformTo2D(matrix.Transform(new Point3D(beplankungX + beplankung.Value.X * measure, beplankungY, 0)));
						Point2D p3 = additionalTransformation.TransformTo2D(matrix.Transform(new Point3D(beplankungX + beplankung.Value.X * measure, beplankungY + beplankung.Value.Y * measure, 0)));
						Point2D p4 = additionalTransformation.TransformTo2D(matrix.Transform(new Point3D(beplankungX, beplankungY + beplankung.Value.Y * measure, 0)));
						g.DrawPolygon(p, new PointF[] { new PointF((float)p1.X, (float)p1.Y), new PointF((float)p2.X, (float)p2.Y), new PointF((float)p3.X, (float)p3.Y), new PointF((float)p4.X, (float)p4.Y) });
						//g.DrawRectangle(p, (float)(beplankungX), (float)(beplankungY), (float)(beplankung.Value.X * measure), (float)(beplankung.Value.Y * measure));
					}
				}
			}

			if (mode == ModulKlimaDeckePlanner.KlimaDeckeMode.KDM_CONSTRUCTION) {
				c = Color.FromArgb(128, 0, 240, 0);
				p = new Pen(c);
				b = new SolidBrush(Color.FromArgb(64, c));
				Region r = new Region();
				r.MakeInfinite();
				g.Clip = r;
				foreach (Polygon2D area in this.GetPossibleAreas(true)) {
					PointF[] poly = new PointF[area.Count];
					int i = 0;
					foreach (Point2D point in area) {
						poly[i++] = new PointF((float)point.X, (float)point.Y);
					}
					g.DrawPolygon(p, poly);
					g.FillPolygon(b, poly);
				}
			}
		}

		public override bool HitTest(Point2D planPoint, Point pointInControl) {
			if (this.Planner == null ||
				this.Planner.Product == null ||
				this.Planner.Product.AssociatedRoom == null ||
				this.CeilingCoordinates == null ||
				this.CeilingCoordinates.Count < 3 ||
				this.Planner.Product.AssociatedRoom.AssociatedPlan == null ||
				this.Planner.Product.AssociatedRoom.AssociatedPlan.Measure == null) {
				return false;
			}

			GraphicsPath roomPath = this.GetProductAreaPath();
			if (!roomPath.IsVisible(pointInControl)) {
				return false;
			}

			List<Polygon2D> schienen = this.GetSchienen(false);
			foreach (Polygon2D schiene in schienen) {
				if (schiene.IsInside(planPoint)) {
					return true;
				}
			}
			return false;
		}

		Point2D startPlanPoint;
		Point startPointInControl;
		double startOffset;
		double startOffsetY;

		public override void StartDrag(Point2D planPoint, Point pointInControl) {
			startPlanPoint = planPoint;
			startPointInControl = pointInControl;
			startOffset = this.offset;
			startOffsetY = this.offsetY;
		}

		public override void MoveDrag(Point2D planPoint, Point pointInControl) {
			Vector2D move = Transformation3D.Rotate(-this.Rotation * Math.PI / 180.0).Transform(planPoint - startPlanPoint);
			this.Offset = startOffset + (move.X / this.Planner.Product.AssociatedRoom.AssociatedPlan.Measure.Value);
			this.OffsetY = startOffsetY + (move.Y / this.Planner.Product.AssociatedRoom.AssociatedPlan.Measure.Value);

			if (this.Planner.ConnectedPlanPanel != null) {
				this.Planner.ConnectedPlanPanel.InvalidateGraphics();
			}
		}

		public override void EndDrag(Point2D planPoint, Point pointInControl) {
		}

		[XmlIgnore]
		public override Cursor PickCursor {
			get { return Cursors.NoMove2D; }
		}
	}
}
