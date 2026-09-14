using System.Collections.Generic;
using UnityEngine;

public class CocktailEvaluationEngine
{
    private const int PASS_THRESHOLD = 60; // GDD v0.4 합격 기준점수 (60점 이상)

    // 채점 결과 및 피드백 전달용 구조체
    public struct EvaluationResult
    {
        public bool isSuccess;             // 합격 여부 (60점 이상)
        public int finalScore;             // 100점 만점 기준 최종 점수
        public int earnedDonation;         // 정산된 도네이션 금액 (P)
        public List<string> feedbackNotes; // 버튜버 아카리 대사 연동용 상세 감점 사유 리스트
    }

    public EvaluationResult EvaluateCocktail(RecipeSO answer, PlayerInputCocktail playerInput)
    {
        int score = 100; // 100점 만점에서 시작하여 감점 방식 적용
        List<string> notes = new List<string>();

        if (answer == null || playerInput == null)
        {
            Debug.LogError("[EvaluationEngine] 레시피 데이터 또는 플레이어 입력 데이터가 null입니다.");
            return new EvaluationResult { isSuccess = false, finalScore = 0, earnedDonation = 0, feedbackNotes = new List<string> { "잘못된 데이터" } };
        }

        // 1. 글라스 검증 (감점 30점)
        if (playerInput.glass != answer.requiredGlass)
        {
            score -= 30;
            notes.Add($"글라스 오선택 (-30점): 정답은 {answer.requiredGlass}입니다.");
            Debug.Log($"[Evaluation] 글라스 불일치 (-30): 제출 {playerInput.glass} / 정답 {answer.requiredGlass}");
        }

        // 2. 조주 기법(Method) 검증 (감점 20점)
        if (playerInput.method != answer.requiredMethod)
        {
            score -= 20;
            notes.Add($"조주 기법 오류 (-20점): {answer.requiredMethod} 기법을 사용해야 합니다.");
            Debug.Log($"[Evaluation] 기법 불일치 (-20): 제출 {playerInput.method} / 정답 {answer.requiredMethod}");
        }

        // 3. 가니쉬(Garnish) 검증 (감점 10점)
        if (string.IsNullOrEmpty(playerInput.garnish) || playerInput.garnish != answer.requiredGarnish)
        {
            score -= 10;
            notes.Add($"가니쉬 오선택 또는 누락 (-10점): 정답은 {answer.requiredGarnish}입니다.");
            Debug.Log($"[Evaluation] 가니쉬 불일치 (-10): 제출 {playerInput.garnish} / 정답 {answer.requiredGarnish}");
        }

        // 4. 필수 재료 누락 및 용량 허용 오차 검사
        foreach (var req in answer.requiredIngredients)
        {
            float poured = playerInput.GetIngredientAmount(req.ingredientId);

            if (poured <= 0f)
            {
                score -= 15; // 필수 재료 아예 누락 시 재료당 -15점
                notes.Add($"필수 재료 누락 (-15점): {req.ingredientId} 재료가 들어가지 않았습니다.");
                Debug.Log($"[Evaluation] 필수 재료 누락 (-15): {req.ingredientId}");
            }
            else
            {
                // 허용 오차 범위를 초과했는지 정밀 연산
                float error = Mathf.Abs(poured - req.amountOz);
                if (error > req.allowableError)
                {
                    // 초과 오차 비례 감점 (오차 0.1oz당 약 1점, 재료당 최대 10점)
                    int penalty = Mathf.Min(10, Mathf.RoundToInt(error * 10f));
                    score -= penalty;
                    notes.Add($"재료 비율 오차 (-{penalty}점): {req.ingredientId} 용량이 적정 범위를 벗어났습니다.");
                    Debug.Log($"[Evaluation] 용량 오차 감점 (-{penalty}): {req.ingredientId} (오차: {error:F2}oz)");
                }
            }
        }

        // 5. [핵심!] 제조 순서(Sequence) 검증 (isSequenceImportant가 true인 층쌓기/플로팅 레시피)
        if (answer.isSequenceImportant)
        {
            List<string> playerSeq = playerInput.GetPouredSequence();
            bool isSequenceCorrect = true;

            if (playerSeq == null || playerSeq.Count < answer.requiredIngredients.Count)
            {
                isSequenceCorrect = false;
            }
            else
            {
                for (int i = 0; i < answer.requiredIngredients.Count; i++)
                {
                    // 정답 리스트의 순서와 플레이어가 실제 투입한 순서를 1:1 비교
                    if (i >= playerSeq.Count || playerSeq[i] != answer.requiredIngredients[i].ingredientId)
                    {
                        isSequenceCorrect = false;
                        break;
                    }
                }
            }

            if (!isSequenceCorrect)
            {
                score -= 15; // 순서 오류 시 -15점 감점
                notes.Add($"제조 순서 오류 (-15점): 층쌓기 또는 투입 순서가 올바르지 않습니다.");
                Debug.Log($"[Evaluation] 순서 불일치 (-15): {answer.cocktailNameKo} 레시피의 순서가 틀렸습니다.");
            }
        }

        // 최저 0점 보장 및 60점 이상 합격 판정
        score = Mathf.Max(0, score);
        bool isPass = score >= PASS_THRESHOLD;

        if (isPass && notes.Count == 0)
        {
            notes.Add("완벽한 조주입니다! 완벽한 비율과 순서입니다.");
        }

        return new EvaluationResult
        {
            isSuccess = isPass,
            finalScore = score,
            earnedDonation = isPass ? answer.baseDonationPrice : 0,
            feedbackNotes = notes
        };
    }
}