using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class GridButton : MonoBehaviour {

    [SerializeField]
    private Button btnGrid;

    [SerializeField]
    private Text txtGridOwnerIcon;

    [SerializeField]
    private GridOwnerType currentGridOwnerType;

    public GridOwnerType CurrentGridOwnerType
    {
        get => currentGridOwnerType;
        set => currentGridOwnerType = value;
    }

    private int gridNo;

    /// <summary>
    /// GridButton の初期設定
    /// </summary>
    /// <param name="no"></param>
    /// <param name="gameManager"></param>
    public void SetUpGrid(int no, GameManager gameManager) {
        gridNo = no;
        
        btnGrid.onClick.AddListener(() => gameManager.OnClickGrid(gridNo));
        UpdateGridData(GridOwnerType.None, string.Empty);
    }

    /// <summary>
    /// Gird の情報更新(オーナーシンボル(○×)のセット、および初期化に利用する)
    /// </summary>
    public void UpdateGridData(GridOwnerType newGridOwnerType, string ownerSymbol) {
        currentGridOwnerType = newGridOwnerType;
        txtGridOwnerIcon.text = ownerSymbol;
    }
}