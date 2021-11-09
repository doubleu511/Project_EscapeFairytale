using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSession : MonoBehaviour
{
    public Image screenShot;
    public Text dateTimeText;
    public SpriteBox spriteBox;

    public Button newGameButton;
    public Button loadGameButton;

    // Start is called before the first frame update
    void Start()
    {
        bool isHaveFile = SecurityPlayerPrefs.GetBool("saved-file-exists", false);

        if(!isHaveFile)
        {
            loadGameButton.interactable = false;
        }

        newGameButton.onClick.AddListener(() =>
        {
            screenShot.sprite = spriteBox.NewGame;
            dateTimeText.text = "새로운 세이브파일로 시작합니다.";
        });

        loadGameButton.onClick.AddListener(() =>
        {

        });

        Debug.Log(DateTime.Now);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
