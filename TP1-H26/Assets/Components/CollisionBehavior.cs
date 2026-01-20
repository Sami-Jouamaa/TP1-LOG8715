using System.Collections.Generic;

public class CollisionBehavior
{  
    public enum Behavior
    {
        Static,
        Dynamic,
        Protected
    }
    public static Dictionary<uint, Behavior> behaviors = new Dictionary<uint, Behavior>();
    
}
