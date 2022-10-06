using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverBehavior : MonoBehaviour
{
    public Color startcolor;
    public Renderer renderer;
    [SerializeField] float intensity;

    public bool inRange = false;
    public bool isHighlighted = false;


    // Start is called before the first frame update
    void Start()
    {
        renderer = gameObject.GetComponent<Renderer>();
    }
    void OnMouseEnter()
    {
        //Debug.Log(gameObject.name);
        if (inRange)
        {
            Color currCol = renderer.material.color;
            renderer.material.color = new Color(currCol.r + intensity, currCol.g + intensity, currCol.b + intensity);
            isHighlighted = true;
        }
        
    }
    void OnMouseExit()
    {
        Color currCol = renderer.material.color;

        // out of range && highlighted
        if ((!inRange && isHighlighted) || (inRange && isHighlighted))
        {
            
            renderer.material.color = new Color(currCol.r - intensity, currCol.g - intensity, currCol.b - intensity);
            isHighlighted = false;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.gameObject.name);
        //// highlight when in range and hovered 
        //if (collision.gameObject.CompareTag("PlayerRange"))
        //{
        //    inRange = true;
        //    Debug.Log("Enter");
        //}
    }

    private void OnCollisionExit(Collision collision)
    {
        //// remove highlight when out of range or 
        //if (collision.gameObject.CompareTag("PlayerRange"))
        //{
        //    inRange = false;
        //    Debug.Log("Exit");

        //}
    }




    public void UpdateStartColor()
    {
        startcolor = renderer.material.color;
    }

}
