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

            // View => Model�@�{�^������������(View)�AModel �̏��������s����
            btnRestart.OnClickAsObservable()
                .ThrottleFirst(TimeSpan.FromSeconds(1.0f))
                .Subscribe(_ => model.Restart());

            // Model => View�@���b�Z�[�W��i�s���(Model)���Ď����A����炪�X�V���ꂽ��AView �̏��������s����
            model.InfoMessage.Subscribe(x => infoView.UpdateDispayInfo(x));
            model.IsGameUp.Subscribe(x => btnRestart.interactable = x);
        }

        SetUpResultView();

        /// <summary>
        /// 
        /// </summary>
        void SetUpResultView() {

            // Model => View
            // GameManager �� RectiveDictionary ���w�ǂ�(Model)�A�������̃J�E���g�ɍ��킹�ĉ�ʂ��X�V
            //for (int i = 0; i < resultViews.Length; i++) {

            model.WinCount.ObserveReplace()
                .Where(x => x.Key == playerResultView.GridOwnerType)
                .Subscribe((DictionaryReplaceEvent<GridOwnerType, int> result) => playerResultView.UpdateDisplayWincount(result.NewValue));
            Debug.Log("�w�ǊJ�n : " + playerResultView.GridOwnerType.ToString());

            model.WinCount.ObserveReplace()
                    .Where(x => x.Key == opponentResultView.GridOwnerType)
                    .Subscribe((DictionaryReplaceEvent<GridOwnerType, int> result) => opponentResultView.UpdateDisplayWincount(result.NewValue));
            Debug.Log("�w�ǊJ�n : " + opponentResultView.GridOwnerType.ToString());

            // GameManager �� RectiveProperty ���w�ǂ��A���s���ʂɍ��킹�ĉ�ʂ��X�V

            model.PlayerResultMessage.Subscribe(x => playerResultView.UpdateDisplayResultGame(x));
            Debug.Log("���s���� �w�ǊJ�n : " + playerResultView.GridOwnerType.ToString());

            model.OpponentResultMessage.Subscribe(x => opponentResultView.UpdateDisplayResultGame(x));
            Debug.Log("���s���� �w�ǊJ�n : " + opponentResultView.GridOwnerType.ToString());

            //}
        }

        model.InitWinCount();
        model.ResetGameParameters();
    }
}
