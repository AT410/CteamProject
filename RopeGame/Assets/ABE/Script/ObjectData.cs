using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;

public enum EnemyType
{
    Zomib,
    Thief,
    Executioner,
    BOSS
}

public class ObjectData : ScriptableObject
{
    /// <summary>
    /// オブジェクト用パラメータデータ
    /// </summary>
    public ObjectType type;

    public EnemyType Etype;

    [SerializeField]
    protected float MoveSpeed;

    //時間制限で消えるかどうか
    [SerializeField]
    protected bool IsLimitActive=false;

    //セッター・ゲッター
    public bool GetDelFlag
    {
        get
        {
            return IsLimitActive;
        }
#if UNITY_EDITOR
        set
        {
            IsLimitActive = value;
        }
#endif
    }

    public float ISMoveSpeed
    {
        get
        {
            return MoveSpeed;
        }
#if UNITY_EDITOR
        set
        {
            MoveSpeed = value;
        }
#endif
    }
}
