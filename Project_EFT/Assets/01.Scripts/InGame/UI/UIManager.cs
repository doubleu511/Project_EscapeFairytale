using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;

    [Header("RightBottomTip")]
    [Header("Tips")]
    [Space(10)]
    public CanvasGroup tip_rb;
    public Image tip_rb_Img;
    public Text tip_rb_Txt;

    [Header("CursorBottomTip")]
    public Text cursorBtTipText;

    [Header("SizeChangeTip")]
    public CanvasGroup tip_size;
    public RectTransform tip_size_arrow;
    public RectTransform[] tip_size_signs;

    [Header("GameOver")]
    public CanvasGroup gameOverScreen;
    public CanvasGroup gameOverDetail;
    public Image gameOverReason;

    [Header("Letter")]
    public CanvasGroup letterCanvasGroup;
    public Image letterImg;
    public Text letterTxt;

    [Header("Book")]
    public CanvasGroup bookDefault;
    public NoteUI[] bookUIs;

    //Detail
    public CanvasGroup bookDetail;
    public Image bookPageLeft;
    public Button bookPageLeftBtn;
    public Image bookPageRight;
    public Color bookPageRight_BaseColor;
    public Button bookPageRightBtn;
    public Button bookPageCloseBtn;

    private Sprite[] bookSprites;
    private Color endPageColor;
    private int page = 0;

    private void Awake()
    {
        if(!instance)
        instance = this;
    }

    private void Start()
    {
        // 책 UI 버튼 이벤트
        {
            bookPageLeftBtn.onClick.AddListener(() =>
            {
                if (page > 0)
                {
                    page--;
                }
                GameManager.PlaySFX(GameManager.Instance.audioBox.object_book_nextpage);
                BookDetailPage();
            });

            bookPageRightBtn.onClick.AddListener(() =>
            {
                if (page < bookSprites.Length / 2 - ((bookSprites.Length % 2) == 0 ? 1 : 0))
                {
                    page++;
                }
                GameManager.PlaySFX(GameManager.Instance.audioBox.object_book_nextpage);
                BookDetailPage();
            });

            bookPageCloseBtn.onClick.AddListener(() =>
            {
                bookDetail.alpha = 0;
                bookDetail.blocksRaycasts = false;
                bookDetail.interactable = false;
                GameManager.PlaySFX(GameManager.Instance.audioBox.object_book_close);
                page = 0;
            });
        }
    }

    public static void Tip_RBAppear(Sprite sprite, string text, float appearTime, float waitTime, float disappearTime)
    {
        instance.tip_rb.DOComplete();
        instance.tip_rb.alpha = 0;
        instance.tip_rb.DOFade(1, appearTime);
        instance.tip_rb_Img.sprite = sprite;
        instance.tip_rb_Txt.text = text;

        instance.tip_rb.DOFade(0, disappearTime).SetDelay(waitTime);
    }

    public static void Tip_SizeChange(int index)
    {
        instance.tip_size.DOComplete();
        instance.tip_size.alpha = 0;
        instance.tip_size.DOFade(1, 0.1f);
        instance.tip_size_arrow.DOMoveX(instance.tip_size_signs[index].transform.position.x, 1.5f).SetEase(Ease.OutBounce);

        instance.tip_size.DOFade(0, 0.5f).SetDelay(3f);
    }

    public static void GameOverUI(Sprite reasonSprite)
    {
        //GameManager.Instance.player.playerState = PlayerState.DEAD; 는 죽을게 확정될때 해준다.

        instance.gameOverReason.sprite = reasonSprite;

        instance.gameOverScreen.interactable = true;
        instance.gameOverScreen.blocksRaycasts = true;
        instance.gameOverScreen.DOFade(1, 2).OnComplete(() =>
        {
            instance.gameOverDetail.DOFade(1, 0.1f).SetDelay(3).OnComplete(() =>
            {
                GameManager.Instance.isGameOver = true;
            });
        });
    }

    public static void LetterUI(Sprite letter)
    {
        instance.letterCanvasGroup.alpha = 1;
        instance.letterImg.sprite = letter;
        instance.letterTxt.text = "";
    }

    public static void LetterUI(Sprite letterImg, string letterTxt)
    {
        instance.letterCanvasGroup.alpha = 1;
        instance.letterImg.sprite = letterImg;
        instance.letterTxt.text = letterTxt;
    }

    public static void BookDefaultUI(bool show)
    {
        instance.bookDefault.DOFade(show ? 1 : 0, 0.5f);
        instance.bookDefault.blocksRaycasts = show;
        instance.bookDefault.interactable = show;
    }

    public static void BookDetailUI(Sprite[] sprites, Color endPageColor)
    {
        instance.bookDetail.alpha = 1;
        instance.bookDetail.blocksRaycasts = true;
        instance.bookDetail.interactable = true;
        instance.endPageColor = endPageColor;
        instance.bookSprites = sprites;
        instance.BookDetailPage();
    }

    private void BookDetailPage()
    {
        Debug.Log($"currentPage : {page}, maxPage : {(bookSprites.Length / 2)}");

        if (page == 0)
        {
            bookPageLeftBtn.gameObject.SetActive(false);
        }
        else
        {
            bookPageLeftBtn.gameObject.SetActive(true);
        }

        if (page == bookSprites.Length / 2 - ((bookSprites.Length % 2) == 0 ? 1 : 0))
        {
            bookPageRightBtn.gameObject.SetActive(false);

            if (bookSprites.Length % 2 == 0)
            {
                bookPageRight.sprite = bookSprites[page * 2 + 1];
                bookPageRight.color = bookPageRight_BaseColor;
            }
            else
            {
                bookPageRight.sprite = null;
                bookPageRight.color = endPageColor;
            }
        }
        else
        {
            bookPageRightBtn.gameObject.SetActive(true);

            bookPageRight.sprite = bookSprites[page * 2 + 1];
            bookPageRight.color = bookPageRight_BaseColor;
        }

        bookPageLeft.sprite = bookSprites[page * 2];
    }
}
