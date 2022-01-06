using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridButton : MonoBehaviour {

    //public int[,] gridNo = new int[1, 1];
    public Button btnGrid;
    public Text txtGridOwnerIcon;
    public GridOwnerType currentGridOwnerType;

    public int gridNo;
    private GameManager gameManager;

    public void SetUpPiecePlace(int no, GameManager gameManager) {
        gridNo = no;
        this.gameManager = gameManager;
        btnGrid.onClick.AddListener(() => gameManager.OnClickGrid(gridNo));
        Initialized();
    }

    public GridOwnerType GetCurrentGridOwnerType() {
        return currentGridOwnerType;
    }

    /// <summary>
    /// Gird ‚Ì‰Šú‰»
    /// </summary>
    public void Initialized() {
        currentGridOwnerType = GridOwnerType.None;
        txtGridOwnerIcon.text = "";
    }
}
