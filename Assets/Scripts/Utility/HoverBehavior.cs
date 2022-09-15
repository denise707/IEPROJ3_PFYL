using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverBehavior : MonoBehaviour
{
    public Color startcolor;
    public Renderer renderer;
    [SerializeField] float intensity;
   
    // Start is called before the first frame update
    void Start()
    {
        renderer = gameObject.GetComponent<Renderer>();
    }
    void OnMouseEnter()
    {

        startcolor = renderer.material.color;
        renderer.material.color =   new Color (startcolor.r+ intensity, startcolor.g + intensity, startcolor.b + intensity ) ;
    }
    void OnMouseExit()
    {
        //startcolor = renderer.material.color;
        Color currCol = renderer.material.color;

        renderer.material.color = new Color(currCol.r - intensity, currCol.g - intensity, currCol.b - intensity);

    }

    public void UpdateStartColor()
    {
        startcolor = renderer.material.color;
    }

}
