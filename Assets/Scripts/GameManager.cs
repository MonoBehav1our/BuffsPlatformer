using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Start()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    public void LevelComplete()
    {

    }

    public void RestartLevel()
    {

    }
}
