using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : ISystem
{
    public ECSController controller = ECSController.Instance;

    public string Name => "CollisionDetection";
    void collide(uint a, int b = -1)
    {
        List<int> list = CollisionPair.collisionPairs.ContainsKey(a) ? CollisionPair.collisionPairs[a] : new();
        list.Add(b);
        CollisionPair.collisionPairs[a] = list;
    }
    public void UpdateSystem()
    {
        for (uint i = 0; i < Positions.circlePositions.Count; i++)
        {
            // Collision with screen border
            float currentX = Positions.circlePositions[i].x;
            float currentY = Positions.circlePositions[i].y;
            float radius = Sizes.sizes[i] / 2;

            float fov = Camera.main.orthographicSize;
            
            float maxX = fov*2 + 1;
            float minX = maxX * -1;
            float maxY = fov;
            float minY = maxY * -1;

            Vector2 currentVel = Velocities.velocities[i];
            if (currentX + radius >= maxX || currentX - radius <= minX)
            {
                currentVel.x *= -1;
                Velocities.velocities[i] = currentVel;
                collide(i);
            }
            if (currentY + radius >= maxY || currentY - radius <= minY)
            {
                currentVel.y *= -1;
                Velocities.velocities[i] = currentVel;
                collide(i);
            }

            for (uint j = i + 1; j < Positions.circlePositions.Count; j++)
            {
                // Collision with other circle
                float xDifference = Mathf.Pow(Positions.circlePositions[i].x - Positions.circlePositions[j].x, 2);
                float yDifference = Mathf.Pow(Positions.circlePositions[i].y - Positions.circlePositions[j].y, 2);
                float radiiSum = Mathf.Pow(((float)Sizes.sizes[i] / 2) + ((float)Sizes.sizes[j] / 2), 2);
                if (xDifference + yDifference <= radiiSum)
                {
                    collide(i,(int)j);
                    collide(j,(int)i);
                }
            }
        }
    }
}
