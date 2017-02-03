using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using System.Collections.Generic;
using iTMO.Help.Model;

namespace iTMO.Help.Controller
{
    /// <summary>
    /// Request types of the HttpController
    /// </summary>
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

    /// <summary>
    /// 
    /// </summary>
    class HttpController
    {
        private static HttpClient httpClient = new HttpClient();

        private static bool isAuthiticated = false;
        private static List<string> SessionCookie = null;

        private static HttpRequestMessage BuildRequest(RequestTypes type, params string[] opts)
        {
            HttpRequestMessage  request = null;
            StringBuilder       resultUri = new StringBuilder();

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
            string messageDe        = "/mail?days={0}";// &unreadOnly={0}";

            switch (type)
            {
                case RequestTypes.AuthDe:           resultUri.AppendFormat(deAuthLink, opts[0], opts[1]);                           break;
                case RequestTypes.Journal:          resultUri.Append(deBaseLink + journal);                                         break;
                case RequestTypes.JournalChangeLog: resultUri.AppendFormat(deBaseLink + journalChangeLog, opts[2]);                 break;
                case RequestTypes.MessagesFromDe:   resultUri.AppendFormat(deBaseLink + messageDe, opts[2]);               break;
                case RequestTypes.Schedule:         resultUri.Append(isuBaseLink + schedule + common + group + ApiKey + opts[0]);   break;
                case RequestTypes.ScheduleExam:     resultUri.Append(isuBaseLink + exams + common + group + ApiKey + opts[0]);      break;
                case RequestTypes.ScheduleTeacher:  resultUri.Append(isuBaseLink + schedule + common + teacher + ApiKey + opts[0]); break;
                case RequestTypes.ScheduleExamTeacher: resultUri.Append(isuBaseLink + exams + common + teacher + ApiKey + opts[0]); break;
                default: return null;
            }

            request = new HttpRequestMessage(HttpMethod.Get, resultUri.ToString());
            request.Headers.Add("User-Agent", "Mozilla/5.0");
            request.Headers.Add("Connection", "keep-alive");

            switch(type)
            {
                case RequestTypes.Journal:
                case RequestTypes.JournalChangeLog:
                case RequestTypes.MessagesFromDe:
                  
                    break;
                default: break;
            }

            return request;
        }

        private static async Task<DataResponse<string>> AuthOnDe(params string[] opts)
        {
            var authResult = new DataResponse<string>() { Code = HttpStatusCode.Unauthorized, isValid = false };

            if (opts.Length > 1)
            {
                try
                {
                    var response = await httpClient.SendAsync(BuildRequest(RequestTypes.AuthDe, opts));
                    if ((authResult.Code = response.StatusCode) == HttpStatusCode.OK)
                        authResult.isValid = true;
                    else
                        isAuthiticated = false;
                }
                catch (HttpRequestException ex) { authResult.Code = HttpStatusCode.RequestTimeout; }
                catch (Exception ex)            { authResult.Code = HttpStatusCode.ExpectationFailed;}
            }
            return authResult;
        }

        public static async Task<DataResponse<string>> ProccessRequest(RequestTypes type, params string[] opts)
        {
            DataResponse<string> dataResponse = new DataResponse<string>() { Data = "", isValid = false };

            try
            {
                var response = await httpClient.SendAsync(BuildRequest(type, opts));

                if (response != null)
                {
                    switch (dataResponse.Code = response.StatusCode)
                    {
                        case HttpStatusCode.OK:
                            dataResponse.Data = await response.Content.ReadAsStringAsync();
                            switch (type)
                            {
                                case RequestTypes.MessagesFromDe:
                                case RequestTypes.Journal:
                                case RequestTypes.JournalChangeLog: isAuthiticated = true; break;
                                default: break;
                            }
                            dataResponse.isValid = true;
                            break;
                        case HttpStatusCode.NotFound:       dataResponse.Data = "Resource not found, ops... [ 404 ]"; break;
                        case HttpStatusCode.NotAcceptable:  dataResponse.Data = "Think that schedule is unavaliable for now... Or check the group may be?"; break;
                        case HttpStatusCode.BadRequest:     dataResponse.Data = "Server got some unexpected error...  [ ITMO : Students] | [ 0 : 1 ]"; break;
                        case HttpStatusCode.Forbidden:      dataResponse.Data = "Think that we are outdated... Contact Developers or check ITMO site"; break;
                        case HttpStatusCode.NoContent:      dataResponse.Data = "Invalid Login/Password"; isAuthiticated = false; break;
                        default:                            dataResponse.Data = "Somethink extraodinary happened... Contact developer"; break;
                    }
                }
            }
            catch(HttpRequestException ex)  { dataResponse.Data = "Some Network Connectivity Error.. Check your Network"; }
            catch(Exception ex)             { dataResponse.Data = "Some unexpected error.."; }

            return dataResponse;
        }

        public static async Task<DataResponse<string>> RetrieveData(RequestTypes type, params string[] opts)
        {
            switch (type)
            {
                // DE
                case RequestTypes.Journal:
                case RequestTypes.JournalChangeLog:
                case RequestTypes.MessagesFromDe:
                    if (!isAuthiticated)
                    {
                        var authResult = await AuthOnDe(opts);

                        if (authResult.Code != HttpStatusCode.OK)
                            return new DataResponse<string>() { Data = authResult.Data.ToString(), isValid = isAuthiticated = false };
                    }
                    return await ProccessRequest(type, opts);
                // ISU
                default:
                    return await ProccessRequest(type, opts);
            }
        }
    }
}
