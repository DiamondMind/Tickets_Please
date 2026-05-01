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

    [Header("Sound Effects")] [SerializeField]
    private AudioClip dayStartSFX;

    [SerializeField] private AudioClip dayEndSFX;
    [SerializeField] private AudioClip nextGuestSFX;
    [Space] [SerializeField] private AudioClip correctDecisionSFX;
    [SerializeField] private AudioClip wrongDecisionSFX;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        ShowBriefing();
    }


    // briefing logic
    private void ShowBriefing()
    {
        userConsole.Hide();
        endDayScreen.Hide();
        briefingScreen.UpdateBriefingText(CurrentDay.dailyBriefing);
        briefingScreen.Show();
        userConsole.UpdateStatsText(CurrentDay.dayNumber, currentIndex, CurrentDay.guestQueue.Length);
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
            FeedbackManager.Instance?.PlayCorrect(playerApproved);
            AudioManager.Instance?.PlaySFX(correctDecisionSFX);
        }
        else
        {
            mistakes++;
            if (playerApproved && guest.isFunKiller) funKillersMissed++;
            FeedbackManager.Instance?.PlayWrong(playerApproved);
            AudioManager.Instance?.PlaySFX(wrongDecisionSFX);
        }

        userConsole.ClearConsole(); // Clear the console immediately after processing the decision
    }

    private bool IsGuestAllowed(GuestProfile guest)
    {
        if (!guest.hasValidTicket) return false;
        if (guest.scanResult.sick) return false;
        if (guest.personalInfo.age < CurrentDay.minAge) return false;
        if (guest.personalInfo.age > CurrentDay.maxAge) return false;
        if (!guest.scanResult.highDopamine) return false;
        if (guest.isFunKiller) return false;
        return true;
    }

    private bool isDayOver = false;

    private void StartDay()
    {
        AudioManager.Instance?.PlaySFX(dayStartSFX);
        isDayOver = false;
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
            isDayOver = true;
            // disable next button, show "end of queue" message
            userConsole.EnableConsoleButtons(false);
            userConsole.ShowQueueEmptyState();
            return;
        }

        userConsole.DisplayGuest(queue[currentIndex]);
        AudioManager.Instance?.PlaySFX(nextGuestSFX);
        userConsole.EnableConsoleButtons(true);
        currentIndex++;
    }

    private void EndDay()
    {
        AudioManager.Instance?.PlaySFX(dayEndSFX);
        totalScore += score;
        totalFunKillersCaught += funKillersCaught;

        userConsole.Hide();

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
        userConsole.Show();
        StartDay();
    }

    public void OnProceedPressed()
    {
        if (IsLastDay)
        {
            // pass total score to next scene
            PlayerPrefs.SetInt("TotalScore", totalScore);
            PlayerPrefs.SetInt("FunKillersCaught", totalFunKillersCaught);
            LevelLoader.LoadLevel(4);
            return;
        }

        currentDayIndex++;
        ShowBriefing();
    }

    public void OnStartDayPressed()
    {
        briefingScreen.Hide();
        userConsole.Show();
        StartDay();
    }

    public void OnNextGuestPressed()
    {
        if (isDayOver)
        {
            EndDay();
            return;
        }

        ShowNextGuest();
    }
}