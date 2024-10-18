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
        positions = _positions;
        movetime = _moveTime;
        finishtime = _finishTime;
        starttime = _startTime;
        currentPosition = _source;
        sink = _sink;

    }

    Color CreateDarkColor(Color originalColor)
    {
        // r, g, b ���� 0~255 ������ ��ȯ
        float r = originalColor.r * 255f;
        float g = originalColor.g * 255f;
        float b = originalColor.b * 255f;

        // 0�� �ƴ� ���� -100 ����
        r = (r != 0) ? Mathf.Clamp(r - 100f, 0f, 255f) : r;
        g = (g != 0) ? Mathf.Clamp(g - 100f, 0f, 255f) : g;
        b = (b != 0) ? Mathf.Clamp(b - 100f, 0f, 255f) : b;

        // �ٽ� 0~1 ������ ������ ��ȯ �� ���ο� darkColor ����
        Color darkColor = new Color(r / 255f, g / 255f, b / 255f);

        return darkColor;
    }

    public List<Vector3> positions;
    public Vector3 sink;
    public int currentindex = 0; // ������ position�̳� time array �� ���� ������ �� ����� index ����
    public Vector3 currentPosition;     // ���� ��ġ
    public Vector3 targetPosition;      // ��ǥ ��ġ
    public Color processColor = Color.green;  // �������� ���� ����
    public Color originalColor;              // ���� ����
    public Color darkColor;
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
        positions[0] = transform.position;
        targetPosition = positions[0];
        blockrenderer = GetComponent<Renderer>();
        blockrenderer.material.color = originalColor;

        if (blockrenderer == null)
        {
            Debug.LogError("Renderer component not found on the object!");
            return;
        }

        darkColor = CreateDarkColor(originalColor);

        num_process = starttime.Length;
        delta = movetime[0];
        target_delta = movetime[1] - movetime[0];
        //target_delta = movetime[0] - 0.0f;
        // Debug.Log("Block" + blockindex + "has to wait till " + target_delta);
        SetTarget(positions[0]);

    }
    
    
    void Update()
    {
        
        if (timer >= finishtime[finishtime.Length - 1])
        {
            if (isFinished == false)
            {
                currentPosition = transform.position;
                blockrenderer.material.color = Color.black;
                moveProgress = 0.0f;
                targetPosition = sink;
            }
            
            Sink();

            isFinished = true;
            return;
        }
        // Part 2
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

        if (timer >= movetime[currentindex] && timer <= starttime[currentindex])
        {
            blockrenderer.material.color = darkColor;
            Move();
        }
        else
        {
            if (transform.position != targetPosition)
            {
                Move();
            }
            if(timer > starttime[currentindex] && timer < finishtime[currentindex])
            {
                check_arrival();
                blockrenderer.material.color = originalColor;
                //UpdateColorOverTime();
            }
            else
            {
                blockrenderer.material.color = darkColor;
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
    void Sink()
    {
        // �̵� ���� ���� (0.0 ~ 1.0) ���
        moveProgress += Time.deltaTime / movingTime;

        // ���� ��ġ���� ��ǥ ��ġ�� �̵� (Lerp ���)
        transform.position = Vector3.Lerp(currentPosition, targetPosition, moveProgress);

        // �̵� �� �α� ���
        //Debug.Log("Block " + blockindex + " is moving towards: " + targetPosition);
    }
    // ������ ��ǥ ��ġ�� 0.5�� ���� �̵��ϴ� �Լ�
    void Move()
    {
        // �̵� ���� ���� (0.0 ~ 1.0) ���
        moveProgress += Time.deltaTime / movingTime;

        // ���� ��ġ���� ��ǥ ��ġ�� �̵� (Lerp ���)
        transform.position = Vector3.Lerp(currentPosition, targetPosition, moveProgress);

        // �̵� �� �α� ���
        //Debug.Log("Block " + blockindex + " is moving towards: " + targetPosition);
    }

    // �ð��� �����鼭 ������ ���ϴ� �Լ�
    void UpdateColorOverTime()
    {
        //Debug.Log("UpdateColorOverTime called");
        colorChangeProgress += Time.deltaTime / colorChangeDuration;

        // ���� ��ȭ ���� ���¿� ���� ���� ���� (Lerp ���)
        Color currentColor = Color.Lerp(originalColor, processColor, colorChangeProgress);
        
        blockrenderer.material.color = currentColor;
    }
}
