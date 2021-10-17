using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class NoteUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Sprite[] noteSprites;
    public Color endPageColor;

    private CanvasGroup noteImg;

    private bool noteEnabled = false;
    public bool NoteEnabled {
        get {
            return noteEnabled;
        }
        set {
            noteEnabled = value;
            noteImg.alpha = (value) ? 1 : 0.2f;
            noteImg.blocksRaycasts = value;
            noteImg.interactable = value;
        }
    }

    private Outline outline;

    public Color color_anim1 = Color.red;
    public Color color_anim2 = Color.red;

    private void Awake()
    {
        outline = GetComponent<Outline>();
        noteImg = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        ColorChange(true);
    }

    private void ColorChange(bool start)
    {
        if (start)
        {
            outline.DOColor(color_anim1, 1).OnComplete(() => ColorChange(false));
        }
        else
        {
            outline.DOColor(color_anim2, 1).OnComplete(() => ColorChange(true));
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        UIManager.BookDetailUI(noteSprites, endPageColor);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        outline.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        outline.enabled = false;
    }
}
