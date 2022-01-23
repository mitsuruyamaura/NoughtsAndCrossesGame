using UnityEngine;
using UnityEngine.UI;

public class Result_View : MonoBehaviour
{
    [SerializeField]
    private Text txtResultMessage;

    [SerializeField]
    private Text txtWinCount;


    /// <summary>
    /// �������\���̍X�V
    /// </summary>
    /// <param name="newValue"></param>
    public void UpdateDisplayWincount(int newValue) {
        txtWinCount.text = newValue.ToString();
    }

    /// <summary>
    /// ���s�\���̍X�V
    /// </summary>
    /// <param name="result"></param>
    public void UpdateDisplayResultGame(string result) {
        txtResultMessage.text = result;
    }
}
