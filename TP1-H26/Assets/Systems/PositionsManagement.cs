using UnityEngine;

public class PositionsManagement : ISystem
{
    public string Name => "PositionsManagement";
    public ECSController controller = ECSController.Instance;

    public void UpdateSystem()
    {
        foreach (var leftSideId in LeftSideCircles.circlesOnLeftSide)
        {
            for (int fasterIteration = 0; fasterIteration < 4; fasterIteration++)
            {
                ChangePositions(leftSideId);
            }
        }

        foreach (var rightSideId in RightSideCircles.circlesOnRightSide)
        {
            ChangePositions(rightSideId);
        }
    }

    public void ChangePositions(uint circleId)
    {
        if (!Positions.circlePositions.ContainsKey(circleId)) return;
        Positions.circlePositions[circleId] += Velocities.velocities[circleId] * Time.deltaTime;
        controller.UpdateShapePosition(circleId, Positions.circlePositions[circleId]);
    }
}
