using Godot;
using System;
using System.Linq;


public partial class MultiplayerLobby : Control
{
	private ENetMultiplayerPeer network = new ENetMultiplayerPeer();
	[Export] private int Port = 8910;
	[Export] private string Adress = "127.0.0.1";
	private ENetMultiplayerPeer peer;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
		Multiplayer.PeerConnected += PlayerConnected;
		Multiplayer.PeerDisconnected += PlayerDisconnected;
		Multiplayer.ConnectedToServer += ConnectedToServer;
		Multiplayer.ConnectionFailed += ConnectionFailed;
		if (OS.GetCmdlineArgs().Contains("--server"))
		{
			peer = new ENetMultiplayerPeer();
			var error = peer.CreateServer(Port, 2);

			//add_player();
			if (error != Error.Ok)
			{
				GD.Print("cannot host" + $"{error}");
				return;
			}

			peer.Host.Compress(ENetConnection.CompressionMode.RangeCoder); //optional compression to get less lag


			Multiplayer.MultiplayerPeer = peer;


			GD.Print("Waiting for player");
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	private void ConnectedToServer()
	{
		GD.Print("connected to server");
		RpcId(1,"SendPlayerInformation", GetNode<LineEdit>("LineEdit").Text, Multiplayer.GetUniqueId());
	}

	private void ConnectionFailed()
	{
		GD.Print("connection failed");

	}

	private void PlayerConnected(long id)
	{
		GD.Print("Peer connected with ID: " + id);

	}

	private void PlayerDisconnected(long id)
	{
		GD.Print("Peer disconnected with ID: " + id);
		MultiplayerManagment.Players.Remove(MultiplayerManagment.Players.Where(i => i.Id == id).First<PlayerInfo>());
		var playerr = GetTree().GetNodesInGroup("Player");
		foreach (var i in playerr)
		{
			//GetNode<Label>("Label").Text = "*disconected*";
			GD.Print(i.Name);
			if (i.Name == id.ToString()) 
			{
				i.QueueFree();
			}
			
			
		}
		
	}
	
	
	private PackedScene player_scene = new PackedScene();

	private void add_player(int id = 1)
	{
		var player = player_scene.Instantiate();
		player.Name = Convert.ToString(id);
		CallDeferred("add_child", player);
	}
	
	private void _on_host_button_down()
	{
		peer = new ENetMultiplayerPeer();
		var error = peer.CreateServer(Port, 2);

		//add_player();
		if (error != Error.Ok)
		{
			GD.Print("cannot host" + $"{error}");
			return;
		}

		peer.Host.Compress(ENetConnection.CompressionMode.RangeCoder); //optional compression to get less lag


		Multiplayer.MultiplayerPeer = peer;


		GD.Print("Waiting for player");
		SendPlayerInformation( GetNode<LineEdit>("LineEdit").Text,1);
	}


	private void _on_join_button_down()
	{
		peer = new ENetMultiplayerPeer();
		peer.CreateClient(Adress, Port);
		peer.Host.Compress(ENetConnection.CompressionMode.RangeCoder); //optional compression to get less lag

		Multiplayer.MultiplayerPeer = peer;

		GD.Print("Joining game");
		//SendPlayerInformation("player 2",2);
	}


	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	public void StartGame()
	{
		foreach (var i in MultiplayerManagment.Players)
		{
			GD.Print(i.Name + " is playing");
		}
		var scene = ResourceLoader.Load<PackedScene>("res://world.tscn").Instantiate<Node2D>();
		GetTree().Root.AddChild(scene);
		this.Hide();
	}


	private void _on_start_button_down()
	{
		Rpc("StartGame");
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	private void SendPlayerInformation(string name, int id)
	{
		PlayerInfo playerInfo = new PlayerInfo()
		{
			Name = name,
			Id = id
		};
		if (!MultiplayerManagment.Players.Contains(playerInfo))
		{
			MultiplayerManagment.Players.Add(playerInfo);
		}

		if (Multiplayer.IsServer())
		{
			foreach (var i in MultiplayerManagment.Players) 
			{
				Rpc("SendPlayerInformation", i.Name, i.Id);
			}
		}
	}
}


