using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TimerManager : MonoBehaviour
{
    public string m_Timer = @"00:00:00.000";

    public bool btn_active; //버튼이 활성화 상태인지 검사.
    public Text text_time; // 시간을 표시할 text
    public float m_TotalSeconds; // 만약 시간에 따라서 이벤트를 발생하려면, 이 값을 사용하면 됩니다. 
    public float endtime;
    float time; // 시간.

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
        btn_active = true; //버튼의 초기 상태를 false로 만듦
        // SetTimerOn();
    }
    
    //public void SetTimerOn()
    //{ // 버튼 활성화 메소드
    //    btn_active = true;
    //}

    //public void SetTimerOff()
    //{ // 버튼 비활성화 메소드
    //    btn_active = false;
    //}

    private void Update() // 바뀌는 시간을 text에 반영 해 줄 update 생명주기
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