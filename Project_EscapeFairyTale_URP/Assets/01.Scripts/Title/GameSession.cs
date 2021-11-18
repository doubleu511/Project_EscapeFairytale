using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    // 귀찮으니 여기서 비동기 로딩(화면), 스샷 로딩을 다 구현한다. 

    private Sprite spr;
    private bool isNewGame = true;

    public Image screenShot;
    public Text dateTimeText;
    public SpriteBox spriteBox;

    public Button newGameButton;
    public Button loadGameButton;
    public Button gameStartButton;

    public AudioSource bgmSource;
    public CanvasGroup blackPanel;
    public CanvasGroup loadingTip;
    public Text loadingText;
    private string[] tips = new string[5]
    {
        "해당 아이콘이 나오면\n게임을 저장하고 있다는 뜻입니다.",
        "주어진 아이템과 힌트를 잘 조합하여\n이 집에서 탈출해보십시오.",
        "옛날 옛날, 아주 먼 옛날에...",
        "그 이후 오래오래 행복하게 잘 살았..을까요?",
        "이 게임 속 이야기는 누가 써내려가는 것일까요?"
    };

    // Start is called before the first frame update
    void Start()
    {
        bool isHaveFile = SecurityPlayerPrefs.GetBool("saved-file-exists", false);
        string path = Application.dataPath;
        path = Path.Combine(path, "../sc/saved.png");
        if (File.Exists(path))
        {
            spr = LoadSprite(path);
        }

        if (!isHaveFile)
        {
            loadGameButton.interactable = false;
        }

        newGameButton.onClick.AddListener(() =>
        {
            isNewGame = true;
            screenShot.sprite = spriteBox.NewGame;
            screenShot.color = Color.white;
            dateTimeText.text = "새로운 세이브파일로 시작합니다.";
        });

        loadGameButton.onClick.AddListener(() =>
        {
            isNewGame = false;
            if (isHaveFile)
            {
                dateTimeText.text = SecurityPlayerPrefs.GetString("saved-dateTime", "");

                if (File.Exists(path))
                {
                    screenShot.sprite = spr;
                    screenShot.color = Color.white;
                }
                else
                {
                    screenShot.sprite = null;
                    screenShot.color = Color.black;
                }
            }
        });

        gameStartButton.onClick.AddListener(() =>
        {
            newGameButton.onClick.RemoveAllListeners();
            loadGameButton.onClick.RemoveAllListeners();
            TitleManager.PlaySFX(TitleManager.Instance.audioBox.ui_game_start, 1f);

            if(isNewGame)
            {
                GameManager.DataReset();
            }


            blackPanel.blocksRaycasts = true;
            blackPanel.interactable = true;
            blackPanel.DOFade(1, 2).SetDelay(0.5f).OnComplete(() =>
            {
                StartCoroutine(MoveScene("NewMap"));
            });

            bgmSource.DOFade(0, 3);
        });
    }

    IEnumerator MoveScene(string moveSceneName)
    {
        yield return new WaitForSeconds(2);
        loadingText.text = tips[UnityEngine.Random.Range(0, tips.Length)];
        loadingTip.DOFade(1, 0.75f);
        yield return new WaitForSeconds(2);
        AsyncOperation async = SceneManager.LoadSceneAsync(moveSceneName);
        async.allowSceneActivation = false;

        while (!(async.isDone))
        {
            float progress = async.progress * 100.0f;

            if (progress >= 0.9f)
            {
                yield return new WaitForSeconds(1.5f);
                async.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    private static Sprite LoadSprite(string path, float pixelPerUnit = 100.0f)
    {
        Texture2D t = LoadTexture(path);
        if (t != null)
        {
            Sprite s = Sprite.Create(t, new Rect(0, 0, t.width, t.height), new Vector2(0.5f, 0.5f), pixelPerUnit);
            // 텍스쳐, 크기, 피봇, 픽셀퍼 유닛
            return s;
        }
        return null;
    }

    public static Texture2D LoadTexture(string path)
    {
        Texture2D t;
        byte[] FileData;

        if (File.Exists(path))
        {
            FileData = File.ReadAllBytes(path);
            //System.io에서 ByteArr로 파일정보를 읽어오고
            t = new Texture2D(2, 2);           // 새로운 비어있는 텍스쳐를 만들어준다. 
            if (t.LoadImage(FileData))           // 이미지 데이터를 로드하여 텍스쳐에 넣는다. 이때 크기는 이미지 크기에 맞춰 자동으로 들어간다.
                return t;                 // 데이터가 읽을 수 있었다면 해당 텍스쳐를 리턴 LoadImage는 성공시 true를 반환
        }
        return null;
    }
}
