namespace XmlScenery
{
  public class Result
  {
    public int triangleIndex;
    public float[] hitPos;

    public Result()
    {
      this.triangleIndex = -1;
      this.hitPos = new float[3];
      this.hitPos[0] = this.hitPos[1] = this.hitPos[2] = float.MaxValue;
    }

    public Result(int param0, float[] param1 = null)
    {
      this.triangleIndex = param0;
      this.hitPos = new float[3];
      if (param1 != null)
      {
        this.hitPos[0] = param1[0];
        this.hitPos[1] = param1[1];
        this.hitPos[2] = param1[2];
      }
      else
        this.hitPos[0] = this.hitPos[1] = this.hitPos[2] = float.MaxValue;
    }
  }
}
