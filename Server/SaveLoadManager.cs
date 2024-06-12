using Godot;
using System;

public partial class SaveLoadManager : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void SaveGame(string name)
	{
		DirAccess directory = DirAccess.Open("user://");
		if (!directory.DirExists("Saves"))
		{
			directory.MakeDir("Saves");
		}

		directory = DirAccess.Open("user://Saves");
		if (!directory.DirExists(name))
		{
			directory.MakeDir(name);
		}
	}
}
