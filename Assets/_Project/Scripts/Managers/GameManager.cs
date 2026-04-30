using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private UserConsole userConsole;
    [SerializeField] private EndDayScreen endDayScreen;
    [SerializeField] private BriefingScreen briefingScreen;

    [Header("Days")] [SerializeField] private DayRuleSet[] days; //add for day1, day 2, day 3 in the editor
    public int currentDayIndex = 0;

    [Space]
    // cumulative across all days
    public int totalScore = 0;

    public int totalFunKillersCaught = 0;

    //per day
    public int mistakes = 0;
    public int score = 0;
    public int funKillersCaught = 0;
    public int funKillersMissed = 0;


    private GuestProfile[] queue;
    private int currentIndex = 0;


    public DayRuleSet CurrentDay => days[currentDayIndex];
    public bool IsLastDay => currentDayIndex >= days.Length - 1;

    public enum GameState
    {
        Breifing,
        Playing,
        EndDay,
        ProceedingToNextDay
    }

    public GameState currentState;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        ShowBriefing();
    }

    /// <summary>
    /// briefing logic
    /// </summary>
    private void ShowBriefing()
    {
        userConsole.gameObject.SetActive(false);
        endDayScreen.Hide();
        briefingScreen.UpdateBriefingText(CurrentDay.dailyBriefing);
        briefingScreen.Show();
        userConsole.UpdateStatsText(CurrentDay.dayNumber, 0);
    }


    public void ProcessDecision(bool playerApproved)
    {
        userConsole.EnableConsoleButtons(false);

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

        // ShowNextGuest();
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
        mistakes = 0;
        score = 0;
        funKillersCaught = 0;
        funKillersMissed = 0;

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
        userConsole.EnableConsoleButtons(true);
        currentIndex++;
    }

    private void EndDay()
    {
        totalScore += score;
        totalFunKillersCaught += funKillersCaught;

        userConsole.gameObject.SetActive(false);
        endDayScreen.UpdateResultsUI(score, mistakes, funKillersCaught);
        endDayScreen.Show(IsLastDay);
    }

    public bool DayFailed() =>
        mistakes >= CurrentDay
            .maxMistakesAllowed; //if the plyaer has more mistakes, they have filed the day... but they will proceed to the next day


    //all the button events for each screen
    public void OnRetryPressed()
    {
        endDayScreen.Hide();
        userConsole.gameObject.SetActive(true);
        StartDay();
    }

    public void OnProceedPressed()
    {
        if (IsLastDay)
        {
            // pass total score to next scene
            PlayerPrefs.SetInt("TotalScore", totalScore);
            PlayerPrefs.SetInt("FunKillersCaught", totalFunKillersCaught);
            LevelLoader.LoadLevel(3);
            return;
        }

        currentDayIndex++;
        ShowBriefing();
    }

    public void OnStartDayPressed()
    {
        briefingScreen.Hide();
        userConsole.gameObject.SetActive(true);
        StartDay();
    }

    public void OnNextGuestPressed()
    {
        ShowNextGuest();
    }
}