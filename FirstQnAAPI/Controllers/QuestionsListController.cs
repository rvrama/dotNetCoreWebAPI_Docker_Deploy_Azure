using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;
using FirstQnAAPI;
using FirstQnAAPI.Data;

namespace FirstQnAAPI.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsListController : ControllerBase
    {
        private readonly FirstQnAAPIContext _context;

        public QuestionsListController(FirstQnAAPIContext context)
        {
            _context = context;
        }

        // GET: api/QuestionsList
        [HttpGet]
        public ActionResult<List<QuestionList>> GetQuestionsList()
        {
            List<QuestionList> qmList = new List<QuestionList>();
            var questionList = _context.Question.ToList();
            foreach (Question a in questionList)
            {
                List<string> choices = _context.QnA.Where<QnA>(
                                        b => (b.QuestionId==a.QuestionId)
                                        ).Select(a => a.ChoiceTitle).ToList();

                string choiceText=string.Join(",", choices);
                QuestionList qm = new()
                 {
                     questionId = a.QuestionId,
                     questionTxt = a.QuestionTitle,
                     choices = choiceText,
                     choiceType = 1, //hardcoded for now...need to update the db to get the value
                     answerChoiceId = 2 //hardcoded for now...need to update the db to get the value through dbset 
                 };
                 qmList.Add(qm);
            }
            return qmList;

        }

    
    }
}
