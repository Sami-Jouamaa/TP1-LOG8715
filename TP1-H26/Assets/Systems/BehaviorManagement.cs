using UnityEngine;

public class BehaviorManagement : ISystem
{
    public ECSController controller = ECSController.Instance;

    public string Name => throw new System.NotImplementedException();

    public void UpdateSystem()
    {
        for (uint i = 0; i < Sizes.sizes.Count; i++)
        {

            if (CollisionCount.collisionCount[i] == controller.Config.protectionCollisionCount)
            {
                CollisionBehavior.behaviors[i] = Behavior.Protected;
            }

        }
    }
}
