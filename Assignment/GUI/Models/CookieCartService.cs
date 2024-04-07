using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Assignment.Models;

public class CookieCartService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private const string CartCookieName = "Cart";

    public CookieCartService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }


    public List<OrderDetail> GetCart()
    {
        var cartJson = _httpContextAccessor.HttpContext.Request.Cookies[CartCookieName];
        if (cartJson == null)
        {
            return new List<OrderDetail>();
        }
        return JsonConvert.DeserializeObject<List<OrderDetail>>(cartJson);
    }

    public void UpdateCart(List<OrderDetail> cart)
    {
        var cartJson = JsonConvert.SerializeObject(cart);
        _httpContextAccessor.HttpContext.Response.Cookies.Append(CartCookieName, cartJson, new CookieOptions
        {
            Expires = DateTime.Now.AddHours(1) // You can adjust the expiration as needed
        });
    }
    public void ResetCart()
    {
        // Clear the cart cookie by setting its value to an empty string and immediately expiring it
        _httpContextAccessor.HttpContext.Response.Cookies.Append(CartCookieName, "", new CookieOptions
        {
            Expires = DateTime.Now.AddHours(-1), // Expire in the past to immediately clear the cookie
        });
    }

}
