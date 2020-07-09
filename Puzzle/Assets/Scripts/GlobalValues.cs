using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    None = 0,
    Normal,
    End,
}

public enum SymbolColor
{
    None = 0,
    Yellow,
    Red,
    Green,
    Orange,
    Violet,
    Rainbow,
}

public enum SymbolType
{
    Blank = 0,
    NormalSeed,
    NormalBud,
    NormalFlower,
    NormalBlossom,
    JewelrySeed,
    JewelryBud,
    JewelryFlower,
    JewelryBlossom,
    Star,
    FullMoon,
}

public class MatchableGridInfo
{
    public int start_x;
    public int start_y;
    public SymbolColor color;
    public SymbolType type;

    public int end_x;
    public int end_y;
}

public static class GlobalValues
{
    
}
