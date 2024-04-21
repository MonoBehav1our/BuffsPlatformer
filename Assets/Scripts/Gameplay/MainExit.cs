using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using YG;

public class MainExit : MonoBehaviour
{
    private AudioSource _audioSource;

    [SerializeField] private AudioClip _exitClip;
    [SerializeField] private float _enterAnimTime;

    [SerializeField] private float _animTime;
    [Range(1, 2)][SerializeField] private float _scaleDiff;

    private Vector2 _defaultScale;
    private Vector2 _boostedScale;

    private void Start()
    {
        _defaultScale = transform.localScale;
        _boostedScale = transform.localScale * _scaleDiff;

        StartCoroutine(IdleAnim());
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>())
        {
            StartCoroutine(ExitLevel(collision.gameObject));
        }
    }

    private IEnumerator ExitLevel(GameObject player)
    {
        _audioSource.clip = _exitClip;
        _audioSource.Play();

        float time = 0;
        Vector2 scale = player.transform.localScale;
        Vector2 pos = player.transform.position;

        while (time < _enterAnimTime)
        {
            player.transform.localScale = Vector3.Lerp(scale, Vector2.zero, time / _enterAnimTime);
            player.transform.position = Vector3.Lerp(pos, transform.position, time / _enterAnimTime);
            time += Time.deltaTime;
            yield return null; 
        }

        player.transform.localScale = Vector2.zero;
        player.transform.position = transform.position;

        if (SceneManager.GetActiveScene().buildIndex == YandexGame.savesData.openedLevels)
        {
            YandexGame.savesData.openedLevels += 1;
            YandexGame.SaveProgress();
        }

        SceneLoader.Instance.LoadNext();
    }

    private IEnumerator IdleAnim()
    {
        while (true)
        {
            float time = 0;
            while (time < _animTime)
            {
                transform.localScale = Vector3.Slerp(_defaultScale, _boostedScale, time / _animTime);
                time += Time.deltaTime;
                yield return null;
            }
            time = 0;

            while (time < _animTime)
            {
                transform.localScale = Vector3.Slerp(_boostedScale, _defaultScale, time / _animTime);
                time += Time.deltaTime;
                yield return null;
            }
        }
    }
}
