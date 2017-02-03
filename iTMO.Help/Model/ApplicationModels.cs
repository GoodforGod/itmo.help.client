using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace iTMO.Help.Model
{
    /// <summary>
    /// HttpController 
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    class DataResponse<TValue>
    {
        public TValue           Data    { get; set; }
        public HttpStatusCode   Code    { get; set; }
        public bool             isValid { get; set; }
        public string           Message { get; set; }
    }

    /// <summary>
    /// Serialisation response with valid state and exception method
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    class SerializeData<TValue>
    {
        public TValue   Data    { get; set; }
        public bool     IsValid { get; set; } = false;
        public string   Message { get; set; } = "";
    }

    /// <summary>
    /// Used as response from methods which try to ChechOrAndRemember user's info
    /// </summary>
    public class CheckResponse
    {
        public bool     IsRemember  { get; set; } = false;
        public bool     IsValid     { get; set; } = false;
        public string   Login       { get; set; } = "";
        public string   Password    { get; set; } = "";
        public string   Message     { get; set; } = "";
    }

    /// <summary>
    /// Used for Main menu <see cref="MainPage"/>
    /// </summary>
    public class MenuItem
    {
        public string Title { get; set; } = "";
        public string Icon { get; set; } = "";
        public Type Page { get; set; }
    }
}
