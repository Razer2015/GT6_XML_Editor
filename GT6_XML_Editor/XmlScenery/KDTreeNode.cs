namespace XmlScenery
{
  public class KDTreeNode
  {
    public float[] position;
    public int axis;
    public int left;
    public int right;
    public int triangleIndex;

    public KDTreeNode()
    {
      this.position = new float[2];
      this.position[0] = this.position[1] = float.MaxValue;
      this.axis = 0;
      this.left = this.right = this.triangleIndex = -1;
    }
  }
}
