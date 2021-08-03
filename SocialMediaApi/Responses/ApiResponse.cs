using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaApi.Responses
{
    //Esta clase para hacer el response de mis APIs
    public class ApiResponse<T> 
    {
        public ApiResponse(T data)
        {
            Data = data;
        }
        public T Data { get; set; }

    }
}
