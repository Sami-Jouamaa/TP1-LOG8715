using UnityEngine;

public class CollisionManagement : ISystem
{
    public ECSController controller = ECSController.Instance;

    public string Name => "CollisionManagement";

    public void UpdateSystem()
    {
        float maxY = Camera.main.orthographicSize;
        float minY = -maxY;
        float maxX = maxY * Camera.main.aspect;
        float minX = -maxX;
        foreach (var (id, colliders) in CollisionPair.collisionPairs)
        {
            uint firstCircle = id;
            foreach (int collider in colliders)  //iterate over every colliding object/wall
            {
                if (collider < 0)
                {
                    WallOrientation wall = (WallOrientation)(-collider);
                    Vector2 velocity = Velocities.velocities[id];
                    Vector2 position = Positions.circlePositions[id];
                    float radius = (float)Sizes.sizes[id] / 2;
                    switch (wall)
                    {
                        case WallOrientation.Vertical:
                            velocity.x *= -1;
                            if (position.x + radius > maxX) position.x = maxX - radius;
                            if (position.x - radius < minX) position.x = minX + radius;
                            break;
                        case WallOrientation.Horizontal:
                            velocity.y *= -1;
                            if (position.y + radius > maxY) position.y = maxY - radius;
                            if (position.y - radius < minY) position.y = minY + radius;
                            break;
                    }
                    Positions.circlePositions[id] = position;
                    Velocities.velocities[id] = velocity;
                }
                else if (id < collider) //we only process each pair once by processing the smaller one
                {
                    if (!CollisionPair.collisionPairs.ContainsKey((uint)collider)) continue;
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
                        Positions.circlePositions[firstCircle] = newPos1;
                        Velocities.velocities[firstCircle] = newVel1;
                    }
                    if (CollisionBehavior.behaviors[secondCircle] == Behavior.Dynamic)
                    {
                        Positions.circlePositions[secondCircle] = newPos2;
                        Velocities.velocities[secondCircle] = newVel2;
                    }

                }
            }
        }
        CollisionPair.collisionPairs.Clear();
    }
}
