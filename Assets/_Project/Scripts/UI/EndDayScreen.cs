using DG.Tweening;
using KBCore.Refs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class EndDayScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI endResultsText;
    [SerializeField] Button retryBtn; //can retry day if not satisfied with results
    [SerializeField] private Button proceedBtn; //proceed to next day
    
    [SerializeField, Self] private CanvasGroup contentGroup;
    [Space]
    [SerializeField] private AudioClip showPanelSFX;
    [SerializeField] private AudioClip hidePanelSFX;

    private void Start()
    {
        retryBtn.onClick.AddListener(() => GameManager.Instance.OnRetryPressed());
        proceedBtn.onClick.AddListener(() => GameManager.Instance.OnProceedPressed());
    }

    public void UpdateResultsUI(int score, int mistakes, int funKillersCaught)
    {
        var text = $"score: {score} \n\nmistakes: {mistakes} \n\nfun killers caught: {funKillersCaught}";
        endResultsText.text = text; 
    }

    public void Show(bool isLastDay)
    {
        retryBtn.gameObject.SetActive(!isLastDay); // no retry on final day
        proceedBtn.GetComponentInChildren<TextMeshProUGUI>().text = isLastDay ? "See Results" : "Next Day";
        gameObject.SetActive(true);
        contentGroup.alpha = 0;
        contentGroup.DOKill(); // kill any previous tween on this target
        contentGroup.DOFade(1f, 0.25f).SetDelay(0.0f);
        AudioManager.Instance?.PlaySFX(showPanelSFX);
    }

    public void Hide()
    {
        contentGroup.DOKill(); // kill any previous tween on this target
        contentGroup.DOFade(0f, 0.25f).SetDelay(0.0f).OnComplete(() => gameObject.SetActive(false));
        AudioManager.Instance?.PlaySFX(hidePanelSFX);
    }
}