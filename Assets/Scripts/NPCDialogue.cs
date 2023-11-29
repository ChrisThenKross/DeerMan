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

    public GameObject TextPanel;
    public GameObject DeerWizardImage;
    public GameObject NPCInstructions; 
    public GameObject Abilities;
    public GameObject PQueue;
    public GameObject PlayerHealth;
    public GameObject WizardLore;

    public void Update()
    {
        if (textDisplay.text == sentences[index])
        {
            if(index == sentences.Length - 1)
                conversationDone = true;
            else
                continueButton.SetActive(true);
        }

        if (conversationDone)
        {
            TextPanel.SetActive(false);
            DeerWizardImage.SetActive(false);
            NPCInstructions.SetActive(true);
            Abilities.SetActive(true);
            PQueue.SetActive(true);
            PlayerHealth.SetActive(true);
            WizardLore.SetActive(false);
        }
    }

    public void StartConversation()
    {
        TextPanel.SetActive(true);
        DeerWizardImage.SetActive(true);
        NPCInstructions.SetActive(false);
        Abilities.SetActive(false);
        PQueue.SetActive(false);
        PlayerHealth.SetActive(false);
        
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
