using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RitualCircle : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;
    private Color brightPurple;

    [SerializeField] 
    private float t;

    private float colorTime;
    
    private float recolorTime;

    [SerializeField]
    private bool isColoring;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.white;
        t = 0f;
        colorTime = 0.1f;
        recolorTime = 0.4f;
        isColoring = false;
        brightPurple = new Color(0.38f, 0.07f, 0.51f, 1f);
    }
    
    private void OnEnable()
    {
        Action1Manager.OnAllCandlesLit += StartColorOverTime;
    }

    private void OnDisable()
    {
        Action1Manager.OnAllCandlesLit -= StartColorOverTime;
    }

    private void StartColorOverTime(float darknessGain)
    {        
        t = 0f;
        if (isColoring == false)
        {
            StopAllCoroutines();
            StartCoroutine(ColorOverTime());
        }
    }

    private IEnumerator ColorOverTime()
    {
        isColoring = true;
        while (t < colorTime)
        {
            t += Time.deltaTime;
            spriteRenderer.color = Color.Lerp(Color.white, brightPurple, t/colorTime);
            yield return null;
        }
        StartCoroutine(RecolorOverTime());
    }

    private IEnumerator RecolorOverTime()
    {
        isColoring = false;
        t = 0f;
        yield return new WaitForSeconds(0.5f);
        while (t < recolorTime)
        {
            t += Time.deltaTime;
            spriteRenderer.color = Color.Lerp(brightPurple, Color.white, t/recolorTime);
            yield return null;
        }
        StopAllCoroutines();
    }
}
