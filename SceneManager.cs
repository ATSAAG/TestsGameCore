	using Godot;
using System;

public partial class SceneManager : Node2D
{

	[Export] private PackedScene playerScene;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		int index = 0;
		foreach (var i in MultiplayerManagment.Players)
		{
			player currentPlayer = playerScene.Instantiate<player>();
			currentPlayer.Name = i.Id.ToString();
			currentPlayer.SetPlayerName(i.Name);
			AddChild(currentPlayer);
			foreach (Node2D spawnPoint in GetTree().GetNodesInGroup("PlayerSpawn"))
			{
				if (int.Parse(spawnPoint.Name)== index)
				{
					currentPlayer.GlobalPosition = spawnPoint.GlobalPosition;
				}
			}

			index ++;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
