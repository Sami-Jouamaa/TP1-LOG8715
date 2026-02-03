using System.Collections.Generic;

public class SizeManagement : ISystem
{
    public ECSController controller = ECSController.Instance;
    public string Name => "SizeManagement";
    readonly List<uint> disappeared = new();

    public void UpdateSystem()
    {
        foreach (var leftSideId in LeftSideCircles.circlesOnLeftSide)
        {
            for (int fasterIteration = 0; fasterIteration < 4; fasterIteration++)
            {
                ManageSize(leftSideId);
            }
        }

        foreach (var rightSideId in RightSideCircles.circlesOnRightSide)
        {
            ManageSize(rightSideId);
        }

        foreach (var id in disappeared)
        {
            controller.DestroyShape(id);
            Positions.circlePositions.Remove(id);
            Velocities.velocities.Remove(id);
            Sizes.sizes.Remove(id);
            Colors.colors.Remove(id);
            Protections.protections.Remove(id);
            CollisionPair.collisionPairs.Remove(id);
            CollisionCount.collisionCount.Remove(id);
            CollisionBehavior.behaviors.Remove(id);
            Explosion.explosions.Remove(id);
            // If id is not in one of them, it'll just say false, instead of if/else
            LeftSideCircles.circlesOnLeftSide.Remove(id);
            RightSideCircles.circlesOnRightSide.Remove(id);
        }
        disappeared.Clear();
    }

    public void ManageSize(uint id)
    {
        if (Sizes.sizes[id] <= 0)
        {
            if (disappeared.Contains(id)) return;
            disappeared.Add(id);
        }
        controller.UpdateShapeSize(id, Sizes.sizes[id]);
    }
}

