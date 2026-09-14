#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class RecipeSOAutoGenerator : EditorWindow
{
    [MenuItem("Tools/Bartender/Generate 40 RecipeSOs")]
    public static void GenerateAllRecipes()
    {
        string folderPath = "Assets/Recipes";
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
            AssetDatabase.Refresh();
        }

        List<RecipeDataRaw> recipesData = Get40RecipesData();
        int createdCount = 0;

        foreach (var raw in recipesData)
        {
            string assetPath = $"{folderPath}/Recipe_{raw.recipeId}.asset";
            RecipeSO recipe = AssetDatabase.LoadAssetAtPath<RecipeSO>(assetPath);

            bool isNew = false;
            if (recipe == null)
            {
                recipe = ScriptableObject.CreateInstance<RecipeSO>();
                isNew = true;
            }

            recipe.recipeId = raw.recipeId;
            recipe.cocktailNameKo = raw.cocktailNameKo;
            recipe.cocktailNameEn = raw.cocktailNameEn;
            recipe.requiredGlass = raw.requiredGlass;
            recipe.requiredMethod = raw.requiredMethod;
            recipe.requiredGarnish = raw.requiredGarnish;
            recipe.isSequenceImportant = raw.isSequenceImportant;
            recipe.requiredIngredients = raw.requiredIngredients;
            recipe.preparationSteps = raw.preparationSteps;
            recipe.baseDonationPrice = raw.baseDonationPrice;

            if (isNew)
            {
                AssetDatabase.CreateAsset(recipe, assetPath);
            }
            else
            {
                EditorUtility.SetDirty(recipe);
            }

            createdCount++;
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"[Recipe Generator] 성공! 총 {createdCount}개의 RecipeSO 에셋이 '{folderPath}' 폴더에 새로 생성/갱신되었습니다.");
    }

    private struct RecipeDataRaw
    {
        public string recipeId;
        public string cocktailNameKo;
        public string cocktailNameEn;
        public GlassType requiredGlass;
        public MethodType requiredMethod;
        public string requiredGarnish;
        public bool isSequenceImportant;
        public List<IngredientRatio> requiredIngredients;
        public List<string> preparationSteps;
        public int baseDonationPrice;
    }

    private static List<RecipeDataRaw> Get40RecipesData()
    {
        var list = new List<RecipeDataRaw>();

        // 1. 푸스카페
        list.Add(new RecipeDataRaw
        {
            recipeId = "pousse_cafe",
            cocktailNameKo = "푸스카페",
            cocktailNameEn = "Pousse Cafe",
            requiredGlass = GlassType.StemmedLiqueur,
            requiredMethod = MethodType.Float,
            requiredGarnish = "None",
            isSequenceImportant = true,
            baseDonationPrice = 2000,
            requiredIngredients = new List<IngredientRatio> {
                new IngredientRatio { ingredientId = "grenadine_syrup", amountOz = 0.33f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "creme_de_menthe_green", amountOz = 0.33f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "brandy", amountOz = 0.33f, allowableError = 0.1f }
            },
            preparationSteps = new List<string> {
                "1. 스템 리큐어 글라스를 준비한다.",
                "2. 그레나딘 시럽 1/3 파트(약 1/4oz)를 계량해 잔 바닥에 따른다.",
                "3. 사용한 지거와 바스푼을 물로 씻고 닦는다.",
                "4. 크렘 드 멘트 그린 1/3 파트(약 1/4oz)를 바스푼으로 천천히 플로팅한다.",
                "5. 지거와 바스푼을 다시 씻고 닦는다.",
                "6. 브랜디 1/3 파트(약 1/3oz)를 맨 위에 바스푼으로 플로팅한다."
            }
        });

        // 2. 맨해튼
        list.Add(new RecipeDataRaw
        {
            recipeId = "manhattan",
            cocktailNameKo = "맨해튼",
            cocktailNameEn = "Manhattan",
            requiredGlass = GlassType.CocktailGlass,
            requiredMethod = MethodType.Stir,
            requiredGarnish = "Cherry",
            isSequenceImportant = false,
            baseDonationPrice = 1500,
            requiredIngredients = new List<IngredientRatio> {
                new IngredientRatio { ingredientId = "bourbon_whiskey", amountOz = 1.5f, allowableError = 0.15f },
                new IngredientRatio { ingredientId = "sweet_vermouth", amountOz = 0.75f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "angostura_bitters", amountOz = 0.05f, allowableError = 0.02f }
            },
            preparationSteps = new List<string> {
                "1. 칵테일 글라스에 얼음을 넣고 칠링한다.",
                "2. 믹싱 글라스에 얼음을 8~10개 넣는다.",
                "3. 버번 위스키 1 1/2oz, 스위트 베르무스 3/4oz, 앙고스트라 비터 1 Dash를 순서대로 넣는다.",
                "4. 바스푼으로 글라스 벽을 따라 6~8회 저어준다.",
                "5. 칠링한 잔의 얼음을 버린다.",
                "6. 믹싱 글라스에 스트레이너를 끼우고 잔에 따른다.",
                "7. 체리를 픽에 꽂아 잔 안에 넣어 마무리한다."
            }
        });

        // 3. 드라이 마티니
        list.Add(new RecipeDataRaw
        {
            recipeId = "dry_martini",
            cocktailNameKo = "드라이 마티니",
            cocktailNameEn = "Dry Martini",
            requiredGlass = GlassType.CocktailGlass,
            requiredMethod = MethodType.Stir,
            requiredGarnish = "Green Olive",
            isSequenceImportant = false,
            baseDonationPrice = 1500,
            requiredIngredients = new List<IngredientRatio> {
                new IngredientRatio { ingredientId = "dry_gin", amountOz = 2.0f, allowableError = 0.2f },
                new IngredientRatio { ingredientId = "dry_vermouth", amountOz = 0.33f, allowableError = 0.1f }
            },
            preparationSteps = new List<string> {
                "1. 칵테일 글라스에 얼음을 넣어 칠링한다.",
                "2. 믹싱 글라스에 얼음을 8~10개 넣는다.",
                "3. 드라이 진 2oz, 드라이 베르무스 1/3oz를 넣는다.",
                "4. 바스푼으로 글라스 벽을 따라 6~8회 저어준다.",
                "5. 칠링한 잔의 얼음을 버린다.",
                "6. 스트레이너를 끼우고 잔에 따른다.",
                "7. 그린 올리브를 픽에 꽂아 잔 안에 넣어 마무리한다."
            }
        });

        // 4. 올드 패션드
        list.Add(new RecipeDataRaw
        {
            recipeId = "old_fashioned",
            cocktailNameKo = "올드 패션드",
            cocktailNameEn = "Old Fashioned",
            requiredGlass = GlassType.OldFashioned,
            requiredMethod = MethodType.Build,
            requiredGarnish = "A Slice of Orange & Cherry",
            isSequenceImportant = true,
            baseDonationPrice = 1500,
            requiredIngredients = new List<IngredientRatio> {
                new IngredientRatio { ingredientId = "powdered_sugar", amountOz = 0.1f, allowableError = 0.05f },
                new IngredientRatio { ingredientId = "angostura_bitters", amountOz = 0.05f, allowableError = 0.02f },
                new IngredientRatio { ingredientId = "soda_water", amountOz = 0.5f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "bourbon_whiskey", amountOz = 1.5f, allowableError = 0.15f }
            },
            preparationSteps = new List<string> {
                "1. 올드 패션드 글라스에 설탕 1tsp, 앙고스트라 비터 1 Dash, 소다수 1/2oz를 넣고 바스푼으로 10회 저어 녹인다.",
                "2. 글라스에 얼음을 5~6개 넣는다.",
                "3. 버번 위스키 1 1/2oz를 넣고 바스푼으로 스티어한다.",
                "4. 오렌지 슬라이스와 체리를 픽에 함께 꽂아 잔 림에 장식한다."
            }
        });

        // 5. 브랜디 알렉산더
        list.Add(new RecipeDataRaw
        {
            recipeId = "brandy_alexander",
            cocktailNameKo = "브랜디 알렉산더",
            cocktailNameEn = "Brandy Alexander",
            requiredGlass = GlassType.CocktailGlass,
            requiredMethod = MethodType.Shake,
            requiredGarnish = "Nutmeg Powder",
            isSequenceImportant = false,
            baseDonationPrice = 1200,
            requiredIngredients = new List<IngredientRatio> {
                new IngredientRatio { ingredientId = "brandy", amountOz = 0.75f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "creme_de_cacao_brown", amountOz = 0.75f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "light_milk", amountOz = 0.75f, allowableError = 0.1f }
            },
            preparationSteps = new List<string> {
                "1. 칵테일 글라스에 얼음을 넣어 칠링한다.",
                "2. 셰이커에 얼음을 8~10개 넣는다.",
                "3. 브랜디 3/4oz, 크렘 드 카카오 브라운 3/4oz, 우유 3/4oz를 넣는다.",
                "4. 스트레이너, 캡 순으로 닫고 8~10회 셰이킹한다.",
                "5. 칠링한 잔의 얼음을 버리고 셰이커 캡만 열어 잔에 따른다.",
                "6. 너트멕 파우더를 위에 톡 뿌려 마무리한다."
            }
        });

        // 6. 싱가포르 슬링
        list.Add(new RecipeDataRaw
        {
            recipeId = "singapore_sling",
            cocktailNameKo = "싱가포르 슬링",
            cocktailNameEn = "Singapore Sling",
            requiredGlass = GlassType.FootedPilsner,
            requiredMethod = MethodType.Shake,
            requiredGarnish = "A Slice of Orange & Cherry",
            isSequenceImportant = true,
            baseDonationPrice = 1800,
            requiredIngredients = new List<IngredientRatio> {
                new IngredientRatio { ingredientId = "dry_gin", amountOz = 1.5f, allowableError = 0.15f },
                new IngredientRatio { ingredientId = "lemon_juice", amountOz = 1.0f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "powdered_sugar", amountOz = 0.1f, allowableError = 0.05f },
                new IngredientRatio { ingredientId = "soda_water", amountOz = 2.5f, allowableError = 0.3f },
                new IngredientRatio { ingredientId = "cherry_flavored_brandy", amountOz = 0.5f, allowableError = 0.1f }
            },
            preparationSteps = new List<string> {
                "1. 풋 필스너 글라스에 얼음을 넣어 칠링한다 (얼음 버리지 않음).",
                "2. 셰이커에 얼음, 드라이 진 1 1/2oz, 레몬 주스 1oz, 설탕 1tsp를 넣고 저은 뒤 8~10회 셰이킹한다.",
                "3. 칠링한 잔의 얼음을 버리지 않고 셰이커 캡만 열어 잔에 따른다.",
                "4. 소다수를 잔의 80~90% 채우고 살짝 저어준다.",
                "5. 체리 브랜디 1/2oz를 바스푼으로 플로팅한다.",
                "6. 오렌지 슬라이스와 체리를 픽에 꽂아 잔 림에 장식한다."
            }
        });

        // 7. 블랙 러시안
        list.Add(new RecipeDataRaw
        {
            recipeId = "black_russian",
            cocktailNameKo = "블랙 러시안",
            cocktailNameEn = "Black Russian",
            requiredGlass = GlassType.OldFashioned,
            requiredMethod = MethodType.Build,
            requiredGarnish = "None",
            isSequenceImportant = false,
            baseDonationPrice = 1000,
            requiredIngredients = new List<IngredientRatio> {
                new IngredientRatio { ingredientId = "vodka", amountOz = 1.0f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "kahlua", amountOz = 1.0f, allowableError = 0.1f }
            },
            preparationSteps = new List<string> {
                "1. 올드 패션드 글라스에 얼음을 가득(5~6개) 넣는다.",
                "2. 보드카 1oz, 깔루아 1oz를 넣는다.",
                "3. 바스푼으로 8~10회 저어 스티어한다."
            }
        });

        // 8. 마가리타
        list.Add(new RecipeDataRaw
        {
            recipeId = "margarita",
            cocktailNameKo = "마가리타",
            cocktailNameEn = "Margarita",
            requiredGlass = GlassType.CocktailGlass,
            requiredMethod = MethodType.Shake,
            requiredGarnish = "Rimming with Salt",
            isSequenceImportant = false,
            baseDonationPrice = 1500,
            requiredIngredients = new List<IngredientRatio> {
                new IngredientRatio { ingredientId = "tequila", amountOz = 1.5f, allowableError = 0.15f },
                new IngredientRatio { ingredientId = "triple_sec", amountOz = 0.5f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "lime_juice", amountOz = 0.5f, allowableError = 0.1f }
            },
            preparationSteps = new List<string> {
                "1. 칵테일 글라스에 얼음을 넣어 칠링한다.",
                "2. 셰이커에 얼음, 테킬라 1 1/2oz, 트리플 색 1/2oz, 라임 주스 1/2oz를 넣는다.",
                "3. 8~10회 셰이킹한다.",
                "4. 칠링한 잔의 얼음을 버리고 레몬 즙을 림에 바른 후 글라스 리머로 소금을 묻힌다.",
                "5. 셰이커 캡만 열어 잔에 따라 제출한다."
            }
        });

        // 9. 러스티 네일
        list.Add(new RecipeDataRaw
        {
            recipeId = "rusty_nail",
            cocktailNameKo = "러스티 네일",
            cocktailNameEn = "Rusty Nail",
            requiredGlass = GlassType.OldFashioned,
            requiredMethod = MethodType.Build,
            requiredGarnish = "None",
            isSequenceImportant = false,
            baseDonationPrice = 1200,
            requiredIngredients = new List<IngredientRatio> {
                new IngredientRatio { ingredientId = "scotch_whisky", amountOz = 1.0f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "drambuie", amountOz = 1.0f, allowableError = 0.1f }
            },
            preparationSteps = new List<string> {
                "1. 올드 패션드 글라스에 얼음을 가득(5~6개) 넣는다.",
                "2. 스카치 위스키 1oz, 드람부이 1oz를 넣는다.",
                "3. 바스푼으로 8~10회 저어 스티어한다."
            }
        });

        // 10. 위스키 사워
        list.Add(new RecipeDataRaw
        {
            recipeId = "whiskey_sour",
            cocktailNameKo = "위스키 사워",
            cocktailNameEn = "Whiskey Sour",
            requiredGlass = GlassType.SourGlass,
            requiredMethod = MethodType.Shake,
            requiredGarnish = "A Slice of Lemon & Cherry",
            isSequenceImportant = false,
            baseDonationPrice = 1500,
            requiredIngredients = new List<IngredientRatio> {
                new IngredientRatio { ingredientId = "bourbon_whiskey", amountOz = 1.5f, allowableError = 0.15f },
                new IngredientRatio { ingredientId = "lemon_juice", amountOz = 0.5f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "powdered_sugar", amountOz = 0.1f, allowableError = 0.05f },
                new IngredientRatio { ingredientId = "soda_water", amountOz = 1.0f, allowableError = 0.1f }
            },
            preparationSteps = new List<string> {
                "1. 사워 글라스에 얼음을 넣어 칠링한다.",
                "2. 셰이커에 얼음, 버번 위스키 1 1/2oz, 레몬 주스 1/2oz, 설탕 1tsp를 넣고 살짝 저은 뒤 8~10회 셰이킹한다.",
                "3. 칠링한 잔의 얼음을 버리고 셰이커 캡만 열어 잔에 따른다.",
                "4. 소다수 1oz를 넣고 살짝 저어준다.",
                "5. 레몬 슬라이스와 체리를 픽에 꽂아 잔 림에 장식한다."
            }
        });

        // 11. 뉴욕
        list.Add(new RecipeDataRaw
        {
            recipeId = "new_york",
            cocktailNameKo = "뉴욕",
            cocktailNameEn = "New York",
            requiredGlass = GlassType.CocktailGlass,
            requiredMethod = MethodType.Shake,
            requiredGarnish = "Twist of Lemon Peel",
            isSequenceImportant = false,
            baseDonationPrice = 1400,
            requiredIngredients = new List<IngredientRatio> {
                new IngredientRatio { ingredientId = "bourbon_whiskey", amountOz = 1.5f, allowableError = 0.15f },
                new IngredientRatio { ingredientId = "lime_juice", amountOz = 0.5f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "powdered_sugar", amountOz = 0.1f, allowableError = 0.05f },
                new IngredientRatio { ingredientId = "grenadine_syrup", amountOz = 0.05f, allowableError = 0.02f }
            },
            preparationSteps = new List<string> {
                "1. 칵테일 글라스에 얼음을 넣어 칠링한다.",
                "2. 셰이커에 얼음, 버번 위스키 1 1/2oz, 라임 주스 1/2oz, 설탕 1tsp, 그레나딘 시럽 1/2tsp를 넣고 저어준다.",
                "3. 8~10회 셰이킹한다.",
                "4. 칠링한 잔의 얼음을 버리고 셰이커 캡만 열어 잔에 따른다.",
                "5. 레몬 껍질을 트위스트하여 잔 안에 넣는다."
            }
        });

        // 12. 다이키리
        list.Add(new RecipeDataRaw
        {
            recipeId = "daiquiri",
            cocktailNameKo = "다이키리",
            cocktailNameEn = "Daiquiri",
            requiredGlass = GlassType.CocktailGlass,
            requiredMethod = MethodType.Shake,
            requiredGarnish = "None",
            isSequenceImportant = false,
            baseDonationPrice = 1300,
            requiredIngredients = new List<IngredientRatio> {
                new IngredientRatio { ingredientId = "light_rum", amountOz = 1.75f, allowableError = 0.15f },
                new IngredientRatio { ingredientId = "lime_juice", amountOz = 0.75f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "powdered_sugar", amountOz = 0.1f, allowableError = 0.05f }
            },
            preparationSteps = new List<string> {
                "1. 칵테일 글라스에 얼음을 넣어 칠링한다.",
                "2. 셰이커에 얼음, 라이트 럼 1 3/4oz, 라임 주스 3/4oz, 설탕 1tsp를 넣고 저어준다.",
                "3. 8~10회 셰이킹한다.",
                "4. 칠링한 잔의 얼음을 버리고 잔에 따른다."
            }
        });

        // 13. B-52
        list.Add(new RecipeDataRaw
        {
            recipeId = "b_52",
            cocktailNameKo = "B-52",
            cocktailNameEn = "B-52",
            requiredGlass = GlassType.Sherry,
            requiredMethod = MethodType.Float,
            requiredGarnish = "None",
            isSequenceImportant = true,
            baseDonationPrice = 2000,
            requiredIngredients = new List<IngredientRatio> {
                new IngredientRatio { ingredientId = "kahlua", amountOz = 0.33f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "baileys_irish_cream", amountOz = 0.5f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "grand_marnier", amountOz = 0.75f, allowableError = 0.1f }
            },
            preparationSteps = new List<string> {
                "1. 셰리 글라스를 준비한다.",
                "2. 깔루아 1/3 파트(약 1/3oz)를 잔 바닥에 따른다.",
                "3. 지거와 바스푼을 씻고 닦은 후, 베일리스 아이리시 크림 1/3 파트(약 1/2oz)를 플로팅한다.",
                "4. 지거와 바스푼을 다시 씻고 닦은 후, 그랑 마니에 1/3 파트(약 3/4oz)를 플로팅한다."
            }
        });

        // 14. 준벅
        list.Add(new RecipeDataRaw
        {
            recipeId = "june_bug",
            cocktailNameKo = "준벅",
            cocktailNameEn = "June Bug",
            requiredGlass = GlassType.Collins,
            requiredMethod = MethodType.Shake,
            requiredGarnish = "A Wedge of Pineapple & Cherry",
            isSequenceImportant = false,
            baseDonationPrice = 1800,
            requiredIngredients = new List<IngredientRatio> {
                new IngredientRatio { ingredientId = "midori", amountOz = 1.0f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "malibu", amountOz = 1.0f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "banana_liqueur", amountOz = 1.0f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "pineapple_juice", amountOz = 2.0f, allowableError = 0.2f },
                new IngredientRatio { ingredientId = "sweet_sour_mix", amountOz = 2.0f, allowableError = 0.2f }
            },
            preparationSteps = new List<string> {
                "1. 콜린스 글라스에 얼음을 가득 넣어 칠링한다 (얼음 버리지 않음).",
                "2. 셰이커에 얼음, 미도리 1oz, 말리부 1oz, 바나나 리큐르 1oz, 파인애플 주스 2oz, 스위트 앤 사워 믹스 2oz를 순서대로 넣는다.",
                "3. 8~10회 셰이킹한다.",
                "4. 칠링한 잔의 얼음을 버리지 않고 셰이커 캡만 열어 잔에 따른다.",
                "5. 파인애플 웨지와 체리를 픽에 꽂아 잔 림에 장식한다."
            }
        });

        // 15. 바카디 칵테일
        list.Add(new RecipeDataRaw
        {
            recipeId = "bacardi_cocktail",
            cocktailNameKo = "바카디 칵테일",
            cocktailNameEn = "Bacardi Cocktail",
            requiredGlass = GlassType.CocktailGlass,
            requiredMethod = MethodType.Shake,
            requiredGarnish = "None",
            isSequenceImportant = false,
            baseDonationPrice = 1400,
            requiredIngredients = new List<IngredientRatio> {
                new IngredientRatio { ingredientId = "bacardi_rum_white", amountOz = 1.75f, allowableError = 0.15f },
                new IngredientRatio { ingredientId = "lime_juice", amountOz = 0.75f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "grenadine_syrup", amountOz = 0.1f, allowableError = 0.05f }
            },
            preparationSteps = new List<string> {
                "1. 칵테일 글라스에 얼음을 넣어 칠링한다.",
                "2. 셰이커에 얼음, 바카디 럼 1 3/4oz, 라임 주스 3/4oz, 그레나딘 시럽 1tsp를 넣고 저어준다.",
                "3. 8~10회 셰이킹한다.",
                "4. 칠링한 잔의 얼음을 버리고 잔에 따른다."
            }
        });

        // 16. 쿠바 리브레
        list.Add(new RecipeDataRaw
        {
            recipeId = "cuba_libre",
            cocktailNameKo = "쿠바 리브레",
            cocktailNameEn = "Cuba Libre",
            requiredGlass = GlassType.Highball,
            requiredMethod = MethodType.Build,
            requiredGarnish = "A Wedge of Lemon",
            isSequenceImportant = false,
            baseDonationPrice = 1200,
            requiredIngredients = new List<IngredientRatio> {
                new IngredientRatio { ingredientId = "light_rum", amountOz = 1.5f, allowableError = 0.15f },
                new IngredientRatio { ingredientId = "lime_juice", amountOz = 0.5f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "cola", amountOz = 3.0f, allowableError = 0.3f }
            },
            preparationSteps = new List<string> {
                "1. 하이볼 글라스에 얼음을 가득 채운다.",
                "2. 라이트 럼 1 1/2oz, 라임 주스 1/2oz를 넣는다.",
                "3. 콜라를 80~90% 채우고 바스푼으로 8~10회 저어준다.",
                "4. 레몬 웨지를 잔 림에 장식한다."
            }
        });

        // 17. 그래스호퍼
        list.Add(new RecipeDataRaw
        {
            recipeId = "grasshopper",
            cocktailNameKo = "그래스호퍼",
            cocktailNameEn = "Grasshopper",
            requiredGlass = GlassType.ChampagneSaucer,
            requiredMethod = MethodType.Shake,
            requiredGarnish = "None",
            isSequenceImportant = false,
            baseDonationPrice = 1300,
            requiredIngredients = new List<IngredientRatio> {
                new IngredientRatio { ingredientId = "creme_de_menthe_green", amountOz = 1.0f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "creme_de_cacao_white", amountOz = 1.0f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "light_milk", amountOz = 1.0f, allowableError = 0.1f }
            },
            preparationSteps = new List<string> {
                "1. 소서 샴페인 글라스에 얼음을 넣어 칠링한다.",
                "2. 셰이커에 얼음, 크렘 드 멘트 그린 1oz, 크렘 드 카카오 화이트 1oz, 우유 1oz를 넣는다.",
                "3. 8~10회 셰이킹한다.",
                "4. 칠링한 잔의 얼음을 버리고 잔에 따른다."
            }
        });

        // 18. 시브리즈
        list.Add(new RecipeDataRaw
        {
            recipeId = "seabreeze",
            cocktailNameKo = "시브리즈",
            cocktailNameEn = "Seabreeze",
            requiredGlass = GlassType.Highball,
            requiredMethod = MethodType.Build,
            requiredGarnish = "A Wedge of Lemon",
            isSequenceImportant = false,
            baseDonationPrice = 1200,
            requiredIngredients = new List<IngredientRatio> {
                new IngredientRatio { ingredientId = "vodka", amountOz = 1.5f, allowableError = 0.15f },
                new IngredientRatio { ingredientId = "cranberry_juice", amountOz = 3.0f, allowableError = 0.3f },
                new IngredientRatio { ingredientId = "grapefruit_juice", amountOz = 0.5f, allowableError = 0.1f }
            },
            preparationSteps = new List<string> {
                "1. 하이볼 글라스에 얼음을 가득 채운다.",
                "2. 보드카 1 1/2oz, 크랜베리 주스 3oz, 자몽 주스 1/2oz를 넣는다.",
                "3. 바스푼으로 8~10회 저어준다.",
                "4. 레몬 웨지를 잔 림에 장식한다."
            }
        });

        // 19. 애플 마티니
        list.Add(new RecipeDataRaw
        {
            recipeId = "apple_martini",
            cocktailNameKo = "애플 마티니",
            cocktailNameEn = "Apple Martini",
            requiredGlass = GlassType.CocktailGlass,
            requiredMethod = MethodType.Shake,
            requiredGarnish = "A Slice of Apple",
            isSequenceImportant = false,
            baseDonationPrice = 1400,
            requiredIngredients = new List<IngredientRatio> {
                new IngredientRatio { ingredientId = "vodka", amountOz = 1.0f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "apple_pucker", amountOz = 1.0f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "lime_juice", amountOz = 0.5f, allowableError = 0.1f }
            },
            preparationSteps = new List<string> {
                "1. 칵테일 글라스에 얼음을 넣어 칠링한다.",
                "2. 셰이커에 얼음, 보드카 1oz, 애플 퍼커 1oz, 라임 주스 1/2oz를 넣는다.",
                "3. 8~10회 셰이킹한다.",
                "4. 칠링한 잔의 얼음을 버리고 잔에 따른다.",
                "5. 사과 슬라이스를 잔 림에 장식한다."
            }
        });

        // 20. 네그로니
        list.Add(new RecipeDataRaw
        {
            recipeId = "negroni",
            cocktailNameKo = "네그로니",
            cocktailNameEn = "Negroni",
            requiredGlass = GlassType.OldFashioned,
            requiredMethod = MethodType.Build,
            requiredGarnish = "Twist of Lemon Peel",
            isSequenceImportant = false,
            baseDonationPrice = 1500,
            requiredIngredients = new List<IngredientRatio> {
                new IngredientRatio { ingredientId = "dry_gin", amountOz = 0.75f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "sweet_vermouth", amountOz = 0.75f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "campari", amountOz = 0.75f, allowableError = 0.1f }
            },
            preparationSteps = new List<string> {
                "1. 올드 패션드 글라스에 얼음을 넣고 칠링한다.",
                "2. 드라이 진 3/4oz, 스위트 베르무스 3/4oz, 캄파리 3/4oz를 넣는다.",
                "3. 바스푼으로 글라스 벽을 따라 8~10회 저어준다.",
                "4. 레몬 껍질을 트위스트하여 잔에 넣는다."
            }
        });

        // 21. 롱아일랜드 아이스티
        list.Add(new RecipeDataRaw
        {
            recipeId = "long_island_iced_tea",
            cocktailNameKo = "롱아일랜드 아이스티",
            cocktailNameEn = "Long Island Iced Tea",
            requiredGlass = GlassType.Collins,
            requiredMethod = MethodType.Build,
            requiredGarnish = "A Wedge of Lemon",
            isSequenceImportant = false,
            baseDonationPrice = 2200,
            requiredIngredients = new List<IngredientRatio> {
                new IngredientRatio { ingredientId = "dry_gin", amountOz = 0.5f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "vodka", amountOz = 0.5f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "light_rum", amountOz = 0.5f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "tequila", amountOz = 0.5f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "triple_sec", amountOz = 0.5f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "sweet_sour_mix", amountOz = 1.5f, allowableError = 0.15f },
                new IngredientRatio { ingredientId = "cola", amountOz = 1.5f, allowableError = 0.2f }
            },
            preparationSteps = new List<string> {
                "1. 콜린스 글라스에 얼음을 가득 채운다.",
                "2. 드라이 진, 보드카, 라이트 럼, 테킬라, 트리플 색을 각 1/2oz씩 넣는다.",
                "3. 스위트 앤 사워 믹스 1 1/2oz를 넣고 바스푼으로 8~10회 저어준다.",
                "4. 콜라로 80~90% 채운 뒤 살짝(2~3회) 저어준다.",
                "5. 레몬 웨지를 잔 림에 장식한다."
            }
        });

        // 22. 사이드카
        list.Add(new RecipeDataRaw
        {
            recipeId = "sidecar",
            cocktailNameKo = "사이드카",
            cocktailNameEn = "Sidecar",
            requiredGlass = GlassType.CocktailGlass,
            requiredMethod = MethodType.Shake,
            requiredGarnish = "None",
            isSequenceImportant = false,
            baseDonationPrice = 1400,
            requiredIngredients = new List<IngredientRatio> {
                new IngredientRatio { ingredientId = "brandy", amountOz = 1.5f, allowableError = 0.15f },
                new IngredientRatio { ingredientId = "triple_sec", amountOz = 0.75f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "lemon_juice", amountOz = 0.25f, allowableError = 0.05f }
            },
            preparationSteps = new List<string> {
                "1. 칵테일 글라스에 얼음을 넣어 칠링한다.",
                "2. 셰이커에 얼음, 브랜디 1 1/2oz, 트리플 색 3/4oz, 레몬 주스 1/4oz를 넣는다.",
                "3. 8~10회 셰이킹한다.",
                "4. 칠링한 잔의 얼음을 버리고 잔에 따른다."
            }
        });

        // 23. 마이타이
        list.Add(new RecipeDataRaw
        {
            recipeId = "mai_tai",
            cocktailNameKo = "마이타이",
            cocktailNameEn = "Mai-Tai",
            requiredGlass = GlassType.FootedPilsner,
            requiredMethod = MethodType.Blend,
            requiredGarnish = "A Wedge of Pineapple & Cherry",
            isSequenceImportant = false,
            baseDonationPrice = 2000,
            requiredIngredients = new List<IngredientRatio> {
                new IngredientRatio { ingredientId = "light_rum", amountOz = 1.25f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "triple_sec", amountOz = 0.75f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "lime_juice", amountOz = 1.0f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "pineapple_juice", amountOz = 1.0f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "orange_juice", amountOz = 1.0f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "grenadine_syrup", amountOz = 0.25f, allowableError = 0.05f }
            },
            preparationSteps = new List<string> {
                "1. 풋 필스너 글라스에 얼음을 넣어 칠링한다.",
                "2. 블렌더 용기에 라이트 럼 1 1/4oz, 트리플 색 3/4oz, 라임 주스 1oz, 파인애플 주스 1oz, 오렌지 주스 1oz, 그레나딘 시럽 1/4oz를 넣는다.",
                "3. 크러시드 아이스를 넣고 10초간 갈아준다.",
                "4. 칠링한 잔의 얼음을 버리고 따른다.",
                "5. 파인애플 웨지와 체리를 픽에 꽂아 잔 림에 장식한다."
            }
        });

        // 24. 피나콜라다
        list.Add(new RecipeDataRaw
        {
            recipeId = "pina_colada",
            cocktailNameKo = "피나콜라다",
            cocktailNameEn = "Pina Colada",
            requiredGlass = GlassType.FootedPilsner,
            requiredMethod = MethodType.Blend,
            requiredGarnish = "A Wedge of Pineapple & Cherry",
            isSequenceImportant = false,
            baseDonationPrice = 1800,
            requiredIngredients = new List<IngredientRatio> {
                new IngredientRatio { ingredientId = "light_rum", amountOz = 1.25f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "pina_colada_mix", amountOz = 2.0f, allowableError = 0.2f },
                new IngredientRatio { ingredientId = "pineapple_juice", amountOz = 3.0f, allowableError = 0.3f }
            },
            preparationSteps = new List<string> {
                "1. 풋 필스너 글라스에 얼음을 넣어 칠링한다.",
                "2. 블렌더에 라이트 럼 1 1/4oz, 피나콜라다 믹스 2oz, 파인애플 주스 3oz를 넣는다.",
                "3. 크러시드 아이스 1스쿱을 넣고 10초간 갈아준다.",
                "4. 칠링한 잔의 얼음을 버리고 따른다.",
                "5. 파인애플 웨지와 체리를 픽에 꽂아 장식한다."
            }
        });

        // 25. 코스모폴리탄
        list.Add(new RecipeDataRaw
        {
            recipeId = "cosmopolitan",
            cocktailNameKo = "코스모폴리탄",
            cocktailNameEn = "Cosmopolitan",
            requiredGlass = GlassType.CocktailGlass,
            requiredMethod = MethodType.Shake,
            requiredGarnish = "Twist of Lemon Peel",
            isSequenceImportant = false,
            baseDonationPrice = 1500,
            requiredIngredients = new List<IngredientRatio> {
                new IngredientRatio { ingredientId = "vodka", amountOz = 1.0f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "triple_sec", amountOz = 0.5f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "lime_juice", amountOz = 0.5f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "cranberry_juice", amountOz = 0.5f, allowableError = 0.1f }
            },
            preparationSteps = new List<string> {
                "1. 칵테일 글라스에 얼음을 넣어 칠링한다.",
                "2. 셰이커에 얼음, 보드카 1oz, 트리플 색 1/2oz, 라임 주스 1/2oz, 크랜베리 주스 1/2oz를 넣는다.",
                "3. 8~10회 셰이킹한다.",
                "4. 칠링한 잔의 얼음을 버리고 따른다.",
                "5. 레몬 껍질을 트위스트하여 잔 안에 넣는다."
            }
        });

        // 26. 모스코 뮬
        list.Add(new RecipeDataRaw
        {
            recipeId = "moscow_mule",
            cocktailNameKo = "모스코 뮬",
            cocktailNameEn = "Moscow Mule",
            requiredGlass = GlassType.Highball,
            requiredMethod = MethodType.Build,
            requiredGarnish = "A Slice of Lemon",
            isSequenceImportant = false,
            baseDonationPrice = 1200,
            requiredIngredients = new List<IngredientRatio> {
                new IngredientRatio { ingredientId = "vodka", amountOz = 1.5f, allowableError = 0.15f },
                new IngredientRatio { ingredientId = "lime_juice", amountOz = 0.5f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "ginger_ale", amountOz = 3.0f, allowableError = 0.3f }
            },
            preparationSteps = new List<string> {
                "1. 하이볼 글라스에 얼음을 가득 넣는다.",
                "2. 보드카 1 1/2oz, 라임 주스 1/2oz를 넣는다.",
                "3. 진저에일을 80~90% 채우고 4~5회 살짝 저어준다.",
                "4. 레몬 슬라이스를 잔 림에 장식한다."
            }
        });

        // 27. 애프리콧 칵테일
        list.Add(new RecipeDataRaw
        {
            recipeId = "apricot_cocktail",
            cocktailNameKo = "애프리콧 칵테일",
            cocktailNameEn = "Apricot Cocktail",
            requiredGlass = GlassType.CocktailGlass,
            requiredMethod = MethodType.Shake,
            requiredGarnish = "None",
            isSequenceImportant = false,
            baseDonationPrice = 1400,
            requiredIngredients = new List<IngredientRatio> {
                new IngredientRatio { ingredientId = "apricot_flavored_brandy", amountOz = 1.5f, allowableError = 0.15f },
                new IngredientRatio { ingredientId = "dry_gin", amountOz = 0.1f, allowableError = 0.05f },
                new IngredientRatio { ingredientId = "lemon_juice", amountOz = 0.5f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "orange_juice", amountOz = 0.5f, allowableError = 0.1f }
            },
            preparationSteps = new List<string> {
                "1. 칵테일 글라스에 얼음을 넣어 칠링한다.",
                "2. 셰이커에 얼음, 살구 브랜디 1 1/2oz, 드라이 진 1tsp, 레몬 주스 1/2oz, 오렌지 주스 1/2oz를 넣는다.",
                "3. 8~10회 셰이킹한다.",
                "4. 칠링한 잔의 얼음을 버리고 잔에 따른다."
            }
        });

        // 28. 허니문 칵테일
        list.Add(new RecipeDataRaw
        {
            recipeId = "honeymoon_cocktail",
            cocktailNameKo = "허니문 칵테일",
            cocktailNameEn = "Honeymoon Cocktail",
            requiredGlass = GlassType.CocktailGlass,
            requiredMethod = MethodType.Shake,
            requiredGarnish = "None",
            isSequenceImportant = false,
            baseDonationPrice = 1600,
            requiredIngredients = new List<IngredientRatio> {
                new IngredientRatio { ingredientId = "apple_brandy", amountOz = 0.75f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "benedictine_dom", amountOz = 0.75f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "triple_sec", amountOz = 0.25f, allowableError = 0.05f },
                new IngredientRatio { ingredientId = "lemon_juice", amountOz = 0.5f, allowableError = 0.1f }
            },
            preparationSteps = new List<string> {
                "1. 칵테일 글라스에 얼음을 넣어 칠링한다.",
                "2. 셰이커에 얼음, 칼바도스 3/4oz, 베네딕틴 DOM 3/4oz, 트리플 색 1/4oz, 레몬 주스 1/2oz를 넣는다.",
                "3. 8~10회 셰이킹한다.",
                "4. 칠링한 잔의 얼음을 버리고 잔에 따른다."
            }
        });

        // 29. 블루 하와이안
        list.Add(new RecipeDataRaw
        {
            recipeId = "blue_hawaiian",
            cocktailNameKo = "블루 하와이안",
            cocktailNameEn = "Blue Hawaiian",
            requiredGlass = GlassType.FootedPilsner,
            requiredMethod = MethodType.Blend,
            requiredGarnish = "A Wedge of Pineapple & Cherry",
            isSequenceImportant = false,
            baseDonationPrice = 1800,
            requiredIngredients = new List<IngredientRatio> {
                new IngredientRatio { ingredientId = "light_rum", amountOz = 1.0f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "blue_curacao", amountOz = 1.0f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "coconut_flavored_rum", amountOz = 1.0f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "pineapple_juice", amountOz = 2.5f, allowableError = 0.25f }
            },
            preparationSteps = new List<string> {
                "1. 풋 필스너 글라스에 얼음을 넣어 칠링한다.",
                "2. 블렌더에 라이트 럼 1oz, 블루 큐라소 1oz, 말리부 1oz, 파인애플 주스 2 1/2oz를 넣는다.",
                "3. 크러시드 아이스 1스쿱을 넣고 10초간 갈아준다.",
                "4. 칠링한 잔의 얼음을 버리고 따른다.",
                "5. 파인애플 웨지와 체리를 픽에 꽂아 장식한다."
            }
        });

        // 30. 키르
        list.Add(new RecipeDataRaw
        {
            recipeId = "kir",
            cocktailNameKo = "키르",
            cocktailNameEn = "Kir",
            requiredGlass = GlassType.WhiteWine,
            requiredMethod = MethodType.Build,
            requiredGarnish = "Twist of Lemon Peel",
            isSequenceImportant = false,
            baseDonationPrice = 1200,
            requiredIngredients = new List<IngredientRatio> {
                new IngredientRatio { ingredientId = "white_wine", amountOz = 3.0f, allowableError = 0.3f },
                new IngredientRatio { ingredientId = "creme_de_cassis", amountOz = 0.5f, allowableError = 0.1f }
            },
            preparationSteps = new List<string> {
                "1. 화이트 와인 글라스를 준비한다 (얼음 칠링 하지 않음).",
                "2. 화이트 와인 3oz, 크렘 드 카시스 1/2oz를 직접 붓는다.",
                "3. 바스푼으로 살짝 저어준다.",
                "4. 레몬 껍질을 트위스트하여 잔에 넣는다."
            }
        });

        // 31. 테킬라 선라이즈
        list.Add(new RecipeDataRaw
        {
            recipeId = "tequila_sunrise",
            cocktailNameKo = "테킬라 선라이즈",
            cocktailNameEn = "Tequila Sunrise",
            requiredGlass = GlassType.FootedPilsner,
            requiredMethod = MethodType.Build,
            requiredGarnish = "None",
            isSequenceImportant = true,
            baseDonationPrice = 1500,
            requiredIngredients = new List<IngredientRatio> {
                new IngredientRatio { ingredientId = "tequila", amountOz = 1.5f, allowableError = 0.15f },
                new IngredientRatio { ingredientId = "orange_juice", amountOz = 3.0f, allowableError = 0.3f },
                new IngredientRatio { ingredientId = "grenadine_syrup", amountOz = 0.5f, allowableError = 0.1f }
            },
            preparationSteps = new List<string> {
                "1. 풋 필스너 글라스에 얼음을 6~8개 넣는다.",
                "2. 테킬라 1 1/2oz를 넣고 오렌지 주스를 80~90% 채운 뒤 바스푼으로 저어준다.",
                "3. 그레나딘 시럽 1/2oz를 바스푼을 이용해 중앙으로 천천히 플로팅(가라앉힘)한다."
            }
        });

        // 32. 힐링
        list.Add(new RecipeDataRaw
        {
            recipeId = "healing",
            cocktailNameKo = "힐링",
            cocktailNameEn = "Healing",
            requiredGlass = GlassType.CocktailGlass,
            requiredMethod = MethodType.Shake,
            requiredGarnish = "Twist of Lemon Peel",
            isSequenceImportant = false,
            baseDonationPrice = 1600,
            requiredIngredients = new List<IngredientRatio> {
                new IngredientRatio { ingredientId = "gam_hong_ro", amountOz = 1.5f, allowableError = 0.15f },
                new IngredientRatio { ingredientId = "benedictine_dom", amountOz = 0.33f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "creme_de_cassis", amountOz = 0.33f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "sweet_sour_mix", amountOz = 1.0f, allowableError = 0.1f }
            },
            preparationSteps = new List<string> {
                "1. 칵테일 글라스에 얼음을 넣어 칠링한다.",
                "2. 셰이커에 얼음, 감홍로 1 1/2oz, 베네딕틴 DOM 1/3oz, 크렘 드 카시스 1/3oz, 스위트 앤 사워 믹스 1oz를 넣는다.",
                "3. 8~10회 셰이킹한다.",
                "4. 칠링한 잔의 얼음을 버리고 따른다.",
                "5. 레몬 껍질을 트위스트하여 잔에 넣는다."
            }
        });

        // 33. 진도
        list.Add(new RecipeDataRaw
        {
            recipeId = "jindo",
            cocktailNameKo = "진도",
            cocktailNameEn = "Jindo",
            requiredGlass = GlassType.CocktailGlass,
            requiredMethod = MethodType.Shake,
            requiredGarnish = "None",
            isSequenceImportant = false,
            baseDonationPrice = 1600,
            requiredIngredients = new List<IngredientRatio> {
                new IngredientRatio { ingredientId = "jindo_hong_ju", amountOz = 1.0f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "creme_de_menthe_white", amountOz = 0.5f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "white_grape_juice", amountOz = 0.75f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "raspberry_syrup", amountOz = 0.5f, allowableError = 0.1f }
            },
            preparationSteps = new List<string> {
                "1. 칵테일 글라스에 얼음을 넣어 칠링한다.",
                "2. 셰이커에 얼음, 진도홍주 1oz, 크렘 드 멘트 화이트 1/2oz, 청포도 주스 3/4oz, 라즈베리 시럽 1/2oz를 넣는다.",
                "3. 8~10회 셰이킹한다.",
                "4. 칠링한 잔의 얼음을 버리고 잔에 따른다."
            }
        });

        // 34. 풋사랑
        list.Add(new RecipeDataRaw
        {
            recipeId = "puppy_love",
            cocktailNameKo = "풋사랑",
            cocktailNameEn = "Puppy Love",
            requiredGlass = GlassType.CocktailGlass,
            requiredMethod = MethodType.Shake,
            requiredGarnish = "A Slice of Apple",
            isSequenceImportant = false,
            baseDonationPrice = 1500,
            requiredIngredients = new List<IngredientRatio> {
                new IngredientRatio { ingredientId = "andong_soju", amountOz = 1.0f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "triple_sec", amountOz = 0.33f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "apple_pucker", amountOz = 1.0f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "lime_juice", amountOz = 0.33f, allowableError = 0.1f }
            },
            preparationSteps = new List<string> {
                "1. 칵테일 글라스에 얼음을 넣어 칠링한다.",
                "2. 셰이커에 얼음, 안동소주 1oz, 트리플 색 1/3oz, 애플 퍼커 1oz, 라임 주스 1/3oz를 넣는다.",
                "3. 8~10회 셰이킹한다.",
                "4. 칠링한 잔의 얼음을 버리고 따른다.",
                "5. 사과 슬라이스를 잔 림에 장식한다."
            }
        });

        // 35. 금산
        list.Add(new RecipeDataRaw
        {
            recipeId = "geumsan",
            cocktailNameKo = "금산",
            cocktailNameEn = "Geumsan",
            requiredGlass = GlassType.CocktailGlass,
            requiredMethod = MethodType.Shake,
            requiredGarnish = "None",
            isSequenceImportant = false,
            baseDonationPrice = 1600,
            requiredIngredients = new List<IngredientRatio> {
                new IngredientRatio { ingredientId = "geumsan_insamju", amountOz = 1.5f, allowableError = 0.15f },
                new IngredientRatio { ingredientId = "kahlua", amountOz = 1.0f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "apple_pucker", amountOz = 1.0f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "lime_juice", amountOz = 0.05f, allowableError = 0.02f }
            },
            preparationSteps = new List<string> {
                "1. 칵테일 글라스에 얼음을 넣어 칠링한다.",
                "2. 셰이커에 얼음, 금산인삼주 1 1/2oz, 깔루아 1oz, 애플 퍼커 1oz, 라임 주스 1tsp를 넣는다.",
                "3. 8~10회 셰이킹한다.",
                "4. 칠링한 잔의 얼음을 버리고 잔에 따른다."
            }
        });

        // 36. 고창
        list.Add(new RecipeDataRaw
        {
            recipeId = "gochang",
            cocktailNameKo = "고창",
            cocktailNameEn = "Gochang",
            requiredGlass = GlassType.FluteChampagne,
            requiredMethod = MethodType.Stir,
            requiredGarnish = "None",
            isSequenceImportant = false,
            baseDonationPrice = 1500,
            requiredIngredients = new List<IngredientRatio> {
                new IngredientRatio { ingredientId = "bokbunja_wine", amountOz = 2.0f, allowableError = 0.2f },
                new IngredientRatio { ingredientId = "triple_sec", amountOz = 0.5f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "sprite", amountOz = 2.0f, allowableError = 0.2f }
            },
            preparationSteps = new List<string> {
                "1. 플루트 샴페인 글라스에 얼음을 넣어 칠링한다.",
                "2. 믹싱 글라스에 얼음, 복분자주 2oz, 트리플 색 1/2oz, 사이다 2oz를 넣는다.",
                "3. 바스푼으로 6~8회 저어준다.",
                "4. 칠링한 잔의 얼음을 버리고 스트레이너를 끼워 플루트 잔 중앙으로 따른다."
            }
        });

        // 37. 진피즈
        list.Add(new RecipeDataRaw
        {
            recipeId = "gin_fizz",
            cocktailNameKo = "진피즈",
            cocktailNameEn = "Gin Fizz",
            requiredGlass = GlassType.Highball,
            requiredMethod = MethodType.Shake,
            requiredGarnish = "A Slice of Lemon",
            isSequenceImportant = false,
            baseDonationPrice = 1300,
            requiredIngredients = new List<IngredientRatio> {
                new IngredientRatio { ingredientId = "dry_gin", amountOz = 1.5f, allowableError = 0.15f },
                new IngredientRatio { ingredientId = "lemon_juice", amountOz = 0.5f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "powdered_sugar", amountOz = 0.1f, allowableError = 0.05f },
                new IngredientRatio { ingredientId = "soda_water", amountOz = 2.5f, allowableError = 0.3f }
            },
            preparationSteps = new List<string> {
                "1. 하이볼 글라스에 얼음을 넣어 칠링한다 (얼음 버리지 않음).",
                "2. 셰이커에 얼음, 드라이 진 1 1/2oz, 레몬 주스 1/2oz, 설탕 1tsp를 넣고 저은 뒤 8~10회 셰이킹한다.",
                "3. 칠링한 잔의 얼음을 버리지 않고 셰이커 캡만 열어 잔에 따른다.",
                "4. 소다수를 80~90% 채우고 살짝 저어준다.",
                "5. 레몬 슬라이스를 잔 림에 장식한다."
            }
        });

        // 38. 프레시 레몬 스쿼시
        list.Add(new RecipeDataRaw
        {
            recipeId = "fresh_lemon_squash",
            cocktailNameKo = "프레시 레몬 스쿼시",
            cocktailNameEn = "Fresh Lemon Squash",
            requiredGlass = GlassType.Highball,
            requiredMethod = MethodType.Build,
            requiredGarnish = "A Slice of Lemon",
            isSequenceImportant = false,
            baseDonationPrice = 1000,
            requiredIngredients = new List<IngredientRatio> {
                new IngredientRatio { ingredientId = "fresh_squeezed_lemon", amountOz = 0.5f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "powdered_sugar", amountOz = 0.2f, allowableError = 0.05f },
                new IngredientRatio { ingredientId = "soda_water", amountOz = 3.0f, allowableError = 0.3f }
            },
            preparationSteps = new List<string> {
                "1. 하이볼 글라스에 얼음을 넣어 칠링한다.",
                "2. 스퀴저로 레몬 반 개의 즙을 짠다 (8~10회 비틂).",
                "3. 칠링한 잔의 얼음을 버리고 레몬즙과 설탕 2tsp를 넣어 녹인다.",
                "4. 글라스에 얼음을 6~8개 채우고 소다수로 80~90% 채운 뒤 살짝 저어준다.",
                "5. 레몬 슬라이스를 잔 림에 장식한다."
            }
        });

        // 39. 버진 프루트 펀치
        list.Add(new RecipeDataRaw
        {
            recipeId = "virgin_fruit_punch",
            cocktailNameKo = "버진 프루트 펀치",
            cocktailNameEn = "Virgin Fruit Punch",
            requiredGlass = GlassType.FootedPilsner,
            requiredMethod = MethodType.Blend,
            requiredGarnish = "A Wedge of Pineapple & Cherry",
            isSequenceImportant = false,
            baseDonationPrice = 1200,
            requiredIngredients = new List<IngredientRatio> {
                new IngredientRatio { ingredientId = "orange_juice", amountOz = 1.0f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "pineapple_juice", amountOz = 1.0f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "cranberry_juice", amountOz = 1.0f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "grapefruit_juice", amountOz = 1.0f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "lemon_juice", amountOz = 0.5f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "grenadine_syrup", amountOz = 0.5f, allowableError = 0.1f }
            },
            preparationSteps = new List<string> {
                "1. 풋 필스너 글라스에 얼음을 넣어 칠링한다.",
                "2. 블렌더에 오렌지/파인애플/크랜베리/자몽 주스 각 1oz, 레몬 주스 1/2oz, 그레나딘 시럽 1/2oz를 넣는다.",
                "3. 크러시드 아이스 1스쿱을 넣고 10초간 갈아준다.",
                "4. 칠링한 잔의 얼음을 버리고 블렌딩된 음료를 따른다.",
                "5. 파인애플 웨지와 체리를 픽에 꽂아 장식한다."
            }
        });

        // 40. 불바디에
        list.Add(new RecipeDataRaw
        {
            recipeId = "boulevardier",
            cocktailNameKo = "불바디에",
            cocktailNameEn = "Boulevardier",
            requiredGlass = GlassType.OldFashioned,
            requiredMethod = MethodType.Stir,
            requiredGarnish = "Twist of Orange Peel",
            isSequenceImportant = false,
            baseDonationPrice = 1500,
            requiredIngredients = new List<IngredientRatio> {
                new IngredientRatio { ingredientId = "bourbon_whiskey", amountOz = 1.0f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "sweet_vermouth", amountOz = 1.0f, allowableError = 0.1f },
                new IngredientRatio { ingredientId = "campari", amountOz = 1.0f, allowableError = 0.1f }
            },
            preparationSteps = new List<string> {
                "1. 올드 패션드 글라스에 얼음을 넣어 칠링한다 (얼음 버리지 않음).",
                "2. 믹싱 글라스에 얼음, 버번 위스키 1oz, 스위트 베르무스 1oz, 캄파리 1oz를 넣는다.",
                "3. 바스푼으로 글라스 벽을 따라 6~8회 저어준다.",
                "4. 올드 패션드 글라스의 얼음을 버리지 않은 채 스트레이너를 끼운 믹싱 글라스 내용을 따른다.",
                "5. 오렌지 껍질을 트위스트하여 잔에 넣는다."
            }
        });

        return list;
    }
}
#endif