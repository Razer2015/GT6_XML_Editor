using System.Xml.Serialization;

namespace XmlScenery
{
  [XmlRoot("KDTree")]
  public class KDTREE
  {
    [XmlElement("nodes")]
    public KDTreeNodes nodes;
    public float[] aabb_min;
    public float[] aabb_max;
  }
}
