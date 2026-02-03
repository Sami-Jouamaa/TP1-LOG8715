using System.Collections.Generic;

public enum CircleRegion
{
    Left,
    Right
    }
public class Regions : IComponent
{
    public static Dictionary<uint, CircleRegion> regions = new();
   
}
