using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FirstQnAAPI.Data;
using Microsoft.AspNetCore.Cors;
using NuGet.Packaging;

namespace FirstQnAAPI.Controllers
{
    [EnableCors]
    [Route("api/[controller]")]
    [ApiController]
    public class QResultController : ControllerBase
    {
        private readonly FirstQnAAPIContext _context;

        public QResultController(FirstQnAAPIContext context)
        {
            _context = context;
        }

        // GET: api/Result?userId={userId}
        [HttpGet]
        public ActionResult<UserResult> GetQResult(string userId)
        {
            List<QWiseResult> res = _context.QWiseResult.Where<QWiseResult>(q => q.UserId == userId).ToList();
            List<QuestionWiseResult> qw = new List<QuestionWiseResult>();
            foreach (var n in res)
            {
                var x = new QuestionWiseResult() { id = n.QuestionId,
                                           answer = n.AnswerChoiceId,
                                           selected = n.SelectedChoiceId };
               qw.Add(x);
            } 

            QResult? qr = _context.QResult.Where<QResult>(a=> a.UserId == userId).FirstOrDefault<QResult>();
             
             if (qr != null) {
             _context.UserResult = new UserResult(){
                 UserId = qr.UserId,
                 GroupId = qr.GroupId,
                 Score = qr.Score,
                 TimeSpent = qr.TimeSpent,
                 Results = qw.ToArray()
             };
             }

             return _context.UserResult;

        //    UserResult[] ur = {
        //     new UserResult
        //     {
        //         id = 1,
        //         UserId=userId,
        //         GroupId = 2,
        //         Score = 100,
        //         TimeSpent = 3000,
        //         Results = new QuestionWiseResult[] {
        //             new QuestionWiseResult{ id = 1, answer = 2, selected = 2 } 
        //         }
        //     }
        // };
        //return ur;
        }
        // POST: api/QResult
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<UserResult> PostQResult(UserResult  results)
        {
            var local_userId = results.UserId;
            var local_groupId = results.GroupId;

            QResult? qResultExist = _context.QResult.Where(x => (x.UserId == results.UserId && x.GroupId == results.GroupId)).FirstOrDefault();
            List<QWiseResult> qWiseRessultExist = _context.QWiseResult.Where(x=> (x.UserId == results.UserId && x.GroupId == results.GroupId)).ToList();


            if (qResultExist == null && qWiseRessultExist.Count == 0)
                {
                    QResult qr = new QResult() {
                        UserId = results.UserId,
                        //Id = 1, //attemptid hardcoded for now
                        GroupId = results.GroupId,
                        Score = results.Score,
                        TimeSpent = results.TimeSpent
                    };

                    _context.QResult.Add(qr);

                    List<QWiseResult> qw = new List<QWiseResult>();
                    
                    foreach (QuestionWiseResult q in results.Results) {
                        qw.Add(new QWiseResult() {
                            UserId = local_userId,
                            GroupId = local_groupId,
                            QuestionId = q.id,
                            AnswerChoiceId = q.answer,
                            SelectedChoiceId = q.selected
                    });
                    }

                _context.QWiseResult.AddRange(qw);

                _context.SaveChanges();
                }

            else {

                    QuestionWiseResult[] qwResult = new QuestionWiseResult[qWiseRessultExist.Count];
                    var index = 0;
                    foreach (var a in qWiseRessultExist){
                        qwResult[index] = 
                        new QuestionWiseResult {
                            id=a.QuestionId,
                            answer=a.AnswerChoiceId,
                            selected=a.SelectedChoiceId
                        };
                        index++;
                    }

                    results = new UserResult() {
                        GroupId = qResultExist.GroupId,
                        //id = 1, //attemptId hardcoded for now
                        UserId = qResultExist.UserId,
                        Score = qResultExist.Score,
                        TimeSpent = qResultExist.TimeSpent,
                        Results = qwResult
                    };             

            }
        
           return CreatedAtAction("GetQResult", new { userId = results.UserId }, results);
        }

        //// DELETE: api/Questions/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteQuestion(int id)
        //{
        //    var question = await _context.Question.FindAsync(id);
        //    if (question == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Question.Remove(question);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}
    }
}
