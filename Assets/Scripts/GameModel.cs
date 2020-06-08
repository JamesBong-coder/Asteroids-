using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;


public interface ICollision
{
    bool Collision(ICollision obj);
    RectangleF Rect { get; }
}





public class GameModel
{
    public PlayerModel Player;
    public SpawnModel Spawn;

    public GameModel(PlayerModel player, SpawnModel spawn)
    {
        Player = player;
        Spawn = spawn;
    }

    public void MoveEnemy()
    {
        foreach (EnemyModel enemy in Spawn.enemies)
        {
            enemy.Move(Player.Pos);
        }
    }
}

public abstract class MoveClass : ICollision
{
    public float Speed;
    public float angle;
    public PointF Pos;
    public SizeF size;

    public MoveClass(float speed, PointF pos, float Angle)
    {
        Speed = speed;
        Pos = pos;
        angle = Angle;
    }
    public MoveClass(float speed)
    {
        Speed = speed;
        Pos = new PointF(0, 0);
        angle = 0;
    }

    public float GetRadians(float angle)
    {
        return (angle * (float)Math.PI) / 180;
    }

    public void Move(double accel)
    {
        Pos.X += (float)(Math.Cos(GetRadians(angle)) * Speed * accel);
        Pos.Y += (float)(Math.Sin(GetRadians(angle)) * Speed * accel);
    }

    //For Collision
    public bool Collision(ICollision o)
    {
        if (o.Rect.IntersectsWith(this.Rect))
            return true;
        else return false;
    }
    
    public RectangleF Rect
    {
        get { return new RectangleF(Pos, size); }
    }
}


