using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager2 : MonoBehaviour
{
    public Door doorLeft;
    public Door doorCenter;
    public GameObject startPointL;
    public GameObject startPointC;
    public Notice2 noticeLeft;
    public FullScreenFade fullScreenFade;
    public FullScreenFade fullScreenFade2;
    public bool enableUI = false;
    public GameResult gameResult;

    public Player2 player;

    // Start is called before the first frame update
    void Start()
    {
        StarttHome();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (enableUI == false) return;

        if (Vector2.SqrMagnitude(startPointC.transform.position - player.transform.position) < 2f * 2f)
        {
            noticeLeft.Show();
        }
        else
        {
            noticeLeft.Hide();
        }
    }

    public void StarttHome()
    {
        player.transform.position = startPointL.transform.position + new Vector3(0f, 1f);
        enableUI = false;
        fullScreenFade.gameObject.SetActive(true);
        fullScreenFade.FadeOut(delegate () {
            fullScreenFade.gameObject.SetActive(false);
            outDoorL(delegate ()
            {
                enableUI = true;
            });
        });

    }

    public void ToNextHome()
    {
        if (enableUI == false) return;

        enableUI = false;
        inDoorC(delegate ()
        {
            fullScreenFade2.gameObject.SetActive(true);
            fullScreenFade2.FadeIn(delegate () {

                gameResult.gameObject.SetActive(true);

                if (PlayerPrefs.HasKey("Ending") && PlayerPrefs.GetInt("Ending") == 2)
                {
                    gameResult.showResultGameClear();
                }
                else
                {
                    gameResult.showResultGameOver();
                }

            });
        });

    }

    private void outDoorL(Action action)
    {
        doorLeft.openDoor(null);
        player.OutDoor(startPointL.transform.position, delegate () {
            doorLeft.closeDoor(delegate () {
                doorLeft.FadeOut(delegate() {
                    if (action != null) action();
                });
            });
        });
    }

    private void inDoorC(Action action)
    {
        doorCenter.openDoor(null);
        player.InDoor(startPointC.transform.position, delegate () {
            doorCenter.closeDoor(delegate () {
                if (action != null) action();
            });
        });
    }

}
