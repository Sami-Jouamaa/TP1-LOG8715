using System.Collections.Generic;
using UnityEngine;

public class CollisionManagement : ISystem
{
    public string Name => "CollisionManager";

    public void UpdateSystem()
    {
        Dictionary<uint, Vector2> positionDictionary = Positions.circlePositions;
        // (x2 - x1)^2 + (y2 - y1) <= (r1 + r2)^2
        for (uint i = 0; i < Positions.circlePositions.Count; i++)
        {
            for (uint j = i + 1; j < Positions.circlePositions.Count; j++)
            {
                return;
            }
        }
    }
}
