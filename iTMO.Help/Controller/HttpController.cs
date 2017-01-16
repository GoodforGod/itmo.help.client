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

        private static bool isAuthiticated = false;

        private static string BuildDeAuthUri(string login, string pass)
        {
            StringBuilder result = new StringBuilder("https://de.ifmo.ru/servlet/");
            result.AppendFormat("?Rule=LOGON&LOGIN={0}&PASSWD={1}", login, pass);
            return result.ToString();
        }

        private static string BuildUri(RequestTypes request, params string[] opts)
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

        private static async Task<DataResponse> AuthOnDe()
        {
            HttpResponseMessage response = null;
            string              result   = null;
            bool                isValid  = false;

            try
            {
                var usr = DatabaseController.Me.DUser;
                if (!string.IsNullOrEmpty(usr.Login) && !string.IsNullOrEmpty(usr.Password))
                {
                    response = await httpClient.GetAsync(BuildDeAuthUri(usr.Login, usr.Password));
                    result = await response.Content.ReadAsStringAsync();
                }
                else result = "Empty Login or Password";
            }
            catch (HttpRequestException ex)     { result = "Some Network Connectivity Error.. Check your Network"; }
            catch (Exception ex)                { result = "Some unexpected error.."; }

            return new DataResponse(result, isValid);
        }

        public static async Task<DataResponse> ProccessRequest(RequestTypes type, params string[] opts)
        {
            HttpResponseMessage response = null;
            string              result  = null;
            bool                isValid = false;

            try
            {
                response    = await httpClient.GetAsync(BuildUri(type, opts));
                result      = await response.Content.ReadAsStringAsync();
            }
            catch(HttpRequestException ex)
            {
                result = "Some Network Connectivity Error.. Check your Network";
            }
            catch(Exception ex)
            {
                result = "Some unexpected error..";
            }

            if (response != null)
            {
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        if (response.Content != null)
                            isValid = true;
                        else
                            result = "Invalid Group";
                        break;
                    case HttpStatusCode.NotFound:       result = "Resource not found, ops... [ 404 ]"; break;
                    case HttpStatusCode.NotAcceptable:  result = "Think that schedule is unavaliable for now... Or check the group may be?"; break;
                    case HttpStatusCode.BadRequest:     result = "Server got some unexpected error...  [ ITMO : Students] | [ 0 : 1 ]"; break;
                    case HttpStatusCode.Forbidden:      result = "Think that we are outdated... Contact Developers or check ITMO site"; break;
                    default:                            result = "Somethink extraodinary happened... Contact developer"; break;
                }
            }
            return new DataResponse(result, isValid);
        }

        public static async Task<DataResponse> RetrieveData(RequestTypes type, params string[] opts)
        {
            DataResponse result = null;

            switch (type)
            {
                case RequestTypes.Journal:
                case RequestTypes.JournalChangeLog:
                case RequestTypes.MessagesFromDe:
                    result = await AuthOnDe();
                    //result = await ProccessRequest(type, opts);
                    break;

                default:
                    result = await ProccessRequest(type, opts);
                    break;
            }

            return result;
        }
    }
}
