using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerInputCocktail
{
    public GlassType glass;
    public MethodType method;
    public string garnish = "None";

    private Dictionary<string, float> pouredIngredients = new Dictionary<string, float>();

    // 플레이어가 재료를 부은 순서를 기록하는 리스트
    private List<string> pouredSequence = new List<string>();

    public void AddIngredient(string ingredientId, float amountOz)
    {
        if (pouredIngredients.ContainsKey(ingredientId))
        {
            pouredIngredients[ingredientId] += amountOz;
        }
        else
        {
            pouredIngredients.Add(ingredientId, amountOz);
            pouredSequence.Add(ingredientId); // 최초 투입 시 순서 기록
        }
    }

    public float GetIngredientAmount(string ingredientId)
    {
        if (pouredIngredients.TryGetValue(ingredientId, out float amount))
        {
            return amount;
        }
        return 0f;
    }

    // 투입된 순서 리스트 반환
    public List<string> GetPouredSequence()
    {
        return pouredSequence;
    }

    public void Reset()
    {
        glass = default;
        method = default;
        garnish = "None";
        pouredIngredients.Clear();
        pouredSequence.Clear(); // 리셋 시 순서 리스트도 초기화
        Debug.Log("[PlayerInput] 조주 테이블이 초기화되었습니다.");
    }
}
