using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RopePanel : MonoBehaviour
{
    public Image keyboardImg;
    public RectTransform keyboardValue;
    public Vector2 keyboardTargetValue;

    public Image mouseImg;
    public RectTransform mouseValue;
    public Vector2 mouseTargetValue;

    public UnityEvent clearEvent;

    private bool isPlaying = false;
    private Coroutine spriteChange;
    private Coroutine valueChange;

    private float _keyboard_value = 0;
    public float keyboard_value
    {
        get { return _keyboard_value; }
        set
        {
            _keyboard_value = Mathf.Clamp(value, 0, 1);
        }
    }

    private float _mouse_value = 0;
    public float mouse_value
    {
        get { return _mouse_value; }
        set
        {
            _mouse_value = Mathf.Clamp(value, 0, 1);
        }
    }

    private void Start()
    {
        RopeStart();
    }

    public void RopeStart()
    {
        isPlaying = true;
        spriteChange = StartCoroutine(SpriteChange());
        valueChange = StartCoroutine(ValueChange());
    }

    public void RopeEnd()
    {
        isPlaying = false;
        StopCoroutine(spriteChange);
        StopCoroutine(valueChange);

        keyboard_value = 0;
        mouse_value = 0;
        SizeUpdate();
    }

    public void RopeClear()
    {
        isPlaying = false;
        StopCoroutine(spriteChange);
        StopCoroutine(valueChange);

        UIManager.CanvasGroup_DefaultShow(UIManager.instance.rope_panel, true, false);
        clearEvent.Invoke();
    }

    private void Update()
    {
        if(isPlaying)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                keyboard_value += 0.04f;
                SizeUpdate();
            }

            if(Input.GetMouseButtonDown(0))
            {
                mouse_value += 0.04f;
                SizeUpdate();
            }
        }
    }

    IEnumerator ValueChange()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.02f);
            keyboard_value -= 0.005f;
            mouse_value -= 0.005f;
            SizeUpdate();
        }
    }

    private void SizeUpdate()
    {
        keyboardValue.transform.localScale = new Vector3(keyboard_value, 1, 1);
        mouseValue.transform.localScale = new Vector3(mouse_value, 1, 1);

        if (keyboard_value >= keyboardTargetValue.x && keyboard_value <= keyboardTargetValue.y)
        {
            if (mouse_value >= mouseTargetValue.x && mouse_value <= mouseTargetValue.y)
            {
                RopeEnd();
            }
        }
    }

    IEnumerator SpriteChange()
    {
        while (true)
        {
            keyboardImg.sprite = GameManager.Instance.spriteBox.UI_Keyboard;
            mouseImg.sprite = GameManager.Instance.spriteBox.UI_Mouse;
            yield return new WaitForSeconds(0.1f);
            keyboardImg.sprite = GameManager.Instance.spriteBox.UI_Keyboard_Click;
            mouseImg.sprite = GameManager.Instance.spriteBox.UI_Mouse_Click;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
