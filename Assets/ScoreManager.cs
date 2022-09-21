using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour
{
	[SerializeField] private int _oneLineScore = 40;
	[SerializeField] private int _twoLineScore = 100;
	[SerializeField] private int _threeLineScore = 300;
	[SerializeField] private int _fourLineScore = 1200;
	[SerializeField] private UnityEvent<int> _scoreUpdatedEvent = new UnityEvent<int>();
	private int _score = 0;

	public int Score { get { return _score; }}
	
	public void ScoreLines(int linesCleared)
	{
		switch (linesCleared)
		{
			case 0:
				break;

			case 1:
				_score += _oneLineScore;
				break;

			case 2:
				_score += _twoLineScore;
				break;

			case 3:
				_score += _threeLineScore;
				break;

			case 4:
				_score += _fourLineScore;
				break;

			default:
				break;
		}

		_scoreUpdatedEvent.Invoke(_score);
	}

	public void ResetScore()
	{
		_score = 0;
		_scoreUpdatedEvent.Invoke(_score);
	}
}
