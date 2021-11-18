using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public enum EndType
{
    HAPPY,
    NORMAL,
    BAD
}

public class EndingDoor : MonoBehaviour
{
    public EnemyAI ai;
    private bool isTrigger = false;

    public EndType type;

    public GameObject[] DisappearBird;
    public GameObject AppearBird;

    public ItemPlacer pib;
    public ItemPlacer alice;

    private bool pib_bad = false;
    private bool alice_bad = false;

    public Sprite[] cin_ending;
    public Sprite[] alice_ending;
    public Sprite[] pib_ending; // 0은 해피, 1은 배드로 한다.
    public Sprite theEnd;
    private List<Sprite> endingBook = new List<Sprite>();
    private Sprite[] endingBookSpr;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isTrigger)
            {
                GameManager.Instance.player.playerState = PlayerState.ENDING;
                ai.gameObject.SetActive(false);
                isTrigger = true;

                UIManager.CanvasGroup_DefaultShow(UIManager.instance.blackScreenCanvasGroup, true, true, 2);
                UIManager.instance.bookPageCloseBtn.onClick.AddListener(Ending);
                StartCoroutine(BookAppear());
            }
        }
    }

    IEnumerator BookAppear()
    {
        EndingCheck();
        yield return new WaitForSecondsRealtime(3);
        UIManager.BookDetailUI(endingBookSpr, Color.red);
        MouseEvent.MouseLock(false);
    }

    private void EndingCheck()
    {
        // 이전에 엔딩 타입을 검사한다
        if (!Cinderella_Clock_TrueButton.cin_bad)
        {
            endingBook.Add(cin_ending[0]);
        }
        else
        {
            endingBook.Add(cin_ending[1]);
        }

        if (pib.GetItemCount() <= 0)
        {
            endingBook.Add(pib_ending[1]);
            pib_bad = true;
        }
        else
        {
            endingBook.Add(pib_ending[0]);
        }

        if (alice.GetItemCount() <= 1)
        {
            endingBook.Add(alice_ending[1]);
            alice_bad = true;
        }
        else
        {
            endingBook.Add(alice_ending[0]);
        }
        endingBook.Add(theEnd);

        endingBookSpr = endingBook.ToArray();
    }

    private void Ending()
    {
        if(!pib_bad && !alice_bad && !Cinderella_Clock_TrueButton.cin_bad)
        {
            type = EndType.HAPPY;
        }
        else if (pib_bad && alice_bad && Cinderella_Clock_TrueButton.cin_bad)
        {
            type = EndType.BAD;
        }
        else
        {
            type = EndType.NORMAL;
        }

        Text endText = UIManager.instance.blackScreenEndText;
        CanvasGroup blackScreen = UIManager.instance.blackScreenCanvasGroup;

        if (type == EndType.HAPPY)
        {
            endText.text = "The Happy End";
        }
        else if (type == EndType.NORMAL)
        {
            endText.text = "Normal End";
        }
        else
        {
            endText.text = "<color=\"#ff0000\">The Bad End</color>";
        }

        endText.DOFade(1, 6).SetDelay(4).OnComplete(() =>
        {
            GameManager.Instance.player.transform.position = new Vector3(7.868f, -5.75f, 1.577f);
            endText.DOFade(0, 5).SetDelay(3).OnComplete(() =>
            {
                if(type == EndType.HAPPY)
                {
                    UIManager.CanvasGroup_DefaultShow(blackScreen, false, true, 2);
                    // 버드 시작
                    MouseEvent.MouseLock(true);
                    foreach(GameObject item in DisappearBird)
                    {
                        item.SetActive(false);
                    }
                    AppearBird.SetActive(true);
                    UIManager.CanvasGroup_DefaultShow(UIManager.instance.birdCanvasGroup, true, true);
                    GameManager.Instance.player.playerState = PlayerState.NORMAL;
                    UIManager.TutorialPanel("미니게임 시작! 집에 숨겨진 7마리의 새를 찾아보세요.");
                }
                else
                {
                    SceneManager.LoadScene("Title");
                }
            });
        });
    }
}
