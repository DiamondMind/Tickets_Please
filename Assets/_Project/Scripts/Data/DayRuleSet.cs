using UnityEngine;

[CreateAssetMenu(fileName = "DayRuleSet", menuName = "Scriptable Objects/DayRuleSet")]
public class DayRuleSet : ScriptableObject
{
    public int dayNumber;
    public GuestProfile[] guestQueue;
    public int TotalGuestsForDay => guestQueue.Length;

    [Header("Rules")] public int minAge;
    public int maxAge;
    public int maxMistakesAllowed;
    public string dailyBriefing;
}