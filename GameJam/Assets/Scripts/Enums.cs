using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;

namespace Assets.Scripts
{
    public enum GameState
    {
        Startup,
        MainMenu,
        StartGame,
        PlayingGame,
        DiedBullet,
        DiedAlcohol,
        DiedZombie,
        GotHighscore,
        ShowHighscores
    }
}
