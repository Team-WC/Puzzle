using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public GameObject TilePrefab;
    public GameObject layout;
    public List<Transform> layoutParents;
    
    public int x_size = 9;
    public int y_size = 7;

    public void GenerateTileMap()
    {
        if(layoutParents != null)
        {
            ClearMap();
        }
        else
        {
            layoutParents = new List<Transform>();
        }

        for (int i = 0; i < y_size; i++)
        {
            GameObject g = Instantiate(layout, this.transform);
            RectTransform rt = g.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(0, -50 * i);

            layoutParents.Add(g.transform);
        }

        for(int y = 0; y < y_size; y++)
        {
            int x_Length = x_size - Mathf.Abs(y - y_size/2);

            for (int x = 0; x < x_Length; x++)
            {
                GameObject g = Instantiate(TilePrefab, layoutParents[y]);
            }
        }
    }

    public void ClearMap()
    {
        for(int i = 0; i < layoutParents.Count; i++)
        {
            Destroy(layoutParents[i]);
        }

        layoutParents.Clear();
    }
}
