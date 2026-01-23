public class ColorSetManagement : ISystem
{
    public ECSController controller = ECSController.Instance;
    public string Name => "ColorSetManagement";
    public void UpdateSystem()
    {
        foreach (var (id, _) in Positions.circlePositions) {
            Behavior behavior = CollisionBehavior.behaviors[id];
            if (behavior == Behavior.Static)
                Colors.colors[id] = CircleColor.Red;
            else if (Protections.protections.ContainsKey(id))
                Colors.colors[id] = Protections.protections[id].Remaining > 0 ? CircleColor.White : CircleColor.Yellow;
            else
                Colors.colors[id] = CircleColor.LightBlue;
        }
    }
}
