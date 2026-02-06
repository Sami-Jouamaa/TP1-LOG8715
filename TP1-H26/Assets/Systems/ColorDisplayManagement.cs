using UnityEngine;
using System.Collections.Generic;
public class ColorDisplayManagement : ISystem
{
    private static readonly Dictionary<CircleColor, Color> RGBTraduction =
        new(){{
            CircleColor.Red, new Color(1f, 0f, 0f)},
            {CircleColor.Green, new Color(0f, 1f, 0f)},
            {CircleColor.White, new Color(1f, 1f, 1f)},
            {CircleColor.LightBlue, new Color(0.5f, 0.7f, 0.8f)},
            {CircleColor.Yellow, new Color(1f, 1f, 0f)},
            {CircleColor.Orange, new Color(1f, 0.5f, 0f)},
            {CircleColor.DarkBlue, new Color(0f, 0f, 0.5f)},
            {CircleColor.Pink, new Color(1f, 0.5f, 0.75f)}};
    public ECSController controller = ECSController.Instance;
    public string Name => "ColorDisplayManagement";
    public void UpdateSystem()
    {
        foreach (uint id in Colors.colors.Keys)
        {
            if (LifeStates.lifeStates[id] == LifeState.Dead)
                continue;
            if (SimStep.currentSimStep == 0 || Regions.regions.TryGetValue(id, out var region) && region == CircleRegion.Left)
                ColorDisplay(id);
        }
    }

    public void ColorDisplay(uint circleId)
    {
        CircleColor currentColour = Colors.colors[circleId];
        Color newColour = RGBTraduction[currentColour];
        controller.UpdateShapeColor(circleId, newColour);
    }
}

