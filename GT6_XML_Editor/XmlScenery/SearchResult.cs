namespace XmlScenery
{
  public class SearchResult
  {
    public int triangleIndex;
    public int attr;
    public double[] hitPos;

    public SearchResult()
    {
      this.triangleIndex = -1;
      this.attr = -1;
      this.hitPos = new double[3];
      this.hitPos[0] = this.hitPos[1] = this.hitPos[2] = 3.40282346638529E+38;
    }
  }
}
