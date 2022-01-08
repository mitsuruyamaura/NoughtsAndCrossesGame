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
            for (int i = 0; i < resultViews.Length; i++) {

                // Subscribe の値が Length に固定されてしまい、登録は成功するが、更新時に失敗するので、1回値を受ける
                // https://qiita.com/isogaminokaze/items/350281f10fa1f2317f7c
                int a = i;

                model.WinCount.ObserveReplace()
                    .Where(x => x.Key == resultViews[a].GridOwnerType)
                    .Subscribe((DictionaryReplaceEvent<GridOwnerType, int> result) => resultViews[a].UpdateDisplayWincount(result.NewValue));
                Debug.Log("購読開始 : " + resultViews[a].GridOwnerType.ToString());

                // GameManager の RectiveProperty を購読し、勝敗結果に合わせて画面を更新
                if (resultViews[a].GridOwnerType == GridOwnerType.Player) {
                    model.PlayerResultMessage.Subscribe(x => resultViews[a].UpdateDisplayResultGame(x));
                    Debug.Log("勝敗結果 購読開始 : " + resultViews[a].GridOwnerType.ToString());
                } else {
                    model.OpponentResultMessage.Subscribe(x => resultViews[a].UpdateDisplayResultGame(x));
                    Debug.Log("勝敗結果 購読開始 : " + resultViews[a].GridOwnerType.ToString());
                }
            }
        }

        model.InitWinCount();
        model.ResetGameParameters();
    }
}
