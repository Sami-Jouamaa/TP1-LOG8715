
using NUnit.Framework.Constraints;

public class ColorSetManagement : ISystem
{
    public ECSController controller = ECSController.Instance;
    public string Name => "ColorSetManagement";
    public void UpdateSystem()
    {
        foreach (var leftSideId in LeftSideCircles.circlesOnLeftSide)
        {
            for (int fasterIteration = 0; fasterIteration < 4; fasterIteration++)
            {
                ColourSet(leftSideId);
            }
        }

        foreach (var rightSideId in RightSideCircles.circlesOnRightSide)
        {
            ColourSet(rightSideId);
        }
    }

    public void ColourSet(uint circleId)
    {
        Behavior behavior = CollisionBehavior.behaviors[circleId];
        bool isInCollision = CollisionPair.collisionPairs.ContainsKey(circleId);
        int size = Sizes.sizes[circleId];
        bool isDebris = Explosion.explosions.ContainsKey(circleId) && Explosion.explosions[circleId] == ExplosionState.Debris;

        if (behavior == Behavior.Static)
            Colors.colors[circleId] = CircleColor.Red;
        else if (isInCollision)
            Colors.colors[circleId] = CircleColor.Green;
        else if (Protections.protections.ContainsKey(circleId))
            Colors.colors[circleId] = Protections.protections[circleId].Remaining > 0 ? CircleColor.White : CircleColor.Yellow;
        else if (size < controller.Config.protectionSize)
            Colors.colors[circleId] = CircleColor.LightBlue;
        else if (size == controller.Config.explosionSize - 1)
            Colors.colors[circleId] = CircleColor.Orange;
        else if (isDebris)
            Colors.colors[circleId] = CircleColor.Pink;
        else
            Colors.colors[circleId] = CircleColor.DarkBlue;
    }
}
