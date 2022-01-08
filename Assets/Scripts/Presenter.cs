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

        // View => Model�@�{�^������������(View)�AModel �̏��������s����
        btnRestart.OnClickAsObservable()
            .ThrottleFirst(TimeSpan.FromSeconds(1.0f))
            .Subscribe(_ => model.Restart());
        
        // Model => View�@���b�Z�[�W��i�s���(Model)���Ď����A����炪�X�V���ꂽ��AView �̏��������s����
        model.InfoMessage.Subscribe(x => infoView.UpdateDispayInfo(x));
        model.IsGameUp.Subscribe(x => btnRestart.interactable = x);
    }
}
