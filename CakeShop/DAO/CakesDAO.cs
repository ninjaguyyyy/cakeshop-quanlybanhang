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
    class CakesDAO
    {
        public static bool InsertCake(Cake cakeEntered)
        {
            var result = true;

            string jsonFilePath = "./Data/cakes.json";
            var json = File.ReadAllText(jsonFilePath);

            try
            {
                JArray cakes = JArray.Parse(json);

                var newCakeString = Newtonsoft.Json.JsonConvert.SerializeObject(cakeEntered);
                var newCakeJObject = JObject.Parse(newCakeString);
                cakes.Add(newCakeJObject);

                string newJsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(cakes,
                               Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(jsonFilePath, newJsonResult);
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public static List<Cake> GetCake()
        {
            var result = new List<Cake>();

            string jsonFilePath = "./Data/cakes.json";
            var json = File.ReadAllText(jsonFilePath);

            try
            {
                JArray cakes = JArray.Parse(json);

                //int skipValue = pageCurrent == 1 ? 0 : (pageCurrent - 1) * perPage;
                //trips = new JArray(trips.Skip(skipValue).Take(perPage));

                foreach (var cake in cakes)
                {
                    Cake cakeObject = new Cake();
                    cakeObject.Id = cake["Id"].ToString();
                    cakeObject.Name = cake["Name"].ToString();
                    cakeObject.Description = cake["Description"].ToString();
                    cakeObject.Price = int.Parse(cake["Price"].ToString());
                    cakeObject.Image_Main = cake["Image_Main"].ToString();
                    cakeObject.Image_Main = cake["Image_Main"].ToString();

                    var categoryObj = JObject.Parse(cake["Category"].ToString());
                    Category category = new Category()
                    {
                        Id = categoryObj["Id"].ToString(),
                        Name = categoryObj["Name"].ToString()
                    };
                    cakeObject.Category = category;

                    result.Add(cakeObject);
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
