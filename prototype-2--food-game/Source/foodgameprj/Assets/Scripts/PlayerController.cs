﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Range(0f, 1f)] public float moveAmount;

    public int playerScore;
    public int playerHeartsCount;

    private UiManager uiManager;
    private int lostFoods;
    private int maxLostFoodsToDecreaseHearts = 3;

    private void Start()
    {
        playerScore = 0;
        uiManager = FindObjectOfType<UiManager>();
        lostFoods = maxLostFoodsToDecreaseHearts;
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(moveAmount, 0, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += new Vector3(-moveAmount, 0, 0);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Food"))
        {
            // access the food object config
            FoodItemConfig conf = collision.gameObject.GetComponent<FoodInstanceController>().config;

            // increase the player's score
            UpdatePlayerScore(conf.score);

            Debug.Log("SCORE: " + playerScore);

            // destroy the food object
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("Combo"))
        {
            // polymorphism!
            // for example, the object of type "TimeFreezerComboController" which is the child of "ComboInstanceController", is put inside the "comboController" object below.
            ComboInstanceController comboController =  collision.gameObject.GetComponent<ComboInstanceController>();
            // the CONTENT of OnConsume method inside "TimeFreezerComboController" is available inside the "comboController"
            comboController.OnConsume();

            Debug.Log("COMBO!!! " + comboController.config.comboName);

            // destroy the combo object
            Destroy(collision.gameObject);
            
        }
    }

    internal void LostOneFood()
    {
        if(--lostFoods < 1)
        {
            UpdateHeartsCount(-1);
            lostFoods = maxLostFoodsToDecreaseHearts;
        }
    }

    public void UpdatePlayerScore(int score)
    {
        playerScore += score;
        uiManager.UpdateScoreText(playerScore);
    }

    public void UpdateHeartsCount(int updateAmount)
    {
        playerHeartsCount += updateAmount;
        uiManager.UpdateHeartCountText(playerHeartsCount);
    }
}
