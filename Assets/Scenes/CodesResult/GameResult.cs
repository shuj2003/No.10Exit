using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResult : MonoBehaviour
{
    public GameObject resultGameOver;
    public GameObject resultGameClear;

    public void showResultGameOver()
    {
        resultGameOver.SetActive(true);
        resultGameClear.SetActive(false);
    }

    public void showResultGameClear()
    {
        resultGameOver.SetActive(false);
        resultGameClear.SetActive(true);
    }

}
