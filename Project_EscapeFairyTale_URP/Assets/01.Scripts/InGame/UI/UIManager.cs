using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;

    // UIManager는 싱글톤을 사용하고, static 함수들을 이용하여 instance 안에있는 변수를 함수에 사용하는 식으로
    // UIManager.instance를 사용하지 않아도 간단히 사용할수 있게 시도해보았다.
    // 모든 UI 애니메이션을 모았다.

    [Header("InGame")]
    public CanvasGroup inGameCanvasGroup;
    public CanvasGroup blackScreenCanvasGroup;
    public Text blackScreenEndText;

    [Header("RightBottomTip")]
    [Header("Tips")]
    [Space(10)]
    public CanvasGroup tip_rb;
    public Image tip_rb_Img;
    public Text tip_rb_Txt;

    [Header("SaveTip")]
    public CanvasGroup saveRing;

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
    public CanvasGroup rope_panel;

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

    [Header("BirdFind")]
    public CanvasGroup birdCanvasGroup;
    public Text birdRemainTxt;
    private int _birdRemain = 0;
    public int birdRemain {
        get { return _birdRemain; }
        set
        {
            _birdRemain = value;
            birdRemainTxt.text = $"{_birdRemain} / 7";
            if(_birdRemain == 7)
            {
                GameManager.Instance.player.playerState = PlayerState.ENDING;
                CanvasGroup_DefaultShow(blackScreenCanvasGroup, true, true, 2);
                blackScreenEndText.text = "<size=100>모든 새를 모으셨군요!\n축하드립니다.\n게임을 플레이 해주셔서 감사합니다!</size>";

                DOTween.To(() => AudioListener.volume, value => AudioListener.volume = value, 0, 7);

                blackScreenEndText.DOFade(1, 4).SetDelay(4).OnStart(() =>
                {
                    blackScreenEndText.color = new Color(1, 1, 1, 0);
                }).OnComplete(() =>
                {
                    
                    blackScreenEndText.DOFade(0, 7).SetDelay(3).OnComplete(() =>
                    {
                        MouseEvent.MouseLock(false);
                        SceneManager.LoadScene("Title");
                    });
                });
                //끝
            }
        }
    }

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

    // 팁 UI - 오른쪽 아래에 등장
    public static void Tip_RBAppear(Sprite sprite, string text, float appearTime, float waitTime, float disappearTime)
    {
        instance.tip_rb.DOComplete();
        instance.tip_rb.alpha = 0;
        instance.tip_rb.DOFade(1, appearTime);
        instance.tip_rb_Img.sprite = sprite;
        instance.tip_rb_Txt.text = text;

        instance.tip_rb.DOFade(0, disappearTime).SetDelay(waitTime);
    }


    // 팁 UI - 플레이어의 스케일이 바뀔때 아래쪽 중앙에 등장
    public static void Tip_SizeChange(int index)
    {
        instance.tip_size.DOComplete();
        instance.tip_size.alpha = 0;
        instance.tip_size.DOFade(1, 0.1f);
        instance.tip_size_arrow.DOMoveX(instance.tip_size_signs[index].transform.position.x, 1.5f).SetEase(Ease.OutBounce);

        instance.tip_size.DOFade(0, 0.5f).SetDelay(3f);
    }

    // 게임 오버 UI - 게임 오버되었을때 화면에 띄울 스프라이트를 매개변수로 두고 호출
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

    // 기본적인 캔버스 애니메이션 함수 - 오버로딩하여 다양하게 쓸 수 있게 해준다.
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

    public static void CanvasGroup_DefaultShow(CanvasGroup group, bool show, bool animatedsetUpdate, float time)
    {
        group.DOComplete();
        group.DOFade(show ? 1 : 0, time).SetUpdate(animatedsetUpdate);

        group.blocksRaycasts = show;
        group.interactable = show;
    }

    #region LetterUI

    // 쪽지 UI - 스프라이트만을 사용할 수 있고, 폰트와 텍스트를 추가하여 인스펙터에서 직접 쓸 수 있게 오버로딩도 하였다
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

    // 책 UI - 책 넘기기, 닫기 등의 상호작용이 가능하도록 버튼에 이벤트를 추가
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

    // 플레이어 시점인 메인 카메라에서 -> 시야를고정하고 자세히 탐색하는 서브 카메라로 옮기는 함수
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

    // 메인 카메라를 리턴한다.
    public static Camera GetMainCam()
    {
        return instance.mainCamera;
    }

    // 서브 카메라에서 메인 카메라로 옮긴다.
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

    // 일시정지 UI - value에 따라 타임스케일을 0으로 해준다.
    public static void PauseUI(bool value)
    {
        GameManager.Instance.player.playerState = value ? PlayerState.PAUSED : PlayerState.NORMAL;

        if (!GameManager.Instance.player.isSubCam)
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

    // 확인 UI - yes / no 응답한것에 따라 델리게이트(이벤트) 함수를 집어넣어 다음 할 함수를 매개변수로 한다.
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
