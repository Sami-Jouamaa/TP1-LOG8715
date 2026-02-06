using System.Collections.Generic;
public class RegisterSystems
{  
    private static void AddSystemList(List<ISystem> list)
    {
        // movement & physics
        list.Add(new RegionManagement());
        list.Add(new PositionsManagement());
        list.Add(new CollisionDetection());

        // protection management 
        list.Add(new ProtectionTickManagement());
        list.Add(new ProtectionStartManagement());

        // colors
        list.Add(new ColorSetManagement());
        list.Add(new ColorDisplayManagement());

        // collision processing and closing (needs to be after all systems checking collisions)
        list.Add(new WallCollision());
        list.Add(new CollisionManagement());

        // sets real size
        list.Add(new SizeManagement());

        // explosion
        list.Add(new ClickExplosionSystem());
        list.Add(new ExplosionDetectionSystem());
        list.Add(new ExplosionExecutionSystem());

        list.Add(new SimStepCounter());
    }
    public static List<ISystem> GetListOfSystems()
    {
        ECSController controller = ECSController.Instance;
        
        List<Config.ShapeConfig> config = controller.Config.circleInstancesToSpawn;
        // determine order of systems to add

        var toRegister = new List<ISystem>();
        // Add your systems here

        // Creating components from config
        for (uint i = 0; i < config.Count; i++)
        {
            Positions.circlePositions.Add(i, config[(int)i].initialPosition);
            Velocities.velocities.Add(i, config[(int)i].initialVelocity);
            int size = config[(int)i].initialSize;
            Sizes.sizes.Add(i, size);
            CollisionCount.collisionCount.Add(i, 0);
            LifeStates.lifeStates.Add(i, LifeState.Alive);
            if (Velocities.velocities[i].x == 0 && Velocities.velocities[i].y == 0)
            {
                CollisionBehavior.behaviors.Add(i, Behavior.Static);
            }
            else
            {
                CollisionBehavior.behaviors.Add(i, Behavior.Dynamic);
            }

            controller.CreateShape(i, size);
        }
        for (int i = 0; i < 4; i++)
            AddSystemList(toRegister);

        return toRegister;
    }

}