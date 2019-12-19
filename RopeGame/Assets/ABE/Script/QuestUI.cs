using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour
{
    public static QuestUI instance;

    public void Awake()
    {
        instance = this;
    }

    public Text EnemyUI1;

    public Text EnemyUI2;

    public Text EnemyUI3;

    bool IsSetParam =false;

    CompletValue Value;

    
}
