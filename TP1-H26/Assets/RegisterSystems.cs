using System.Collections.Generic;
using System.Diagnostics;

public class RegisterSystems
{
    public static List<ISystem> GetListOfSystems()
    {
        List<Config.ShapeConfig> config = ECSController.Instance.Config.circleInstancesToSpawn;
        UnityEngine.Debug.Log(config[0].initialPosition);
        // determine order of systems to add

        var toRegister = new List<ISystem>();
        
        // Creating components from config
        for (int i = 0; i < config.Count; i++)
        {
            Positions.circlePositions.Add(i, config[i].initialPosition);
            InitialVelocity.initialVelocities.Add(i, config[i].initialVelocity);
            InitialSizes.initialSizes.Add(i, config[i].initialSize);
        }

        // Add your systems here
        //Positions.circlePositions = 
        return toRegister;
    }
}