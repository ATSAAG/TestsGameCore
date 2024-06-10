using Godot;

namespace TestMovements.Server;

public partial class NetworkEntity : CharacterBody2D
{
    [Export]
    public bool IsSyncEnabled = true;
    [Export]
    public float Health { get; set; } 
    [Export]
    public float Speed { get; set; }
    [Export]
    public bool IsAlive { get; set; } = true;
    public override void _Ready()
    {
        if (IsSyncEnabled && GetTree().GetMultiplayer().IsServer())
        {
            // Set the network master
            SetMultiplayerAuthority(GetTree().GetMultiplayer().GetUniqueId());
        }
    }

    public void _Process(float delta)
    {
        if (IsSyncEnabled && GetTree().GetMultiplayer().IsServer())
        {
            Rpc(nameof(SyncState), Position, Rotation, Health, IsAlive, Speed);
        }
    }

    [Rpc]
    public void SyncState(Vector2 position, float rotation, float health, bool isAlive, float speed)
    {
        if (GetTree().GetMultiplayer().IsServer()) return; 
        Position = position;
        Rotation = rotation;
        Health = health;
        IsAlive = isAlive;
        Speed = speed;
        if (!IsAlive)
        {
            QueueFree();
        }
    }

    public void Hit(float damage)
    {
        if (!IsAlive) return;

        Health -= damage;
        if (Health <= 0)
        {
            Health = 0;
            IsAlive = false;
            //QueueFree();
        }
        Rpc(nameof(SyncState), Position, Rotation, Health, IsAlive, Speed);
        Rpc(nameof(RemoveEntity));
    }
   [Rpc]
    public void RemoveEntity()
    {
        QueueFree();
    }
}