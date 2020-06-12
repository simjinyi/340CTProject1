using UnityEngine;

public class HighwayBridge : Platform
{
    private static readonly GameObject GAME_OBJECT = (GameObject)Resources.Load("Prefabs/HighwayBridge");

    public HighwayBridge()
        : base(new Vector2(-9.4f, -0.35f), 2, GAME_OBJECT) { } 
}