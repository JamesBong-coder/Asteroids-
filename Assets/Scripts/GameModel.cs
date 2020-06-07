using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            enemy.Move(Player.GetPos());
        }
    }



}
