using System;
using System.Collections.Generic;
using System.Text;

namespace Earlz.TypeSafeHttp
{
	public class HttpRequest
	{
		public string ProtocolVersion{get;set;}
		public string Url{get;set;}
		public string Method{get;set;}
		public string Body{get;set;}
		List<KeyValuePair<string, string>> Headers=new List<KeyValuePair<string, string>>();
		public HttpRequest()
		{
		}
		public void AddHeader(string name, string value)
		{
			Headers.Add(new KeyValuePair<string, string>(name, value));
		}
		public string Render()
		{
			StringBuilder sb=new StringBuilder();
			sb.AppendFormat("HTTP/{0} {1} {2}",ProtocolVersion, Method, Url);
			sb.AppendLine();
			foreach(var h in Headers)
			{
				sb.AppendFormat("{0}:{1}", h.Key, h.Value);
				sb.AppendLine();
			}
			sb.AppendLine();
			sb.Append(Body);
			return sb.ToString();
		}
	}
}

