using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// オブジェクトデータ
    /// </summary>
    [SerializeField]
    private List<ObjectData> Datas = new List<ObjectData>();

    private List<GameObject> Objects;
    /// <summary>
    /// 同時に生成されるオブジェクト上限数
    /// </summary>
    [SerializeField]
    [Range(1,50)]
    private int MaxNumObjects;

    // Start is called before the first frame update
    void Start()
    {
        Objects = new List<GameObject>();
        Debug.Log(Objects.Count);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //生成関数

    //死亡管理


}
