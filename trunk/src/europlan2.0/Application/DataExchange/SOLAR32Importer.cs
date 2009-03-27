using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Application {
	
	public class SOLAR32Importer : IBuildingDataImporter{

		public SOLAR32Importer() {

		}

		public string FileExtension {
			get { 
				return ".h72";
			}
		}

		public string FileExtensionFilter {
			get { 
				return "SOLAR 32 (*.h72)|*.h72";
			}
		}

		public IList<Floor> ImportBuildingDataFromFile(string fileName) {
			return null;
		}

	}

}
