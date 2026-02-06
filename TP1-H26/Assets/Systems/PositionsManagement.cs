using System.Linq;
using UnityEngine;

public class PositionsManagement : ISystem
{
    public string Name => "PositionsManagement";
    public ECSController controller = ECSController.Instance;

    public void UpdateSystem()
    {
        foreach (uint id in Positions.circlePositions.Keys.ToList()){
            if (LifeStates.lifeStates[id] == LifeState.Dead)
                continue;
            if (SimStep.currentSimStep == 0 || Regions.regions.TryGetValue(id, out var region) && region == CircleRegion.Left)
                ChangePositions(id);
        }
    }

    public void ChangePositions(uint circleId)
    {
        Positions.circlePositions[circleId] += Velocities.velocities[circleId] * Time.deltaTime;
        controller.UpdateShapePosition(circleId, Positions.circlePositions[circleId]);
    }
}
