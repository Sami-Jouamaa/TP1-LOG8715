public class ExplosionDetectionSystem : ISystem
{
    public ECSController controller = ECSController.Instance;
    public string Name => "ExplosionDetection";

    public void UpdateSystem()
    {
        int explosionSize =
            ECSController.Instance.Config.explosionSize;

        foreach (var (entity, size) in Sizes.sizes)
        {
            if (size >= explosionSize)
            {
                if (!Explosion.requests.ContainsKey(entity))
                {
                    Explosion.requests.Add(entity, true);
                }
            }
        }
    }
}
