using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ScreenText : MonoBehaviour
{
    public static ScreenText screenTxt;
    public RectTransform screenTxtBox;

    void Awake()
    {
        screenTxt = this;
    }

    public void Full()
    {
        Sequence s = DOTween.Sequence();
        s.Append(screenTxtBox.DOScale(new Vector3(1f, 1f, 1f), 0.75f));
        s.Append(screenTxtBox.DOScale(new Vector3(0, 0, 0), 0.75f));
    }
}