using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPanelController : MonoBehaviour
{
    [System.Serializable]
    public struct TabButtonMapping
    {
        public IngredientCategory category;
        public Button tabButton;
        public GameObject activeIndicator; // 탭 밑에 활성화를 표시하는 주황색/흰색 라인 등
    }

    [Header("탭 UI 구성")]
    [SerializeField] private List<TabButtonMapping> tabButtonMappings;
    [SerializeField] private Color activeTabColor = Color.white;
    [SerializeField] private Color inactiveTabColor = new Color(0.7f, 0.7f, 0.7f, 0.5f);

    [Header("그리드 생성 설정")]
    [SerializeField] private Transform gridContentParent; // ScrollView - Content (Grid Layout Group 부착 필수)
    [SerializeField] private GameObject itemPrefab;       // InventoryItemUI 프리팹

    [Header("아이템 데이터 베이스")]
    [SerializeField] private List<IngredientSO> allIngredients; // 전체 IngredientSO 레퍼런스 리스트

    private IngredientCategory currentTab = IngredientCategory.Spirits;
    private List<InventoryItemUI> activeSpawnedItems = new List<InventoryItemUI>();

    // 재료 선택 시 발생하는 이벤트 (MainSceneUIManager 등에서 구독하여 조주 루프 처리)
    public Action<IngredientSO> OnIngredientSelected;

    private void Start()
    {
        InitializeTabButtons();
        SwitchTab(IngredientCategory.Spirits); // 시작 시 '기주' 탭 기본 활성화
    }

    // 탭 버튼 클릭 이벤트 바인딩
    private void InitializeTabButtons()
    {
        foreach (var mapping in tabButtonMappings)
        {
            IngredientCategory targetCategory = mapping.category;
            mapping.tabButton.onClick.AddListener(() => SwitchTab(targetCategory));
        }
    }

    // 탭을 전환하고 그리드를 리바인딩하는 메인 함수
    public void SwitchTab(IngredientCategory nextCategory)
    {
        currentTab = nextCategory;

        // 1. 탭 버튼 비주얼 업데이트 (활성/비활성 색상 및 인디케이터 처리)
        UpdateTabVisuals();

        // 2. 기존 그리드 아이템 클리어
        ClearCurrentGrid();

        // 3. 전체 데이터베이스에서 현재 카테고리에 맞는 재료 필터링
        List<IngredientSO> filteredItems = allIngredients.FindAll(item => item.category == nextCategory);

        // 4. 프리팹 동적 생성 및 바인딩
        foreach (var ingredient in filteredItems)
        {
            GameObject go = Instantiate(itemPrefab, gridContentParent);
            InventoryItemUI itemUI = go.GetComponent<InventoryItemUI>();
            if (itemUI != null)
            {
                itemUI.Bind(ingredient, RaiseIngredientSelectedEvent);
                activeSpawnedItems.Add(itemUI);
            }
        }
    }

    private void UpdateTabVisuals()
    {
        foreach (var mapping in tabButtonMappings)
        {
            bool isCurrent = (mapping.category == currentTab);
            
            // 활성 인디케이터 라인 온/오프
            if (mapping.activeIndicator != null)
            {
                mapping.activeIndicator.SetActive(isCurrent);
            }

            // 버튼의 텍스트 또는 이미지 알파 값/색상 변화 피드백
            var textComponent = mapping.tabButton.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            if (textComponent != null)
            {
                textComponent.color = isCurrent ? activeTabColor : inactiveTabColor;
            }
        }
    }

    private void ClearCurrentGrid()
    {
        foreach (var item in activeSpawnedItems)
        {
            if (item != null)
            {
                Destroy(item.gameObject);
            }
        }
        activeSpawnedItems.Clear();
    }

    private void RaiseIngredientSelectedEvent(IngredientSO item)
    {
        // 구독하고 있는 MainSceneUIManager나 플레이어 조주 모듈로 이벤트 발송
        OnIngredientSelected?.Invoke(item);
    }
}