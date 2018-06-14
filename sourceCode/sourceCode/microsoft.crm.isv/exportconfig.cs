using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace Microsoft.Crm.Isv
{
	[XmlRoot("exportconfig")]
	public class ExportConfig
	{
        public static ExportConfig Load(string filePath)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportConfig));
            using (StreamReader reader = new StreamReader(filePath))
            {
                return (ExportConfig)xmlSerializer.Deserialize(reader);
            }
        }

        public void Save(string filePath)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportConfig));
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                xmlSerializer.Serialize(writer, this);
            }
        }        

		[XmlArray("entities")]
		[XmlArrayItem("entity")]
		public List<String> Entities { get; set; }

        public ExportConfig()
            : this(new List<String>())
        {
        }

        public ExportConfig(List<String> entitiesForExport)
        {
            Entities = entitiesForExport;
        }
	}
}
