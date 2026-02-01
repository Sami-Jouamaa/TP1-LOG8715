using System.Collections.Generic;
enum WallOrientation
{
    Vertical = 1,
    Horizontal = 2
}
public class CollisionPair : IComponent
{
    public static Dictionary<uint, List<int>> collisionPairs = new();
}
