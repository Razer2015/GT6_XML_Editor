namespace XmlScenery
{
  public class Triangle
  {
    public int[] vertexIndices;
    public int attributes;
    public int attrModelSetIndex;

    public Triangle()
    {
      this.vertexIndices = new int[3];
      this.vertexIndices[0] = this.vertexIndices[1] = this.vertexIndices[2] = 0;
      this.attributes = -1;
      this.attrModelSetIndex = -1;
    }

    public enum SurfaceAttr
    {
      SurfaceAttr_Forest = 1,
      SurfaceAttr_Water = 2,
      SurfaceAttr_Rural = 4,
      SurfaceAttr_City = 8,
      SurfaceAttr_Mountain = 16,
      SurfaceAttr_Rock = 32,
      SurfaceAttr_Veld = 64,
      SurfaceAttr_Desert = 128,
      SurfaceAttr_HomeStraight = 256,
      SurfaceAttr_Normal = 512,
      SurfaceAttr_CornerRight = 1024,
      SurfaceAttr_CornerLeft = 2048,
      SurfaceAttr_Bridge = 4096,
      SurfaceAttr_CliffRight = 8192,
      SurfaceAttr_CliffLeft = 16384,
      SurfaceAttr_NoEntry = 32768,
      SurfaceAttr_PlacementArea = 65536,
    }
  }
}
