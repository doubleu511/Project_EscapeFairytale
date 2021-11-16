using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;

    [Header("InGame")]
    public CanvasGroup inGameCanvasGroup;

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
    public Button letterExitBtn;

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
    public Image bookCover;

    private Sprite[] bookSprites;
    private int page = 0;

    [Header("SubCameraEvent")]
    public Camera subCamera;
    private Camera mainCamera;
    public Button subCamera_Back;
    public Image cameraAim;
    public SelectableObject currentShowObject;
    public CanvasGroup aliceClock_input;
    public CanvasGroup pinocio_tree;
    public Button pinocio_tree_backBtn;
    public CanvasGroup pinocio_tree_branch;
    public GameObject[] pinocio_tree_branches;
    public Button pinocio_tree_branch_backBtn;
    public CanvasGroup pinocio_tree_leaf;
    public Text pinocio_tree_leaf_text;
    public Button pinocio_tree_leaf_backBtn;

    [Header("PausePanel")]
    public CanvasGroup pausePanel;
    public RectTransform menuPanel;
    public Button menu_exitBtn;

    [Space(20)]
    public CanvasGroup confirmBG;
    public RectTransform confirmPanel;
    public Text question;
    public Text leftBtnText;
    public Text rightBtnText;
    public Button leftBtn;
    public Button rightBtn;

    [Space(20)]
    public CanvasGroup optionPanel;
    public GameObject[] optionDetailPanel;
    public Dropdown graphics_WindowMode;
    public Dropdown graphics_Resolution;
    public Slider sounds_bgmSlider;
    public Slider sounds_sfxSlider;
    public Text sounds_bgmText;
    public Text sounds_sfxText;

    [Header("Tutorial")]
    public CanvasGroup tutorialPanel;
    public Text tutorialPanelText;
    public CanvasGroup tutorialTip;
    private Queue<string> tutorialQueue = new Queue<string>();
    [HideInInspector] public bool isTutorialPanelAppear = false;

    private void Awake()
    {
        if (!instance)
            instance = this;

        mainCamera = Camera.main;
        inGameCanvasGroup.alpha = 0;
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

            bookPageCloseBtn.onClick.AddListener(BookDetailClose);
        }

        // 편지 UI 버튼 이벤트
        letterExitBtn.onClick.AddListener(() =>
        {
            LetterUIClose();
        });
        subCamera_Back.onClick.AddListener(ChangeToMainCamera);

        // 게임 종료 버튼
        menu_exitBtn.onClick.AddListener(() =>
        {
            ConfirmUI("정말로 게임을 종료하시겠습니까?", "예", "아니오", () =>
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false; //play모드를 false로.
#else
                    Application.Quit();
#endif
            });
        });

        pinocio_tree_backBtn.onClick.AddListener(() =>
        {
            CanvasGroup_DefaultShow(pinocio_tree, false, false);
            currentShowObject = null;
            ChangeToMainCamera();
        });

        pinocio_tree_branch_backBtn.onClick.AddListener(() =>
        {
            CanvasGroup_DefaultShow(pinocio_tree_branch, false);
        });

        pinocio_tree_leaf_backBtn.onClick.AddListener(() =>
        {
            CanvasGroup_DefaultShow(pinocio_tree_leaf, false);
        });

        // 설정 드롭다운
        graphics_WindowMode.onValueChanged.AddListener(value => SettingManager.ScreenMode(value));
        graphics_Resolution.onValueChanged.AddListener(value => SettingManager.Resolution(value));
        sounds_bgmSlider.onValueChanged.AddListener(value => SettingManager.BGMVolume(value));
        sounds_bgmSlider.onValueChanged.AddListener(value => sounds_bgmText.text = Mathf.RoundToInt(value * 100).ToString());
        sounds_sfxSlider.onValueChanged.AddListener(value => SettingManager.SFXVolume(value));
        sounds_sfxSlider.onValueChanged.AddListener(value => sounds_sfxText.text = Mathf.RoundToInt(value * 100).ToString());

        int fullScreenValue = SecurityPlayerPrefs.GetInt(SettingManager.FULL_SCREEN, 0);
        int resolutionValue = SecurityPlayerPrefs.GetInt(SettingManager.RESOLUTION, 0);
        float bgmValue = SecurityPlayerPrefs.GetFloat(SettingManager.BGMVOLUME, 1);
        float sfxValue = SecurityPlayerPrefs.GetFloat(SettingManager.SFXVOLUME, 1);

        graphics_WindowMode.value = fullScreenValue;
        graphics_Resolution.value = resolutionValue;
        sounds_bgmSlider.value = bgmValue;
        sounds_sfxSlider.value = sfxValue;

        SettingManager.bgmVolume = bgmValue;
        SettingManager.sfxVolume = sfxValue;

        // 튜토리얼
        tutorialTip.DOFade(0, 1).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }

    public static void InGameAppear(bool value)
    {
        if (value)
        {
            MouseEvent.MouseLock(true);
            instance.inGameCanvasGroup.DOFade(1, 0.5f);
        }
        else
        {
            MouseEvent.MouseLock(false);
            instance.inGameCanvasGroup.DOFade(0, 0.5f);
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
        instance.gameOverScreen.alpha = 1;

/*        Sequence seq = DOTween.Sequence();

        seq.AppendInterval(3);
        seq.AppendCallback(() =>
        {*/
            instance.gameOverDetail.alpha = 1;
            GameManager.Instance.isGameOver = true;
        //});
    }

    public static void CanvasGroup_DefaultShow(CanvasGroup group, bool show)
    {
        group.DOComplete();
        group.alpha = show ? 1 : 0;
        group.blocksRaycasts = show;
        group.interactable = show;
    }

    public static void CanvasGroup_DefaultShow(CanvasGroup group, bool show, bool animatedsetUpdate)
    {
        group.DOComplete();
        group.DOFade(show ? 1 : 0, 0.5f).SetUpdate(animatedsetUpdate);

        group.blocksRaycasts = show;
        group.interactable = show;
    }

    #region LetterUI

    public static void LetterUI(Sprite letter)
    {
        MouseEvent.MouseLock(false);
        instance.letterCanvasGroup.alpha = 1;
        instance.letterCanvasGroup.interactable = true;
        instance.letterCanvasGroup.blocksRaycasts = true;
        instance.letterImg.sprite = letter;
        instance.letterTxt.text = "";
    }

    public static void LetterUI(Sprite letterImg, string letterTxt, Font letterFont)
    {
        MouseEvent.MouseLock(false);
        instance.letterCanvasGroup.alpha = 1;
        instance.letterCanvasGroup.interactable = true;
        instance.letterCanvasGroup.blocksRaycasts = true;
        instance.letterImg.sprite = letterImg;
        instance.letterTxt.text = letterTxt;
        instance.letterTxt.font = letterFont;
    }

    public static void LetterUIClose()
    {
        MouseEvent.MouseLock(true);
        instance.letterCanvasGroup.alpha = 0;
        instance.letterCanvasGroup.interactable = false;
        instance.letterCanvasGroup.blocksRaycasts = false;
    }

    #endregion

    #region BookUI

    public static void BookDefaultUI(bool show)
    {
        CanvasGroup_DefaultShow(instance.bookDefault, show, false);
    }

    public static void BookDetailUI(Sprite[] sprites, Color coverColor)
    {
        CanvasGroup_DefaultShow(instance.bookDetail, true);
        instance.bookCover.color = coverColor;
        instance.bookSprites = sprites;
        instance.BookDetailPage();
    }

    public static void BookDetailClose()
    {
        CanvasGroup_DefaultShow(instance.bookDetail, false);

        if (GameManager.Instance.player.playerState == PlayerState.OPEN_BOOK)
        {
            GameManager.PlaySFX(GameManager.Instance.audioBox.object_book_close);
        }
        instance.page = 0;
    }

    private void BookDetailPage()
    {
        Debug.Log($"currentPage : {page}, maxPage : {bookSprites.Length / 2 - ((bookSprites.Length % 2) == 0 ? 1 : 0)}");

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
                bookPageRight.enabled = true;
                bookPageRight.sprite = bookSprites[page * 2 + 1];
            }
            else
            {
                bookPageRight.enabled = false;
                bookPageRight.sprite = null;
            }
        }
        else
        {
            bookPageRightBtn.gameObject.SetActive(true);

            bookPageRight.enabled = true;
            bookPageRight.sprite = bookSprites[page * 2 + 1];
        }

        bookPageLeft.sprite = bookSprites[page * 2];
    }
    #endregion

    #region CameraMove

    public static void ChangeToSubCamera(Vector3 pos, Quaternion rotation, bool backButtonAppear = true)
    {
        GetMainCam().gameObject.SetActive(false);
        instance.cameraAim.gameObject.SetActive(false);
        instance.subCamera.transform.position = pos;
        instance.subCamera.transform.rotation = rotation;
        instance.subCamera.gameObject.SetActive(true);
        instance.subCamera_Back.gameObject.SetActive(backButtonAppear);
        MouseEvent.MouseLock(false);
        GameManager.Instance.player.isSubCam = true;
    }

    public static Camera GetMainCam()
    {
        return instance.mainCamera;
    }

    public static void ChangeToMainCamera()
    {
        GetMainCam().gameObject.SetActive(true);
        instance.cameraAim.gameObject.SetActive(true);
        instance.cursorBtTipText.rectTransform.anchoredPosition = new Vector3(0, -64, 0);
        instance.subCamera.gameObject.SetActive(false);
        instance.subCamera_Back.gameObject.SetActive(false);
        MouseEvent.MouseLock(true);
        GameManager.Instance.player.isSubCam = false;

        if (instance.currentShowObject != null)
        {
            instance.currentShowObject = null;
        }
    }

    #endregion

    #region PauseMenu

    public static void PauseUI(bool value)
    {
        GameManager.Instance.player.playerState = value ? PlayerState.PAUSED : PlayerState.NORMAL;
        MouseEvent.MouseLock(!value);

        Time.timeScale = value ? 0 : 1;
        instance.pausePanel.interactable = value;
        instance.pausePanel.blocksRaycasts = value;
        instance.pausePanel.DOComplete();
        instance.menuPanel.DOComplete();

        if (value)
        {
            instance.pausePanel.DOFade(1, 0.5f).SetUpdate(true);
            instance.menuPanel.DOSizeDelta(new Vector2(510, 0), 0.5f).OnStart(() =>
            {
                instance.menuPanel.sizeDelta = new Vector2(0, instance.menuPanel.sizeDelta.y);
            }).SetRelative().SetUpdate(true);
        }
        else
        {
            instance.pausePanel.DOFade(0, 0.75f).SetUpdate(true);
            instance.menuPanel.DOSizeDelta(new Vector2(-510, 0), 0.5f).OnStart(() =>
            {
                instance.menuPanel.sizeDelta = new Vector2(510, instance.menuPanel.sizeDelta.y);
            }).SetRelative().SetUpdate(true);
            OptionPanel(false);
        }
    }

    public static void ConfirmUI(string qustion, string leftBtnAnswer, string rightBtnAnswer, Action leftBtnAction, Action rightBtnAction = null)
    {
        ConfirmPanelAppear(true);
        instance.question.text = qustion;
        instance.leftBtnText.text = leftBtnAnswer;
        instance.rightBtnText.text = rightBtnAnswer;

        if (leftBtnAction != null)
        {
            instance.leftBtn.onClick.RemoveAllListeners();
            instance.leftBtn.onClick.AddListener(() =>
            {
                leftBtnAction();
            });
        }

        instance.rightBtn.onClick.RemoveAllListeners();
        instance.rightBtn.onClick.AddListener(() =>
        {
            ConfirmPanelAppear(false);
        });
        if (rightBtnAction != null)
        {
            instance.rightBtn.onClick.AddListener(() => rightBtnAction());
        }
    }

    public static void ConfirmPanelAppear(bool value)
    {
        instance.confirmBG.interactable = value;
        instance.confirmBG.blocksRaycasts = value;
        RectTransform rect = instance.confirmPanel;
        instance.confirmBG.DOComplete();
        rect.DOComplete();

        if (value)
        {
            instance.confirmBG.DOFade(1, 0.3f).SetUpdate(true);
            rect.DOSizeDelta(new Vector2(0, 500), 0.75f).OnStart(() =>
            {
                rect.sizeDelta = new Vector2(rect.sizeDelta.x, 0);
            }).SetRelative(true).SetUpdate(true);
        }
        else
        {
            instance.confirmBG.DOFade(0, 0.3f).SetUpdate(true);
            rect.DOSizeDelta(new Vector2(0, -500), 0.75f).OnStart(() =>
            {
                rect.sizeDelta = new Vector2(rect.sizeDelta.x, 500);
            }).SetRelative(true).SetUpdate(true);
        }
    }

    public static void OptionPanel(bool value)
    {
        CanvasGroup_DefaultShow(instance.optionPanel, value, true);
    }

    public static void OptionDetailPanel(int index)
    {
        for (int i = 0; i < instance.optionDetailPanel.Length; i++)
        {
            instance.optionDetailPanel[i].gameObject.SetActive(false);
        }
        instance.optionDetailPanel[index].gameObject.SetActive(true);
    }

    #endregion

    #region Tutorial

    public static void TutorialPanel(string text)
    {
        if (text.Equals(""))
        {
            print("테스트");
            if (instance.tutorialQueue.Count > 0)
            {
                instance.isTutorialPanelAppear = false;
                instance.tutorialPanel.DOKill();
                instance.tutorialPanel.DOFade(0, 1).OnComplete(() =>
                {
                    TutorialPanel(instance.tutorialQueue.Dequeue());
                });

                return;
            }

            instance.isTutorialPanelAppear = false;
            instance.tutorialPanel.DOKill();
            instance.tutorialPanel.DOFade(0, 1);
        }
        else
        {
            if (instance.isTutorialPanelAppear)
            {
                instance.tutorialQueue.Enqueue(text);
            }
            else
            {
                instance.isTutorialPanelAppear = true;
                instance.tutorialPanelText.text = text;
                instance.tutorialPanel.DOKill();
                instance.tutorialPanel.DOFade(1, 1);
            }
        }
    }

    #endregion

    #region SubCamera_Events

    public static void AliceClockInput(bool value)
    {
        CanvasGroup_DefaultShow(instance.aliceClock_input, value, false);
    }

    public static void Pinocio_Branch(int index)
    {
        CanvasGroup_DefaultShow(instance.pinocio_tree_branch, true, false);
        for(int i = 0; i < instance.pinocio_tree_branches.Length; i++)
        {
            instance.pinocio_tree_branches[i].SetActive(false);
        }
        instance.pinocio_tree_branches[index].SetActive(true);
    }

    public static void Pinocio_Leaf(string text)
    {
        CanvasGroup_DefaultShow(instance.pinocio_tree_leaf, true, false);
        instance.pinocio_tree_leaf_text.text = text;
    }

    #endregion
}
