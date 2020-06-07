using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameView : MonoBehaviour
{
    public PlayerView Player;
    public SpawnView Spawn;

    public GameView(PlayerView player, SpawnView spawn)
    {
        Player = player;
        Spawn = spawn;
    }
}
