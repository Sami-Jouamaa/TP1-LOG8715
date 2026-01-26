using UnityEngine;
using System.Collections.Generic;

public class CollisionManagement : ISystem
{
    public ECSController controller = ECSController.Instance;

    public string Name => "CollisionManagement";
    List<uint> disappeared = new();

    public void UpdateSystem()
    {
        foreach (var (id, colliders) in CollisionPair.collisionPairs)
        {
            foreach (int collider in colliders)
            {
                if (id < collider) //we only process each pair once by processing the smaller one
                {
                    uint firstCircle = id;
                    uint secondCircle = (uint)collider;
                    if (CollisionBehavior.behaviors[firstCircle] == Behavior.Dynamic && CollisionBehavior.behaviors[secondCircle] == Behavior.Dynamic)
                    {
                        if (Sizes.sizes[firstCircle] <= controller.Config.protectionSize && Sizes.sizes[firstCircle] == Sizes.sizes[secondCircle])
                        {
                            CollisionCount.collisionCount[firstCircle] += 1;
                            CollisionCount.collisionCount[secondCircle] += 1;
                        }

                        if (Sizes.sizes[firstCircle] != Sizes.sizes[secondCircle])
                        {
                            if (Sizes.sizes[firstCircle] < Sizes.sizes[secondCircle])
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
                        disappeared.Add(firstCircle);
                    }
                    if (Sizes.sizes[secondCircle] == 0)
                    {
                        disappeared.Add(firstCircle);
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

                }
            }
        }
        foreach(var id in disappeared)
        {
            controller.DestroyShape(id);
        }
        CollisionPair.collisionPairs.Clear();
    }
}
