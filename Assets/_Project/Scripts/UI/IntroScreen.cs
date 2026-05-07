using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IntroScreen : MonoBehaviour
{
    [Header("Name Input")] [SerializeField]
    private GameObject nameInputCanvas;

    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private Button submitButton;

    [Header("Story Display")] [SerializeField]
    private GameObject storyCanvas;

    [SerializeField] private TextMeshProUGUI storyText;
    [SerializeField] private Button skipButton;

    [Header("Settings")] [SerializeField] private float typeSpeed = 0.04f;
    [SerializeField] private int linesPerPage = 3;
    [SerializeField] private AudioClip typeSound;

    private string[] storyLines;
    private bool isTyping = false;
    private bool skipRequested = false;

    private void Start()
    {
        storyCanvas.SetActive(false);
        submitButton.onClick.AddListener(OnSubmitName);
        skipButton.onClick.AddListener(OnSkip);
    }

    private void OnSubmitName()
    {
        string username = nameInputField.text.Trim();
        if (string.IsNullOrEmpty(username)) username = "Friend";

        PlayerPrefs.SetString("PlayerName", username);

        storyLines = new string[]
        {
            $"Welcome, {username}.",
            "You are the Gatekeeper of Fun Park — where joy knows no borders.",
            "Guests arrive here to laugh, play, and create shared memories.",
            "\"Play should be safe, joyful, and open to all who protect it.\"",
            "But not every path to play is the same.",
            "Some conditions affect how safely and fairly guests can participate.",
            "Poor decisions or unstable guests can disrupt the experience for everyone.",
            $"Your mission {username} is to assess each guest before entry.",
            "VALID ENTRY GUIDELINES:",
            "- Guest must have a GOLDEN ticket.",
            "- Guest must be under 25 years old.",
            "- Guest must have LOW cortisol levels.",
            "- Guest must have HIGH dopamine levels.",
            "- Guest must NOT be sick.",
            "To decide, drag the to approve or to reject.",
            "Place your stamp on the guest document to shape their access to play.",
            "Incorrect judgments will reduce park harmony.",
            "Protect borderless fun.",
            "Remember: every decision shapes what 'without borders' truly means.",
            "Keep play flowing. Good luck, Gatekeeper."
        };

        nameInputCanvas.SetActive(false);
        storyCanvas.SetActive(true);
        StartCoroutine(PlayStory());
    }

    private IEnumerator PlayStory()
    {
        for (int i = 0; i < storyLines.Length; i += linesPerPage)
        {
            if (skipRequested)
                break;

            storyText.text = "";
            isTyping = true;

            int end = Mathf.Min(i + linesPerPage, storyLines.Length);

            for (int j = i; j < end; j++)
            {
                yield return StartCoroutine(TypeLine(storyLines[j]));
                storyText.text += "\n"; // spacing between lines inside page
            }

            isTyping = false;

            yield return new WaitForSeconds(0.6f);
        }

        LevelLoader.LoadLevel(3);
    }

    private IEnumerator TypeLine(string line)
    {
        foreach (char c in line)
        {
            if (skipRequested)
            {
                storyText.text += line;
                yield break;
            }

            storyText.text += c;

            // Only play sound if character is NOT a space
            if (c != ' ' && c != '\n' && c != '\t')
            {
                AudioManager.Instance?.PlaySFX(typeSound);
            }

            yield return new WaitForSeconds(typeSpeed);
        }

        storyText.text += "\n";
    }

    private void OnSkip()
    {
        if (isTyping)
        {
            // first skip press: finish current line instantly
            skipRequested = true;
            StartCoroutine(ResetSkipAfterFrame());
        }
        else
        {
            // second skip press while not typing: jump to end
            skipRequested = true;
        }
    }

    private IEnumerator ResetSkipAfterFrame()
    {
        yield return new WaitForSeconds(0.1f);
        skipRequested = false; // reset so next line types normally
    }
}