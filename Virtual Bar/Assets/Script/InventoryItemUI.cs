using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class InventoryItemUI : MonoBehaviour
{
    [Header("UI 컴포넌트 참조")]
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Button actionButton;

    private IngredientSO itemData;
    private Action<IngredientSO> onClickCallback;

    // 데이터를 받아 UI 컴포넌트에 바인딩하는 메서드
    public void Bind(IngredientSO data, Action<IngredientSO> onClick)
    {
        itemData = data;
        onClickCallback = onClick;

        if (iconImage != null && data.icon != null)
        {
            iconImage.sprite = data.icon;
            iconImage.enabled = true;
        }
        else if (iconImage != null)
        {
            iconImage.enabled = false;
        }

        if (nameText != null)
        {
            nameText.text = data.displayNameKo;
        }

        // 이전 리스너 제거 후 새 액션 연결
        actionButton.onClick.RemoveAllListeners();
        actionButton.onClick.AddListener(OnItemClicked);
    }

    private void OnItemClicked()
    {
        onClickCallback?.Invoke(itemData);
    }
}