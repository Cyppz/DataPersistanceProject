using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System; // MIGRATED: New Input System namespace

public class MainManager : MonoBehaviour
{
    //public event Action<int> RoundOver;
    List<Brick> brickList = new List<Brick>();

    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;
    public Text BestScoreText;

    private bool m_Started = false;
    private int m_Points;

    private bool m_GameOver = false;

    // MIGRATED: InputAction replaces Input.GetKeyDown(KeyCode.Space)
    private InputAction m_LaunchAction;

    // MIGRATED: bind the Space key as a button action
    void Awake()
    {
        m_LaunchAction = new InputAction("Launch", InputActionType.Button, "<Keyboard>/space");
    }

    // MIGRATED: enable the action while the component is active
    void OnEnable()
    {
        m_LaunchAction.Enable();
    }

    // MIGRATED: disable the action when the component is inactive
    void OnDisable()
    {
        m_LaunchAction.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        ScoreText.text = $"Score :" + SaveManager.instance.currentPlayer.playerName + ": " + m_Points;
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
                brickList.Add(brick);
            }
        }
        UpdateHighScore();
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (m_LaunchAction.WasPressedThisFrame()) // MIGRATED: was Input.GetKeyDown(KeyCode.Space)
            {
                m_Started = true;
                float randomDirection = UnityEngine.Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (m_LaunchAction.WasPressedThisFrame()) // MIGRATED: was Input.GetKeyDown(KeyCode.Space)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                //update highscore-display update logic here
            }
        }
        if (brickList.Count <= 0)
        {
            GameWon();
            m_GameOver = true;
        }
    }

    void AddPoint(int point, Brick brick)
    {
        m_Points += point;
        ScoreText.text = $"Score :" + SaveManager.instance.currentPlayer.playerName + ": " + m_Points;
        brickList.Remove(brick);
    }

    public void GameOver()
    {
        m_GameOver = true;
        SendScore();
        GameOverText.SetActive(true);
    }

    void GameWon()
    {
        SendScore();
        
    }

    void SendScore()
    {
        SaveManager.instance.UpdateCurrentScore(m_Points);
        UpdateHighScore();
    }

    void UpdateHighScore()
    {
        BestScoreText.text =
            "Best Score:" + SaveManager.instance.highScorePlayer.playerName + ": " +
            SaveManager.instance.highScorePlayer.highscore;
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
