using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class Result_View : MonoBehaviour
{
    [SerializeField]
    private Text txtResultMessage;

    [SerializeField]
    private Text txtWinCount;

    [SerializeField]
    private GridOwnerType gridOwnerType;
    public GridOwnerType GridOwnerType
    {
        get => gridOwnerType;
        set => gridOwnerType = value;
    }

    //private GameManager gameManager;


    //public void SetUpResultView(GameManager gameManager) {
    //    this.gameManager = gameManager;

    //    // GameManager の RectiveDictionary を購読し、勝利数のカウントに合わせて画面を更新
    //    gameManager.WinCount.ObserveReplace()
    //        .Where(x => x.Key == gridOwnerType)
    //        .Subscribe((DictionaryReplaceEvent<GridOwnerType, int> result) => UpdateDisplayWincount(result.NewValue));
    //    Debug.Log("購読開始 : " + gridOwnerType.ToString());

    //    // GameManager の RectiveProperty を購読し、勝敗結果に合わせて画面を更新
    //    if (gridOwnerType == GridOwnerType.Player) {
    //        gameManager.PlayerResultMessage.Subscribe(x => UpdateDisplayResultGame(x));
    //    } else {
    //        gameManager.OpponentResultMessage.Subscribe(x => UpdateDisplayResultGame(x));
    //    }
    //}


    public void UpdateDisplayWincount(int newValue) {
        txtWinCount.text = newValue.ToString();
    }


    public void UpdateDisplayResultGame(string result) {
        txtResultMessage.text = result;
    }
}
