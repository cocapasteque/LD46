using System.Collections;
using Doozy.Engine;
using TMPro;
using UnityEngine;

public class AliasInput : MonoBehaviour
{
    public TMP_InputField inputField;

    private string alias;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(2);
        alias = PlayerPrefs.GetString("player_alias", string.Empty);
        Debug.Log(alias);
        if (!string.IsNullOrEmpty(alias))
        {
            GameEventMessage.SendEvent("AliasExists");
        }
        else
        {
            GameEventMessage.SendEvent("AliasNotExists");
        }
    }

    public void ConfirmAlias()
    {
        PlayerPrefs.SetString("player_alias", inputField.text);
        Debug.Log("Setting alias to " + inputField.text);
        PlayerPrefs.Save();
    }
}
