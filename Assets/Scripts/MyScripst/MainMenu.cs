using System.Linq;
using TMPro;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Scripting;

public class MainMenu : MonoBehaviour
{
    SaveManager saveManager;
    [SerializeField] TextMeshProUGUI playerName;
    [SerializeField] TextMeshProUGUI highScores;
    string _playerName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        saveManager = SaveManager.instance;
        DisplayHighScores();
    }

    public void StartGame()
    {
        _playerName = playerName.text;

        if (_playerName == null || _playerName == string.Empty || _playerName == " ")
            return;
        
        saveManager.SetCurrentPlayer(_playerName, 0); //savemanager handles checking for existing
        //Debug.Log("main menu before loading scene: " +
          //  SaveManager.instance.currentPlayer.playerName +
           // SaveManager.instance.currentPlayer.highscore); here the current player i set up correctly
        SceneManager.LoadScene(1);
        //Debug.Log(_playerName);
    }

    public void Close()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif  
    }

    void DisplayHighScores()
    {
        var sorted = SaveManager.instance.saveData.dataList.
            OrderByDescending(e => e.highscore);

        foreach (var entry in sorted)
        {
            highScores.text += "\n" + entry.playerName + ": " + entry.highscore;
        }
    }
    
}
