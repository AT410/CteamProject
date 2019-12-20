using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public GameObject[] hpBrandy;

    public static PlayerHealthController instance;
    private string PlayerDeathEffect = "PlayerDeathEffect";
    public int currentHealth;
    public int maxHealth;
    public string BloodEffect = "BloodEffect";
    float damageInvincLength = 1f;
    private float invincCount;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        UIController.instance.healthSlider.maxValue = maxHealth;
        UIController.instance.healthSlider.value = currentHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(!EventScript.Instance().GetStateMachine().ChackState(DefaultEventState.Instance()))
        {
            return;
        }

        if(invincCount > 0)
        {
            invincCount -= Time.deltaTime;

            if(invincCount <= 0)
            {
                PlayerController.instance.bodySR.color = new Color(PlayerController.instance.bodySR.color.r, PlayerController.instance.bodySR.color.g, PlayerController.instance.bodySR.color.b, 1f);
            }
        }
    }

    public void DamagePlayer()
    {
        if (invincCount <= 0)
        {
            currentHealth-=1;
            // EffectManager.instance.playInPlace(PlayerController.instance.playerTrans.position, BloodEffect);
            invincCount = damageInvincLength;
            
            PlayerController.instance.bodySR.color = new Color(PlayerController.instance.bodySR.color.r, PlayerController.instance.bodySR.color.g, PlayerController.instance.bodySR.color.b, .5f);
            AudioManager.Instance.PlaySE("Damage");  //ダメージSE再生
            hpBrandy[currentHealth].SetActive(false);

            if (currentHealth <= 0)
            {    
                PlayerController.instance.gameObject.SetActive(false);
                var Temp = GameObject.Find("ChangeScene");
                if(Temp)
                {
                    Temp.GetComponent<ChageScene>().SceneName = "GameOverScene";
                    Color DeathFadeC = new Color(201.0f / 255.0f, 51.0f / 255.0f, 41.0f / 255.0f);
                    Temp.GetComponent<ChageScene>().SetFadeColor(DeathFadeC);
                    Temp.GetComponent<ChageScene>().PushStart();
                }
                //UIController.instance.deathScreen.SetActive(true);
            }

            //UIController.instance.healthSlider.value = currentHealth;
            //UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
        }
    }

    public void MakeInvincible(float length)
    {
        invincCount = length;
        PlayerController.instance.bodySR.color = new Color(PlayerController.instance.bodySR.color.r, PlayerController.instance.bodySR.color.g, PlayerController.instance.bodySR.color.b, .5f);

    }

    public void HealPlayer(int healAmount)
    {
        hpBrandy[currentHealth].SetActive(true);
        AudioManager.Instance.PlaySE("Drink");  //回復SE再生

        currentHealth += healAmount;
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        UIController.instance.healthSlider.value = currentHealth;

    }
}
