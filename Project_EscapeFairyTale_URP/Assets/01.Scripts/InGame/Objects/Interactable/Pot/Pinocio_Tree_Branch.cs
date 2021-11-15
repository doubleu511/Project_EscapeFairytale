using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Pinocio_Tree_Branch : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private const string selectText = "클릭하여 자세히 봅니다.";
    public bool inGameBranch = true;
    public RectTransform[] branchOfBranches;
    public GameObject[] leaves;

    [HideInInspector] public UnityEngine.UI.Outline[] outline;
    [HideInInspector] public Pinocio_Tree.Pinocio_Tree_Branch_Type branch_type;
    [HideInInspector] public int myIdx;

    private int _level = 2;
    public int level
    {
        get { return _level; }
        set
        {
            _level = Mathf.Clamp(value, 0, 4);
            LevelChange(_level);
        }
    }
    private bool isHighlighted = false;

    private void Awake()
    {
        outline = GetComponentsInChildren<UnityEngine.UI.Outline>();
    }

    private void Start()
    {
        for (int i = 0; i < leaves.Length; i++)
        {
            Pinocio_Tree_Leaf leaf = leaves[i].GetComponent<Pinocio_Tree_Leaf>();

            if (leaf != null)
            {
                leaf.myBranch = this;
                leaf.myIdx = i;
            }
        }
    }

    public void LevelChange(int level)
    {
        for (int i = 0; i < branchOfBranches.Length; i++)
        {
            if (i < level)
            {
                branchOfBranches[i].DOSizeDelta(new Vector2(0, 0), 2);
            }
            else
            {
                branchOfBranches[i].DOSizeDelta(new Vector2(85, 85), 2);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Pinocio_Tree.isCleared) return;

        if (inGameBranch)
        {
            isHighlighted = true;
            foreach (UnityEngine.UI.Outline item in outline)
            {
                item.enabled = true;
            }
            UIManager.instance.cursorBtTipText.text = selectText;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Pinocio_Tree.isCleared) return;

        if (inGameBranch)
        {
            isHighlighted = false;
            foreach (UnityEngine.UI.Outline item in outline)
            {
                item.enabled = false;
            }
            UIManager.instance.cursorBtTipText.text = "";
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Pinocio_Tree.isCleared) return;

        UIManager.Pinocio_Branch(myIdx);
    }

    void Update()
    {
        if (isHighlighted)
        {
            Ray ray = UIManager.instance.subCamera.ScreenPointToRay(Input.mousePosition);
            UIManager.instance.cursorBtTipText.transform.position = UIManager.instance.subCamera.WorldToScreenPoint(ray.origin) - new Vector3(0, 50f, 0);
        }
    }
}
