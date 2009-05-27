using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Europlan.Common {

	[XmlInclude(typeof(EurovalProduct))]
	public abstract class Product {

		protected static readonly object padlock = new object();

		protected Product() {

		}


	}
}
