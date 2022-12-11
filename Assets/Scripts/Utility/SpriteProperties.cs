using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteProperties : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        GetComponent<SpriteRenderer>().receiveShadows = true;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}