using System;
using System.Linq;
using UnityEngine;

public class ProtectionTickManagement : ISystem
{
    public ECSController controller = ECSController.Instance;
    public string Name => "ProtectionTickManagement";

    public void UpdateSystem()
    {
        foreach (var (id, protection) in Protections.protections.ToList()) {
            {
                float newProtectionRemaining = Math.Max(protection.Remaining - Time.deltaTime, 0);
                float newProtectionCooldown = Math.Max(protection.Cooldown - Time.deltaTime, 0);
                if (newProtectionCooldown <= 0){
                    Protections.protections.Remove(id);
                    CollisionCount.collisionCount[id] = 0;
                }
                else
                    Protections.protections[id] = new ProtectionTimers{Remaining = newProtectionRemaining,Cooldown = newProtectionCooldown};
            }
        }
    }
}
