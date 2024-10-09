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
        positions = new List<Vector3>(_positions);// ���� ����� ���� 
        movetime = _moveTime;
        finishtime = _finishTime;
        starttime = _startTime;
        positions[0] = _source;
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
        // �� block�� �Ϸ� ���θ� �����ϴ� ������ �ʱ�ȭ
        isFinished = false;


        // ó���� (0,0,0)���� �ʱ�ȭ�Ǿ� �ִ� ������ positions ����Ʈ����, ���� ó�� �������� source�� ��ġ�� �߰�
        // Initialize()�� �������� ó�� source ��ġ�� �̵�
        transform.position = positions[0];
        targetPosition = positions[0];

        blockrenderer = GetComponent<Renderer>();
        blockrenderer.material.color = originalColor;
        darkColor = CreateDarkColor(originalColor);

        num_process = starttime.Length; // �����δ� GameManager���� �����ߴ� numProcess���� 1 ū ���� ���� �� (source�� ���ԵǾ��� ����)
        timer2 = movetime[0];
        target_delta = movetime[1] - movetime[0];

    }
    
    
    void Update()
    {
        // �۾� �Ϸ� ���θ� �Ǵ�
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

        // ���� ������ index�� update�ϴ� ���������� timer2�� ���� �����ϰ� ����
        if (timer2 > target_delta) // �� �� �� ȣ��Ǵ� �Լ�
        {
            timer2 = 0.0f;
            currentindex += 1; // 0���� 1�� ����
            moveProgress = 0.0f;
            colorChangeProgress = 0.0f;
            
            colorChangeDuration = finishtime[currentindex] - starttime[currentindex];
            blockrenderer.material.color = originalColor;

            // ���� timer2 ȣ�� ������ ���
            if (currentindex != num_process - 1) // ���� ������ process�� �ƴ϶��
            {
                target_delta = movetime[currentindex + 1] - movetime[currentindex];
            }
            else // ���� ������ process���, ���� timer2 ȣ�� ������ ����
            {
                target_delta = float.MaxValue;
            }

        }

        if (timer1 >= movetime[currentindex] && timer1 <= starttime[currentindex])
        {
            // �̵��ϴ� ���ȿ��� darkColor�� ����
            blockrenderer.material.color = darkColor;
            Move();
        }
        else
        {
            if (transform.position != targetPosition)
            {
                // Recording �� Move()�� �� �����Ӿ� �� ȣ��Ǵ� ������ ����. 
                // �̸� �����ϱ� ���� Move�ϱ�� ������ �ð� �̿ܿ��� ���� target�� �������� ���� ��� Move()�� �̾���� ����
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
        
        // Ÿ�̸� ����
        timer1 += Time.deltaTime;
        timer2 += Time.deltaTime;
        
    }


    // ������ ��ǥ ��ġ�� 0.5�� ���� �̵��ϴ� �Լ�
    void Move()
    {
        // �̵� ���� ���� (0.0 ~ 1.0) ���
        moveProgress += Time.deltaTime / movingTime;

        // ���� ��ġ���� ��ǥ ��ġ�� �̵� (Lerp ���)
        transform.position = Vector3.Lerp(currentPosition, targetPosition, moveProgress);
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
