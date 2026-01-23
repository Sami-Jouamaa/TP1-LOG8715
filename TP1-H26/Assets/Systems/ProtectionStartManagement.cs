public class ProtectionStartManagement : ISystem
{
    public ECSController controller = ECSController.Instance;
    public string Name => "ProtectionStartManagement";
    public void UpdateSystem()
    {
        Config config = controller.Config;
        foreach (var (id, size) in Sizes.sizes) {
            if (size > config.protectionSize) 
                continue;
            if (CollisionCount.collisionCount[id] < config.protectionCollisionCount) 
                continue;
            if (Protections.protections.ContainsKey(id)) 
                continue;

            Protections.protections.Add(id, new ProtectionTimers{Remaining = config.protectionDuration, Cooldown = config.protectionCooldown});
        }
    }
}
