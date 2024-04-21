using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using YG;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    [SerializeField] private float _animTime;
    [SerializeField] private TextMeshProUGUI _levelPassedText;
    [SerializeField] private TextMeshProUGUI _gameoverText;
    [SerializeField] private Color _baseTextColor;

    [Space]

    [SerializeField] private GameObject _mobileContinue;
    [SerializeField] private GameObject _desktopContinue;
    [SerializeField] private float _timeToAnimEnd;

    private Image image;
    private AsyncOperation load;
    private bool _loading;

    private void Start()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);

        image = GetComponentInChildren<Image>();
        StartCoroutine(EnterAnim());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) Restart();
        if (Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift))
        {
            YandexGame.ResetSaveProgress();
            YandexGame.SaveProgress();
        }
    }

    public void LoadLastOpened() => StartCoroutine(SimpleExitAnim(YandexGame.savesData.openedLevels));

    public void Restart() => StartCoroutine(SimpleExitAnim(SceneManager.GetActiveScene().buildIndex));

    public void LoadByNumber(int number) => StartCoroutine(SimpleExitAnim(number));

    public void DieRestart() => StartCoroutine(ExitAnimWithWaiting(SceneManager.GetActiveScene().buildIndex, WaitingAnimType.Gameover));

    public void LoadNext() => StartCoroutine(ExitAnimWithWaiting(SceneManager.GetActiveScene().buildIndex + 1, WaitingAnimType.LevelPassed));

    private IEnumerator EnterAnim()
    {
        image.color = Color.black;
        float time = _animTime;

        while (time >= 0)
        {
            time -= Time.deltaTime;
            image.color = new Color(0, 0, 0, time / _animTime);
            yield return null;
        }

        image.color = Color.clear;
        yield return null;

        if (BuffManager.Instance != null) BuffManager.Instance.DisableBlock();
    }

    private IEnumerator SimpleExitAnim(int sceneNumber)
    {
        if (sceneNumber <= YandexGame.savesData.openedLevels && !_loading)
        {
            if (PauseHandler.Instance != null) PauseHandler.Instance.DisableUI();

            _loading = true;
            Time.timeScale = 1;
            load = SceneManager.LoadSceneAsync(sceneNumber);
            load.allowSceneActivation = false;

            image.color = Color.clear;
            float time = 0;
            while (time <= _animTime)
            {
                time += Time.deltaTime;
                image.color = new Color(0, 0, 0, time / _animTime);
                yield return null;
            }

            image.color = Color.black;
            load.allowSceneActivation = true;   
        }
    }

    private IEnumerator ExitAnimWithWaiting(int sceneNumber, WaitingAnimType state)
    {
        TextMeshProUGUI text = null;
        switch (state)
        {
            case WaitingAnimType.Gameover:
                text = _gameoverText;
                break;
            case WaitingAnimType.LevelPassed:
                text = _levelPassedText;
                break;
        }

        if (!_loading)
        {
            PauseHandler.Instance.DisableUI();

            _loading = true;
            Time.timeScale = 1;
            load = SceneManager.LoadSceneAsync(sceneNumber); 
            load.allowSceneActivation = false;

            image.color = Color.clear;
            text.color = Color.clear;
            text.gameObject.SetActive(true);
            float time = 0;

            while (time <= _animTime)
            {
                time += Time.deltaTime;
                image.color = new Color(0, 0, 0, time / _animTime);
                text.color = new Color(_baseTextColor.r, _baseTextColor.g, _baseTextColor.b, time / _animTime);
                yield return null;
            }
            image.color = Color.black;
            text.color = new Color(_baseTextColor.r, _baseTextColor.g, _baseTextColor.b, 1);

            yield return new WaitForSeconds(_timeToAnimEnd);

            if (DeviceInfo.IsMobile()) _mobileContinue.SetActive(true);
            else _desktopContinue.SetActive(true);

            while (true)
            {
                yield return null; 
                if (Input.anyKeyDown) load.allowSceneActivation = true;
            }
        }
    }

    private enum WaitingAnimType
    {
        Gameover,
        LevelPassed
    }
}
