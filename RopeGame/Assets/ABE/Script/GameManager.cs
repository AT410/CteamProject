using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public struct CompletValue
{
    public int T1;
    public int T2;
    public int T3;

    public CompletValue(int AllVal)
    {
        int Del1, Del2, Del3;
        int temp = AllVal;
        do
        {
            AllVal = temp;
            Del1 = Random.Range(2, AllVal - 4);
            AllVal -= Del1;
            Del2 = Random.Range(2, AllVal - 4);
            AllVal -= Del2;
            Del3 = Random.Range(2, AllVal - 4);
            AllVal -= Del3;

        } while (AllVal>0 && Del1 <= 2 && Del2 <= 2 && Del3 <= 2);

        T1 = Del1;
        T2 = Del2;
        T3 = Del3;
    }

    public float Chack100P()
    {
        var P = T1 + T2 + T3;
        return P / 100.0f;
    }
}


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
    private List<GameObject> EnemyPrefabs = new List<GameObject>();

    public GameObject Test;

    private Queue<GameObject> Objects;

    public GameObject pauseUI;

    /// <summary>
    /// 同時に生成されるオブジェクト上限数
    /// </summary>
    [SerializeField]
    [Range(1, 50)]
    private int MaxNumObjects;


    [SerializeField]
    [Range(1,5)]
    private float _GenerateTime;

    [SerializeField]
    [Range(1, 50)]
    private int QuestEnemyNum;

    private CompletValue MM;

    //ステートマシン
    StateMachine<GameManager> state;

    public StateMachine<GameManager> GetStateMachine()
    {
        return state;
    }

    // Start is called before the first frame update
    void Start()
    {
        state = new StateMachine<GameManager>(this);
        Objects = new Queue<GameObject>();
        state.ChangeState(ExecuteSceneState.Instance());
        MM = new CompletValue(0);
        SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
    }

    private void SceneManager_activeSceneChanged(Scene Currnt, Scene Next)
    {
        if (Next.name != "MainGameScene")
        {
            state.IsUpdateActive = false;
            return;
        }
        else
        {
            //クエスト生成
            CreateQuest();
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

    private void CreateQuest()
    {
        MM = new CompletValue(QuestEnemyNum);
        Debug.Log("T1=" + MM.T1.ToString() + "T2=" + MM.T2.ToString() + "T3=" + MM.T3.ToString());
    }

    //生成関数
    public IEnumerator CreateObject()
    {
        while(Objects.Count != MaxNumObjects)
        {
            int SelectNum  = Random.Range(0,EnemyPrefabs.Count);
            var Pre = EnemyPrefabs[SelectNum];
            Vector3 Pos = new Vector3(Random.Range(-4, 4), Random.Range(-4, 4));
            var s = GameObject.Instantiate(Pre, Pos,Quaternion.identity);
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