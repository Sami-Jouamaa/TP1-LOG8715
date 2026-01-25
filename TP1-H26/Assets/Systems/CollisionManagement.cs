using UnityEngine;
using System.Collections.Generic;
using System;

public class CollisionManagement : ISystem
{
    public ECSController controller = ECSController.Instance;

    public string Name => "CollisionManagement";

    public void UpdateSystem()
    {
        if (CollisionPair.CollisionPairs.Count > 0)
        {
            for (uint entity = 0; entity < CollisionPair.CollisionPairs.Count; entity++)
            {
                if (!CollisionPair.CollisionPairs.ContainsKey(entity)) continue;
                if (CollisionPair.CollisionPairs[entity] == -1) continue;

                uint firstCircle = entity;
                uint secondCircle = (uint)CollisionPair.CollisionPairs[entity];

                if (CollisionBehavior.behaviors[firstCircle] == Behavior.Dynamic && CollisionBehavior.behaviors[secondCircle] == Behavior.Dynamic)
                {
                    if (Sizes.sizes[firstCircle] <= controller.Config.protectionSize && Sizes.sizes[firstCircle] == Sizes.sizes[secondCircle])
                    {
                        CollisionCount.collisionCount[firstCircle] += 1;
                        CollisionCount.collisionCount[secondCircle] += 1;
                    }

                    int minSize = (int)MathF.Min(Sizes.sizes[firstCircle], Sizes.sizes[secondCircle]);
                    if (Sizes.sizes[firstCircle] != Sizes.sizes[secondCircle])
                    {
                        if (minSize == Sizes.sizes[firstCircle])
                        {
                            Sizes.sizes[firstCircle] -= 1;
                            Sizes.sizes[secondCircle] += 1;
                        }
                        else
                        {
                            Sizes.sizes[firstCircle] += 1;
                            Sizes.sizes[secondCircle] -= 1;
                        }
                    }
                }

                if (Sizes.sizes[firstCircle] == 0)
                {
                    controller.DestroyShape(firstCircle);
                }
                if (Sizes.sizes[secondCircle] == 0)
                {
                    controller.DestroyShape(secondCircle);
                }

                CollisionResult resultingPosVel = CollisionUtility.CalculateCollision(
                    Positions.circlePositions[firstCircle],
                    Velocities.velocities[firstCircle],
                    Sizes.sizes[firstCircle],
                    Positions.circlePositions[secondCircle],
                    Velocities.velocities[secondCircle],
                    Sizes.sizes[secondCircle]
                );

                Vector2 newPos1 = resultingPosVel.position1;
                Vector2 newVel1 = resultingPosVel.velocity1;

                Vector2 newPos2 = resultingPosVel.position2;
                Vector2 newVel2 = resultingPosVel.velocity2;

                if (CollisionBehavior.behaviors[firstCircle] == Behavior.Dynamic)
                {
                    controller.UpdateShapePosition(firstCircle, newPos1);
                    Velocities.velocities[firstCircle] = newVel1;
                }
                if (CollisionBehavior.behaviors[secondCircle] == Behavior.Dynamic)
                {
                    controller.UpdateShapePosition(secondCircle, newPos2);
                    Velocities.velocities[secondCircle] = newVel2;
                }

                CollisionPair.CollisionPairs.Clear();
            }
        }
    }
}
