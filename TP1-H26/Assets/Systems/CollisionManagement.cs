using UnityEngine;
public class CollisionManagement : ISystem
{
    public ECSController controller = ECSController.Instance;

    public string Name => "CollisionManagement";

    public void UpdateSystem()
    {
        foreach (uint id in CollisionPair.collisionPairs.Keys)
        {
            if (LifeStates.lifeStates[id] == LifeState.Dead)
                continue;
            if (SimStep.currentSimStep == 0 || Regions.regions.TryGetValue(id, out var region) && region == CircleRegion.Left)
                ApplyCollision(id);
        }
        CollisionPair.collisionPairs.Clear();
    }
    public void ApplyCollision(uint firstCircle)
    {
        foreach (uint secondCircle in CollisionPair.collisionPairs[firstCircle])
        {
            if (LifeStates.lifeStates[secondCircle] == LifeState.Dead)
                continue;
            if (firstCircle > secondCircle)
                continue;
            bool firstDynamic = CollisionBehavior.behaviors[firstCircle] == Behavior.Dynamic;
            bool secondDynamic = CollisionBehavior.behaviors[secondCircle] == Behavior.Dynamic;
            CollisionResult resultingPosVel = CollisionUtility.CalculateCollision(
                Positions.circlePositions[firstCircle],
                Velocities.velocities[firstCircle],
                Sizes.sizes[firstCircle],
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
                Positions.circlePositions[firstCircle] = newPos1;
                Velocities.velocities[firstCircle] = newVel1;
            }
            if (secondDynamic)
            {
                Positions.circlePositions[secondCircle] = newPos2;
                Velocities.velocities[secondCircle] = newVel2;
            }
            if (!(firstDynamic && secondDynamic))
                continue;
            
            int firstSize = Sizes.sizes[firstCircle];
            int secondSize = Sizes.sizes[secondCircle];
            if (firstSize == secondSize)
            {
                if (firstSize <= controller.Config.protectionSize)
                {
                    CollisionCount.collisionCount[firstCircle]++;
                    CollisionCount.collisionCount[secondCircle]++;
                }
            }
            else
                ModulateSizes(firstCircle, secondCircle);
            
        }
    }
    
    public void ModulateSizes(uint firstCircle, uint secondCircle)
    {
        int firstSize = Sizes.sizes[firstCircle];
        int secondSize = Sizes.sizes[secondCircle];

        uint smallerCircle = firstSize < secondSize ? firstCircle : secondCircle;
        uint biggerCircle = firstSize < secondSize ? secondCircle : firstCircle;

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
                //reset collision count if smaller circle reaches protection collision count
                if (Sizes.sizes[smallerCircle] == (int)controller.Config.protectionCollisionCount)
                {
                    CollisionCount.collisionCount[smallerCircle] = 0;
                }
            }
        }
    }
}
