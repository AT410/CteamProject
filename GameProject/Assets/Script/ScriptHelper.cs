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

    //コンストラクタ
    public StateMachine(T Obj)
    {
        m_Owner = Obj;
    }

    ~StateMachine() { }

    public void Update()
    {
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
}