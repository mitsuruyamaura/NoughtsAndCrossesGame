using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// MVP パターンでの実装
/// </summary>
public class MainGame_Model : MonoBehaviour
{
    [SerializeField]
    private Transform gridSetTran;

    [SerializeField]
    private Grid_Model gridModelPrefab;

    [HideInInspector]
    public List<Grid_Model> gridModelList = new List<Grid_Model>();

    //[HideInInspector]
    //public ReactiveProperty<string> PlayerResultMessage = new ReactiveProperty<string>();

    //[HideInInspector]
    //public ReactiveProperty<string> OpponentResultMessage = new ReactiveProperty<string>();

    [HideInInspector]
    public ReactiveProperty<string> InfoMessage = new ReactiveProperty<string>();

    [HideInInspector]
    public ReactiveProperty<bool> IsGameUp = new ReactiveProperty<bool>();

    private int putCount;
    private int gridCount = 9;

    public GridOwnerType winner;

    /// <summary>
    /// ゲームの初期設定
    /// </summary>
    public Grid_View[] InitialSettings() {

        List<Grid_View> gridViewList = new List<Grid_View>();

        for (int i = 0; i < gridCount; i++) {
            Grid_Model gridModel = Instantiate(gridModelPrefab, gridSetTran, false);
            gridModel.SetUpGridModel(i);
            gridModelList.Add(gridModel);
            gridViewList.Add(gridModel.GetComponent<Grid_View>());
        }

        //WinCount.Add(GridOwnerType.Player, 0);
        //WinCount.Add(GridOwnerType.Opponent, 0);

        return gridViewList.ToArray();
    }

    /// <summary>
    /// 勝利数の初期化
    /// </summary>
    public void InitWinCount() {
        //WinCount[GridOwnerType.Player] = 0;
        //WinCount[GridOwnerType.Opponent] = 0;
    }

    /// <summary>
    /// ゲームに利用する情報の初期化
    /// </summary>
    public void ResetGameParameters() {

        winner = GridOwnerType.None;
        IsGameUp.Value = false;

        //PlayerResultMessage.Value = string.Empty;
        //OpponentResultMessage.Value = string.Empty;

        putCount = 0;
    }

    /// <summary>
    /// Player がボタンをクリックした際の処理
    /// </summary>
    public void OnClickGrid(int no) {
        if (IsGameUp.Value) {
            return;
        }

        //Debug.Log(no);

        // ○が置けるか確認
        if (gridModelList[no].CurrentGridOwnerType.Value == GridOwnerType.None) {
            InfoMessage.Value = string.Empty;

            // ○をセット
            gridModelList[no].CurrentGridOwnerType.Value = GridOwnerType.Player;

            // 結果を判定
            JudgeWinner();

            putCount++;

            // 勝負付かず引き分け
            if (putCount >= 5 && !IsGameUp.Value) {
                ShowResult(GridOwnerType.Draw);
                return;
            }

            // 敵の順番
            PutOpponentGrid();
        } else {
            InfoMessage.Value = "そこには配置出来ません。";
        }
    }

    /// <summary>
    /// 敵の順番(×が置けるか確認してから置く)
    /// </summary>
    private void PutOpponentGrid() {

        while (!IsGameUp.Value) {
            int randomPieceIndex = Random.Range(0, gridModelList.Count);
            if (gridModelList[randomPieceIndex].CurrentGridOwnerType.Value == GridOwnerType.None) {
                gridModelList[randomPieceIndex].CurrentGridOwnerType.Value = GridOwnerType.Opponent;
                // 結果を判定
                JudgeWinner();
                break;
            }
        }
    }

    /// <summary>
    /// 勝者がいるか判定
    /// </summary>
    private void JudgeWinner() {

        GridOwnerType winner = GridOwnerType.None;

        // 横列確認
        for (int i = 0; i < 2; i++) {

            int gridOwnerTypeNo = i + 1;

            // 0,1,2 || 3,4,5 || 6,7,8
            for (int x = 0; x < 3; x++) {
                if (gridModelList[x * 3].CurrentGridOwnerType.Value == (GridOwnerType)gridOwnerTypeNo
                && gridModelList[x * 3 + 1].CurrentGridOwnerType.Value == (GridOwnerType)gridOwnerTypeNo
                && gridModelList[x * 3 + 2].CurrentGridOwnerType.Value == (GridOwnerType)gridOwnerTypeNo) {
                    winner = (GridOwnerType)gridOwnerTypeNo;
                    //Debug.Log((PieceType)pieceTypeNo);
                    break;
                }
            }

            // 勝者が確定している場合
            if (winner != GridOwnerType.None) {
                // 結果発表
                ShowResult(winner);
                return;
            }
        }

        // 縦列確認
        for (int i = 0; i < 2; i++) {

            int gridOwnerTypeNo = i + 1;

            // 0,3,6 || 1,4,7 || 2,5,8
            for (int x = 0; x < 3; x++) {
                if (gridModelList[x].CurrentGridOwnerType.Value == (GridOwnerType)gridOwnerTypeNo
                && gridModelList[x + 3].CurrentGridOwnerType.Value == (GridOwnerType)gridOwnerTypeNo
                && gridModelList[x + 6].CurrentGridOwnerType.Value == (GridOwnerType)gridOwnerTypeNo) {
                    winner = (GridOwnerType)gridOwnerTypeNo;
                    //Debug.Log((PieceType)pieceTypeNo);
                    break;
                }
            }

            // 勝者が確定している場合
            if (winner != GridOwnerType.None) {
                // 結果発表
                ShowResult(winner);
                return;
            }
        }

        // 斜め列確認
        for (int i = 0; i < 2; i++) {

            int gridOwnerTypeNo = i + 1;

            // 2, 4, 6 ||  0, 4, 8
            for (int x = -1; x < 1; x++) {
                if (gridModelList[0 - x * 2].CurrentGridOwnerType.Value == (GridOwnerType)gridOwnerTypeNo
                && gridModelList[4].CurrentGridOwnerType.Value == (GridOwnerType)gridOwnerTypeNo
                && gridModelList[8 + x * 2].CurrentGridOwnerType.Value == (GridOwnerType)gridOwnerTypeNo) {
                    winner = (GridOwnerType)gridOwnerTypeNo;
                    //Debug.Log((PieceType)pieceTypeNo);
                    break;
                }
            }

            // 勝者が確定している場合
            if (winner != GridOwnerType.None) {
                // 結果発表
                ShowResult(winner);
                return;
            }
        }
    }

    /// <summary>
    /// 結果発表
    /// </summary>
    /// <param name="winner"></param>
    private void ShowResult(GridOwnerType winner) {

        this.winner = winner;
        IsGameUp.Value = true;

        //if (winner == GridOwnerType.Player) {
        //    PlayerResultMessage.Value = "Win!";
        //    OpponentResultMessage.Value = "Lose...";
        //    WinCount[GridOwnerType.Player]++;
        //} else if (winner == GridOwnerType.Opponent) {
        //    PlayerResultMessage.Value = "Lose...";
        //    OpponentResultMessage.Value = "Win!";
        //    WinCount[GridOwnerType.Opponent]++;
        //} else {
        //    PlayerResultMessage.Value = "Draw";
        //    OpponentResultMessage.Value = "Draw";
        //}
    }

    /// <summary>
    /// 次のゲームを再開する準備
    /// </summary>
    public void Restart() {

        for (int i = 0; i < gridModelList.Count; i++) {
            gridModelList[i].CurrentGridOwnerType.Value = GridOwnerType.None;
        }

        // ゲームに利用する情報の初期化
        ResetGameParameters();
    }
}
