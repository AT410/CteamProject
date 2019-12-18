using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEditor;

public enum ObjectType
{
    Default,
    Enemy,
    Box
}

public class EditDatas : EditorWindow
{
    [MenuItem("EditData/TEST")]
    private static void Create()
    {
        GetWindow<EditDatas>("TEST");
    }

    /// <summary>
    /// Editor用メンバ変数
    /// </summary>

    public ObjectType _type = ObjectType.Enemy;

    public EnemyType _Etype = EnemyType.Zomib;

    //
    private ObjectData _result;

    //ファイル名
    private string _filename = "NULL";
    //出力ファイルパス
    private const string EXPORT_PATH = "Assets/Resources/";

    private void OnGUI()
    {
        if(_result ==null)
        {
            _result = CreateInstance<ObjectData>();
        }

        Color defaultColor = GUI.backgroundColor;
        using (new GUILayout.VerticalScope(EditorStyles.helpBox))
        {
            GUI.backgroundColor = Color.gray;
            using (new GUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                GUILayout.Label("ファイル名");
            }
            GUI.backgroundColor = defaultColor;
            _filename = EditorGUILayout.TextField(_filename); ;
            using (new GUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                GUI.backgroundColor = Color.magenta;
                GUILayout.Label("Type");
                GUI.backgroundColor = defaultColor;
                _type = (ObjectType)EditorGUILayout.EnumPopup(_type);
            }
            GUI.backgroundColor = defaultColor;
            using (new GUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                GUI.backgroundColor = Color.magenta;
                GUILayout.Label("Type");
                GUI.backgroundColor = defaultColor;
                _Etype = (EnemyType)EditorGUILayout.EnumPopup(_Etype);
            }
            GUI.backgroundColor = defaultColor;
            using (new GUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                GUI.backgroundColor = Color.magenta;
                GUILayout.Label("IsLimit");
                GUI.backgroundColor = defaultColor;
                _result.GetDelFlag = EditorGUILayout.Toggle(_result.GetDelFlag);
            }
            GUI.backgroundColor = defaultColor;
            using (new GUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                GUI.backgroundColor = Color.magenta;
                GUILayout.Label("MoveSpeed");
                GUI.backgroundColor = defaultColor;
                _result.ISMoveSpeed = EditorGUILayout.FloatField(_result.ISMoveSpeed);
            }

        }

        using (new GUILayout.VerticalScope(EditorStyles.helpBox))
        {
            GUI.backgroundColor = Color.green;
            // データを作成する
            if (GUILayout.Button("作成"))
            {
                _result.type = _type;
                _result.Etype = _Etype;
                Export();
                EditorUtility.DisplayDialog("", "", "OK");
            }
            GUI.backgroundColor = defaultColor;
        }
    }

    private void Export()
    {
        var PATH = EXPORT_PATH + _filename + ".asset";
        var result = AssetDatabase.LoadAssetAtPath<ObjectData>(PATH);
        if(result)
        {
            EditorUtility.DisplayDialog("Extention", "同名ファイルが存在しています!", "OK");
            return;
        }

        // 新規の場合は作成
        if (!AssetDatabase.Contains(_result as UnityEngine.Object))
        {
            string directory = Path.GetDirectoryName(PATH);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            // アセット作成
            AssetDatabase.CreateAsset(_result, PATH);
        }
        // コピー
        //result.Copy(_result);

        // 直接編集できないようにする
        _result.hideFlags = HideFlags.NotEditable;
        // 更新通知
        EditorUtility.SetDirty(_result);
        // 保存
        AssetDatabase.SaveAssets();
        // エディタを最新の状態にする
        AssetDatabase.Refresh();
        _result = null;
    }
}