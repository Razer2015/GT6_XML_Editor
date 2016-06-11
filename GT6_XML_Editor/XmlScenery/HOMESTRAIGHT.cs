using System.Collections.Generic;
using System.Xml.Serialization;

namespace XmlScenery
{
    [XmlRoot("HomeStraight")]
    public class HOMESTRAIGHT
    {
        [XmlElement("position")]
        public List<float[]> Positions = new List<float[]>();
        [XmlElement("rotation")]
        public List<float> Rotation = new List<float>();
        [XmlElement("width")]
        public List<float> Width = new List<float>();
        [XmlElement("length")]
        public List<float> Length = new List<float>();
    }
}
