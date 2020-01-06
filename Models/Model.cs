using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AT03___Model.Models
{
    public class Model
    {
        public string Name;
        public string Tlf;
        public string Address;
        public int Height;
        public int Weight;
        public string HairColor;
        public string Comments;

        public Model() { }

        public Model(string name, string tlf, string address,
            int height, int weight, string hairColor, string comments)
        {
            Name = name;
            Tlf = tlf;
            Address = address;
            Height = height;
            Weight = weight;
            HairColor = hairColor;
            Comments = comments;
        }
    }
}
