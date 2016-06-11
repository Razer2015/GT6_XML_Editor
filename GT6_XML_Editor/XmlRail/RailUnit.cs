namespace XmlRail
{
  public class RailUnit
  {
    public float[] width;
    public float[] modelWidth;
    public float unitLength;
    public int type;
    public int flag;
    public float checkpoint;
    public float floorHeight;
    public ulong uuid;
    public float[] dispositionA;
    public float[] dispositionB;
    public int decoratedRailType;
    public int[] dispositionType;
    public int[] decoratedRailRot;
    public int crowdCount;

    public RailUnit()
    {
      this.width = new float[4];
      this.modelWidth = new float[2];
      this.dispositionA = new float[4];
      this.dispositionB = new float[4];
      this.dispositionType = new int[2];
      this.decoratedRailRot = new int[2];
    }
  }
}
