using UnityEngine;
public class WallCollision : ISystem
{
    public ECSController controller = ECSController.Instance;
    public string Name => "WallCollision";

    public void UpdateSystem()
    {
        foreach (var (id, wall) in WallHits.wallHits)
        {
            if (LifeStates.lifeStates[id] == LifeState.Dead)
                continue;
            if (SimStep.currentSimStep == 0 || Regions.regions.TryGetValue(id, out var region) && region == CircleRegion.Left)
                WallBounce(id, wall);
        }
        WallHits.wallHits.Clear();
            
    }

    public static void WallBounce(uint id, WallPos wall)
    {
        float maxY = Camera.main.orthographicSize;
        float minY = -maxY;
        float maxX = maxY * Camera.main.aspect;
        float minX = -maxX;

        Vector2 velocity = Velocities.velocities[id];
        Vector2 position = Positions.circlePositions[id];
        float radius = (float)Sizes.sizes[id] / 2;
        switch (wall)
        {
            case WallPos.Bottom:
                velocity.y *= -1;
                position.y = minY + radius;
                break;
            case WallPos.Top:
                velocity.y *= -1;
                position.y = maxY - radius;
                break;
            case WallPos.Left:
                velocity.x *= -1;
                position.x = minX + radius;
                break;
            case WallPos.Right:
                velocity.x *= -1;
                position.x = maxX - radius;
                break;
        }
        Positions.circlePositions[id] = position;
        Velocities.velocities[id] = velocity;
    }

}
