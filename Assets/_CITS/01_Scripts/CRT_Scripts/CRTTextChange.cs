using TMPro;
using UnityEngine;
using Sirenix.OdinInspector;

public class CRTTextChange : MonoBehaviour
{
    public float normalFontSize;

    private void Start() {
        normalFontSize = gameObject.GetComponent<TextMeshPro>().fontSize;
    }

    [Button ("Change Text to Underline")]
    public void ChangeTextToUnderline()
    {
        gameObject.GetComponent<TextMeshPro>().fontStyle = FontStyles.Underline;
    }

    [Button ("Change Text to Normal")]
    public void ChangeTextToNormal()
    {
        gameObject.GetComponent<TextMeshPro>().fontSize = normalFontSize;
        gameObject.GetComponent<TextMeshPro>().fontStyle = FontStyles.Normal;
    }

    [Button ("Change Text to Strikethrough")]
    public void ChangeTextToStrikethrough()
    {
        gameObject.GetComponent<TextMeshPro>().fontStyle = FontStyles.Strikethrough;
    }

}
