using UnityEngine;

public class CollisionManagement : ISystem
{
    public ECSController controller = ECSController.Instance;

    public string Name => "CollisionManagement";

    public void ApplyCollision(uint firstCircleId)
    {
        if (!CollisionPair.collisionPairs.ContainsKey(firstCircleId)) return;

        foreach (int collider in CollisionPair.collisionPairs[firstCircleId])
        {
            uint secondCircle = (uint)collider;
            if (firstCircleId > collider)
                continue;
            bool firstDynamic = CollisionBehavior.behaviors[firstCircleId] == Behavior.Dynamic;
            bool secondDynamic = CollisionBehavior.behaviors[secondCircle] == Behavior.Dynamic;
            CollisionResult resultingPosVel = CollisionUtility.CalculateCollision(
                Positions.circlePositions[firstCircleId],
                Velocities.velocities[firstCircleId],
                Sizes.sizes[firstCircleId],
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
                Positions.circlePositions[firstCircleId] = newPos1;
                Velocities.velocities[firstCircleId] = newVel1;
            }
            if (secondDynamic)
            {
                Positions.circlePositions[secondCircle] = newPos2;
                Velocities.velocities[secondCircle] = newVel2;
            }
            if (!(firstDynamic && secondDynamic))
                continue;
            
            int firstSize = Sizes.sizes[firstCircleId];
            int secondSize = Sizes.sizes[secondCircle];
            if (firstSize == secondSize)
            {
                if (firstSize <= controller.Config.protectionSize)
                {
                    CollisionCount.collisionCount[firstCircleId]++;
                    CollisionCount.collisionCount[secondCircle]++;
                }
                continue;
            }
            
            uint smallerCircle = firstSize < secondSize ? firstCircleId : secondCircle;
            uint biggerCircle = firstSize < secondSize ? secondCircle : firstCircleId;

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
                }
            }
        }
    }

    public void UpdateSystem()
    {
        foreach (uint id in CollisionPair.collisionPairs.Keys)
            ApplyCollision(id);
        CollisionPair.collisionPairs.Clear();
    }
}
