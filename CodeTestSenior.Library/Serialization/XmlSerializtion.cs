using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace CodeTestSenior.Library.Serialization
{

    //Generated using Json2csharp.com 
    [XmlRoot(ElementName="Starts")]
    public class Starts { 

        [XmlElement(ElementName="string")] 
        public List<string> String { get; set; } 
    }

    [XmlRoot(ElementName="Stops")]
    public class Stops { 

        [XmlElement(ElementName="string")] 
        public List<string> String { get; set; } 
    }

    [XmlRoot(ElementName="CodonPair")]
    public class CodonPair { 

        public string Codon { get; set; } 

        public string AminoAcid { get; set; } 
    }

    [XmlRoot(ElementName="CodonMap")]
    public class CodonMap { 

        [XmlElement(ElementName="CodonPair")] 
        public List<CodonPair> CodonPair { get; set; } 
    }

    [XmlRoot(ElementName="Data")]
    public class Data { 

        [XmlElement(ElementName="Starts")] 
        public Starts Starts { get; set; } 

        [XmlElement(ElementName="Stops")] 
        public Stops Stops { get; set; } 

        [XmlElement(ElementName="CodonMap")] 
        public CodonMap CodonMap { get; set; } 

        [XmlAttribute(AttributeName="xsd")] 
        public string Xsd { get; set; } 

        [XmlAttribute(AttributeName="xsi")] 
        public string Xsi { get; set; } 

        [XmlText] 
        public string Text { get; set; }

        
        /// <summary>
        /// Maps the XML generated objects to the more general use CodonTable object. I'm not a huge for of this approach, but I'm not willing
        /// to spend any more time on this area.
        /// </summary>
        /// <returns></returns>
        public CodonTable ToCodonTable()
        {
            var result = new CodonTable()
            {
                Starts = new HashSet<string>(Starts.String),
                Stops = new HashSet<string>(Stops.String),
                CodonMap = new List<Library.CodonMap>(CodonMap.CodonPair.Select(item => new Library.CodonMap()
                    {Codon = item.Codon, AminoAcid = item.AminoAcid}))
            };
            return result;
        }
    }


}