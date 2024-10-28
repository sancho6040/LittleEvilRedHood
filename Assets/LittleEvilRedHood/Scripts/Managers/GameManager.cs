using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI elements")]
    public FadingPanel MissionUI;
    public FadingPanel TopMissionUI;
    public FadingPanel EndModalUI;
    public FadingPanel LoseModalUI;

    [Header("Game Actors")]
    private PlayerController _player;
    private List<BasicEnemie> _enemiesList;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

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
            EndGame();
        }
    }

    private IEnumerator InitGame()
    {
        _player.IsControlsActive = false;

        //---------- muestra la mision --------------
        MissionUI.gameObject.SetActive(true);
        TopMissionUI.gameObject.SetActive(true);
        MissionUI.FadeIn(0.5f);
        yield return new WaitForSeconds(2f);

        MissionUI.FadeOut(0.5f);
        TopMissionUI.FadeIn(0.5f);

        yield return new WaitForSeconds(0.5f);
        MissionUI.gameObject.SetActive(false);

        //---------- Activa al jugador --------------
        _player.IsControlsActive = true;
    }

    private void EndGame()
    {
        _player.IsControlsActive = false;
        EndModalUI.FadeIn(0.5f);
    }

    public void GameLost()
    {
        LoseModalUI.gameObject.SetActive(true);
        LoseModalUI.FadeIn(0.5f);
    }

    public void RestartGame()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(index);
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }
    public void UnpauseGame()
    {
        Time.timeScale = 1f;
    }

    public void ExitGame()
    {
        SceneManager.LoadScene(0);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

}
