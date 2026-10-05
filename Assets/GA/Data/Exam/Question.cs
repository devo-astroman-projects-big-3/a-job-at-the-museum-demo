using UnityEngine;

[CreateAssetMenu(fileName = "Question", menuName = "Game/Exam/Question")]
public class Question : ScriptableObject
{
    [SerializeField] private string _id;
    [SerializeField] private string _question;
    [SerializeField] private string _choiceA;
    [SerializeField] private string _choiceB;
    [SerializeField] private string _choiceC;
    [SerializeField] private string _choiceD;

    [SerializeField] private char _correctChoiceLetter; //a,b,c,e



    public string Id => _id;
    public string QuestionStatement => _question;
    public string ChoiceA => _choiceA;
    public string ChoiceB => _choiceB;
    public string ChoiceC => _choiceC;
    public string ChoiceD => _choiceD;
    public char CorrectChoiceLetter => _correctChoiceLetter;

}
