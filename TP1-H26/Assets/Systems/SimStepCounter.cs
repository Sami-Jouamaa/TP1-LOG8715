public class SimStepCounter : ISystem
{
    public ECSController controller = ECSController.Instance;
    public string Name => "SimStepCounter";
    public void UpdateSystem()
    {
        SimStep.currentSimStep = (SimStep.currentSimStep + 1) % 4;
    }
}

