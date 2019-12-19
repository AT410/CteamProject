using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventScript : MonoBehaviour
{
    private static EventScript _Ins;
    
    public static EventScript Instance()
    {
        return _Ins;
    }

    /// <summary>
    /// イベントプール
    /// </summary>
    private Dictionary<string, MakeEvent> EventPool = new Dictionary<string, MakeEvent>();

    //イベント開始時のPlayerのポジション

    //ステートマシン
    StateMachine<EventScript> state;

    [SerializeField]
    private GameObject MainCamera;

    [SerializeField]
    private GameObject Player;

    public GameObject BossPrefub;

    private string ActiveEventKey="NULL";

    public StateMachine<EventScript> GetStateMachine()
    {
        return state;
    }

    private void Awake()
    {
        if(_Ins == null)
        {
            _Ins = this;
        }
        else
        {
            Destroy(gameObject);
        }
        EventPool.Clear();
        EventPool.Add("BossEvent", new MakeEvent(new Vector3(5, 5, 0), 1.0f));
    }

    // Start is called before the first frame update
    void Start()
    {
        state = new StateMachine<EventScript>(this);
        state.ChangeState(DefaultEventState.Instance());
    }

    // Update is called once per frame
    void Update()
    {
        state.Update();
    }

    public MakeEvent GetEvent(string key)
    {
        ActiveEventKey = key;
        return EventPool[key];
    }

    public bool CheckPosition(Vector3 TargetPos)
    {
        var CameraPos = MainCamera.transform.position;

        if((TargetPos.x - 0.5f < CameraPos.x && CameraPos.x < TargetPos.x + 0.5f) &&
            (TargetPos.y - 0.5f < CameraPos.y && CameraPos.y < TargetPos.y + 0.5f))
        {
            return true;
        }
        return false;
    }

    public void UpdateCamera(Vector3 MoveForce)
    {
        MainCamera.transform.position += new Vector3(MoveForce.x,MoveForce.y,0.0f);
    }

    public Vector3 GetPlayerPos()
    {
        return Player.transform.position;
    }

    public Vector3 GetCameraPos()
    {
        return MainCamera.transform.position;
    }
}

//むかう
//待機・終了
//もどる

/// <summary>
/// デフォルトステート
/// イベントが発生するまで待機
/// </summary>
public class DefaultEventState : ObjState<EventScript>
{
    private static DefaultEventState instance = new DefaultEventState();

    public static DefaultEventState Instance()
    {
        return instance;
    }

    private DefaultEventState() { }

    public override void Enter(ref EventScript other)
    {
        //登録イベント数確認
    }

    public override void Execute(ref EventScript other)
    {
        //イベントが発生するまで待機
        //イベントが発生したらイベントが実行ステートに遷移
        if(Input.GetKeyDown(KeyCode.Return))
        {
            other.GetStateMachine().ChangeState(GoToEventState.Instance());
        }
    }

    public override void Exit(ref EventScript other)
    {
        
    }
}

/// <summary>
/// イベント発生地点まで移動をする。遷移
/// 行き
/// </summary>
public class GoToEventState : ObjState<EventScript>
{
    private static GoToEventState instance = new GoToEventState();

    public static GoToEventState Instance()
    {
        return instance;
    }

    private Vector3 StartPositon;

    private Vector3 ToTargetPoint;

    private float _TotalTime = 5;

    private float _CurrntTime;

    public override void Enter(ref EventScript other)
    {
        //イベント開始地点に移動を開始する
        //前回と移動が異なるとき向きを変える
        //移動地点の取得
        var Event = other.GetEvent("BossEvent");
        _TotalTime = Event.MoveSpeed;
        ToTargetPoint = Event.EventPoint;
        StartPositon = other.GetPlayerPos();
        //前回の移動方向を同期
        _CurrntTime = 0;
        Debug.Log("MoveStart");

        //マネージャーにイベント開始を通知
    }

    public override void Execute(ref EventScript other)
    {
        //ターゲットポジションへ移動を開始
        _CurrntTime += Time.deltaTime;
        Move(ref other);
        //移動が完了したらイベント実行ステートに変更
        if (other.CheckPosition(ToTargetPoint))
        {
            other.GetStateMachine().ChangeState(EventActiveState.Instance());
            Debug.Log("MoveEnd");
        }
    }

    public override void Exit(ref EventScript other)
    {
        //帰り状態にする
    }

    private void Move(ref EventScript other)
    {
        _CurrntTime /= _TotalTime;

        Vector3 SpanVec = new Vector3(0, 0, 0);

        SpanVec = ToTargetPoint - StartPositon;

        var Force = SpanVec * Time.deltaTime;
        Debug.Log(Force);
        other.UpdateCamera(Force);
        StartPositon = other.GetCameraPos();
    }
}


/// <summary>
/// イベント実行中
/// イベント終了したら遷移
/// </summary>
public class EventActiveState : ObjState<EventScript>
{
    private static EventActiveState instance = new EventActiveState();

    public static EventActiveState Instance()
    {
        return instance;
    }

    private EventActiveState() { }

    private float TestTime = 0;

    public override void Enter(ref EventScript other)
    {
        Debug.Log("StartMove");
        //Bossの出現イベント開始
        var BossGeneratePos  = other.GetCameraPos();
        BossGeneratePos.z = 0;
        GameObject.Instantiate(other.BossPrefub,BossGeneratePos,Quaternion.identity);
    }

    public override void Execute(ref EventScript other)
    {
        //イベントが発生するまで待機
        //イベントが発生したらイベントが実行ステートに遷移
        if (TestTime>=5.0f)
        {
            other.GetStateMachine().ChangeState(GoToStartState.Instance());
        }
        TestTime += Time.deltaTime;
    }

    public override void Exit(ref EventScript other)
    {
        Debug.Log("StartMove");
    }
}

/// <summary>
/// プレイヤー地点。遷移
/// 戻り
/// </summary>
public class GoToStartState : ObjState<EventScript>
{
    private static GoToStartState instance = new GoToStartState();

    public static GoToStartState Instance()
    {
        return instance;
    }

    private Vector3 StartPositon;

    private Vector3 ToTargetPoint;

    private float _TotalTime = 5;

    private float _CurrntTime;

    public override void Enter(ref EventScript other)
    {
        //プレイヤー地点に移動を開始する
        //移動地点の取得
        var Event = other.GetEvent("BossEvent");
        _TotalTime = Event.MoveSpeed;
        ToTargetPoint = other.GetPlayerPos();
        StartPositon =  other.GetCameraPos();
        //前回の移動方向を同期
        _CurrntTime = 0;
        Debug.Log("MoveStart");
    }

    public override void Execute(ref EventScript other)
    {
        //ターゲットポジションへ移動を開始
        _CurrntTime += Time.deltaTime;
        Move(ref other);
        //移動が完了したらイベント実行ステートに変更
        if (other.CheckPosition(ToTargetPoint))
        {
            other.GetStateMachine().ChangeState(DefaultEventState.Instance());
            Debug.Log("MoveEnd");
        }
    }

    public override void Exit(ref EventScript other)
    {
        //マネージャーにイベント終了を通知

    }

    private void Move(ref EventScript other)
    {
        _CurrntTime /= _TotalTime;

        Vector3 SpanVec = new Vector3(0, 0, 0);

        SpanVec = ToTargetPoint - StartPositon;

        var Force = SpanVec * Time.deltaTime;
        Debug.Log(Force);
        other.UpdateCamera(Force);
        StartPositon = other.GetCameraPos();
    }
}
