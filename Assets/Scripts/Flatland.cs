using UnityEngine;

public class Flatland : Platform
{
    private static readonly GameObject GAME_OBJECT = (GameObject)Resources.Load("Prefabs/Flatland");

    public Flatland()
        : base(new Vector2(0, 0), 1, GAME_OBJECT) { }
}
