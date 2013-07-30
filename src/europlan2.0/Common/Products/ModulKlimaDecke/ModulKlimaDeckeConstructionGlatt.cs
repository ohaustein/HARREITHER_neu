using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using WW.Math;
using System.Drawing;
using WW.Math.Geometry;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using WW.Cad.Model;
using WW.Cad.Model.Tables;
using WW.Cad.Model.Entities;
using Europlan.Common.Products.ModulKlimaDecke;

namespace Europlan.Common {
	public class ModulKlimaDeckeConstructionGlatt : ModulKlimaDeckeConstruction {
		private Nullable<double> schienenBreite = null; // meter
		private double schienenAbstand = 0.3; // meter
		private double offset = 0; // meter
		private double offsetY = 0; // meter (only used for beplankung)
        private int beplankungXShift = 0;
		private Nullable<Size2D> beplankung = new Size2D(2.0, 1.25);

		private Nullable<Size2D> beplankungStart = null;
		private Nullable<Size2D> beplankungEnd = null;

        private ConstructionModifyMode mode = ConstructionModifyMode.MOVE_SCHIENEN;

		private ModulKlimaDeckeProduct.ModulCeilingConstructionEnum constructionType = (ModulKlimaDeckeProduct.ModulCeilingConstructionEnum)ModulKlimaDeckeProduct.ConfigModulCeilingConstruction == ModulKlimaDeckeProduct.ModulCeilingConstructionEnum.KASSETTENDECKE ? ModulKlimaDeckeProduct.ModulCeilingConstructionEnum.C_PROFIL : (ModulKlimaDeckeProduct.ModulCeilingConstructionEnum)ModulKlimaDeckeProduct.ConfigModulCeilingConstruction;

        public enum ConstructionModifyMode {
            MOVE_SCHIENEN,
            MOVE_BEPLANKUNG
        }

		public ModulKlimaDeckeConstructionGlatt() {
		}

        [XmlIgnore]
        public ConstructionModifyMode Mode {
            get { return this.mode; }
            set { this.mode = value; }
        }

		[XmlIgnore]
		public override List<Point2D> CeilingCoordinates {
			get {
				if (this.Product == null || this.Product.AssociatedRoom == null || this.Product.AssociatedRoom.CeilingCoordinatesToUse == null) {
					return null;
				}
				return this.Product.AssociatedRoom.CeilingCoordinatesToUse;
			}
		}

		private static int ComparePoint(Point2D p1, Point2D p2) {
			if (p1.X.Equals(p2.X)) {
				return p1.Y.CompareTo(p2.Y);
			} else {
				return p1.X.CompareTo(p2.X);
			}
		}

		public double GetStaffelnLength(double measure) {
			if (this.schienen == null) {
				this.RecalculateSchienen();
			}
			List<Point2D> ceilingCoordinates = this.CeilingCoordinates;
			Point2D start, end;
			Line2D staffel;
			Segment2D seg;
			Nullable<Point2D> intersection;
			List<Point2D> intersections = new List<Point2D>();
			double length = 0;
			foreach (Polygon2D schiene in this.schienen) {
				start = schiene[0] + (schiene[1] - schiene[0]) / 2.0;
				end = schiene[2] + (schiene[3] - schiene[2]) / 2.0;
				staffel = new Line2D(start, end - start);
				Point2D lastPoint = ceilingCoordinates[ceilingCoordinates.Count - 1];
				intersections.Clear();
				foreach (Point2D curPoint in ceilingCoordinates) {
					seg = new Segment2D(lastPoint, curPoint);
					intersection = Line2D.GetIntersection(staffel, seg);
					if (intersection.HasValue) {
						intersections.Add(intersection.Value);
					}
					lastPoint = curPoint;
				}
				intersections.Sort(ComparePoint);
				for (int i = 0; i < intersections.Count - 1; i += 2) {
					length += (intersections[i] - intersections[i + 1]).GetLength();
				}
			}
			return length / measure;
		}

        public int BeplankungXShift {
            get {
                if (!this.Beplankung.HasValue) {
                    return 0;
                }
                return this.beplankungXShift % (int)Math.Ceiling(this.Beplankung.Value.X / (this.SchienenBreite + this.SchienenAbstand));
            }
            set {
                this.beplankungXShift = value;
                while (this.beplankungXShift < 0) {
                    this.beplankungXShift += (int)Math.Ceiling(this.Beplankung.Value.X / (this.SchienenBreite + this.SchienenAbstand));
                }
            }
        }

		public override void RecalculateSchienen() {
			if (this.Product == null ||
				this.Product.AssociatedRoom == null ||
				this.CeilingCoordinates == null ||
				this.Product.AssociatedRoom.AssociatedPlan == null) {
				return;
			}
			List<Point2D> ceilingCoordinates = this.CeilingCoordinates;
			if (ceilingCoordinates.Count < 3) {
				this.schienen.Clear();
				return;
			}
			Matrix3D matrix = Transformation3D.Rotate(-this.Rotation * Math.PI / 180.0);

			Point2D tmp;
			double maxX = double.MinValue;
			double minX = double.MaxValue;
			double maxY = double.MinValue;
			double minY = double.MaxValue;
			foreach (Point2D point in ceilingCoordinates) {
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

			double measure = this.Product.AssociatedRoom.AssociatedPlan.Measure.Value;
			double increment = (SchienenBreite + schienenAbstand) * measure;
			double curPos = (minX + maxX - SchienenBreite * measure) / 2.0 + (offset * measure);
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
                this.beplankungStart = new Size2D(curPos + SchienenBreite / 2.0 * measure + this.BeplankungXShift * (SchienenAbstand + SchienenBreite) * measure - this.beplankung.Value.X * measure, curYPos);
				this.beplankungEnd = new Size2D(maxX, maxY);
				while (curPos + increment < minX) {
					curPos += increment;
					curBeplankungsPos += SchienenBreite + schienenAbstand;
				}
			} else {
				while (curPos > minX) {
					curPos -= increment;
				}
				this.beplankungStart = null;
				this.beplankungEnd = null;
			}
			List<int> ignoreLanes = new List<int>();
			double schieneStartY = minY - measure * 0.1;
			double schieneEndY = maxY + measure * 0.1;
			while (curPos < maxX) {
				Polygon2D schiene = new Polygon2D();
				schiene.Add(matrix.Transform(new Point2D(curPos, schieneStartY)));
				schiene.Add(matrix.Transform(new Point2D(curPos + SchienenBreite * measure, schieneStartY)));
				schiene.Add(matrix.Transform(new Point2D(curPos + SchienenBreite * measure, schieneEndY)));
				schiene.Add(matrix.Transform(new Point2D(curPos, schieneEndY)));
				this.schienen.Add(schiene);
				curBeplankungsPos += (SchienenBreite + schienenAbstand);
				curPos += increment;
				if (beplankung.HasValue && curBeplankungsPos >= beplankung.Value.X) {
					if (curBeplankungsPos > beplankung.Value.X) {
						curPos -= (curBeplankungsPos - beplankung.Value.X) * measure;
						ignoreLanes.Add(this.schienen.Count - 1);
					}
					curBeplankungsPos = 0;
				}
			}

			Polygon2D room = new Polygon2D(ceilingCoordinates);
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
			List<Point2D> ceilingCoordinates = this.Product.AssociatedRoom.CeilingCoordinatesToUse;
			List<LineSegment> unusableSegments = GetUnusableSegments(leftBorder, rightBorder, ceilingCoordinates, false, 0.15);
			if (this.Product.AssociatedRoom.CeilingUnusedAreaCoordinates != null) {
				List<LineSegment> tmp;
				foreach (List<Point2D> unusedArea in this.Product.AssociatedRoom.CeilingUnusedAreaCoordinates) {
					tmp = GetUnusableSegments(leftBorder, rightBorder, unusedArea, true, 0);
					unusableSegments.AddRange(tmp);
					LineSegment.NormalizeSegments(unusableSegments);
				}
			}

			List<LineSegment> usableSegments = LineSegment.InvertSegments(unusableSegments);
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
			Vector2D normalizedBorderDirection = new Vector2D(0, 1);
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
			LineSegment.NormalizeSegments(segmentsUnusable);
			if (wallDist != 0) {
				List<LineSegment> segmentsUsable = LineSegment.InvertSegments(segmentsUnusable);
				int i = 0;
				double wallDistAdd = wallDist * this.Product.AssociatedRoom.AssociatedPlan.Measure.Value;
				while (i < segmentsUsable.Count) {
					segmentsUsable[i].Start += wallDistAdd;
					segmentsUsable[i].End -= wallDistAdd;
					if (segmentsUsable[i].Start > segmentsUsable[i].End) {
						segmentsUsable.RemoveAt(i);
					} else {
						i++;
					}
				}
				segmentsUnusable = LineSegment.InvertSegments(segmentsUsable);
			}
			return segmentsUnusable;
		}

		private void CheckLeftBorder(Line2D borderLeft, Segment2D roomBorder, ref bool inside, List<double> bordersTop, List<CompareablePair<double>> removes, ref List<double> possiblePoints, ref bool enteredLeft) {
			Nullable<Point2D> intersection = Line2D.GetIntersection(borderLeft, roomBorder);
			if (intersection.HasValue) {
				possiblePoints.Add(intersection.Value.Y);
				if (inside) {
					if (enteredLeft) {
						removes.Add(new CompareablePair<double>(LineSegment.GetMax(possiblePoints), LineSegment.GetMin(possiblePoints)));
					} else {
						bordersTop.Add(LineSegment.GetMin(possiblePoints));
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
						bordersBottom.Add(LineSegment.GetMax(possiblePoints));
					} else {
						removes.Add(new CompareablePair<double>(LineSegment.GetMax(possiblePoints), LineSegment.GetMin(possiblePoints)));
					}
					possiblePoints = new List<double>();
					inside = false;
				} else {
					inside = true;
					enteredLeft = false;
				}
			}
		}

		[XmlIgnore]
		public override ModulKlimaDeckeProduct.ModulCeilingConstructionEnum CeilingConstruction {
			get { return this.constructionType; }
		}

		public ModulKlimaDeckeProduct.ModulCeilingConstructionEnum ContructionType {
			get { return this.constructionType; }
			set {
                if (this.constructionType != value) {
                    this.schienenBreite = null;
                    this.constructionType = value;
                }
            }
		}

		public double SchienenBreite {
			get {
                if (this.schienenBreite.HasValue) {
                    return this.schienenBreite.Value;
                }
				switch (this.ContructionType) {
					case ModulKlimaDeckeProduct.ModulCeilingConstructionEnum.C_PROFIL:
						return 0.065;

                    case ModulKlimaDeckeProduct.ModulCeilingConstructionEnum.HOLZSTAFFEL:
						return 0.045;

                    default:
						return 0.045;

                }
			}
            set {
                if (this.schienenBreite != value) {
                    this.schienenBreite = value;
                    this.RecalculateSchienen();
                }
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
				double increment = this.beplankung.HasValue ? this.beplankung.Value.X : this.SchienenBreite + this.schienenAbstand;
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
					this.offsetY = 0;
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
                this.RecalculateSchienen();
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
			foreach (Point2D point in this.Product.AssociatedRoom.CeilingCoordinatesToUse) {
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
			if (this.Product == null ||
				this.Product.AssociatedRoom == null ||
				this.CeilingCoordinates == null ||
				this.CeilingCoordinates.Count < 3 ||
				this.Product.AssociatedRoom.AssociatedPlan == null ||
				this.Product.AssociatedRoom.AssociatedPlan.Measure == null) {
				return;
			}
			double measure = this.Product.AssociatedRoom.AssociatedPlan.Measure.Value;
			Matrix4D additionalTransformation = this.AdditionalTransformation;

			GraphicsPath roomPath = this.GetProductAreaPath();
			g.Clip = new Region(roomPath);

			System.Drawing.Color c = System.Drawing.Color.Gray;
			Pen p = new Pen(c);
			Brush b = new HatchBrush(System.Drawing.Drawing2D.HatchStyle.DiagonalCross, c, System.Drawing.Color.FromArgb(0, c));

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
					}
				}
			}

			if (mode == ModulKlimaDeckePlanner.KlimaDeckeMode.KDM_CONSTRUCTION && this.mode == ConstructionModifyMode.MOVE_SCHIENEN) {
				c = System.Drawing.Color.FromArgb(128, 0, 240, 0);
				p = new Pen(c);
				b = new SolidBrush(System.Drawing.Color.FromArgb(64, c));
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

		public override void PaintDxf(DxfModel model, DxfLayer constructionLayer, DxfLayer beplankungLayer, bool drawBeplankung) {
			if (this.Product == null ||
				this.Product.AssociatedRoom == null ||
				this.CeilingCoordinates == null ||
				this.CeilingCoordinates.Count < 3 ||
				this.Product.AssociatedRoom.AssociatedPlan == null ||
				this.Product.AssociatedRoom.AssociatedPlan.Measure == null) {
				return;
			}
			double measure = this.Product.AssociatedRoom.AssociatedPlan.Measure.Value;

			Polygon2D clipRegion = new Polygon2D();
			foreach (Point2D point in this.CeilingCoordinates) {
				clipRegion.Add(point);
			}
			if (clipRegion.IsClockwise()) {
				clipRegion.Reverse();
			}

			EntityColor c = EntityColor.CreateFromRgb(System.Drawing.Color.Gray.ToArgb());

			DxfHatch hatch = new DxfHatch();
			hatch.Color = c;
			
			foreach (Polygon2D schiene in this.GetSchienen(false)) {
				List<Point2D> dxfPoints = new List<Point2D>();
				foreach (Point2D point in schiene) {
					dxfPoints.Add(point);
				}
				Polygon2D clipped = new Polygon2D(dxfPoints);
				if (clipped.IsClockwise()) {
					clipped.Reverse();
				}
				List<Polygon2D> list1 = new List<Polygon2D>();
				list1.Add(clipRegion);
				List<Polygon2D> list2 = new List<Polygon2D>();
				list2.Add(clipped);
				try {
                    IList<Polygon2D> clippedPolygons = null;
                    try {
                        clippedPolygons = Polygon2D.GetIntersection(list1, list2);
                    } catch {
                        clippedPolygons = new List<Polygon2D>();
                    }
					foreach (Polygon2D polygon in clippedPolygons) {
						DxfPolyline2D polyLine = new DxfPolyline2D(c, polygon);
						polyLine.Closed = true;
						polyLine.Layer = constructionLayer;
						model.Entities.Add(polyLine);

						DxfHatch.BoundaryPath boundaryPath = new DxfHatch.BoundaryPath();
						boundaryPath.Type = BoundaryPathType.Polyline;
						boundaryPath.PolylineData = new DxfHatch.BoundaryPath.Polyline(polygon.ToArray());
						boundaryPath.PolylineData.Closed = true;
						hatch.BoundaryPaths.Add(boundaryPath);
					}
				} catch (Exception) {
					// TODO log warning
				}
			}

			hatch.Pattern = new DxfPattern();
			DxfPattern.Line patternLine = new DxfPattern.Line();
			patternLine.Angle = Math.PI / 4d;
			patternLine.Offset = new Vector2D(0.02 * measure, -0.02d * measure);
			hatch.Pattern.Lines.Add(patternLine);
			patternLine = new DxfPattern.Line();
			patternLine.Angle = 3d * Math.PI / 4d;
			patternLine.Offset = new Vector2D(0.02 * measure, 0.02d * measure);
			hatch.Pattern.Lines.Add(patternLine);

			hatch.Layer = constructionLayer;
			model.Entities.Add(hatch);

			if (drawBeplankung && this.beplankung.HasValue && this.beplankungStart.HasValue && this.beplankungEnd.HasValue) {
				Matrix3D matrix = Transformation3D.Rotate(this.Rotation * Math.PI / 180.0);
				for (double beplankungX = this.beplankungStart.Value.X; beplankungX < this.beplankungEnd.Value.X; beplankungX += beplankung.Value.X * measure) {
					for (double beplankungY = this.beplankungStart.Value.Y; beplankungY < this.beplankungEnd.Value.Y; beplankungY += beplankung.Value.Y * measure) {
						Point2D p1 = matrix.Transform(new Point2D(beplankungX, beplankungY));
						Point2D p2 = matrix.Transform(new Point2D(beplankungX + beplankung.Value.X * measure, beplankungY));
						Point2D p3 = matrix.Transform(new Point2D(beplankungX + beplankung.Value.X * measure, beplankungY + beplankung.Value.Y * measure));
						Point2D p4 = matrix.Transform(new Point2D(beplankungX, beplankungY + beplankung.Value.Y * measure));
						Polygon2D clipped = new Polygon2D(new Point2D[] { p1, p2, p3, p4 });

						if (clipped.IsClockwise()) {
							clipped.Reverse();
						}
						List<Polygon2D> list1 = new List<Polygon2D>();
						list1.Add(clipRegion);
						List<Polygon2D> list2 = new List<Polygon2D>();
						list2.Add(clipped);

                        IList<Polygon2D> clippedPolygons = null;
                        try {
                            clippedPolygons = Polygon2D.GetIntersection(list1, list2);
                        } catch {
                            clippedPolygons = new List<Polygon2D>();
                        }
						foreach (Polygon2D polygon in clippedPolygons) {
							DxfPolyline2D polyLine = new DxfPolyline2D(c, polygon);
							polyLine.Closed = true;
							polyLine.Layer = beplankungLayer;
							model.Entities.Add(polyLine);
						}
					}
				}
			}
		}

		public override bool HitTest(Point2D planPoint, Point pointInControl) {
			if (this.Product == null ||
				this.Product.AssociatedRoom == null ||
				this.CeilingCoordinates == null ||
				this.CeilingCoordinates.Count < 3 ||
				this.Product.AssociatedRoom.AssociatedPlan == null ||
				this.Product.AssociatedRoom.AssociatedPlan.Measure == null) {
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
            if (this.mode == ConstructionModifyMode.MOVE_SCHIENEN) {
                Vector2D move = Transformation3D.Rotate(-this.Rotation * Math.PI / 180.0).Transform(planPoint - startPlanPoint);
                this.Offset = startOffset + (move.X / this.Product.AssociatedRoom.AssociatedPlan.Measure.Value);
            } else {
			    if (this.Product == null ||
				    this.Product.AssociatedRoom == null ||
				    this.CeilingCoordinates == null ||
				    this.CeilingCoordinates.Count < 3 ||
				    this.Product.AssociatedRoom.AssociatedPlan == null ||
				    this.Product.AssociatedRoom.AssociatedPlan.Measure == null) {
				    return;
			    }
			    double measure = this.Product.AssociatedRoom.AssociatedPlan.Measure.Value;
                Vector2D move = Transformation3D.Rotate(-this.Rotation * Math.PI / 180.0).Transform(planPoint - startPlanPoint);
                this.BeplankungXShift = (int)Math.Round((move.X / this.Product.AssociatedRoom.AssociatedPlan.Measure.Value) / (this.SchienenAbstand + this.SchienenBreite));
                this.OffsetY = startOffsetY + (move.Y / this.Product.AssociatedRoom.AssociatedPlan.Measure.Value);
            }
			if (this.PlanPanel != null) {
				this.PlanPanel.InvalidateGraphics();
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
