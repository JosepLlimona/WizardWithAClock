using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeObject : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private bool InicialmentVisualitzable;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (!InicialmentVisualitzable)
        {
            Color c = spriteRenderer.material.color;
            c.a = 0f;
            spriteRenderer.material.color = c;
        }


    }

    // Update is called once per frame
    IEnumerator FadeIn()
    {
        for (float f = 0.05f; f <= 1; f += 0.05f)
        {
            Color c = spriteRenderer.material.color;
            c.a = f;
            spriteRenderer.material.color = c;
            yield return new WaitForSeconds(0.05f);
        }
    }

    IEnumerator FadeOut()
    {
        for(float f = 1f; f>=-0.05f; f -= 0.05f)
        {
            Color c = spriteRenderer.material.color;
            c.a = f;
            spriteRenderer.material.color = c;
            yield return new WaitForSeconds(0.05f);
        }
    }
    public void startFadingOut()
    {
        StartCoroutine("FadeOut");
    }

    public void startFadingIn()
    {
        StartCoroutine("FadeIn");
    }



}
