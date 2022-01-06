using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// �Q�l
// https://yuzame-gohan.com/marubatsu/

public class GameManager : MonoBehaviour {

    //public int[,] piecePlaces = new int[3, 3];

    [SerializeField]
    private Text txtPlayerResult;

    [SerializeField]
    private Text txtOpponentResult;

    [SerializeField]
    private GridButton[] grids = new GridButton[9];
    
    [SerializeField]
    private Transform gridSetTran;

    [SerializeField]
    private GridButton gridPrefab;

    [SerializeField]
    private bool isGameUp;
    
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

    private int putCount;


    void Start() {

        //// piecePlaces �����l�ݒ�
        //for (int i = 0; i < 3; i++) {
        //    for (int j = 0; 0 < j; j++) {
        //        piecePlaces[i, j] = (int)PieceType.None;
        //    }
        //}

        // �Q�[���̏����ݒ�
        InitialSettings();

        //  �Q�[���ɗ��p������̏�����
        ResetGameParameters();
    }

    /// <summary>
    /// �Q�[���̏����ݒ�
    /// </summary>
    private void InitialSettings() {
        grids = new GridButton[9];

        // �{�^���̕����̏����ݒ�
        for (int i = 0; i < grids.Length; i++) {
            grids[i] = Instantiate(gridPrefab, gridSetTran, false);
            grids[i].SetUpGrid(i, this);
            Debug.Log(i);
        }

        btnRestart.onClick.AddListener(OnClickRestart);

        winCount = 0;
        loseCount = 0;
        txtWinCount.text = winCount.ToString();
        txtLoseCount.text = loseCount.ToString();
    }

    /// <summary>
    /// �Q�[���ɗ��p������̏�����
    /// </summary>
    private void ResetGameParameters() {
        isGameUp = false;
        btnRestart.interactable = false;

        txtPlayerResult.text = "";
        txtOpponentResult.text = "";
        putCount = 0;
    }

    /// <summary>
    /// Player ���{�^�����N���b�N�����ۂ̏���
    /// </summary>
    public void OnClickGrid(int no) {
        if (isGameUp) {
            return;
        }

        Debug.Log(no);

        // ���~���u���邩�m�F
        if (grids[no].CurrentGridOwnerType == GridOwnerType.None) {

            txtInfo.text = "";

            // ���~���Z�b�g
            SetOwnerTypeOnGrid(grids[no], GridOwnerType.Player);

            putCount++;

            // �����t������������
            if (putCount >= 5 && !isGameUp) {
                ShowResult(GridOwnerType.Draw);
                return;
            }

            // �G�̏���
            PutOpponentGrid();
        } else {
            txtInfo.text = "�����ɂ͔z�u�o���܂���B";
        }
    }

    /// <summary>
    /// Grid �ɃI�[�i�[(���~)���Z�b�g
    /// </summary>
    /// <param name="targetGrid"></param>
    /// <param name="setOwnerType"></param>
    private void SetOwnerTypeOnGrid(GridButton targetGrid, GridOwnerType setOwnerType) {

        targetGrid.UpdateGridData(setOwnerType, setOwnerType == GridOwnerType.Player ? "�Z" : "�~");

        // ���ʂ𔻒�
        JudgeWinner();
    }

    /// <summary>
    /// �G�̏���
    /// </summary>
    private void PutOpponentGrid() {
        
        while (!isGameUp) {
            int randomPieceIndex = Random.Range(0, grids.Length);
            if (grids[randomPieceIndex].CurrentGridOwnerType == GridOwnerType.None) {
                SetOwnerTypeOnGrid(grids[randomPieceIndex], GridOwnerType.Opponent);
                break;
            }
        }
    }

    /// <summary>
    /// ���҂����邩����
    /// </summary>
    private void JudgeWinner() {

        GridOwnerType winner = GridOwnerType.None;

        // ����m�F
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

            // ���҂��m�肵�Ă���ꍇ
            if (winner != GridOwnerType.None) {
                // ���ʔ��\
                ShowResult(winner);
                return;
            }
        }

        // �c��m�F
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

            // ���҂��m�肵�Ă���ꍇ
            if (winner != GridOwnerType.None) {
                // ���ʔ��\
                ShowResult(winner);
                return;
            }
        }

        // �΂ߗ�m�F
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

            // ���҂��m�肵�Ă���ꍇ
            if (winner != GridOwnerType.None) {
                // ���ʔ��\
                ShowResult(winner);
                return;
            }
        }
    }

    /// <summary>
    /// ���ʔ��\
    /// </summary>
    /// <param name="winner"></param>
    private void ShowResult(GridOwnerType winner) {
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
    /// ���̃Q�[�����ĊJ���鏀��
    /// </summary>
    private void OnClickRestart() {
        // �e��ݒ��������
        for (int i = 0; i < grids.Length; i++) {
            grids[i].UpdateGridData(GridOwnerType.None, string.Empty);
        }

        // �Q�[���ɗ��p������̏�����
        ResetGameParameters();
    }
}