using UnityEngine;
using System.Collections.Generic;
using System;

public class CollisionManagement : ISystem
{
    public ECSController controller = ECSController.Instance;

    public string Name => "CollisionManagement";

    public void UpdateSystem()
    {
        Dictionary<uint, Vector2> positionDictionary = Positions.circlePositions;
        Dictionary<uint, int> sizesDictionary = Sizes.sizes;
        Dictionary<uint, Vector2> velocities = Velocities.velocities;

        for (uint i = 0; i < Positions.circlePositions.Count; i++)
        {
            // Collision with screen border
            float currentX = positionDictionary[i].x;
            float currentY = positionDictionary[i].y;
            float radius = sizesDictionary[i] / 2;

            float maxY = 15;
            float minY = -15;
            float maxX = 31;
            float minX = -31;

            Vector2 currentVel = Velocities.velocities[i];
            if (currentX + radius >= maxX || currentX - radius <= minX)
            {
                currentVel.x *= -1;
                Velocities.velocities[i] = currentVel;
            }
            if (currentY + radius >= maxY || currentY - radius <= minY)
            {
                currentVel.y *= -1;
                Velocities.velocities[i] = currentVel;
            }

            for (uint j = i + 1; j < Positions.circlePositions.Count; j++)
            {
                // Collision with other circle
                float xDifference = Mathf.Pow(positionDictionary[i].x - positionDictionary[j].x, 2);
                float yDifference = Mathf.Pow(positionDictionary[i].y - positionDictionary[j].y, 2);
                float radiiSum = Mathf.Pow(((float)sizesDictionary[i] / 2) + (float)(sizesDictionary[j] / 2), 2);
                if (xDifference + yDifference <= radiiSum)
                {
                    if (CollisionBehavior.behaviors[i] == Behavior.Dynamic && CollisionBehavior.behaviors[j] == Behavior.Dynamic)
                    {
                        if (sizesDictionary[i] <= controller.Config.protectionSize && sizesDictionary[i] == sizesDictionary[j])
                        {
                            CollisionCount.collisionCount[i] += 1;
                            CollisionCount.collisionCount[j] += 1;
                        }

                        int minSize = (int)MathF.Min(sizesDictionary[i], sizesDictionary[j]);
                        if (sizesDictionary[i] != sizesDictionary[j])
                        {
                            if (minSize == sizesDictionary[i])
                            {
                                Sizes.sizes[i] -= 1;
                                Sizes.sizes[j] += 1;
                            }
                            else
                            {
                                Sizes.sizes[i] += 1;
                                Sizes.sizes[j] -= 1;
                            }
                        }
                    }

                    if (sizesDictionary[i] == 0)
                    {
                        controller.DestroyShape(i);
                    }
                    if (sizesDictionary[j] == 0)
                    {
                        controller.DestroyShape(j);
                    }

                    CollisionResult resultingPosVel = CollisionUtility.CalculateCollision(
                        positionDictionary[i],
                        velocities[i],
                        sizesDictionary[i],
                        positionDictionary[j],
                        velocities[j],
                        sizesDictionary[j]
                    );

                    Vector2 newPos1 = resultingPosVel.position1;
                    Vector2 newVel1 = resultingPosVel.velocity1;

                    Vector2 newPos2 = resultingPosVel.position2;
                    Vector2 newVel2 = resultingPosVel.velocity2;

                    if (CollisionBehavior.behaviors[i] == Behavior.Dynamic)
                    {
                        controller.UpdateShapePosition(i, newPos1);
                        Velocities.velocities[i] = newVel1;
                    }
                    if (CollisionBehavior.behaviors[j] == Behavior.Dynamic)
                    {
                        controller.UpdateShapePosition(j, newPos2);
                        Velocities.velocities[j] = newVel2;
                    }

                }
            }
        }
    }
}
