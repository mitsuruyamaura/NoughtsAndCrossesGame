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

    //    // GameManager �� RectiveDictionary ���w�ǂ��A�������̃J�E���g�ɍ��킹�ĉ�ʂ��X�V
    //    gameManager.WinCount.ObserveReplace()
    //        .Where(x => x.Key == gridOwnerType)
    //        .Subscribe((DictionaryReplaceEvent<GridOwnerType, int> result) => UpdateDisplayWincount(result.NewValue));
    //    Debug.Log("�w�ǊJ�n : " + gridOwnerType.ToString());

    //    // GameManager �� RectiveProperty ���w�ǂ��A���s���ʂɍ��킹�ĉ�ʂ��X�V
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
