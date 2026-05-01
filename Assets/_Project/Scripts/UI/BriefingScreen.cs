using DG.Tweening;
using KBCore.Refs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class BriefingScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI briefText;
    [SerializeField] Button startDayBtn; //start main game

    [SerializeField, Self] private CanvasGroup contentGroup;
    [Space]
    [SerializeField] private AudioClip showPanelSFX;
    [SerializeField] private AudioClip hidePanelSFX;
    
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
        contentGroup.alpha = 0;
        contentGroup.DOKill(); // kill any previous tween on this target
        contentGroup.DOFade(1f, 0.8f).SetDelay(0.0f);
        AudioManager.Instance?.PlaySFX(showPanelSFX);
    }

    public void Hide()
    {
        contentGroup.DOKill(); // kill any previous tween on this target
        contentGroup.DOFade(0f, 0.25f).SetDelay(0.0f).OnComplete(() => gameObject.SetActive(false));
        AudioManager.Instance?.PlaySFX(hidePanelSFX);
    }
}