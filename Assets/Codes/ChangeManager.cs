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
    void Start()
    {
        switch (GameManager.instance.changeNum)
        {
            case GameManager.OUT_CHANGES.WALL_1:// 変化：真ん中のボー段々太くなります。
                {
                    timeCount = 0f;
                    timeEnd = 15f;
                    startDatas = new float[] { 0.5f };
                    endDatas = new float[] { 1f };
                }
                break;
            case GameManager.OUT_CHANGES.WALL_2:// 変化：絵全部逆さま
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
            case GameManager.OUT_CHANGES.WALL_3:// 絵が落ちます
                {
                }
                break;
            case GameManager.OUT_CHANGES.WINDOW_1:// 変化：段々夜になります
                {
                    timeCount = 0f;
                    timeEnd = 15f;
                    startDatas = new float[] { 1f, 1f };
                    endDatas = new float[] { 0.25f, 0f };
                }
                break;
            case GameManager.OUT_CHANGES.MAN_1:// 変化：向き逆
                {
                    Vector3 localScale = man.GetComponent<Transform>().localScale;
                    localScale.x *= -1f;
                    man.GetComponent<Transform>().localScale = localScale;
                }
                break;
            case GameManager.OUT_CHANGES.MAN_2:// 変化：移動
                {
                    timeCount = 0f;
                    timeEnd = 15f;
                    startDatas = new float[] { 5f };
                    endDatas = new float[] { 4.5f };
                }
                break;
            case GameManager.OUT_CHANGES.MAN_3:// 変化：大きいめ
                {
                    timeCount = 0f;
                    timeEnd = 15f;
                    startDatas = new float[] { 0.2f };
                    endDatas = new float[] { 0.24f };
                }
                break;
            case GameManager.OUT_CHANGES.MAN_4:// 変化：髪型
                {
                    man.hair.SetActive(false);
                    man.hair2.SetActive(true);
                }
                break;
            case GameManager.OUT_CHANGES.MAN_5:// 足動き速くする
                {
                    man.aniTime = 0.35f;
                }
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

        switch (GameManager.instance.changeNum)
        {
            case GameManager.OUT_CHANGES.WALL_1:// 変化：真ん中のボー段々太くなります。
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
            case GameManager.OUT_CHANGES.WALL_2:// 変化：絵全部逆さま
                {
                    
                }
                break;
            case GameManager.OUT_CHANGES.WALL_3:// 絵が落ちます
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
            case GameManager.OUT_CHANGES.WINDOW_1:// 変化：段々夜になります
                {
                    // 程度は0.75f,10秒かけて変化します
                    float r = NowData(0);
                    window_bg.GetComponent<SpriteRenderer>().color = new Color(r, r, r);

                    // 程度は1f,10秒かけて変化します
                    Color c = window_light.GetComponent<SpriteRenderer>().color;
                    c.a = NowData(1);
                    window_light.GetComponent<SpriteRenderer>().color = c;
                }
                break;
            case GameManager.OUT_CHANGES.MAN_1:// 変化：向き逆
                {
                    
                }
                break;
            case GameManager.OUT_CHANGES.MAN_2:// 変化：移動
                {
                    Vector3 localPosition = man.GetComponent<Transform>().localPosition;
                    localPosition.x = NowData(0);
                    man.GetComponent<Transform>().localPosition = localPosition;
                }
                break;
            case GameManager.OUT_CHANGES.MAN_3:// 変化：大きいめ
                {
                    Vector3 localScale = man.GetComponent<Transform>().localScale;
                    localScale.x = NowData(0);
                    man.GetComponent<Transform>().localScale = localScale;
                }
                break;
            case GameManager.OUT_CHANGES.MAN_4:// 変化：髪型
                {
                    
                }
                break;
            case GameManager.OUT_CHANGES.MAN_5:// 足動き速くする
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
