using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class IntManager : MonoBehaviour
{
    public string text = "";
    private int intValue;
    public bool btn_active; //��ư�� Ȱ��ȭ �������� �˻�.
    public Text text_ui; // �ð��� ǥ���� text
    float time; // �ð�.

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

    private void Update() // �ٲ�� �ð��� text�� �ݿ� �� �� update �����ֱ�
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