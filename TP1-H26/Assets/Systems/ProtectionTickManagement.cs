using System;
using System.Linq;
using UnityEngine;

public class ProtectionTickManagement : ISystem
{
    public ECSController controller = ECSController.Instance;
    public string Name => "ProtectionTickManagement";

    public void UpdateSystem()
    {
        foreach (uint id in Protections.protections.Keys.ToList())
            if (SimStep.currentSimStep == 0 || Regions.regions.TryGetValue(id, out var region) && region == CircleRegion.Left)
                ProtectionTick(id);
    }

    public void ProtectionTick(uint id)
    {
        if (!Protections.protections.ContainsKey(id)) return;
        float newProtectionRemaining = Math.Max(Protections.protections[id].Remaining - Time.deltaTime, 0);
        float newProtectionCooldown = Math.Max(Protections.protections[id].Cooldown - Time.deltaTime, 0);
        if (newProtectionCooldown <= 0)
        {
            Protections.protections.Remove(id);
            CollisionCount.collisionCount[id] = 0;
        }
        else
        {
            Protections.protections[id] = new ProtectionTimers { Remaining = newProtectionRemaining, Cooldown = newProtectionCooldown };
        }
    }
}
