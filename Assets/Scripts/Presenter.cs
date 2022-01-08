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

    [SerializeField]
    private Result_View[] resultViews;

    [SerializeField]
    private Result_View playerResultView;

    [SerializeField]
    private Result_View opponentResultView;


    void Start()
    {
        model.InitialSettings();

        SetUpRestartButtonAndInfoView();

        /// <summary>
        /// 
        /// </summary>
        void SetUpRestartButtonAndInfoView() {

            // View => Model　ボタンを押したら(View)、Model の処理を実行する
            btnRestart.OnClickAsObservable()
                .ThrottleFirst(TimeSpan.FromSeconds(1.0f))
                .Subscribe(_ => model.Restart());

            // Model => View　メッセージや進行状態(Model)を監視し、それらが更新されたら、View の処理を実行する
            model.InfoMessage.Subscribe(x => infoView.UpdateDispayInfo(x));
            model.IsGameUp.Subscribe(x => btnRestart.interactable = x);
        }

        SetUpResultView();

        /// <summary>
        /// 
        /// </summary>
        void SetUpResultView() {

            // Model => View
            // GameManager の RectiveDictionary を購読し(Model)、勝利数のカウントに合わせて画面を更新
            //for (int i = 0; i < resultViews.Length; i++) {

            model.WinCount.ObserveReplace()
                .Where(x => x.Key == playerResultView.GridOwnerType)
                .Subscribe((DictionaryReplaceEvent<GridOwnerType, int> result) => playerResultView.UpdateDisplayWincount(result.NewValue));
            Debug.Log("購読開始 : " + playerResultView.GridOwnerType.ToString());

            model.WinCount.ObserveReplace()
                    .Where(x => x.Key == opponentResultView.GridOwnerType)
                    .Subscribe((DictionaryReplaceEvent<GridOwnerType, int> result) => opponentResultView.UpdateDisplayWincount(result.NewValue));
            Debug.Log("購読開始 : " + opponentResultView.GridOwnerType.ToString());

            // GameManager の RectiveProperty を購読し、勝敗結果に合わせて画面を更新

            model.PlayerResultMessage.Subscribe(x => playerResultView.UpdateDisplayResultGame(x));
            Debug.Log("勝敗結果 購読開始 : " + playerResultView.GridOwnerType.ToString());

            model.OpponentResultMessage.Subscribe(x => opponentResultView.UpdateDisplayResultGame(x));
            Debug.Log("勝敗結果 購読開始 : " + opponentResultView.GridOwnerType.ToString());

            //}
        }

        model.InitWinCount();
        model.ResetGameParameters();
    }
}
