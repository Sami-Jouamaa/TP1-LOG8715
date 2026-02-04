using UnityEngine;

public class CirclesManagement : ISystem
{
    public ECSController controller = ECSController.Instance;
    public string Name => "CirclesManagement";
    public bool firstTimeRunning = true;

    public void UpdateSystem()
    {
        if (firstTimeRunning)
        {
            for (uint i = 0; i < Sizes.sizes.Count; i++)
            {
                controller.CreateShape(i, Sizes.sizes[i]);
            }
            firstTimeRunning = false;
        }

        // Move elements around depending on x coordinate
        foreach (var (id, position) in Positions.circlePositions)
        {
            int index;
            if (position.x >= 0 && !RightSideCircles.circlesOnRightSide.Contains(id) && LeftSideCircles.circlesOnLeftSide.Contains(id))
            {
                index = LeftSideCircles.circlesOnLeftSide.IndexOf(id);
                LeftSideCircles.circlesOnLeftSide.RemoveAt(index);
                RightSideCircles.circlesOnRightSide.Add(id);
            }
            else if (position.x < 0 && !LeftSideCircles.circlesOnLeftSide.Contains(id) && RightSideCircles.circlesOnRightSide.Contains(id))
            {
                index = RightSideCircles.circlesOnRightSide.IndexOf(id);
                RightSideCircles.circlesOnRightSide.RemoveAt(index);
                LeftSideCircles.circlesOnLeftSide.Add(id);
            }
        }
    }
}
