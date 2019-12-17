using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChageScene : MonoBehaviour
{
    [SerializeField]
    [Range(0.0f, 5.0f)]
    private float _fadeTime;

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


    public static ChageScene instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        if(_fadetype==FadeType.FadeOut)
        {
            PushStart();
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (_cangetype)
        { 
            case ChangeType.TimeLimit:
                StartCoroutine(LimitMove());
                break;
            case ChangeType.Fade:
                StartCoroutine(fadeMove());
                break;
        }   
    }

    public void PushStart()
    {
        Test = true;
    }

    public void SetFadeColor(Color change)
    {
        GetComponent<FadeImage>().SetColor(change);
    }

    private IEnumerator LimitMove()
    {
        yield return new WaitForSeconds(_fadeTime);
        LoadScene();
    }

    private IEnumerator fadeMove()
    {
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
                    LoadScene();
                    break;
                case FadeType.FadeOut:
                    _fadetype = FadeType.FadeIn;
                    fade.IsEnd = false;
                    break;
            }
        }
        yield break;
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(_SceneName);
    }
}
