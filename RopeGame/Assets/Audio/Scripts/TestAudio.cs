using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAudio : MonoBehaviour
{
    private void Start()
    {
        AudioManager.Instance.PlayBGM("Title");  //タイトルSE再生
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            AudioManager.Instance.FadeOutBGM();           //BGMフェードアウト
            AudioManager.Instance.PlayBGM("Title");  //タイトルSE再生
        }
        if(Input.GetKeyDown(KeyCode.X))
        {
            AudioManager.Instance.FadeOutBGM();           //BGMフェードアウト
            AudioManager.Instance.PlayBGM("Main");  //ゲーム内SE再生
        }
        if(Input.GetKeyDown(KeyCode.C))
        {
            AudioManager.Instance.PlaySE("Avoidance");  //回避SE再生
        }
        if(Input.GetKeyDown(KeyCode.V))
        {
            AudioManager.Instance.PlaySE("Shoot");  //射撃SE再生
        }
        if(Input.GetKeyDown(KeyCode.B))
        {
            AudioManager.Instance.PlaySE("Hook");  //フックSE再生
        }

    }
}
