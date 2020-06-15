using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;
using UnityEngine.SceneManagement;

public enum GameState { gameMenu,gamePlay,gamePause,gameOver,gameVictory}

public class GameManager : Manager<GameManager> {

    #region Attributs
    //Game Level
    public static int levelIndex = 1;

	//Game State
	private GameState m_GameState;
	public bool IsPlaying { get { return m_GameState == GameState.gamePlay; } }

    //Key
    private static bool m_Key = false;

    //Audio
    private GameObject audio;

    //Chrono
    private float m_TimeScale;
    public float TimeScale { get { return m_TimeScale; } }
    public static List<DateTime> bestChronos = new List<DateTime>();
    public static string dateFormat = "mm:ss";
    private static DateTime m_Chrono = new DateTime(2000, 1, 1, 0, 0, 0);

    #endregion


    #region Chrono fonctions
    
    
    public static void initBestChronos()
    {
        if (bestChronos.Count < 1)
        {
            for (int level = 0; level < 3; level++)
            {
                bestChronos.Add(new DateTime(2000, 1, 1, 0, 0, 0));
            }
        }
    }
    
    public static DateTime Chrono
    {
        get {
            return m_Chrono;
        }
        set
        {
            m_Chrono = value;
        }
    }

    public static void IncrementChrono()
    {
        SetChrono(m_Chrono.AddSeconds(1));
    }

    public static void SetChrono(DateTime chrono)
    {
        Chrono = chrono;
        EventManager.Instance.Raise(new GameStatisticsChangedEvent() { eBestChrono = bestChronos[levelIndex - 1], eChrono = m_Chrono, eKey = m_Key });
    }

    public static void SetBestChrono(DateTime chrono)
    {
        int min = DateTime.Compare(chrono, bestChronos[levelIndex - 1]);
        if ((min < 0) || (bestChronos[levelIndex - 1] == new DateTime(2000, 1, 1, 0, 0, 0)))
        {
            bestChronos[levelIndex - 1] = chrono;
            SetChrono(chrono);
        }
    }

    void SetTimeScale(float newTimeScale)
    {
        m_TimeScale = newTimeScale;
        Time.timeScale = m_TimeScale;
    }
    #endregion

    #region Sub et Unsub Event
    public override void SubscribeEvents()
	{
		base.SubscribeEvents();
		EventManager.Instance.AddListener<MainMenuButtonClickedEvent>(MainMenuButtonClicked);
		EventManager.Instance.AddListener<PlayButtonClickedEvent>(PlayButtonClicked);
		EventManager.Instance.AddListener<ResumeButtonClickedEvent>(ResumeButtonClicked);
        EventManager.Instance.AddListener<QuitButtonClickedEvent>(QuitButtonClicked);
        EventManager.Instance.AddListener<NextLevelButtonClickedEvent>(NextButtonClicked);
        EventManager.Instance.AddListener<TryAgainButtonClickedEvent>(TryAgainButtonClicked);
        EventManager.Instance.AddListener<EscapeButtonClickedEvent>(EscapeButtonClicked);

        EventManager.Instance.AddListener<PlayerHasBeenDetectedEvent>(PlayerHasBeenDetected);
        EventManager.Instance.AddListener<PlayerFindGoldEvent>(PlayerFindGold);
        EventManager.Instance.AddListener<PlayerFindKeyEvent>(PlayerFindKey);
    }

	public override void UnsubscribeEvents()
	{
		base.UnsubscribeEvents();

		EventManager.Instance.RemoveListener<MainMenuButtonClickedEvent>(MainMenuButtonClicked);
		EventManager.Instance.RemoveListener<PlayButtonClickedEvent>(PlayButtonClicked);
		EventManager.Instance.RemoveListener<ResumeButtonClickedEvent>(ResumeButtonClicked);
        EventManager.Instance.RemoveListener<QuitButtonClickedEvent>(QuitButtonClicked);
        EventManager.Instance.RemoveListener<NextLevelButtonClickedEvent>(NextButtonClicked);
        EventManager.Instance.RemoveListener<TryAgainButtonClickedEvent>(TryAgainButtonClicked);
        EventManager.Instance.RemoveListener<EscapeButtonClickedEvent>(EscapeButtonClicked);

        EventManager.Instance.RemoveListener<PlayerHasBeenDetectedEvent>(PlayerHasBeenDetected);
        EventManager.Instance.RemoveListener<PlayerFindGoldEvent>(PlayerFindGold);
        EventManager.Instance.RemoveListener<PlayerFindKeyEvent>(PlayerFindKey);
        
    }
    #endregion

    #region Initialisation
    protected override IEnumerator InitCoroutine()
	{
        while (!MenuManager.Instance.IsReady) yield return null;
        //Audio
        audio = GameObject.Find("AudioMenu");
        yield break;
	}

	private void InitNewGame()
	{
        initBestChronos();
    }
    #endregion

    #region Event
    //Key Event
    private void PlayerFindKey(PlayerFindKeyEvent e)
    {
        m_Key = true;
        EventManager.Instance.Raise(new GameStatisticsChangedEvent() { eBestChrono = bestChronos[levelIndex - 1], eChrono = m_Chrono, eKey = m_Key });
    }

    //Victory Event
    private void PlayerFindGold(PlayerFindGoldEvent e)
    {
        Victory();
    }
    
    //GameOver Event
    private void PlayerHasBeenDetected(PlayerHasBeenDetectedEvent e)
    {
        Over();
    }
    #endregion

    #region Button Events
    private void MainMenuButtonClicked(MainMenuButtonClickedEvent e)
	{
		Menu();
	}

	private void PlayButtonClicked(PlayButtonClickedEvent e)
	{
		Play();
	}

	private void ResumeButtonClicked(ResumeButtonClickedEvent e)
	{
        Resume();
	}

    private void QuitButtonClicked(QuitButtonClickedEvent e)
    {
        Quit();
    }

    private void NextButtonClicked(NextLevelButtonClickedEvent e)
    {
        NextLevel();
    }
    
    private void TryAgainButtonClicked(TryAgainButtonClickedEvent e)
    {
        TryAgain();
    }

    private void EscapeButtonClicked(EscapeButtonClickedEvent e)
	{
		if(IsPlaying)
			Pause();
	}
    #endregion

    #region Game State
    private void Menu()
	{
        Instantiate(audio);
        audio.SetActive(true);
		SetTimeScale(0);
        SetChrono(new DateTime(2000, 1, 1, 0, 0, 0));
        m_GameState = GameState.gameMenu;
        Destroy(GameObject.Find("Level " + levelIndex));
        Destroy(GameObject.Find("Audio")); //celui de la scene
        EventManager.Instance.Raise(new GameMenuEvent());
	}

	private void Play()
	{
		InitNewGame();
        SetTimeScale(1);
        audio.SetActive(false);
        m_Key = false;
        SceneManager.LoadScene(levelIndex, LoadSceneMode.Additive);
        m_GameState = GameState.gamePlay;
		EventManager.Instance.Raise(new GamePlayEvent());
	}

	private void Pause()
	{
		SetTimeScale(0);
        m_GameState = GameState.gamePause;
		EventManager.Instance.Raise(new GamePauseEvent());
	}

	private void Resume()
	{
		SetTimeScale(1);
		m_GameState = GameState.gamePlay;
		EventManager.Instance.Raise(new GameResumeEvent());
	}

    private void Quit()
    {
        Application.Quit();
    }

    private void NextLevel()
    {
        
        if(levelIndex + 1 <= 2)
        {
            SetTimeScale(1);
            m_GameState = GameState.gamePlay;
            levelIndex++;
            SceneManager.LoadScene(levelIndex, LoadSceneMode.Additive);
            EventManager.Instance.Raise(new GamePlayEvent());
        }
        else
        {
            EventManager.Instance.Raise(new GameMenuEvent());
        }
        
    }

    private void TryAgain()
    {
        SetTimeScale(1);
        m_GameState = GameState.gamePlay;
        SceneManager.LoadScene(levelIndex, LoadSceneMode.Additive);
        EventManager.Instance.Raise(new GamePlayEvent());
    }

    private void Over()
	{
		SetTimeScale(0);
        m_Key = false;

        SetChrono(new DateTime(2000, 1, 1, 0, 0, 0));

        m_GameState = GameState.gameOver;
        Destroy(GameObject.Find("Level " + levelIndex));
        EventManager.Instance.Raise(new GameOverEvent());
	}

	private void Victory()
	{
		SetTimeScale(0);
        m_Key = false;

        SetBestChrono(m_Chrono);
        SetChrono(new DateTime(2000, 1, 1, 0, 0, 0));

        m_GameState = GameState.gameVictory;
        GameObject.Destroy(GameObject.Find("Level " + levelIndex));
        EventManager.Instance.Raise(new GameVictoryEvent());
	}
    #endregion
}
