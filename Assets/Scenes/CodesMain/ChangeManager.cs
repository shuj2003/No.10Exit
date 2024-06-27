using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeManager : MonoBehaviour
{
    public static ChangeManager instance;

    [Header(" # Man ")]
    public Man man;

    [Header(" # Wall ")]
    public Wall wall;
    public GameObject[] wall_pictures1;
    public GameObject[] wall_pictures2;

    [Header(" # Window ")]
    public GameObject window_bg;
    public GameObject window_city;
    public GameObject window_light;
    public GameObject window_Head;

    [Header(" # Screct ")]
    public GameObject[] screctObjects;

    [Header(" # Notice ")]
    public Text noticeTextL;
    public Text noticeTextR;
    public Text noticeButtonTextL;
    public Text noticeButtonTextR;

    [Header(" # Canvas ")]
    public GameObject eyesNum;

    [Header(" # Door ")]
    public GameObject[] doorNums;

    [Header(" # Book_VASE ")]
    public GameObject[] book1;
    public GameObject[] book2;
    public GameObject[] vase1;
    public GameObject[] vase2;

    [Header(" # TABLE ")]
    public GameObject[] tables;
    private Boolean[] tablesMoved;
    public GameObject[] armchairs;
    public GameObject table2;

    private float timeCount = 1f;
    private float timeEnd = 1f;
    private float[] startDatas;
    private float[] endDatas;
    private int timeStep = 0;

    private void Awake()
    {
        instance = this;
    }

    private float NowData(int idx)
    {
        float endData = endDatas[idx];
        float startData = startDatas[idx];
        return startData + timeCount / timeEnd * (endData - startData);
    }

    // Start is called before the first frame update
    public void StartChange()
    {
        foreach(GameObject s in screctObjects)
        {
            if (s != null)
                s.SetActive(false);
        }
        window_Head.gameObject.SetActive(false);
        eyesNum.SetActive(false);
        Image img = eyesNum.GetComponent<Image>();
        Color color = img.color;
        img.color = new Color(color.r, color.g, color.b, 0f);

        noticeTextL.text = "Open?";
        noticeButtonTextL.text = "YES";
        noticeTextR.text = "Open?";
        noticeButtonTextR.text = "YES";

        tablesMoved = new Boolean[tables.Length];
        for (int i = 0; i < tablesMoved.Length ; i++)
        {
            tablesMoved[i] = false;
        }

        switch (GameManager.instance.changeNum)
        {
            case GameManager.OUT_CHANGES.WALL_1:// 壁線太くなる
                {
                    timeCount = 0f;
                    timeEnd = 15f;
                    startDatas = new float[] { 0.5f };
                    endDatas = new float[] { 1f };
                }
                break;
            case GameManager.OUT_CHANGES.WALL_2:// 絵が逆さま
                {
                    for (int i = 0; i < wall_pictures1.Length; i++)
                    {
                        var obj = wall_pictures1[i];
                        Vector3 localScale = obj.GetComponent<Transform>().localScale;
                        localScale.y = -1f;
                        obj.GetComponent<Transform>().localScale = localScale;
                    }

                    for (int i = 0; i < wall_pictures2.Length; i++)
                    {
                        var obj = wall_pictures2[i];
                        Vector3 localScale = obj.GetComponent<Transform>().localScale;
                        localScale.y = -2f;
                        obj.GetComponent<Transform>().localScale = localScale;
                    }
                }
                break;
            case GameManager.OUT_CHANGES.WALL_3:// 絵が落ちる
                 {
                     //秘密番号表示
                     if (PlayerPrefs.HasKey("Ending") && PlayerPrefs.GetInt("Ending") >= 1)
                     {
                         if (GameManager.foundScrectNum < GameManager.instance.screctNums.Length && GameManager.instance.screctNums[GameManager.foundScrectNum] == GameManager.count)
                         {
                             if(GameManager.foundScrectNum + 1 < GameManager.instance.screctNums.Length)
                             {
                                screctObjects[0].gameObject.SetActive(true);
                                SpriteRendererNo[] texts = screctObjects[0].GetComponentsInChildren<SpriteRendererNo>();
                                foreach (SpriteRendererNo text in texts)
                                {
                                    text.gameObject.SetActive(false);
                                }
                                var idx = UnityEngine.Random.Range(0, texts.Length);
                                texts[idx].gameObject.SetActive(true);
                                texts[idx].SetNo(GameManager.instance.screctNums[GameManager.foundScrectNum + 1]);
                            }
                         }
                     }
                 }
                 break;
            case GameManager.OUT_CHANGES.WINDOW_1:// 外暗くなる
                {
                    timeCount = 0f;
                    timeEnd = 15f;
                    startDatas = new float[] { 1f, 1f };
                    endDatas = new float[] { 0.25f, 0f };
                }
                break;
            case GameManager.OUT_CHANGES.WINDOW_2:// 大きい頭
                {
                    window_Head.gameObject.SetActive(true);
                    timeCount = 0f;
                    timeEnd = 7f;
                    startDatas = new float[] { -2.75f, 0f, 0f   , 2.75f, 2.75f, 0f, 0f    , -2.75f };
                    endDatas = new float[]   { 0f    , 0f, 2.75f, 2.75f, 0f   , 0f, -2.75f, -2.75f };
                    timeStep = 0;
                }
                break;
            case GameManager.OUT_CHANGES.MAN_1:// 男左右逆転
                {
                    Vector3 localScale = man.GetComponent<Transform>().localScale;
                    localScale.x *= -1f;
                    man.GetComponent<Transform>().localScale = localScale;
                }
                break;
            case GameManager.OUT_CHANGES.MAN_2:// 男移動
                {
                    timeCount = 0f;
                    timeEnd = 15f;
                    startDatas = new float[] { 5f };
                    endDatas = new float[] { 4.5f };
                }
                break;
            case GameManager.OUT_CHANGES.MAN_3:// 男大きく
                {
                    timeCount = 0f;
                    timeEnd = 15f;
                    startDatas = new float[] { 0.2f };
                    endDatas = new float[] { 0.3f };
                }
                break;
            case GameManager.OUT_CHANGES.MAN_4:// 男髪チェ
                {
                    man.hair.SetActive(false);
                    man.hair2.SetActive(true);
                }
                break;
            case GameManager.OUT_CHANGES.MAN_5:// 男イライラする
                {
                    man.aniTime = 0.35f;
                    foreach(var eyebrow in man.eyebrows)
                    {
                        eyebrow.GetComponent<Transform>().rotation = Quaternion.Euler(0, 0, 180);
                    }
                    man.mouse.GetComponent<Transform>().rotation = Quaternion.Euler(0, 0, 180);
                }
                break;
            case GameManager.OUT_CHANGES.FLOOR_1://床へ沈む
                {
                    timeCount = 0f;
                    timeEnd = 15f;
                    startDatas = new float[] { 0f };
                    endDatas = new float[] { -4.2f };
                }
                break;
            case GameManager.OUT_CHANGES.NOTICE_1:
                if (GameManager.isLeftStart)
                {
                    noticeTextR.text = "Back Now!";
                    noticeButtonTextR.text = "NO";
                }
                else
                {
                    noticeTextL.text = "Back Now!";
                    noticeButtonTextL.text = "NO";
                }
                break;
            case GameManager.OUT_CHANGES.CANVAS_1://目玉一杯出す
                {
                    eyesNum.SetActive(true);
                    List<Image> eyeList = new List<Image>(eyesNum.GetComponentsInChildren<Image>(true));
                    eyeList.RemoveAt(0);
                    foreach (var eye in eyeList)
                    {
                        eye.gameObject.SetActive(false);
                    }
                    List<int> nums = new List<int>();
                    for (int i = 0; i < eyeList.Count; i++)
                    {
                        nums.Add(i);
                    }
                    List<float> numsRandom = new List<float>();
                    while(nums.Count > 0)
                    {
                        int idx = UnityEngine.Random.Range(0, nums.Count);
                        numsRandom.Add((float)nums[idx]);
                        nums.RemoveAt(idx);
                    }
                    startDatas = numsRandom.ToArray();
                    timeCount = 0f;
                    timeEnd = 0.1f + 0.003f * eyeList.Count * eyeList.Count;
                    timeStep = 0;
                    //秘密番号表示
                    if (PlayerPrefs.HasKey("Ending") && PlayerPrefs.GetInt("Ending") >= 1)
                    {
                        if (GameManager.foundScrectNum < GameManager.instance.screctNums.Length && GameManager.instance.screctNums[GameManager.foundScrectNum] == GameManager.count)
                        {
                            if (GameManager.foundScrectNum + 1 < GameManager.instance.screctNums.Length)
                            {
                                Image imgNum = eyesNum.GetComponent<Image>();
                                Color colorNum = img.color;
                                imgNum.color = new Color(colorNum.r, colorNum.g, colorNum.b, 1f);
                                NumImageForCanvas numImageForCanvas = eyesNum.GetComponent<NumImageForCanvas>();
                                numImageForCanvas.SetNum(GameManager.instance.screctNums[GameManager.foundScrectNum + 1]);
                            }
                        }
                    }
                }
                break;
            case GameManager.OUT_CHANGES.DOOR_1://数字が回転
                break;
            case GameManager.OUT_CHANGES.TABLE_1: //もの増える
                {
                    timeCount = 0f;
                    timeEnd = 5f;
                    startDatas = new float[] { 0f };
                    endDatas = new float[] { 1f };
                    timeStep = 0;
                }
                break;
            case GameManager.OUT_CHANGES.TABLE_2: //テーブル移動
                { 
                }
                break;
            case GameManager.OUT_CHANGES.TABLE_3: //テーブル色チェンジ
                {
                    timeCount = 0f;
                    timeEnd = 30f;
                    startDatas = new float[] { 1f };
                    endDatas = new float[] { 0f };
                }
                break;
            case GameManager.OUT_CHANGES.TABLE_4: //テーブル回転
                {
                    timeCount = 0f;
                    timeEnd = 4f;
                    startDatas = new float[] { 0f };
                    endDatas = new float[] { 360f };
                }
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (!GameManager.isLive) return;
        if (!GameManager.instance.player.enableControll) return;
        if (GameManager.instance.player.isAuto) return;

        switch (GameManager.instance.changeNum)
        {
            case GameManager.OUT_CHANGES.WALL_1:// 壁線太くなる
                {
                    for (int i = 0; i < wall.strips.Count; i++)
                    {
                        var obj = wall.strips[i];
                        Transform tra = obj.GetComponent<Transform>();
                        Vector3 scale = tra.localScale;
                        scale.x = NowData(0);
                        tra.localScale = scale;
                    }
                }
                break;
            case GameManager.OUT_CHANGES.WALL_2:// 絵が逆さま
                {
                    
                }
                break;
            case GameManager.OUT_CHANGES.WALL_3:// 絵が落ちる
                {
                    for (int i = 0; i < wall_pictures1.Length; i++)
                    {
                        var obj = wall_pictures1[i];
                        Vector3 localPosition = obj.GetComponent<Transform>().localPosition;
                        if ((GameManager.instance.player.transform.localPosition - localPosition).magnitude < 6 && obj.GetComponent<Rigidbody2D>().gravityScale == 0f)
                        {
                            obj.GetComponent<Rigidbody2D>().gravityScale = 1f;
                            int r = (UnityEngine.Random.Range(0, 2) * 2 - 1) * UnityEngine.Random.Range(1, 31) * 2;
                            obj.GetComponent<Transform>().rotation = Quaternion.Euler(0, 0, r);
                        }
                    }

                    for (int i = 0; i < wall_pictures2.Length; i++)
                    {
                        var obj = wall_pictures2[i];
                        Vector3 localPosition = obj.GetComponent<Transform>().localPosition;
                        if ((GameManager.instance.player.transform.localPosition - localPosition).magnitude < 6 && obj.GetComponent<Rigidbody2D>().gravityScale == 0f)
                        {
                            obj.GetComponent<Rigidbody2D>().gravityScale = 1f;
                            int r = (UnityEngine.Random.Range(0, 2) * 2 - 1) * UnityEngine.Random.Range(1, 31) * 2;
                            obj.GetComponent<Transform>().rotation = Quaternion.Euler(0, 0, r);
                        }
                    }
                }
                break;
            case GameManager.OUT_CHANGES.WINDOW_1:// 外暗くなる
                {
                    float r = NowData(0);
                    window_bg.GetComponent<SpriteRenderer>().color = new Color(r, r, r);

                    Color c = window_light.GetComponent<SpriteRenderer>().color;
                    c.a = NowData(1);
                    window_light.GetComponent<SpriteRenderer>().color = c;
                }
                break;
            case GameManager.OUT_CHANGES.WINDOW_2:// 大きい頭
                {
                    Vector3 localPosition = window_Head.GetComponent<Transform>().localPosition;
                    localPosition.x = NowData(timeStep);
                    window_Head.GetComponent<Transform>().localPosition = localPosition;
                    if(timeStep == 0 || timeStep == 7)
                    {
                        Vector3 localScale = window_Head.GetComponent<Transform>().localScale;
                        localScale.x = 5f;
                        window_Head.GetComponent<Transform>().localScale = localScale;
                    }
                    else if (timeStep == 3)
                    {
                        Vector3 localScale = window_Head.GetComponent<Transform>().localScale;
                        localScale.x = -5f;
                        window_Head.GetComponent<Transform>().localScale = localScale;
                    }
                }
                break;
            case GameManager.OUT_CHANGES.MAN_1:// 男左右逆転
                {
                    
                }
                break;
            case GameManager.OUT_CHANGES.MAN_2:// 男移動
                {
                    Vector3 localPosition = man.GetComponent<Transform>().localPosition;
                    localPosition.x = NowData(0);
                    man.GetComponent<Transform>().localPosition = localPosition;
                }
                break;
            case GameManager.OUT_CHANGES.MAN_3:// 男大きく
                {
                    Vector3 localScale = man.GetComponent<Transform>().localScale;
                    localScale.x = NowData(0);
                    man.GetComponent<Transform>().localScale = localScale;
                }
                break;
            case GameManager.OUT_CHANGES.MAN_4:// 男髪チェ
                {
                    
                }
                break;
            case GameManager.OUT_CHANGES.MAN_5:// 男イライラする
                {

                }
                break;
            case GameManager.OUT_CHANGES.FLOOR_1://床へ沈む
                {

                    Vector3 localPosition = GameManager.instance.player.Skeletal.GetComponent<Transform>().localPosition;
                    Boolean canChange = localPosition.y != endDatas[0];
                    localPosition.y = NowData(0);
                    GameManager.instance.player.Skeletal.GetComponent<Transform>().localPosition = localPosition;
                    if (localPosition.y == endDatas[0] && canChange)
                    {
                        GameManager.instance.GameSet();   
                    }
                }
                break;
            case GameManager.OUT_CHANGES.CANVAS_1://目玉一杯出す
                {
                    
                }
                break;
            case GameManager.OUT_CHANGES.DOOR_1://数字が回転
                {
                    foreach(var doorNum in doorNums)
                    {
                        // transformを取得
                        Transform transform = doorNum.transform;

                        // ローカル座標基準で、現在の回転量へ加算する
                        transform.Rotate(0f, 0f, -15.0f * Time.deltaTime);
                    }
                }
                break;
            case GameManager.OUT_CHANGES.TABLE_1: //もの増える
                {
                    List<GameObject> objs = new List<GameObject>();
                    if (timeStep < book1.Length) objs.Add(book1[timeStep]);
                    if (timeStep < book2.Length) objs.Add(book2[timeStep]);
                    if (timeStep < vase1.Length) objs.Add(vase1[timeStep]);
                    if (timeStep < vase2.Length) objs.Add(vase2[timeStep]);
                    foreach (var obj in objs)
                    {
                        Color c = obj.GetComponent<SpriteRenderer>().color;
                        c.a = NowData(0);
                        obj.GetComponent<SpriteRenderer>().color = c;
                    }
                }
                break;
            case GameManager.OUT_CHANGES.TABLE_2: //テーブル移動
                {
                    for (int i = 0; i < tables.Length; i++)
                    {
                        var table = tables[i];

                        if (!tablesMoved[i] && Math.Abs(GameManager.instance.player.transform.localPosition.x - table.transform.localPosition.x) <= 3f)
                        {
                            tablesMoved[i] = true;
                            timeCount = 0f;
                            timeEnd = 0.2f;
                            startDatas = new float[] { table.transform.localPosition.y };
                            endDatas = new float[] { GameManager.instance.player.transform.localPosition.y };
                            timeStep = i;
                        }
                    }

                    if (tablesMoved[timeStep])
                    {
                        var table = tables[timeStep];
                        Vector3 localPosition = table.GetComponent<Transform>().localPosition;
                        localPosition.y = NowData(0);
                        table.GetComponent<Transform>().localPosition = localPosition;
                    }
                }
                break;
            case GameManager.OUT_CHANGES.TABLE_3: //テーブル色チェンジ
                {
                    foreach (var armchair in armchairs)
                    {
                        Color c = armchair.GetComponent<SpriteRenderer>().color;
                        c.b = NowData(0);
                        armchair.GetComponent<SpriteRenderer>().color = c;
                    }
                }
                break;
            case GameManager.OUT_CHANGES.TABLE_4: //テーブル回転
                {
                    Vector2 center = new Vector2(-12f, -4.2f);
                    Vector2 r = new Vector2(3f, 1.4f);
                    
                    for(int i = 0; i < armchairs.Length; i++)
                    {
                        var armchair = armchairs[i];
                        Vector3 localPosition = armchair.GetComponent<Transform>().localPosition;
                        float dgree = (NowData(0) + i * 90f) % 360f;
                        Vector2 move = new Vector2(r.x * (float)Math.Cos(Math.PI * dgree / 180.0f), r.y * (float)Math.Sin(Math.PI * dgree / 180.0f));
                        localPosition.x = center.x + move.x;
                        localPosition.y = center.y - move.y;
                        armchair.GetComponent<Transform>().localPosition = localPosition;
                    }

                    Vector2 center2 = new Vector2(8.2f, -4.08f);
                    Vector2 r2 = new Vector2(3f, 0f);

                    Vector3 localPosition2 = table2.GetComponent<Transform>().localPosition;
                    float dgree2 = (NowData(0) + 270f) % 360f;
                    Vector2 move2 = new Vector2(r2.x * (float)Math.Cos(Math.PI * dgree2 / 180.0f), r2.y * (float)Math.Sin(Math.PI * dgree2 / 180.0f));
                    localPosition2.x = center2.x + move2.x;
                    localPosition2.y = center2.y - move2.y;
                    table2.GetComponent<Transform>().localPosition = localPosition2;

                }
                break;
            default:
                break;
        }

        if (timeCount < timeEnd)
        {
            timeCount += Time.deltaTime;
        }
        else
        {
            timeCount = timeEnd;
            switch (GameManager.instance.changeNum)
            {
                case GameManager.OUT_CHANGES.WINDOW_2:// 大きい頭
                    {
                        timeCount = 0f;
                        timeStep++;
                        if(timeStep >= startDatas.Length)
                        {
                            timeStep = 0;
                        }
                    }
                    break;
                case GameManager.OUT_CHANGES.CANVAS_1:
                    {
                        List<Image> eyeList = new List<Image>(eyesNum.GetComponentsInChildren<Image>(true));
                        eyeList.RemoveAt(0);
                        if (timeStep < eyeList.Count)
                        {
                            eyeList[(int)startDatas[timeStep]].gameObject.SetActive(true);
                            timeStep++;
                            timeCount = 0f;
                            timeEnd = 0.1f + 0.003f * (eyeList.Count - timeStep) * (eyeList.Count - timeStep);
                        }
                        else if(timeStep == eyeList.Count)
                        {
                            timeStep++;
                            GameManager.instance.GameSet();
                        }
                    }
                    break;
                case GameManager.OUT_CHANGES.TABLE_1: //もの増える
                    {
                        timeCount = 0f;
                        if (timeStep < 999)
                            timeStep++;
                    }
                    break;
                case GameManager.OUT_CHANGES.TABLE_2: //テーブル移動
                    {
                        
                    }
                    break;
                case GameManager.OUT_CHANGES.TABLE_4: //テーブル回転
                    {
                        timeCount = 0f;
                    }
                    break;
                default:
                    break;
            }
        }
    }

}
