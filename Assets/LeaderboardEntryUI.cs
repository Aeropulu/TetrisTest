using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderboardEntryUI : MonoBehaviour
{
	[SerializeField] private Color _normalColor = Color.white;
	[SerializeField] private Color _highLightColor = Color.magenta;
	[SerializeField] private TextMeshProUGUI _nickNameText;
	[SerializeField] private TextMeshProUGUI _scoreText;
    public void SetValues(string nickName, int score, bool highlighted = false)
	{
		_nickNameText.text = nickName;
		_nickNameText.color = highlighted ? _highLightColor : _normalColor;
		_scoreText.text = score.ToString();
		_scoreText.color = highlighted ? _highLightColor : _normalColor;
	}

	
}
