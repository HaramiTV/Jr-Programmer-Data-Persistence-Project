using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{
    private MainManager mainManager;
    private static GameManager instance;
    public static string playerName;
    public static string bestPlayerName;
    public static int currentScore;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        LoadHighScore();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        TextMeshProUGUI highScore = GameObject.Find("HighScore").GetComponent<TextMeshProUGUI>();
        highScore.text = "Best Score: " + bestPlayerName + " : " + currentScore;

        Debug.Log(Application.persistentDataPath);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
#if     UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    public void SetPlayerName(string input)
    {
        playerName = input;
    }

    public void SetHighScore(int points)
    {
        TextMeshProUGUI highScore = GameObject.Find("HighScore").GetComponent<TextMeshProUGUI>();
        mainManager = FindObjectOfType<MainManager>();

        if (points > currentScore)
        {
            currentScore = points;
            bestPlayerName = playerName;

            highScore.text = "Best Score: " + bestPlayerName + " : " + currentScore;
        }
    }

    [System.Serializable]
    class SaveScoreData
    {
        public int highscore;
        public string bestPlayerName;
    }

    public void SaveHighScore()
    {
        SaveScoreData saveScoreData = new SaveScoreData();

        saveScoreData.highscore = currentScore;
        saveScoreData.bestPlayerName = bestPlayerName;

        string json = JsonUtility.ToJson(saveScoreData);
        File.WriteAllText(Application.persistentDataPath + "/highscore.json", json);
    }

    public void LoadHighScore()
    {
        string path = Application.persistentDataPath + "/highscore.json";

        if(File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveScoreData saveScoreData = JsonUtility.FromJson<SaveScoreData>(json);

            currentScore = saveScoreData.highscore;
            bestPlayerName = saveScoreData.bestPlayerName;
        }
    }
}
