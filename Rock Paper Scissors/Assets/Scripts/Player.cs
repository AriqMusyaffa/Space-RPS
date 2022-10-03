using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class Player : MonoBehaviour
{
    public Transform atkPosRef;
    public Card chosenCard;
    public HealthBar healthBar;
    public TMP_Text healthText;
    public float health;
    public float minHealth;
    public float maxHealth;
    private Tweener animationTweener;
    private GameManager GM;

    void Start()
    {
        health = minHealth;
        GM = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }

    public Attack? AttackValue
    {
        get => chosenCard == null ? null : chosenCard.AttackValue;
    }

    public void Reset()
    {
        if (chosenCard != null)
        {
            chosenCard.Reset();
        }

        chosenCard = null;
    }

    public void SetChosenCard(Card newCard)
    {
        if (chosenCard != null)
        {
            //chosenCard.Reset();
            ResetAllCards();
        }

        chosenCard = newCard;

        if (GM.Difficulty != GameManager.GameDifficulty.Versus)
        {
            chosenCard.transform.DOScale(chosenCard.transform.localScale * 1.1f, 0.2f);
        }
    }

    public void ChangeHealth(float amount)
    {
        health += amount;
        health = Mathf.Clamp(health, 0, 100);
        healthBar.UpdateBar(health / maxHealth);
        healthText.text = (100 - health) + " / " + maxHealth;
    }

    public void AnimateAttack()
    {
        if (GM.Difficulty == GameManager.GameDifficulty.Versus)
        {
            chosenCard.transform.DOScale(chosenCard.transform.localScale * 1.1f, 0.2f);
        }

        animationTweener = chosenCard.transform
            .DOMove(atkPosRef.position, 0.5f);
    }

    public void AnimateDamage()
    {
        var image = chosenCard.transform.GetComponent<Image>();
        animationTweener = image
            .DOColor(Color.red, 0.1f)
            .SetLoops(3, LoopType.Yoyo)
            .SetDelay(0.2f);
    }

    public void AnimateAfterDamage()
    {
        animationTweener = chosenCard.transform
            .DOMove(chosenCard.originalPosition, 0.5f)
            .SetDelay(0.2f);
    }

    public void AnimateDraw()
    {
        animationTweener = chosenCard.transform
            .DOMove(chosenCard.originalPosition, 0.5f)
            .SetEase(Ease.InBack)
            .SetDelay(0.2f);
    }

    public bool IsAnimating()
    {
        return animationTweener.IsActive();
    }

    public void IsClickable(bool value)
    {
        Card[] cards = GetComponentsInChildren<Card>();
        foreach (var card in cards)
        {
            card.SetClickable(value);
        }
    }

    public void ResetAllCards()
    {
        Card[] cards = GetComponentsInChildren<Card>();
        foreach (var card in cards)
        {
            card.Reset();
        }
    }

    public void DisableRaycastTarget()
    {
        Card[] cards = GetComponentsInChildren<Card>();
        foreach (var card in cards)
        {
            card.GetComponent<Image>().raycastTarget = false;
        }
    }
}
