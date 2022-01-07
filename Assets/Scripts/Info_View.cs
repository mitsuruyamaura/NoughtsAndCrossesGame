using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;

public class Info_View : MonoBehaviour
{
    [SerializeField]
    private Text txtInfo;


    public void UpdateDispayInfo(string message) {
        txtInfo.text = message;
    } 
}
