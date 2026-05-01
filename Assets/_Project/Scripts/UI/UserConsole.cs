using System;
using DG.Tweening;
using KBCore.Refs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UserConsole : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statsText; //day, count of proccesed tickets

    [Space] [SerializeField] private Button acceptButton;
    [SerializeField] private Button rejectButton;
    [SerializeField] private Button nextGuestButton;

    [Space] [SerializeField] private Image portraitImage;
    [SerializeField] private Image ticketImage;

    [Space] [SerializeField] private TextMeshProUGUI personalInfoText;
    [SerializeField] private TextMeshProUGUI scanResultsText;

    [Space] [SerializeField] private Sprite validTicketSprite;
    [SerializeField] private Sprite[] invalidTicketsSprites;

    [Space] [SerializeField] AudioClip buttonClickSFX;
    [SerializeField] AudioClip clearConsoleSFX;
    [Space]
    [SerializeField] AudioClip stampDownSFX;
    
    [Space]
    [SerializeField, Self] private CanvasGroup contentGroup;
    [SerializeField] AudioClip showPanelSFX;
    [SerializeField] AudioClip hidePanelSFX;

    
    private void Start()
    {
        acceptButton.onClick.AddListener(() =>
        {
            AudioManager.Instance?.PlaySFX(stampDownSFX);
            GameManager.Instance.ProcessDecision(true);
        });
        rejectButton.onClick.AddListener(() =>
        {
            AudioManager.Instance?.PlaySFX(stampDownSFX);
            GameManager.Instance.ProcessDecision(false);
        });

        nextGuestButton.onClick.AddListener(GameManager.Instance.OnNextGuestPressed);
    }

    public void DisplayGuest(GuestProfile guest)
    {
        UpdatePersonalInfo(guest);
        UpdateScanResults(guest);
        UpdateTicketInfo(guest);
    }


    public void EnableConsoleButtons(bool isAttending)
    {
        if (isAttending)
        {
            acceptButton.interactable = true;
            rejectButton.interactable = true;
            nextGuestButton.interactable = false;
        }
        else
        {
            acceptButton.interactable = false;
            rejectButton.interactable = false;
            nextGuestButton.interactable = true;
        }
    }

    void UpdatePersonalInfo(GuestProfile guestProfile)
    {
        var personalInfo = guestProfile.personalInfo;
        var text =
            $"name: {personalInfo.name} \n \nage: {personalInfo.age} \n \nreason for coming: {personalInfo.reasonForVisit}";
        personalInfoText.text = text;
        portraitImage.sprite = personalInfo.portrait;
    }

    void UpdateScanResults(GuestProfile guestProfile)
    {
        var scanResult = guestProfile.scanResult;
        var results = "";
        results += $"Sick: {(scanResult.sick ? "Yes" : "No")}\n \n";
        results += $"Low Cortisol: {(scanResult.lowCortisol ? "Yes" : "No")}\n \n";
        results += $"High Dopamine: {(scanResult.highDopamine ? "Yes" : "No")}";

        scanResultsText.text = results;
    }

    void UpdateTicketInfo(GuestProfile guestProfile)
    {
        if (guestProfile.hasValidTicket)
        {
            ticketImage.sprite = validTicketSprite;
        }
        else if (invalidTicketsSprites is { Length: > 0 })
        {
            var index = Random.Range(0, invalidTicketsSprites.Length);
            ticketImage.sprite = invalidTicketsSprites[index];
        }
    }

    public void UpdateStatsText(int day, int currentGuestIndex, int totalGuests)
    {
        statsText.text = $"Day {day}\n{currentGuestIndex}/{totalGuests}";
    }

    public void ClearConsole()
    {
        scanResultsText.text = "";
        personalInfoText.text = "";
        portraitImage.sprite = null;
        ticketImage.sprite = null;
        AudioManager.Instance?.PlaySFX(clearConsoleSFX);
    }

    public void ShowQueueEmptyState()
    {
        personalInfoText.text = "No more guests today.";
        scanResultsText.text = "";
        portraitImage.sprite = null;
        ticketImage.sprite = null;
        //nextGuestButton active so player can press to end day
        AudioManager.Instance?.PlaySFX(clearConsoleSFX);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        contentGroup.alpha = 0;
        contentGroup.DOKill(); // kill any previous tween on this target
        contentGroup.DOFade(1f, 0.8f).SetDelay(0.0f);
        AudioManager.Instance?.PlaySFX(showPanelSFX);
    }

    public void Hide()
    {
        contentGroup.DOKill(); // kill any previous tween on this target
        contentGroup.DOFade(0f, 0.1f).SetDelay(0.0f).OnComplete(() => gameObject.SetActive(false));
        AudioManager.Instance?.PlaySFX(hidePanelSFX);
    }
}