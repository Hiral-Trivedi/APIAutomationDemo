using automationAPIDemo1.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace automationAPIDemo1.Routes
{
    public class APILayer
    {


        public static bool IsNullCheck(PropertyInfo pi, Object obj)
        {
            try
            {
                pi.GetValue(obj).ToString();
                return true;
            }
            catch
            {
                return false;
            }
        }


        public static string GetTransactionPreferenceRequestToMuleSoft(string baseurl, string endpoint, FeatureFileData fromfeature, DataRow[] dtrwauthparam)
        {
            DataRow[] dtrwauthheaders=null;
            //DataTableOperations.setValueForColumn(PostRequestToMuleSoftEndPoint.getdtrwauthparam(), "Parameter", "Authorization", "ParameterValue", bearervalue);
            foreach (PropertyInfo pi in fromfeature.GetType().GetProperties())
            {
                for (int i = 0; i < dtrwauthparam.Count(); i++)
                {
                    if (pi.Name.ToString().Equals(dtrwauthparam[i]["Parameter"].ToString()) && IsNullCheck(pi, fromfeature))
                    {
                        setValueForColumn(dtrwauthparam, "Parameter", dtrwauthparam[i]["Parameter"].ToString(), "ParameterValue", pi.GetValue(fromfeature).ToString());
                    }
                }
            }
            string respval = GetRequest(baseurl, "data/2.5/forecast/hourly", dtrwauthheaders, dtrwauthparam);
            return respval;
        }



        public static DataRow[] setValueForColumn(DataRow[] dtrw, string identifiercol, string identifiercolvalue, string columntoset, string valuetoset)
        {
            for (int i = 0; i < dtrw.Count(); i++)
            {
                if (dtrw[i][identifiercol].Equals(identifiercolvalue))
                {
                    dtrw[i][columntoset] = valuetoset;
                }
            }
            return dtrw;
        }


        public static string GetRequest(string baseUrl, string resourceUri, DataRow[] headers, DataRow[] parameters)
        {
            RestClient client = new RestClient(baseUrl);
            RestRequest request = new RestRequest(resourceUri, Method.GET);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            if (headers!=null && headers.Count() > 0)
            {
                AddRequestHeaders(request, headers);
            }
            if (parameters.Count() > 0)
            {
                AddRequestParameters(request, parameters);
            }
            var resp = client.Execute(request);
            if (resp.StatusCode.ToString().Equals("OK") || resp.StatusCode.ToString().Equals("BadRequest"))
            {
                return resp.Content.ToString();
            }
            else
            {
                return resp.StatusDescription;
            }


        }


        public static RestRequest AddRequestHeaders(RestRequest request, DataRow[] headers)
        {
            for (int i = 0; i < headers.Count(); i++)
            {
                request.AddHeader(headers[i]["Header"].ToString(), headers[i]["HeaderValue"].ToString());
            }
            return request;
        }

        private static RestRequest AddRequestParameters(RestRequest request, DataRow[] parameters)
        {
            for (int i = 0; i < parameters.Count(); i++)
            {
                if (parameters[i]["ParameterType"].ToString().Equals("NA") && !(parameters[i]["ParameterValue"].ToString().Equals("ignore")))
                    request.AddParameter(parameters[i]["Parameter"].ToString(), parameters[i]["ParameterValue"].ToString());
                else
                {
                    if (parameters[i]["ParameterType"].ToString().Equals("HttpHeader"))
                    {
                        request.AddParameter(parameters[i]["Parameter"].ToString(), parameters[i]["ParameterValue"].ToString(), ParameterType.HttpHeader);
                    }
                    else if (parameters[i]["ParameterType"].ToString().Equals("RequestBody"))
                    {
                        request.AddParameter(parameters[i]["Parameter"].ToString(), parameters[i]["ParameterValue"].ToString(), ParameterType.RequestBody);
                    }
                    else if (parameters[i]["ParameterType"].ToString().Equals("Cookie"))
                    {
                        request.AddParameter(parameters[i]["Parameter"].ToString(), parameters[i]["ParameterValue"].ToString(), ParameterType.Cookie);
                    }



                    else if (parameters[i]["ParameterType"].ToString().Equals("GetOrPost"))
                    {
                        request.AddParameter(parameters[i]["Parameter"].ToString(), parameters[i]["ParameterValue"].ToString(), ParameterType.GetOrPost);
                    }



                    else if (parameters[i]["ParameterType"].ToString().Equals("UrlSegment"))
                    {
                        request.AddParameter(parameters[i]["Parameter"].ToString(), parameters[i]["ParameterValue"].ToString(), ParameterType.UrlSegment);
                    }



                    else if (parameters[i]["ParameterType"].ToString().Equals("QueryString"))
                    {
                        request.AddParameter(parameters[i]["Parameter"].ToString(), parameters[i]["ParameterValue"].ToString(), ParameterType.QueryString);
                    }
                    else if (parameters[i]["ParameterType"].ToString().Equals("QueryStringWithoutEncode"))
                    {
                        request.AddParameter(parameters[i]["Parameter"].ToString(), parameters[i]["ParameterValue"].ToString(), ParameterType.QueryStringWithoutEncode);
                    }
                }






            }
            return request;
        }


        public static T GetJsonobj<T>(string jsonreq)
        {
            var jsonobj = JsonConvert.DeserializeObject<T>(jsonreq, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            return jsonobj;
        }


    }
}
