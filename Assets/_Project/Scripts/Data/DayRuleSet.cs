using UnityEngine;

[CreateAssetMenu(fileName = "DayRuleSet", menuName = "Scriptable Objects/DayRuleSet")]
public class DayRuleSet : ScriptableObject
{
    public GuestProfile[] guestForTheDay;
    public int TotalGuestsForDay => guestForTheDay.Length;
}
