using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public Player P1;
    public Player P2;
    public GameState State = GameState.ChooseAttack;
    public GameObject P1Panel;
    public GameObject P2Panel;
    public GameObject menuPanel;
    public GameObject gameOverPanel;
    public TMP_Text winnerText;
    private Player damagedPlayer;
    private Player winner;
    public Vector2 bgSpawnPos;
    public Vector2 bgDeletePos;
    public GameDifficulty Difficulty = GameDifficulty.Medium;
    private Bot bot;
    public Button easyButton, mediumButton, hardButton, versusButton;
    private ColorBlock easyColorYes, mediumColorYes, hardColorYes, versusColorYes, 
                       easyColorNo, mediumColorNo, hardColorNo, versusColorNo;
    private Vector3 defaultButtonLocalScale;
    private DontDestroy DD;
    public GameObject P2Cards, P2HealthBar, P2HealthText;

    public enum GameDifficulty
    {
        Easy,
        Medium,
        Hard,
        Versus,
    }

    public enum GameState
    {
        ChooseAttack,
        Attacks,
        Damages,
        Draw,
        GameOver,
    }

    void Start()
    {
        DD = GameObject.FindWithTag("DontDestroy").GetComponent<DontDestroy>();

        P1Panel.SetActive(false);
        P2Panel.SetActive(false);
        gameOverPanel.SetActive(false);

        bgSpawnPos = GameObject.FindWithTag("BGspawn").GetComponent<Transform>().localPosition;
        bgDeletePos = GameObject.FindWithTag("BGdelete").GetComponent<Transform>().localPosition;
        Destroy(GameObject.FindWithTag("BGspawn").gameObject);

        Difficulty = GameDifficulty.Versus;
        bot = P2.GetComponent<Bot>();

        defaultButtonLocalScale = easyButton.GetComponent<Transform>().localScale;

        easyColorYes = easyColorNo = easyButton.colors;
        mediumColorYes = mediumColorNo = mediumButton.colors;
        hardColorYes = hardColorNo = hardButton.colors;
        versusColorYes = versusColorNo = versusButton.colors;

        easyColorYes.normalColor = easyButton.colors.selectedColor;
        easyColorNo.normalColor = easyButton.colors.normalColor;

        mediumColorYes.normalColor = mediumButton.colors.selectedColor;
        mediumColorNo.normalColor = mediumButton.colors.normalColor;

        hardColorYes.normalColor = hardButton.colors.selectedColor;
        hardColorNo.normalColor = hardButton.colors.normalColor;

        versusColorYes.normalColor = versusButton.colors.selectedColor;
        versusColorNo.normalColor = versusButton.colors.normalColor;

        switch (DD.GameDifficulty)
        {
            case "Easy" :
                SetToEasy();
                break;
            case "Medium" :
                SetToMedium();
                break;
            case "Hard" :
                SetToHard();
                break;
            case "Versus" :
                SetToVersus();
                break;
        }

        DD.audioSource.clip = DD.menuBGM;
        DD.audioSource.Play();
    }

    void Update()
    {
        switch (State)
        {
            case GameState.ChooseAttack :
                if (P1.AttackValue != null && P2.AttackValue != null)
                {
                    P1.AnimateAttack();
                    P2.AnimateAttack();
                    P1.IsClickable(false);
                    P2.IsClickable(false);
                    State = GameState.Attacks;
                }
                break;


            case GameState.Attacks :
                if (P1.IsAnimating() == false && P2.IsAnimating() == false)
                {
                    damagedPlayer = GetDamagedPlayer();
                    if (damagedPlayer != null)
                    {
                        damagedPlayer.AnimateDamage();
                        State = GameState.Damages;
                    }
                    else
                    {
                        P1.AnimateDraw();
                        P2.AnimateDraw();
                        State = GameState.Draw;
                    }
                }
                break;


            case GameState.Damages :
                if (P1.IsAnimating() == false && P2.IsAnimating() == false)
                {
                    switch (Difficulty)
                    {
                        case GameDifficulty.Easy :
                            if (damagedPlayer == P1)
                            {
                                P1.ChangeHealth(+10);
                                P2.ChangeHealth(-5);
                            }
                            else
                            {
                                P1.ChangeHealth(-10);
                                P2.ChangeHealth(+30);
                            }
                            break;
                        case GameDifficulty.Medium:
                            if (damagedPlayer == P1)
                            {
                                P1.ChangeHealth(+20);
                                P2.ChangeHealth(-10);
                            }
                            else
                            {
                                P1.ChangeHealth(-10);
                                P2.ChangeHealth(+20);
                            }
                            break;
                        case GameDifficulty.Hard:
                            if (damagedPlayer == P1)
                            {
                                P1.ChangeHealth(+30);
                                P2.ChangeHealth(-10);
                            }
                            else
                            {
                                P1.ChangeHealth(-5);
                                P2.ChangeHealth(+10);
                            }
                            break;
                        case GameDifficulty.Versus:
                            if (damagedPlayer == P1)
                            {
                                P1.ChangeHealth(+20);
                                P2.ChangeHealth(-10);
                            }
                            else
                            {
                                P1.ChangeHealth(-10);
                                P2.ChangeHealth(+20);
                            }
                            break;
                    }

                    var winner = GetWinner();

                    if (winner == null)
                    {
                        //ResetPlayers();
                        //P1.IsClickable(true);
                        //P2.IsClickable(true);
                        //State = GameState.ChooseAttack;
                        P1.AnimateAfterDamage();
                        P2.AnimateAfterDamage();
                        State = GameState.Draw;
                    }
                    else
                    {
                        Debug.Log(winner + " wins");
                        gameOverPanel.SetActive(true);
                        //winnerText.text = winner == P1 ? "Player 1 wins" : "Player 2 wins";
                        if (winner == P1)
                        {
                            if (Difficulty != GameDifficulty.Versus)
                            {
                                winnerText.color = new Color(0f / 255f, 225f / 255f, 0f / 255f);
                                winnerText.text = "You win!";
                            }
                            else
                            {
                                winnerText.color = new Color(0f / 255f, 225f / 255f, 0f / 255f);
                                winnerText.text = "Player 1 wins!";
                            }
                        }
                        else
                        {
                            if (Difficulty != GameDifficulty.Versus)
                            {
                                winnerText.color = new Color(225f / 255f, 0f / 255f, 0f / 255f);
                                winnerText.text = "You lose...";
                            }
                            else
                            {
                                gameOverPanel.GetComponent<RectTransform>().Rotate(0, 0, 180);
                                winnerText.color = new Color(0f / 255f, 225f / 255f, 0f / 255f);
                                winnerText.text = "Player 2 wins!";
                            }
                        }
                        //ResetPlayers();
                        P1.AnimateAfterDamage();
                        P2.AnimateAfterDamage();
                        State = GameState.GameOver;
                    }
                }
                break;


            case GameState.Draw :
                if (P1.IsAnimating() == false && P2.IsAnimating() == false)
                {
                    ResetPlayers();
                    P1.IsClickable(true);
                    P2.IsClickable(true);
                    State = GameState.ChooseAttack;
                    //P1.ResetAllCards();
                    //P2.ResetAllCards();
                }
                break;

            case GameState.GameOver :
                if (P1.IsAnimating() == false && P2.IsAnimating() == false)
                {
                    ResetPlayers();
                }
                break;
        }
    }

    private void ResetPlayers()
    {
        damagedPlayer = null;
        P1.Reset();
        P2.Reset();
    }

    private Player GetDamagedPlayer()
    {
        Attack? PlayerAtk1 = P1.AttackValue;
        Attack? PlayerAtk2 = P2.AttackValue;

        if (PlayerAtk1 == Attack.Rock && PlayerAtk2 == Attack.Paper)
        {
            return P1;
        }
        else if (PlayerAtk1 == Attack.Rock && PlayerAtk2 == Attack.Scissors)
        {
            return P2;
        }
        else if (PlayerAtk1 == Attack.Paper && PlayerAtk2 == Attack.Rock)
        {
            return P2;
        }
        else if (PlayerAtk1 == Attack.Paper && PlayerAtk2 == Attack.Scissors)
        {
            return P1;
        }
        else if (PlayerAtk1 == Attack.Scissors && PlayerAtk2 == Attack.Rock)
        {
            return P1;
        }
        else if (PlayerAtk1 == Attack.Scissors && PlayerAtk2 == Attack.Paper)
        {
            return P2;
        }

        return null;
    }

    private Player GetWinner()
    {
        if (P1.health == 100)
        {
            return P2;
        }
        else if (P2.health == 100)
        {
            return P1;
        }
        else
        {
            return null;
        }
    }

    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void StartGame()
    {
        bot.SetInterval();
        menuPanel.SetActive(false);
        P1Panel.SetActive(true);
        P2Panel.SetActive(true);

        if (Difficulty == GameDifficulty.Versus)
        {
            P2Cards.GetComponent<RectTransform>().Rotate(0, 0, 180);
            P2HealthBar.GetComponent<RectTransform>().Rotate(0, 0, 180);
            P2HealthText.GetComponent<RectTransform>().Rotate(0, 0, 180);
        }
        else
        {
            P2.DisableRaycastTarget();
        }

        DD.audioSource.clip = DD.levelBGM;
        DD.audioSource.Play();
    }

    public void SetToEasy()
    {
        Difficulty = GameDifficulty.Easy;
        DD.GameDifficulty = "Easy";
        easyButton.transform.localScale 
            = mediumButton.transform.localScale 
            = hardButton.transform.localScale 
            = versusButton.transform.localScale 
            = defaultButtonLocalScale;
        easyButton.transform.DOScale(easyButton.transform.localScale * 1.1f, 0.2f);
        easyButton.colors = easyColorYes;
        mediumButton.colors = mediumColorNo;
        hardButton.colors = hardColorNo;
        versusButton.colors = versusColorNo;
    }

    public void SetToMedium()
    {
        Difficulty = GameDifficulty.Medium;
        DD.GameDifficulty = "Medium";
        easyButton.transform.localScale
            = mediumButton.transform.localScale
            = hardButton.transform.localScale
            = versusButton.transform.localScale
            = defaultButtonLocalScale;
        mediumButton.transform.DOScale(easyButton.transform.localScale * 1.1f, 0.2f);
        easyButton.colors = easyColorNo;
        mediumButton.colors = mediumColorYes;
        hardButton.colors = hardColorNo;
        versusButton.colors = versusColorNo;
    }

    public void SetToHard()
    {
        Difficulty = GameDifficulty.Hard;
        DD.GameDifficulty = "Hard";
        easyButton.transform.localScale
            = mediumButton.transform.localScale
            = hardButton.transform.localScale
            = versusButton.transform.localScale
            = defaultButtonLocalScale;
        hardButton.transform.DOScale(easyButton.transform.localScale * 1.1f, 0.2f);
        easyButton.colors = easyColorNo;
        mediumButton.colors = mediumColorNo;
        hardButton.colors = hardColorYes;
        versusButton.colors = versusColorNo;
    }

    public void SetToVersus()
    {
        Difficulty = GameDifficulty.Versus;
        DD.GameDifficulty = "Versus";
        easyButton.transform.localScale
            = mediumButton.transform.localScale
            = hardButton.transform.localScale
            = versusButton.transform.localScale
            = defaultButtonLocalScale;
        versusButton.transform.DOScale(easyButton.transform.localScale * 1.1f, 0.2f);
        easyButton.colors = easyColorNo;
        mediumButton.colors = mediumColorNo;
        hardButton.colors = hardColorNo;
        versusButton.colors = versusColorYes;
    }
}
