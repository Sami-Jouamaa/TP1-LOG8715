using System.Collections.Generic;
using UnityEngine;

public struct ProtectionTimers
{
    public float Remaining;
    public float Cooldown;
}
public class Protections : IComponent
{
    public static Dictionary<uint, ProtectionTimers> protections = new Dictionary<uint, ProtectionTimers>();
}
