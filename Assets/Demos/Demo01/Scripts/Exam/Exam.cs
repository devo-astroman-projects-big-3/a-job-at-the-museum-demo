using System;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class Exam : MonoBehaviour
{
    [SerializeField] private DialogueRunner _dialogueRunner;
    [SerializeField] private QuestionsDatabase _questionsDatabase;
    [SerializeField, Min(1)] private int _nQuestions = 3;

    private Question[] _questions;
    private IDisposable _questionChangeSubscription;
    private bool _hasSubscribedToQuestionChanges;

    public void InitialSetup()
    {
        CreateANewExam();
        if (_questions != null && _questions.Length > 0)
        {
            if (!_hasSubscribedToQuestionChanges)
                SubscribeToQuestionChanges();
            SetNextQuestion();
        }
    }

    private void OnEnable()
    {
        InitialSetup();
    }

    private void OnDestroy()
    {
        _questionChangeSubscription?.Dispose();
        _questionChangeSubscription = null;
    }

    public void CreateANewExam()
    {
        if (_dialogueRunner == null || _dialogueRunner.VariableStorage == null || _questionsDatabase == null)
        {
            Debug.LogWarning("Exam requires a dialogue runner, variable storage, and questions database.", this);
            return;
        }

        List<Question> candidates = new List<Question>();
        if (_questionsDatabase.Items != null)
        {
            foreach (Question question in _questionsDatabase.Items)
            {
                if (question != null && !candidates.Contains(question))
                {
                    int correct = char.ToUpperInvariant(question.CorrectChoiceLetter) - 'A' + 1;
                    if (correct >= 1 && correct <= 4)
                        candidates.Add(question);
                    else
                        Debug.LogWarning($"Question '{question.Id}' requires a correct choice from A to D.", this);
                }
            }
        }

        if (_nQuestions <= 0 || candidates.Count == 0)
        {
            Debug.LogWarning("Exam requires a positive question count and valid questions.", this);
            return;
        }

        int count = Mathf.Min(_nQuestions, candidates.Count);
        if (count < _nQuestions)
            Debug.LogWarning($"Only {count} distinct valid questions are available for the exam.", this);

        _questions = new Question[count];
        for (int i = 0; i < count; i++)
        {
            int selectedIndex = UnityEngine.Random.Range(i, candidates.Count);
            Question selected = candidates[selectedIndex];
            candidates[selectedIndex] = candidates[i];
            candidates[i] = selected;
            _questions[i] = selected;
        }

        var storage = _dialogueRunner.VariableStorage;
        storage.SetValue("$totalQuestions", (float)count);
        storage.SetValue("$currentQuestion", 0f);
        storage.SetValue("$nCorrects", 0f);
        storage.SetValue("$selection", 0f);
    }

    private void SubscribeToQuestionChanges()
    {
        if (!_hasSubscribedToQuestionChanges && _dialogueRunner != null && _dialogueRunner.VariableStorage != null)
        {
            Debug.Log("SubscribeToQuestionChanges");
            _questionChangeSubscription = _dialogueRunner.VariableStorage.AddChangeListener(OnVariableChanged);
            _hasSubscribedToQuestionChanges = true;
        }
    }

    private void OnVariableChanged(string variableName, object newValue)
    {
        Debug.Log("OnVariableChanged");
        if (variableName == "$currentQuestion")
            SetNextQuestion();
    }

    private void SetNextQuestion()
    {
        if (_questions == null || _dialogueRunner == null || _dialogueRunner.VariableStorage == null)
            return;

        var storage = _dialogueRunner.VariableStorage;
        if (!storage.TryGetValue<float>("$currentQuestion", out float currentQuestion))
            return;

        // Yarn increments before displaying a question, so its index starts at 1.
        if (currentQuestion < 1 || currentQuestion > _questions.Length || currentQuestion != Mathf.Floor(currentQuestion))
            return;

        Question question = _questions[(int)currentQuestion - 1];
        storage.SetValue("$question", question.QuestionStatement ?? string.Empty);
        storage.SetValue("$choiceA", question.ChoiceA ?? string.Empty);
        storage.SetValue("$choiceB", question.ChoiceB ?? string.Empty);
        storage.SetValue("$choiceC", question.ChoiceC ?? string.Empty);
        storage.SetValue("$choiceD", question.ChoiceD ?? string.Empty);
        storage.SetValue("$correct", (float)(char.ToUpperInvariant(question.CorrectChoiceLetter) - 'A' + 1));
    }
}
