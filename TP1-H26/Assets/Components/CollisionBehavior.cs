using System.Collections.Generic;

public enum Behavior
{
    Static,
    Dynamic
    }
public class CollisionBehavior : IComponent
{  
    public static Dictionary<uint, Behavior> behaviors = new Dictionary<uint, Behavior>();
    
}
