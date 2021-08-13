using System.Collections;
using TMPro;
using UnityEngine;

public class StatusText : MonoBehaviour
{
    private TMP_Text _statusText;

    // Start is called before the first frame update
    private void Awake()
    {
        _statusText = GetComponent<TMP_Text>();
        GetComponent<CanvasRenderer>().SetAlpha(0);
    }

    public IEnumerator ChangeStatus(string displayText, bool gameOver)
    {
        _statusText.text = displayText;
        _statusText.CrossFadeAlpha(1, 1.5f, false);
        yield return new WaitForSeconds(1.50f);
        _statusText.CrossFadeAlpha(0, 1.5f, false);
        yield return new WaitForSeconds(1.50f);
        gameObject.SetActive(false);
        
        if (gameOver)
        {
            EventBroker.CallActivateRestartButton();  
        }
    }
}

