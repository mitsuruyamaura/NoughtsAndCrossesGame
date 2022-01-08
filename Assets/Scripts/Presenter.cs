using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;

public class Presenter : MonoBehaviour
{
    [SerializeField]
    private Button btnRestart;

    [SerializeField]
    private GameManager model;

    [SerializeField]
    private Info_View infoView;


    void Start()
    {
        model.SetUpModel();

        // View => Model　ボタンを押したら(View)、Model の処理を実行する
        btnRestart.OnClickAsObservable()
            .ThrottleFirst(TimeSpan.FromSeconds(1.0f))
            .Subscribe(_ => model.Restart());
        
        // Model => View　メッセージや進行状態(Model)を監視し、それらが更新されたら、View の処理を実行する
        model.InfoMessage.Subscribe(x => infoView.UpdateDispayInfo(x));
        model.IsGameUp.Subscribe(x => btnRestart.interactable = x);
    }
}
