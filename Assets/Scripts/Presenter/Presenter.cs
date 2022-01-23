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
    private MainGame_Model model;

    [SerializeField]
    private Info_View infoView;

    [SerializeField]
    private Result_View[] resultViews;

    [SerializeField]
    private Result_Model[] resultModels;

    [SerializeField, HideInInspector]
    private Grid_View[] gridViews;


    void Start()
    {
        // ゲームの初期設定を行い、Grid_View の情報を配列で受け取る
        gridViews = model.InitialSettings();

        // リスタートボタンと InfoView の設定
        SetUpRestartButtonAndInfoView();

        /// <summary>
        /// リスタートボタンと InfoView の設定
        /// </summary>
        void SetUpRestartButtonAndInfoView() {

            // View => Model　ボタンを押したら(View)、Model の処理を実行する
            btnRestart.OnClickAsObservable()
                .ThrottleFirst(TimeSpan.FromSeconds(1.0f))
                .Subscribe(_ => model.Restart());

            // Model => View　メッセージや進行状態(Model)を監視し、それらが更新されたら、View の処理を実行する
            model.InfoMessage.Subscribe(x => infoView.UpdateDispayInfo(x));

            model.IsGameUp.Subscribe(x => {
                btnRestart.interactable = x;
                GameUp(model.winner);
            });
        }

        // ResultView と ResultModel の設定
        SetUpResult();

        /// <summary>
        /// ResultView と ResultModel の設定
        /// </summary>
        void SetUpResult() {

            // Model => View
            // GameManager の RectiveDictionary を購読し(Model)、勝利数のカウントに合わせて画面を更新
            for (int i = 0; i < resultViews.Length; i++) {

                // Subscribe の値が Length に固定されてしまい、登録は成功するが、更新時に失敗するので、1回値を受ける
                // https://qiita.com/isogaminokaze/items/350281f10fa1f2317f7c
                int a = i;

                // 勝利数の購読と更新
                resultModels[a].WinCount.Subscribe(x => resultViews[a].UpdateDisplayWincount(x));
                resultModels[a].InitWinCount();

                // Dictionary の場合
                //resultModels[a].WinInfoData.ObserveReplace()
                //    .Where(x => resultModels[a].WinInfoData.ContainsKey(x.Key))
                //    .Subscribe((DictionaryReplaceEvent<GridOwnerType, int> result) => resultViews[a].UpdateDisplayWincount(result.NewValue));
                Debug.Log("購読開始 : " + resultModels[a].currentGridOwnerType.ToString());

                // Result_Model の RectiveProperty を購読し、勝敗結果に合わせて画面を更新
                resultModels[a].ResultMessage.Subscribe(x => resultViews[a].UpdateDisplayResultGame(x));
                Debug.Log("勝敗結果 購読開始 : " + resultModels[a].currentGridOwnerType.ToString());

                resultModels[a].InitResultMessage();
            }
        }

        // 勝利数の初期化、ゲーム情報の初期化
        //model.InitWinCount();
        model.ResetGameParameters();

        // GridOwnerType を購読し、Text を更新する
        for (int i =0; i < model.gridModelList.Count; i++) {
            int index = i;

            // 各 Grid ボタンの購読を行い、クリック(タップ)入力を受付
            gridViews[index].GetGridButton().OnClickAsObservable()
                .ThrottleFirst(TimeSpan.FromSeconds(1.0f))
                .Select(_ => model.gridModelList[index].GridNo)　　// Subscribe の引数用にストリームの情報を gridNo に置き換える 
                .Subscribe(x => model.OnClickGrid(x))              // x は gridNo
                .AddTo(this);

            // 各 Grid_Model のオーナー情報を購読し、更新された際には画面表示を更新する
            model.gridModelList[index].CurrentGridOwnerType
                .Subscribe(x => gridViews[index].UpdateGridOwnerSymbol(x == GridOwnerType.Player ? "〇" : x == GridOwnerType.Opponent ? "×" : string.Empty))
                .AddTo(this);
        }
    }

    /// <summary>
    /// ゲーム終了
    /// </summary>
    /// <param name="winner"></param>
    public void GameUp(GridOwnerType winner) {

        // 結果表示
        for (int i = 0; i < resultModels.Length; i++) {
            resultModels[i].ShowResult(winner);
        }
    }
}
