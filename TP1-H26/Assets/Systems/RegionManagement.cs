public class RegionManagement : ISystem
{
    public string Name => "RegionManagement";
    public void UpdateSystem() {
        foreach (var (id, position) in Positions.circlePositions) {
            float CircleRadius = Sizes.sizes[id] / 2f;
            if (position.x - CircleRadius < 0)
                Regions.regions[id] = CircleRegion.Left;
            else
                Regions.regions[id] = CircleRegion.Right;
        }
    }
}
