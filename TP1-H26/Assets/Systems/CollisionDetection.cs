using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : ISystem
{
    public ECSController controller = ECSController.Instance;

    public string Name => "CollisionDetection";
    static void collide(uint a, int b)
    {
        List<int> list = CollisionPair.collisionPairs.ContainsKey(a) ? CollisionPair.collisionPairs[a] : new();
        list.Add(b);
        CollisionPair.collisionPairs[a] = list;
    }
    static void wallHit(uint id, WallOrientation wall)
    {
        collide(id, -(int)wall);
    }
    public void UpdateSystem()
    {
        foreach (var (id, position) in Positions.circlePositions )
        {
            // Collision with screen border
            float currentX = position.x;
            float currentY = position.y;
            float radius = (float)Sizes.sizes[id] / 2;

            float fov = Camera.main.orthographicSize;
            
            float maxX = fov*2 + 1;
            float minX = maxX * -1;
            float maxY = fov;
            float minY = maxY * -1; 
            if (currentX + radius >= maxX || currentX - radius <= minX)
                wallHit(id, WallOrientation.Vertical);
            if (currentY + radius >= maxY || currentY - radius <= minY)
                wallHit(id, WallOrientation.Horizontal);

            foreach (var (secondId, secondPosition) in Positions.circlePositions)
            {
                if (secondId <= id) continue;
                // Collision with other circle
                float xDifference = Mathf.Pow(currentX - secondPosition.x, 2);
                float yDifference = Mathf.Pow(currentY - secondPosition.y, 2);
                float radiiSum = Mathf.Pow(radius + ((float)Sizes.sizes[secondId] / 2), 2);
                if (xDifference + yDifference <= radiiSum)
                {
                    collide(id,(int)secondId);
                    collide(secondId,(int)id);
                }
            }
        }
    }
}
