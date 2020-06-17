﻿using System.Text;
using UnityEngine;

public class LevelScene: BaseScene 
{
	[SerializeField]
	GameView gameView;

	[SerializeField]
	RuleView ruleView;

	protected override void Awake()
	{
		base.Awake();

		StringBuilder sb = new StringBuilder();
		sb.AppendFormat(Literals.level_0, ReadLevelIndex());

		TextAsset levelData = Resources.Load(sb.ToString()) as TextAsset;
		var levelModel = JsonUtility.FromJson<LevelModel>(levelData.text);

		gameView.PassTheLevelModel(levelModel);
		ruleView.PassTheLevelModel(levelModel);

		gameView.OnGemRemoved.AddListener(ruleView.OnGemRemoved);
		gameView.OnPhaseNext.AddListener(ruleView.OnPhaseNext);
		ruleView.OnAllMissionAchieved.AddListener(gameView.OnAllMissionAchieved);
	}

	void OnDestroy()
	{
		gameView.OnGemRemoved.RemoveAllListeners();
		gameView.OnPhaseNext.RemoveAllListeners();
		ruleView.OnAllMissionAchieved.RemoveAllListeners();
	}

	public void LoadLobbyScene()
	{
		if (gameView.IsPlaying) {
			Toast.Show("A coroutine is still running.", .8f);
			return;
		}
		
		var sceneLoader = GameObject.Find(Literals.SceneLoader).GetComponent<SceneLoader>();
		sceneLoader.Load(Literals.LobbyScene);
		MatchSound.Instance.Play("Back");
	}

	protected virtual int ReadLevelIndex()
	{
		return PlayerPrefs.GetInt(Literals.LatestLevel);
	}
}
