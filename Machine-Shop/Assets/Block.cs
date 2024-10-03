using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    // 초기화 메서드 (생성자 대신)
    public void Initialize(int _blockIndex, Vector3[] _positions, float[] _moveTime, float[] _finishTime, float[] _startTime, Vector3 _currPos)
    {
        blockindex = _blockIndex;
        positions = _positions;
        movetime = _moveTime;
        finishtime = _finishTime;
        starttime = _startTime;
        currentPosition = _currPos;
    }

    public Vector3[] positions;

    public int currentindex = 0; // 앞으로 position이나 time array 의 값을 참조할 때 사용할 index 변수
    public Vector3 currentPosition;     // 현재 위치
    public Vector3 targetPosition;      // 목표 위치
    public Color processColor = Color.green;  // 진행중일 때의 색상
    public Color originalColor;              // 원래 색상

    public float movingTime = 0.5f;     // 이동 시간 (0.5초)
    private float moveProgress = 0.0f;   // 이동 진행 상태 (0.0f ~ 1.0f)
    private float colorChangeDuration;  // 색상이 변하는 시간
    private float colorChangeProgress = 0.0f;  // 색상 변경 진행 상태 (0.0f ~ 1.0f)

    public float[] finishtime;
    public float[] movetime;
    public float[] starttime;
    private int num_process;
    public int blockindex;
    private float timer = 0.0f;          
    private float delta;
    private float target_delta;

    public bool isFinished;

    public Renderer blockrenderer;

    void SetColor(Color _color)
    {
        originalColor = _color;
    }

    //void SetPositions(Vector3[] _positions)
    //{
    //    positions = _positions;
    //}

    //void SetTimeVariables(float[] _movetime, float[] _finishtime, float[] _starttime)
    //{
    //    movetime = _movetime;
    //    finishtime = _finishtime;
    //    starttime = _starttime;
    //}

    void Start()
    {
        isFinished = false;
        transform.position = currentPosition;
        targetPosition = positions[currentindex];
        blockrenderer = GetComponent<Renderer>();
        originalColor = blockrenderer.material.color;
        num_process = starttime.Length;
        delta = movetime[0];
        target_delta = movetime[1] - movetime[0];
        Debug.Log("Block" + blockindex + "has to wait till " + target_delta);
        colorChangeDuration = finishtime[0] - starttime[0];
        SetTarget(positions[0]);

    }
    
    
    void Update()
    {
        
        if (timer >= finishtime[finishtime.Length - 1])
        {
            isFinished = true;
            blockrenderer.material.color = Color.black;
            return;
        }

        if (delta > target_delta) // 단 한 번 호출되는 함수. movetime 도달을 제어
        {
            delta = 0.0f;
            // 0.5에 timer로 0.5를 감지해서 update
            currentindex += 1; // 0에서 1로 변경
            SetTarget(positions[currentindex]);
            moveProgress = 0.0f;
            colorChangeProgress = 0.0f;
            colorChangeDuration = finishtime[currentindex] - starttime[currentindex];
            blockrenderer.material.color = originalColor;

            if (currentindex != num_process - 1) // 만약 마지막 process가 아니라면, 1 더해줌
            {
                target_delta = movetime[currentindex + 1] - movetime[currentindex];
            }
            else
            {
                target_delta = float.MaxValue;
            }

        }

        if (timer >= movetime[currentindex] && timer < starttime[currentindex])
        {
            
            Move();
        }
        else
        {
            if(timer >= starttime[currentindex] && timer < finishtime[currentindex])
            {
                check_arrival();
                UpdateColorOverTime();
            }
            else
            {
                blockrenderer.material.color = Color.gray;
            }
        }
        
        // 타이머 증가
        timer += Time.deltaTime;
        delta += Time.deltaTime;
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

    // 지정된 목표 위치로 0.5초 동안 이동하는 함수
    void Move()
    {
        // 이동 진행 상태 (0.0 ~ 1.0) 계산
        moveProgress += Time.deltaTime / movingTime;

        // 현재 위치에서 목표 위치로 이동 (Lerp 사용)
        transform.position = Vector3.Lerp(currentPosition, targetPosition, moveProgress);

        // 이동 중 로그 출력
        Debug.Log("Block " + blockindex + " is moving towards: " + targetPosition);
    }

    // 시간이 지나면서 색상이 변하는 함수
    void UpdateColorOverTime()
    {
        Debug.Log("UpdateColorOverTime called");
        colorChangeProgress += Time.deltaTime / colorChangeDuration;

        // 색상 변화 진행 상태에 따라 색상 보간 (Lerp 사용)
        Color currentColor = Color.Lerp(originalColor, processColor, colorChangeProgress);
        
        blockrenderer.material.color = currentColor;
    }
}
