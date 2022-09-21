using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Leaderboard : MonoBehaviour
{
	[SerializeField] private int _numEntries = 10;
	[SerializeField] private GameObject _entryPrefab;
	[SerializeField] private UnityEvent _hideLeaderboardEvent = new UnityEvent();
	private string[] _nicknames;
	private int[] _scores;
	private LeaderboardEntryUI[] _entriesUI;

	private const string NICK_KEY_PREFIX = "nick";
	private const string SCORE_KEY_PREFIX = "score";

	
	void Start()
	{
		gameObject.SetActive(false);
		_nicknames = new string[_numEntries];
		_scores = new int[_numEntries];
		LoadScores();
		InstantiateEntries();
		UpdateUI();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			HideLeaderboard();
		}
	}

	private void HideLeaderboard()
	{
		gameObject.SetActive(false);
		_hideLeaderboardEvent.Invoke();
	}

	public void EvaluateNewHighScore(string nickName, int score)
	{
		if (_scores == null)
		{
			return;
		}

		for (int i = _numEntries - 1; i > 0; i--)
		{
			int storedScore = _scores[i];
			if (storedScore > score)
			{
				InsertHighScore(i + 1, nickName, score);
				SaveScores();
				UpdateUI();
				return;
			}
		}
	}

	private void UpdateUI()
	{
		for (int i = 0; i < _numEntries; i ++)
		{
			_entriesUI[i].SetValues(_nicknames[i], _scores[i]);
		}
	}

	private void InsertHighScore(int index, string nickName, int score)
	{
		if (index >= _numEntries)
		{
			return;
		}

		for (int i = _numEntries - 1; i > index; i--)
		{
			_nicknames[i] = _nicknames[i - 1];
			_scores[i] = _scores[i - 1];
		}
		_nicknames[index] = nickName;
		_scores[index] = score;
	}

	private void LoadScores()
	{
		if (PlayerPrefs.HasKey($"{SCORE_KEY_PREFIX}{0}") == false)
		{
			InitializeDefaultLeaderBoard();
			SaveScores();
		}
		else
		{
			for (int i = 0; i < _numEntries; i++)
			{
				string nickKey = $"{NICK_KEY_PREFIX}{i}";
				string scoreKey = $"{SCORE_KEY_PREFIX}{i}";

				_nicknames[i] = PlayerPrefs.GetString(nickKey);
				_scores[i] = PlayerPrefs.GetInt(scoreKey);
			}
		}
	}

	private void SaveScores()
	{
		for (int i = 0; i < _numEntries; i++)
		{
			string nickKey = $"{NICK_KEY_PREFIX}{i}";
			string scoreKey = $"{SCORE_KEY_PREFIX}{i}";

			PlayerPrefs.SetString(nickKey, _nicknames[i]);
			PlayerPrefs.SetInt(scoreKey, _scores[i]);
		}
	}

	private void InitializeDefaultLeaderBoard()
	{
		int score = 1000;
		int scoreIncrement = 1000;
		string defaultName = "Pulu";

		for (int i = _numEntries - 1; i >= 0; i--)
		{
			_nicknames[i] = defaultName;
			_scores[i] = score;

			score += scoreIncrement;
		}
	}

	private void InstantiateEntries()
	{
		if (_entryPrefab == null)
		{
			return;
		}

		_entriesUI = new LeaderboardEntryUI[_numEntries];
		for (int i = 0; i < _numEntries; i++)
		{
			GameObject entryObject = Instantiate(_entryPrefab, transform);
			_entriesUI[i] = entryObject.GetComponent<LeaderboardEntryUI>();
		}
	}
}
