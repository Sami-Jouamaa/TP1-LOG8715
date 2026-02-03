public class SizeManagement : ISystem
{
    public ECSController controller = ECSController.Instance;
    public string Name => "SizeManagement";

    public void UpdateSystem()
    {
        foreach (var leftSideId in LeftSideCircles.circlesOnLeftSide)
        {
            for (int fasterIteration = 0; fasterIteration < 4; fasterIteration++)
            {
                ManageSize(leftSideId);
            }
        }

        foreach (var rightSideId in RightSideCircles.circlesOnRightSide)
        {
            ManageSize(rightSideId);
        }
    }

    public void ManageSize(uint circle)
    {
        if (Sizes.sizes[circle] <= 0)
            DeadCircles.deadCircles.Add(circle);
        controller.UpdateShapeSize(circle, Sizes.sizes[circle]);
    }
}

