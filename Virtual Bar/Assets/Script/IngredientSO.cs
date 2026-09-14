using UnityEngine;

// GDD v0.4 분류 기반 카테고리 정의
public enum IngredientCategory
{
    Spirits,         // 1번 탭: 기주 (Brandy, Gin, Vodka 등)
    Liqueurs,        // 2번 탭: 리큐어 (Kahlua, Triple Sec, Midori 등)
    SubIngredients,  // 3번 탭: 부재료 (Wine, Juice, Syrup, Ice, Lemon Slice 등)
    GarnishAndTools  // 4번 탭: 가니쉬 / 기구 및 글라스 (GlassType, MethodType 대응 기구)
}

[CreateAssetMenu(fileName = "Ingredient_", menuName = "Bartender/IngredientSO")]
public class IngredientSO : ScriptableObject
{
    [Header("기본 식별 정보")]
    public string ingredientId;         // GDD 내 정답 매핑 ID (예: "gin_dry", "lemon_wedge", "shaker")
    public string displayNameKo;        // 화면 노출용 한글 이름 (예: "드라이 진", "레몬 웨지")
    public string displayNameEn;        // 영문 이름
    public IngredientCategory category;   // 소속 탭 카테고리
    public Sprite icon;                 // 원형 UI 슬롯에 들어갈 일러스트 스프라이트

    [Header("속성 세부 설정")]
    public float defaultPourAmount = 0.75f; // 클릭 시 기본 추가될 용량 (oz 단위)
    
    [Header("기구 특수 타입 매핑")]
    public bool isGlass;                // 글라스 선택 도구인지 여부
    public GlassType glassType;         // isGlass가 true일 때 대응되는 GlassType
    
    public bool isMethod;               // 조주 기법 활성화 도구인지 여부 (예: 셰이커 클릭)
    public MethodType methodType;       // isMethod가 true일 때 대응되는 MethodType
}