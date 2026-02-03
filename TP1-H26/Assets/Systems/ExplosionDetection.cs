using UnityEngine;

public class ExplosionDetectionSystem : ISystem
{
    public ECSController controller = ECSController.Instance;
    public string Name => "ExplosionDetection";
    public int ExplosionSize = ECSController.Instance.Config.explosionSize;

    public void UpdateSystem()
    {
        foreach (var leftSideId in LeftSideCircles.circlesOnLeftSide)
        {
            for (int fasterIteration = 0; fasterIteration < 4; fasterIteration++)
            {
                ExplosionDetection(leftSideId);
            }
        }

        foreach (var rightSideId in RightSideCircles.circlesOnRightSide)
        {
            ExplosionDetection(rightSideId);
        }
    }

    public void ExplosionDetection(uint circleId)
    {
        if (!Sizes.sizes.ContainsKey(circleId)) return;
        if (Sizes.sizes[circleId] >= ExplosionSize)
        {
            Explosion.explosions[circleId] = ExplosionState.Exploding;
        }
    }
}
