public class ColorSetManagement : ISystem
{
    public ECSController controller = ECSController.Instance;
    public string Name => "ColorSetManagement";
    public void UpdateSystem()
    {
        foreach (var (id, _) in Positions.circlePositions) {
            Behavior behavior = CollisionBehavior.behaviors[id];
            bool isInCollision = CollisionPair.collisionPairs.ContainsKey(id);
            if (behavior == Behavior.Static)
                Colors.colors[id] = CircleColor.Red;
            else if (isInCollision)
                Colors.colors[id] = CircleColor.Green;
            else if (Protections.protections.ContainsKey(id))
                Colors.colors[id] = Protections.protections[id].Remaining > 0 ? CircleColor.White : CircleColor.Yellow;
            else
                Colors.colors[id] = CircleColor.LightBlue;
            //verify if can be protected means under the size threshold
        }
    }
}
