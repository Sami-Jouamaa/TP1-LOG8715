using UnityEngine;

public class PositionsManagement : ISystem
{
    public string Name => "PositionsManagement";
    public ECSController controller = ECSController.Instance;
    public bool firstTime = true;

    public void UpdateSystem()
    {
        for (uint i = 0; i < Positions.circlePositions.Count; i++)
        {
            float newX;
            float newY;
            if (firstTime)
            {
                newX = Positions.circlePositions[i].x;
                newY = Positions.circlePositions[i].y;
                firstTime = false;
            }
            else
            {
                newX = InitialVelocity.initialVelocities[i].x + Positions.circlePositions[i].x;
                newY = InitialVelocity.initialVelocities[i].y + Positions.circlePositions[i].y;
            }
            Vector2 newPosition = new Vector2(newX, newY);
            Positions.circlePositions[i] = newPosition;
            controller.UpdateShapePosition(i, newPosition);
        }
    }
}
