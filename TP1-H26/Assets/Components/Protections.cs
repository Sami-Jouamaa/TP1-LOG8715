using System.Collections.Generic;

public struct ProtectionTimers
{
    public float Remaining;
    public float Cooldown;
}
public class Protections : IComponent
{
    public static Dictionary<uint, ProtectionTimers> protections = new();
}
