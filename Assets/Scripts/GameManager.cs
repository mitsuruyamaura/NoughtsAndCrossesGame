using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
using UniRx;

// 参考
// https://yuzame-gohan.com/marubatsu/

// MVP + UniRX
// https://takap-tech.com/entry/2021/11/27/195909


/// <summary>
/// Model
/// </summary>
public class GameManager : MonoBehaviour {

    //public int[,] piecePlaces = new int[3, 3];

    //[SerializeField]
    //private Text txtPlayerResult;

    //[SerializeField]
    //private Text txtOpponentResult;

    [SerializeField]
    private GridButton[] grids = new GridButton[9];
    
    [SerializeField]
    private Transform gridSetTran;

    [SerializeField]
    private GridButton gridPrefab;

    //[SerializeField]
    //private bool isGameUp;
    
    //[SerializeField]
    //private Button btnRestart;

    //[SerializeField]
    //private int winCount;
    
    //[SerializeField]
    //private int loseCount;

    //[SerializeField]
    //private Text txtWinCount;
    
    //[SerializeField]
    //private Text txtLoseCount;

    //[SerializeField]
    //private Text txtInfo;

    private int putCount;

    // Model
    public ReactiveDictionary<GridOwnerType, int> WinCount = new ReactiveDictionary<GridOwnerType, int>();

    public ReactiveProperty<string> PlayerResultMessage = new ReactiveProperty<string>();
    public ReactiveProperty<string> OpponentResultMessage = new ReactiveProperty<string>();
    public ReactiveProperty<string> InfoMessage = new ReactiveProperty<string>();
    public ReactiveProperty<bool> IsGameUp = new ReactiveProperty<bool>();

    [SerializeField]
    private Result_View playerResultView;

    [SerializeField]
    private Result_View opponentResultView;


    public void SetUpModel() {

        //// piecePlaces 初期値設定
        //for (int i = 0; i < 3; i++) {
        //    for (int j = 0; 0 < j; j++) {
        //        piecePlaces[i, j] = (int)PieceType.None;
        //    }
        //}

        // ゲームの初期設定
        InitialSettings();

        //  ゲームに利用する情報の初期化
        ResetGameParameters();
    }

    /// <summary>
    /// ゲームの初期設定
    /// </summary>
    private void InitialSettings() {
        grids = new GridButton[9];

        // ボタンの文字の初期設定
        for (int i = 0; i < grids.Length; i++) {
            grids[i] = Instantiate(gridPrefab, gridSetTran, false);
            grids[i].SetUpGrid(i, this);
            Debug.Log(i);
        }

        //btnRestart.onClick.AddListener(OnClickRestart);

        //winCount = 0;
        //loseCount = 0;
        //txtWinCount.text = winCount.ToString();
        //txtLoseCount.text = loseCount.ToString();

        WinCount.Add(GridOwnerType.Player, 0);
        WinCount.Add(GridOwnerType.Opponent, 0);

        playerResultView.SetUpResultView(this);
        opponentResultView.SetUpResultView(this);

        WinCount[GridOwnerType.Player] = 0;
        WinCount[GridOwnerType.Opponent] = 0;
    }

    /// <summary>
    /// ゲームに利用する情報の初期化
    /// </summary>
    private void ResetGameParameters() {
        //isGameUp = false;
        //infoModel.SwitchActivateButton(false);

        //btnRestart.interactable = false;

        IsGameUp.Value = false;

        PlayerResultMessage.Value = string.Empty;
        OpponentResultMessage.Value = string.Empty;

        //txtPlayerResult.text = "";
        //txtOpponentResult.text = "";
        putCount = 0;
    }

    /// <summary>
    /// Player がボタンをクリックした際の処理
    /// </summary>
    public void OnClickGrid(int no) {
        if (IsGameUp.Value) {
            return;
        }

        Debug.Log(no);

        // ○×が置けるか確認
        if (grids[no].CurrentGridOwnerType == GridOwnerType.None) {

            //txtInfo.text = "";

            InfoMessage.Value = string.Empty;

            // ○×をセット
            SetOwnerTypeOnGrid(grids[no], GridOwnerType.Player);

            putCount++;

            // 勝負付かず引き分け
            if (putCount >= 5 && !IsGameUp.Value) {
                ShowResult(GridOwnerType.Draw);
                return;
            }

            // 敵の順番
            PutOpponentGrid();
        } else {
            //txtInfo.text = "そこには配置出来ません。";

            InfoMessage.Value = "そこには配置出来ません。";
        }
    }

    /// <summary>
    /// Grid にオーナー(○×)をセット
    /// </summary>
    /// <param name="targetGrid"></param>
    /// <param name="setOwnerType"></param>
    private void SetOwnerTypeOnGrid(GridButton targetGrid, GridOwnerType setOwnerType) {

        targetGrid.UpdateGridData(setOwnerType, setOwnerType == GridOwnerType.Player ? "〇" : "×");

        // 結果を判定
        JudgeWinner();
    }

    /// <summary>
    /// 敵の順番
    /// </summary>
    private void PutOpponentGrid() {
        
        while (!IsGameUp.Value) {
            int randomPieceIndex = Random.Range(0, grids.Length);
            if (grids[randomPieceIndex].CurrentGridOwnerType == GridOwnerType.None) {
                SetOwnerTypeOnGrid(grids[randomPieceIndex], GridOwnerType.Opponent);
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
                if (grids[x * 3].CurrentGridOwnerType == (GridOwnerType)gridOwnerTypeNo
                && grids[x * 3 + 1].CurrentGridOwnerType == (GridOwnerType)gridOwnerTypeNo
                && grids[x * 3 + 2].CurrentGridOwnerType == (GridOwnerType)gridOwnerTypeNo) {
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
                if (grids[x].CurrentGridOwnerType == (GridOwnerType)gridOwnerTypeNo
                && grids[x + 3].CurrentGridOwnerType == (GridOwnerType)gridOwnerTypeNo
                && grids[x + 6].CurrentGridOwnerType == (GridOwnerType)gridOwnerTypeNo) {
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
                if (grids[0 - x * 2].CurrentGridOwnerType == (GridOwnerType)gridOwnerTypeNo
                && grids[4].CurrentGridOwnerType == (GridOwnerType)gridOwnerTypeNo
                && grids[8 + x * 2].CurrentGridOwnerType == (GridOwnerType)gridOwnerTypeNo) {
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
        //isGameUp = true;
        //infoModel.SwitchActivateButton(true); 

        //btnRestart.interactable = true;

        IsGameUp.Value = true;

        if (winner == GridOwnerType.Player) {
            //txtPlayerResult.text = "Win!";
            //txtOpponentResult.text = "Lose...";
            //winCount++;
            //txtWinCount.text = winCount.ToString();

            PlayerResultMessage.Value = "Win!";
            OpponentResultMessage.Value = "Lose...";
            WinCount[GridOwnerType.Player]++;
        } else if (winner == GridOwnerType.Opponent) {
            //txtPlayerResult.text = "Lose...";
            //txtOpponentResult.text = "Win!";
            //loseCount++;
            //txtLoseCount.text = loseCount.ToString();

            PlayerResultMessage.Value = "Lose...";
            OpponentResultMessage.Value = "Win!";
            WinCount[GridOwnerType.Opponent]++;

        } else {
            //txtPlayerResult.text = "Draw";
            //txtOpponentResult.text = "Draw";

            PlayerResultMessage.Value = "Draw";
            OpponentResultMessage.Value = "Draw";
        }
    }

    /// <summary>
    /// 次のゲームを再開する準備
    /// </summary>
    public void Restart() {
        // 各種設定を初期化
        for (int i = 0; i < grids.Length; i++) {
            grids[i].UpdateGridData(GridOwnerType.None, string.Empty);
        }

        // ゲームに利用する情報の初期化
        ResetGameParameters();
    }
}
