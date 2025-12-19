using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int _lives;
    [SerializeField] private Ball _ball;
    [SerializeField] private Player _player;
    public static GameManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    public void LoseHealth()
    {
        _lives--;

        if(_lives <= 0)
        {
            SceneManager.LoadScene("GameOver");
        }
        else
        {
            ResetLevel();
        }
    }

    public void ResetLevel()
    {
        _ball.ResetBall();
        _player.ResetPlayer();
    }

    public void CheckLevelComplete()
    {
        if(transform.childCount <= 1)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
