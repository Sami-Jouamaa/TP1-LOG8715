using System.Collections.Generic;
using System.Diagnostics;

public class RegisterSystems
{
    public static List<ISystem> GetListOfSystems()
    {
        List<Config.ShapeConfig> config = ECSController.Instance.Config.circleInstancesToSpawn;
        // determine order of systems to add

        var toRegister = new List<ISystem>();
        // Add your systems here

        // Creating components from config
        for (uint i = 0; i < config.Count; i++)
        {
            Positions.circlePositions.Add(i, config[(int)i].initialPosition);
            Velocities.velocities.Add(i, config[(int)i].initialVelocity);
            Sizes.sizes.Add(i, config[(int)i].initialSize);
            CollisionCount.collisionCount.Add(i, 0);
            if (Velocities.velocities[i].x == 0 && Velocities.velocities[i].y == 0)
            {
                CollisionBehavior.behaviors.Add(i, Behavior.Static);
            }
            else
            {
                CollisionBehavior.behaviors.Add(i, Behavior.Dynamic);
            }
            ProtectedTime.protectedTime.Add(i, 0);
        }
        toRegister.Add(new CirclesManagement());
        toRegister.Add(new PositionsManagement());
        toRegister.Add(new CollisionManagement());
        toRegister.Add(new BehaviorManagement());



        return toRegister;
    }
}