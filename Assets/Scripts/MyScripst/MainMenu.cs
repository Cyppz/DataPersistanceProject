using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI playerName;
    string _playerName;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        _playerName = playerName.text;
        SceneManager.LoadScene(1);
        Debug.Log(_playerName);
    }
}
