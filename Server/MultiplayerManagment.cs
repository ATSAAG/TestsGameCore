using Godot;
using System;
using System.Collections.Generic;
using TestMovements.Enemies;

public partial class MultiplayerManagment : Node
{
	public static List<PlayerInfo> Players = new List<PlayerInfo>();

	public static List<EnemyInfo> Enemies = new List<EnemyInfo>();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
