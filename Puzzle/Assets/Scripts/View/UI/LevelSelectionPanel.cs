﻿using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionPanel: MonoBehaviour 
{
	[SerializeField]
	ScrollContentCanSnap scrollContent;
	[SerializeField]
	Sprite[] levelTextures;
	const float GAP_OF_ITEM = 1000f;

	public void Setup() 
	{
		RectTransform[] levelItems = new RectTransform[levelTextures.Length];
		for (var i = 0; i < levelTextures.Length; i++)
		{
			var levelIndex = i + 1;
			var levelItem = ResourceCache.Instantiate(Literals.LevelItem, scrollContent.transform, false).GetComponent<RectTransform>();

			var sb = new StringBuilder();
			sb.AppendFormat(Literals.LevelItem0, levelIndex);
			levelItem.name = sb.ToString();
			levelItem.anchoredPosition = new Vector2(GAP_OF_ITEM * i, levelItem.anchoredPosition.y);
			levelItem.GetComponent<Image>().sprite = levelTextures[i];

			sb = new StringBuilder();
			sb.AppendFormat(Literals.LEVEL0, levelIndex);
			levelItem.GetComponent<LevelItem>().title.text = sb.ToString();

			levelItems[i] = levelItem;
		}

		int latestLevelIndex = -1;
		if (PlayerPrefs.HasKey(Literals.LatestLevel)) {
			latestLevelIndex = PlayerPrefs.GetInt(Literals.LatestLevel);
		}
		scrollContent.Setup(levelItems, latestLevelIndex);
	}
	
	public LevelItem GetLevelItem(int levelIndex)
	{
		var sb = new StringBuilder();
		sb.AppendFormat(Literals.LevelItem0, levelIndex);
		var levelItem = scrollContent.transform.Find(sb.ToString());
		
		return levelItem != null ? levelItem.GetComponent<LevelItem>() : null;
	}
}
