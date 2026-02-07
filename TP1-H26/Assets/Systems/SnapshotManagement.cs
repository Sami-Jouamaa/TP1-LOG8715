using System;
using System.Collections.Generic;
using UnityEngine;

public class SnapshotManagement : ISystem
{
    public string Name => "SnapshotManagement";
    public ECSController controller = ECSController.Instance;
    private struct Snapshot
    {
        public float TimeStamp;
        public Dictionary<uint, Vector2> CirclePositions;
        public Dictionary<uint, Vector2> Velocities;
        public Dictionary<uint, int> Sizes;
        public Dictionary<uint, LifeState> LifeStates;
        public Dictionary<uint, ExplosionState> Explosions;
        public Dictionary<uint, int> collisionCounts;
        public Dictionary<uint, ProtectionTimers> Protections; 
        public Dictionary<uint, CircleColor> Colors; 
    }
    private static float firstSnapshotTime = 0f;
    private static readonly Queue<Snapshot> snapshots = new();

    public void UpdateSystem()
    {
        bool onCooldown = Time.time - firstSnapshotTime < 3f;
        while(!onCooldown && snapshots.Count > 0 && Time.time - snapshots.Peek().TimeStamp > 3f)
            snapshots.Dequeue();
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (Time.time - firstSnapshotTime < 3f)
            {
                double cooldown = Math.Round(3f - (Time.time - firstSnapshotTime), 2);
                Debug.Log("cooldown remaining: " + cooldown + " seconds");
            }
            else
                Rollback();
        }
        TakeSnapshot();
    }
    private static void TakeSnapshot()
    {
        snapshots.Enqueue(new Snapshot
        {
            TimeStamp = Time.time,
            CirclePositions = new Dictionary<uint, Vector2>(Positions.circlePositions),
            Velocities = new Dictionary<uint, Vector2>(Velocities.velocities),
            Sizes = new Dictionary<uint, int>(Sizes.sizes),
            LifeStates = new Dictionary<uint, LifeState>(LifeStates.lifeStates),
            Explosions = new Dictionary<uint, ExplosionState>(Explosion.explosions),
            collisionCounts = new Dictionary<uint, int>(CollisionCount.collisionCount),
            Protections = new Dictionary<uint, ProtectionTimers>(Protections.protections),
            Colors = new Dictionary<uint, CircleColor>(Colors.colors)
        });

    }
    private void Rollback()
    {
        Snapshot snapshot = snapshots.Dequeue();
        Positions.circlePositions = new Dictionary<uint, Vector2>(snapshot.CirclePositions);
        Velocities.velocities = new Dictionary<uint, Vector2>(snapshot.Velocities);
        Sizes.sizes = new Dictionary<uint, int>(snapshot.Sizes);
        DestroyOldCircles(snapshot);
        LifeStates.lifeStates = new Dictionary<uint, LifeState>(snapshot.LifeStates);
        Explosion.explosions = new Dictionary<uint, ExplosionState>(snapshot.Explosions);
        CollisionCount.collisionCount = new Dictionary<uint, int>(snapshot.collisionCounts);
        Protections.protections = new Dictionary<uint, ProtectionTimers>(snapshot.Protections);
        Colors.colors = new Dictionary<uint, CircleColor>(snapshot.Colors);

        snapshots.Clear();
        firstSnapshotTime = Time.time;
    }

    private void DestroyOldCircles(Snapshot snapshot)
    {
        foreach (uint id in LifeStates.lifeStates.Keys)
        {
            if (!snapshot.LifeStates.ContainsKey(id))
            {
                controller.DestroyShape(id);
            }
        }
    }
}
