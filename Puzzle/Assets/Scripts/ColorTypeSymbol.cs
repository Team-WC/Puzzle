using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ColorTypeSymbol : MonoBehaviour
{
    [SerializeField]
    private SymbolType type;
    [SerializeField]
    private GameObject[] symbolImages;

    public void ShowSymbol(SymbolColor color)
    {
        int index = (int)color;

        for (int i = 0; i < symbolImages.Length; i++)
        {
            if(i == index)
            {
                symbolImages[i].SetActive(true);
            }
            else
            {
                symbolImages[i].SetActive(false);
            }
        }
    }

    public void HideSymbol()
    {
        for (int i = 0; i < symbolImages.Length; i++)
        {
            symbolImages[i].SetActive(false);
        }
    }
}
