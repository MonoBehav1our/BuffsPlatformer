using UnityEngine;

[CreateAssetMenu(fileName = "Buff")]
public class BuffInfo : ScriptableObject
{
    public BuffType Type { get { return _type;} private set { } }
    public Sprite Sprite { get { return _sprite; } private set { } }

    [SerializeField] private BuffType _type;
    [SerializeField] private Sprite _sprite;
}
