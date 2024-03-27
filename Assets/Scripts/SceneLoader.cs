using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    [SerializeField] private float _animTime;
    private Image image;

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
    }

    public void LoadNext() => StartCoroutine(ExitAnim(SceneManager.GetActiveScene().buildIndex + 1));

    public void Restart() => StartCoroutine(ExitAnim(SceneManager.GetActiveScene().buildIndex));

    public void LoadByNumber(int number) => StartCoroutine(ExitAnim(number));

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
    }

    private IEnumerator ExitAnim(int sceneNumber)
    {
        AsyncOperation load = SceneManager.LoadSceneAsync(sceneNumber);
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
        //SceneManager.LoadScene(sceneNumber);
        load.allowSceneActivation = true;
    }
}
