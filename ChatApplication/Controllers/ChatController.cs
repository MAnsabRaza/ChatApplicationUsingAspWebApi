using ChatApplication.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace ChatApplication.Controllers
{
    public class ChatController : Controller
    {
        private AppDbContext db = new AppDbContext();

        public ActionResult Chat(Guid? sessionId = null)
        {
            ViewBag.SessionId = sessionId;
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> SendMessage(string message, Guid? sessionId)
        {
            try
            {
                int userId = Convert.ToInt32(Session["userId"]);
                if (sessionId == null) sessionId = Guid.NewGuid();

                string responseText = await CallGeminiApi(message);

                var chat = new Chat
                {
                    userId = userId,
                    current_date = DateTime.Now,
                    sessionId = sessionId.Value,
                    response = responseText,
                    message = message,
                };

                db.Chat.Add(chat);
                db.SaveChanges();

                return Json(new { sessionId = sessionId, response = responseText });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
                return Json(new { error = ex.Message });
            }
        }

        private async Task<string> CallGeminiApi(string prompt)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string apiKey = System.Configuration.ConfigurationManager.AppSettings["GeminiApiKey"];

                    var body = new
                    {
                        contents = new[] {
                            new {
                                parts = new[] {
                                    new { text = prompt }
                                }
                            }
                        }
                    };

                    string jsonBody = JsonConvert.SerializeObject(body);
                    var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(
                        $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={apiKey}",
                        content
                    );

                    var result = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"API Response: {result}");

                    if (!response.IsSuccessStatusCode)
                    {
                        System.Diagnostics.Debug.WriteLine($"API Error: {response.StatusCode} - {result}");
                        return $"API Error: {response.StatusCode} - {result}";
                    }

                    dynamic json = JsonConvert.DeserializeObject(result);
                    try
                    {
                        string responseText = json?.candidates?[0]?.content?.parts?[0]?.text;
                        return !string.IsNullOrEmpty(responseText) ? responseText : "No response from AI";
                    }
                    catch
                    {
                        return $"Failed to parse response: {result}";
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Gemini API Error: {ex.Message}");
                return $"Error calling AI service: {ex.Message}";
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public JsonResult GetUserChats()
        {
            int userId = Convert.ToInt32(Session["userId"]);
            var chats = db.Chat
                .Where(c => c.userId == userId)
                .GroupBy(c => c.sessionId)
                .Select(g => new {
                    SessionId = g.Key,
                    LastMessage = g.OrderByDescending(x => x.current_date).FirstOrDefault().message,
                    LastDate = g.OrderByDescending(x => x.current_date).FirstOrDefault().current_date
                })
                .OrderByDescending(x => x.LastDate)
                .ToList();

            return Json(chats, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetChatBySession(Guid sessionId)
        {
            int userId = Convert.ToInt32(Session["userId"]);
            var chats = db.Chat
               .Where(c => c.userId == userId && c.sessionId == sessionId)
               .OrderBy(c => c.current_date)
               .Select(c => new
               {
                   c.message,
                   c.response,
                   c.current_date
               })
               .ToList();

            return Json(chats, JsonRequestBehavior.AllowGet);
        }
    }
}