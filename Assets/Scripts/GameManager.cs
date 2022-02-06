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
    /// �Q�[���}�l�[�W���[�̏����ݒ�
    /// </summary>
    public void SetUpGamaManager() {

        // �Q�[���̏����ݒ�
        InitialSettings();

        //  �Q�[���ɗ��p������̏�����
        ResetGameParameters();
    }

    /// <summary>
    /// �Q�[���̏����ݒ�
    /// </summary>
    public void InitialSettings() {
        grids = new GridController[gridCount];

        // �{�^���̕����̏����ݒ�
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
    /// �Q�[���ɗ��p������̏�����
    /// </summary>
    public void ResetGameParameters() {

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

        Debug.Log($"�N���b�N���s : Grid �̒ʂ��ԍ� : { no }");

        if (isGameUp) {
            return;
        }

        //Debug.Log(no);

        // �I�[�i�[�V���{��(�v���C���[�́���)���u���邩�m�F
        if (grids[no].CurrentGridOwnerType == GridOwnerType.None) {

            putCount++;

            //Debug.Log(no + " �Ԗڂ� Grid �ɁZ�������");


            // �C���t�H�\�������Z�b�g
            txtInfo.text = "";

            // �����Z�b�g
            SetOwnerTypeOnGrid(grids[no], GridOwnerType.Player);

            // �����t������������
            if (putCount >= 5 && !isGameUp) {
                ShowResult(GridOwnerType.Draw);
                return;
            }

            // �G�̏���
            PutOpponentGrid();
        } else {
            Debug.Log("�����ɂ͔z�u�o���܂���B");
            //txtInfo.text = "�����ɂ͔z�u�o���܂���B";
        }
    }

    /// <summary>
    /// Grid �ɃI�[�i�[�V���{��(���~)���Z�b�g
    /// </summary>
    /// <param name="targetGrid"></param>
    /// <param name="setOwnerType"></param>
    private void SetOwnerTypeOnGrid(GridController targetGrid, GridOwnerType setOwnerType) {

        targetGrid.UpdateGridData(setOwnerType, setOwnerType == GridOwnerType.Player ? "�Z" : "�~");

        // ���ʂ𔻒�
        JudgeWinner();
    }

    /// <summary>
    /// �G�̏���(�~���u���邩�m�F���Ă���u��)
    /// </summary>
    private void PutOpponentGrid() {

        while (!isGameUp) {
            // �����_���� Grid ��I��
            int randomPieceIndex = Random.Range(0, grids.Length);

            // ���� Grid �ɃI�[�i�[�V���{�����Ȃ���΁~��ݒu
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

        Debug.Log("���� : " + winner);

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
    public void OnClickRestart() {
        // �e��ݒ��������
        for (int i = 0; i < grids.Length; i++) {
            grids[i].UpdateGridData(GridOwnerType.None, string.Empty);
        }

        // �Q�[���ɗ��p������̏�����
        ResetGameParameters();
    }
}
