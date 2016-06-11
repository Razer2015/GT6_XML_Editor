using System.Collections.Generic;
using System.Xml.Serialization;

namespace XmlScenery
{
    [XmlRoot("nodes")]
    public class KDTreeNodes
    {
        [XmlElement("position")]
        public List<float[]> position = new List<float[]>();
        [XmlElement("axis")]
        public List<int> axis = new List<int>();
        [XmlElement("left")]
        public List<int> left = new List<int>();
        [XmlElement("right")]
        public List<int> right = new List<int>();
        [XmlElement("triangleIndex")]
        public List<int> triangleIndex = new List<int>();

        public KDTreeNodes()
        {
            this.position = new List<float[]>();
            this.axis = new List<int>();
            this.left = new List<int>();
            this.right = new List<int>();
            this.triangleIndex = new List<int>();
        }

        public KDTreeNode getKDTreeNode(int param0)
        {
            if (param0 < 0 || this.position == null || this.position.Count < param0)
                return new KDTreeNode();
            return new KDTreeNode()
            {
                position = this.position[param0],
                axis = this.axis[param0],
                left = this.left[param0],
                right = this.right[param0],
                triangleIndex = this.triangleIndex[param0]
            };
        }
    }
}
