using UnityEngine;
using UnityEngine.Events;

public static class EventBus
{
    public static readonly AudioEvents Audio = new AudioEvents();
    public static readonly GameEvents GameE = new GameEvents();

    public class AudioEvents
    {
        public UnityAction<AudioClip> OnPlayOnShotSound;
    }
    
    public class GameEvents
    {
        public UnityAction OnDayStart;
        public UnityAction<int, int, int> OnDayEnd;
    }
}