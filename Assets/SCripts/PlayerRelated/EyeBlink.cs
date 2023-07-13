using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeBlink : MonoBehaviour
{
    public Texture2D[] eyeTexts;

    public int index;
    public int matIndex;

    public Renderer render;


    private void Start()
    {
        StartCoroutine(Blink());
    }

    IEnumerator Blink()
    {
        while(true)
        {
            render.materials[matIndex].SetTexture("_MainTex", eyeTexts[0]);
            yield return new WaitForSeconds(0.05f);

            render.materials[matIndex].SetTexture("_MainTex", eyeTexts[1]);
            yield return new WaitForSeconds(0.05f);

            render.materials[matIndex].SetTexture("_MainTex", eyeTexts[2]);
            yield return new WaitForSeconds(0.05f);

            render.materials[matIndex].SetTexture("_MainTex", eyeTexts[1]);
            yield return new WaitForSeconds(0.05f);

            render.materials[matIndex].SetTexture("_MainTex", eyeTexts[0]);
            yield return new WaitForSeconds(Random.Range(1f, 10f));
        }
    }
}
