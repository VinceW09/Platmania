using System.Collections.Generic;
using UnityEngine;

public class Scenes
{
    public struct Scene
    {
        public Scene(string sceneName, int sceneIndex)
        {
            name = sceneName;
            index = sceneIndex;
        }

        public string name;
        public int index;
    }

    public enum SceneKey
    {
        MAINMENU,
        TOURNAMENTMENU,
        LOBBY,
        LEVEL_,
        TUTORIAL
    }

    public static readonly Dictionary<SceneKey, Scene> sceneList = new Dictionary<SceneKey, Scene>()
    {
        { SceneKey.MAINMENU, new Scene("MainMenu", 0) },
        { SceneKey.TOURNAMENTMENU, new Scene("TournamentMenu", 1) },
        { SceneKey.LOBBY, new Scene("Lobby", 2) },
        { SceneKey.LEVEL_, new Scene("Level_", -1) },
        { SceneKey.TUTORIAL, new Scene("Tutorial", 3) },
    };
}
