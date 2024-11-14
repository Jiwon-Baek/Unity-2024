using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class IntManager : MonoBehaviour
{
    public string text = "";
    private int intValue;
    public bool btn_active; //버튼이 활성화 상태인지 검사.
    public Text text_ui; // 시간을 표시할 text
    float time; // 시간.

    string ShowUI()
    {
        string text = string.Format("Setup:{0}",intValue);
        return text;
    }
    public void UpdateValue(int delta)
    {
        intValue += delta;
        return;
    }

    private void Start()
    {
        btn_active = true;
    }

    private void Update() // 바뀌는 시간을 text에 반영 해 줄 update 생명주기
    {
        if (btn_active)
        {
            text_ui.text = ShowUI();
        }
        else
        {
            return;
        }
        
    }
}