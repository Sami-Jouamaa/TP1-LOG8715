using System.Collections.Generic;
public enum ExplosionState
{
    Exploding,
    Debris
    }
public class Explosion : IComponent
{
    public static Dictionary<uint, ExplosionState> explosions = new();
}
