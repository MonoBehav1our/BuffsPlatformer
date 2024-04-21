using YG;
using TMPro;
using UnityEngine;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private int _levelNumber;
    private TextMeshProUGUI _textMeshPro;

    void Start()
    {   
        _textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
        CheckUnlocked();    
    }

    private void CheckUnlocked()
    {
        if (_levelNumber <= YandexGame.savesData.openedLevels) _textMeshPro.text = _levelNumber.ToString();
        else _textMeshPro.text = "";
    }

    public void LoadLevel()
    {
        if (_levelNumber <= YandexGame.savesData.openedLevels) SceneLoader.Instance.LoadByNumber(_levelNumber);
    }
}
