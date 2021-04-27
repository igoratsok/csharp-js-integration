using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Linq;

namespace JSIntegration
{
    /// <summary>
    /// Jasper server integration for generating reports.
    /// 
    /// <author>Igor Augusto de Faria Costa</author>
    /// </summary>
    public class JasperServerIntegration
    {
        private string _jasperUrl;
        private string _reportPath;
        private string _type;
        private string _user;
        private string _password;
        private IDictionary<string, string> _parameters;

        /// <summary>
        /// Initializes the instance of the <see cref="T:JSIntegration.JasperServerIntegration"/> class.
        /// </summary>
        /// <param name="jasperUrl">Jaspersoft Server base URL.</param>
        /// <param name="reportPath">Report Unit path.</param>
        /// <param name="type">Export type, eg.: pdf, xlsx, docx.</param>
        /// <param name="user">Username that has access to the Report Unit.</param>
        /// <param name="password">User password.</param>
        /// <param name="parameters">Dictionary with the parameters to the report.</param>
        /// 
        public JasperServerIntegration(string jasperUrl, string reportPath, string type, string user, string password, IDictionary<string, string> parameters)
        {
            this._jasperUrl = jasperUrl;
            this._reportPath = reportPath;
            this._type = type;
            this._user = user;
            this._password = password;
            this._parameters = parameters;
        }

        /// <summary>
        /// Initializes the instance of the <see cref="T:JSIntegration.JasperServerIntegration"/> class. 
        /// You should use this if the report does not have parameters.
        /// 
        /// </summary>
        /// <param name="jasperUrl">Jaspersoft Server base URL.</param>
        /// <param name="reportPath">Report Unit path.</param>
        /// <param name="type">Export type, eg.: pdf, xlsx, docx.</param>
        /// <param name="user">Username that has access to the Report Unit.</param>
        /// <param name="password">User password.</param>
        public JasperServerIntegration(string jasperUrl, string reportPath, string type, string user, string password) : this(jasperUrl, reportPath, type, user, password, new Dictionary<string, string>())
        {
        }

        /// <summary>
        /// Adds a parameter to the parameter list.
        /// </summary>
        /// <param name="parameter">Parameter name.</param>
        /// <param name="value">Parameter value.</param>
        public void AddParameter(string parameter, string value) 
        {
            _parameters.Add(parameter, value);
        }

        private string _getQueryString()
        {
            string queryString = ""; 

            foreach(KeyValuePair<string, string> entry in _parameters)
            {
                queryString += (queryString.Length == 0 ? '?' : '&') + entry.Key + "=" + entry.Value;
            }

            return queryString;
        }

        private string _getAuthenticationString()
        {
            return "Basic " + System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(_user + ":" + _password));
        }

        /// <summary>
        /// Executes the report and returns an array of bytes of the generated report.
        /// </summary>
        /// <returns>The array of bytes of the generated report.</returns>
        public byte[] Execute()
        {
            string url = this._jasperUrl + "/rest_v2/reports/" + this._reportPath + '.' + this._type + this._getQueryString();

            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
            request.Headers.Add("Authorization", _getAuthenticationString());


            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                Stream stream = response.GetResponseStream();
                MemoryStream ms = new MemoryStream();
                stream.CopyTo(ms);

                byte[] data = ms.ToArray();

                return data;
            } catch(WebException e)
            {
                Stream stream = e.Response.GetResponseStream();
                StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                string result = readStream.ReadToEnd();
                var xml = System.Xml.Linq.XElement.Parse(result);
                string errorCode = xml.Elements("errorCode").FirstOrDefault().Value;
                string errorMessage = xml.Elements("message").FirstOrDefault().Value;

                throw new JSIntegrationException(errorCode, errorMessage);
            }



        }



    }
}