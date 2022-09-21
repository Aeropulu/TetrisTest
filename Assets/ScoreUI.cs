using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ScoreUI : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI _scoreNumberTextMesh;

	public void UpdateScore(int score)
	{
		if (_scoreNumberTextMesh == null)
		{
			Debug.LogError("No TextMesh on ScoreUI");
			return;
		}
		_scoreNumberTextMesh.text = score.ToString();
	}
}
