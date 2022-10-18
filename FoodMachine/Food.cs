using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodMachine
{
    internal class Food
    {

        public string? Name { get; set; }
        public double Price { get; set; }
        public byte Count { get; set; }


        public Food(string? name, double price, byte count)
        {
            Name = name;
            Price = price;
            Count = count;
        }


        public override string ToString()
        {
            return @$"Name : {Name}
Price : {Price}
Count : {Count}";
        }
    }
}
