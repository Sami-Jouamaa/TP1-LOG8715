using UnityEngine;

public class CollisionManagement : ISystem
{
    public ECSController controller = ECSController.Instance;

    public string Name => "CollisionManagement";

    public void ApplyCollision(uint firstCircleId)
    {
        float maxY = Camera.main.orthographicSize;
        float minY = -maxY;
        float maxX = maxY * Camera.main.aspect;
        float minX = -maxX;

        if (!CollisionPair.collisionPairs.ContainsKey(firstCircleId)) return;

        foreach (int collider in CollisionPair.collisionPairs[firstCircleId])
        {
            if (collider < 0)
            {
                WallOrientation wall = (WallOrientation)(-collider);
                Vector2 velocity = Velocities.velocities[firstCircleId];
                Vector2 position = Positions.circlePositions[firstCircleId];
                float radius = (float)Sizes.sizes[firstCircleId] / 2;
                switch (wall)
                {
                    case WallOrientation.Vertical:
                        velocity.x *= -1;
                        if (position.x + radius > maxX) position.x = maxX - radius;
                        if (position.x - radius < minX) position.x = minX + radius;
                        CollisionPair.collisionPairs[firstCircleId].Remove((int)WallOrientation.Vertical);
                        break;
                    case WallOrientation.Horizontal:
                        velocity.y *= -1;
                        if (position.y + radius > maxY) position.y = maxY - radius;
                        if (position.y - radius < minY) position.y = minY + radius;
                        CollisionPair.collisionPairs[firstCircleId].Remove((int)WallOrientation.Horizontal);
                        break;
                }
                Positions.circlePositions[firstCircleId] = position;
                Velocities.velocities[firstCircleId] = velocity;
            }
            else
            {
                uint secondCircle = (uint)collider;
                if (!CollisionPair.collisionPairs.ContainsKey(secondCircle))
                    continue;

                bool firstDynamic = CollisionBehavior.behaviors[firstCircleId] == Behavior.Dynamic;
                bool secondDynamic = CollisionBehavior.behaviors[secondCircle] == Behavior.Dynamic;
                CollisionResult resultingPosVel = CollisionUtility.CalculateCollision(
                    Positions.circlePositions[firstCircleId],
                    Velocities.velocities[firstCircleId],
                    Sizes.sizes[firstCircleId],
                    Positions.circlePositions[secondCircle],
                    Velocities.velocities[secondCircle],
                    Sizes.sizes[secondCircle]
                );
                Vector2 newPos1 = resultingPosVel.position1;
                Vector2 newVel1 = resultingPosVel.velocity1;

                Vector2 newPos2 = resultingPosVel.position2;
                Vector2 newVel2 = resultingPosVel.velocity2;

                if (firstDynamic)
                {
                    Positions.circlePositions[firstCircleId] = newPos1;
                    Velocities.velocities[firstCircleId] = newVel1;
                }
                if (secondDynamic)
                {
                    Positions.circlePositions[secondCircle] = newPos2;
                    Velocities.velocities[secondCircle] = newVel2;
                }
                if (firstDynamic && secondDynamic)
                {
                    int firstSize = Sizes.sizes[firstCircleId];
                    int secondSize = Sizes.sizes[secondCircle];
                    if (firstSize == secondSize)
                    {
                        if (firstSize <= controller.Config.protectionSize)
                        {
                            CollisionCount.collisionCount[firstCircleId]++;
                            CollisionCount.collisionCount[secondCircle]++;
                        }
                    }
                    else
                    {
                        uint smallerCircle = firstSize < secondSize ? firstCircleId : secondCircle;
                        uint biggerCircle = firstSize < secondSize ? secondCircle : firstCircleId;

                        bool smallerIsProtected =
                            Protections.protections.ContainsKey(smallerCircle) && Protections.protections[smallerCircle].Remaining > 0;

                        bool biggerIsProtected =
                            Protections.protections.ContainsKey(biggerCircle) && Protections.protections[biggerCircle].Remaining > 0;

                        if (!biggerIsProtected)
                        {
                            if (smallerIsProtected)
                                Sizes.sizes[biggerCircle]--;
                            else
                            {
                                Sizes.sizes[biggerCircle]++;
                                Sizes.sizes[smallerCircle]--;
                            }
                        }
                    }
                }

            }
        }
        CollisionPair.collisionPairs.Remove(firstCircleId);
    }

    public void UpdateSystem()
    {
        foreach (var leftSideId in LeftSideCircles.circlesOnLeftSide)
        {
            for (int fasterIteration = 0; fasterIteration < 4; fasterIteration++)
            {
                ApplyCollision(leftSideId);
            }
        }

        foreach (var rightSideId in RightSideCircles.circlesOnRightSide)
        {
            ApplyCollision(rightSideId);
        }

        CollisionPair.collisionPairs.Clear();
    }
}
