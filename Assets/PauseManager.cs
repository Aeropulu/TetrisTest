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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

	public void Pause(bool paused = true)
	{
        _isPaused = true;
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
