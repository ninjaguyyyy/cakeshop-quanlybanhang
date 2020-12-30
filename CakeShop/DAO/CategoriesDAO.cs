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
    class CategoriesDAO
    {
        public static List<Category> GetCategories()
        {
            var result = new List<Category>();

            string jsonFilePath = "./Data/categories.json";
            var json = File.ReadAllText(jsonFilePath);

            try
            {
                JArray categories = JArray.Parse(json);

                foreach (var item in categories)
                {
                    Category category = new Category();
                    category.Id = item["Id"].ToString();
                    category.Name = item["Name"].ToString();

                    result.Add(category);
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
