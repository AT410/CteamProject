using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventScript : MonoBehaviour
{
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

    public StateMachine<EventScript> GetStateMachine()
    {
        return state;
    }

    private void Awake()
    {
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
        return EventPool[key];
    }

    public bool CheckPosition()
    {
        var CameraPos = MainCamera.transform.position;
        var PlayerPos = Player.transform.position;

        if(PlayerPos.x == CameraPos.x&& PlayerPos.y == CameraPos.y)
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
            other.GetStateMachine().ChangeState(MoveToEventState.Instance());
        }
    }

    public override void Exit(ref EventScript other)
    {
        
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

    public override void Enter(ref EventScript other)
    {
        Debug.Log("StartMove");
    }

    public override void Execute(ref EventScript other)
    {
        //イベントが発生するまで待機
        //イベントが発生したらイベントが実行ステートに遷移
        if (Input.GetKeyDown(KeyCode.Return))
        {
            other.GetStateMachine().ChangeState(MoveToEventState.Instance());
        }

    }

    public override void Exit(ref EventScript other)
    {
        Debug.Log("StartMove");
    }
}

/// <summary>
/// イベント発生地点まで移動をする。遷移
/// 終了したらもどる。遷移
/// </summary>
public class MoveToEventState : ObjState<EventScript>
{
    private static MoveToEventState instance = new MoveToEventState();

    public static MoveToEventState Instance()
    {
        return instance;
    }

    //向かう・戻る（true・false）
    private bool MoveAxis = true;

    //前回の移動向き
    private bool BeforeActive = true;

    private MoveToEventState() { }

    private Vector3 StartPositon;

    private Vector3 ToTargetPoint;

    private float _TotalTime=5;

    private float _CurrntTime;

    public override void Enter(ref EventScript other)
    {
        //イベント開始地点に移動を開始する
        //前回と移動が異なるとき向きを変える
        if (MoveAxis != BeforeActive)
        {
            Vector3 Temp = StartPositon;
            StartPositon = ToTargetPoint;
            ToTargetPoint = Temp;
        }
        else
        {
            //移動地点の取得
            var Event = other.GetEvent("BossEvent");
            _TotalTime = Event.MoveSpeed;
            ToTargetPoint = Event.EventPoint;
            StartPositon = other.GetPlayerPos();
        }

        //前回の移動方向を同期
        BeforeActive = MoveAxis;
        _CurrntTime = 0;
        Debug.Log("MoveStart");
    }

    public override void Execute(ref EventScript other)
    {
        //ターゲットポジションへ移動を開始
        _CurrntTime += Time.deltaTime;
        Move(ref other);
        //移動が完了したらイベント実行ステートに変更
        if (other.CheckPosition())
        {
            other.GetStateMachine().ChangeState(EventActiveState.Instance());
            Debug.Log("MoveEnd");
        }
    }

    public override void Exit(ref EventScript other)
    {
        //帰り状態にする
        MoveAxis = false;
    }

    private void Move(ref EventScript other)
    {
        _CurrntTime /= _TotalTime;
        var SpanVec = ToTargetPoint - StartPositon;

        var Force = SpanVec * Time.deltaTime;
        Debug.Log(Force);
        other.UpdateCamera(Force);
        StartPositon = other.GetCameraPos();
    }
}
