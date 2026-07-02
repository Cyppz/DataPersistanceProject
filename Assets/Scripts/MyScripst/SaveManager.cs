using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using System.IO;
using System.Linq;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    public SaveData saveData { get; private set; } //should be created on first time saving, then always laoded on start.

    public PlayerData currentPlayer {  get; private set; }
    PlayerData currentPlayerInData; //set this to the corresponding entry if player has played before

    public PlayerData highScorePlayer { get; private set; } = new PlayerData("None", 0);

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        Load();
        if (saveData == null) //if no data is found, create data and fill with dummy player called NONE
        {
            saveData = new SaveData();
            saveData.dataList = new List<PlayerData>();
            saveData.dataList.Add(highScorePlayer);
        }
        GetHighscore();
    }

    private void Start()
    {
        /*Load();
        if (saveData == null) //if no data is found, create data and fill with dummy player called NONE
        {
            saveData = new SaveData();
            saveData.dataList = new List<PlayerData>();
            saveData.dataList.Add(highScorePlayer);
        }
        GetHighscore();*/
    }

    void GetHighscore()
    {
        //after loading the savedata, sor by scrore and get the last item from the list
        //this has to be the highest score.
        var sorted = from player in saveData.dataList
                     orderby player.highscore descending
                     select player;

        highScorePlayer = sorted.First();
        //Debug.Log($"High score player: {highScorePlayer.playerName} - {highScorePlayer.highscore}");
    }

    void OnRoundEnd()
    {
        
    }

    public void SetCurrentPlayer(string name, int score)
    {
        name = name.ToLower();
        string pName = char.ToUpper(name[0]) + name.Substring(1);
        if (pName.Length > 10)
        {
            pName = pName.Substring(0, 10);
        }

        PlayerData p = new PlayerData(pName, score);
        currentPlayer = p;

        if (CheckExisting(pName, out PlayerData existing))
        {
            currentPlayerInData = existing;
        }
        else
        {
            saveData.dataList.Add(p);
            currentPlayerInData = currentPlayer;
        }
    }

    bool CheckExisting(string name, out PlayerData existing)
    {
        foreach (PlayerData p in saveData.dataList)
        {
            if (p.playerName == name)
            {
                existing = p;
                return true;
            }
        }
        existing= null;
        return false;
    }

    public void UpdateCurrentScore(int score)
    {
        //Debug.Log($"Current player: {currentPlayer.playerName}");
        //Debug.Log($"Score passed in: {score}");

        currentPlayer.highscore = score;
        Save();
    }
    
    void Save()
    {
        //Debug.Log($"currentPlayer.highscore = {currentPlayer.highscore}");
        //Debug.Log($"currentPlayerInData.highscore = {currentPlayerInData.highscore}");

        if (currentPlayerInData!=null && currentPlayer.highscore > currentPlayerInData.highscore)
        {
            currentPlayerInData.highscore = currentPlayer.highscore;
        }
        string dataJson = JsonUtility.ToJson(saveData);
        File.WriteAllText(Application.persistentDataPath + "/SaveData.json", dataJson);
        GetHighscore();
    }

    void Load()
    {
        string path = Application.persistentDataPath + "/SaveData.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            saveData = JsonUtility.FromJson<SaveData>(json);
        }
    }


    [System.Serializable] 
    public class SaveData
    {
        public List<PlayerData> dataList = new List<PlayerData>();
    }

    [System.Serializable]
    public class PlayerData
    {
        public string playerName;
        public int highscore;

        public PlayerData(string name, int score)
        {
            playerName = name;
            highscore = score;
        }
    }
}
