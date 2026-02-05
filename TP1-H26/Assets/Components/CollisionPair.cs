using System.Collections.Generic;
public class CollisionPair : IComponent
{
    public static Dictionary<uint, List<uint>> collisionPairs = new();
}
