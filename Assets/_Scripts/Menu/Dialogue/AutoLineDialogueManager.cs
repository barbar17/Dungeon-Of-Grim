using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.SceneManagement;

public class AutoLineDialogueManager : MonoBehaviour
{
    public UnityEvent OnFinishedDialogue;
    public DialogeDataSO[] dialogeDatas;
    public TextMeshProUGUI textDialogue;
    private string[] lines;
    public float textTypingSpeed;

    private int index;

    // Start is called before the first frame update
    void Start()
    {
        textDialogue.text = string.Empty;

        int pickDialogue = Random.Range(0, dialogeDatas.Length);
        lines = dialogeDatas[pickDialogue].lines;

        StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if (textDialogue.text == lines[index])
        {
            NextLine();
        }
    }

    private void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine(0f));
    }

    private IEnumerator TypeLine(float lineDelay)
    {
        yield return new WaitForSeconds(lineDelay);
        textDialogue.text = string.Empty;
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
            StartCoroutine(TypeLine(1f));
        }
        else
        {
            OnFinishedDialogue?.Invoke();
        }
    }

    public void LoadMainMenu()
    {
        SceneController.instance.LoadMainMenuScene();
    }
}
