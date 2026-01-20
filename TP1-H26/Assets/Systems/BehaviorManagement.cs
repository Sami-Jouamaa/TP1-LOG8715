using UnityEngine;

public class BehaviorManagement
{
    public ECSController controller = ECSController.Instance;
    
    public void UpdateSystem()
    {
        for (uint i = 0; i < Sizes.sizes.Count; i++)
        {
            if (CollisionCount.collisionCount[i] == controller.Config.protectionCollisionCount)
            {
                
            }
        }
        
    }
}
