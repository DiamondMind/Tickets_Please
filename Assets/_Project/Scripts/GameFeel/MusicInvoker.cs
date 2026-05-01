using System;
using UnityEngine;

public class MusicInvoker : MonoBehaviour
{
    [SerializeField] private AudioClip musicToPlay;

    private void Start()
    {
        if (musicToPlay != null)
        {
            AudioManager.Instance?.PlayMusic(musicToPlay);
        }
    }
}
