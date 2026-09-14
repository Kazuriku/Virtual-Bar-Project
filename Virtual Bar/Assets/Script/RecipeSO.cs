using System;
using System.Collections.Generic;
using UnityEngine;

public enum MethodType { Shake, Stir, Build, Float, Blend }

public enum GlassType
{
    StemmedLiqueur, CocktailGlass, OldFashioned, FootedPilsner,
    SourGlass, Sherry, Collins, Highball, ChampagneSaucer,
    WhiteWine, FluteChampagne
}

[Serializable]
public struct IngredientRatio
{
    public string ingredientId;  // 예: "gin_dry", "grenadine_syrup"
    public float amountOz;       // 정답 용량 (oz 또는 파트 비율)
    public float allowableError; // 허용 오차
}

[CreateAssetMenu(fileName = "Recipe_", menuName = "Bartender/RecipeSO")]
public class RecipeSO : ScriptableObject
{
    public string recipeId;
    public string cocktailNameKo;
    public string cocktailNameEn;

    [Header("조주 표준 정답")]
    public GlassType requiredGlass;
    public MethodType requiredMethod;
    public string requiredGarnish;

    [Header("순서 판정 설정")]
    public bool isSequenceImportant = false; // 층쌓기 등 순서가 필수인 칵테일 여부

    [Header("필수 재료 목록")]
    public List<IngredientRatio> requiredIngredients = new List<IngredientRatio>();

    [Header("상세 제조 순서 (가이드 및 레시피북 표시용)")]
    [TextArea(2, 5)]
    public List<string> preparationSteps = new List<string>();

    [Header("경제/점수 메타데이터")]
    public int baseDonationPrice; // 기본 도네이션 금액 (P)
}