using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FinalMenuMessage : MonoBehaviour
{
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private FinalMenu finalMenu;
    [SerializeField] private Button playAgain;
    private float _shortFadingTime = 1.5f;
    private float _longFadingTime = 2f;


    private void Awake()
    {
        statusText.gameObject.SetActive(false);
    }
    
    private void Start()
    {
        StartCoroutine(CallTheEndGame());
    }
    
    //Todo: find a better way to implement this coroutine
    private IEnumerator CallTheEndGame()
    {
        statusText.gameObject.SetActive(true);
        statusText.text = "The End";
        statusText.CrossFadeAlpha(1, 1.5f, false);
        yield return new WaitForSeconds(_shortFadingTime);
        statusText.CrossFadeAlpha(0, 1.5f, false);
        yield return new WaitForSeconds(_shortFadingTime);
        statusText.gameObject.SetActive(false);
        
        yield return new WaitForSeconds(_shortFadingTime);
    
        statusText.gameObject.SetActive(true);
        statusText.text = "Your total score is: " + DataManager.Instance.currentScore;
        statusText.CrossFadeAlpha(1, 1.5f, false);
        yield return new WaitForSeconds(_shortFadingTime);
        statusText.CrossFadeAlpha(0, 1.5f, false);
        yield return new WaitForSeconds(_shortFadingTime);
        statusText.gameObject.SetActive(false);
    
        yield return new WaitForSeconds(_shortFadingTime);
    
        if (DataManager.Instance.currentScore > DataManager.Instance.bestScore)
        {
            statusText.gameObject.SetActive(true);
            statusText.text = "Congratulations! you are the new champion";
            statusText.CrossFadeAlpha(1, 1.5f, false);
            yield return new WaitForSeconds(_shortFadingTime);
            statusText.CrossFadeAlpha(0, 1.5f, false);
            yield return new WaitForSeconds(_shortFadingTime);
            statusText.gameObject.SetActive(false);
            playAgain.Select();
        }
    
        else
        {
            statusText.gameObject.SetActive(true);
            statusText.text = "Sorry, you haven't beaten our best player, Good luck next time";
            statusText.CrossFadeAlpha(1, 1.5f, false);
            yield return new WaitForSeconds(_longFadingTime);
            statusText.CrossFadeAlpha(0, 1.5f, false);
            yield return new WaitForSeconds(_longFadingTime);
            statusText.gameObject.SetActive(false);
            playAgain.Select();
        }
    
        yield return new WaitForSeconds(_shortFadingTime);
    
        if (!finalMenu.showMenu)
        {
            finalMenu.finalMenu.enabled = !finalMenu.finalMenu.enabled;
        }
    }
}
