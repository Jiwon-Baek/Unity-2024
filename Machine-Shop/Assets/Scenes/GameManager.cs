using System.Collections;
using System.Collections.Generic;
using UnityEngine;


class Job
{
    public Job(int _blockindex, GameObject prefab, Vector3[] _positions, Vector3 _currpos, float[] _movetime, float[] _finishtime, float[] _starttime)
    {
        
        // Instantiate는 static 메서드이므로 Object.Instantiate로 호출해야 합니다.
        block = Object.Instantiate(prefab);
        // 생성한 인스턴스에서 Block 컴포넌트 가져오기
        Block blockComp = block.GetComponent<Block>();
        // 초기화 메서드 호출
        blockComp.Initialize(_blockindex, _positions, _movetime, _finishtime, _starttime, _currpos);

        // 활성화
        block.SetActive(true);

    }

    public GameObject block;

}
class Machine
{
    public Machine(GameObject prefab, Vector3 initialPosition)
    {

        // Instantiate는 static 메서드이므로 Object.Instantiate로 호출해야 합니다.
        process = Object.Instantiate(prefab);
        // 생성한 인스턴스에서 Block 컴포넌트 가져오기
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

    // Job 객체 배열 선언
    private Job[] jobs;  // Job[] 타입으로 선언
    private Machine[] machines;
    private Vector3 pos;
    private Vector3 processposition;
    private int num_machine = 3;
    int count;

    // Start is called before the first frame update
    void Start()
    {
        // Job 배열 크기 지정
        jobs = new Job[3];  // 3개의 Job을 담을 수 있는 배열 생성
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
        // 각각의 Job 객체 생성
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
