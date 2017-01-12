using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;
using iTMO.Help.Model;

namespace iTMO.Help.Services.Net
{
    enum RequestTypes
    {
        Journal,
        JournalChangeLog,
        MessagesFromDe,
        Schedule,
        ScheduleExam
    }

    class HttpController
    {
        private HttpClient httpClient;
        private HttpResponseMessage httpResponse;

        public HttpController()
        {
            httpClient = new HttpClient();
        }

        private Uri BuildUri(RequestTypes request)
        {
            string result = "";

            switch(request)
            {
                case RequestTypes.Journal:
                    break;
                case RequestTypes.JournalChangeLog:
                    break;
                case RequestTypes.MessagesFromDe:
                    break;
                case RequestTypes.Schedule:
                    break;
                case RequestTypes.ScheduleExam:
                    break;
                default:
                    break;
            }

            return new Uri(result);
        }

        public Journal RetrieveJournal()
        {

            return null;
        }

        public JournalChangeLog RetrieveJournalChangeLog()
        {
            httpResponse = new HttpResponseMessage();

            string httpResponseBody = "";


            return null;
        }

        public MessageDe RetrieveMessagesFromDe()
        {

            return null;
        }

        public Schedule RetrieveSchedule()
        {

            return null;
        }

        public ScheduleExam RetrieveScheduleExam()
        {

            return null;
        }
    }
}
