using System.Collections.Generic;

public class CollisionPair : IComponent
{
    public static Dictionary<uint, List<int>> collisionPairs = new Dictionary<uint, List<int>>();
}
