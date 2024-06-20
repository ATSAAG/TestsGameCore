using Godot;
using System;

public partial class boss_2 : GroundedEnemy
{	
	//[Export] public PackedScene bullet;
	
	public float DistanceOfDetction = 450.0f;
	public Vector2 PlayerPos;
	public bool isMissileLaunched = false;
	public new bool isAttacking = false;
	player player;
	
	public override void _Ready()
	{
		GD.Print("Boss2 ready");
		
		Speed = 100.0f;
		_rayCasts = new RayCast2D[2];
		_rayCasts[0] = GetNode<RayCast2D>("RayCast2D_boss2");
		_rayCasts[1] = GetNode<RayCast2D>("RayCast2D2_boss2");
		_sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D_boss2");
		Health = 10;
		PlayerPos.X = 0;	
		PlayerPos.Y = 0;
		
				
	}
	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		
		if (isAttacking) 
		{
			if (!isMissileLaunched)
			{
				_sprite.Play("shoot");
				//GD.Print("Boss tire à gauche);		
				
				var bulletScene = (PackedScene)ResourceLoader.Load("res://Enemies/bullet.tscn");
				
				if (bulletScene != null)
				{
					GD.Print("Launching missiles");
					bullet Bullet1 = (bullet)bulletScene.Instantiate();
					bullet Bullet2 = (bullet_2)bulletScene.Instantiate();
					bullet Bullet3 = (bullet_3)bulletScene.Instantiate();
					
					Vector2 basePosition = this.GlobalPosition;
					float spacing = 20.0f; // Space between missiles
					
					Bullet1.GlobalPosition = new Vector2(basePosition.X - spacing, basePosition.Y);
					Bullet2.GlobalPosition = basePosition;
					Bullet3.GlobalPosition = new Vector2(basePosition.X + spacing, basePosition.Y);
					
					GetTree().Root.AddChild(Bullet1);
					GetTree().Root.AddChild(Bullet2);
					GetTree().Root.AddChild(Bullet3);
					
					int direction = GlobalPosition.X < PlayerPos.X ? 1 : -1;
					Bullet1.Direction = direction;
					Bullet2.Direction = direction;
					Bullet3.Direction = direction;
					
					isMissileLaunched = true;
					player.Health -= 5;
				
					//GD.Print("player.Health = " + player.Health);	
				}
			}
		}
		
	}
	
	public override Vector2 Move()
	{
		return Velocity;
	}
	
	public override void CheckRaycasts()
	{
		if (_rayCasts[0].IsColliding())
		{
			GD.Print("CheckRaycasts right");
			Speed = -Mathf.Abs(Speed);
			_sprite.FlipH = false;
		}
		else if (_rayCasts[1].IsColliding())
		{
			GD.Print("CheckRaycasts left");
			Speed = Mathf.Abs(Speed);
			_sprite.FlipH = true;
		}
		
	}

	public override void HandleAnimations()
	{
		//GD.Print("HandleAnimations");
		_sprite.FlipH = Speed < 0;
		if (isAttacking) 
		{
			_sprite.Play("attack");
		}
		else
		{
			_sprite.Play("walk");
		}
			
	}
	
	private void _on_hitboxe_body_entered(Node2D body)
	{
		GD.Print("Hit");
		if (body is player)
		{	
			((player)body).TakeHit(5);
			GD.Print("Health of the player = " + $"{((player)body).Health}");			
		}
		//Explode();
	}
}

