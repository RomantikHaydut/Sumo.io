using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    #region Texts
    [SerializeField] private TMP_Text _countDownText;

    [SerializeField] private TMP_Text _scoreText;

    [SerializeField] private TMP_Text _timeText;

    [SerializeField] private TMP_Text _timeOutText;

    [SerializeField] private TMP_Text _killCountText;
    #endregion

    #region Buttons
    [SerializeField] private GameObject _startButton;

    [SerializeField] private GameObject _pauseButton;

    [SerializeField] private GameObject _resumeButton;
    #endregion

    #region Panels
    [SerializeField] private GameObject _startPanel;

    [SerializeField] private GameObject _gamePanel;

    [SerializeField] private GameObject _gameWinPanel;

    [SerializeField] private GameObject _gameLosePanel;

    [SerializeField] private GameObject _timeOutPanel;
    #endregion

    private bool isGameStarted = false;

    public bool isLevelFinished = false;

    private float _roundTime;


    private void Start()
    {
        ClosePanels();

        _roundTime = GameManager.Instance.roundTime;
    }

    private void ClosePanels()
    {
        _startPanel.SetActive(true);
        _gamePanel.SetActive(false);
        _gameLosePanel.SetActive(false);
        _gameWinPanel.SetActive(false);
        _resumeButton.SetActive(false);
        _timeOutPanel.SetActive(false);
    }

    private void Update()
    {
        if (isGameStarted && !isLevelFinished)
        {
            Timer();
        }
    }

    private void Timer()
    {
        _roundTime -= Time.deltaTime;
        _timeText.text = "Time : " + ((int)_roundTime).ToString();

        if (_roundTime <= 0)
        {
            _roundTime = 0;
            GameManager.Instance.TimeOut();
            isLevelFinished = true;
        }
    }

    public void StartGame()
    {
        StartCoroutine(StartCountDown());
        DisplayScore(0);
    }

    private IEnumerator StartCountDown()
    {
        _countDownText.gameObject.SetActive(true);
        _startButton.SetActive(false);

        float countDownTime = GameManager.Instance.countDownTime;
        while (true)
        {
            yield return null;
            countDownTime -= Time.deltaTime;

            if (countDownTime <= 0)
            {
                countDownTime = 0;
                yield return new WaitForSecondsRealtime(0.4f);
                GameManager.Instance.StartGame();
                _countDownText.gameObject.SetActive(false);
                OpenGamePanel();
                _startPanel.SetActive(false);
                isGameStarted = true;
                yield break;
            }

            _countDownText.text = countDownTime.ToString("f1");
        }
    }

    private void OpenGamePanel() => _gamePanel.SetActive(true);


    public void DisplayScore(int score)
    {
        _scoreText.text = "Score : " + score;
    }

    public void DisplayKillCount()
    {
        _killCountText.gameObject.SetActive(true);
        _killCountText.text = "You Killed : " + MovementBase.killCount + " Mummy";
    }

    public void LoseGame()
    {
        _gameLosePanel.SetActive(true);
        DisplayKillCount();
    }

    public void WinGame()
    {
        _gameWinPanel.SetActive(true);
        DisplayKillCount();
    }

    public void TimeOut()
    {
        GameObject winner = FindObjectOfType<ScoreManager>().HighScoreOwner();
        _timeOutText.text = "Time Out... Winner is : " + winner.name;
        _timeOutPanel.SetActive(true);
        DisplayKillCount();

    }

    public void PauseGame()
    {
        _pauseButton.SetActive(false);
        _resumeButton.SetActive(true);
        GameManager.Instance.PauseGame();
    }

    public void ResumeGame()
    {
        _pauseButton.SetActive(true);
        _resumeButton.SetActive(false);
        GameManager.Instance.ResumeGame();

    }


}
