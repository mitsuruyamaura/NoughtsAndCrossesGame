using UnityEngine;
using UnityEngine.UI;

public class Grid_View : MonoBehaviour
{
    [SerializeField]
    private Text txtGridOwnerSymbol;

    [SerializeField]
    private Button btnGrid;

    /// <summary>
    /// �I�[�i�[�V���{��(���~)�̕\���X�V
    /// </summary>
    /// <param name="ownerSymbol"></param>
    public void UpdateGridOwnerSymbol(string ownerSymbol) {
        txtGridOwnerSymbol.text = ownerSymbol;
    }

    /// <summary>
    /// �{�^���̎擾
    /// </summary>
    /// <returns></returns>
    public Button GetGridButton() {
        return btnGrid;
    }
}
