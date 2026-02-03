using System;
using System.Linq;
using UnityEngine;

public class ProtectionTickManagement : ISystem
{
    public ECSController controller = ECSController.Instance;
    public string Name => "ProtectionTickManagement";

    public void UpdateSystem()
    {
        foreach (var leftSideId in LeftSideCircles.circlesOnLeftSide)
        {
            for (int fasterIteration = 0; fasterIteration < 4; fasterIteration++)
            {
                ProtectionTick(leftSideId);
            }
        }

        foreach (var rightSideId in RightSideCircles.circlesOnRightSide)
        {
            ProtectionTick(rightSideId);
        }

        // foreach (var (id, protection) in Protections.protections.ToList())
        // {
        //     {
        //         float newProtectionRemaining = Math.Max(protection.Remaining - Time.deltaTime, 0);
        //         float newProtectionCooldown = Math.Max(protection.Cooldown - Time.deltaTime, 0);
        //         if (newProtectionCooldown <= 0)
        //         {
        //             Protections.protections.Remove(id);
        //             CollisionCount.collisionCount[id] = 0;
        //         }
        //         else
        //             Protections.protections[id] = new ProtectionTimers { Remaining = newProtectionRemaining, Cooldown = newProtectionCooldown };
        //     }
        // }
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
