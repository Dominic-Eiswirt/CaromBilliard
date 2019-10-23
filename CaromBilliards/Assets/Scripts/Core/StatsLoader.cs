using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Text;
public class StatsLoader : MonoBehaviour
{
#pragma warning disable CS0649
    [SerializeField] GameObject PreviousGameContainer;
    [SerializeField] Text movesText;
    [SerializeField] Text scoreText;
    [SerializeField] Text timeText;
#pragma warning restore

    void Awake()
    {
        Load();
    }
    void Load()
    {
        if (File.Exists(Application.dataPath + "/save.txt"))
        {
            PreviousGameContainer.SetActive(true);
            string saveString = File.ReadAllText(Application.dataPath + "/save.txt");
            SaveObject saveObject = JsonUtility.FromJson<SaveObject>(saveString);

            StringBuilder sb = new StringBuilder("Moves: ");
            sb.Append(saveObject.saveMoves.ToString());
            movesText.text = sb.ToString();
            sb.Clear();

            sb.Insert(0, "Score: ");
            sb.Append(saveObject.saveScore.ToString());
            scoreText.text = sb.ToString();
            sb.Clear();

            sb.Insert(0, "Time: ");
            sb.Append(saveObject.saveTime.ToString("#"));
            timeText.text = sb.ToString();
        }
        else
        {
            PreviousGameContainer.SetActive(false);
        }
    }
    
}
