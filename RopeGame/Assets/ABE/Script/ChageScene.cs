using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChageScene : MonoBehaviour
{
    

    [SerializeField]
    [Range(0.0f, 50.0f)]
    private float _fadeTime;

    public float FadeTime
    {
        get
        {
            return _fadeTime;
        }
        set
        {
            _fadeTime = value;
        }
    }

    private float _currntTime;

    [SerializeField]
    private Fade fade;

    [SerializeField]
    private string _SceneName;


    public string SceneName
    {
        get
        {
            return _SceneName;
        }

        set
        {
            _SceneName = value;
        }
    }

    public bool Test;

    public enum ChangeType
    {
        Fade,
        TimeLimit
    }

    public enum FadeType
    {
        FadeIn,
        FadeOut,
    };

    [SerializeField]
    private ChangeType _cangetype;

    [SerializeField]
    private FadeType _fadetype;

    public FadeType IsFadeType
    {
        get
        {
            return _fadetype;
        }
        set
        {
            _fadetype = value;
        }
    }

    //実行中フラグ
    public bool IsRunning = false;

    public bool EndExe = false;

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.PlayBGM("Title");  //タイトルBGM再生
        if (_fadetype==FadeType.FadeOut && IsRunning ==false)
        {
            PushStart();
        }
        //AudioManager.Instance.PlayBGM("Title");  //タイトルBGM再生
    }

    // Update is called once per frame
    void Update()
    {
        if (IsRunning)
            return;

        switch (_cangetype)
        { 
            case ChangeType.TimeLimit:
                GoCoroutine(LimitMove());
                break;
            case ChangeType.Fade:
                GoCoroutine(fadeMove());
                break;
        }   
    }

    public void PushStart()
    {
        Test = true;
        GoCoroutine(fadeMove());
    }

    private void GoCoroutine(IEnumerator Active)
    {
        GlobalCoroutine.Go(Active);
    }

    public void SetFadeColor(Color change)
    {
        GetComponent<FadeImage>().SetColor(change);
    }

    private IEnumerator LimitMove()
    {
        yield return new WaitForSeconds(_fadeTime);
        IsRunning = true;
        LoadScene();
        IsRunning = false;
        
    }

    private IEnumerator fadeMove()
    {
        if (IsRunning)
            yield break;
        IsRunning = true;

        if (Test)
        {
            switch (_fadetype)
            {
                case FadeType.FadeIn:
                    fade.FadeIn(_fadeTime);
                    break;
                case FadeType.FadeOut:
                    fade.FadeOut(_fadeTime);
                    break;
            }
            Test = !Test;
        }

        if (fade.IsEnd)
        {
            switch (_fadetype)
            {
                case FadeType.FadeIn:
                    fade.IsEnd = false;
                    AudioManager.Instance.FadeOutBGM(); //BGMフェードアウト
                    LoadScene();
                    break;
                case FadeType.FadeOut:
                    _fadetype = FadeType.FadeIn;
                    fade.IsEnd = false;
                    break;
            }
        }
        IsRunning = false;
        yield break;
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(_SceneName);
    }
}
