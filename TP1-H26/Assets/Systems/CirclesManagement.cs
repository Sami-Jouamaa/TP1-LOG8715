using System;
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
            for (uint i = 0; i < InitialSizes.initialSizes.Count; i++)
            {
                controller.CreateShape(i, InitialSizes.initialSizes[i]);
            }
            firstTimeRunning = false;
        }
    }
}
