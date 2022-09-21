using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private List<MonoBehaviour> _pausables = new List<MonoBehaviour>();
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
            _isPaused = !_isPaused;
            foreach (MonoBehaviour behaviour in _pausables)
            {
                behaviour.enabled = !_isPaused;
            }

            Debug.Log($"paused: {_isPaused}");
        }
    }
}
