using System.Collections.Generic;
using UnityEngine;
public class CollisionDetection : ISystem
{
    public ECSController controller = ECSController.Instance;

    public string Name => "CollisionDetection";
    static void Collide(uint a, uint b)
    {
        List<uint> list = CollisionPair.collisionPairs.ContainsKey(a) ? CollisionPair.collisionPairs[a] : new();
        if (!list.Contains(b))
            list.Add(b);
            CollisionPair.collisionPairs[a] = list;
    }
    public void UpdateSystem()
    {
        foreach (uint id in Positions.circlePositions.Keys)
            if (SimStep.currentSimStep == 0 || Regions.regions.TryGetValue(id, out var region) && region == CircleRegion.Left)
                DetectCollision(id);
    }

    private static void DetectCollision(uint firstCircleId)
    {
        Vector2 firstPosition = Positions.circlePositions[firstCircleId];
        float radius = (float)Sizes.sizes[firstCircleId] / 2;

        float maxY = Camera.main.orthographicSize;
        float minY = -maxY;
        float maxX = maxY * Camera.main.aspect;
        float minX = -maxX;

        if (firstPosition.x - radius <= minX)
            WallHits.wallHits.Add(firstCircleId, WallPos.Left);
        if (firstPosition.x + radius >= maxX)
            WallHits.wallHits.Add(firstCircleId, WallPos.Right);
        if (firstPosition.y - radius <= minY)
            WallHits.wallHits.Add(firstCircleId, WallPos.Bottom);
        if (firstPosition.y + radius >= maxY)
            WallHits.wallHits.Add(firstCircleId, WallPos.Top);

        foreach (var (secondId, secondPosition) in Positions.circlePositions)
        {
            if (secondId <= firstCircleId) continue;
            // Collision with other circle
            float xDifference = Mathf.Pow(firstPosition.x - secondPosition.x, 2);
            float yDifference = Mathf.Pow(firstPosition.y - secondPosition.y, 2);
            float radiiSum = Mathf.Pow(radius + ((float)Sizes.sizes[secondId] / 2), 2);
            if (xDifference + yDifference <= radiiSum)
            {
                Collide(firstCircleId, secondId);
                Collide(secondId, firstCircleId);
            }
        }

    }
}
