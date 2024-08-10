using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ButtonAniamtion : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public float scaleFactor = 1.2f;
    public float animationDuration = .2f;


    private Vector3 originalScale;
    private Button button;
    private AudioSource clickSound;
    // Start is called before the first frame update
    void Start()
    {
        clickSound = GetComponent<AudioSource>();
        button = GetComponent<Button>();
       
        originalScale = transform.localScale;
    }

    // Update is called once per frame
    public void OnPointerDown(PointerEventData eventData)
    {
        clickSound.Play();
        StartCoroutine(AnimateScale(originalScale * scaleFactor));
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        StartCoroutine(AnimateScale(originalScale));
    }

    private IEnumerator AnimateScale(Vector3 targetScale)
    {
        Vector3 startScale = transform.localScale;
        float elaspedTime = 0f;
        while (elaspedTime < animationDuration)
        {
            transform.localScale = Vector3.Lerp(startScale, targetScale, elaspedTime / animationDuration);
            elaspedTime += Time.deltaTime;
            yield return null;
        }
        transform.localScale = targetScale;
    }
}
