using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class CandleFlame : MonoBehaviour
{

    [SerializeField]
    private bool isLit;

    private SpriteRenderer sr;
    private Animator anim;
    private CapsuleCollider2D capsule;
    private float dwindleTime;
    private Color faintPurple;
    [SerializeField]
    private Vector3 originalPosition;
    [SerializeField]
    private Vector3 lowerPosition;
    private Vector3 originalScale;
    public static event Action<Vector2> OnCandleLit;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        capsule = GetComponent<CapsuleCollider2D>();
        sr.enabled = false;
        anim.enabled = false;
        capsule.enabled = true;
        isLit = false;
        originalPosition = transform.position;
        lowerPosition = new Vector3(originalPosition.x, originalPosition.y - 0.36f, originalPosition.z);
        originalScale = new Vector3(1.5f, 1.5f, 2f);
        faintPurple = new Color(0.4f, 0, 0.5f, 0.2f);
    }

    private void OnEnable()
    {
        Action1Manager.OnAllCandlesLit += GoOut;
    }

    private void OnDisable()
    {
        Action1Manager.OnAllCandlesLit -= GoOut;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isLit == false && collision.gameObject.tag == "Player" )
        {
            sr.enabled = true;
            anim.enabled = true;
            sr.color = Color.white;
            isLit = true;
            capsule.enabled = false;
            OnCandleLit?.Invoke(transform.position);
        }
    }

    private void GoOut(float darknessGain)
    {
        isLit = false;
        dwindleTime = Random.Range(0.25f, 0.5f);
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
            time += Time.deltaTime;
            if (time >= dwindleTime)
            {
                sr.enabled = false;
                anim.enabled = false;
                capsule.enabled = true;
                transform.localScale = originalScale;
                transform.position = originalPosition;
            }
            yield return null;
        }
    }

}
