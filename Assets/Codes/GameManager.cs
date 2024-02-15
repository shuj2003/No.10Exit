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
    private bool showLeftNotice = false;

    private static bool isLeftStart = true;
    private static int count = 0;

    public void GameSet()
    {
        GameManager.count = 0;
        GameManager.isLeftStart = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        instance = this;
        StarttHome();
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
            showLeftNotice = true;
        }
        else
        {
            noticeLeft.Hide();
        }

        if (Vector2.SqrMagnitude(startPointR.transform.position - player.transform.position) < 2f * 2f)
        {
            noticeRight.Show();
            showLeftNotice = false;
        }
        else
        {
            noticeRight.Hide();
        }
    }

    public void StarttHome()
    {
        if (GameManager.isLeftStart)
        {
            fullScreenFade.gameObject.SetActive(true);
            fullScreenFade.FadeOut(delegate () {
                fullScreenFade.gameObject.SetActive(false);
                outDoorL(delegate ()
                {

                });
            });
        }
        else
        {
            fullScreenFade.gameObject.SetActive(true);
            fullScreenFade.FadeOut(delegate () {
                fullScreenFade.gameObject.SetActive(false);
                outDoorR(delegate ()
                {

                });
            });
        }

    }

    public void ToNextHome()
    {
        if (showLeftNotice)
        {
            inDoorL(delegate ()
            {
                fullScreenFade.gameObject.SetActive(true);
                fullScreenFade.FadeIn(delegate () {
                    SceneManager.LoadScene(0);
                    GameManager.isLeftStart = false;
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
                    GameManager.isLeftStart = true;
                });
            });
        }
        
    }

    private void inDoorL(Action action)
    {
        doorLeft.openDoor(null);
        player.InDoor(startPointL.transform.position, delegate () {
            doorLeft.closeDoor(delegate () {
                if (action != null) action();
            });
        });
    }

    private void inDoorR(Action action)
    {
        doorRight.openDoor(null);
        player.InDoor(startPointR.transform.position, delegate () {
            doorRight.closeDoor(delegate () {
                if (action != null) action();
            });
        });
    }

    private void outDoorL(Action action)
    {
        doorLeft.openDoor(null);
        player.OutDoor(startPointL.transform.position, delegate () {
            doorLeft.closeDoor(delegate () {
                if (action != null) action();
            });
        });
    }

    private void outDoorR(Action action)
    {
        doorRight.openDoor(null);
        player.OutDoor(startPointR.transform.position, delegate () {
            doorRight.closeDoor(delegate () {
                if (action != null) action();
            });
        });
    }

}
