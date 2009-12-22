using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Europlan.Common {
	public class WallConstruction : Construction {

		private double factor = 1;
		private bool isHithermWall = false;
		private bool isHithermCompactWall = false;
		private HithermWall defaultWall = null;

		public WallConstruction() : base() {
		}

		public WallConstruction(string id, string name, ConstructionType type, double factor)
			: base(id, name, type) {
			this.factor = factor;
		}

		public override Construction Clone() {
			WallConstruction construction = new WallConstruction(this.Id, this.Name, this.Type, this.Factor);
			return construction;
		}

		public double Factor {
			get { return this.factor; }
			set { this.factor = value; }
		}

		public bool IsHithermWall {
			get { return this.isHithermWall; }
			set { this.isHithermWall = value; }
		}

		public bool IsHithermCompactWall {
			get { return this.isHithermCompactWall; }
			set { this.isHithermCompactWall = value; }
		}

		[XmlIgnore]
		public HithermWall DefaultWall {
			get {
				if (defaultWall == null) {
					defaultWall = new HithermWall(this.Id, this.Name, this, null, null, false, null, -16, 30, true);
				}
				return defaultWall;
			}
		}
	}
}
