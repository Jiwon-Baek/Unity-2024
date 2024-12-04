using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class FloatManager : MonoBehaviour
{
    public string text = "";
    private float floatValue;
    public bool btn_active; //��ư�� Ȱ��ȭ �������� �˻�.
    public Text text_ui; // �ð��� ǥ���� text
    float time; // �ð�.
    

    string ShowUI()
    {
        string text = string.Format("Tardiness:{0:F2}", floatValue);
        return text;
    }
    public void UpdateValue(float delta)
    {
        floatValue += delta;
    }

    private void Start()
    {
        //btn_active = true;
        return;
    }
    
    private void Update() // �ٲ�� �ð��� text�� �ݿ� �� �� update �����ֱ�
    {
        if (btn_active)
        {
            text_ui.text = ShowUI();
        }
        else
        {
            text_ui.text = "";
            return;
        }
        
    }
}