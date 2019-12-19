using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SelectKeyButton : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> Buttons = new List<GameObject>();

    public int SelectNum = 0;

    private bool ActiveCol = false;

    private int currentConnectionCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        SetSelected(SelectNum);
    }

    // Update is called once per frame
    void Update()
    {
        var controllerNames = Input.GetJoystickNames();

        string[] cName = Input.GetJoystickNames();
        currentConnectionCount = 0;
        for (int i = 0; i < cName.Length; i++)
        {
            if (cName[i] != "")
            {
                currentConnectionCount++;
            }
        }

        if (currentConnectionCount > 0 &&!ActiveCol)
        {
            SetSelected(SelectNum);
            ActiveCol = !ActiveCol;
        }

        //if(Input.GetKeyDown(KeyCode.DownArrow))
        //{
        //    SetSelected(SelectNum++);
        //}
        //if (SelectNum == Buttons.Count)
        //    SelectNum = 0;
    }

    private void SetSelected(int num)
    {
        Buttons[num].GetComponent<Button>().interactable = true;
        EventSystem.current.SetSelectedGameObject(Buttons[num]);
    }

    public void EndButtonClick()
    {
        Application.Quit();
    }

    public void RetrunTitleScene()
    {
        var Temp = GameObject.Find("ChangeScene");
        if (Temp)
        {
            Temp.GetComponent<ChageScene>().FadeTime = 1.0f;
            Color DeathFadeC = new Color(201.0f / 255.0f, 51.0f / 255.0f, 41.0f / 255.0f);
            Temp.GetComponent<ChageScene>().SceneName = "Title";
            Temp.GetComponent<ChageScene>().PushStart();
        }
        GameManager.GetGameManager().GetStateMachine().ChangeState(ExecuteSceneState.Instance());
    }

    public void ShotSE()
    {
        AudioManager.Instance.PlaySE("Shoot");  //銃声のSE再生
    }

    public void BackGameScene()
    {
        //実行中に戻す

    }
}
