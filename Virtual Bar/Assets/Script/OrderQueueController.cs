using System;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class OrderQueueController : MonoBehaviour
{
    [Header("UI 생성 프리팹 및 컨테이너")]
    [SerializeField] private GameObject orderBubblePrefab; // OrderBubbleUI가 붙어 있는 프리팹
    [SerializeField] private Transform queueParent;         // Vertical Layout Group이 부착된 UI 패널

    [Header("주문 생성 규칙 설정")]
    [SerializeField] private List<RecipeSO> availableRecipes; // 조주기능사 40종 레시피 풀 중 현재 세션용 [4]
    [SerializeField] private float orderSpawnInterval = 15f;  // 새 주문이 들어오는 주기 (초)
    [SerializeField] private float defaultOrderDuration = 45f;// 한 주문당 제한 시간 (초)
    [SerializeField] private int maxActiveOrders = 3;         // 목업 상 최대 주문 개수 (3개 고정)

    private List<OrderData> activeOrders = new List<OrderData>();
    private List<OrderBubbleUI> spawnedBubbles = new List<OrderBubbleUI>();

    private float spawnTimer = 0f;
    private OrderData currentFocusedOrder; // 플레이어가 현재 제작 타깃으로 선택해 둔 주문

    // 외부 시스템(아카리 피드백 리액션 및 사운드 매니저)과 연동할 이벤트 정의 [2]
    public Action<OrderData> OnOrderSpawned;      // 주문 도네이션 수신 시 (효과음 & 아카리 대사 유도)
    public Action<OrderData> OnOrderTimedOut;     // 시간 초과로 주문이 소실되었을 때 (아카리 걱정 대사 유도)
    public Action<OrderData> OnOrderSelected;     // 제작 타깃이 변경되었을 때

    private void Start()
    {
        // 첫 번째 시작 주문은 세션 로드와 함께 즉시 하나를 스폰합니다.
        SpawnNewRandomOrder();
    }

    private void Update()
    {
        HandleOrderSpawning();
        HandleOrderTimers();
    }

    // 시간 경과에 따라 자동으로 다음 주문 생성
    private void HandleOrderSpawning()
    {
        if (activeOrders.Count >= maxActiveOrders) return;

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= orderSpawnInterval)
        {
            spawnTimer = 0f;
            SpawnNewRandomOrder();
        }
    }

    // 실시간으로 모든 액티브 주문들의 남은 시간을 깎음
    private void HandleOrderTimers()
    {
        for (int i = activeOrders.Count - 1; i >= 0; i--)
        {
            OrderData order = activeOrders[i];
            order.remainingTime -= Time.deltaTime;

            if (order.remainingTime <= 0f)
            {
                OnOrderExpired(order);
            }
        }
    }

    // 새로운 랜덤 주문 동적 스폰
    public void SpawnNewRandomOrder()
    {
        if (activeOrders.Count >= maxActiveOrders || availableRecipes.Count == 0) return;

        // 1. 레시피 풀에서 무작위 선택 및 가상 시청자 데이터 바인딩
        RecipeSO randomRecipe = availableRecipes[UnityEngine.Random.Range(0, availableRecipes.Count)];
        string viewerName = $"시청자_{UnityEngine.Random.Range(100, 999)}";
        string comment = GetPoliteDonationMessage(randomRecipe.cocktailNameKo);

        OrderData newOrder = new OrderData(viewerName, comment, randomRecipe, defaultOrderDuration);
        activeOrders.Add(newOrder);

        // 2. UI 캔버스에 프리팹 생성 후 바인딩
        GameObject go = Instantiate(orderBubblePrefab, queueParent);
        OrderBubbleUI bubbleUI = go.GetComponent<OrderBubbleUI>();
        if (bubbleUI != null)
        {
            bubbleUI.Bind(newOrder, SelectOrder);

            // 프리팹 내부 Button 컴포넌트에 클릭 콜백 등록
            Button btn = go.GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.AddListener(bubbleUI.OnClickBubble);
            }

            spawnedBubbles.Add(bubbleUI);

            // 현재 작업 중인 주문이 하나도 없었다면 자동으로 이 주문을 포커싱
            if (currentFocusedOrder == null)
            {
                SelectOrder(newOrder);
            }
        }

        OnOrderSpawned?.Invoke(newOrder);
        Debug.Log($"[Order System] 신규 주문 스폰: {viewerName} 님의 {randomRecipe.cocktailNameKo}");
    }

    // 플레이어가 특정 주문을 클릭하여 제작 타깃으로 설정할 때
    public void SelectOrder(OrderData selectedOrder)
    {
        currentFocusedOrder = selectedOrder;
        OnOrderSelected?.Invoke(selectedOrder);

        // 모든 말풍선들의 하이라이트 배지 및 배경 아웃라인 활성화 상태 변경
        foreach (var bubble in spawnedBubbles)
        {
            bubble.SetSelectedState(bubble.Data == currentFocusedOrder);
        }
    }

    // 조주 성공/실패 시 또는 시간 초과 시 대기열에서 즉시 제거하는 청소 함수
    public void RemoveOrder(OrderData order)
    {
        int index = activeOrders.IndexOf(order);
        if (index >= 0)
        {
            activeOrders.RemoveAt(index);

            OrderBubbleUI targetBubble = spawnedBubbles.Find(b => b.Data == order);
            if (targetBubble != null)
            {
                spawnedBubbles.Remove(targetBubble);
                Destroy(targetBubble.gameObject);
            }

            // 현재 조주 중이던 주문이 삭제된 경우 다음 남은 주문을 자동으로 타깃팅
            if (currentFocusedOrder == order)
            {
                currentFocusedOrder = activeOrders.Count > 0 ? activeOrders[0] : null;

                if (currentFocusedOrder != null)
                {
                    SelectOrder(currentFocusedOrder);
                }
                else
                {
                    OnOrderSelected?.Invoke(null);
                }
            }
        }
    }

    private void OnOrderExpired(OrderData expiredOrder)
    {
        OnOrderTimedOut?.Invoke(expiredOrder);
        RemoveOrder(expiredOrder);
    }

    // GDD v0.4 2.3 채팅창 톤앤매너를 지키는 정중한 예의바른 랜덤 메시지 [1]
    private string GetPoliteDonationMessage(string cocktailName)
    {
        string[] templates = new string[]
        {
            $"점장님, 오늘 하루도 고생 많으셨습니다. {cocktailName} 한 잔 조용히 청해봅니다.",
            $"언제나 고즈넉한 방송 고맙습니다. {cocktailName} 부탁드려요.",
            $"비 오는 깊은 밤에 참 어울리는 목소리네요. 따뜻한 {cocktailName} 주문합니다.",
            $"오늘 속상한 일이 조금 있었는데, {cocktailName} 한 잔 마시며 가만히 힐링하고 싶네요."
        };
        return templates[UnityEngine.Random.Range(0, templates.Length)];
    }
}