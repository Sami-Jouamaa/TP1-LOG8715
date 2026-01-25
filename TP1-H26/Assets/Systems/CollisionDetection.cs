using System;
using System.Linq;
using UnityEngine;

public class CollisionDetection : ISystem
{
    public ECSController controller = ECSController.Instance;

    public string Name => "CollisionManagement";
    public void UpdateSystem()
    {
        for (uint i = 0; i < Positions.circlePositions.Count; i++)
        {
            // Collision with screen border
            float currentX = Positions.circlePositions[i].x;
            float currentY = Positions.circlePositions[i].y;
            float radius = Sizes.sizes[i] / 2;

            float fov = Camera.main.fieldOfView;
            float maxX = fov / 2 + 1;
            float minX = maxX * -1;
            float maxY = fov / 4;
            float minY = maxY * -1;

            Vector2 currentVel = Velocities.velocities[i];
            if (currentX + radius >= maxX || currentX - radius <= minX)
            {
                currentVel.x *= -1;
                Velocities.velocities[i] = currentVel;
                if (!CollisionPair.CollisionPairs.ContainsKey(i))
                {
                    CollisionPair.CollisionPairs.Add(i, -1);
                }
            }
            if (currentY + radius >= maxY || currentY - radius <= minY)
            {
                currentVel.y *= -1;
                Velocities.velocities[i] = currentVel;
                if (!CollisionPair.CollisionPairs.ContainsKey(i))
                {
                    CollisionPair.CollisionPairs.Add(i, -1);
                }
            }

            for (uint j = i + 1; j < Positions.circlePositions.Count; j++)
            {
                // Collision with other circle
                float xDifference = Mathf.Pow(Positions.circlePositions[i].x - Positions.circlePositions[j].x, 2);
                float yDifference = Mathf.Pow(Positions.circlePositions[i].y - Positions.circlePositions[j].y, 2);
                float radiiSum = Mathf.Pow(((float)Sizes.sizes[i] / 2) + ((float)Sizes.sizes[j] / 2), 2);
                if (xDifference + yDifference <= radiiSum)
                {
                    if (CollisionPair.CollisionPairs.ContainsKey(i)) continue;
                    if (CollisionPair.CollisionPairs.ContainsKey(j)) continue;
                    CollisionPair.CollisionPairs.Add(i, (int)j);
                    CollisionPair.CollisionPairs.Add(j, (int)i);
                }
            }
        }
    }
}
