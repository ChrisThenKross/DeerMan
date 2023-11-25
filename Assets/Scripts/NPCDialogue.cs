using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCDialogue : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    public string[] sentences;
    protected int index = 0;
    public float typingSpeed;
    protected bool conversationDone;

    public GameObject continueButton;

    public void Update()
    {
        if (textDisplay.text == sentences[index])
        {
            if(index == sentences.Length - 1)
                conversationDone = true;
            else
                continueButton.SetActive(true);
        }
    }

    public void StartConversation()
    {
        conversationDone = false;
        textDisplay.text = "";
        StartCoroutine(Type());
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
        Debug.Log("Next sentence");

        continueButton.SetActive(false);
        if (index < sentences.Length -1)
        {
            index++;
            textDisplay.text = "";
            StartCoroutine(Type());
        } else
        {
            textDisplay.text = "";
            Debug.Log("Conversation has ended");
            conversationDone = true;
        }
        continueButton.SetActive(false);
    }

    public bool getConversationStatus()
    {
        return conversationDone;
    }


}
