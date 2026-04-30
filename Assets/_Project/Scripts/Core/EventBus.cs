using UnityEngine;
using UnityEngine.Events;

public static class EventBus
{
    public static readonly AudioEvents Audio = new AudioEvents();

    public class AudioEvents
    {
        public UnityAction<AudioClip> OnPlayOnShotSound;
    }
}