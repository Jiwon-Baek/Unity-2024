using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Block : MonoBehaviour
{
    public void Initialize(Color _color, int _blockIndex, 
        List<Vector3> _positions, Vector3 _source, Vector3 _sink,
        float[] _moveTime, float[] _finishTime, float[] _startTime)
    {
        originalColor = _color;
        blockindex = _blockIndex;
        positions = new List<Vector3>(_positions);// 깊은 복사로 변경 
        movetime = _moveTime;
        finishtime = _finishTime;
        starttime = _startTime;
        positions[0] = _source;
        currentPosition = _source;
        sink = _sink;

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

    public List<Vector3> positions;
    public Vector3 sink;
    public int currentindex = 0; // 앞으로 position이나 time array 의 값을 참조할 때 사용할 index 변수
    public Vector3 currentPosition;     // 현재 위치
    public Vector3 targetPosition;      // 목표 위치
    public Color processColor = Color.green;  // 진행중일 때의 색상
    public Color originalColor;              // 원래 색상
    public Color darkColor;
    public float movingTime = 0.5f;     // 이동 시간 (0.5초)
    private float moveProgress = 0.0f;   // 이동 진행 상태 (0.0f ~ 1.0f)
    private float colorChangeDuration;  // 색상이 변하는 시간
    private float colorChangeProgress = 0.0f;  // 색상 변경 진행 상태 (0.0f ~ 1.0f)

    public float[] finishtime;
    public float[] movetime;
    public float[] starttime;
    private int num_process;
    public int blockindex;
    private float timer1 = 0.0f;          
    private float timer2;
    private float target_delta;

    public bool isFinished;

    public Renderer blockrenderer;

    void SetColor(Color _color)
    {
        originalColor = _color;
    }

    void Start()
    {
        // 각 block의 완료 여부를 감지하는 변수를 초기화
        isFinished = false;


        // 처음에 (0,0,0)으로 초기화되어 있는 각각의 positions 리스트에서, 각자 처음 배정받은 source의 위치를 추가
        // Initialize()로 지정받은 처음 source 위치로 이동
        transform.position = positions[0];
        targetPosition = positions[0];

        blockrenderer = GetComponent<Renderer>();
        blockrenderer.material.color = originalColor;
        darkColor = CreateDarkColor(originalColor);

        num_process = starttime.Length; // 실제로는 GameManager에서 지정했던 numProcess보다 1 큰 값을 갖게 됨 (source가 포함되었기 때문)
        timer2 = movetime[0];
        target_delta = movetime[1] - movetime[0];

    }
    
    
    void Update()
    {
        // 작업 완료 여부를 판단
        if (timer1 >= finishtime[finishtime.Length - 1])
        {
            if (isFinished == false)
            {
                currentPosition = transform.position;
                blockrenderer.material.color = Color.black;
                moveProgress = 0.0f;
                targetPosition = sink;
            }
            
            Move();
            isFinished = true;
            return;
        }

        // 현재 시점이 index를 update하는 시점인지를 timer2를 통해 감지하고 제어
        if (timer2 > target_delta) // 단 한 번 호출되는 함수
        {
            timer2 = 0.0f;
            currentindex += 1; // 0에서 1로 변경
            moveProgress = 0.0f;
            colorChangeProgress = 0.0f;
            
            colorChangeDuration = finishtime[currentindex] - starttime[currentindex];
            blockrenderer.material.color = originalColor;

            // 다음 timer2 호출 시점을 계산
            if (currentindex != num_process - 1) // 만약 마지막 process가 아니라면
            {
                target_delta = movetime[currentindex + 1] - movetime[currentindex];
            }
            else // 만약 마지막 process라면, 다음 timer2 호출 시점은 없음
            {
                target_delta = float.MaxValue;
            }

        }

        if (timer1 >= movetime[currentindex] && timer1 <= starttime[currentindex])
        {
            // 이동하는 동안에는 darkColor를 유지
            blockrenderer.material.color = darkColor;
            Move();
        }
        else
        {
            if (transform.position != targetPosition)
            {
                // Recording 시 Move()가 한 프레임씩 덜 호출되는 문제가 있음. 
                // 이를 방지하기 위해 Move하기로 지정된 시간 이외에도 아직 target에 도달하지 못한 경우 Move()를 이어가도록 수정
                Move();
            }
            if(timer1 > starttime[currentindex] && timer1 < finishtime[currentindex])
            {
                blockrenderer.material.color = originalColor;
                //UpdateColorOverTime();
            }
            else
            {
                blockrenderer.material.color = darkColor;
            }
        }
        
        // 타이머 증가
        timer1 += Time.deltaTime;
        timer2 += Time.deltaTime;
        
    }


    // 지정된 목표 위치로 0.5초 동안 이동하는 함수
    void Move()
    {
        // 이동 진행 상태 (0.0 ~ 1.0) 계산
        moveProgress += Time.deltaTime / movingTime;

        // 현재 위치에서 목표 위치로 이동 (Lerp 사용)
        transform.position = Vector3.Lerp(currentPosition, targetPosition, moveProgress);
    }

    // 시간이 지나면서 색상이 변하는 함수
    void UpdateColorOverTime()
    {
        //Debug.Log("UpdateColorOverTime called");
        colorChangeProgress += Time.deltaTime / colorChangeDuration;

        // 색상 변화 진행 상태에 따라 색상 보간 (Lerp 사용)
        Color currentColor = Color.Lerp(originalColor, processColor, colorChangeProgress);
        
        blockrenderer.material.color = currentColor;
    }
}
