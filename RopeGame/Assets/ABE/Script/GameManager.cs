using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// MakeCode:Abe Tatsuya
/// </summary>

public struct CompletValue
{
    public float T1;
    public float T2;
    public float T3;

    private float All;

    public CompletValue(int I = 0)
    {
        int Del1, Del2, Del3;
        Del1 = Random.Range(3, 6);
        Del2 = Random.Range(3, 6);
        Del3 = Random.Range(3, 6);

        T1 = Del1;
        T2 = Del2;
        T3 = Del3;
        All = T1 + T2 + T3;
    }

    public float AllCount()
    {
        return All;
    }

    public float Chack100P()
    {
        var P = T1 + T2 + T3;
        //Debug.Log(P / All);
        return 1.0f-(P / All);
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
        if (Ins ==null)
        {
            Ins = this;
        }
        else
        {
            Debug.Log("Awake:GM");
        }

        //gameObject.SetActive(false);
        //state.IsUpdateActive = false;
    }

    public bool ClearFlag = false;
    /// <summary>
    /// オブジェクトデータ
    /// </summary>
    [SerializeField]
    private List<GameObject> EnemyPrefabs = new List<GameObject>();

    [SerializeField]
    public CompletValue BossEnemy;

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
    private int QuestEnemyMaxNum;

    public CompletValue QuestData;

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
        QuestData = new CompletValue(0);
        SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
    }

    private void SceneManager_activeSceneChanged(Scene Currnt, Scene Next)
    {
        if (Next.name != "MainGameScene")
        {
            state.IsUpdateActive = false;
            //gameObject.SetActive(false);
            return;
        }
        else
        {
            //クエスト生成
            CreateQuest();
            //MaxNumObjects = (int)QuestData.AllCount()+5;
            state.IsUpdateActive = true;
        }

        //if(Next.name =="GameClearScene")
        //{
        //    ClearFlag = true;
        //}
        //gameObject.SetActive(true);
        return;
    }

    // Update is called once per frame
    void Update()
    {
        //StartCoroutine(CreateObject());
        state.Update();

        //Debug.Log(QuestData.Chack100P());
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
        //クエスト生成
        QuestData = new CompletValue(QuestEnemyMaxNum);
    }

    //生成関数
    public IEnumerator CreateObject()
    {
        while(Objects.Count != MaxNumObjects)
        {
            int SelectNum  = Random.Range(0,EnemyPrefabs.Count-1);
            var Pre = EnemyPrefabs[SelectNum];
            Vector3 Pos = new Vector3(Random.Range(-18, 18), Random.Range(-4, 16));
            var s = GameObject.Instantiate(Pre, Pos,Quaternion.identity);
            Objects.Enqueue(s);
            yield return new WaitForSeconds(_GenerateTime);
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

    public void QUIUpdate(EnemyType type)
    {
        if (!ChackQuestData(type))
            return;

        switch (type)
        {
            case EnemyType.Zomib:
                QuestData.T1 -= 1;
                QuestUI.instance.UI2.SetActive(true);
                break;
            case EnemyType.Thief:
                QuestData.T2 -= 1;
                break;
            case EnemyType.Executioner:
                QuestData.T3 -= 1;
                break;
        }
    }

    private bool ChackQuestData(EnemyType type)
    {
        switch (type)
        {
            case EnemyType.Zomib:
                if (QuestData.T1 == 0)
                {
                    //QuestUI.instance.UI1.SetActive(true);
                    Debug.Log("LLSOSOS");
                    return false;
                }
                break;
            case EnemyType.Thief:
                if (QuestData.T2 == 0)
                {
                    //QuestUI.instance.UI2.SetActive(true);
                    return false;
                }
                break;
            case EnemyType.Executioner:
                if (QuestData.T3 == 0)
                {
                    //QuestUI.instance.UI3.SetActive(true);
                    return false;
                }
                break;
            case EnemyType.BOSS:
                Test.Instance().ClearFlag = true;
                //GameObject.Find("XMK").SetActive(true);
                SceneManager.LoadScene("GameClearScene");
                break;
        }
        return true;

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
        //クエストUIに値を渡す。
        //既に渡している場合渡さない
        UpdateUI(ref other);
    }

    public override void Execute(ref GameManager other)
    {
        if (Input.GetKeyDown("joystick button 9")||Input.GetKeyDown(KeyCode.E))
        {
            other.GetStateMachine().ChangeState(PauseSceneState.Instance());
        }
        UpdateUI(ref other);

        //目標値を達成したらBoss出現
        if(other.QuestData.Chack100P()==1.0f)
        {
            other.GetStateMachine().ChangeState(BossSceneState.Instance());
        }
    }

    public override void Exit(ref GameManager other)
    {
        //
        if (other.GetStateMachine().ChackNextState(PauseSceneState.Instance()))
        {
            other.pauseUI.SetActive(true);
            //すべての更新を止める
            Time.timeScale = 0;
            other.StopAllCoroutines();
        }
        //ToBossState
        if (other.GetStateMachine().ChackNextState(BossSceneState.Instance()))
        {
            //クエストUIを非表示
            QuestUI.instance.enabled = false;
            var Obj = GameObject.Find("QuestUIs");
            if(Obj)
            {
                Obj.SetActive(false);
            }
            //現在配置されているEnemyを削除または、非アクティブ
            var EnemyObjects = GameObject.FindGameObjectsWithTag("Enemy");
            foreach(var Enemy in EnemyObjects)
            {
                GameObject.Destroy(Enemy);
            }
            //現在の全コルーチンを停止
            other.StopAllCoroutines();
        }
    }

    private void UpdateUI(ref GameManager other)
    {
        var E1 = other.QuestData.T1;
        var E2 = other.QuestData.T2;
        var E3 = other.QuestData.T3;

        QuestUI.instance.EnemyUI1.text = E1.ToString();
        QuestUI.instance.EnemyUI2.text = E2.ToString();
        QuestUI.instance.EnemyUI3.text = E3.ToString();
    }
}

/// <summary>
/// Boss出現中
/// </summary>
public class BossSceneState :ObjState<GameManager>
{
    private static BossSceneState _instance = new BossSceneState();

    public static BossSceneState Instance()
    {
        return _instance;
    }

    private BossSceneState() { }

    public override void Enter(ref GameManager other)
    {
        //Bossの出現イベントを発生させる
        //イベント
        EventScript.Instance().GetStateMachine().ChangeState(GoToEventState.Instance());
    }

    public override void Execute(ref GameManager other)
    {
        //Bossの出現イベントが終わったら(カメラが
        if (EventScript.Instance().GetStateMachine().ChackState(DefaultEventState.Instance()))
        {
            Debug.Log("WWWWWW");
        }
    }

    public override void Exit(ref GameManager other)
    {

    }
}

/// <summary>
/// ポーズ中
/// </summary>
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