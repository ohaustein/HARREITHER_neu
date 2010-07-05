using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Europlan.Common {

	public interface IRequiredMaterial {

		void CalculateRequiredMaterial(SerializableDictionary<string, double> requiredMaterial);

	}

}
