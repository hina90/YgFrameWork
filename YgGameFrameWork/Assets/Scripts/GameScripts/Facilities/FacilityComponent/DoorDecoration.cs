using UnityEngine;

public class DoorDecoration : MonoBehaviour
{
    public SpriteRenderer defaultDoorRender;
    private SpriteRenderer doorWallRender;

    private void Awake()
    {
        doorWallRender = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        gameObject.AddComponent<SortLayerCom>();
        GameManager.Instance.AddToSortList(transform);
    }

    private void LateUpdate()
    {
        if (defaultDoorRender != null)
        {
            defaultDoorRender.sortingOrder = doorWallRender.sortingOrder + 1;
        }
    }
}
