﻿using System;
using DG.Tweening;
using UnityEngine;

public class GemView: BaseView<GemModel, GemController<GemModel>>
{
    SpriteRenderer spriteRenderer;
    // TextMesh idText;
    // TextMesh markerIdText;
    bool isDebugging = true;
    MaterialPropertyBlock mpb;
    Tween highlight;
    
    void Awake()
    {
        // idText = ResourceCache.Instantiate(Literals.ID, transform).GetComponent<TextMesh>();
        // markerIdText = ResourceCache.Instantiate(Literals.MarkerID, transform).GetComponent<TextMesh>();
        // markerIdText.gameObject.SetActive(false);
        mpb = new MaterialPropertyBlock();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
#if DISABLE_DEBUG
        isDebugging = false;
#endif
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        
        mpb = null;
        if (highlight != null) 
        { 
            highlight.Kill(); 
            highlight = null; 
        }
    }
    
    public Position Position 
    { 
        get { return Model.Position; } 
    }

    public Int64 PreservedFromMatch
    {
        get { return Model.preservedFromMatch; }
    }

    public override void UpdateModel(GemModel gemModel) 
    {
        base.UpdateModel(gemModel);

        // idText.text = gemModel.id.ToString();
    }
    
    public void Squash() 
    {
        // if it does in disappearing already
        if (transform.localScale.x < .9f && transform.localScale.y < .9f) { return; }

        var squash = DOTween.Sequence();
        squash.OnStart(() => transform.localScale = Vector3.one);
        squash.Append(transform.DOScale(new Vector3(1.08f, 0.92f, 1), 0.12f));
        squash.Append(transform.DOScale(Vector3.one, 0.68f).SetEase(Ease.OutElastic));
        
        if (!isDebugging) { return; }
        spriteRenderer.GetPropertyBlock(mpb);
        mpb.SetFloat("_FlashAmount", 0.0f);
        spriteRenderer.SetPropertyBlock(mpb);
    }

    public void Reveal() 
    {
        gameObject.SetActive(true);
        transform.DOScale(new Vector3(0.92f, 1.08f, 1), 0.12f);

        if (!isDebugging) { return; }
        var color = spriteRenderer.color;
        spriteRenderer.color = new Color(color.r, color.g, color.b, 1f);

        spriteRenderer.GetPropertyBlock(mpb);
        mpb.SetFloat("_FlashAmount", 0.4f);
        spriteRenderer.SetPropertyBlock(mpb);
    }

    public void Hide()
    {
        gameObject.SetActive(false);

        if (!isDebugging) { return; }
        gameObject.SetActive(true);
        var color = spriteRenderer.color;
        spriteRenderer.color = new Color(color.r, color.g, color.b, 0.1f);
    }

    public void SetBlock(Int64 markerID) 
    {
        if (!isDebugging) { return; }

        spriteRenderer.GetPropertyBlock(mpb);
        mpb.SetFloat("_FlashAmount", 0.4f);
        mpb.SetColor("_FlashColor", new Color32(255, 0, 0, 255));
        spriteRenderer.SetPropertyBlock(mpb);

        // markerIdText.text = markerID.ToString();
        // markerIdText.gameObject.SetActive(true);
    }

    public void Highlight() 
    {
        spriteRenderer.GetPropertyBlock(mpb);

        highlight = DOTween.To(
            GetFlashAmount,
            SetFlashAmount, 
            .4f, 
            .395f
        ).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutSine).OnComplete(() => highlight = null);
    }

    float GetFlashAmount()
    {
        return mpb.GetFloat("_FlashAmount");
    }

    void SetFlashAmount(float value)
    {
        mpb.SetFloat("_FlashAmount", value);
        spriteRenderer.SetPropertyBlock(mpb);
    }

    public void SetActive(bool visible)
    {
        gameObject.SetActive(visible);
    }

    public void ReturnToPool(bool withAnimation = true, Vector2 combiningPosition = default(Vector2))
    {   
        // markerIdText.gameObject.SetActive(false);

        if (withAnimation)
        {
            if (object.Equals(combiningPosition, default(Vector2))) { 
                transform.DOScale(Vector3.zero, .295f).OnComplete(() => {
                    base.ReturnToPool();
                    transform.localScale = Vector3.one;    
                }).SetEase(Ease.OutCirc);
                MatchSound.Instance.Play("Pop");
            } 
            else {
                var sequence = DOTween.Sequence().OnComplete(() => {
                    base.ReturnToPool();
                    transform.localScale = Vector3.one; 
                });
                sequence.Insert(0, transform.DOScale(Vector3.zero, .295f).SetEase(Ease.OutCirc));
                sequence.Insert(0, transform.DOLocalMove(combiningPosition, .295f));
                MatchSound.Instance.Play("Bite");
            }
        }
        else
        {
            base.ReturnToPool();
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.GetPropertyBlock(mpb);
            mpb.SetFloat("_FlashAmount", 0.0f);
            mpb.SetColor("_FlashColor", new Color32(255, 255, 255, 255));
            spriteRenderer.SetPropertyBlock(mpb);
        }
    }
}
