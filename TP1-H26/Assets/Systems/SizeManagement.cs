using System.Linq;

public class SizeManagement : ISystem
{
    public ECSController controller = ECSController.Instance;
    public string Name => "SizeManagement";

    public void UpdateSystem()
    {
        foreach (uint id in Sizes.sizes.Keys.ToList())
            ManageSize(id);
    }

    public void ManageSize(uint circle)
    {
        if (Sizes.sizes[circle] <= 0)
            LifeStates.lifeStates[circle] = LifeState.Dead;
        if (LifeStates.lifeStates[circle] == LifeState.Dead)
            Sizes.sizes[circle] = 0;
        controller.UpdateShapeSize(circle, Sizes.sizes[circle]);
    }
}

