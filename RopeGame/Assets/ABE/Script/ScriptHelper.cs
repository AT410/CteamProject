using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.CodeDom;

public abstract class ObjState<T>
{
    public ObjState() { }
    ~ObjState() { }

    public abstract void Enter(ref T other);
    public abstract void Execute(ref T other);
    public abstract void Exit(ref T other);
}

public class StateMachine<T>
{
    //このステートマシンを持つオーナー
    private T m_Owner;
    //現在のステート
    private ObjState<T> m_CurrentState;
    //一つ前のステート
    private ObjState<T> m_PreviousState;

    //更新可能
    private bool _IsActive = true;

    public bool IsUpdateActive
    {
        get
        {
            return _IsActive;
        }
        set
        {
            _IsActive = value;
        }
    }

    //コンストラクタ
    public StateMachine(T Obj)
    {
        m_Owner = Obj;
    }

    ~StateMachine() { }

    public void Update()
    {
        if(!_IsActive)
        {
            return;
        }

        if (m_CurrentState!=null && m_Owner !=null)
        {
            m_CurrentState.Execute(ref m_Owner);
        }

    }

    public void ChangeState(ObjState<T> objState)
    {
        m_PreviousState = m_CurrentState;
        if(m_CurrentState!=null && m_Owner!=null)
        {
            m_CurrentState.Exit(ref m_Owner);
        }

        m_CurrentState = objState;
        if (m_CurrentState != null && m_Owner != null)
        {
            m_CurrentState.Enter(ref m_Owner);
        }

    }

    public bool ChackState(ObjState<T> other)
    {
        return other == m_CurrentState;
    }
}

//コルーチン実行用
public class GlobalCoroutine : MonoBehaviour
{

    public static void Go(IEnumerator coroutine)
    {
        GameObject obj = new GameObject();     // コルーチン実行用オブジェクト作成
        obj.name = "GlobalCoroutine";

        GlobalCoroutine component = obj.AddComponent<GlobalCoroutine>();
        if (component != null)
        {
            component.StartCoroutine(component.Do(coroutine));
        }
    }

    IEnumerator Do(IEnumerator src)
    {
        while (src.MoveNext())
        {               // コルーチンの終了を待つ
            yield return null;
        }

        Destroy(this.gameObject);              // コルーチン実行用オブジェクトを破棄
    }
}