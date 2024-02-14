using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject doorLeft;
    public GameObject doorRight;
    public Notice noticeLeft;
    public Notice noticeRight;

    public Man man;
    public Player player;

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
        if(Vector2.SqrMagnitude(doorLeft.transform.position - player.transform.position) < 2f * 2f)
        {
            noticeLeft.Show();
        }
        else
        {
            noticeLeft.Hide();
        }

        if (Vector2.SqrMagnitude(doorRight.transform.position - player.transform.position) < 2f * 2f)
        {
            noticeRight.Show();
        }
        else
        {
            noticeRight.Hide();
        }
    }

}
