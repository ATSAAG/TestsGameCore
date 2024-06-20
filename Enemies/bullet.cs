using Godot;
using System;

public partial class bullet : GroundedEnemy
{
	private RayCast2D[] _rayCasts;
	private float speed = 100.0f;
	public int Direction = -1; // aller à gauche
	//public int AutreDirection = 1; // aller à droite
	private bool launched = false;
	public player player;
	
	// player
	// public Camera2D camera =  new Camera2D();
	// AddChild(camera);
	
	// multijoueur
	// var scene = ResourceLoader.Load<PackedScene>("res://world.tscn").Instantiate<Node2D>();
	// GetTree().Root.AddChild(scene);
	
	public override void _Ready()
	{
		GD.Print("Missile creee");
		_rayCasts = new RayCast2D[1];
		_rayCasts[0] = GetNode<RayCast2D>("RayCast2D");

		boss_2 boss_testee = GetParent().GetNode("World").GetNode<boss_2>("boss_2");
		GlobalPosition = new Vector2(boss_testee.GlobalPosition.X, boss_testee.GlobalPosition.Y - 65);
		
	}
	
	public override void _PhysicsProcess(double delta)
	{
		//base._PhysicsProcess(delta);
		
		Vector2 velocity = Velocity;
		velocity.X += speed * Direction * (float)delta;
		Velocity = velocity;
		
		//GD.Print("missile move" + velocity);
		MoveAndSlide();
		
		//CheckRaycasts();
		
		//GD.Print("Missile process" + Position);
		
		if (Position.X<1)
		{
			QueueFree();
			boss_2 boss_testee = GetParent().GetNode("World").GetNode<boss_2>("boss_2");
			boss_testee.isMissileLaunched=false;
		}
	}
	
	private void _on_hitboxe_body_entered(Node2D body)
	{
		if (body is player)
		{
			((player)body).TakeHit(5);
		}
		if (!(body is boss_2))
		{
			QueueFree();
			GD.Print("Missile detruite avec " +body);
			boss_2 boss_testee = GetParent().GetNode("World").GetNode<boss_2>("boss_2");
			//GD.Print("boss_testee" + boss_testee );
			boss_testee.isMissileLaunched=false;
		}
	}
	
	
	
		
	public override Vector2 Move()
	{
		return Velocity;
	}
	

	
	public override void CheckRaycasts()
	{ 
	}
	
	public override void HandleAnimations()
	{
	}
	
	
}



