
public class CirclesManagement : ISystem
{
    public ECSController controller = ECSController.Instance;
    public string Name => "CirclesManagement";
    public bool firstTimeRunning = true;

    public void UpdateSystem()
    {
        if (firstTimeRunning)
        {
            for (uint i = 0; i < Sizes.sizes.Count; i++)
            {
                controller.CreateShape(i, Sizes.sizes[i]);
            }
            firstTimeRunning = false;
        }
    }
}
