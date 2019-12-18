using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager Ins;

    public static GameManager GetGameManager()
    {
        return Ins;
    }

    private void Awake()
    {
        Ins = this;
    }


    /// <summary>
    /// オブジェクトデータ
    /// </summary>
    [SerializeField]
    private List<ObjectData> Datas = new List<ObjectData>();

    public GameObject Test;

    private Queue<GameObject> Objects;

    public GameObject pauseUI;

    //ステートマシン
    StateMachine<GameManager> state;

    public StateMachine<GameManager> GetStateMachine()
    {
        return state;
    }

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
        state.ChangeState(ExecuteSceneState.Instance());
        SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
    }

    private void SceneManager_activeSceneChanged(Scene Currnt, Scene Next)
    {
        if (Next.name != "MainGameScene")
        {
            state.IsUpdateActive = false;
            return;
        }

        state.IsUpdateActive = true;
        return;
    }

    // Update is called once per frame
    void Update()
    {
        //StartCoroutine(CreateObject());
        state.Update();

        //ChackNumObj();
        //SleepObj();
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


///<summary>
///実行中ステート
/// </summary>
public class ExecuteSceneState : ObjState<GameManager>
{
    private static ExecuteSceneState _instance = new ExecuteSceneState();

    public static ExecuteSceneState Instance()
    {
        return _instance;
    }

    private ExecuteSceneState() { }

    public override void Enter(ref GameManager other)
    {
        other.StartCoroutine(other.CreateObject());
    }

    public override void Execute(ref GameManager other)
    {
        if (Input.GetKeyDown("joystick button 9")||Input.GetKeyDown(KeyCode.E))
        {
            other.GetStateMachine().ChangeState(PauseSceneState.Instance());
        }
    }

    public override void Exit(ref GameManager other)
    {
        //
        other.pauseUI.SetActive(true);
        //すべての更新を止める
        Time.timeScale = 0;
    }
}

public class PauseSceneState : ObjState<GameManager>
{
    private static PauseSceneState _instance = new PauseSceneState();

    public static PauseSceneState Instance()
    {
        return _instance;
    }

    private PauseSceneState() { }

    public override void Enter(ref GameManager other)
    {

    }

    public override void Execute(ref GameManager other)
    {
        if (Input.GetKeyDown("joystick button 9") || Input.GetKeyDown(KeyCode.E))
        {
            other.GetStateMachine().ChangeState(ExecuteSceneState.Instance());
        }
    }

    public override void Exit(ref GameManager other)
    {
        other.pauseUI.SetActive(false);
        //更新を再開
        Time.timeScale = 1.0f;
    }
}