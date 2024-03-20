using Godot;
using Vector2 = Godot.Vector2;

public abstract partial class GroundedEnemy : CharacterBody2D
{
	protected float Speed;
	protected RayCast2D[] _rayCasts;
	protected AnimatedSprite2D _sprite;
	public float Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	protected bool isAttacking = false;
	
	public override void _PhysicsProcess(double delta)
	{
		// Add constant speed
		Vector2 velocity = Velocity;

		// Add the gravity
		if (!IsOnFloor())
			velocity.Y += Gravity * (float)delta;
		
		velocity = Move();
		CheckRaycasts();
		HandleAnimations();
		Velocity = velocity;
		MoveAndSlide();
	}

	public abstract Godot.Vector2 Move();
	public abstract void CheckRaycasts();

	public abstract void HandleAnimations();
}
