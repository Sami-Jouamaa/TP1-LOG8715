
public class ColorSetManagement : ISystem
{
    public ECSController controller = ECSController.Instance;
    public string Name => "ColorSetManagement";
    public void UpdateSystem()
    {
        foreach (var (id, _) in Positions.circlePositions) {
            Behavior behavior = CollisionBehavior.behaviors[id];
            bool isInCollision = CollisionPair.collisionPairs.ContainsKey(id);
            int size = Sizes.sizes[id];
            if (behavior == Behavior.Static)
                Colors.colors[id] = CircleColor.Red;
            else if (isInCollision)
                Colors.colors[id] = CircleColor.Green;
            else if (Protections.protections.ContainsKey(id))
                Colors.colors[id] = Protections.protections[id].Remaining > 0 ? CircleColor.White : CircleColor.Yellow;
            else if (size < controller.Config.protectionSize)
                Colors.colors[id] = CircleColor.LightBlue;
            else if (size == controller.Config.explosionSize - 1)
                Colors.colors[id] = CircleColor.Orange;
            //else if (isNewExplosionCircle)
            //  Colors.colors[id] = CircleColor.Pink;
            else
                Colors.colors[id] = CircleColor.DarkBlue;
            //verify if can be protected means under the size threshold
        }
    }
}
