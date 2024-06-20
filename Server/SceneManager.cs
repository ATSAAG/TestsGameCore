	using Godot;
using System;

public partial class SceneManager : Node2D
{

	[Export] private PackedScene playerScene;
	[Export] private PackedScene enemyScene;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SpawnPlayer();
	}

	private void SpawnPlayer()
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

	private void SpawnEnemies()
	{
		int index = 0;
		foreach (var i in MultiplayerManagment.Enemies)
		{
			GroundedEnemy currentEnemy = enemyScene.Instantiate<GroundedEnemy>();
			currentEnemy.Name = i.Id.ToString();
			
			AddChild(currentEnemy);
			// foreach (Node2D spawnPoint in GetTree().GetNodesInGroup("PlayerSpawn"))
			// {
			// 	if (int.Parse(spawnPoint.Name)== index)
			// 	{
			// 		currentEnemy.GlobalPosition = spawnPoint.GlobalPosition;
			// 	}
			// }

			index += 1;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	private void _on_go_to_level_2_area_entered(Area2D area)
	{
		CheckPointManager._player1.Position = GetNode<Area2D>("TileMap3/Pos2").Position;
		CheckPointManager._player2.Position = GetNode<Area2D>("TileMap3/Pos2").Position.MoveToward(CheckPointManager._player1.Position,-5);
	}
	private void _on_go_to_level_1_area_entered(Area2D area)
	{
		string info = "TileMap2/Pos1";
		CheckPointManager._player1.Position = GetNode<Area2D>(info).Position;
		CheckPointManager._player2.Position = GetNode<Area2D>(info).Position.MoveToward(CheckPointManager._player1.Position,-5);
	}
	private void _on_go_to_boss_area_entered(Area2D area)
	{
	
		string info = "TileMap7/Pos10";
		CheckPointManager._player1.Position = GetNode<Area2D>(info).Position;
		CheckPointManager._player2.Position = GetNode<Area2D>(info).Position.MoveToward(CheckPointManager._player1.Position,-5);
	}
	private void _on_go_to_level_4_area_entered(Area2D area)
	{
		string info = "TileMap6/Pos6";
		CheckPointManager._player1.Position = GetNode<Area2D>(info).Position;
		CheckPointManager._player2.Position = GetNode<Area2D>(info).Position.MoveToward(CheckPointManager._player1.Position,-5);
	}
	private void _on_go_to_level_42_area_entered(Area2D area)
	{
		string info = "TileMap6/Pos7";
		CheckPointManager._player1.Position = GetNode<Area2D>(info).Position;
		CheckPointManager._player2.Position = GetNode<Area2D>(info).Position.MoveToward(CheckPointManager._player1.Position,-5);
	}
	private void _on_go_to_level_5_area_entered(Area2D area)
	{
		string info = "TileMap5/Pos8";
		CheckPointManager._player1.Position = GetNode<Area2D>(info).Position;
		CheckPointManager._player2.Position = GetNode<Area2D>(info).Position.MoveToward(CheckPointManager._player1.Position,-5);
	}
	private void _on_go_to_level_32_area_entered(Area2D area)
	{
		string info = "TileMap4/Pos5";
		CheckPointManager._player1.Position = GetNode<Area2D>(info).Position;
		CheckPointManager._player2.Position = GetNode<Area2D>(info).Position.MoveToward(CheckPointManager._player1.Position,-5);
	}
	private void _on_go_to_demo_area_entered(Area2D area)
	{
		string info = "TileMap/Pos11";
		CheckPointManager._player1.Position = GetNode<Area2D>(info).Position;
		CheckPointManager._player2.Position = GetNode<Area2D>(info).Position.MoveToward(CheckPointManager._player1.Position,-5);
	}
	private void _on_go_to_level_22_area_entered(Area2D area)
	{
		string info = "TileMap3/Pos3";
		CheckPointManager._player1.Position = GetNode<Area2D>(info).Position;
		CheckPointManager._player2.Position = GetNode<Area2D>(info).Position.MoveToward(CheckPointManager._player1.Position,-5);
	}
	private void _on_go_to_level_3_area_entered(Area2D area)
	{
		string info = "TileMap4/Pos4";
		CheckPointManager._player1.Position = GetNode<Area2D>(info).Position;
		CheckPointManager._player2.Position = GetNode<Area2D>(info).Position.MoveToward(CheckPointManager._player1.Position,-5);
	}


	
}

	







