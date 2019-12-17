using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// オブジェクトデータ
    /// </summary>
    [SerializeField]
    private List<ObjectData> Datas = new List<ObjectData>();

    public GameObject Test;

    private Queue<GameObject> Objects;

    //ステートマシン
    StateMachine<GameManager> state;

    /// <summary>
    /// 同時に生成されるオブジェクト上限数
    /// </summary>
    [SerializeField]
    [Range(1,50)]
    private int MaxNumObjects;

    // Start is called before the first frame update
    void Start()
    {
        state = new StateMachine<GameManager>(this);
        Objects = new Queue<GameObject>();
        state.ChangeState(TestState.Instance());
    }

    // Update is called once per frame
    void Update()
    {
        //StartCoroutine(CreateObject());
        state.Update();
        if(Input.GetKeyDown(KeyCode.Return))
        {
            state.ChangeState(TestState2.Instance());
        }
        //ChackNumObj();
        SleepObj();
    }

    /// <summary>
    /// オブジェクト数が上限を超えたら削除
    /// </summary>
    private void ChackNumObj()
    {
       if(Objects.Count > MaxNumObjects)
        {
            var Obj = Objects.Dequeue();
            Destroy(Obj);
        }
    }

    //生成関数
    public IEnumerator CreateObject()
    {
        while(Objects.Count != MaxNumObjects)
        {
            var s = GameObject.Instantiate(Test);
            Objects.Enqueue(s);
            yield return new WaitForSeconds(1.0f);
        }
        if (Objects.Count == MaxNumObjects)
            yield break;
    }

    //死亡管理
    private void SleepObj()
    {
        //敵の死亡判定を取る
        foreach(var Deat in Objects)
        {
            if(Deat)
            {
                //非アクティブ化
                Deat.SetActive(false);
                //死亡時のAnimetionを再生
                //終了したらSleepさせる
            }
        }
    }

}


/// <summary>
/// ステート実体
/// </summary>
public class TestState : ObjState<GameManager>
{
    private static TestState _instance = new TestState();

    public static TestState Instance()
    {
        return _instance;
    }

    private TestState() { }

    public override void Enter(ref GameManager other)
    {
        other.StartCoroutine(other.CreateObject());
    }

    public override void Execute(ref GameManager other)
    {

    }

    public override void Exit(ref GameManager other)
    {
        Debug.Log("ExitState1");
    }
}

public class TestState2 : ObjState<GameManager>
{
    private static TestState2 _instance = new TestState2();

    public static TestState2 Instance()
    {
        return _instance;
    }

    private TestState2() { }

    public override void Enter(ref GameManager other)
    {
        Debug.Log("EnterState2");
    }

    public override void Execute(ref GameManager other)
    {
       // Debug.Log("ExcuteState2");
    }

    public override void Exit(ref GameManager other)
    {
        Debug.Log("ExitState2");
    }
}