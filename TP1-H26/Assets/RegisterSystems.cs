using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor.PackageManager.Requests;

public class RegisterSystems
{
    private static void AddSystemList(List<ISystem> list)
    {
        list.Add(new CirclesManagement());

        // movement & physics
        list.Add(new PositionsManagement());
        list.Add(new CollisionDetection());

        // protection management 
        list.Add(new ProtectionTickManagement());
        list.Add(new ProtectionStartManagement());

        // colors
        list.Add(new ColorSetManagement());
        list.Add(new ColorDisplayManagement());

        // collision processing and closing (needs to be after all systems checking collisions)
        list.Add(new CollisionManagement());

        // sets real size
        list.Add(new SizeManagement());

        // explosion
        list.Add(new ExplosionDetectionSystem());
        list.Add(new ExplosionExecutionSystem());

        list.Add(new CircleDeleter());
    }
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

            if (Positions.circlePositions[i].x < 0)
            {
                LeftSideCircles.circlesOnLeftSide.Add(i);
            }
            else
            {
                RightSideCircles.circlesOnRightSide.Add(i);
            }
        }
        AddSystemList(toRegister);


        return toRegister;
    }

}