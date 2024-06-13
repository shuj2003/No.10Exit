using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public GameObject[] screctObjects;

    private float timeCount = 1f;
    private float timeEnd = 1f;
    private float[] startDatas;
    private float[] endDatas;

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
                                var idx = Random.Range(0, texts.Length);
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
                    endDatas = new float[] { 0.24f };
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
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (!GameManager.isLive) return;

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
                            int r = (Random.Range(0, 2) * 2 - 1) * Random.Range(1, 31) * 2;
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
                            int r = (Random.Range(0, 2) * 2 - 1) * Random.Range(1, 31) * 2;
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
            default:
                break;
        }

        if(timeCount < timeEnd)
            timeCount += Time.deltaTime;

    }
}
