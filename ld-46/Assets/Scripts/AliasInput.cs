using System.Collections;
using Doozy.Engine;
using Doozy.Engine.UI;
using TMPro;
using UnityEngine;

public class AliasInput : MonoBehaviour
{
    public TMP_InputField inputField;
    public UIButton ConfirmAliasButton;

    private string alias;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        if (GameManager.Instance.gameFinished)
        {
            yield return null;
            GameEventMessage.SendEvent("GameFinished");
            GameManager.Instance.gameFinished = false;
        }
        else
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
    }

    public void ConfirmAlias()
    {
        PlayerPrefs.SetString("player_alias", inputField.text);
        Debug.Log("Setting alias to " + inputField.text);
        PlayerPrefs.Save();
    }

    public void CheckAlias()
    {
        if (inputField.text.Length <= 0 || inputField.text.Length > 12)
        {
            ConfirmAliasButton.DisableButton();
        }
        else
        {
            ConfirmAliasButton.EnableButton();
        }
    }
}
