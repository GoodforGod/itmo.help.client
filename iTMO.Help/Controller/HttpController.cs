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
    /// Avaliable request types of the HttpController
    /// </summary>
    public enum TRequest
    {
        /// <summary>
        /// Reqiure 2 params, LOGIN & PASSWD
        /// </summary>
        DeAuth              = 1,
        /// <summary>
        /// Doesn't reqiure params
        /// </summary>
        DeJournal           = 3,
        /// <summary>
        /// Require 1 param, DAYS
        /// </summary>
        DeJournalChangeLog  = 5,
        /// <summary>
        /// Require 1 param, DAYS
        /// </summary>
        DeMessages          = 7,
        /// <summary>
        /// Require 3 params, DATA & LOGIN & PASSWD
        /// </summary>
        DeAuthAttestation   = 9,
        /// <summary>
        /// Require 3 params, DATA & LOGIN & PASSWD
        /// </summary>
        DeAttestationRegister = 11,
        /// <summary>
        /// Require 2 params, SEMESTER_ID & GROUP
        /// </summary>
        DeAttestationSchedule = 100,
        /// <summary>
        /// Require 1 param, GROUP
        /// </summary>
        Schedule            = 0,
        /// <summary>
        /// Require 1 param, GROUP
        /// </summary>
        ScheduleExam        = 2,
        /// <summary>
        /// Require 1 param, TEACHER_ID
        /// </summary>
        ScheduleTeacher     = 4,
        /// <summary>
        /// Require 1 param, TEACHER_ID
        /// </summary>
        ScheduleExamTeacher = 6
    }

    /// <summary>
    /// Used to communicate with (ISU & DE) API throught HTTP(S) transport protocol
    /// </summary>
    class HttpController
    {
        /// <summary>
        /// Main application Http end point client
        /// </summary>
        private static HttpClient httpClient = new HttpClient();

        /// <summary>
        /// Indicates is user Authiticated on DE services
        /// </summary>
        private static bool isAuthiticated = false;

        /// <summary>
        /// Builds HttpRequests for <see cref="ProcessRequest(TRequest, UserData)"/>
        /// </summary>
        private static HttpRequestMessage BuildHttpRequest(TRequest type, UserData user)
        {
            HttpRequestMessage  request = null;
            StringBuilder       resultUri = new StringBuilder();

            string isuApiKey         = "/AYPlvngDDdVoZdJgPAKoeHQnWUDrvOwEHpEXxNGNfaAKsugiJyCGxotWsTdqCdHL/";
            string isuBaseLink       = "https://isu.ifmo.ru/ords/isurest/v1/api/public";

            string deBaseLink        = "https://de.ifmo.ru/api/private";
            string deAuthLink        = "https://de.ifmo.ru/servlet/?Rule=LOGON&LOGIN={0}&PASSWD={1}";
            string deAuthAttestation = "https://de.ifmo.ru/--schedule/index.php?data={0}&login={1}&passwd={2}&role=%D1%F2%F3%E4%E5%ED%F2";
            string deAttestationSchedule    = "http://de.ifmo.ru/?node=schedule&index=sched&semiId={0}&group={1}";

            // ISU
            string schedule         = "/schedule";
            string exams            = "/exams";
            string group            = "/group";
            string teacher          = "/teacher";
            string common           = "/common";

            // DE
            string journal          = "/eregister";
            string journalChangeLog = "/eregisterlog?days={0}";
            string messageDe        = "/mail?days={0}"; // &unreadOnly={0}";

            switch (type)
            {
                // DE
                case TRequest.DeAttestationSchedule:
                    resultUri.AppendFormat(deAttestationSchedule, user.Data.Opts[0], user.Data.Group);
                    break;
                case TRequest.DeAuthAttestation:
                    resultUri.AppendFormat(deAuthAttestation, user.Data.Opts[0], user.Data.Login, user.Data.Password);
                    break;                                
                case TRequest.DeAuth:
                    resultUri.AppendFormat(deAuthLink, user.Data.Login, user.Data.Password);
                    break;
                case TRequest.DeJournal:
                    resultUri.Append(deBaseLink + journal);
                    break;
                case TRequest.DeJournalChangeLog:
                    resultUri.AppendFormat(deBaseLink + journalChangeLog, user.Data.Opts[0]);
                    break;
                case TRequest.DeMessages:
                    resultUri.AppendFormat(deBaseLink + messageDe, user.Data.Opts[0]);
                    break;

                // ISU
                case TRequest.Schedule:
                    resultUri.Append(isuBaseLink + schedule + common + group + isuApiKey + user.Data.Group);
                    break;
                case TRequest.ScheduleExam:
                    resultUri.Append(isuBaseLink + exams + common + group + isuApiKey + user.Data.Group);
                    break;
                case TRequest.ScheduleTeacher:
                    resultUri.Append(isuBaseLink + schedule + common + teacher + isuApiKey + user.Data.Opts[0]);
                    break;
                case TRequest.ScheduleExamTeacher:
                    resultUri.Append(isuBaseLink + exams + common + teacher + isuApiKey + user.Data.Opts[0]);
                    break;
                default: return null;
            }

            request = new HttpRequestMessage(HttpMethod.Get, resultUri.ToString());
            request.Headers.Add("User-Agent", "Mozilla/5.0");
            request.Headers.Add("Connection", "keep-alive");

            return request;
        }

        /// <summary>
        /// Authorize on DE account "de.ifmo.ru"
        /// </summary>
        /// <returns> <see cref="HttpData{TValue}"/> </returns>
        private static async Task<HttpData<string>> AuthOnDe(UserData user)
        {
            var authResult = new HttpData<string>() { Code = HttpStatusCode.Unauthorized, isValid = false };

            if (user.Data.Opts.Count > 1)
            {
                try
                {
                    var response = await httpClient.SendAsync(BuildHttpRequest(TRequest.DeAuth, user));
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

        /// <summary>
        /// Process httpRequest
        /// </summary>
        /// <returns> <see cref="HttpData{TValue}"/> </returns>
        private static async Task<HttpData<string>> ProcessRequest(TRequest type, UserData user)
        {
            HttpData<string> dataResponse = new HttpData<string>() { Data = "", isValid = false };

            try
            {
                var response = await httpClient.SendAsync(BuildHttpRequest(type, user));

                if (response != null)
                {
                    switch (dataResponse.Code = response.StatusCode)
                    {
                        case HttpStatusCode.OK:
                            dataResponse.Data = await response.Content.ReadAsStringAsync();
                            switch (type)
                            {
                                case TRequest.DeMessages:
                                case TRequest.DeJournal:
                                case TRequest.DeJournalChangeLog: isAuthiticated = true; break;
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

        /// <summary>
        /// Retrive data from DE/ISU
        /// </summary>
        /// <param name="type">
        /// Request paramer type, used to process stratagy for specific request
        /// </param>
        /// <param name="opts">
        /// Parameters for the specific request type, read REQUEST TYPE SUMMARY!
        /// </param>
        /// <returns></returns>
        public static async Task<HttpData<string>> RetrieveData(TRequest type, UserData user)
        {
            switch (type)
            {
                // Process DE
                case TRequest.DeJournal:
                case TRequest.DeJournalChangeLog:
                case TRequest.DeMessages:
                case TRequest.DeAuthAttestation:
                    if (!isAuthiticated)
                    {
                        var authResult = await AuthOnDe(user);
                        if (authResult.Code != HttpStatusCode.OK)
                            return authResult;
                    }
                    return await ProcessRequest(type, user);

                // Process ISU & DE AttestationSchedule
                default:
                    return await ProcessRequest(type, user);
            }
        }
    }
}
