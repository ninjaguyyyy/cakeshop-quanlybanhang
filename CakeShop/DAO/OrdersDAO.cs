using CakeShop.DTO;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeShop.DAO
{
    class OrdersDAO
    {
        public static bool InsertOrder(Order orderEntered)
        {
            var result = true;

            string jsonFilePath = "./Data/orders.json";
            var json = File.ReadAllText(jsonFilePath);

            try
            {
                JArray orders = JArray.Parse(json);

                var newOrderString = Newtonsoft.Json.JsonConvert.SerializeObject(orderEntered);
                var newOrderJObject = JObject.Parse(newOrderString);
                orders.Add(newOrderJObject);

                string newJsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(orders,
                               Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(jsonFilePath, newJsonResult);
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public static List<Order> GetOrders()
        {
            var result = new List<Order>();

            string jsonFilePath = "./Data/orders.json";
            var json = File.ReadAllText(jsonFilePath);

            try
            {
                JArray orders = JArray.Parse(json);

                //int skipValue = pageCurrent == 1 ? 0 : (pageCurrent - 1) * perPage;
                //trips = new JArray(trips.Skip(skipValue).Take(perPage));

                foreach (var order in orders)
                {
                    Order orderObject = new Order();
                    orderObject.Id = order["Id"].ToString();
                    orderObject.Name = order["Name"].ToString();
                    orderObject.TotalPrice = int.Parse(order["TotalPrice"].ToString());
                    orderObject.CreatedDate = order["CreatedDate"].ToString();

                    orderObject.CartItems = new List<CartItem>();
                    JArray CartItems = JArray.Parse(order["CartItems"].ToString());
                    foreach (var cartItem in CartItems)
                    {
                        CartItem newCartItem = new CartItem()
                        {
                            Id = cartItem["Id"].ToString(),
                            Quantity = int.Parse(cartItem["Quantity"].ToString()),
                            TotalPrice = int.Parse(cartItem["TotalPrice"].ToString()),
                            
                        
                        };
                        var cakeObj = JObject.Parse(cartItem["Cake"].ToString());
                        Cake cake = new Cake()
                        {
                            Id = cakeObj["Id"].ToString(),
                            Name = cakeObj["Name"].ToString(),
                            Description = cakeObj["Description"].ToString(),
                            Price = int.Parse(cakeObj["Price"].ToString()),
                            Image_Main = cakeObj["Image_Main"].ToString(),
                        };
                        newCartItem.Cake = cake;

                        orderObject.CartItems.Add(newCartItem);
                    }

                    result.Add(orderObject);
                }
            }
            catch (Exception)
            {
                throw;
            }



            return result;
        }
    }
}
