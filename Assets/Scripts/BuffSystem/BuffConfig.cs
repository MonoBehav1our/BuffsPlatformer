using UnityEngine;

public class BuffConfig : ScriptableObject
{
    public BuffType Type { get { return _type; } private set { } }
    public Sprite Sprite { get { return _sprite; } private set { } }
    public AudioClip Sound { get { return _sound; } private set { } }

    [SerializeField] private BuffType _type;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private AudioClip _sound;
}
