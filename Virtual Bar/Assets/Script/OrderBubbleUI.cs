using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class OrderBubbleUI : MonoBehaviour
{
    [Header("UI 컴포넌트 참조")]
    [SerializeField] private TextMeshProUGUI customerNameText;
    [SerializeField] private TextMeshProUGUI commentText;
    [SerializeField] private TextMeshProUGUI donationAmountText;
    [SerializeField] private Image cocktailIcon;
    [SerializeField] private Slider timerSlider;        // 주황색 게이지 바 (UI Slider)
    [SerializeField] private Image timerFillImage;      // 시간에 따라 색상을 오렌지->빨강으로 바꿀 때 사용
    [SerializeField] private GameObject activeBadge;     // 현재 조주 중인지 나타내는 '대기 중' 혹은 '조주 중' 배지

    [Header("기본 비주얼 설정")]
    [SerializeField] private Color normalTimerColor = new Color(0.9f, 0.6f, 0.2f); // 따뜻한 주황색
    [SerializeField] private Color urgentTimerColor = Color.red;                   // 빨간색 (시간 임박)

    private OrderData orderData;
    private Action<OrderData> onSelectedCallback;
    private bool isInitialized = false;

    public OrderData Data => orderData;

    // 데이터를 받아서 UI에 즉시 바인딩
    public void Bind(OrderData data, Action<OrderData> onSelected)
    {
        orderData = data;
        onSelectedCallback = onSelected;

        if (customerNameText != null) customerNameText.text = data.customerName;
        if (commentText != null) commentText.text = data.donationComment;
        if (donationAmountText != null) donationAmountText.text = $"(+{data.donationAmount})";

        // TODO: 칵테일 레시피 정보에 따른 대표 썸네일 스프라이트 세팅
        if (cocktailIcon != null && data.targetRecipe != null)
        {
            // cocktailIcon.sprite = data.targetRecipe.cocktailIcon;
        }

        if (timerSlider != null)
        {
            timerSlider.maxValue = data.maxDuration;
            timerSlider.value = data.remainingTime;
        }

        isInitialized = true;
    }

    // 플레이어가 특정 말풍선을 클릭해 타깃으로 삼았을 때 비주얼 피드백
    public void SetSelectedState(bool isSelected)
    {
        if (activeBadge != null)
        {
            activeBadge.SetActive(isSelected);
        }
    }

    private void Update()
    {
        if (!isInitialized || orderData == null) return;

        // 매 프레임 남은 시간에 따른 게이지 갱신 및 색상 보간(Lerp) 연출
        if (timerSlider != null)
        {
            timerSlider.value = orderData.remainingTime;

            if (timerFillImage != null)
            {
                float ratio = orderData.remainingTime / orderData.maxDuration;
                // 남은 시간이 30% 미만으로 떨어지면 빨갛게 물들기 시작함
                timerFillImage.color = Color.Lerp(urgentTimerColor, normalTimerColor, ratio);
            }
        }
    }

    // 유니티 UI Button 컴포넌트 이벤트에 이 함수를 연결합니다.
    public void OnClickBubble()
    {
        onSelectedCallback?.Invoke(orderData);
    }
}