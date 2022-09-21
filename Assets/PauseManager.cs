using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private List<MonoBehaviour> _pausables = new List<MonoBehaviour>();
    [SerializeField] private UnityEvent<bool> _pausedEvent = new UnityEvent<bool>();
    private bool _isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

	public void SetPause(bool paused = true)
	{
        _isPaused = paused;
        ApplyPause();
    }

    private void TogglePause()
	{
        _isPaused = !_isPaused;
        ApplyPause();
    }

	private void ApplyPause()
	{
        foreach (MonoBehaviour behaviour in _pausables)
        {
            behaviour.enabled = !_isPaused;
        }
        _pausedEvent.Invoke(_isPaused);
    }
}
