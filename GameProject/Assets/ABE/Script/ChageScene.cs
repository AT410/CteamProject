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
    private string _SceneName;

    public bool Test;

    private enum FadeType
    {
        FadeIn,
        FadeOut
    };

    [SerializeField]
    private FadeType _fadetype;

    Color _alpha;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Test)
        {
            fade();
        }
        else
        {
            _currntTime = 0;
        }
    }

    private void fade()
    {
        _currntTime += Time.deltaTime;
        switch (_fadetype)
        {
            case FadeType.FadeIn:
                _alpha.a = 1.0f - (_currntTime) / _fadeTime;
                GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, _alpha.a);
                if (_alpha.a < 0) { Test = !Test; _fadetype = FadeType.FadeOut;}
                break;
            case FadeType.FadeOut:
                _alpha.a = (_currntTime) / _fadeTime;
                GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, _alpha.a);
                if (_alpha.a > 1) LoadScene();
                break;
        }
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(_SceneName);
    }
}
