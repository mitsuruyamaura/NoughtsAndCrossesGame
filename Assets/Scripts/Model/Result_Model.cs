using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class Result_Model : MonoBehaviour
{
    //public ReactiveDictionary<GridOwnerType, int> WinInfoData = new ReactiveDictionary<GridOwnerType, int>();

    public GridOwnerType currentGridOwnerType;
    public ReactiveProperty<int> WinCount = new ReactiveProperty<int>(); 

    public ReactiveProperty<string> ResultMessage = new ReactiveProperty<string>();


    /// <summary>
    /// 勝利数の初期化
    /// </summary>
    public void InitWinCount() {
        WinCount.Value = 0;
    }

    /// <summary>
    /// 勝敗表示の初期化
    /// </summary>
    public void InitResultMessage() {
        ResultMessage.Value = string.Empty;
    }

    /// <summary>
    /// 結果発表
    /// </summary>
    /// <param name="winner"></param>
    public void ShowResult(GridOwnerType winner) {

        if (winner == currentGridOwnerType) {
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