using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button playBtn;
    public Button exitBtn;
    
    [Header("Settings")] 
    [SerializeField] private Button music, sfx;
    
    [Header("UI Elements")]
    [SerializeField] private Image musicIcon, sfxIcon;
    [SerializeField] private Sprite musicOn, musicOff, sfxOn, sfxOff;
    
    void Start()
    {
        playBtn.onClick.AddListener(() => { UnityEngine.SceneManagement.SceneManager.LoadScene(1); });
        exitBtn.onClick.AddListener(Application.Quit);
        
        UpdateSettingsVisuals();
        
        music.onClick.AddListener(() => {
            AudioManager.Instance.ToggleMusic();
            UpdateSettingsVisuals();
        });
        sfx.onClick.AddListener(() => {
            AudioManager.Instance.ToggleSFX();
            UpdateSettingsVisuals();
        });
    }
    
    void UpdateSettingsVisuals()
    {
        if (AudioManager.Instance == null)
            return;
        
        musicIcon.sprite = AudioManager.Instance.IsMusicOn ? musicOn : musicOff;
        sfxIcon.sprite = AudioManager.Instance.IsSFXOn ? sfxOn : sfxOff;
    }
}
