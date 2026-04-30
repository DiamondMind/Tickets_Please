using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndDayScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI endResultsText;
    [SerializeField] Button retryBtn; //can retry day if not satisfied with results
    [SerializeField] private Button proceedBtn; //proceed to next day

    private void Start()
    {
        retryBtn.onClick.AddListener(() => GameManager.Instance.OnRetryPressed());
        proceedBtn.onClick.AddListener(() => GameManager.Instance.OnProceedPressed());
        Hide();
    }

    public void UpdateResultsUI(int score, int mistakes, int funKillersCaught)
    {
        var text = $"score: {score} \n\nmistakes: {mistakes} \n\nfun killers caught: {funKillersCaught}";
        endResultsText.text = text; // this line was missing
    }

    public void Show(bool isLastDay)
    {
        retryBtn.gameObject.SetActive(!isLastDay); // no retry on final day
        proceedBtn.GetComponentInChildren<TextMeshProUGUI>().text = isLastDay ? "See Results" : "Next Day";
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}