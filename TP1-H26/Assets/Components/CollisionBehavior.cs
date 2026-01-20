using System.Collections.Generic;

public enum Behavior
{
    Static,
    Dynamic,
    Protected
}
public class CollisionBehavior
{  
    public static Dictionary<uint, Behavior> behaviors = new Dictionary<uint, Behavior>();
    
}
