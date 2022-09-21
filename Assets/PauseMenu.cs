using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;

public class PauseMenu : MonoBehaviour
{
	[SerializeField] private MovePiece _movePiece;
	[SerializeField] private TetrisField _field;
	[SerializeField] private PauseManager _pauseManager;
	[SerializeField] private Leaderboard _leaderboard;
	[SerializeField] private ScoreManager _scoreManager;
	[SerializeField] private TMPro.TextMeshProUGUI _nickNameText;
	[SerializeField] private Selectable _gameOverSelectable;
	[SerializeField] private List<Button> _gameNotOverButtons = new List<Button>();
	

	private void Start()
	{
		if (_movePiece == null)
		{
			Debug.LogError("No movepiece on PauseMenu");
		}

		if (_field == null)
		{
			Debug.LogError("No TetrisField on PauseMenu");
		}

		if (_pauseManager == null)
		{
			Debug.LogError("No pause manager on PauseMenu");
		}

		if (_nickNameText == null)
		{
			Debug.LogError("No nickname test on PauseMenu");
		}
	}

	private void OnEnable()
	{
		if (_movePiece.IsGameOver)
		{
			SetGameNotOverButtonsVisible(false);

			if (_gameOverSelectable != null)
			{
				_gameOverSelectable.Select();
			}
		}
		else 
		{
			SetGameNotOverButtonsVisible(true);
		}
	}


	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			ResumeGame();
		}
	}

	public void ResumeGame()
	{
		if (_movePiece.IsGameOver)
		{
			return;
		}

		_pauseManager.SetPause(false);
	}

	public void NewGame()
	{
		_field.Clear();
		_movePiece.NewGame();

		_pauseManager.SetPause(false);
	}

	public void EndGame()
	{
		_pauseManager.SetPause(false);
		_movePiece.EndGame();
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	public void ShowLeaderBoard()
	{
		SetLeaderboardVisible(true);
	}

	public void ChangeLanguage(int index)
	{
		LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
	}

	private void SetGameNotOverButtonsVisible(bool visible)
	{
		foreach (Button button in _gameNotOverButtons)
		{
			button.gameObject.SetActive(visible);
		}
	}

	private void SetLeaderboardVisible(bool visible)
	{
		if (_leaderboard == null)
		{
			Debug.LogError("No Leaderboard set on PauseMenu");
			return;
		}
		gameObject.SetActive(!visible);
		_leaderboard.gameObject.SetActive(visible);
	}

	private void TryNewHighScore()
	{
		if (_leaderboard == null)
		{
			Debug.LogError("No Leaderboard set on PauseMenu");
			return;
		}
		
		int score = _scoreManager.Score;
		_leaderboard.EvaluateNewHighScore(_nickNameText.text, score);
	}
}
