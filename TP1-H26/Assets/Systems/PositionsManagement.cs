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
            int newX;
            int newY;
            if (firstTime)
            {
                newX = (int)Positions.circlePositions[i].x;
                newY = (int)Positions.circlePositions[i].y;
                firstTime = false;
            }
            else
            {
                newX = (int)(InitialVelocity.initialVelocities[i].x + Positions.circlePositions[i].x);
                newY = (int)(InitialVelocity.initialVelocities[i].y + Positions.circlePositions[i].y);
            }
            Vector2 newPosition = new Vector2(newX, newY);
            controller.UpdateShapePosition(i, newPosition);
        }
    }
}
