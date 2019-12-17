using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class ActivateUIObj : MonoBehaviour
{

    ////　画面UI
    //[SerializeField]
    //private GameObject statusWindow;
    ////　ボタンのインタラクティブに関する処理が書かれているスクリプト
    //[SerializeField]
    //private ActiveButton select1;
    //[SerializeField]
    //private ActiveButton select2;

    //void Update()
    //{

    //    //　スペースキーを押したら画面UIのオン・オフ
    //    if (Input.GetKeyDown("space"))
    //    {
    //        statusWindow.SetActive(!statusWindow.activeSelf);

    //        //　画面を開いた時にBackground1のボタンのインタラクティブをtrue、Background2のボタンのインタラクティブをfalseにする
    //        if (statusWindow.activeSelf)
    //        {

    //            select1.ActivateOrNotActivate(true);
    //            select2.ActivateOrNotActivate(false);

    //            //　画面を閉じたら選択を解除
    //        }
    //        else
    //        {
    //            EventSystem.current.SetSelectedGameObject(null);
    //        }
    //    }
    //}
}
