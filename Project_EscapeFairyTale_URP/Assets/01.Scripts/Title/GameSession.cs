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
    // �������� ���⼭ �񵿱� �ε�(ȭ��), ���� �ε��� �� �����Ѵ�. 

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
        "�ش� �������� ������\n������ �����ϰ� �ִٴ� ���Դϴ�.",
        "�־��� �����۰� ��Ʈ�� �� �����Ͽ�\n�� ������ Ż���غ��ʽÿ�.",
        "���� ����, ���� �� ������...",
        "�� ���� �������� �ູ�ϰ� �� ���..�����?",
        "�� ���� �� �̾߱�� ���� �᳻������ ���ϱ��?"
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
            dateTimeText.text = "���ο� ���̺����Ϸ� �����մϴ�.";
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
            // �ؽ���, ũ��, �Ǻ�, �ȼ��� ����
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
            //System.io���� ByteArr�� ���������� �о����
            t = new Texture2D(2, 2);           // ���ο� ����ִ� �ؽ��ĸ� ������ش�. 
            if (t.LoadImage(FileData))           // �̹��� �����͸� �ε��Ͽ� �ؽ��Ŀ� �ִ´�. �̶� ũ��� �̹��� ũ�⿡ ���� �ڵ����� ����.
                return t;                 // �����Ͱ� ���� �� �־��ٸ� �ش� �ؽ��ĸ� ���� LoadImage�� ������ true�� ��ȯ
        }
        return null;
    }
}
