public class SizeManagement : ISystem
{
    public ECSController controller = ECSController.Instance;
    public string Name => "SizeManagement";

    public void UpdateSystem()
    {
        foreach (var (id, _) in Sizes.sizes)
            ManageSize(id);
    }

    public void ManageSize(uint circle)
    {
        if (Sizes.sizes[circle] <= 0)
            DeadCircles.deadCircles.Add(circle);
        controller.UpdateShapeSize(circle, Sizes.sizes[circle]);
    }
}

