using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Course.Shared.Dtos
{
    public class Response<T>
    {
        public T Data { get; private set; }// dışarıdan set edilmesin private set
        [JsonIgnore] //Responsun icinde statu code olmasın.
        public int StatusCode { get; set; }
        [JsonIgnore]
        public bool IsSuccessfull { get; private  set; }

        public List<string> Errors { get; set; }
        #region Static Factory Method
        public static Response<T> Success(T data,int statusCode)
        {
            return new Response<T> { Data = data, StatusCode = statusCode, IsSuccessfull = true };
        }
        public static Response<T> Success( int statusCode)
        {
            return new Response<T> {Data = default(T) ,StatusCode = statusCode, IsSuccessfull = true };
        }

        public static Response<T> Fail(List<string> errors,int statusCode)
        {
            return new Response<T> { Errors = errors, StatusCode = statusCode, IsSuccessfull = false };
        }
        public static Response<T> Fail(string errors, int statusCode)
        {
        
            return new Response<T> { Errors = new List<string>(){errors}, StatusCode = statusCode, IsSuccessfull = false };
        }

        #endregion
    }
}
