using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public float startWaitTime;
    public GameObject[] hazards;
    public int hazardcount;
    public Vector3 spawnValues;
    public float waveWaitTime;

    public Text StartOrGameOverText;
    public Text scoreText;
    public Text restartText;

    private bool gameRestart;
    private bool gameOver;
    private int score;

    // Use this for initialization
    void Start()
    {
        gameRestart = false;
        gameOver = false;
        restartText.text = "";
        StartOrGameOverText.text = "";
        score = 0;
        UpdateScore();
        StartCoroutine(SpawnWaves());
    }

    // Update is called once per frame
    void Update()
    {
        if (gameRestart)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWaitTime);
        while (true)
        {
            for (int i = 0; i < hazardcount; i++)
            {
                GameObject hazard = hazards[Random.Range(0, hazards.Length)];
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(hazard, spawnPosition, spawnRotation);
                yield return new WaitForSeconds(waveWaitTime);
            }
            yield return new WaitForSeconds(waveWaitTime);
            if (gameOver)
            {
                restartText.text = "Press 'R' for Restart";
                gameRestart = true;
                break;
            }

        }

    }
    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
    }

    void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }

    public void GameOver()
    {
        StartOrGameOverText.text = "jgj";
        gameOver = true;
    }
}
