using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureScroll : MonoBehaviour
{
    [SerializeField] private float scrollX;
    [SerializeField] private float scrollY;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float offsetX = Time.time * scrollX;
        float offsetY = Time.time * scrollY;
        GetComponent<Renderer>().material.SetTextureOffset("_OffsetTex", new Vector2(offsetX,offsetY));
    }
}
