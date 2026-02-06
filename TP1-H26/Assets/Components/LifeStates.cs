using System.Collections.Generic;
public enum LifeState
{
    Alive,
    Dead
}
public class LifeStates : IComponent
{
    public static Dictionary<uint, LifeState> lifeStates = new();
}
