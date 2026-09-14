using UnityEngine;
using TMPro;

public class MainSceneUIManager : MonoBehaviour
{
    [Header("인벤토리 & 대기열 시스템 참조")]
    [SerializeField] private InventoryPanelController inventoryController;
    [SerializeField] private OrderQueueController orderQueueController;

    [Header("전체 게임 실시간 데이터")]
    [SerializeField] private PlayerInputCocktail currentInput = new PlayerInputCocktail(); // 1번 스크립트 인스턴스
    private CocktailEvaluationEngine evaluationEngine = new CocktailEvaluationEngine(); // 2번 스크립트 인스턴스

    [Header("방송 정보 및 도네이션 UI")]
    [SerializeField] private TextMeshProUGUI totalSuperChatText; // 화면 최상단 1,500 P 점수판 [5]
    private int totalSuperChat = 0; // 누적 점수

    private OrderData activeTargetOrder; // 현재 주문 대기열에서 가리키는 대상

    private void OnEnable()
    {
        if (inventoryController != null)
            inventoryController.OnIngredientSelected += OnIngredientClicked;

        if (orderQueueController != null)
            orderQueueController.OnOrderSelected += HandleOrderFocused;
    }

    private void OnDisable()
    {
        if (inventoryController != null)
            inventoryController.OnIngredientSelected -= OnIngredientClicked;

        if (orderQueueController != null)
            orderQueueController.OnOrderSelected -= HandleOrderFocused;
    }

    // 주문 리스트에서 특정 시청자의 주문을 클릭했을 때 타깃 주문 갱신
    private void HandleOrderFocused(OrderData selectedOrder)
    {
        activeTargetOrder = selectedOrder;
    }

    private void OnIngredientClicked(IngredientSO selectedItem)
    {
        // ... (앞서 세팅된 재료 추가 로직 실행) ...
        if (selectedItem.isGlass) currentInput.glass = selectedItem.glassType;
        else if (selectedItem.isMethod) currentInput.method = selectedItem.methodType;
        else currentInput.AddIngredient(selectedItem.ingredientId, selectedItem.defaultPourAmount);
    }

    // [중요!] 홈 UI 목업의 '가니쉬/서빙' 단계가 완료되었거나 별도의 '서빙하기' 버튼을 눌렀을 때 호출될 핵심 이벤트
    public void SubmitAndServeCocktail()
    {
        if (activeTargetOrder == null)
        {
            Debug.LogWarning("[Submit] 현재 접수되어 집중 중인 주문이 없습니다.");
            return;
        }

        // 1. 채점 엔진 가동하여 결과 수령 [2, 3]
        CocktailEvaluationEngine.EvaluationResult result = evaluationEngine.EvaluateCocktail(activeTargetOrder.targetRecipe, currentInput);

        if (result.isSuccess)
        {
            // 성공: 점수에 누적 도네이션 상향 적립 [4]
            totalSuperChat += result.earnedDonation;
            UpdateSuperChatUI();

            // 버튜버 아카리 칭찬 대사 연출 트리거 (예: "최고야. 밸런스가 정말 훌륭해.") [6]
            Debug.Log($"[Submit] 조주 성공! 획득 도네이션: {result.earnedDonation} P. 점수: {result.finalScore}");
        }
        else
        {
            // 실패: 아카리 격려/피드백 대사 연출 트리거 (예: "레시피의 비율을 다시 한번 확인해 보자.") [6]
            Debug.Log($"[Submit] 조주 실패. 점수: {result.finalScore}. 도네이션 적립 실패.");
        }

        // 2. 주문 대기열에서 해당 주문 제거 [4]
        orderQueueController.RemoveOrder(activeTargetOrder);

        // 3. 조주 테이블 리셋하여 새 잔 제조 준비
        currentInput.Reset();
    }

    private void UpdateSuperChatUI()
    {
        if (totalSuperChatText != null)
        {
            totalSuperChatText.text = $"{totalSuperChat:N0} P";
        }
    }
}