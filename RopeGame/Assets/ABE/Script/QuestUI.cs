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

    public GameObject UI1;

    public GameObject UI2;

    public GameObject UI3;

    bool IsSetParam =false;

    CompletValue Value;

    private void Update()
    {
        if (EnemyUI1.text == "0")
        {
            UI1.SetActive(true);
        }
        if (EnemyUI2.text == "0")
        {
            UI2.SetActive(true);
        }
        if (EnemyUI3.text == "0")
        {
            UI3.SetActive(true);
        }
    }
}
