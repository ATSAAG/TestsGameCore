using Godot;

namespace TestMovements;

public class CheckPointManager
{
    public static check_point CheckPoint;
  
    public static player _player1;
    public static player _player2;

   
    public static void respawn()
    {
        if (CheckPoint != null)
        { _player1.Position = CheckPoint.Position;
            _player2.Position = CheckPoint.Position.MoveToward(CheckPoint.Position, 10);
            
            
            
        }
    }
}