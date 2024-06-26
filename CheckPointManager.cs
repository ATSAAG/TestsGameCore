﻿using Godot;

using TestMovements;

public class CheckPointManager
{
    public static check_point CheckPoint;
  
    public static player _player1;
    public static player _player2;

    [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
    public static void respawn()
    {
        if (CheckPoint != null)
        {
            
            //MultiplayerManagment.multiplayerManagment.LoadLevel("res://world.tscn");

            _player1.GlobalPosition = CheckPoint.GlobalPosition;
            _player2.GlobalPosition = CheckPoint.GlobalPosition.MoveToward(CheckPoint.GlobalPosition, 10);
            _player1.Health = 10;
            _player2.Health = 10;

        }
    }
}