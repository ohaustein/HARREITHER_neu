using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using WW.Math.Geometry;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Europlan.Common {

	[XmlInclude(typeof(GraphicalDoor))]
	[XmlInclude(typeof(GraphicalWindow))]
	[XmlInclude(typeof(GraphicalOtherObstacle))]
	public abstract class GraphicalWallObstacle : IGraphicalWallObject {

		public class ObstacleTypeConverter : System.ComponentModel.TypeConverter {
			private static readonly string door = EuroplanRes.GraphicalWallObstacle_Door; //"Tür";
			private static readonly string window = EuroplanRes.GraphicalWallObstacle_Window; //"Fenster";
			private static readonly string windowTriangleLeft = EuroplanRes.GraphicalWallObstacle_WindowTriangleLeft; //"Dreiecksfenster Links";
			private static readonly string windowTriangleRight = EuroplanRes.GraphicalWallObstacle_WindowTriangleRight; //"Dreiecksfenster Rechts";
			private static readonly string other = EuroplanRes.GraphicalWallObstacle_Other; //"Anderes";

			private Dictionary<string, ObstacleTypeEnum> mappingFromString = new Dictionary<string, ObstacleTypeEnum>();
			private Dictionary<ObstacleTypeEnum, string> mappingToString = new Dictionary<ObstacleTypeEnum, string>();

			public ObstacleTypeConverter() {
				mappingFromString.Add(door, ObstacleTypeEnum.Door);
				mappingFromString.Add(window, ObstacleTypeEnum.Window);
				mappingFromString.Add(windowTriangleLeft, ObstacleTypeEnum.WindowTriangleLeft);
				mappingFromString.Add(windowTriangleRight, ObstacleTypeEnum.WindowTriangleRight);
				mappingFromString.Add(other, ObstacleTypeEnum.Other);

				mappingToString.Add(ObstacleTypeEnum.Door, door);
				mappingToString.Add(ObstacleTypeEnum.Window, window);
				mappingToString.Add(ObstacleTypeEnum.WindowTriangleLeft, windowTriangleLeft);
				mappingToString.Add(ObstacleTypeEnum.WindowTriangleRight, windowTriangleRight);
				mappingToString.Add(ObstacleTypeEnum.Other, other);
			}

			public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext context, Type sourceType) {
				return sourceType == typeof(string);
			}

			public override bool CanConvertTo(System.ComponentModel.ITypeDescriptorContext context, Type destinationType) {
				return destinationType == typeof(string);
			}

			public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value) {
				if (value is string) {
					if (mappingFromString.ContainsKey((string)value)) {
						return mappingFromString[(string)value];
					}
				}
				return base.ConvertFrom(context, culture, value);
			}

			public override object ConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType) {
				if (value is ObstacleTypeEnum && destinationType == typeof(string)) {
					if (mappingToString.ContainsKey((ObstacleTypeEnum)value)) {
						return mappingToString[(ObstacleTypeEnum)value];
					}
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}
		}

		[System.ComponentModel.TypeConverter(typeof(ObstacleTypeConverter))]
		public enum ObstacleTypeEnum {
			Door,
			Window,
			WindowTriangleLeft,
			WindowTriangleRight,
			Other
		}

		protected bool error = false;

		public abstract bool HitTest(WW.Math.Point2D planPoint, double xOffset, double yOffset);
		public abstract void PaintObject(System.Drawing.Graphics g, double xOffset, double yOffset, IGraphicalWallObject selectedObject, double scale, bool export);
		public abstract void PaintObject(System.Drawing.Graphics g, double xOffset, double yOffset, IGraphicalWallObject selectedObject, double scale, bool error, bool export);
		public abstract IGraphicalWallObject GetPickedObject(WW.Math.Point2D planPoint, double xOffset, double yOffset);
		public abstract List<WW.Math.Geometry.Polygon2D> GetObjectBorders(double xOffset, double yOffset);
		public abstract bool StartDrag(Anchor anchor, WW.Math.Point2D planPoint, GraphicalWall owningWall, Room owningRoom, Product owningProduct, bool useSnap);
		public abstract bool MoveDrag(Anchor anchor, WW.Math.Point2D planPoint, GraphicalWall owningWall, Room owningRoom, Product owningProduct, bool useSnap);
		public abstract bool EndDrag(Anchor anchor, WW.Math.Point2D planPoint, GraphicalWall owningWall, Room owningRoom, Product owningProduct, bool useSnap);
		public abstract List<Anchor> GetAnchors(double scale);
		public abstract void BackupState();
		public abstract void RevertState();
		public abstract bool IsMoveable {
			get;
		}
		public abstract double BorderDistance {
			get;
			set;
		}
		public abstract ObstacleTypeEnum ObstacleType {
			get;
			set;
		}
		public abstract double GraphPosX {
			get;
			set;
		}
		public abstract double GraphPosY {
			get;
			set;
		}
		public abstract double Width {
			get;
			set;
		}
		public abstract double Height {
			get;
			set;
		}
		[XmlIgnore]
		public bool Error {
			get { return this.error; }
			set { this.error = value; }
		}

		public double GetGraphPosXLeft(GraphicalWall owningWall) {
			return GraphPosX;
		}

		public void SetGraphPosXLeft(double graphPosXLeft, GraphicalWall owningWall) {
			if (owningWall != null && graphPosXLeft >= 0) {
				if (graphPosXLeft + Width <= (owningWall.GetWallWidth() * 100.0)) {
					GraphPosX = graphPosXLeft;
				}
			}
		}

		public double GetGraphPosXRight(GraphicalWall owningWall) {
			double result = 0;
			if (owningWall != null) {
				result = (owningWall.GetWallWidth() * 100.0) - (GraphPosX + Width);
			}
			return result;
		}

		public void SetGraphPosXRight(double graphPosXRight, GraphicalWall owningWall) {
			if (owningWall != null && graphPosXRight >= 0) {
				GraphPosX = (owningWall.GetWallWidth() * 100.0) - graphPosXRight - Width;
			}
		}

		public Nullable<double> GetGraphDistanceXLeft(GraphicalWall owningWall) {
			Nullable<double> result = null;
			if (owningWall != null) {
				foreach (GraphicalWallObstacle obstacle in owningWall.Obstacles) {
					if (obstacle != this) {
						if ((obstacle.GraphPosX + obstacle.Width) <= GraphPosX) {
							if (!result.HasValue) {
								result = GraphPosX - (obstacle.GraphPosX + obstacle.Width);
							} else {
								double distance = GraphPosX - (obstacle.GraphPosX + obstacle.Width);
								if (distance < result) {
									result = distance;
								}
							}
						}
					}
				}
			}
			return result;
		}

		public void SetGraphDistanceXLeft(double graphDistanceXLeft, GraphicalWall owningWall) {
			if (owningWall != null && graphDistanceXLeft >= 0) {
				GraphicalWallObstacle closest = null;
				double result = Double.PositiveInfinity;
				foreach (GraphicalWallObstacle obstacle in owningWall.Obstacles) {
					if (obstacle != this) {
						if ((obstacle.GraphPosX + obstacle.Width) <= GraphPosX) {
							double distance = GraphPosX - (obstacle.GraphPosX + obstacle.Width);
							if (distance < result) {
								result = distance;
								closest = obstacle;
							}
						}
					}
				}
				if (closest != null) {
					GraphPosX = closest.GraphPosX + closest.Width + graphDistanceXLeft;
				}
			}
		}

		public Nullable<double> GetGraphMiddleDistanceXLeft(GraphicalWall owningWall) {
			Nullable<double> result = null;
			if (owningWall != null) {
				foreach (GraphicalWallObstacle obstacle in owningWall.Obstacles) {
					if (obstacle != this) {
						if ((obstacle.GraphPosX + (obstacle.Width / 2.0)) <= (GraphPosX + (Width / 2.0))) {
							if (!result.HasValue) {
								result = (GraphPosX + (Width / 2.0)) - (obstacle.GraphPosX + (obstacle.Width / 2.0));
							} else {
								double distance = (GraphPosX + (Width / 2.0)) - (obstacle.GraphPosX + (obstacle.Width / 2.0));
								if (distance < result) {
									result = distance;
								}
							}
						}
					}
				}
			}
			return result;
		}

		public void SetGraphMiddleDistanceXLeft(double graphDistanceXLeft, GraphicalWall owningWall) {
			if (owningWall != null && graphDistanceXLeft >= 0) {
				GraphicalWallObstacle closest = null;
				double result = Double.PositiveInfinity;
				foreach (GraphicalWallObstacle obstacle in owningWall.Obstacles) {
					if (obstacle != this) {
						if ((obstacle.GraphPosX + (obstacle.Width / 2.0)) <= (GraphPosX + (Width / 2.0))) {
							double distance = (GraphPosX + (Width / 2.0)) - (obstacle.GraphPosX + (obstacle.Width / 2.0));
							if (distance < result) {
								result = distance;
								closest = obstacle;
							}
						}
					}
				}
				if (closest != null) {
					GraphPosX = (closest.GraphPosX + (closest.Width / 2.0)) + graphDistanceXLeft - (Width / 2.0);
				}
			}
		}

		public Nullable<double> GetGraphDistanceXRight(GraphicalWall owningWall) {
			Nullable<double> result = null;
			if (owningWall != null) {
				foreach (GraphicalWallObstacle obstacle in owningWall.Obstacles) {
					if (obstacle != this) {
						if (GraphPosX + Width <= obstacle.GraphPosX) {
							if (!result.HasValue) {
								result = obstacle.GraphPosX - (GraphPosX + Width);
							} else {
								double distance = obstacle.GraphPosX - (GraphPosX + Width);
								if (distance < result) {
									result = distance;
								}
							}
						}
					}
				}
			}
			return result;
		}

		public void SetGraphDistanceXRight(double graphDistanceXRight, GraphicalWall owningWall) {
			if (owningWall != null && graphDistanceXRight >= 0) {
				GraphicalWallObstacle closest = null;
				double result = Double.PositiveInfinity;
				foreach (GraphicalWallObstacle obstacle in owningWall.Obstacles) {
					if (obstacle != this) {
						if (GraphPosX + Width <= obstacle.GraphPosX) {
							double distance = obstacle.GraphPosX - (GraphPosX + Width);
							if (distance < result) {
								result = distance;
								closest = obstacle;
							}
						}
					}
				}
				if (closest != null) {
					GraphPosX = closest.GraphPosX - graphDistanceXRight - Width;
				}
			}
		}

		public Nullable<double> GetGraphMiddleDistanceXRight(GraphicalWall owningWall) {
			Nullable<double> result = null;
			if (owningWall != null) {
				foreach (GraphicalWallObstacle obstacle in owningWall.Obstacles) {
					if (obstacle != this) {
						if ((GraphPosX + (Width / 2.0) <= (obstacle.GraphPosX + (obstacle.Width / 2.0)))) {
							if (!result.HasValue) {
								result = ((obstacle.GraphPosX + (obstacle.Width / 2.0) - GraphPosX + (Width / 2.0)));
							} else {
								double distance = ((obstacle.GraphPosX + (obstacle.Width / 2.0) - GraphPosX + (Width / 2.0)));
								if (distance < result) {
									result = distance;
								}
							}
						}
					}
				}
			}
			return result;
		}

		public void SetGraphMiddleDistanceXRight(double graphDistanceXRight, GraphicalWall owningWall) {
			if (owningWall != null && graphDistanceXRight >= 0) {
				GraphicalWallObstacle closest = null;
				double result = Double.PositiveInfinity;
				foreach (GraphicalWallObstacle obstacle in owningWall.Obstacles) {
					if (obstacle != this) {
						if ((GraphPosX + (Width / 2.0) <= (obstacle.GraphPosX + (obstacle.Width / 2.0)))) {
							double distance = ((obstacle.GraphPosX + (obstacle.Width / 2.0) - GraphPosX + (Width / 2.0)));
							if (distance < result) {
								result = distance;
								closest = obstacle;
							}
						}
					}
				}
				if (closest != null) {
					GraphPosX = (closest.GraphPosX + (closest.Width / 2.0)) - graphDistanceXRight - (Width / 2.0);
				}
			}
		}



		public bool CheckValidity(GraphicalWall owningWall, double offsetX, double offsetY) {
			List<Polygon2D> borders = this.GetObjectBorders(offsetX, offsetY);
			if (this.Width < 10 || this.Height < 10) {
				return false;
			}
			if (owningWall.CollisionTest(borders, offsetX, offsetY, true)) {
				return false;
			}
			return true;
		}

		public bool CollisionTest(IList<WW.Math.Geometry.Polygon2D> polygon, double xOffset, double yOffset, bool ignoreBorders) {
			List<Polygon2D> door;
			if (!ignoreBorders) {
				door = GetOutsideBorder(xOffset, yOffset);
			} else {
				door = GetObjectBorders(xOffset, yOffset);
			}

			/*if (polygon.IsClockwise()) {
				polygon.Reverse();
			}
			if (door.IsClockwise()) {
				door.Reverse();
			}
			List<Polygon2D> list1 = new List<Polygon2D>();
			list1.Add(polygon);
			List<Polygon2D> list2 = new List<Polygon2D>();
			list2.Add(door);*/

			try {
				return Polygon2D.GetIntersection(polygon, door).Count > 0;
#if DEBUG
			} catch (Exception e) {
				Console.WriteLine(e);
				return true;
			}
#else
			} catch {
				return true;
			}
#endif
		}

		public abstract List<Polygon2D> GetOutsideBorder(double xOffset, double yOffset);

		protected Polygon2D GetOutsideBorder(Polygon2D border) {
			Polygon2D usableArea = new Polygon2D(border);
			usableArea.Outset(this.BorderDistance * 100.0);
			return usableArea;
		}

		protected Pen GetObstacleBorderPen(double scale, bool selected, bool error) {
			Pen pen = selected ? new Pen(Color.FromArgb(255, 0, 0), (float)(1.0 / scale)) : new Pen(Color.Black, (float)(1.0 / scale));
			if (error || this.error) {
				pen.DashPattern = new float[] { 1, 2 };
			}
			return pen;
		}

		protected Brush GetObstacleBrush(double scale, bool selected, bool error) {
			return new SolidBrush(error ? Color.FromArgb(127, SystemColors.ControlLight) : SystemColors.ControlLight);
		}

		protected Pen GetUnusableBorderPen(double scale, bool selected, bool error) {
			Pen pen = selected ? new Pen(Color.FromArgb(127, 63, 63), (float)(1.0 / scale)) : new Pen(Color.Gray, (float)(1.0 / scale));
			if (error || this.error) {
				pen.DashPattern = new float[] { 1, 2 };
			}
			return pen;
		}

		protected Brush GetUnusableBrush(double scale, bool selected, bool error) {
			Color c = selected ? Color.FromArgb(127, 63, 63) : Color.Gray;
			if (error) {
				c = Color.FromArgb(127, c);
			}
			return new HatchBrush(HatchStyle.BackwardDiagonal, c, Color.Transparent);
		}

		protected Brush GetUnusableBrushForOther(double scale, bool selected, bool error) {
			Color fg = Color.Gray;
			Color bg = SystemColors.ControlLight;
			if (error) {
				fg = Color.FromArgb(127, fg);
				bg = Color.FromArgb(127, bg);
			}
			return new HatchBrush(HatchStyle.BackwardDiagonal, fg, bg);
		}

		private bool isNew = false;
		[XmlIgnore]
		public bool IsNew {
			get { return this.isNew; }
			set { this.isNew = value; }
		}

		public abstract bool SnapToHelplines(List<double> helplines, bool snapTop, bool snapBottom);
	}

}
