using UnityEngine;

public class ClickExplosionSystem : ISystem
{
    public string Name => "ClickExplosion";

    public void UpdateSystem()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        Vector2 mouseWorld =
            Camera.main.ScreenToWorldPoint(Input.mousePosition);

        foreach (var (id, position) in Positions.circlePositions)
        {
            float radius = Sizes.sizes[id] / 2f;

            if (Vector2.Distance(mouseWorld, position) <= radius)
            {
                HandleClick(id);
                break;
            }
        }
    }

    private void HandleClick(uint id)
    {
        int size = Sizes.sizes[id];

        // small → disappear
        if (size < 4)
        {
            DeadCircles.deadCircles.Add(id);
            return;
        }

        // big → explode
        Explosion.explosions[id] = ExplosionState.Exploding;
    }
}
