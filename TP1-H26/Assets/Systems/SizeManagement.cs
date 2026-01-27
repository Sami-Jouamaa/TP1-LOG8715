using System.Collections.Generic;

public class SizeManagement : ISystem
{
    public ECSController controller = ECSController.Instance;
    public string Name => "SizeManagement";
    List<uint> disappeared = new();

    public void UpdateSystem()
    {
        foreach (var (id, size) in Sizes.sizes)
        {
            if (size <= 0)
            {
                disappeared.Add(id);
                continue;
            }
            controller.UpdateShapeSize(id, size);
        }

        foreach(var id in disappeared)
        {
            controller.DestroyShape(id);
            Positions.circlePositions.Remove(id);
            Velocities.velocities.Remove(id);
            Sizes.sizes.Remove(id);
            Colors.colors.Remove(id);
            Protections.protections.Remove(id);
            CollisionPair.collisionPairs.Remove(id);
        }
        disappeared.Clear();
    }
}

