using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eastlaws.Entities;

namespace Eastlaws.Services
{
    public interface IRestaurantData
    {
        IEnumerable<Restaurant> GetAll();
        Restaurant Get(int id);
        void Add(Restaurant newRestaurant);
    }
    public class InMemopryRestaurantData : IRestaurantData
    {
        static List<Restaurant> _restaurants;

        static InMemopryRestaurantData()
        {
            _restaurants = new List<Restaurant>
            {
                new Restaurant { Id=1,Name = "restaurant Number One"},
                new Restaurant { Id=2,Name = "restaurant Number Two"},
                new Restaurant { Id=3,Name = "restaurant Number Three"}
            };
        }

        public void Add(Restaurant newRestaurant)
        {
            
            newRestaurant.Id = _restaurants.Max(r => r.Id) + 1;
           _restaurants.Add(newRestaurant);
        }

        public Restaurant Get(int id)
        {
 
            return _restaurants.FirstOrDefault(r => r.Id == id);
        }

        public IEnumerable<Restaurant> GetAll()
        {
            return _restaurants;
        }
    }
}
