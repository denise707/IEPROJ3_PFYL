using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private GameObject hotBar;
    public TextMeshProUGUI textComponent;
    public string[] lines;
    private int index;
    public static Dialogue instance;
    float INTERVAL = 1.0f;
    float ticks = 0f;
    bool start = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        textComponent.text = string.Empty;
        dialogueBox.SetActive(false);
        hotBar.SetActive(true);
        StartDialogue(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && dialogueBox.activeInHierarchy)
        {
            if(textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }

        if (start)
        {
            ticks += Time.deltaTime;            
            if (ticks > INTERVAL)
            {
                dialogueBox.SetActive(true);
                hotBar.SetActive(false);
                textComponent.text = string.Empty;
                StartCoroutine(TypeLine());
                ticks = 0;
                start = false;
            }            
        }        
    }

    public void StartDialogue(int index)
    {
        this.index = index;
        start = true;
    }

    void NextLine()
    {
        index++;
        if (lines[index] == "-")
        {
            Tutorial.tutorialStep++;
            Debug.Log("Step " + Tutorial.tutorialStep);
            Tutorial.isDone = false;
            ticks = 0f;
            dialogueBox.SetActive(false);
            hotBar.SetActive(true);
        }

        else if (index < lines.Length)
        {                                     
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());           
        }
        else
        {
            dialogueBox.SetActive(false);
            hotBar.SetActive(true);
        }
    }

    IEnumerator TypeLine()
    {
        foreach(char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(0.3f);
        }
    }
}