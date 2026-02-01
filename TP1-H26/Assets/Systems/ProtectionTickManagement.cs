using System;
using System.Collections.Generic;
using System.Linq;

public class ProtectionTickManagement : ISystem
{
    public ECSController controller = ECSController.Instance;
    public string Name => "ProtectionTickManagement";

    List<uint> expired = new();
    public void UpdateSystem()
    {
        foreach (var (id, protection) in Protections.protections.ToList()) {
            {
                float newProtectionRemaining = Math.Max(protection.Remaining - 1, 0);
                float newProtectionCooldown = Math.Max(protection.Cooldown - 1, 0);
                if (newProtectionCooldown > 0)
                {
                    expired.Add(id);
                }

                Protections.protections[id] = new ProtectionTimers{Remaining = newProtectionRemaining,Cooldown = newProtectionCooldown};
            }
        }
        foreach(var id in expired)
        {
            Protections.protections.Remove(id);
        }
    }
}
