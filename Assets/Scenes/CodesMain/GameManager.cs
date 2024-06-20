using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    public Canvas canvasTitle;

    public GameObject cameraFollowTitle;
    public GameObject classShader;
    public GameObject gameTitle;
    public GameObject gameRule;

    public Cinemachine.CinemachineVirtualCamera virtualCamera;

    public Man man;
    public Player player;
    public TitleText titleText;
    public GameRule gameRuleText;

    private bool showLeftNotice = false;

    public static bool isFirst;
    public static bool isLive;
    public static bool isLeftStart;
    public static int count;
    public static int foundScrectNum;

    private static int outStandard = 4;
    private int outLen = 12;

    public int[] screctNums;
    public OUT_CHANGES[] screctChanges;

    private bool _isOut
    {
        get {
            int rd = UnityEngine.Random.Range(0, outLen);
            bool flag = rd > GameManager.outStandard;
            if(rd <= GameManager.outStandard)
            {
                GameManager.outStandard--;
            }
            else
            {
                GameManager.outStandard = 4;
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
        WINDOW_2,
        MAN_1,
        MAN_2,
        MAN_3,
        MAN_4,
        MAN_5,
        FLOOR_1,
        NOTICE_1,
        CANVAS_1,
        DOOR_1,
        NONE,
    }

    // Start is called before the first frame update
    void Start()
    {

        if (GameManager.isLive)
        {
            GameManager.isFirst = false;

            // game loop
            classShader.SetActive(false);
            cameraFollowTitle.SetActive(false);
            canvasTitle.enabled = false;
            virtualCamera.Follow = player.CameraFollow.transform;
            virtualCamera.m_Lens.OrthographicSize = 5.5f;

            GameLoop();

        }
        else
        {

            if (PlayerPrefs.HasKey("Ending") && PlayerPrefs.GetInt("Ending") >= 1)
            {
                titleText.ShowTitleX();
                gameRuleText.ShowText(screctNums[0]);
            }
            else
            {
                titleText.ShowTitle10();
                gameRuleText.ShowText(10);
            }

            //show Title
            classShader.SetActive(true);
            cameraFollowTitle.SetActive(true);
            canvasTitle.enabled = true;
            virtualCamera.Follow = cameraFollowTitle.transform;
            virtualCamera.m_Lens.OrthographicSize = 1f;

            gameTitle.SetActive(true);
            gameRule.SetActive(false);

        }

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

    public void GameRuleShow()
    {
        gameTitle.SetActive(false);
        gameRule.SetActive(true);
    }

    private void GameLoop()
    {
        AudioManager.instance.PlayBgm(true);

        doorLeft.no.SetNo(GameManager.count);
        doorRight.no.SetNo(GameManager.count);

        StarttHome();

    }

    public void GameStart()
    {
        GameManager.isLeftStart = true;
        GameManager.count = 0;
        GameManager.isLive = true;
        GameManager.isFirst = true;
        GameManager.foundScrectNum = 0;

        classShader.SetActive(false);
        cameraFollowTitle.SetActive(false);
        canvasTitle.enabled = false;
        virtualCamera.Follow = player.CameraFollow.transform;
        virtualCamera.m_Lens.OrthographicSize = 5.5f;

        GameLoop();
    }

    private IEnumerator Wait(Action action, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (action != null) action();
    }

    public void GameSet()
    {
        RectTransform tr = noticeLeft.GetComponent<RectTransform>();
        tr.anchoredPosition = new Vector2(-60f, tr.anchoredPosition.y);
        tr = noticeRight.GetComponent<RectTransform>();
        tr.anchoredPosition = new Vector2(60f, tr.anchoredPosition.y);

        fullScreenFade.gameObject.SetActive(true);
        var img = fullScreenFade.image.GetComponent<Image>();
        Color color = img.color;
        img.color = new Color(color.r, color.g, color.b, 1f);
        AudioManager.instance.EffectBgm(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Stab);
        StartCoroutine(Wait(delegate () {

            GameManager.isLeftStart = true;
            GameManager.count = 0;

            AudioManager.instance.EffectBgm(false);
            SceneManager.LoadScene(0);

        }, 2f));

    }

    public void StarttHome()
    {
        changeNum = OUT_CHANGES.NONE;
        if (GameManager.isFirst)
        {
            isOut = false;
        }
        else
        {
            bool isScrectNum = false;
            if (PlayerPrefs.HasKey("Ending") && PlayerPrefs.GetInt("Ending")  >= 1)
            {
                if (GameManager.foundScrectNum < screctNums.Length && screctNums[GameManager.foundScrectNum] == GameManager.count)
                {

                    isScrectNum = true;

                    isOut = true;
                    changeNum = screctChanges[UnityEngine.Random.Range(0, screctChanges.Length)];
                    ChangeManager.instance.StartChange();

                    GameManager.foundScrectNum++;
                }
            }

            if (!isScrectNum)
            {
                isOut = _isOut;
                if (isOut)
                {
                    changeNum = (OUT_CHANGES)Enum.ToObject(typeof(OUT_CHANGES), UnityEngine.Random.Range(0, (int)OUT_CHANGES.NONE));
                    changeNum = OUT_CHANGES.DOOR_1;
                }
                ChangeManager.instance.StartChange();
            }

        }

        if (isLeftStart)
        {
            player.transform.position = startPointL.transform.position + new Vector3(0f, 1f);
            enableUI = false;
            fullScreenFade.gameObject.SetActive(true);
            fullScreenFade.FadeOut(delegate () {
                fullScreenFade.gameObject.SetActive(false);
                OutDoorL(delegate ()
                {
                    enableUI = true;
                });
            });
        }
        else
        {
            player.transform.position = startPointR.transform.position + new Vector3(0f, 1f);
            enableUI = false;
            fullScreenFade.gameObject.SetActive(true);
            fullScreenFade.FadeOut(delegate () {
                fullScreenFade.gameObject.SetActive(false);
                OutDoorR(delegate ()
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
            InDoorL(delegate ()
            {
                fullScreenFade.gameObject.SetActive(true);
                fullScreenFade.FadeIn(delegate () {
                    if (isOut)
                    {
                        if (isLeftStart)
                        {
                            GameManager.count = 0; 
                        }
                        else
                        {
                            GameManager.count++;
                        }
                    }
                    else
                    {
                        if (isLeftStart)
                        {
                            GameManager.count++; 
                        }
                        else
                        {
                            GameManager.count = 0;
                        }
                    }
                    GameManager.isLeftStart = false;

                    //if (GameManager.count == 11 || GameManager.foundScrectNum >= GameManager.instance.screctNums.Length)
                    if (GameManager.count == 5 || GameManager.foundScrectNum >= GameManager.instance.screctNums.Length)
                    {
                        SceneManager.LoadScene(1);
                    }
                    else
                    {
                        SceneManager.LoadScene(0);
                    }

                });
            });
        }
        else
        {
            enableUI = false;
            InDoorR(delegate ()
            {
                fullScreenFade.gameObject.SetActive(true);
                fullScreenFade.FadeIn(delegate () {
                    if (isOut)
                    {
                        if (isLeftStart)
                        {
                            GameManager.count++;
                        }
                        else
                        {
                            GameManager.count = 0; 
                        }
                    }
                    else
                    {
                        if (isLeftStart)
                        {
                            GameManager.count = 0; 
                        }
                        else
                        {
                            GameManager.count++;
                        }
                    }
                    GameManager.isLeftStart = true;

                    //if (GameManager.count == 11 || GameManager.foundScrectNum >= GameManager.instance.screctNums.Length)
                    if (GameManager.count == 5 || GameManager.foundScrectNum >= GameManager.instance.screctNums.Length)
                    {
                        SceneManager.LoadScene(1);
                    }
                    else
                    {
                        SceneManager.LoadScene(0);
                    }

                });
            });
        }
        
    }

    private void InDoorL(Action action)
    {
        doorLeft.OpenDoor(null);
        player.InDoor(startPointL.transform.position, delegate () {
            doorLeft.CloseDoor(delegate () {
                if (action != null) action();
            });
        });
    }

    private void InDoorR(Action action)
    {
        doorRight.OpenDoor(null);
        player.InDoor(startPointR.transform.position, delegate () {
            doorRight.CloseDoor(delegate () {
                if (action != null) action();
            });
        });
    }

    private void OutDoorL(Action action)
    {
        doorLeft.OpenDoor(null);
        player.OutDoor(startPointL.transform.position, delegate () {
            doorLeft.CloseDoor(delegate () {
                if (action != null) action();
            });
        });
    }

    private void OutDoorR(Action action)
    {
        doorRight.OpenDoor(null);
        player.OutDoor(startPointR.transform.position, delegate () {
            doorRight.CloseDoor(delegate () {
                if (action != null) action();
            });
        });
    }

}
