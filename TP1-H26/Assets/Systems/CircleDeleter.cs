public class CircleDeleter : ISystem
{
    public string Name => "RegionManagement";
    public ECSController controller = ECSController.Instance;

    public void UpdateSystem()
    {
        foreach (uint circle in DeadCircles.deadCircles)
        {
            controller.DestroyShape(circle);
            Positions.circlePositions.Remove(circle);
            Velocities.velocities.Remove(circle);
            Sizes.sizes.Remove(circle);
            Colors.colors.Remove(circle);
            Protections.protections.Remove(circle);
            CollisionPair.collisionPairs.Remove(circle);
            CollisionCount.collisionCount.Remove(circle);
            CollisionBehavior.behaviors.Remove(circle);
            Explosion.explosions.Remove(circle);
            LeftSideCircles.circlesOnLeftSide.Remove(circle);
            RightSideCircles.circlesOnRightSide.Remove(circle);
            Regions.regions.Remove(circle);
        }
        DeadCircles.deadCircles.Clear();
    }
}
