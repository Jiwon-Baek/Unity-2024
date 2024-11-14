using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SingleJobBlock : MonoBehaviour
{
    public void Initialize(Color _color, int _blockIndex, 
        Vector3 _source, Vector3 _position, Vector3 _sink, float[] timetable, int _tardLevel)
    {
        originalColor = _color;
        blockindex = _blockIndex;

        T_created = timetable[0];
        T_move = timetable[1];
        T_setup = timetable[2];
        T_start = timetable[3];
        T_finish = timetable[4];
        T_end = timetable[4]+0.5f;
        currentPosition = _source;
        source = _source;
        position = _position;
        sink = _sink;
        tardLevel = _tardLevel;
    }

    public void SetSink(Vector3 newsink)
    {
        sink = newsink;
    }

    Color CreateDarkColor(Color originalColor)
    {
        // r, g, b 값을 0~255 범위로 변환
        float r = originalColor.r * 255f;
        float g = originalColor.g * 255f;
        float b = originalColor.b * 255f;

        // 0이 아닌 값에 -100 적용
        r = (r != 0) ? Mathf.Clamp(r - 100f, 0f, 255f) : r;
        g = (g != 0) ? Mathf.Clamp(g - 100f, 0f, 255f) : g;
        b = (b != 0) ? Mathf.Clamp(b - 100f, 0f, 255f) : b;

        // 다시 0~1 사이의 값으로 변환 후 새로운 darkColor 생성
        Color darkColor = new Color(r / 255f, g / 255f, b / 255f);

        return darkColor;
    }
    GameObject block;
    public Vector3 sink;
    public Vector3 source;
    public Vector3 position;

    public int currentindex = 0; // 앞으로 position이나 time array 의 값을 참조할 때 사용할 index 변수
    public Vector3 currentPosition;     // 현재 위치
    public Vector3 targetPosition;      // 목표 위치
    //public Color processColor = Color.green;  // 진행중일 때의 색상
    public Color originalColor;              // 원래 색상
    public Color transparentColor;              // 원래 색상
    public Color darkColor;

    public float movingTime = 2.0f;     // 이동 시간 (0.5초)
    private float moveProgress = 0.0f;   // 이동 진행 상태 (0.0f ~ 1.0f)
    // private float colorchangeduration;  // 색상이 변하는 시간
    // private float colorchangeprogress = 0.0f;  // 색상 변경 진행 상태 (0.0f ~ 1.0f)

    private int tardLevel;
    private float T_created;
    private float T_move;
    private float T_setup;
    private float T_start;
    private float T_finish;
    private float T_end;
    public bool isSinkUpdated;
    public int blockindex;

    private float timer1 = 0.0f;          
    private float timer2;

    public bool isFinished;

    public Renderer blockrenderer;

    void SetColor(Color _color)
    {
        originalColor = _color;
    }

    void Start()
    {
        // originalColor.a = 0.0f;

        isSinkUpdated = false;
        isFinished = false;
        transform.position = currentPosition;
        targetPosition = source;
        transparentColor = originalColor;
        transparentColor.a = 0.0f;
        blockrenderer = GetComponent<Renderer>();
        // blockrenderer.material.color = originalColor;
        blockrenderer.material.color = transparentColor;
        if (blockrenderer == null)
        {
            Debug.LogError("Renderer component not found on the object!");
            return;
        }
        darkColor = CreateDarkColor(originalColor);
        SetTarget(position);

    }

    void Update()
    {
        
        if (isFinished)
        {
            // tardLevel 값에 따라 색상 밝기를 조정
            float intensity = Mathf.Clamp01(tardLevel / 5.0f); // 0~5의 값을 0.0~1.0 사이로 변환
            Debug.Log("Color of "+blockindex+": \t"+ tardLevel);
            Color tardColor = new Color(1.0f * intensity, 0.0f, 0.0f, 1.0f); // R 값에만 intensity를 곱하고 알파를 1로 설정

            blockrenderer.material.color = tardColor; // 색상 설정
            Sink();
            return;
        }

        if (timer1 >= T_created)
        {
            blockrenderer.material.color = originalColor;
            if (timer1 >= T_move && timer1 <= T_setup)
            {

                SetTarget(position);
                blockrenderer.material.color = darkColor;
                Move();
            }
            else if (timer1 >= T_setup && timer1 <= T_start)
            {
                if (transform.position != targetPosition)
                {
                    Move();
                }
                // Setup
                blockrenderer.material.color = darkColor;
            }
            else if (timer1 >= T_start && timer1 <= T_finish)
            {
                // Progress
                blockrenderer.material.color = originalColor;
            }
            else if (timer1 > T_finish)
            {

                // move to Sink
                currentPosition = transform.position;
                blockrenderer.material.color = Color.black;
                isFinished = true;
                moveProgress = 0.0f;
                targetPosition = sink;
                Sink();
            }
            else
            {
                // wait
            }

        }
        else
        {

        }

        // 타이머 증가
        timer1 += Time.deltaTime;
        // timer2 += Time.deltaTime;
        // Debug.Log("Time:"+timer+"| colorprogress: "+colorChangeProgress+ "| colorduration: " + colorChangeDuration);
        
    }

    void check_arrival()
    {
        if (transform.position == targetPosition)
        {
            currentPosition = transform.position;
        }
    }
    void SetTarget(Vector3 newtarget)
    {
        // 라인 A와 라인 B에 따라 목표 위치 설정
        targetPosition = newtarget;
        
        // 현재 위치 갱신
        currentPosition = transform.position;
    }
    void Sink()
    {
        // 이동 진행 상태 (0.0 ~ 1.0) 계산
        moveProgress += Time.deltaTime / movingTime;

        // 현재 위치에서 목표 위치로 이동 (Lerp 사용)
        transform.position = Vector3.Lerp(currentPosition, sink, moveProgress);

        // 이동 중 로그 출력
        //Debug.Log("Block " + blockindex + " is moving towards: " + targetPosition);
    }
    // 지정된 목표 위치로 0.5초 동안 이동하는 함수
    void Move()
    {
        // 이동 진행 상태 (0.0 ~ 1.0) 계산
        moveProgress += Time.deltaTime / movingTime;

        // 현재 위치에서 목표 위치로 이동 (Lerp 사용)
        transform.position = Vector3.Lerp(currentPosition, targetPosition, moveProgress);

        // 이동 중 로그 출력
        //Debug.Log("Block " + blockindex + " is moving towards: " + targetPosition);
    }

    // 시간이 지나면서 색상이 변하는 함수
    //void UpdateColorOverTime()
    //{
    //    //Debug.Log("UpdateColorOverTime called");
    //    colorChangeProgress += Time.deltaTime / colorChangeDuration;

    //    // 색상 변화 진행 상태에 따라 색상 보간 (Lerp 사용)
    //    Color currentColor = Color.Lerp(originalColor, processColor, colorChangeProgress);
        
    //    blockrenderer.material.color = currentColor;
    //}
}
