﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;

    public int currentHealth;
    public int maxHealth;

    public float damageInvincLength = 1f;
    private float invictCount;

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
        UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if(invictCount > 0)
        {
            invictCount -= Time.deltaTime;

            if(invictCount <= 0)
            {
                PlayerController.instance.bodySR.color = new Color(PlayerController.instance.bodySR.color.r, PlayerController.instance.bodySR.color.g, PlayerController.instance.bodySR.color.b, 1f);
            }
        }
    }

    public void DamagePlayer()
    {
        if(invictCount <= 0)
        {
           currentHealth--;

            invictCount = damageInvincLength;

            PlayerController.instance.bodySR.color = new Color(PlayerController.instance.bodySR.color.r, PlayerController.instance.bodySR.color.g, PlayerController.instance.bodySR.color.b, .5f);
        
           if(currentHealth <= 0)
           {
               PlayerController.instance.gameObject.SetActive(false);

                ChageScene.instance.SceneName = "GameOverScene";
                Color DeathFadeC = new Color(201.0f/255.0f,51.0f/255.0f,41.0f/255.0f);
                ChageScene.instance.SetFadeColor(DeathFadeC);
                ChageScene.instance.PushStart();
               //UIController.instance.deathScreen.SetActive(true);
           }

           UIController.instance.healthSlider.value = currentHealth;
           UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
        }
    }
}