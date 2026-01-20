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
        toRegister.Add(new CirclesManagement());

        // Creating components from config
        for (uint i = 0; i < config.Count; i++)
        {
            Positions.circlePositions.Add(i, config[(int)i].initialPosition);
            InitialVelocity.initialVelocities.Add(i, config[(int)i].initialVelocity);
            InitialSizes.initialSizes.Add(i, config[(int)i].initialSize);
        }


        return toRegister;
    }
}