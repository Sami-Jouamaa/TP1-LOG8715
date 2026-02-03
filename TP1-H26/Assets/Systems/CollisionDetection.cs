using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : ISystem
{
    public ECSController controller = ECSController.Instance;

    public string Name => "CollisionDetection";
    static void Collide(uint a, int b)
    {
        List<int> list = CollisionPair.collisionPairs.ContainsKey(a) ? CollisionPair.collisionPairs[a] : new();
        if (list.Contains(b))
        {
            CollisionPair.collisionPairs[a] = list;
        }
        else
        {
            list.Add(b);
            CollisionPair.collisionPairs[a] = list;
        }
    }
    static void wallHit(uint id, WallOrientation wall)
    {
        Collide(id, -(int)wall);
    }
    public void UpdateSystem()
    {
        foreach (var leftSideId in LeftSideCircles.circlesOnLeftSide)
        {
            for (int fasterIteration = 0; fasterIteration < 4; fasterIteration++)
            {
                DetectCollision(leftSideId);
            }
        }

        foreach (var rightSideId in RightSideCircles.circlesOnRightSide)
        {
            DetectCollision(rightSideId);
        }
    }

    public void DetectCollision(uint firstCircleId)
    {
        Vector2 firstPosition = Positions.circlePositions[firstCircleId];
        float radius = (float)Sizes.sizes[firstCircleId] / 2;

        float maxY = Camera.main.orthographicSize;
        float minY = -maxY;
        float maxX = maxY * Camera.main.aspect;
        float minX = -maxX;
        if (firstPosition.x + radius >= maxX || firstPosition.x - radius <= minX)
            wallHit(firstCircleId, WallOrientation.Vertical);
        if (firstPosition.y + radius >= maxY || firstPosition.y - radius <= minY)
            wallHit(firstCircleId, WallOrientation.Horizontal);

        foreach (var (secondId, secondPosition) in Positions.circlePositions)
        {
            if (secondId <= firstCircleId) continue;
            // Collision with other circle
            float xDifference = Mathf.Pow(firstPosition.x - secondPosition.x, 2);
            float yDifference = Mathf.Pow(firstPosition.y - secondPosition.y, 2);
            float radiiSum = Mathf.Pow(radius + ((float)Sizes.sizes[secondId] / 2), 2);
            if (xDifference + yDifference <= radiiSum)
            {
                Collide(firstCircleId, (int)secondId);
                Collide(secondId, (int)firstCircleId);
            }
        }

    }
}
