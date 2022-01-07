using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;

public class Info_View : MonoBehaviour
{
    [SerializeField]
    private Button btnRestart;

    [SerializeField]
    private Text txtInfo;

    private GameManager gameManager;


    public void SetUpinfoView(GameManager gameManager) {
        this.gameManager = gameManager;

        btnRestart.OnClickAsObservable()
            .ThrottleFirst(TimeSpan.FromSeconds(1.0f))
            .Subscribe(_ => OnClickRestart());

        gameManager.InfoMessage.Subscribe(x => UpdateDispayInfo(x));
    }


    private void UpdateDispayInfo(string message) {
        txtInfo.text = message;
    } 


    private void OnClickRestart() {
        gameManager.Restart();
    }


    public void SwitchActivateButton(bool isSwitch) {
        btnRestart.interactable = isSwitch;
    }
}
