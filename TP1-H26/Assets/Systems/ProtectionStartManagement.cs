public class ProtectionStartManagement : ISystem
{
    public ECSController controller = ECSController.Instance;
    public string Name => "ProtectionStartManagement";

    public void UpdateSystem()
    {
        foreach (uint id in Positions.circlePositions.Keys)
            if (SimStep.currentSimStep == 0 || Regions.regions.TryGetValue(id, out var region) && region == CircleRegion.Left)
                ProtectionManagement(id);
    }

    public void ProtectionManagement(uint id)
    {
        Config config = controller.Config;
        int size = Sizes.sizes[id];
        if (size > config.protectionSize)
            return;
        if (CollisionCount.collisionCount[id] < config.protectionCollisionCount)
            return;
        if (Protections.protections.ContainsKey(id))
            return;

        Protections.protections.Add(id, new ProtectionTimers { Remaining = config.protectionDuration, Cooldown = config.protectionCooldown });

    }
}
