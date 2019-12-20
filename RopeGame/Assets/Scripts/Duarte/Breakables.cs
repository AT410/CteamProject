using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakables : MonoBehaviour
{
    //アイテムドロップ変数 Variables for item's drop
    public GameObject[] brokenPieces;
    public int maxPieces = 5;
    public bool shouldDropItem;
    public GameObject[] itemsToDrop;
    public float itemDropPercent;
    private string BoxDestroyEffect = "BoxDestroyEffect";

    void Start()
    {
    }
    void Update()
    {
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            //スキルでレベルオブジェクトを破壊するために To destroy the level's objets by skill
            if (PlayerController.instance.dashCounter > 0)
            {
                EffectManager.instance.playInPlace(transform.position, BoxDestroyEffect);
                Destroy(gameObject);

                //破壊されたレベルオブジェクトのパーツのドロップのために To drop the pieces of the broken level's objetcs
                int piecesToDrop = Random.Range(1, maxPieces);

                for(int i = 0; i < piecesToDrop; i++)
                {
                    int randomPiece = Random.Range(0, brokenPieces.Length);

                    Instantiate(brokenPieces[randomPiece], transform.position, transform.rotation);
                }

                //アイテムドロップ Drop items
                if (shouldDropItem)
                {
                    float dropChance = Random.Range(0f, 100f);

                    if(dropChance < itemDropPercent)
                    {
                        int randomItem = Random.Range(0, itemsToDrop.Length);

                        Instantiate(itemsToDrop[randomItem], transform.position, transform.rotation);
                    }

                }
            }
        }
    }
}
