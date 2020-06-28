using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class FacilityEffectList : MonoBehaviour
{
    public List<SkeletonDataAsset> animAssets;
    public bool isTable;
    private SpriteRenderer spriteRender;
    private MeshRenderer meshRenderer;
    private float timeHit = 0f;

    private void Start()
    {
        spriteRender = transform.parent.GetComponent<SpriteRenderer>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        timeHit += Time.deltaTime;
        if (timeHit > 0.2f)
        {
            meshRenderer.sortingOrder = isTable ? spriteRender.sortingOrder + 1 : spriteRender.sortingOrder;
            timeHit = 0;
        }
    }
}
