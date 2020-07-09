using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    public static GameManager Instance
    {
        get { return instance; }

    }

    public SymbolManager symbolManager;

    public GameState state = GameState.None;

    //public GridGenerator gridGenerator;

    private void Awake()
    {
        instance = this;
    }

    private void OnDestroy()
    {
        instance = null;
    }

    private void Start()
    {
        SetGameStart();
        //gridGenerator.GenerateTileMap();
    }

    public void SetGameStart()
    {
        GC.Collect();
        state = GameState.Normal;
        symbolManager.InitGridMap();
    }
}
