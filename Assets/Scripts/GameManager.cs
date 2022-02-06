using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Model
/// </summary>
public class GameManager : MonoBehaviour {

    [SerializeField]
    private Transform gridSetTran;

    [SerializeField]
    private GridController gridPrefab;

    [SerializeField]
    private GridController[] grids = new GridController[9];

    private int putCount;

    [SerializeField]
    private bool isGameUp;


    // mi

    [SerializeField]
    private Text txtPlayerResult;

    [SerializeField]
    private Text txtOpponentResult;

    [SerializeField]
    private Button btnRestart;

    [SerializeField]
    private int winCount;

    [SerializeField]
    private int loseCount;

    [SerializeField]
    private Text txtWinCount;

    [SerializeField]
    private Text txtLoseCount;

    [SerializeField]
    private Text txtInfo;

    private int gridCount = 9;


    private void Start() {
        SetUpGamaManager();
    }

    /// <summary>
    /// ゲームマネージャーの初期設定
    /// </summary>
    public void SetUpGamaManager() {

        // ゲームの初期設定
        InitialSettings();

        //  ゲームに利用する情報の初期化
        ResetGameParameters();
    }

    /// <summary>
    /// ゲームの初期設定
    /// </summary>
    public void InitialSettings() {
        grids = new GridController[gridCount];

        // ボタンの文字の初期設定
        for (int i = 0; i < grids.Length; i++) {
            grids[i] = Instantiate(gridPrefab, gridSetTran, false);
            grids[i].SetUpGrid(i, this);
            //Debug.Log(i);
        }

        btnRestart.onClick.AddListener(OnClickRestart);

        winCount = 0;
        loseCount = 0;
        txtWinCount.text = winCount.ToString();
        txtLoseCount.text = loseCount.ToString();
    }

    /// <summary>
    /// ゲームに利用する情報の初期化
    /// </summary>
    public void ResetGameParameters() {

        isGameUp = false;
        btnRestart.interactable = false;

        txtPlayerResult.text = "";
        txtOpponentResult.text = "";

        putCount = 0;
    }

    /// <summary>
    /// Player がボタンをクリックした際の処理
    /// </summary>
    public void OnClickGrid(int no) {

        Debug.Log($"クリック実行 : Grid の通し番号 : { no }");

        if (isGameUp) {
            return;
        }

        //Debug.Log(no);

        // オーナーシンボル(プレイヤーは○印)が置けるか確認
        if (grids[no].CurrentGridOwnerType == GridOwnerType.None) {

            putCount++;

            //Debug.Log(no + " 番目の Grid に〇印をつける");


            // インフォ表示をリセット
            txtInfo.text = "";

            // ○をセット
            SetOwnerTypeOnGrid(grids[no], GridOwnerType.Player);

            // 勝負付かず引き分け
            if (putCount >= 5 && !isGameUp) {
                ShowResult(GridOwnerType.Draw);
                return;
            }

            // 敵の順番
            PutOpponentGrid();
        } else {
            Debug.Log("そこには配置出来ません。");
            //txtInfo.text = "そこには配置出来ません。";
        }
    }

    /// <summary>
    /// Grid にオーナーシンボル(○×)をセット
    /// </summary>
    /// <param name="targetGrid"></param>
    /// <param name="setOwnerType"></param>
    private void SetOwnerTypeOnGrid(GridController targetGrid, GridOwnerType setOwnerType) {

        targetGrid.UpdateGridData(setOwnerType, setOwnerType == GridOwnerType.Player ? "〇" : "×");

        // 結果を判定
        JudgeWinner();
    }

    /// <summary>
    /// 敵の順番(×が置けるか確認してから置く)
    /// </summary>
    private void PutOpponentGrid() {

        while (!isGameUp) {
            // ランダムな Grid を選択
            int randomPieceIndex = Random.Range(0, grids.Length);

            // その Grid にオーナーシンボルがなければ×を設置
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

        Debug.Log("勝者 : " + winner);

        isGameUp = true;
        btnRestart.interactable = true;

        if (winner == GridOwnerType.Player) {
            txtPlayerResult.text = "Win!";
            txtOpponentResult.text = "Lose...";
            winCount++;
            txtWinCount.text = winCount.ToString();
        } else if (winner == GridOwnerType.Opponent) {
            txtPlayerResult.text = "Lose...";
            txtOpponentResult.text = "Win!";
            loseCount++;
            txtLoseCount.text = loseCount.ToString();
        } else {
            txtPlayerResult.text = "Draw";
            txtOpponentResult.text = "Draw";
        }
    }

    /// <summary>
    /// 次のゲームを再開する準備
    /// </summary>
    public void OnClickRestart() {
        // 各種設定を初期化
        for (int i = 0; i < grids.Length; i++) {
            grids[i].UpdateGridData(GridOwnerType.None, string.Empty);
        }

        // ゲームに利用する情報の初期化
        ResetGameParameters();
    }
}
