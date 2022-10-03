using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : MonoBehaviour
{
    public Player player;
    public GameManager GM;
    public float choosingInterval;
    private float timer = 0;
    int lastSelected = 0;
    Card[] cards;

    void Start()
    {
        cards = GetComponentsInChildren<Card>();
    }

    void Update()
    {
        if (GM.State != GameManager.GameState.ChooseAttack)
        {
            timer = 0;
            return;
        }

        if (timer < choosingInterval)
        {
            timer += Time.deltaTime;
            return;
        }

        timer = 0;
        ChooseAttack();
    }

    public void ChooseAttack()
    {
        /*
            var random = Random.Range(1, cards.Length);
            var selection = (lastSelected + random) % cards.Length;
            // last + random % length = value
            // (0 + 1) % 3 = 1
            // (0 + 2) % 3 = 2
            // (1 + 1) % 3 = 2
            // (1 + 2) % 3 = 0
            // (2 + 1) % 3 = 0
            // (2 + 2) % 3 = 1
            player.SetChosenCard(cards[selection]);
            lastSelected = selection;
        */

        var random = Random.Range(0, 3);
        player.SetChosenCard(cards[random]);
        lastSelected = random;
    }

    public void SetInterval()
    {
        //switch (GM.Difficulty)
        //{
        //    case GameManager.GameDifficulty.Easy :
        //        choosingInterval = 1f;
        //        break;
        //    case GameManager.GameDifficulty.Medium :
        //        choosingInterval = 0.5f;
        //        break;
        //    case GameManager.GameDifficulty.Hard :
        //        choosingInterval = 0.2f;
        //        break;
        //    case GameManager.GameDifficulty.Versus :
        //        enabled = false;
        //        break;
        //}

        if (GM.Difficulty != GameManager.GameDifficulty.Versus)
        {
            choosingInterval = 0.2f;
        }
        else
        {
            enabled = false;
        }
    }
}
