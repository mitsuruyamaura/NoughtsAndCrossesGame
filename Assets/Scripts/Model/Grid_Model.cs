using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class Grid_Model : MonoBehaviour
{
    public ReactiveProperty<GridOwnerType> CurrentGridOwnerType;

    private int gridNo;

    /// <summary>
    /// �v���p�e�B
    /// </summary>
    public int GridNo { get => gridNo; set => gridNo = value; }

    /// <summary>
    /// �����ݒ�
    /// </summary>
    /// <param name="no"></param>
    public void SetUpGridModel(int no) {
        gridNo = no;
    }
}