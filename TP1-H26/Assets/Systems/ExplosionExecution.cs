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
        exploding = Explosion.explosions.Where(explosion => explosion.Value == ExplosionState.Exploding
         && LifeStates.lifeStates[explosion.Key] != LifeState.Dead)
         .Select(explosion => explosion.Key).ToList();

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

        List<uint> freeIds = LifeStates.lifeStates.Keys.Where(id => LifeStates.lifeStates[id] == LifeState.Dead).ToList();
        foreach (Vector2 dir in diagonals)
        {
            uint newCircle;
            if (freeIds.Count > 0)
            {
                newCircle = freeIds[0];
                freeIds.RemoveAt(0);
            }
            else
            {
                newCircle = Positions.circlePositions.Keys.Max() + 1;
                controller.CreateShape(newCircle, newSize);
            }

            float offset = newSize * Mathf.Sqrt(2f);

            Positions.circlePositions[newCircle] = position + dir.normalized * offset;
            Sizes.sizes[newCircle] = newSize;

            Velocities.velocities[newCircle] = dir.normalized * velocity.magnitude;

            CollisionCount.collisionCount[newCircle] = 0;
            CollisionBehavior.behaviors[newCircle] = Behavior.Dynamic;
            Explosion.explosions[newCircle] = ExplosionState.Debris;
            LifeStates.lifeStates[newCircle] = LifeState.Alive;
            if (Protections.protections.ContainsKey(newCircle))
                Protections.protections.Remove(newCircle);
                

        }
        LifeStates.lifeStates[circleId] = LifeState.Dead;
    }
}
