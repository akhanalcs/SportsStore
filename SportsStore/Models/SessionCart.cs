using System;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SportsStore.Infrastructure;

namespace SportsStore.Models
{
    // If you return an object ('SessionCart') that is cast to the base class ('Cart' as in the return type of GetCart), it
    // uses derived object's methods and properties.
    // Reference: https://stackoverflow.com/q/8329470/8644294
    public class SessionCart : Cart
    {
        [JsonIgnore]
        public ISession Session { get; set; }

        //Factory for creating SessionCart objects
        public static Cart GetCart(IServiceProvider services)
        {
            var session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
            var sessionCart = session?.GetJson<SessionCart>("Cart") ?? new SessionCart();
            sessionCart.Session = session;
            return sessionCart;
        }

        //Type "override *start typing virtual method name* and hit enter, it completes the code snippet!"
        public override void AddItem(Product product, int quantity)
        {
            base.AddItem(product, quantity);
            Session.SetJson("Cart", this);
        }

        public override void RemoveLine(Product product)
        {
            base.RemoveLine(product);
            Session.SetJson("Cart", this);
        }

        public override void Clear()
        {
            base.Clear();
            Session.Remove("Cart");
        }
    }
}
