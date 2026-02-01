using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExplosionExecutionSystem : ISystem
{
    public ECSController controller = ECSController.Instance;
    public string Name => "ExplosionExecution";
    
    public void UpdateSystem()
    {

        List<uint> toExplode =
            new List<uint>(Explosion.explosions.Keys);

        foreach (uint circle in toExplode)
        {
            Vector2 position = Positions.circlePositions[circle];
            Vector2 velocity = Velocities.velocities[circle];
            int size = Sizes.sizes[circle];

            int newSize = Mathf.Max(1, size / 4);

            Vector2[] diagonals =
            {
                new Vector2( 1,  1),
                new Vector2(-1,  1),
                new Vector2( 1, -1),
                new Vector2(-1, -1)
            };

            foreach (Vector2 dir in diagonals)
            {
                uint newCircle = Positions.circlePositions.Keys.Max() + 1;
                Debug.Log(newCircle);

                float offset = newSize * 0.6f;

                Positions.circlePositions.Add(
                    newCircle,
                    position + dir.normalized * offset
                );
                Sizes.sizes.Add(newCircle, newSize);

                Velocities.velocities.Add(
                    newCircle,
                    dir.normalized * velocity.magnitude
                );

                CollisionCount.collisionCount.Add(newCircle, 0);
                CollisionBehavior.behaviors.Add(newCircle, Behavior.Dynamic);

                controller.CreateShape(newCircle, newSize);
            }
            RemoveCircle(circle);
        }

        Explosion.explosions.Clear();
    }

    private void RemoveCircle(uint circle)
    {
        controller.DestroyShape(circle);
        Positions.circlePositions.Remove(circle);
        Velocities.velocities.Remove(circle);
        Sizes.sizes.Remove(circle);
        Colors.colors.Remove(circle);
        Protections.protections.Remove(circle);
        CollisionPair.collisionPairs.Remove(circle);
    }
}
