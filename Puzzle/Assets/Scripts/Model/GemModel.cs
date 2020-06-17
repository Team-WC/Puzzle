﻿using System;
using System.Text;

[System.Serializable]
public enum GemType 
{
    Nil = -1, BlockedGem = 0, EmptyGem = 1, ChocoGem = 4, SuperGem = 5, SpawneeGem = 7, SpawnerGem = 8, 
    RedGem = 10,    RedGemC = 11,   RedGemH = 12,   RedGemV = 13,
    BlueGem = 20,   BlueGemC= 21,   BlueGemH= 22,   BlueGemV= 23,
    GreenGem = 30,  GreenGemC=31,   GreenGemH=32,   GreenGemV=33,
    PurpleGem = 40, PurpleGemC=41,  PurpleGemH=42,  PurpleGemV=43,
    OrangeGem = 50, OrangeGemC=51,  OrangeGemH=52,  OrangeGemV=53,
    YellowGem = 60, YellowGemC=61,  YellowGemH=62,  YellowGemV=63
}

[System.Serializable]
public class GemModel: BaseModel, IComparable
{
    static Int64 GEM_ID = 0;
    static Int64 SEQUENCE_ID = 0;
    public GemType Type 
    {
        set { 
            type = value; 
            name = type.ToString();
        }
        get { return type; }
    }
    [UnityEngine.SerializeField]
    GemType type;
    public string Name 
    { 
        get { 
            var sb = new StringBuilder();
            sb.Append(name);
            sb.Append(specialKey);
            return sb.ToString(); 
        }
    }
    string name;
    public Position Position 
    { 
        set { 
            positionBefore = (position == null) ? value : position;
            position = value;
            sequence = SEQUENCE_ID++;
        }
        get { return position; }
    }
    [UnityEngine.SerializeField]
    Position position;
    public PositionVector PositionVector 
    { 
        get {
            return new PositionVector {
                colOffset = position.col - positionBefore.col,
                rowOffset = position.row - positionBefore.row
            };
        }
    }
    [System.NonSerializedAttribute] public Position positionBefore;
    [System.NonSerializedAttribute] public Int64 id;
    [System.NonSerializedAttribute] public Int64 markedBy;
    [System.NonSerializedAttribute] public Int64 replacedBy;
    [System.NonSerializedAttribute] public Int64 sequence;
    [System.NonSerializedAttribute] public string specialKey;
    [System.NonSerializedAttribute] public int endurance;
    [System.NonSerializedAttribute] public /* gameModel.turn */Int64 preservedFromBreak;
    [System.NonSerializedAttribute] public /* gameModel.turn */Int64 preservedFromMatch;
    [System.NonSerializedAttribute] public /* gameModel.turn */Int64 preservedFromFall;

    public GemModel(GemType type, Position position) 
    {
        Type = type;
        Position = position;
        id = GEM_ID++;
    }

    public override string ToString() 
    {
        return string.Format("{0}: {1}, {2}, {3}", id, type, position.ToString(), preservedFromMatch);
    }

    public bool CanMatch(GemType matchingType) 
    {
        return type >= GemType.RedGem && type <= GemType.YellowGemV && type == matchingType;
    }

    public bool IsPrimitiveType()
    {
        return type >= GemType.RedGem && specialKey == "";
    }

    public bool IsSpecialType()
	{
		return (
			Type == GemType.SuperGem 
			|| Type == GemType.ChocoGem 
			|| specialKey == Literals.H
			|| specialKey == Literals.V
			|| specialKey == Literals.C
		);
	}

    public int CompareTo(object obj)
    {
        if (obj == null) return 1;

        GemModel comparing = obj as GemModel;
        return (id == comparing.id) ? 0 : 1;
    }
}

public class GemInfo {
    public Position position;
    public Int64 id;
    public bool endOfFall;
}

static class GemModelFactory 
{
    public static GemModel Get(GemType gemType, Position position) 
    {
        string className = gemType.ToString();
        var specialKey = Literals.Nil;

        int rawGemType = (int)gemType;
        if (rawGemType >= 10)
        {
            var baseGemType = (GemType)(rawGemType - (rawGemType % 10));
            specialKey = gemType.ToString().Replace(baseGemType.ToString(), Literals.Nil);
            gemType = baseGemType;
        }

        switch (gemType)
        {
            case GemType.SuperGem:
            case GemType.ChocoGem:
            case GemType.RedGem:
            case GemType.BlueGem:
            case GemType.GreenGem:
            case GemType.PurpleGem:
            case GemType.OrangeGem:
            case GemType.YellowGem:
            className = GemType.EmptyGem.ToString();
            break;

            case GemType.SpawneeGem:
            gemType = GemType.EmptyGem;
            break;
        }
        
        var sb = new StringBuilder();
        sb.Append(className);
        sb.Append(Literals.Model);

        var gemModel = (GemModel)Activator.CreateInstance(
            Type.GetType(sb.ToString()),
            gemType,
            position
        );
        gemModel.specialKey = specialKey;
        SetEndurance(gemModel);

        return gemModel;
    }

    static void SetEndurance(GemModel gemModel) 
    {
        int endurance = 0;
        switch(gemModel.Type)
        {
            case GemType.ChocoGem:
            endurance = 5;
            break;
        }
        
        switch(gemModel.specialKey) 
        {
            case Literals.H:
            case Literals.V:
            endurance = int.MaxValue;
            break;

            case Literals.C:
            endurance = 1;
            break;
        }
        
        gemModel.endurance = endurance;
    }
}

public interface IBlockable {}
public interface IMovable {}
public class BlockedGemModel: GemModel, IBlockable 
{
    public BlockedGemModel(GemType type, Position position): base(type, position) {}
}

public class SpawnerGemModel: GemModel, IBlockable 
{
    public int[] spawnTo = new int[]{0, -1};
    public SpawnerGemModel(GemType type, Position position): base(type, position) {}
}

public class SpawneeGemModel: GemModel 
{
    public SpawneeGemModel(GemType type, Position position): base(type, position) {}
}

public class EmptyGemModel: GemModel, IMovable 
{
    public EmptyGemModel(GemType type, Position position): base(type, position) {}
}