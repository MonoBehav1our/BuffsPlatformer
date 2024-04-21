using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(AudioSource))]
public class BuffAdder : MonoBehaviour
{
    [SerializeField] private BuffConfig _buff;
    [SerializeField] private float _animTime;
    [Range(1, 2)][SerializeField] private float _scaleDiff;

    private AudioSource _audioSource;
    private Vector2 _defaultScale;
    private Vector2 _boostedScale;
    private bool _used;
 
    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = _buff.Sprite;
        _audioSource = GetComponent<AudioSource>();

        _defaultScale = transform.localScale;
        _boostedScale = transform.localScale * _scaleDiff;

        StartCoroutine(IdleAnimation());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>() && !_used)
        {
            _used = true;

            BuffFactory.Instance.AddBuff(_buff);
            StopAllCoroutines();
            StartCoroutine(Die(collision.transform));
        }
    }

    private IEnumerator IdleAnimation()
    {
        while (true) 
        {
            float time = 0;
            while (time < _animTime)
            {
                transform.localScale = Vector3.Slerp(_defaultScale, _boostedScale, time/_animTime);
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

    private IEnumerator Die(Transform player)
    {
        _audioSource.Play();
        float time = 0;
        Vector2 scale = transform.localScale;
        Vector2 startPos = transform.position;

        while (time < _animTime)
        {
            transform.localScale = Vector3.Lerp(scale, Vector2.zero, time / _animTime);
            transform.position = Vector3.Lerp(startPos, player.position, time / _animTime);
            time += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
