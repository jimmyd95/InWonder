using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

public class RadioManager : MonoBehaviour
{

    [SerializeField] private List<AudioClip> _radioSongs = new List<AudioClip>();
    [SerializeField] private GameObject _radio;
    private bool _isOn = true;


    [Button("Power Button")]
    public void PowerButtonMusic()
    {
        if(_isOn){
            _radio.GetComponent<AudioSource>().Stop();
            _isOn = false;
        }else{
            _radio.GetComponent<AudioSource>().Play();
            _isOn = true;
        }
    }

    [Button("Next Song")]
    public void NextSong()
    {
        int currentSong = _radioSongs.IndexOf(_radio.GetComponent<AudioSource>().clip);
        if(currentSong == _radioSongs.Count - 1){
            _radio.GetComponent<AudioSource>().clip = _radioSongs[0];
        }else{
            _radio.GetComponent<AudioSource>().clip = _radioSongs[currentSong + 1];
        }
        _radio.GetComponent<AudioSource>().Play();
    }

    [Button("Previous Song")]
    public void PreviousSong()
    {
        int currentSong = _radioSongs.IndexOf(_radio.GetComponent<AudioSource>().clip);
        if(currentSong == 0){
            _radio.GetComponent<AudioSource>().clip = _radioSongs[_radioSongs.Count - 1];
        }else{
            _radio.GetComponent<AudioSource>().clip = _radioSongs[currentSong - 1];
        }
        _radio.GetComponent<AudioSource>().Play();
    }

    [Button("Volume Up")]
    public void VolumeUp()
    {
        _radio.GetComponent<AudioSource>().volume += 0.1f;
    }

    [Button("Volume Down")]
    public void VolumeDown()
    {
        _radio.GetComponent<AudioSource>().volume -= 0.1f;
    }

    [Button("Mute")]
    public void Mute()
    {
        _radio.GetComponent<AudioSource>().volume = 0;
    }

    [Button("Unmute")]
    public void Unmute()
    {
        _radio.GetComponent<AudioSource>().volume = 1;
    }

    [Button("Pause")]
    public void Pause()
    {
        _radio.GetComponent<AudioSource>().Pause();
    }

    [Button("Play")]
    public void Play()
    {
        _radio.GetComponent<AudioSource>().Play();
    }

}
