using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private UserConsole userConsole;

    [Header("Days")] [SerializeField] private DayRuleSet[] days; //add for day1, day 2, day 3 in the editor
    public int currentDayIndex = 0;

    public int mistakes = 0;
    public int score = 0;
    public int funKillersCaught = 0;
    public int funKillersMissed = 0;


    private GuestProfile[] queue;
    private int currentIndex = 0;


    public DayRuleSet CurrentDay => days[currentDayIndex];

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        StartDay();
    }

    public void ProcessDecision(bool playerApproved)
    {
        GuestProfile guest = CurrentDay.guestQueue[currentIndex - 1];
        bool guestShouldEnter = IsGuestAllowed(guest);
        bool correct = playerApproved == guestShouldEnter;

        if (correct)
        {
            score += 100;
            if (!playerApproved && guest.isFunKiller) funKillersCaught++;
        }
        else
        {
            mistakes++;
            if (playerApproved && guest.isFunKiller) funKillersMissed++;
        }

        ShowNextGuest();
    }

    private bool IsGuestAllowed(GuestProfile guest)
    {
        if (!guest.hasValidTicket) return false;
        if (guest.personalInfo.age < CurrentDay.minAge) return false;
        if (guest.personalInfo.age > CurrentDay.maxAge) return false;
        if (guest.isFunKiller) return false;
        return true;
    }

    public void StartDay()
    {
        queue = CurrentDay.guestQueue;
        currentIndex = 0;
        ShowNextGuest();
    }

    private void ShowNextGuest()
    {
        if (currentIndex >= queue.Length)
        {
            // all guests processed  so end day
            EndDay();
            return;
        }

        userConsole.DisplayGuest(queue[currentIndex]);
        currentIndex++;
    }

    private void EndDay()
    {
    }

    public void AdvanceDay()
    {
        currentDayIndex++;
        mistakes = 0;
        //update UI to reflect days
    }

    public bool DayFailed() =>
        mistakes >= CurrentDay
            .maxMistakesAllowed; //if the plyaer has more mistakes, they have filed the day... but they will proceed to the next day
}