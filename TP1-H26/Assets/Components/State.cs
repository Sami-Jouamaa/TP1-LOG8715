using System.Collections.Generic;

public class State
{  
    public enum CircleState
    {
        Static,
        Dynamic,
        Protected
    }
    public static Dictionary<uint, CircleState> sizes = new Dictionary<uint, CircleState>();
    
}
