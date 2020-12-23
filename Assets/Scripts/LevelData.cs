using System;

[Serializable]
public class LevelData
{
    public Values[] val;
}

[Serializable]
public class Values
{
    public int[] Red;
    public int[] Green;
    public int[] Blue;
    public int[] Orange;
    public int[] Purple;
}
