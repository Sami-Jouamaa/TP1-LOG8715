using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExplosionExecutionSystem : ISystem
{
    public ECSController controller = ECSController.Instance;
    public string Name => "ExplosionExecution";
    
    public void UpdateSystem()
    {
        if (Explosion.requests.Count == 0)
            return;

        List<uint> toExplode =
            new List<uint>(Explosion.requests.Keys);

        foreach (uint entity in toExplode)
        {
            if (!Sizes.sizes.ContainsKey(entity))
                continue;

            Vector2 position = Positions.circlePositions[entity];
            Vector2 velocity = Velocities.velocities[entity];
            int size = Sizes.sizes[entity];

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
                uint newEntity = Positions.circlePositions.Keys.Max() + 1;

                float offset = newSize * 0.6f;

                Positions.circlePositions.Add(
                    newEntity,
                    position + dir.normalized * offset
                );
                Sizes.sizes.Add(newEntity, newSize);

                Velocities.velocities.Add(
                    newEntity,
                    dir.normalized * velocity.magnitude
                );

                CollisionCount.collisionCount.Add(newEntity, 0);
                CollisionBehavior.behaviors.Add(newEntity, Behavior.Dynamic);

                controller.CreateShape(newEntity, newSize);
            }


            RemoveEntity(entity);
        }

        Explosion.requests.Clear();
    }

    private void RemoveEntity(uint entity)
    {
        controller.DestroyShape(entity);
        Positions.circlePositions.Remove(entity);
        Velocities.velocities.Remove(entity);
        Sizes.sizes.Remove(entity);
        CollisionCount.collisionCount.Remove(entity);
        CollisionBehavior.behaviors.Remove(entity);
    }
}
