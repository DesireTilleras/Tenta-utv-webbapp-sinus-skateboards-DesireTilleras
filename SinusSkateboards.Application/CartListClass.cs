using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SinusSkateboards.Domain;
using System;
using System.Collections.Generic;



namespace SinusSkateboards.Application
{
    public class CartListClass
    {
        public static List<ProductModel> ListOfCartItems { get; set; } = new List<ProductModel>();


    }
}
