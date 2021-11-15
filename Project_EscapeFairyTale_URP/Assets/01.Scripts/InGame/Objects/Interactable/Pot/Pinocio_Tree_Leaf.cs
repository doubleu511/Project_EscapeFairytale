using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Pinocio_Tree_Leaf : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private const string selectText = "클릭하여 자세히 봅니다.\n가위를 선택하고 우클릭하여 잘라냅니다.";

    [TextArea]
    public string question;

    public bool isLie = false;

    private bool isHighlighted = false;
    private Button button;
    [HideInInspector] public Pinocio_Tree_Branch myBranch;
    [HideInInspector] public int myIdx;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Start()
    {
        button.onClick.AddListener(() =>
        {
            UIManager.Pinocio_Leaf(question);
        });
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Pinocio_Tree.isCleared) return;

        isHighlighted = true;
        UIManager.instance.cursorBtTipText.text = selectText;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Pinocio_Tree.isCleared) return;

        isHighlighted = false;
        UIManager.instance.cursorBtTipText.text = "";
    }

    void Update()
    {
        if (Pinocio_Tree.isCleared) return;

        if (isHighlighted)
        {
            Ray ray = UIManager.instance.subCamera.ScreenPointToRay(Input.mousePosition);
            UIManager.instance.cursorBtTipText.transform.position = UIManager.instance.subCamera.WorldToScreenPoint(ray.origin) - new Vector3(0, 50f, 0);

            if (Input.GetMouseButtonDown(1))
            {
                const int SCISSOR_ID = 5;

                if (GameManager.Instance.selectedItemId == SCISSOR_ID)
                {
                    if (isLie)
                    {
                        myBranch.branch_type.ingame_branch.level += 1;
                        myBranch.branch_type.ui_branch.level += 1;
                    }
                    else
                    {
                        myBranch.branch_type.ingame_branch.level -= 1;
                        myBranch.branch_type.ui_branch.level -= 1;
                    }

                    Pinocio_Tree.AnswerTest();
                    myBranch.branch_type.ingame_branch.leaves[myIdx].SetActive(false);
                    myBranch.branch_type.ui_branch.leaves[myIdx].SetActive(false);
                    isHighlighted = false;
                    UIManager.instance.cursorBtTipText.text = "";

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
    }
}
