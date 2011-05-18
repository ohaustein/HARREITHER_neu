using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using WW.Math.Geometry;

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

		public abstract bool HitTest(WW.Math.Point2D planPoint, double xOffset, double yOffset);
		public abstract void PaintObject(System.Drawing.Graphics g, double xOffset, double yOffset, IGraphicalWallObject selectedObject, double scale);
		public abstract void PaintObject(System.Drawing.Graphics g, double xOffset, double yOffset, IGraphicalWallObject selectedObject, double scale, bool error);
		public abstract IGraphicalWallObject GetPickedObject(WW.Math.Point2D planPoint, double xOffset, double yOffset);
		public abstract WW.Math.Geometry.Polygon2D GetObjectBorders(double xOffset, double yOffset);
		public abstract bool StartDrag(Anchor anchor, WW.Math.Point2D planPoint, GraphicalWall owningWall);
		public abstract bool MoveDrag(Anchor anchor, WW.Math.Point2D planPoint, GraphicalWall owningWall);
		public abstract bool EndDrag(Anchor anchor, WW.Math.Point2D planPoint, GraphicalWall owningWall);
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

		public bool PositionAndSizeOk(GraphicalWall owningWall, double offsetX, double offsetY) {
			Polygon2D borders = this.GetObjectBorders(offsetX, offsetY);
			if (this.Width < 10 || this.Height < 10) {
				return false;
			}
			if (owningWall.CollisionTest(borders, offsetX, offsetY, true)) {
				return false;
			}
			return true;
		}

		public bool CollisionTest(WW.Math.Geometry.Polygon2D polygon, double xOffset, double yOffset, bool ignoreBorders) {
			Polygon2D door;
			if (!ignoreBorders) {
				door = GetOutsideBorder(xOffset, yOffset);
			} else {
				door = GetObjectBorders(xOffset, yOffset);
			}

			if (polygon.IsClockwise()) {
				polygon.Reverse();
			}
			if (door.IsClockwise()) {
				door.Reverse();
			}
			List<Polygon2D> list1 = new List<Polygon2D>();
			list1.Add(polygon);
			List<Polygon2D> list2 = new List<Polygon2D>();
			list2.Add(door);

			return Polygon2D.GetIntersection(list1, list2).Count > 0;
		}

		public abstract Polygon2D GetOutsideBorder(double xOffset, double yOffset);

		protected Polygon2D GetOutsideBorder(Polygon2D border) {
			Polygon2D usableArea = new Polygon2D(border);
			usableArea.Outset(this.BorderDistance * 100.0);
			return usableArea;
		}
	}

}
