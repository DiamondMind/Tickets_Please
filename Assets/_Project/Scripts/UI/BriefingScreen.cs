using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BriefingScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI briefText;
    [SerializeField] Button startDayBtn; //start main game

    private void Start()
    {
        startDayBtn.onClick.AddListener(() => GameManager.Instance.OnStartDayPressed());
    }
    public void UpdateBriefingText(string b)
    {
        briefText.text = b;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}