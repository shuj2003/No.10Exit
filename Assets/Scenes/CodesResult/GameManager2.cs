using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

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
    public ResultNotice resultNotice;
    public GameObject tapAnywhere;

    public Player2 player;
    public GameObject head;

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

    public void TapAnywhere()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Decision);

        fullScreenFade.gameObject.SetActive(true);
        fullScreenFade.FadeIn(delegate () {
            
            GameManager.count = 0;
            GameManager.isLeftStart = true;
            GameManager.isLive = false;

            SceneManager.LoadScene(0);

        });
    }

    private IEnumerator Wait(Action action, float second)
    {
        yield return new WaitForSeconds(second);
        if (action != null) action();
    }

    public void StarttHome()
    {
        player.DeadAction = delegate () {

            //ƒLƒƒƒ‰Ž€–SƒV[ƒ“
            fullScreenFade.gameObject.SetActive(true);
            head.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            head.transform.position = player.transform.position - new Vector3(1f, 1f, 0f);
            fullScreenFade.gameObject.SetActive(true);
            var img = fullScreenFade.image.GetComponent<Image>();
            Color color = img.color;
            img.color = new Color(color.r, color.g, color.b, 1f);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.HIT);
            StartCoroutine(Wait(delegate ()
            {
                head.GetComponent<Rigidbody2D>().velocity = new Vector2(-1f, 0f);
                head.GetComponent<Rigidbody2D>().angularVelocity = 135f;

                fullScreenFade.FadeOut(delegate () {

                    AudioManager.instance.PlaySfx(AudioManager.Sfx.Laughter);

                    doorCenter.CloseDoor(null);

                    StartCoroutine(Wait(delegate ()
                    {

                        fullScreenFade2.gameObject.SetActive(true);
                        fullScreenFade2.FadeIn(delegate () {

                            gameResult.gameObject.SetActive(true);

                            gameResult.ShowResultGameOver();

                            if (!PlayerPrefs.HasKey("Ending"))
                            {
                                PlayerPrefs.SetInt("Ending", 1);
                                resultNotice.Show("Ending 1\nClear!");

                                StartCoroutine(Wait(delegate ()
                                {
                                    tapAnywhere.SetActive(true);
                                }, 5f));
                            }
                            else
                            {
                                tapAnywhere.SetActive(true);
                            }

                        });

                    }, 1f)
                    );

                });

            }, 2f)
            );

        };

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

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);

        enableUI = false;
        if (GameManager.foundScrectNum >= GameManager.instance.screctNums.Length)
        {
            InDoorC(delegate ()
            {
                fullScreenFade2.gameObject.SetActive(true);
                fullScreenFade2.FadeIn(delegate () {

                    gameResult.gameObject.SetActive(true);

                    gameResult.ShowResultGameClear();

                    if (!PlayerPrefs.HasKey("Ending") || PlayerPrefs.GetInt("Ending") != 2)
                    {
                        PlayerPrefs.SetInt("Ending", 2);
                        resultNotice.Show("Ending\nAll Clear!");

                        StartCoroutine(Wait(delegate ()
                        {
                            tapAnywhere.SetActive(true);
                        }, 5f));
                    }
                    else
                    {
                        tapAnywhere.SetActive(true);
                    }

                });
            });
        }
        else
        {
            InDoorFail(delegate ()
            {
                HeadAttack(null);
            });
        }

    }

    private void outDoorL(Action action)
    {
        doorLeft.OpenDoor(null);
        player.OutDoor(startPointL.transform.position, delegate () {
            doorLeft.CloseDoor(delegate () {
                doorLeft.FadeOut(delegate() {
                    if (action != null) action();
                });
            });
        });
    }

    private void InDoorC(Action action)
    {
        doorCenter.OpenDoor(null);
        player.InDoor(startPointC.transform.position, delegate () {
            doorCenter.CloseDoor(delegate () {
                if (action != null) action();
            });
        });
    }

    private void InDoorFail(Action action)
    {
        player.AutoMove(startPointC.transform.position);
        doorCenter.OpenDoor(delegate () {
            if (action != null) action();
        });
    }

    private void HeadAttack(Action action)
    {
        Vector2 vec = player.headPos.transform.position - head.transform.position;
        head.GetComponent<Rigidbody2D>().velocity = vec.normalized * 50f;
    }

}
