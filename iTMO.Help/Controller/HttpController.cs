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
using System.Net.Http.Headers;

namespace iTMO.Help.Controller
{
    public enum RequestTypes
    {
        AuthDe,
        Journal,
        JournalChangeLog,
        MessagesFromDe,
        Schedule,
        ScheduleExam,
        ScheduleTeacher,
        ScheduleExamTeacher
    }

    class DataResponse<TValue>
    {
        public TValue Data    { get; set; }
        public bool     isValid { get; set; }

        public DataResponse(TValue data, bool isValid)
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

        private static HttpRequestMessage BuildRequest(RequestTypes type, params string[] opts)
        {
            HttpRequestMessage request = null;
            StringBuilder resultUri = new StringBuilder();

            string ApiKey       = "/AYPlvngDDdVoZdJgPAKoeHQnWUDrvOwEHpEXxNGNfaAKsugiJyCGxotWsTdqCdHL/";

            string isuBaseLink  = "https://isu.ifmo.ru/ords/isurest/v1/api/public";
            string deBaseLink   = "https://de.ifmo.ru/api/private";
            string deAuthLink   = "https://de.ifmo.ru/servlet/?Rule=LOGON&LOGIN={0}&PASSWD={1}";

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

            switch (type)
            {
                case RequestTypes.AuthDe:           resultUri.AppendFormat(deAuthLink, opts[0], opts[1]);                           break;
                case RequestTypes.Journal:          resultUri.Append(deBaseLink + journal);                                         break;
                case RequestTypes.JournalChangeLog: resultUri.AppendFormat(deBaseLink + journalChangeLog, opts[0]);                 break;
                case RequestTypes.MessagesFromDe:   resultUri.AppendFormat(deBaseLink + messageDe, opts[0], opts[1]);               break;
                case RequestTypes.Schedule:         resultUri.Append(isuBaseLink + schedule + common + group + ApiKey + opts[0]);   break;
                case RequestTypes.ScheduleExam:     resultUri.Append(isuBaseLink + exams + common + group + ApiKey + opts[0]);      break;
                case RequestTypes.ScheduleTeacher:  resultUri.Append(isuBaseLink + schedule + common + teacher + ApiKey + opts[0]); break;
                case RequestTypes.ScheduleExamTeacher: resultUri.Append(isuBaseLink + exams + common + teacher + ApiKey + opts[0]); break;
                default: return null;
            }

            request = new HttpRequestMessage(HttpMethod.Get, resultUri.ToString());
            request.Headers.Add("User-Agent", "Mozilla/5.0");
            request.Headers.Add("Connection", "keep-alive");

            return request;
        }

        private static async Task<DataResponse<HttpStatusCode>> AuthOnDe(params string[] opts)
        {
            HttpStatusCode      result   = HttpStatusCode.ExpectationFailed;
            bool                isValid  = false;

            if(opts.Length != 2)
                return new DataResponse<HttpStatusCode>(result, isValid);

            try
            {
                var response = await httpClient.SendAsync(BuildRequest(RequestTypes.AuthDe, opts));
                result = response.StatusCode;
                isAuthiticated = true;
            }
            catch (HttpRequestException ex)     { result = HttpStatusCode.RequestTimeout; }
            catch (Exception ex)                { result = HttpStatusCode.ExpectationFailed; }

            return new DataResponse<HttpStatusCode>(result, isValid);
        }

        public static async Task<DataResponse<string>> ProccessRequest(RequestTypes type, params string[] opts)
        {
            HttpResponseMessage response = null;
            string              result  = null;
            bool                isValid = false;

            try
            {
                response    = await httpClient.SendAsync(BuildRequest(type, opts));
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
            return new DataResponse<string>(result, isValid);
        }

        public static async Task<DataResponse<string>> RetrieveData(RequestTypes type, params string[] opts)
        {
            DataResponse<string> result = null;

            switch (type)
            {
                case RequestTypes.Journal:
                case RequestTypes.JournalChangeLog:
                case RequestTypes.MessagesFromDe:
                    if (!isAuthiticated)
                    {
                        var authResult = await AuthOnDe(opts);
                        switch (authResult.Data)
                        {
                            case HttpStatusCode.OK: break;
                            case HttpStatusCode.ExpectationFailed:
                                return result = new DataResponse<string>(authResult.Data.ToString(), false);
                            default:
                                return result = new DataResponse<string>(authResult.Data.ToString(), false);
                        }
                    }
                    if(opts.Length == 3)
                        result = await ProccessRequest(type, opts[2]);
                    else
                        result = await ProccessRequest(type);

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
