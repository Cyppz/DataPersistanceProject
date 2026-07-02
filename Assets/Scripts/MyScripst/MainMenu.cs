using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] SaveManager saveManager;
    [SerializeField] TextMeshProUGUI playerName;
    string _playerName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void StartGame()
    {
        _playerName = playerName.text;

        if (_playerName == null || _playerName == string.Empty || _playerName == " ")
            return;
        
        saveManager.SetCurrentPlayer(_playerName, 0); //savemanager handles checking for existing
        SceneManager.LoadScene(1);
        Debug.Log(_playerName);
    }
}
