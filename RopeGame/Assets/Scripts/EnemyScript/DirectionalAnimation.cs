using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator _anim;
    public Animator Anim { get { return this._anim ? this._anim : this._anim = GetComponent<Animator>(); } }
    public Vector2 Direction { get; private set; }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
