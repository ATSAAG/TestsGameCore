using System.Linq;
using Godot;
using TestMovements.Enemies;
using Vector2 = Godot.Vector2;

public abstract partial class GroundedEnemy : CharacterBody2D
{
	public float Health;
	protected float Speed;
	protected RayCast2D[] _rayCasts;
	protected AnimatedSprite2D _sprite;
	public float Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	protected bool isAttacking = false;

	public override void _PhysicsProcess(double delta)
	{

		Godot.Vector2 velocity = Move();
		// Add the gravity
		if (!IsOnFloor())
			velocity.Y += Gravity * (float)delta;
		CheckRaycasts();
		HandleAnimations();
		Velocity = velocity;
		MoveAndSlide();
	}

	public abstract Godot.Vector2 Move();
	public abstract void CheckRaycasts();

	public abstract void HandleAnimations();

	public void Hit(float damage)
	{
		Health -= damage;
		if (Health <= 0)
		{
			/*var id = 0;
			MultiplayerManagment.Enemies.Remove(MultiplayerManagment.Enemies.Where(i => i.Id == id).First<EnemyInfo>());
			var playerr = GetTree().GetNodesInGroup("Player");
			foreach (var i in playerr)
			{
				//GetNode<Label>("Label").Text = "*disconected*";
				GD.Print(i.Name);
				if (i.Name == id.ToString())
				{
					i.QueueFree();
				}
			}*/

			Die(this);
			Rpc("Die", this);

		}
	}

	public void Die(GroundedEnemy a)
	{
		a.QueueFree();
	}
}
