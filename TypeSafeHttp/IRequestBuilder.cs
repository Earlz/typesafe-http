using System;

namespace Earlz.TypeSafeHttp.IRequestBuilder
{
	public interface ICreateProtocol : ICreateMethod
	{
		ICreateMethod WithProtocol(string version);
	}
	public interface ICreateMethod
	{
		ICreateUrlPost Post();
		ICreateUrlGet Get();
	}
	public interface ICreateUrlGet
	{
		INewGet Url(string url);
	}
	public interface ICreateUrlPost
	{
		INewPost Url(string url);
	}

	public interface INewGet
	{

	}
	public interface INewPost
	{

	}

}

