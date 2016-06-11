using System.Collections.Generic;
using System.Xml.Serialization;

namespace XmlScenery
{
  [XmlRoot("SurfaceAttrTriangle")]
  public class SurfaceAttrTriangles
  {
    [XmlElement("vertexIndices")]
    public List<int[]> vertexIndices;
    [XmlElement("attributes")]
    public List<int> attributes;

    public SurfaceAttrTriangles()
    {
      this.vertexIndices = new List<int[]>();
      this.attributes = new List<int>();
    }

    public Triangle getTriangle(int param0)
    {
      if (param0 < 0 || this.vertexIndices == null || (this.vertexIndices.Count < param0 || this.attributes == null) || this.attributes.Count < param0)
        return new Triangle();
      return new Triangle()
      {
        attributes = this.attributes[param0],
        vertexIndices = this.vertexIndices[param0]
      };
    }
  }
}
