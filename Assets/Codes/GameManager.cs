using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Door doorLeft;
    public Door doorRight;
    public GameObject startPointL;
    public GameObject startPointR;
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
        if(Vector2.SqrMagnitude(startPointL.transform.position - player.transform.position) < 2f * 2f)
        {
            noticeLeft.Show();
        }
        else
        {
            noticeLeft.Hide();
        }

        if (Vector2.SqrMagnitude(startPointR.transform.position - player.transform.position) < 2f * 2f)
        {
            noticeRight.Show();
        }
        else
        {
            noticeRight.Hide();
        }
    }

    public void inDoorL()
    {
        doorLeft.openDoor();
        player.OutDoor(startPointL.transform.position, delegate () {
            doorLeft.closeDoor();
        });
    }

    public void inDoorR()
    {
        doorRight.openDoor();
        player.OutDoor(startPointR.transform.position, delegate () {
            doorRight.closeDoor();
        });
    }

}
