using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTMO.Help.Model;
using System.Net.Http;
using System.Net;
using System.Diagnostics;
using Newtonsoft.Json;

namespace iTMO.Help.Controller
{
    public enum RequestTypes
    {
        Journal,
        JournalChangeLog,
        MessagesFromDe,
        Schedule,
        ScheduleExam,
        ScheduleTeacher,
        ScheduleExamTeacher
    }

    class DataResponse
    {
        public string   Data    { get; set; }
        public bool     isValid { get; set; }

        public DataResponse(string data, bool isValid)
        {
            this.Data = data;
            this.isValid = isValid;
        }
    }

    class HttpController
    {
        private static HttpClient httpClient = new HttpClient();

        private static string BuildDeAuthUri(string login, string pass)
        {
            StringBuilder result = new StringBuilder("https://de.ifmo.ru/servlet/");
            result.AppendFormat("?Rule=LOGON&LOGIN={0}&PASSWD={1}", login, pass);
            return result.ToString();
        }

        public static string BuildUri(RequestTypes request, params string[] opts)
        {
            StringBuilder result = new StringBuilder();

            string ApiKey       = "/AYPlvngDDdVoZdJgPAKoeHQnWUDrvOwEHpEXxNGNfaAKsugiJyCGxotWsTdqCdHL/";

            string isuBaseLink  = "https://isu.ifmo.ru/ords/isurest/v1/api/public";
            string deBaseLink   = "https://de.ifmo.ru/api/private";

            // ISU
            string schedule         = "/schedule";
            string exams            = "/exams";
            string group            = "/group";
            string teacher          = "/teacher";
            string common           = "/common";

            // DE
            string journal          = "/eregister";
            string journalChangeLog = "/eregisterlog?days={0}";
            string messageDe        = "/mail?days={0}&unreadOnly={0}";

            switch (request)
            {
                case RequestTypes.Journal:
                    result.Append(deBaseLink + journal);
                    break;
                case RequestTypes.JournalChangeLog:
                    if(opts.Length == 1)
                        result.AppendFormat(deBaseLink + journalChangeLog, opts[0]);
                    break;
                case RequestTypes.MessagesFromDe:
                    if(opts.Length == 2)
                        result.AppendFormat(deBaseLink + messageDe, opts[0], opts[1]);
                    break;
                case RequestTypes.Schedule:
                    if (opts.Length == 1)
                        result.Append(isuBaseLink + schedule + common + group + ApiKey + opts[0]);
                    break;
                case RequestTypes.ScheduleExam:
                    if(opts.Length == 1)
                        result.Append(isuBaseLink + exams + common + group + ApiKey + opts[0]);
                    break;
                case RequestTypes.ScheduleTeacher:
                    if(opts.Length == 1)
                        result.Append(isuBaseLink + schedule + common + teacher + ApiKey + opts[0]);
                    break;
                case RequestTypes.ScheduleExamTeacher:
                    if(opts.Length == 1)
                        result.Append(isuBaseLink + exams + common + teacher + ApiKey + opts[0]);
                    break;
                default:
                    return null;
            }
            return result.ToString();
        }

        public static async Task<DataResponse> ProccessRequest(RequestTypes type, params string[] opts)
        {
            HttpResponseMessage response = null;
            string result = null;

            try
            {
                response = await httpClient.GetAsync(BuildUri(type, opts));
                result = await response.Content.ReadAsStringAsync();
            }
            catch(Exception ex)
            {
                if(response != null && response.Content != null)
                    Debug.WriteLine(ex.ToString() + " | IN :" + response.Content);
            }

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return new DataResponse(result, true);

                case HttpStatusCode.NotFound:
                    break;

                case HttpStatusCode.NotAcceptable:
                    break;

                case HttpStatusCode.BadRequest:
                    break;

                case HttpStatusCode.Forbidden:
                    break;

                default:
                    break;
            }
            return null;
        }

        public static async Task<DataResponse> RetrieveData(RequestTypes type, params string[] opts)
        {
            DataResponse result = null;

            try
            {
                switch (type)
                {
                    case RequestTypes.Journal:
                    case RequestTypes.JournalChangeLog:
                    case RequestTypes.MessagesFromDe:
                        break;

                    default:
                        result = await ProccessRequest(type, opts);
                        break;
                }
            }

            catch(Exception ex)
            {
                Debug.WriteLine(ex.ToString() + " | IN : " + result.Data);
            }
            return null;
        }

        public static async Task<string> RetrieveScheduleExam()
        {
            var result = await ProccessRequest(RequestTypes.ScheduleExam, "P3310");

            try
            {
               // return JsonConvert.DeserializeObject<ScheduleExam>(result);
            }
            catch(Exception ex) { Debug.WriteLine(result); }
            return null;
        }
    }
}
