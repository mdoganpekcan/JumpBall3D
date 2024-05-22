using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    private Text ScoreText;
    public int Score;

    private void Awake()
    {
        ScoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        MakeSingleton();
    }
    void Start()
    {
        AddScore(0);
    }

    void Update()
    {
        if (ScoreText == null)
        {
            ScoreText = GameObject.Find("ScoreText").GetComponent<Text>();
            ScoreText.text = Score.ToString();
        }
    }

    void MakeSingleton()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void AddScore(int amount)
    {
        Score += amount;
        if (Score > PlayerPrefs.GetInt("HighScore", 0))
            PlayerPrefs.SetInt("HighScore", Score);

        ScoreText.text = Score.ToString();
    }

    public void ResetScore()
    {
        Score = 0;
    }

}
