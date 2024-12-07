namespace FirstQnAAPI
{
    public class QnA
    {
        public int QuestionId {  get; set; }

        public int ChoiceId { get; set; }

        public bool? isAnswer { get; set; }

        public string ChoiceTitle { get; set; }

    }
}
