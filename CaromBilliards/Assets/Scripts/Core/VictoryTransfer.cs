using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.IO;
using UnityEngine.SceneManagement;

public class VictoryTransfer : MonoBehaviour, IVictoryTransfer
{
    static VictoryTransfer instance;
    [SerializeField] int score;
    [SerializeField] int moves;
    [SerializeField] float time;
    [SerializeField] GameObject Canvas;
    [SerializeField] Text scoreText, timerText, shotsFiredText;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);            
        }
        else if(this != instance) Destroy(this.gameObject);
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
    }
    void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        //If the score more than 0, then we know that we just won the game, and the new scene is the victory screen. 
        if(score > 0)
        {
            StringBuilder sb = new StringBuilder();
            Canvas = Instantiate(Canvas);
            //Needs a better way to do this than looking at names of child objects
            timerText = Canvas.transform.Find("Timer/TimerText").GetComponent<Text>();
            shotsFiredText = Canvas.transform.Find("ShotsFiredBG/ShotsFiredText").GetComponent<Text>();
            scoreText = Canvas.transform.Find("Score/ScoreText").GetComponent<Text>();

            sb.Insert(0, "Time: ");
            sb.Append(time.ToString("#"));
            timerText.text = sb.ToString();
            sb.Clear();

            sb.Insert(0, "Score: ");
            sb.Append(score.ToString());
            scoreText.text = sb.ToString();
            sb.Clear();

            sb.Insert(0, "Moves: ");
            sb.Append(moves.ToString());
            shotsFiredText.text = sb.ToString();
            
            Save();
            //Remove gameobject as we don't need it and don't want it doing stuff in the main menu
            Destroy(this.gameObject);            
        }
    }

    void Save()
    {
        //Simple save just using .txt file
        SaveObject saver = new SaveObject()
        {
            saveMoves = moves,
            saveScore = score,
            saveTime = time
        };
        string json = JsonUtility.ToJson(saver);
        File.WriteAllText(Application.dataPath + "/save.txt", json);
    }

    public void SetSessionInformation(int score, int moves, float time)
    {
        this.score = score;
        this.moves = moves;
        this.time = time;
    }   

}

public class SaveObject
{
    public int saveMoves;
    public int saveScore;
    public float saveTime;
}
