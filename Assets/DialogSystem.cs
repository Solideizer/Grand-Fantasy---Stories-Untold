using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    public string[] sentences;
    private int index;
    public float typingSpeed;
    public Button continueButton;

    private CanvasGroup continueHUD;

    private void Awake()
    {
        continueHUD = continueButton.GetComponent<CanvasGroup>();
    }
    private void Update()
    {
        if(textDisplay.text == sentences[index])
        {
            continueHUD.alpha = 1f;
            continueHUD.interactable = true;
            continueHUD.blocksRaycasts = true;
        }
    }
    IEnumerator Type()
    {
        foreach (char letter in sentences[index].ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void NextSentence()
    {
        continueHUD.alpha = 0f;
        continueHUD.interactable = false;
        continueHUD.blocksRaycasts = false;

        if (index < sentences.Length - 1)
        {
            index++;
            textDisplay.text = "";
            StartCoroutine(Type());
        }
        else
        {
            textDisplay.text = "";
        }        
                
    }
}
