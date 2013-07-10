using System;
using Earlz.TypeSafeHttp.IRequestBuilder;

namespace Earlz.TypeSafeHttp
{
	/*
	public class RequestBuilder : ICreateProtocol, ICreateUrl, INewState
	{
		HttpRequest Request=new HttpRequest();
		#region ICreateUrl implementation

		INewState ICreateUrl.At(string url)
		{
			Request.Url=url;
			return this;
		}

		#endregion

		#region ICreateProtocol implementation

		ICreateMethod ICreateProtocol.WithProtocol(string version)
		{
			Request.ProtocolVersion="1.1";
			return this;
		}

		#endregion

		#region ICreateMethod implementation

		ICreateUrl ICreateMethod.UsingMethod(string method)
		{
			Request.Method=method;
			return this;
		}

		#endregion

		public static ICreateProtocol Create()
		{
			return new RequestBuilder();
		}
		private RequestBuilder()
		{
			Request.ProtocolVersion="1.1";
		}
	}
	*/
}

