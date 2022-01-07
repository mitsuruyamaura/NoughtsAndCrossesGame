using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;

public class Info_Model : MonoBehaviour
{
    [SerializeField]
    private Button btnRestart;

    private GameManager gameManager;


    public void SetUpInfoModel(GameManager gameManager) {
        this.gameManager = gameManager;

        btnRestart.OnClickAsObservable()
            .ThrottleFirst(TimeSpan.FromSeconds(1.0f))
            .Subscribe(_ => OnClickRestart());
    }

    public void OnClickRestart() {
        gameManager.Restart();
    }

    public void SwitchActivateButton(bool isSwitch) {
        btnRestart.interactable = isSwitch;
    }
}
