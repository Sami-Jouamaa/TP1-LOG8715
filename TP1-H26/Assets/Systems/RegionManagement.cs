public class RegionManagement : ISystem
{
    public string Name => "RegionManagement";
    public void UpdateSystem() {
        foreach (var (id, position) in Positions.circlePositions) {
            if (LifeStates.lifeStates[id] == LifeState.Dead)
                continue;
            if (position.x < 0)
                Regions.regions[id] = CircleRegion.Left;
            else
                Regions.regions[id] = CircleRegion.Right;
        }
    }
}
