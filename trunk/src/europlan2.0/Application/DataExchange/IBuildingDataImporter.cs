using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Application {

	public interface IBuildingDataImporter {

		string FileExtension {
			get;
		}

		string FileExtensionFilter {
			get;
		}

		List<Floor> ImportBuildingDataFromFile(string fileName);

	}

}
