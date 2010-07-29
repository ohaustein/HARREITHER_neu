using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {
	public class FloorList : List<Floor> , IGuiRepresentation, IClipboard {

		public Type AssociatedPanelType {
			get { return typeof(FloorListSummaryPanel); }
		}

		public bool SupportsCut {
			get { return false; }
		}

		public bool SupportsCopy {
			get { return false; }
		}

		public string DataFormat {
			get { return this.GetType().ToString(); }
		}

		public bool SupportsPaste(string data) {
			if (data == typeof(Floor).ToString()) {
				return true;
			}
			return false;
		}

		public string SupportedPasteFormat {
			get { return typeof(Floor).ToString(); }
		}

		public object Copy() {
			throw new Exception("Copy not supported");
		}

		public void Paste(object o) {
			if (o.GetType() == typeof(Floor)) {
				this.Add(o as Floor);
			} else {
				throw new Exception("Paste of this type not supported");
			}
		}

		internal void UpdateTree(System.Windows.Forms.TreeNode floorsNode) {
			int i = 0;
			bool expand = floorsNode.Nodes.Count == 0;
			foreach (Floor floor in this) {
				int index = floorsNode.Nodes.IndexOf(floor.Node);
				if (index < 0) {
					floorsNode.Nodes.Insert(i, floor.Node);
				} else if (index > i) {
					if (floor.Node.IsSelected) {
						for (int j = i; j < index; j++) {
							floorsNode.Nodes.RemoveAt(i);
						}
					} else {
						floorsNode.Nodes.RemoveAt(index);
						floorsNode.Nodes.Insert(i, floor.Node);
					}
				}
				floor.UpdateTree();
				i++;
			}
			while (floorsNode.Nodes.Count > i) {
				floorsNode.Nodes.RemoveAt(i);
			}
			if (expand) {
				floorsNode.Expand();
			}
		}
	}
}
