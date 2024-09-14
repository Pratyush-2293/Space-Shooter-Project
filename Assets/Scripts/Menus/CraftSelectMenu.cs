using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftSelectMenu : Menu
{
    public static CraftSelectMenu instance = null;

    public Image player1ShipA = null;
    public Image player1ShipB = null;
    public Image player1ShipC = null;
    public Image player1ShipX = null;
    public Image player1ShipZ = null;

    public Image player2ShipA = null;
    public Image player2ShipB = null;
    public Image player2ShipC = null;
    public Image player2ShipX = null;
    public Image player2ShipZ = null;

    public Slider powerSlider1 = null;
    public Slider speedSlider1 = null;
    public Slider beamSlider1 = null;
    public Slider bombSlider1 = null;
    public Slider optionsSlider1 = null;

    public Slider powerSlider2 = null;
    public Slider speedSlider2 = null;
    public Slider beamSlider2 = null;
    public Slider bombSlider2 = null;
    public Slider optionsSlider2 = null;

    public Text countdownText = null;
    public Text player2StartText = null;

    public GameObject player2Panel = null;

    private float lastUnscaledTime = 0;
    private float timer = 5.9f;
    private bool countdown = false;

    private void Start()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one CraftSelectMenu");
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public void Reset()
    {
        countdownText.gameObject.SetActive(false);
        countdown = false;
        timer = 5.9f;
    }

    public override void TurnOn(Menu previous)
    {
        base.TurnOn(previous);
        Reset();
    }

    private void FixedUpdate()
    {
        if (countdown)
        {
            float dUnscaled = Time.unscaledTime - lastUnscaledTime;
            lastUnscaledTime = Time.unscaledTime;
            timer -= dUnscaled;
            countdownText.text = ((int)timer).ToString();

            if (timer < 1)
            {
                GameManager.instance.StartGame();
            }
        }
    }

    public void OnPlayButton()
    {
        StartCountdown();
    }

    private void StartCountdown()
    {
        timer = 5.9f;
        lastUnscaledTime = Time.unscaledTime;
        countdown = true;
        countdownText.gameObject.SetActive(true);
    }

    private void StopCountdown()
    {
        countdown = false;
        countdownText.gameObject.SetActive(false);
    }

    public void OnCraftA_P1Button()
    {

    }

    public void OnCraftB_P1Button()
    {

    }

    public void OnCraftC_P1Button()
    {

    }

    public void OnCraftX_P1Button()
    {

    }

    public void OnCraftZ_P1Button()
    {

    }

    public void OnCraftA_P2Button()
    {

    }

    public void OnCraftB_P2Button()
    {

    }

    public void OnCraftC_P2Button()
    {

    }

    public void OnCraftX_P2Button()
    {

    }

    public void OnCraftZ_P2Button()
    {

    }

    public void OnBackButton()
    {
        TurnOff(true);
    }
}
