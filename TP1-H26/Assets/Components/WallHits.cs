using System.Collections.Generic;
public enum WallPos
{
    Top,
    Bottom,
    Left,
    Right
    }
public class WallHits : IComponent
{
    public static Dictionary<uint, WallPos> wallHits = new();
    
}
