using UnityEngine;

[ExecuteInEditMode]
public class ResetCanvasOpacityAtStart : MonoBehaviour
{
    [Range(0f, 1f)]
    public float editorAlpha = 0.5f; 

    private CanvasGroup c;

    void OnEnable()
    {
        c = GetComponent<CanvasGroup>();
    }

    void Start()
    {
        if (c != null)
        {
            c.alpha = 1f; 
        }
    }

    void Update()
    {
        if (!Application.isPlaying && c != null)
        {
            c.alpha = editorAlpha; 
        }
    }
}
