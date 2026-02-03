using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;
using UnityEngine;

public class ExplosionExecutionSystem : ISystem
{
    public ECSController controller = ECSController.Instance;
    public string Name => "ExplosionExecution";
    List<uint> exploding = new();

    List<uint> NewCirclesRightSide = new List<uint>();

    List<uint> NewCirclesLeftSide = new List<uint>();
    public void UpdateSystem()
    {
        exploding = Explosion.explosions.Where(explosion => explosion.Value == ExplosionState.Exploding).Select(explosion => explosion.Key).ToList();
        List<uint> circlesToRemoveFromLeft = new List<uint>();
        List<uint> circlesToRemoveFromRight = new List<uint>();

        foreach (var leftSideId in LeftSideCircles.circlesOnLeftSide)
        {
            if (!exploding.Contains(leftSideId)) continue;
            ApplyExplosion(leftSideId);
            circlesToRemoveFromLeft.Add(leftSideId);
        }

        foreach (var rightSideId in RightSideCircles.circlesOnRightSide)
        {
            if (!exploding.Contains(rightSideId)) continue;
            ApplyExplosion(rightSideId);
            circlesToRemoveFromRight.Add(rightSideId);
        }

        foreach (var id in circlesToRemoveFromLeft)
        {
            LeftSideCircles.circlesOnLeftSide.Remove(id);
        }

        foreach (var id in circlesToRemoveFromRight)
        {
            RightSideCircles.circlesOnRightSide.Remove(id);
        }

        foreach (var newCircleId in NewCirclesLeftSide)
        {
            LeftSideCircles.circlesOnLeftSide.Add(newCircleId);
        }

        foreach (var newCircleId in NewCirclesRightSide)
        {
            RightSideCircles.circlesOnRightSide.Add(newCircleId);
        }

        NewCirclesLeftSide.Clear();
        NewCirclesRightSide.Clear();
        Explosion.explosions.Clear();
    }

    public void ApplyExplosion(uint circleId)
    {
        Vector2 position = Positions.circlePositions[circleId];
        Vector2 velocity = Velocities.velocities[circleId];
        int size = Sizes.sizes[circleId];

        int newSize = Mathf.Max(1, size / 4);

        Vector2[] diagonals =
        {
            new Vector2( 1,  1),
            new Vector2(-1,  1),
            new Vector2( 1, -1),
            new Vector2(-1, -1)
        };

        foreach (Vector2 dir in diagonals)
        {
            uint newCircle = Positions.circlePositions.Keys.Max() + 1;

            float offset = newSize * Mathf.Sqrt(2f);

            Positions.circlePositions.Add(
                newCircle,
                position + dir.normalized * offset
            );
            Sizes.sizes.Add(newCircle, newSize);

            Velocities.velocities.Add(
                newCircle,
                dir.normalized * velocity.magnitude
            );

            CollisionCount.collisionCount.Add(newCircle, 0);
            CollisionBehavior.behaviors.Add(newCircle, Behavior.Dynamic);

            controller.CreateShape(newCircle, newSize);
            Explosion.explosions.Add(newCircle, ExplosionState.Debris);

            if (Positions.circlePositions[newCircle].x < 0)
            {
                NewCirclesLeftSide.Add(newCircle);
            }
            else
            {
                NewCirclesRightSide.Add(newCircle);
            }
        }
        DeadCircles.deadCircles.Add(circleId);
    }
}
