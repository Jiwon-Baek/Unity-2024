using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TimerManager : MonoBehaviour
{
    public string m_Timer = @"00:00:00.000";

    public bool btn_active; //��ư�� Ȱ��ȭ �������� �˻�.
    public Text text_time; // �ð��� ǥ���� text
    public float m_TotalSeconds; // ���� �ð��� ���� �̺�Ʈ�� �߻��Ϸ���, �� ���� ����ϸ� �˴ϴ�. 
    public float endtime;
    float time; // �ð�.

    string StopwatchTimer()
    {
        m_TotalSeconds += Time.deltaTime;
        TimeSpan timespan = TimeSpan.FromSeconds(m_TotalSeconds);
        string timer = string.Format("{0:00}:{1:00}.{2:00}",
            timespan.Minutes, timespan.Seconds, timespan.Milliseconds);

        return timer;
    }

    private void Start()
    {
        m_TotalSeconds = 0.0f;
        btn_active = true; //��ư�� �ʱ� ���¸� false�� ����
        // SetTimerOn();
    }
    
    //public void SetTimerOn()
    //{ // ��ư Ȱ��ȭ �޼ҵ�
    //    btn_active = true;
    //}

    //public void SetTimerOff()
    //{ // ��ư ��Ȱ��ȭ �޼ҵ�
    //    btn_active = false;
    //}

    private void Update() // �ٲ�� �ð��� text�� �ݿ� �� �� update �����ֱ�
    {
        if (btn_active)
        {
            time += Time.deltaTime;
            text_time.text = StopwatchTimer();
        }
        else
        {
            return;
        }
        
    }
}