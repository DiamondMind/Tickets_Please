using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserConsole : MonoBehaviour
{
    [SerializeField] private Button acceptButton;
    [SerializeField] private Button rejectButton;
    [SerializeField] private Button nextGuestButton;

    [Space]
    [SerializeField] private Image portraitImage;
    [SerializeField] private Image ticketImage;

    [Space]
    [SerializeField] private TextMeshProUGUI personalInfoText;
    [SerializeField] private TextMeshProUGUI scanResultsText;

    [Space]
    [SerializeField] private Sprite validTicketSprite;
    [SerializeField] private Sprite[] invalidTicketsSprites;


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

    public void UpdatePersonalInfo(GuestProfile guestProfile)
    {
        var personalInfo = guestProfile.personalInfo;
        var text =
            $"name: {personalInfo.name} \n age: {personalInfo.age} \n reason for coming: {personalInfo.reasonForVisit}";
        personalInfoText.text = text;
        portraitImage.sprite = personalInfo.portrait;
    }

    public void UpdateScanResults(GuestProfile guestProfile)
    {
        var scanResult = guestProfile.scanResult;
        var results = "";
        results += $"Sick: {(scanResult.sick ? "Yes" : "No")}\n";
        results += $"Low Cortisol: {(scanResult.lowCortisol ? "Yes" : "No")}\n";
        results += $"High Dopamine: {(scanResult.highDopamine ? "Yes" : "No")}";

        scanResultsText.text = results;
    }

    public void UpdateTicketInfo(GuestProfile guestProfile)
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
}