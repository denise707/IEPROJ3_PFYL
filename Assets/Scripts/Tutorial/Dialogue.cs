using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private GameObject hotBar;
    public Image imageSlot;
    public Sprite georgeImage;
    public Sprite mcImage;

    public TextMeshProUGUI textComponent;
    public string[] lines;
    private int index;
    public static Dialogue instance;
    float INTERVAL = 2.0f;
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
        textComponent.text = "(...)";
        dialogueBox.SetActive(false);
        hotBar.SetActive(true);
        StartDialogue(0);    
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && dialogueBox.activeInHierarchy)
        {
            if (textComponent.text == lines[index])
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
            ticks += 0.02f;
            if (ticks > INTERVAL)
            {
                Time.timeScale = 0;
                dialogueBox.SetActive(true);
                hotBar.SetActive(false);
                textComponent.text = string.Empty;
                StartCoroutine(TypeLine());
                ticks = 0;
                start = false;
            }
        }

        Debug.Log(ticks);
    }

    public void StartDialogue(int index)
    {
        this.index = index;
        start = true;
        SetImage();
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
            Time.timeScale = 1;
        }

        else if(lines[index] == "+")
        {
            dialogueBox.SetActive(false);
            hotBar.SetActive(true);
            Time.timeScale = 1;
            ticks = 0f;
            //StartDialogue(index++);
            StartCoroutine(Transition());
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

        SetImage();
    }

    void SetImage()
    {
        int[] mcLines = {0, 1, 4, 35};

        bool isMC = false;

        for(int i = 0; i < mcLines.Length; i++)
        {
            if(index == mcLines[i])
            {
                isMC = true;
            }
        }

        if (isMC)
        {
            imageSlot.sprite = mcImage;
        }

        else
        {
            imageSlot.sprite = georgeImage;
        }
    }

    IEnumerator TypeLine()
    {
        foreach(char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSecondsRealtime(0.08f);
        }
    }

    IEnumerator Transition()
    {
        
        yield return new WaitForSecondsRealtime(1f);
        StartDialogue(3);
    }
}