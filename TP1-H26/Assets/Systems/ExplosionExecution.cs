using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExplosionExecutionSystem : ISystem
{
    public ECSController controller = ECSController.Instance;
    public string Name => "ExplosionExecution";
    List<uint> exploding = new();

    public void UpdateSystem()
    {
        exploding = Explosion.explosions.Where(explosion => explosion.Value == ExplosionState.Exploding).Select(explosion => explosion.Key).ToList();


        foreach (uint id in exploding)
        {
            if (SimStep.currentSimStep == 0 || Regions.regions.TryGetValue(id, out var region) && region == CircleRegion.Left)
            {
                ApplyExplosion(id);
            }
                
        }
            
        Explosion.explosions.Clear();
    }

    public void ApplyExplosion(uint circleId)
    {
        Vector2 position = Positions.circlePositions[circleId];
        Vector2 velocity = Velocities.velocities[circleId];
        int size = Sizes.sizes[circleId];

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
            float offset = newSize * Mathf.Sqrt(2f);

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
            Explosion.explosions.Add(newCircle, ExplosionState.Debris);

        }
        DeadCircles.deadCircles.Add(circleId);
    }
}
