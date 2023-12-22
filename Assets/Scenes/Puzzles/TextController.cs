using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TextController : MonoBehaviour
{
    public TMP_Text text;
    // Start is called before the first frame update
    public float amplitude = 10f;
    public float frequency = 2f;


    public float fadeTime;
    private TextMeshProUGUI fadeAwayText;
    private float maxFadeTime;

    private void Start()
    {
        fadeAwayText = GetComponent<TextMeshProUGUI>();
        maxFadeTime = fadeTime;
    }

    void Update()
    {
        text.ForceMeshUpdate();
        var textInfo = text.textInfo;

        for(int i = 0; i<textInfo.characterCount; i++)
        {
            var charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible)
            {
                continue;
            }

            var verts = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;
            //guardam els vertexs de cada caixa de cada caracter
            for(int j = 0; j< 4; j++)
            {
                var orig = verts[charInfo.vertexIndex + j];
                verts[charInfo.vertexIndex + j] = orig + new Vector3(0, Mathf.Sin(Time.time * 2f + orig.x * 0.01f) * 3f, 0);
        
            }
        }

        for(int i = 0; i<textInfo.meshInfo.Length; i++)
        {
            var meshInfo = textInfo.meshInfo[i];
            meshInfo.mesh.vertices = meshInfo.vertices;
            text.UpdateGeometry(meshInfo.mesh, i);
        }





    }



    private void ShowText()
    {
        //StartCoroutine(FadeIn());
        float elapsedTime = 0;
        Color originalColor = text.color;
        while (elapsedTime < fadeTime)
        {
            float alpha = Mathf.Lerp(0, 2, (elapsedTime / fadeTime));
            text.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            elapsedTime += Time.deltaTime;
            
        }
    }
}
