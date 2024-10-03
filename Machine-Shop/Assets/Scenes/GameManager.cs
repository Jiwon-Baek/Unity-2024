using System.Collections;
using System.Collections.Generic;
using UnityEngine;


class Job
{
    public Job(int _blockindex, GameObject prefab, Vector3[] _positions, Vector3 _currpos, float[] _movetime, float[] _finishtime, float[] _starttime)
    {
        
        // Instantiate�� static �޼����̹Ƿ� Object.Instantiate�� ȣ���ؾ� �մϴ�.
        block = Object.Instantiate(prefab);
        // ������ �ν��Ͻ����� Block ������Ʈ ��������
        Block blockComp = block.GetComponent<Block>();
        // �ʱ�ȭ �޼��� ȣ��
        blockComp.Initialize(_blockindex, _positions, _movetime, _finishtime, _starttime, _currpos);

        // Ȱ��ȭ
        block.SetActive(true);

    }

    public GameObject block;

}
class Machine
{
    public Machine(GameObject prefab, Vector3 initialPosition)
    {

        // Instantiate�� static �޼����̹Ƿ� Object.Instantiate�� ȣ���ؾ� �մϴ�.
        process = Object.Instantiate(prefab);
        // ������ �ν��Ͻ����� Block ������Ʈ ��������
        Process processComp = process.GetComponent<Process>();
        processComp.transform.position = initialPosition;

        process.SetActive(true);
    }
    public GameObject process;

}




public class GameManager : MonoBehaviour
{
    public GameObject BlockPrefab;
    public GameObject ProcessPrefab;


    public Transform parent;

    public bool isFinished;

    // Job ��ü �迭 ����
    private Job[] jobs;  // Job[] Ÿ������ ����
    private Machine[] machines;
    private Vector3 pos;
    private Vector3 processposition;
    private int num_machine = 3;
    int count;

    // Start is called before the first frame update
    void Start()
    {
        // Job �迭 ũ�� ����
        jobs = new Job[3];  // 3���� Job�� ���� �� �ִ� �迭 ����
        machines = new Machine[num_machine];

        //------------------------------------------------
        Vector3[] pos = new Vector3[]
        {
            new Vector3(-2.0f, 0.0f, 0.0f),
            new Vector3(0.0f, 0.0f, 0.0f),
            new Vector3(1.5f, 0.0f, 0.0f),
            new Vector3(3.0f, 0.0f, 0.0f),
            new Vector3(4.5f, 0.0f, 0.0f)
        };
        Vector3 source = new Vector3(-2.0f, 0.0f, 0.0f);

        float[] move1 = new float[] { 0.0f, 0.0f, 4.0f, 6.0f ,9.0f};
        float[] start1 = new float[] { 0.0f, 0.5f, 4.5f, 6.5f ,9.5f};
        float[] finish1 = new float[] { 0.0f, 4.0f, 6.0f, 9.0f ,9.5f};
        // ������ Job ��ü ����
        jobs[0] = new Job(0, BlockPrefab, pos, source, move1, finish1, start1);

        //------------------------------------------------
        float[] move2 = new float[] { 0.0f, 4.0f, 7.0f, 9.0f, 11.0f };
        float[] start2 = new float[] { 0.0f, 4.5f, 7.5f, 9.5f, 11.5f};
        float[] finish2 = new float[] { 0.0f, 7.0f, 9.0f, 11.0f ,11.5f};
        jobs[1] = new Job(1, BlockPrefab, pos, source, move2, finish2, start2);
        //------------------------------------------------
        
        float[] move3 = new float[] { 0.0f, 7.0f, 9.0f, 11.0f, 13.0f};
        float[] start3 = new float[] { 0.0f, 7.5f, 9.5f, 11.5f, 13.5f};
        float[] finish3 = new float[] { 0.0f, 8.0f, 10.0f, 13.0f, 13.5f};

        jobs[2] = new Job(2, BlockPrefab, pos, source, move3, finish3, start3);

        //------------------------------------------------
        int n = 0;
        for (int j = 0; j < 3; j++)
        {
            processposition = new Vector3(j * 1.5f, -0.31f, 0.0f);
            machines[n] = new Machine(ProcessPrefab, processposition);
            Debug.Log("New machine " + n + " generated on " + processposition);
            n++;
        }


        
        
    }

    // Update is called once per frame
    void Update()
    {
        
        check_termination();

    }
    void check_termination()
    {

        int count = 0;
        for (int i = 0; i < jobs.Length; i++)
        {
            if (jobs[i].block.GetComponent<Block>().isFinished == true)
            {
                count++;
            }
        }

        if (count == jobs.Length)
        {
            Debug.Log("All Jobs Finished!");
        }
    }
}
