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

            "Guests from all walks of life come here to laugh, play, and create unforgettable memories.",

            "Fun Park was built on one core belief:",

            "\"Play should be safe, joyful, and open to all who protect it.\"",

            "But some visitors threaten that joy.",

            "Fun Killers, rule breakers, and unhealthy guests can ruin the experience for everyone.",

            $"Your mission is clear, {username}:",

            "Protect the gates.",

            "Inspect every guest carefully before granting entry.",

            "VALID ENTRY REQUIREMENTS:",

            "- Guest must possess a GOLDEN ticket.",
            "- Guest must be under 25 years old.",
            "- Guest must have LOW cortisol levels.",
            "- Guest must have HIGH dopamine levels.",
            "- Guest must NOT be sick.",

            "If any requirement is broken — DENY ENTRY.",

            "APPROVE only qualified guests.",

            "REJECT Fun Killers and harmful visitors.",

            "Too many mistakes will lower park happiness.",

            "Protect borderless fun.",

            "Keep Fun Park joyful.",

            "Good luck, Gatekeeper."
        };

        nameInputCanvas.SetActive(false);
        storyCanvas.SetActive(true);
        StartCoroutine(PlayStory());
    }

    private IEnumerator PlayStory()
    {
        foreach (var line in storyLines)
        {
            yield return StartCoroutine(TypeLine(line));
            yield return new WaitForSeconds(1.2f); // pause between lines

            if (skipRequested) break;
        }

        // done, load next scene
        LevelLoader.LoadLevel(3);
    }

    private IEnumerator TypeLine(string line)
    {
        storyText.text = "";
        isTyping = true;

        foreach (char c in line)
        {
            if (skipRequested)
            {
                storyText.text = line; // snap to full line
                break;
            }

            storyText.text += c;
            AudioManager.Instance?.PlaySFX(typeSound);
            yield return new WaitForSeconds(typeSpeed);
        }

        isTyping = false;
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