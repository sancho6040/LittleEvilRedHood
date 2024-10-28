using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI elements")]
    public TMP_Text MissionUI;
    public TMP_Text TopMissionUI;
    public Button MissionCompleteButton;

    [Header("Game Actors")]
    private PlayerController _player;
    private List<BasicEnemie> _enemiesList;

    private void Start()
    {
        _player = FindAnyObjectByType<PlayerController>();
        _enemiesList = new List<BasicEnemie>(FindObjectsOfType<BasicEnemie>());

        StartCoroutine(InitGame());
    }
    public void EnemieCaptured(BasicEnemie inEnemie)
    {
        _enemiesList.Remove(inEnemie);
        if (_enemiesList.Count <= 0)
        {
            StartCoroutine(EndGame());
        }
    }

    private IEnumerator InitGame()
    {
        _player.IsControlsActive = false;

        //---------- muestra la mision --------------
        MissionUI.gameObject.SetActive(true);
        TopMissionUI.gameObject.SetActive(true);
        TopMissionUI.CrossFadeAlpha(0f, 0f, false);
        yield return new WaitForSeconds(2f);

        MissionUI.CrossFadeAlpha(0f, 0.5f, false);
        yield return new WaitForSeconds(0.5f);
        TopMissionUI.CrossFadeAlpha(1f, 0.5f, false);
        MissionUI.gameObject.SetActive(false);

        //---------- Activa al jugador --------------
        _player.IsControlsActive = true;
    }

    private IEnumerator EndGame()
    {
        _player.IsControlsActive = false;

        MissionUI.gameObject.SetActive(true);
        MissionUI.text = "Mission completa!";
        TopMissionUI.CrossFadeAlpha(1f, 0.5f, false);
        yield return new WaitForSeconds(0.5f);
        MissionCompleteButton.gameObject.SetActive(true);

    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }
    public void UnpauseGame()
    {
        Time.timeScale = 1f;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

}
