using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Door doorLeft;
    public Door doorRight;
    public GameObject startPointL;
    public GameObject startPointR;
    public Notice noticeLeft;
    public Notice noticeRight;
    public FullScreenFade fullScreenFade;

    public Man man;
    public Player player;

    private bool isLeft;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if(Vector2.SqrMagnitude(startPointL.transform.position - player.transform.position) < 2f * 2f)
        {
            noticeLeft.Show();
            isLeft = true;
        }
        else
        {
            noticeLeft.Hide();
        }

        if (Vector2.SqrMagnitude(startPointR.transform.position - player.transform.position) < 2f * 2f)
        {
            noticeRight.Show();
            isLeft = false;
        }
        else
        {
            noticeRight.Hide();
        }
    }
    public void ResetGame()
    {
        if (isLeft)
        {
            inDoorL(delegate ()
            {
                fullScreenFade.gameObject.SetActive(true);
                fullScreenFade.FadeIn(delegate () {
                    SceneManager.LoadScene(0);
                    fullScreenFade.FadeOut(delegate() {
                        fullScreenFade.gameObject.SetActive(false);
                    });
                });
            });
        }
        else
        {
            inDoorR(delegate ()
            {
                fullScreenFade.gameObject.SetActive(true);
                fullScreenFade.FadeIn(delegate () {
                    SceneManager.LoadScene(0);
                    fullScreenFade.FadeOut(delegate () {
                        fullScreenFade.gameObject.SetActive(false);
                    });
                });
            });
        }
        
    }

    public void inDoorL(Action action)
    {
        doorLeft.openDoor(null);
        player.OutDoor(startPointL.transform.position, delegate () {
            doorLeft.closeDoor(delegate () {
                if (action != null) action();
            });
        });
    }

    public void inDoorR(Action action)
    {
        doorRight.openDoor(null);
        player.OutDoor(startPointR.transform.position, delegate () {
            doorRight.closeDoor(delegate () {
                if (action != null) action();
            });
        });
    }

}
