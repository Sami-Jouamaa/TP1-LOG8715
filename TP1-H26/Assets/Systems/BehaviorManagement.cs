using UnityEngine;

public class BehaviorManagement : ISystem
{
    public ECSController controller = ECSController.Instance;

    public string Name => "BehaviorManagement";

    public void UpdateSystem()
    {
        for (uint i = 0; i < Sizes.sizes.Count; i++)
        {
            int size = Sizes.sizes[i];
            int collisionCount = CollisionCount.collisionCount[i];
            // if (collisionCount == controller.Config.protectionCollisionCount & size <= controller.Config.protectionSize)
            // {
            //     #CollisionBehavior.behaviors[i] = Behavior.Protected;
            // }
        }
    }
}
