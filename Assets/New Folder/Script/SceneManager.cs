using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;
using UnityEngine.UI;
using TMPro;

public class SceneManager : MonoBehaviour
{
    // SceneManagerスクリプトに次のものを追加
    // フィールドの追加
    public TextMeshProUGUI GameOverText;
    public TextMeshProUGUI GameClearText;
    public TextMeshProUGUI ScoreText;
    int _currentScore = 0;


    public void Start()
    {
        GameOverText.gameObject.SetActive(false);
        GameClearText.gameObject.SetActive(false);

        ScoreText.text = _currentScore.ToString();
    }

    public void ShowGameover()
    {
        GameOverText.gameObject.SetActive(true);
    }

    public void ShowGameClear()
    {
        GameClearText.gameObject.SetActive(true);
    }

    public void AddScore(int score)
    {
        _currentScore += score;
        ScoreText.text = _currentScore.ToString();
    }
}
