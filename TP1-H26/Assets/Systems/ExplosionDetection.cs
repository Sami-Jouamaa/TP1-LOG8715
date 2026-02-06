public class ExplosionDetectionSystem : ISystem
{ 
    public ECSController controller = ECSController.Instance; 
    public string Name => "ExplosionDetection"; public int ExplosionSize = ECSController.Instance.Config.explosionSize; 
    public void UpdateSystem() {
        foreach (uint id in Sizes.sizes.Keys)
            if (SimStep.currentSimStep == 0 || Regions.regions.TryGetValue(id, out var region) && region == CircleRegion.Left) 
                ExplosionDetection(id); 
    } 
    public void ExplosionDetection(uint circleId) 
    { 
        if (!Sizes.sizes.ContainsKey(circleId)) return; 
        if (Sizes.sizes[circleId] >= ExplosionSize) 
        { 
            Explosion.explosions[circleId] = ExplosionState.Exploding; 
        } 
    } 
}