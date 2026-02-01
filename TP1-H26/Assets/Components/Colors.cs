using System.Collections.Generic;

public enum CircleColor
{
    Red,
    Green,
    White,
    LightBlue,
    Yellow,
    Orange,
    DarkBlue,
    Pink
    }
public class Colors : IComponent
{  
    public static Dictionary<uint, CircleColor> colors = new();
    
}
