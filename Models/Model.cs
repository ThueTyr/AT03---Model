using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AT03___Model.Models
{
    public class Model
    {
        public string Name { get; set; }
        public string Tlf { get; set; }
        public string Address { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public string HairColor { get; set; }
        public string Comments { get; set; }

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
