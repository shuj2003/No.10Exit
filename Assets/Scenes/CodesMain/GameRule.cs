using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameRule : MonoBehaviour
{

    public Text text;

    public void ShowText(int num)
    {
        text.text = string.Format(text.text, num.ToString());
    }

}
