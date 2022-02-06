using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;

public class Presenter : MonoBehaviour
{
    [SerializeField]
    private MainGame_Model model;

    [SerializeField]
    private Result_View[] resultViews;

    [SerializeField]
    private Result_Model[] resultModels;

    [SerializeField]
    private Info_View infoView;

    [SerializeField]
    private Info_Model infoMode;

    [SerializeField]
    private Button btnRestart;

    [SerializeField]
    private Grid_View[] gridViews;


    void Start()
    {
        // �Q�[���̏����ݒ���s���AGrid_View �̏���z��Ŏ󂯎��
        gridViews = model.InitialSettings();

        // ���X�^�[�g�{�^���� InfoView �̐ݒ�
        SetUpRestartButtonAndInfoView();

        /// <summary>
        /// ���X�^�[�g�{�^���� InfoView �̐ݒ�
        /// </summary>
        void SetUpRestartButtonAndInfoView() {

            // View => Model�@�{�^������������(View)�AModel �̏��������s����
            btnRestart.OnClickAsObservable()
                .ThrottleFirst(TimeSpan.FromSeconds(1.0f))
                .Subscribe(_ => model.Restart()).AddTo(gameObject);

            // Model => View�@���b�Z�[�W��i�s���(Model)���Ď����A����炪�X�V���ꂽ��AView �̏��������s����
            //model.InfoMessage.Subscribe(x => infoView.UpdateDispayInfo(x));

            infoMode.InfoMessage.Subscribe(x => infoView.UpdateDispayInfo(x)).AddTo(gameObject);

            model.IsGameUp.Subscribe(x => {
                btnRestart.interactable = x;
                PrepareResult(model.winner);
            }).AddTo(gameObject);
        }

        // ResultView �� ResultModel �̐ݒ�
        SetUpResult();

        /// <summary>
        /// ResultView �� ResultModel �̐ݒ�
        /// </summary>
        void SetUpResult() {

            // Model => View
            // GameManager �� RectiveDictionary ���w�ǂ�(Model)�A�������̃J�E���g�ɍ��킹�ĉ�ʂ��X�V
            for (int i = 0; i < resultViews.Length; i++) {

                // Subscribe �̒l�� Length �ɌŒ肳��Ă��܂��A�o�^�͐������邪�A�X�V���Ɏ��s����̂ŁA1��l���󂯂�
                // https://qiita.com/isogaminokaze/items/350281f10fa1f2317f7c
                int a = i;

                // �������̍w�ǂƍX�V
                resultModels[a].WinCount.Subscribe(x => resultViews[a].UpdateDisplayWincount(x)).AddTo(gameObject);
                resultModels[a].SetUpResultModel((GridOwnerType)a + 1);

                // Dictionary �̏ꍇ
                //resultModels[a].WinInfoData.ObserveReplace()
                //    .Where(x => resultModels[a].WinInfoData.ContainsKey(x.Key))
                //    .Subscribe((DictionaryReplaceEvent<GridOwnerType, int> result) => resultViews[a].UpdateDisplayWincount(result.NewValue));
                Debug.Log("�w�ǊJ�n : " + resultModels[a].CurrentGridOwnerType.ToString());

                // Result_Model �� RectiveProperty ���w�ǂ��A���s���ʂɍ��킹�ĉ�ʂ��X�V
                resultModels[a].ResultMessage.Subscribe(x => resultViews[a].UpdateDisplayResultGame(x)).AddTo(gameObject);
                Debug.Log("���s���� �w�ǊJ�n : " + resultModels[a].CurrentGridOwnerType.ToString());

                resultModels[a].InitResultMessage();
            }
        }

        // �������̏������A�Q�[�����̏�����
        //model.InitWinCount();
        model.ResetGameParameters();

        // GridOwnerType ���w�ǂ��AText ���X�V����
        for (int i =0; i < model.gridModelList.Count; i++) {
            int index = i;

            // �e Grid �{�^���̍w�ǂ��s���A�N���b�N(�^�b�v)���͂���t
            gridViews[index].GetGridButton().OnClickAsObservable()
                .ThrottleFirst(TimeSpan.FromSeconds(1.0f))
                .Select(_ => model.gridModelList[index].GridNo)�@�@// Subscribe �̈����p�ɃX�g���[���̏��� gridNo �ɒu�������� 
                .Subscribe(x => model.OnClickGrid(x))              // x �� gridNo
                .AddTo(this);

            // �e Grid_Model �̃I�[�i�[�����w�ǂ��A�X�V���ꂽ�ۂɂ͉�ʕ\�����X�V����
            model.gridModelList[index].CurrentGridOwnerType
                .Subscribe(x => gridViews[index].UpdateGridOwnerSymbol(x == GridOwnerType.Player ? "�Z" : x == GridOwnerType.Opponent ? "�~" : string.Empty))
                .AddTo(this);
        }
    }

    /// <summary>
    /// ���U���g�̏���
    /// </summary>
    /// <param name="winner"></param>
    public void PrepareResult(GridOwnerType winner) {

        // ���ʕ\��
        for (int i = 0; i < resultModels.Length; i++) {
            resultModels[i].ShowResult(winner);
        }
    }
}
