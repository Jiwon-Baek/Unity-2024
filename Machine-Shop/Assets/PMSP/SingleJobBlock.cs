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
    GameObject block;
    public Vector3 sink;
    public Vector3 source;
    public Vector3 position;

    public int currentindex = 0; // ������ position�̳� time array �� ���� ������ �� ����� index ����
    public Vector3 currentPosition;     // ���� ��ġ
    public Vector3 targetPosition;      // ��ǥ ��ġ
    //public Color processColor = Color.green;  // �������� ���� ����
    public Color originalColor;              // ���� ����
    public Color transparentColor;              // ���� ����
    public Color darkColor;

    public float movingTime = 2.0f;     // �̵� �ð� (0.5��)
    private float moveProgress = 0.0f;   // �̵� ���� ���� (0.0f ~ 1.0f)
    // private float colorchangeduration;  // ������ ���ϴ� �ð�
    // private float colorchangeprogress = 0.0f;  // ���� ���� ���� ���� (0.0f ~ 1.0f)

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
            // tardLevel ���� ���� ���� ��⸦ ����
            float intensity = Mathf.Clamp01(tardLevel / 5.0f); // 0~5�� ���� 0.0~1.0 ���̷� ��ȯ
            Debug.Log("Color of "+blockindex+": \t"+ tardLevel);
            Color tardColor = new Color(1.0f * intensity, 0.0f, 0.0f, 1.0f); // R ������ intensity�� ���ϰ� ���ĸ� 1�� ����

            blockrenderer.material.color = tardColor; // ���� ����
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

        // Ÿ�̸� ����
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
        transform.position = Vector3.Lerp(currentPosition, sink, moveProgress);

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
    //void UpdateColorOverTime()
    //{
    //    //Debug.Log("UpdateColorOverTime called");
    //    colorChangeProgress += Time.deltaTime / colorChangeDuration;

    //    // ���� ��ȭ ���� ���¿� ���� ���� ���� (Lerp ���)
    //    Color currentColor = Color.Lerp(originalColor, processColor, colorChangeProgress);
        
    //    blockrenderer.material.color = currentColor;
    //}
}
