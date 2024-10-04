using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public DialogeDataSO[] dialogeDatas;
    public TextMeshProUGUI textDialogue;
    private string[] lines;
    public float textTypingSpeed;
    private bool isDialogueFinished;
    private int index;

    // Start is called before the first frame update
    void Start()
    {
        textDialogue.text = string.Empty;
        isDialogueFinished = false;

        int pickDialogue = Random.Range(0, dialogeDatas.Length);
        lines = dialogeDatas[pickDialogue].lines;

        AudioManager.instance.PlayMusic(AudioManager.instance.gameOverMusic);
        StartDialogue();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (textDialogue.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textDialogue.text = lines[index];
            }
        }
    }

    private void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    private IEnumerator TypeLine()
    {
        foreach (var charachter in lines[index].ToCharArray())
        {
            textDialogue.text += charachter;
            AudioManager.instance.TypingSFX();
            yield return new WaitForSeconds(textTypingSpeed);
        }
    }

    private void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textDialogue.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else if (index == lines.Length - 1 && !isDialogueFinished)
        {
            isDialogueFinished = true;
            LoadMainMenu();
        }
    }

    public void LoadMainMenu()
    {
        SceneController.instance.LoadMainMenuScene();
    }
}
