using System.Linq;
using UnityEngine;

public class PositionsManagement : ISystem
{
    public string Name => "PositionsManagement";
    public ECSController controller = ECSController.Instance;

    public void UpdateSystem()
    {   
        foreach (var (id, _) in Positions.circlePositions.ToList()) 
        {

            Positions.circlePositions[id] += Velocities.velocities[id] * Time.deltaTime;
            controller.UpdateShapePosition(id, Positions.circlePositions[id]);
        }
    }
}
