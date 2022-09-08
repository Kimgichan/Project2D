using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using NaughtyAttributes;

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

public class InventoryUI : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
    #region 변수 목록
    [SerializeField] private Canvas canvas;
    [SerializeField] private List<ItemSlot> slots;
    [SerializeField] private ItemDescription selectSlotDesc;
    [SerializeField] private ProgressBar progressBar;

    [ReadOnly] [SerializeField] private InventoryDecorator inventory;
    [SerializeField] private ItemSlot dragSlot;
    [SerializeField] private int dragItemCount;
    [SerializeField] private float pressDelay;
    [SerializeField] private float pressTimer;

    private IEnumerator pressSlotCor;
    private TweenerCore<float, float, FloatOptions> progressTween;
    #endregion


    #region 프로퍼티 목록
    #endregion


    #region 함수 목록
    private void OnEnable()
    {
        progressBar.GetComponent<CanvasGroup>().alpha = 0f;
    }

    public bool Open()
    {
        var board = GameManager.Instance.board;
        var user = GameManager.Instance.playerController;

        if(board == null || user == null)
        {
            gameObject.SetActive(false);
            return false;
        }

        if(!user.TryGetDecorator(Enums.Decorator.Inventory, out Component val))
        {
            gameObject.SetActive(false);
            return false;
        }

        gameObject.SetActive(true);

        inventory = val as InventoryDecorator;
        InventoryUpdate();
        return true;
    }

    public void Close()
    {
        inventory = null;
        gameObject.SetActive(false);
    }

    [Button("인벤토리 업데이트")] public void InventoryUpdate()
    {
        if(!gameObject.activeSelf || inventory == null)
        {
            gameObject.SetActive(false);
            return;
        }

        var destCount = inventory.GetItemCount();
        var currentCount = slots.Count;


        slots[0].indx = 0;
        // destCount > currentCount 일 때
        for(int i = 0, icount = destCount - currentCount; i<icount; i++)
        {
            var slot = Instantiate(slots[0], slots[0].transform.parent);
            slot.indx = slots.Count;
            slots.Add(slot);
        }


        // destCount < currentCount 일 때
        for(int i = currentCount - 1; i>=destCount; i--)
        {
            Destroy(slots[i]);
        }


        for(int i = 0; i<destCount; i++)
        {
            slots[i].gameObject.SetActive(true);
            slots[i].Item = inventory.GetItem(i);
        }
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        OnReleaseSlot();
        var raycaster = canvas.GetComponent<GraphicRaycaster>();
        if (raycaster == null) return;

        var results = new List<RaycastResult>();
        raycaster.Raycast(eventData, results);

        for(int i = 0, icount = results.Count; i<icount; i++)
        {
            var itemSlot = results[i].gameObject.GetComponent<ItemSlot>();
            if (itemSlot == null) continue;

            if (itemSlot.Item == null) return;

            dragSlot.gameObject.SetActive(true);

            (dragSlot.transform as RectTransform).localScale = Vector3.one;
            (dragSlot.transform as RectTransform).sizeDelta = (itemSlot.transform as RectTransform).sizeDelta;

            (dragSlot.transform as RectTransform).anchorMin = new Vector2(0.5f, 0.5f);
            (dragSlot.transform as RectTransform).anchorMax = new Vector2(0.5f, 0.5f);

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, eventData.position, (canvas.renderMode == RenderMode.ScreenSpaceOverlay) ? null : canvas.worldCamera, out Vector2 outLocalPos))
            {
                (dragSlot.transform as RectTransform).localPosition = outLocalPos;
            }

            dragSlot.Item = itemSlot.Item;
            dragSlot.indx = itemSlot.indx;

            break;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        OnReleaseSlot();
        if (!dragSlot.gameObject.activeSelf) return;

        //(dragSlot.transform as RectTransform).localPosition = eventData.position;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, eventData.position, (canvas.renderMode == RenderMode.ScreenSpaceOverlay) ? null : canvas.worldCamera, out Vector2 outLocalPos))
        {
            (dragSlot.transform as RectTransform).localPosition = outLocalPos;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnReleaseSlot();
        if (!dragSlot.gameObject.activeSelf) return;

        var raycaster = canvas.GetComponent<GraphicRaycaster>();
        if (raycaster == null) return;

        var results = new List<RaycastResult>();
        raycaster.Raycast(eventData, results);

        for(int i = 0, icount = results.Count; i<icount; i++)
        {
            var itemSlot = results[i].gameObject.GetComponent<ItemSlot>();
            if (itemSlot == null || itemSlot == dragSlot) continue;

            var inventory = GameManager.Instance.playerController.GetDecorator(Enums.Decorator.Inventory) as InventoryDecorator;

            if (inventory == null) break;

            inventory.SendItem(dragSlot.indx, dragItemCount, inventory, itemSlot.indx);

            InventoryUpdate();

            break;
        }
        dragSlot.gameObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        var raycaster = canvas.GetComponent<GraphicRaycaster>();
        if (raycaster == null) return;

        var results = new List<RaycastResult>();
        raycaster.Raycast(eventData, results);

        for (int i = 0, icount = results.Count; i < icount; i++)
        {
            var itemSlot = results[i].gameObject.GetComponent<ItemSlot>();
            if (itemSlot == null) continue;

            selectSlotDesc.Play(itemSlot.Item, 1f);
            OnPressSlot(itemSlot);
            break;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnReleaseSlot();
        progressBar.GetComponent<CanvasGroup>().alpha = 0f;
    }

    private void OnPressSlot(ItemSlot slot)
    {
        dragItemCount = slot.Item.CurrentCount;
        if (slot.Item.CurrentCount > 1)
        {
            if (pressSlotCor != null) StopCoroutine(pressSlotCor);
            pressSlotCor = PressSlotCor(slot);

            StartCoroutine(pressSlotCor);
        }
    }

    private IEnumerator PressSlotCor(ItemSlot slot)
    {
        yield return new WaitForSeconds(pressDelay);

        if(progressTween != null)
        {
            progressTween.Kill();
        }

        AttachProgressBar(slot);

        float start = 0f;
        progressTween = DOTween.To(() => start, (getter) => start = getter, 1f, pressTimer).OnUpdate(() =>
        {
            progressBar.Percentage = start;
            dragItemCount = (int)(start * (float)slot.Item.CurrentCount);
            if (dragItemCount <= 0) dragItemCount = 1;
        }).OnComplete(() =>
        {
            dragItemCount = slot.Item.CurrentCount;
            progressTween = null;
        });
    }

    private void AttachProgressBar(ItemSlot slot)
    {
        progressBar.GetComponent<CanvasGroup>().alpha = 1f;

        var rectTr = progressBar.transform as RectTransform;

        rectTr.parent = slot.transform;

        rectTr.localScale = Vector3.one;

        var min = rectTr.offsetMin;
        var max = rectTr.offsetMax;

        min.x = 1f;
        max.x = -1f;

        rectTr.offsetMin = min;
        rectTr.offsetMax = max;

        var pos = rectTr.anchoredPosition;
        pos.y = -20f;
        rectTr.anchoredPosition = pos;
    }

    private void OnReleaseSlot()
    {
        if(pressSlotCor != null)
        {
            StopCoroutine(pressSlotCor);
            pressSlotCor = null;
        }

        if (progressTween != null)
        {
            progressTween.Kill();
            progressTween = null;
        }
    }

    #endregion
}
