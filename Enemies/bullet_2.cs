using Godot;
using System;

public partial class bullet_2 : bullet
{
	public override void _Ready()
	{
		base._Ready();
		GD.Print("bullet_2 created");
		// Comportements spécifiques à bullet_2
	}
}
