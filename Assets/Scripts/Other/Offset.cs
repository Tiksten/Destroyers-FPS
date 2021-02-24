using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Offset : MonoBehaviour
{
public Renderer rend;
    void Start()
    {
        rend.material.SetTextureOffset("_MainTex", new Vector2(-1, -1));
    }
}
