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
    public bool enableUI = false;

    public Man man;
    public Player player;
    private bool showLeftNotice = false;

    public bool isLeftStart;
    public int count;

    private int outStandard = 4;
    private int outLen = 100;
    private bool _isOut
    {
        get {
            int rd = UnityEngine.Random.Range(0, outLen);
            bool flag = rd > outStandard;
            if(rd <= outStandard)
            {
                outStandard--;
            }
            else
            {
                outStandard = 4;
            }
            return flag;
        }
    }
    public bool isOut;

    public OUT_CHANGES changeNum = OUT_CHANGES.NONE;
    public enum OUT_CHANGES 
    {
        WALL_1 = 0,
        WALL_2,
        WALL_3,
        WINDOW_1,
        MAN_1,
        MAN_2,
        MAN_3,
        MAN_4,
        MAN_5,
        NONE,
    }

    // Start is called before the first frame update
    void Start()
    {
        isLeftStart = !PlayerPrefs.HasKey("isLeftStart") || PlayerPrefs.GetInt("isLeftStart") == 1;
        count = !PlayerPrefs.HasKey("count") ? 0 : PlayerPrefs.GetInt("count");

        isOut = _isOut;
        changeNum = OUT_CHANGES.NONE;
        if (isOut) changeNum = (OUT_CHANGES)Enum.ToObject(typeof(OUT_CHANGES), UnityEngine.Random.Range(0, (int)OUT_CHANGES.NONE));
        doorLeft.no.SetNo(count);
        doorRight.no.SetNo(count);
        StarttHome();
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
        if (enableUI == false) return;

        if (Vector2.SqrMagnitude(startPointL.transform.position - player.transform.position) < 2f * 2f)
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
        if (isLeftStart)
        {
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
        else
        {
            enableUI = false;
            fullScreenFade.gameObject.SetActive(true);
            fullScreenFade.FadeOut(delegate () {
                fullScreenFade.gameObject.SetActive(false);
                outDoorR(delegate ()
                {
                    enableUI = true;
                });
            });
        }

    }

    public void ToNextHome()
    {
        if (enableUI == false) return;

        if (showLeftNotice)
        {
            enableUI = false;
            inDoorL(delegate ()
            {
                fullScreenFade.gameObject.SetActive(true);
                fullScreenFade.FadeIn(delegate () {
                    if (isOut)
                    {
                        if (isLeftStart)
                        {
                            count++;
                        }
                        else
                        {
                            count = 0;
                        }
                    }
                    else
                    {
                        if (isLeftStart)
                        {
                            count = 0;
                        }
                        else
                        {
                            count++;
                        }
                    }
                    isLeftStart = false;
                    PlayerPrefs.SetInt("count", count);
                    PlayerPrefs.SetInt("isLeftStart", isLeftStart ? 1 : 0);
                    SceneManager.LoadScene(0);                    
                });
            });
        }
        else
        {
            enableUI = false;
            inDoorR(delegate ()
            {
                fullScreenFade.gameObject.SetActive(true);
                fullScreenFade.FadeIn(delegate () {
                    if (isOut)
                    {
                        if (isLeftStart)
                        {
                            count = 0; 
                        }
                        else
                        {
                            count++;
                        }
                    }
                    else
                    {
                        if (isLeftStart)
                        {
                            count++; 
                        }
                        else
                        {
                            count = 0;
                        }
                    }
                    isLeftStart = true;
                    PlayerPrefs.SetInt("count", count);
                    PlayerPrefs.SetInt("isLeftStart", isLeftStart ? 1 : 0);
                    SceneManager.LoadScene(0);
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
