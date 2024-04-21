using System.Collections;
using UnityEngine;

public class MainMenuText : MonoBehaviour
{
    [SerializeField] private float _minSize;
    [SerializeField] private float _maxSize;
    [SerializeField] private float _animTime;

    private void OnEnable()
    {
        StartCoroutine(Animation());
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator Animation()
    {
        while (true) 
        {
            float time = 0;
            while (time < _animTime)
            {
                transform.localScale = Vector3.Lerp(new Vector2(_minSize, _minSize), new Vector2(_maxSize, _maxSize), time / _animTime);
                time += Time.deltaTime;
                yield return null;
            }
            transform.localScale = new Vector2(_maxSize, _maxSize);

            time = 0;
            while (time < _animTime)
            {
                transform.localScale = Vector3.Lerp(new Vector2(_maxSize, _maxSize), new Vector2(_minSize, _minSize), time / _animTime);
                time += Time.deltaTime;
                yield return null;
            }
            transform.localScale = new Vector2(_minSize, _minSize);
        }
    }
}

