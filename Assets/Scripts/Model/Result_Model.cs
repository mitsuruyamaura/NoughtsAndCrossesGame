using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class Result_Model : MonoBehaviour
{
    //public ReactiveDictionary<GridOwnerType, int> WinInfoData = new ReactiveDictionary<GridOwnerType, int>();

    private GridOwnerType currentGridOwnerType;

    /// <summary>
    /// �v���p�e�B
    /// </summary>
    public GridOwnerType CurrentGridOwnerType { get => currentGridOwnerType; set => currentGridOwnerType = value; }

    public ReactiveProperty<int> WinCount = new ReactiveProperty<int>(); 
    public ReactiveProperty<string> ResultMessage = new ReactiveProperty<string>();


    /// <summary>
    /// Result_Model �̏����ݒ�
    /// </summary>
    public void SetUpResultModel(GridOwnerType gridOwnerType) {
        WinCount.Value = 0;
        CurrentGridOwnerType = gridOwnerType;
    }

    /// <summary>
    /// ���s�\���̏�����
    /// </summary>
    public void InitResultMessage() {
        ResultMessage.Value = string.Empty;
    }

    /// <summary>
    /// ���ʔ��\
    /// </summary>
    /// <param name="winner"></param>
    public void ShowResult(GridOwnerType winner) {

        if (winner == CurrentGridOwnerType) {
            ResultMessage.Value = "Win!";
            WinCount.Value++;
        } else if (winner == GridOwnerType.Draw) {
            ResultMessage.Value = "Draw";
        } else if (winner == GridOwnerType.None){
            ResultMessage.Value = string.Empty;
        } else {
            ResultMessage.Value = "Lose...";
        }
    }
}