using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectKeyButton : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> Buttons;

    private int SelectNum = 0;
    // Start is called before the first frame update
    void Start()
    {
        SetSelected(SelectNum);
    }

    // Update is called once per frame
    void Update()
    {
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
}
