using Godot;

public partial class player : CharacterBody2D
{
	//Setup player attributes.
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;
	private float _canJump;
	private bool _canFastFall;
	private bool _canDoubleJump;
	public float _animGoingTime;
	private AnimatedSprite2D _sprite;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public override void _Ready()
	{
		_sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer").SetMultiplayerAuthority(int.Parse(Name));
		//permet de differencier les joueurs
	}

	public override void _PhysicsProcess(double delta)
	{
		string animSpe = "normal";
		if (GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer").GetMultiplayerAuthority() ==
			Multiplayer.GetUniqueId())
		{
			//permet de gerer les animations des joueurs


			Vector2 velocity = Velocity;
			

			// Add the gravity, fast fall.
			if (!IsOnFloor())
				if (Input.IsActionJustPressed("down") && _canFastFall)
				{
					animSpe = "FastFall";
					_canFastFall = false;
				}
				else
				{
					if (!_canFastFall)
					{
						animSpe = "FastFall";
						velocity.Y += Gravity * (float)delta * 10;
					}
					else
					{
						velocity.Y += Gravity * (float)delta;
					}
				}

			// Handle Jump and double jump.
			if (IsOnFloor())
			{
				_canJump = 0.2f;
				_canFastFall = true;
				_canDoubleJump = false;
			}
			else
			{

				if (Input.IsActionJustPressed("jump") && _canDoubleJump)
				{
					velocity.Y = JumpVelocity;
					animSpe = "DoubleJump";
					_canDoubleJump = false;
				}
				else
				{
					_canJump -= 0.05f;
					_canDoubleJump = true;
				}
			}

			if (Input.IsActionJustPressed("jump") && _canJump > 0)
				velocity.Y = JumpVelocity;

			// Get the input direction and handle the movement/deceleration.
			Vector2 direction = Input.GetVector("left", "right", "up", "down");
			if (direction != Vector2.Zero)
			{
				velocity.X = direction.X * Speed;
			}
			else
			{
				velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			}

			Velocity = velocity;

			// Handle animations.

			

			MoveAndSlide();
		}
		_HandleAnimations(Velocity, animSpe);
	}
	
	// Handle animations.
	private void _HandleAnimations(Vector2 vitesse, string animSpe)
	{
		// Handle which direction the player is looking.
		if (vitesse.X < 0)
		{
			_sprite.FlipH = true;
		}

		if (vitesse.X > 0)
		{
			_sprite.FlipH = false;
		}

		// Handle basic animations
		if (_animGoingTime == 0)
		{

			if (animSpe == "normal")
			{
				if (IsOnFloor())
				{
					if (vitesse == Vector2.Zero)
					{
						animSpe = "Idle";
					}
					else
					{
						animSpe = "Run";
					}
				}
				else
				{
					if (vitesse.Y > 0)
					{
						animSpe = "Fall";
					}
					else
					{
						animSpe = "Jump";
					}
				}
			}

			// Handle continuous animations.
			if (animSpe != _sprite.Animation)
			{
				_sprite.Play(animSpe);

			}
		}
		else
		{
			_animGoingTime--;
		}
	}

	public void SetPlayerName(string name)
	{
		GetNode<Label>("Label").Text = name;
	}
}
