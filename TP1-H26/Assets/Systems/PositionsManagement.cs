using System.Linq;
using UnityEngine;

public class PositionsManagement : ISystem
{
    public string Name => "PositionsManagement";
    public ECSController controller = ECSController.Instance;

    public void UpdateSystem()
    {   
        foreach (var (id, position) in Positions.circlePositions.ToList()) 
        {
            float newX;
            float newY;
            newX = Velocities.velocities[id].x + position.x;
            newY = Velocities.velocities[id].y + position.y;
            Vector2 newPosition = new Vector2(newX, newY);
            Positions.circlePositions[id] = newPosition;
            controller.UpdateShapePosition(id, newPosition);
            // Debug.Log("id: " + id + " x: " + newX + " y: " + newY + " velx: " + Velocities.velocities[id].x + " vely: " + Velocities.velocities[id].y);
        }
    }
}
