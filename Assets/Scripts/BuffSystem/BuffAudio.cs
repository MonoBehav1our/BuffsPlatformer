using UnityEngine;

public class BuffAudio : MonoBehaviour
{
    private AudioSource _audioSource;
    [SerializeField] private AudioClip voidSound;

    private void Start() => _audioSource = GetComponent<AudioSource>();

    private void OnEnable()
    {
        BuffManager.BuffUsed += PlayAudio;
        BuffManager.VoidBuffUsed += VoidBuffAudio;
    }
    private void OnDisable()
    {
        BuffManager.BuffUsed -= PlayAudio;
        BuffManager.VoidBuffUsed -= VoidBuffAudio;
    }

    private void PlayAudio(IAction buff)
    {
        _audioSource.clip = buff.GetConfig().Sound;
        _audioSource.Play();
    }

    private void VoidBuffAudio()
    {
        _audioSource.clip = voidSound;
        _audioSource.Play();
    }
}
