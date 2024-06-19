using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumImageForCanvas : MonoBehaviour
{
    public List<Sprite> componentList = new List<Sprite>();

    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetNum(int num)
    {
        image.sprite = componentList[num];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
