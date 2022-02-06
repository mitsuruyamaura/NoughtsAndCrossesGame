using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class Info_Model : MonoBehaviour
{
    public ReactiveProperty<string> InfoMessage = new ReactiveProperty<string>();

    /// <summary>
    /// ílÇÃçXêV
    /// </summary>
    /// <param name="message"></param>
    public void UpdateInfoMessage(string message) {
        InfoMessage.Value = message;
    }
}