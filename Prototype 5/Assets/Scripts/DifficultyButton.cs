using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyButton : MonoBehaviour
{
    private Button button;
    private GameManager gameManager;
    public int difficulty;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(SetDifficulty);
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    //sends difficulty rating (and int) to game manager for the manager to adjust game
    void SetDifficulty()
    {
        Debug.Log(gameObject.name + " was clicked.");
        gameManager.StartGame(difficulty);
    }
}
