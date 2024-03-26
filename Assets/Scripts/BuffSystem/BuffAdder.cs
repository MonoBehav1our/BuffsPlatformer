using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BuffAdder : MonoBehaviour
{
    [SerializeField] private BuffInfo _buff;

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = _buff.Sprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>())
        {
            BuffManager.Instance.EnqueueBuff(_buff);
            Destroy(gameObject);
        }
    }
}
