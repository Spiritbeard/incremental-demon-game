using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using UnityEngine.Rendering.Universal;

public class CandleFlame : MonoBehaviour
{

    [SerializeField]
    private bool isLit;

    private SpriteRenderer sr;
    private Animator anim;
    private CapsuleCollider2D capsule;
    private Light2D light2D;
    private float dwindleTime;
    private Color faintPurple;
    private Vector3 originalPosition;
    private Vector3 lowerPosition;
    private Vector3 originalScale;
    public static event Action<Vector2> OnCandleLit;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.enabled = false;
        anim = GetComponent<Animator>();
        anim.enabled = false;
        capsule = GetComponent<CapsuleCollider2D>();
        capsule.enabled = true;
        light2D = GetComponent<Light2D>();
        light2D.enabled = false;
        isLit = false;
        originalPosition = transform.position;
        lowerPosition = new Vector3(originalPosition.x, originalPosition.y - 0.36f, originalPosition.z);
        originalScale = new Vector3(1.5f, 1.5f, 2f);
        faintPurple = new Color(0.4f, 0, 0.5f, 0.2f);
    }

    private void OnEnable()
    {
        Activity1Manager.OnAllCandlesLit += GoOut;
    }

    private void OnDisable()
    {
        Activity1Manager.OnAllCandlesLit -= GoOut;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isLit == false && collision.gameObject.tag == "Player" )
        {
            sr.enabled = true;
            anim.enabled = true;
            light2D.enabled = true;
            light2D.intensity = 2f;
            sr.color = Color.white;
            isLit = true;
            capsule.enabled = false;
            OnCandleLit?.Invoke(transform.position);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isLit == false && collision.gameObject.tag == "Player" )
        {
            sr.enabled = true;
            anim.enabled = true;
            light2D.enabled = true;
            light2D.intensity = 2f;
            sr.color = Color.white;
            isLit = true;
            capsule.enabled = false;
            OnCandleLit?.Invoke(transform.position);
        }
    }

    private void GoOut(float darknessGain)
    {
        isLit = false;
        dwindleTime = 0.35f;
        StartCoroutine("DwindleFlame");
    }

    private IEnumerator DwindleFlame()
    {
        float time = 0f;
        while (time < dwindleTime)
        {
            transform.localScale = Vector3.Lerp(originalScale, originalScale * 0.5f, time/dwindleTime);
            transform.position = Vector3.Lerp(originalPosition, lowerPosition, time/dwindleTime);
            sr.color = Color.Lerp(Color.white, faintPurple, time/dwindleTime);
            light2D.intensity = Mathf.Lerp(2.5f, 0.5f, time/dwindleTime);
            time += Time.deltaTime;
            if (time >= dwindleTime)
            {
                sr.enabled = false;
                anim.enabled = false;
                light2D.enabled = false;   
                capsule.enabled = true;
                transform.localScale = originalScale;
                transform.position = originalPosition;
            }
            yield return null;
        }
    }

}
