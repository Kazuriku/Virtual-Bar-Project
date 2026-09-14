using UnityEngine;

[System.Serializable]
public class OrderData
{
    public string customerName;     // 시청자 이름 (예: "시청자1")
    public string donationComment;  // 정중한 후원 사연 대사
    public RecipeSO targetRecipe;   // 목표 칵테일 레시피 데이터 [3]
    public float maxDuration;       // 주어진 제한 시간 (초 단위)
    public float remainingTime;     // 실시간 남은 시간 (초 단위)
    public int donationAmount;      // 칵테일 성공 시 획득할 도네이션 포인트 [3]

    public OrderData(string name, string comment, RecipeSO recipe, float duration)
    {
        customerName = name;
        donationComment = comment;
        targetRecipe = recipe;
        maxDuration = duration;
        remainingTime = duration;
        donationAmount = recipe != null ? recipe.baseDonationPrice : 0; // RecipeSO의 기본 단가 매핑 [3]
    }
}