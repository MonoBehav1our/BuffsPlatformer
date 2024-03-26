using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour
{
    [SerializeField] private Color[] colors;
    [SerializeField] private float _delay;

    [Space]

    [SerializeField] private bool _isEnable;
    private SpriteRenderer spriteRenderer;
    

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (_isEnable)
        {
            StartCoroutine(EnableAnim());
        }
        else
        {
            StopCoroutine(EnableAnim());
            spriteRenderer.color = Color.clear;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>() && _isEnable)
        {
            
        }
    }

    public void ChangeState()
    {
        _isEnable = !_isEnable;

        if (_isEnable)
        {
            StartCoroutine(EnableAnim());
        }
        else
        {
            StopAllCoroutines();
            spriteRenderer.color = Color.clear;
        }
    }

    private IEnumerator EnableAnim()
    {
        while (true) 
        {
            foreach (Color color in colors) 
            {
                spriteRenderer.color = color;
                yield return new WaitForSeconds(_delay);
            }
        }
    }
}
