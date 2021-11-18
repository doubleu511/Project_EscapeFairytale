using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Pinocio_Tree : MonoBehaviour, ISaveAble
{
    private static Pinocio_Tree instance;

    [System.Serializable]
    public struct Pinocio_Tree_Branch_Type
    {
        public Pinocio_Tree_Branch ui_branch;
        public Pinocio_Tree_Branch ingame_branch;
    }

    public int[] answer;
    public Pinocio_Tree_Branch_Type[] branches;
    public CanvasGroup deco;
    public RectTransform key;
    public static bool isCleared = false;
    public static bool isKeyAppear = false;

    [Header("Save")]
    public string saveKey;
    private string _eventFlow = "haskey";
    public static string eventFlow
    {
        get { return instance._eventFlow; }
        set
        {
            if (instance._eventFlow != value)
            {
                instance._eventFlow = value;
                instance.TempSave();
            }
        }
    } // haskey이면 나무가 키를 가지고있음, hasnotkey이면 안가지고 있음

    private void Awake()
    {
        instance = this;

        for (int i = 0; i < branches.Length; i++)
        {
            branches[i].ui_branch.branch_type = branches[i];
            branches[i].ingame_branch.branch_type = branches[i];
            branches[i].ui_branch.myIdx = i;
            branches[i].ingame_branch.myIdx = i;
        }
    }

    private void Start()
    {
        if (!saveKey.Equals(""))
        {
            if (!GameManager.saveDic.ContainsKey(saveKey))
            {
                GameManager.saveDic.Add(saveKey, eventFlow);
            }
            else
            {
                Load();
            }
        }
    }

    public static void AnswerTest()
    {
        for(int i = 0; i<instance.branches.Length;i++)
        {
            if(instance.branches[i].ingame_branch.level != instance.answer[i])
            {
                return;
            }
        }

        // 정답
        isCleared = true;
        UIManager.CanvasGroup_DefaultShow(UIManager.instance.pinocio_tree_branch, false);
        UIManager.CanvasGroup_DefaultShow(UIManager.instance.pinocio_tree_leaf, false);
        UIManager.instance.pinocio_tree_branch_backBtn.gameObject.SetActive(false);
        UIManager.instance.pinocio_tree_leaf_backBtn.gameObject.SetActive(false);
        UIManager.instance.pinocio_tree_backBtn.interactable = false;

        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(3);
        seq.Append(instance.deco.DOFade(0, 2).OnComplete(() =>
        {
            isKeyAppear = true;
        }));
        seq.AppendCallback(() =>
        {
            GameManager.PlaySFX(GameManager.Instance.audioBox.object_tree_break);
        });

        for (int i = 0; i < instance.branches.Length; i++)
        {
            foreach(UnityEngine.UI.Outline item in instance.branches[i].ingame_branch.outline)
            {
                item.enabled = false;
            }

            for (int j = 0; j< instance.branches[i].ingame_branch.leaves.Length;j++)
            {
                if(instance.branches[i].ingame_branch.leaves[j].activeInHierarchy)
                {
                    seq.Join(instance.branches[i].ingame_branch.leaves[j].GetComponent<Image>().DOFade(0, 2));
                }
            }
        }
        seq.AppendInterval(2);
        seq.Append(instance.key.DOAnchorPos(new Vector2(-100, 0), 1).SetRelative().OnComplete(() =>
        {
            UIManager.instance.pinocio_tree_backBtn.interactable = true;
        }));
    }

    public static void PourMilk()
    {
        if(!isCleared)
        {
            GameManager.Instance.inventoryManager.DecreaseTab(GameManager.Instance.selectedTab.tabId);
            GameManager.Instance.CreateMilkTest();

            for (int i = 0; i < instance.branches.Length; i++)
            {
                instance.branches[i].ingame_branch.level = 2;
                instance.branches[i].ui_branch.level = 2;

                for (int j = 0; j < instance.branches[i].ingame_branch.leaves.Length; j++)
                {
                    instance.branches[i].ingame_branch.leaves[j].SetActive(true);
                }

                for (int j = 0; j < instance.branches[i].ui_branch.leaves.Length; j++)
                {
                    instance.branches[i].ui_branch.leaves[j].SetActive(true);
                }

                int random = Random.Range(0, 2);
                if (random == 0)
                {
                    GameManager.PlaySFX(GameManager.Instance.audioBox.object_tree_grow1);
                }
                else
                {
                    GameManager.PlaySFX(GameManager.Instance.audioBox.object_tree_grow2);
                }
            }
        }
    }

    public void TempSave()
    {
        if (saveKey != "")
        {
            GameManager.saveDic[saveKey] = eventFlow;
        }
    }

    public void Load()
    {
        eventFlow = GameManager.saveDic[saveKey];
        if (eventFlow.Equals("hasnotkey"))
        {
            isCleared = true;
            isKeyAppear = true;
            deco.alpha = 0;
            key.gameObject.SetActive(false);
            Pinocio_Key._plant.SetActive(false);
            for (int i = 0; i < branches.Length; i++)
            {
                for (int j = 0; j < branches[i].ingame_branch.leaves.Length; j++)
                {
                    branches[i].ingame_branch.leaves[j].SetActive(false);
                }
            }
        }
    }
}