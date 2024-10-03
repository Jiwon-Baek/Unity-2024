using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    // �ʱ�ȭ �޼��� (������ ���)
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

    public int currentindex = 0; // ������ position�̳� time array �� ���� ������ �� ����� index ����
    public Vector3 currentPosition;     // ���� ��ġ
    public Vector3 targetPosition;      // ��ǥ ��ġ
    public Color processColor = Color.green;  // �������� ���� ����
    public Color originalColor;              // ���� ����

    public float movingTime = 0.5f;     // �̵� �ð� (0.5��)
    private float moveProgress = 0.0f;   // �̵� ���� ���� (0.0f ~ 1.0f)
    private float colorChangeDuration;  // ������ ���ϴ� �ð�
    private float colorChangeProgress = 0.0f;  // ���� ���� ���� ���� (0.0f ~ 1.0f)

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

        if (delta > target_delta) // �� �� �� ȣ��Ǵ� �Լ�. movetime ������ ����
        {
            delta = 0.0f;
            // 0.5�� timer�� 0.5�� �����ؼ� update
            currentindex += 1; // 0���� 1�� ����
            SetTarget(positions[currentindex]);
            moveProgress = 0.0f;
            colorChangeProgress = 0.0f;
            colorChangeDuration = finishtime[currentindex] - starttime[currentindex];
            blockrenderer.material.color = originalColor;

            if (currentindex != num_process - 1) // ���� ������ process�� �ƴ϶��, 1 ������
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
        
        // Ÿ�̸� ����
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
        // ���� A�� ���� B�� ���� ��ǥ ��ġ ����
        targetPosition = newtarget;
        
        // ���� ��ġ ����
        currentPosition = transform.position;
    }

    // ������ ��ǥ ��ġ�� 0.5�� ���� �̵��ϴ� �Լ�
    void Move()
    {
        // �̵� ���� ���� (0.0 ~ 1.0) ���
        moveProgress += Time.deltaTime / movingTime;

        // ���� ��ġ���� ��ǥ ��ġ�� �̵� (Lerp ���)
        transform.position = Vector3.Lerp(currentPosition, targetPosition, moveProgress);

        // �̵� �� �α� ���
        Debug.Log("Block " + blockindex + " is moving towards: " + targetPosition);
    }

    // �ð��� �����鼭 ������ ���ϴ� �Լ�
    void UpdateColorOverTime()
    {
        Debug.Log("UpdateColorOverTime called");
        colorChangeProgress += Time.deltaTime / colorChangeDuration;

        // ���� ��ȭ ���� ���¿� ���� ���� ���� (Lerp ���)
        Color currentColor = Color.Lerp(originalColor, processColor, colorChangeProgress);
        
        blockrenderer.material.color = currentColor;
    }
}
