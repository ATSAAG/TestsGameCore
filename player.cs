using System;
using Godot;

public partial class player : CharacterBody2D
{
	//Setup player attributes.
	public const float BaseSpeed = 300.0f;
	public const float JumpVelocity = -400.0f;
	public float Health;
	public float Speed = BaseSpeed;
	private float _canJump;
	private bool _canFastFall;
	private bool _canDoubleJump;
	private bool _canDash;
	private float _dashing;
	private float _isShooting;
	private AnimatedSprite2D _sprite;
	private CollisionShape2D _collisionShape;
	private Area2D gunRight;
	private Area2D gunLeft;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public override void _Ready()
	{
		
		gunRight = GetNode<Area2D>("GunRight");
		gunRight.Monitoring = false;
		gunLeft = GetNode<Area2D>("GunLeft");
		gunLeft.Monitoring = false;
		_isShooting = 10;
		Health = 10;
		_collisionShape =GetNode<CollisionShape2D>("CollisionShape2D");
		_sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer").SetMultiplayerAuthority(int.Parse(Name));
		//permet de differencier les joueurs
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		string animSpe = "normal";
		if (GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer").GetMultiplayerAuthority() ==
			Multiplayer.GetUniqueId())
		{
			// Add the gravity, fast fall.
			if (!IsOnFloor())
				if (Name != "1")
				{
					if (Input.IsActionJustPressed("Joycon_down") && _canFastFall)
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
				}
				else
				{
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
				}

			// Handle Jump and double jump.
			if (IsOnFloor())
			{
				_canJump = 0.2f;
				_canFastFall = true;
				_canDoubleJump = false;
				_canDash = true;
			}
			else
			{
				if (Name != "1")
				{
					if (Input.IsActionJustPressed("Joycon_jump") && _canDoubleJump)
					{
						velocity.Y = JumpVelocity;
						_canDoubleJump = false;
					}
					else
					{
						if (_canJump == 0.2f)
						{
							_canDoubleJump = true;
						}
						_canJump -= 0.05f;
					}
				}
				else
				{
					if (Input.IsActionJustPressed("jump") && _canDoubleJump)
					{
						velocity.Y = JumpVelocity;
						_canDoubleJump = false;
					}
					else
					{
						if (_canJump == 0.2f)
						{
							_canDoubleJump = true;
						}
						_canJump -= 0.05f;
					}
				}
			}
			// Handle wall jump and slide

			if (IsOnWall() && !IsOnFloor() && (Input.IsActionPressed("right") || Input.IsActionPressed("left") || Input.IsActionPressed("Joycon_right") || Input.IsActionPressed("Joycon_left")))
			{
				animSpe = "WallSlide";
				_canDash = true;
				_canJump = 0.2f;
			}
			
			// Handle dash

			if (Name == "1")
			{
				if (Input.IsActionJustPressed("dash") && _canDash)
				{
					_dashing = 1f;
					animSpe = "Dash";
					_canDash = false;
					Speed *= 7;
				}
				else
				{
					_dashing -= 0.1f;
					if (Speed > BaseSpeed)
					{
						Speed -= 300;
					}
				}
			}
			else
			{
				if (Input.IsActionJustPressed("Joycon_dash") && _canDash)
				{
					_dashing = 1f;
					animSpe = "Dash";
					_canDash = false;
					Speed *= 7;
				}
				else
				{
					_dashing -= 0.1f;
					if (Speed > BaseSpeed)
					{
						Speed -= 300;
					}
				}
			}

			if (Name == "1")
			{
				if (Input.IsActionJustPressed("jump") && _canJump > 0)
				{
					velocity.Y = JumpVelocity;
					_canDoubleJump = true;
				}
			}
			else
			{
				if (Input.IsActionJustPressed("Joycon_jump") && _canJump > 0)
				{
					velocity.Y = JumpVelocity;
					_canDoubleJump = true;
				}
			}


			// Get the input direction and handle the movement/deceleration.
			if (Name == "1")
			{
				Vector2 direction = Input.GetVector("left", "right", "up", "down");
				if (direction != Vector2.Zero)
				{
					velocity.X = direction.X * Speed;
				}
				else
				{
					if (_dashing > 0)
					{
						_sprite.Offset = direction;
						if (!_sprite.FlipH)
						{
							velocity.X = Speed;
						}
						else
						{
							velocity.X = -Speed;
						}
					}
					else
					{
						velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
					}
				}
			}
			else
			{
				Vector2 direction = Input.GetVector("Joycon_left", "Joycon_right", "Joycon_up", "Joycon_down");
				if (direction != Vector2.Zero)
				{
					velocity.X = direction.X * Speed;
				}
				else
				{
					if (_dashing > 0)
					{
						_sprite.Offset = direction;
						if (!_sprite.FlipH)
						{
							velocity.X = Speed;
						}
						else
						{
							velocity.X = -Speed;
						}
					}
					else
					{
						velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
					}
				}
			}
			
			if (Input.IsActionJustPressed("Joycon_shoot") && Name != "1")
			{
				_isShooting--;
					if (_sprite.FlipH)
					{
						velocity.X += 1500;
						gunLeft.Monitoring = true;
					}
					else
					{
						velocity.X += -1500;
						gunRight.Monitoring = true;
					}
			}
			
			if (Input.IsActionJustPressed("shoot") && Name == "1")
			{
				_isShooting--;
				if (_sprite.FlipH)
				{
					velocity.X += 1500;
					gunLeft.Monitoring = true;
				}
				else
				{
					velocity.X += -1500;
					gunRight.Monitoring = true;
				}
			}
			
			// Handle animations.
			if (_dashing > 0)
			{
				animSpe = "Dash";
			}
			
			if (_isShooting <= 0)
			{
				_isShooting = 10;
				gunRight.Monitoring = false;
				gunLeft.Monitoring = false;
			}
			if (_isShooting != 10)
			{
				animSpe = "Shoot";
				_isShooting--;
			}

			Velocity = velocity;
			_HandleAnimations(Velocity, animSpe);
			MoveAndSlide();

		}
		
	}
	// Handle animations.
	private void _HandleAnimations(Vector2 vitesse, string animSpe)
	{
		// Handle which direction the player is looking.
		if (animSpe != "Dash" && animSpe != "Shoot")
		{
			if (vitesse.X < 0)
			{
				_sprite.FlipH = true;
			}

			if (vitesse.X > 0)
			{
				_sprite.FlipH = false;
			}
		}

		// Handle basic animations
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
		if (Health == 10)
		{
			_sprite.Play(animSpe);
		}
		else
		{
			Health++;
			_sprite.Play("Hit");
		}
	}

	public void SetPlayerName(string name)
	{
		GetNode<Label>("Label").Text = name;
	}
	
	private void _on_gun_body_entered(Node2D body)
	{
		if ((body.Name == "RunningFrog" || body.Name == "SniperFrog"))
		{
			((GroundedEnemy)body).Hit(5);
		}
	}

	public void TakeHit(float damage)
	{
		Vector2 velocity = Velocity;
		if (Velocity.X > 0)
		{
			velocity.X = 2500;
			Health -= damage;
		}
		else
		{
			velocity.X = -2500;
			Health -= damage;
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
