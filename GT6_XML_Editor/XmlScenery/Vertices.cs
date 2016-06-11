using System.Collections.Generic;
using System.Xml.Serialization;

namespace XmlScenery
{
    [XmlRoot("Vertex")]
    public class Vertices
    {
        [XmlElement("data")]
        public List<float[]> data = new List<float[]>();

        public Vertex getVertex(int param0)
        {
            if ((param0 >= -1) && (this.data != null) && (this.data.Count >= param0))
            {
                return new Vertex { data = this.data[param0] };

            }
            return new Vertex();
        }
    }
}
