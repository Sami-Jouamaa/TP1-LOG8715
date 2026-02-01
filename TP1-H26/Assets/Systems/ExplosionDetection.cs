public class ExplosionDetectionSystem : ISystem
{
    public ECSController controller = ECSController.Instance;
    public string Name => "ExplosionDetection";

    public void UpdateSystem()
    {
        int explosionSize =
            ECSController.Instance.Config.explosionSize;

        foreach (var (circle, size) in Sizes.sizes)
        {
            if (size >= explosionSize)
            {
                if (!Explosion.explosions.ContainsKey(circle))
                {
                    Explosion.explosions.Add(circle, true);
                }
            }
        }
    }
}
