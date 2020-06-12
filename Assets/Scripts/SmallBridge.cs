using UnityEngine;

public class SmallBridge : Platform
{
    private static readonly GameObject GAME_OBJECT = (GameObject)Resources.Load("Prefabs/Small Bridge");

    public SmallBridge()
        : base(new Vector2(0, 0), 1, GAME_OBJECT) { }
}
