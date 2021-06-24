using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using CodeTestSenior.Library.Serialization;

namespace CodeTestSenior.Library
{
    public class CodonSerialization
    {
        public CodonSerialization()
        {
            /// Needed for deserializtion of Windows-1252 encoded xml
            System.Text.EncodingProvider ppp = System.Text.CodePagesEncodingProvider.Instance;
            Encoding.RegisterProvider(ppp);
        }
        
        public CodonTable Deserialize(string codonTableFileName)
        {
            var extension =  Path.GetExtension(codonTableFileName);

            switch (extension)
            {
                case ".json":
                    return DeserializeJson(codonTableFileName);
                case ".xml":
                    return DeserializeXml(codonTableFileName);
                case ".txt":
                    return DeserializeCsv(codonTableFileName);
                default:
                    throw new NotSupportedException($"Unable to deserialize {extension} files");
            }
        }

        private CodonTable DeserializeCsv(string codonTableFileName)
        {
            var serializer = new CsvSerialization();
            return serializer.Deserialize(codonTableFileName);
        }

        private CodonTable DeserializeXml(string codonTableFileName)
        {
            using var fs = new FileStream(codonTableFileName, FileMode.Open);
            var serializer = new XmlSerializer(typeof(Data));
            var xmlCodonTable = serializer.Deserialize(fs) as Data;

            return xmlCodonTable?.ToCodonTable() ?? throw new FormatException($"Unable to deserialize xml {codonTableFileName}");

        }

        private CodonTable DeserializeJson(string codonTableFileName)
        {
            var fileContents = File.ReadAllText(codonTableFileName);
            return System.Text.Json.JsonSerializer.Deserialize<CodonTable>(fileContents) ?? throw new FormatException($"Unable to deserialize json {codonTableFileName}");
        }
    }
}