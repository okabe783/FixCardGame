using UnityEngine;

public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
    [SerializeField] private AudioSource _audioSource;

    //Title画面のBGMを流す
    public void OnPlayBGM(AudioClip bgmClip)
    {
        if (_audioSource.clip == bgmClip) return;
        
        _audioSource.clip = bgmClip;
        _audioSource.Play();
    }
}