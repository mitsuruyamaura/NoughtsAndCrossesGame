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
            for (int i = 0; i < resultViews.Length; i++) {

                // Subscribe �̒l�� Length �ɌŒ肳��Ă��܂��A�o�^�͐������邪�A�X�V���Ɏ��s����̂ŁA1��l���󂯂�
                // https://qiita.com/isogaminokaze/items/350281f10fa1f2317f7c
                int a = i;

                model.WinCount.ObserveReplace()
                    .Where(x => x.Key == resultViews[a].GridOwnerType)
                    .Subscribe((DictionaryReplaceEvent<GridOwnerType, int> result) => resultViews[a].UpdateDisplayWincount(result.NewValue));
                Debug.Log("�w�ǊJ�n : " + resultViews[a].GridOwnerType.ToString());

                // GameManager �� RectiveProperty ���w�ǂ��A���s���ʂɍ��킹�ĉ�ʂ��X�V
                if (resultViews[a].GridOwnerType == GridOwnerType.Player) {
                    model.PlayerResultMessage.Subscribe(x => resultViews[a].UpdateDisplayResultGame(x));
                    Debug.Log("���s���� �w�ǊJ�n : " + resultViews[a].GridOwnerType.ToString());
                } else {
                    model.OpponentResultMessage.Subscribe(x => resultViews[a].UpdateDisplayResultGame(x));
                    Debug.Log("���s���� �w�ǊJ�n : " + resultViews[a].GridOwnerType.ToString());
                }
            }
        }

        model.InitWinCount();
        model.ResetGameParameters();
    }
}
