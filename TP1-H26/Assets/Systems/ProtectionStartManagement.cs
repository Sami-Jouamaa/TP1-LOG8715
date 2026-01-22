using UnityEngine;
using System;
using System.Collections.Generic;
using NUnit.Framework;

public class ProtectionStartManagement : ISystem
{
    public ECSController controller = ECSController.Instance;
    public string Name => "ProtectionStartManagement";
    public void UpdateSystem()
    {
        foreach (var (id, size) in Sizes.sizes) {
            if (size > controller.Config.protectionSize) 
                continue;
            if (CollisionCount.collisionCount[id] < controller.Config.protectionCollisionCount) 
                continue;
            if (Protections.protections.ContainsKey(id)) 
                continue;

            Protections.protections.Add(id, new ProtectionTimers{Remaining = controller.Config.protectionDuration, Cooldown = controller.Config.protectionCooldown});
        }
    }
}
