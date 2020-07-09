using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolManager : MonoBehaviour
{
    [SerializeField]
    private Symbol[] symbolItems = new Symbol[25];
    [SerializeField]
    private Symbol nextSymbol;
    [SerializeField]
    private Symbol[] InventorySymbols;
    [SerializeField]
    private Sprite[] symbolSprites;
    [SerializeField]
    private Color[] symbolColors;

    public List<MatchableGridInfo> matchList = new List<MatchableGridInfo>();

    private void Reset()
    {
        symbolItems = new Symbol[25];

        for (int i = 0; i < symbolItems.Length; i++)
        {
            string _name = "Symbol_" + (i + 1).ToString();
            symbolItems[i] = GameObject.Find(_name).GetComponent<Symbol>();
        }

        nextSymbol = GameObject.Find("NextSymbol").GetComponent<Symbol>();

        InventorySymbols = new Symbol[3];

        for (int i = 1; i < InventorySymbols.Length + 1; i++)
        {
            string _name = "InventorySymbol_" + i.ToString();
            InventorySymbols[i - 1] = GameObject.Find(_name).GetComponent<Symbol>();
        }

        symbolSprites = new Sprite[7];

        for (int i = 1; i < symbolSprites.Length; i++)
        {
            string _name = "candy1_" + ((SymbolColor)i).ToString().ToLower();
            symbolSprites[i] = Resources.Load<Sprite>("Symbols/" + _name);
        }

        symbolColors = new Color[7];

        symbolColors[0] = new Color(0.8235295f, 0.7921569f, 0.6196079f);
        symbolColors[1] = Color.yellow;
        symbolColors[2] = Color.red;
        symbolColors[3] = Color.green;
        symbolColors[4] = new Color(1, 0.5590622f, 0);
        symbolColors[5] = new Color(1, 0, 1);
        symbolColors[6] = Color.black;
    }

    public Color GetSymbolColor(SymbolColor color)
    {
        return symbolColors[(int)color];
    }

    public Sprite GetSymbolSprite(SymbolType type)
    {
        return symbolSprites[(int)type];
    }

    public void InitGridMap()
    {
        for(int y = 0; y < 5; y++)
        {
            for(int x = 0; x < 5; x++)
            {
                int index = (y * 5) + x; 
                symbolItems[index].SetItem(x, y, SymbolColor.None, SymbolType.Blank);
            }
        }

        SetNextSymbol();
        InventorySymbolInit();
    }

    public void SetGridItem(int x, int y, SymbolColor color, SymbolType type)
    {
        symbolItems[x * 5 + y].SetItem(x, y, color, type);
        symbolItems[x * 5 + y].PlayGradeAnimation();
    }

    public void InventorySymbolInit()
    {
        for(int i = 0; i < 3; i++)
        {
            SymbolColor color = (SymbolColor)Random.Range(1, 6);
            SymbolType type = CalculateRandomNextSymbol();

            InventorySymbols[i].SetItem(0, 0, color, type);
        }
    }

    public void SetNextSymbol()
    {
        SymbolColor color = (SymbolColor)Random.Range(1, 6);
        SymbolType type = CalculateRandomNextSymbol();

        nextSymbol.SetItem(0, 0, color, type);
    }

    private SymbolType CalculateRandomNextSymbol()
    {
        int value = Random.Range(0, 100);

        if(value < 60)
        {
            return SymbolType.NormalSeed;
        }
        else if (value >= 60 && value < 80)
        {
            return SymbolType.NormalBud;
        }
        else if (value >= 80 && value < 87)
        {
            return SymbolType.NormalFlower;
        }
        else if (value >= 87 && value < 94)
        {
            return SymbolType.NormalBlossom;
        }
        else if (value >= 94 && value < 97)
        {
            return SymbolType.JewelrySeed;
        }
        else if (value >= 97 && value < 99)
        {
            return SymbolType.JewelryBud;
        }
        else
        {
            return SymbolType.JewelryFlower;
        }
    }

    public bool IsCombine()
    {
        matchList.Clear();

        getHorizontalMatch();
        getVerticalMatch();

        return matchList.Count > 0;
    }

    private void getHorizontalMatch()
    {
        for (int y = 0; y < 4; y++)
        {
            int matchCnt = 0;

            MatchableGridInfo matchInfo = new MatchableGridInfo();
            
            for (int x = 0; x < 4; x++)
            {
                int index = y * 5 + x;
                int nextIndex = index + 1;
                if (symbolItems[index].type == symbolItems[nextIndex].type &&
                    symbolItems[index].color == symbolItems[nextIndex].color && symbolItems[index].color != SymbolColor.None)
                {
                    if(symbolItems[index].color != SymbolColor.None)
                    {
                        matchInfo.start_x = x;
                        matchInfo.start_y = y;
                        matchInfo.type = symbolItems[index].type;
                        matchInfo.color = symbolItems[index].color;
                    }

                    if(symbolItems[index].color == SymbolColor.Rainbow)
                    {
                        matchInfo.color = symbolItems[nextIndex].color;
                    }

                    matchCnt++;
                }
                else
                {
                    if (matchCnt < 3)
                    {
                        matchInfo = null;
                    }
                    else
                    {
                        matchInfo.end_x = x;
                        matchInfo.end_y = y;

                        matchList.Add(matchInfo);
                    }

                    matchCnt = 0;
                }
            }
        }
    }

    private void getVerticalMatch()
    {
        for (int x = 0; x < 4; x++)
        {
            int matchCnt = 0;

            MatchableGridInfo matchInfo = new MatchableGridInfo();

            for (int y = 0; y < 4; y++)
            {
                int index = y * 5 + x;
                int nextIndex = (y + 1) * 5 + x;

                if (symbolItems[index].type == symbolItems[nextIndex].type &&
                    symbolItems[index].color == symbolItems[nextIndex].color && symbolItems[index].color != SymbolColor.None)
                {
                    if (symbolItems[index].color != SymbolColor.None)
                    {
                        matchInfo.start_x = x;
                        matchInfo.start_y = y;
                        matchInfo.type = symbolItems[index].type;
                        matchInfo.color = symbolItems[index].color;
                    }

                    if (symbolItems[index].color == SymbolColor.Rainbow)
                    {
                        matchInfo.color = symbolItems[nextIndex].color;
                    }

                    matchCnt++;
                }
                else
                {
                    if (matchCnt < 3)
                    {
                        matchInfo = null;
                    }
                    else
                    {
                        matchInfo.end_x = x;
                        matchInfo.end_y = y;

                        matchList.Add(matchInfo);
                    }

                    matchCnt = 0;
                }
            }
        }
    }
}
