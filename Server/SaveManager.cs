using Godot;
using System;

public partial class SaveManager : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	private void _on_go_back_button_down()
	{
		var cur = GetTree().CurrentScene;
		var scene = ResourceLoader.Load<PackedScene>("res://Server/MainMenu.tscn").Instantiate<Control>();
		GetTree().Root.AddChild(scene);
		
		cur.QueueFree();
		GetTree().CurrentScene = scene;
	}
	private void _on_load_level_button_down()
	{
		GetNode<loading_screen>("LoadingScreen").LoadLevel("res://world.tscn");
	}
	private void _on_continue_button_down()
	{
		SaveLoadManager.SaveGame("save");
		var gameData = SaveLoadManager.LoadGame("save");
		GD.Print(gameData["Path"]);
		MultiplayerManagment.multiplayerManagment.LoadLevel(gameData["Path"]);
	}


	private void _on_new_button_down()
	{
		DirAccess directory = DirAccess.Open("user://");
		if (directory.DirExists("Saves"))
		{
			directory.Remove("Saves");
		}
		GetNode<loading_screen>("LoadingScreen").LoadLevel("res://world.tscn");

		SaveLoadManager.SaveGame("save");

	
	}

}

