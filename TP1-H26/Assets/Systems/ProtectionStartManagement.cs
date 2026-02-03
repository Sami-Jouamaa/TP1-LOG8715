public class ProtectionStartManagement : ISystem
{
    public ECSController controller = ECSController.Instance;
    public string Name => "ProtectionStartManagement";

    public void UpdateSystem()
    {
        foreach (var leftSideId in LeftSideCircles.circlesOnLeftSide)
        {
            for (int fasterIteration = 0; fasterIteration < 4; fasterIteration++)
            {
                ProtectionManagement(leftSideId);
            }
        }

        foreach (var rightSideId in RightSideCircles.circlesOnRightSide)
        {
            ProtectionManagement(rightSideId);
        }
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
