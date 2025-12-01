using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LanternPickup: MonoBehaviour, IInteractable
{
    [SerializeField] private string prompt = "Взять фонарик (F)";
    [SerializeField] private float lanternDuration = 90f;
    [SerializeField] private GameObject playerLantern;
    [SerializeField] private TMP_Text timerText; // UI текст для таймера

    [SerializeField] private GameObject forchOnfloor;
    private Light light;

    public string InteractionPrompt => prompt;
    public bool IsInteractable => true;

    private Coroutine timerCoroutine;

    public void Interact()
    {
        forchOnfloor.SetActive(false);

        if (playerLantern != null)
             light =playerLantern.GetComponent<Light>();

        light.range = 15;

           // playerLantern.SetActive(true);

        if (timerCoroutine != null)
            StopCoroutine(timerCoroutine);

        timerCoroutine = StartCoroutine(LanternTimer());
    }

    private IEnumerator LanternTimer()
    {
        float timeLeft = lanternDuration;

        while (timeLeft > 0)
        {
            if (timerText != null)
                timerText.text = $" {Mathf.CeilToInt(timeLeft)}с";

            timeLeft -= Time.deltaTime;
            yield return null;
        }

        TimeEnd();
    }

    private void TimeEnd()
    {

        GameManager.Instance.playerHP--;
       GameManager.Instance.RestartScene();
        //if (playerLantern != null)
        // playerLantern.SetActive(false);

        // if (timerText != null)
        // timerText.text = "";

        // Debug.Log("Фонарь разрядился!");
    }
}