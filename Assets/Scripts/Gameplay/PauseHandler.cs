using UnityEngine;

public class PauseHandler : MonoBehaviour
{
    public static PauseHandler Instance;

    [SerializeField] private GameObject _pauseButton;
    [SerializeField] private GameObject _pausePanel;

    public bool IsPausing { get { return _isPausing; } }
    private bool _isPausing;

    private void Start()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);

        _isPausing = false;
        Time.timeScale = 1f;  
    }

    public void ChangePauseState()
    {
        _isPausing = !_isPausing;

        if (_isPausing) Time.timeScale = 0f;
        else Time.timeScale = 1f;

        _pauseButton.SetActive(!_isPausing);
        _pausePanel.SetActive(_isPausing);
    }

    public void DisableUI()
    {
        _pauseButton.SetActive(false);
    }
}
