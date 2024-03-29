using Godot;
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
			QueueFree();
		}
	}
}
