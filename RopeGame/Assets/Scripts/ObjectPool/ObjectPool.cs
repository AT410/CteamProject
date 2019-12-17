using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*オブジェクトプール
  *製作者　篠﨑*/
public class ObjectPool : MonoBehaviour
{
    [SerializeField]
    GameObject m_poolObj; // プールするオブジェクト。
    [SerializeField]
    internal int m_createObj; // 最初に生成するオブジェクトの数。
    private List<GameObject> m_poolObjList; // 生成した弾用のリスト。このリストの中から未使用のものを探したりする 

    void Awake()
    {
        CreatePool();
    }
    /// <summary>
    /// 最初にある程度の数、オブジェクトを作成してプールしておく処理
    /// </summary>
    private void CreatePool()
    {
        m_poolObjList = new List<GameObject>();
        for (int i = 0; i < m_createObj; i++)
        {
            var newObj = CreateNewObject(); // 弾を生成して
            newObj.SetActive(false); // 物理演算を切って(=未使用にして)
            m_poolObjList.Add(newObj); // リストに保存しておく
        }
    }
    /// <summary>
    /// 未使用の弾を探して返す処理、未使用のものがなければ新しく作って返す
    /// </summary>
    /// <returns></returns>
    public GameObject GetObject()
    {
        // 使用中でないものを探して返す
        foreach (var obj in m_poolObjList)
        {
            if (obj.activeSelf == false)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        // 全て使用中だったら新しく作り、リストに追加してから返す
        var newObj = CreateNewObject();
        m_poolObjList.Add(newObj);
        return newObj;
    } 
    /// <summary>
    /// 新しく弾を作成する処理
    /// </summary>
    /// <returns></returns>
    private GameObject CreateNewObject()
    {
        var pos = new Vector2(100, 100); // 画面外であればどこでもOK
        var newObj = Instantiate(m_poolObj, pos, Quaternion.identity); // 弾を生成しておいて
        newObj.name = m_poolObj.name + (m_poolObjList.Count + 1); // 名前を連番でつけてから
        return newObj; // 返す
    }
}