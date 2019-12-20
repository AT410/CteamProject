using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeSceneLimit : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    private float _ChangeTime;

    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    private string LoadScene;

    private float _currnttime;

    // Start is called before the first frame update
    void Start()
    {
        _currnttime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(_currnttime>_ChangeTime)
            SceneManager.LoadScene(LoadScene);
        _currnttime += Time.deltaTime;
    }
}
