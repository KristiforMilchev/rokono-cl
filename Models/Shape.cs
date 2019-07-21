using System.Collections.Generic;

namespace RokonoDbManager.Models
{
    public class Shape
    {
        public List<BindingRow> Attribute { get; set; }
        public string Name { get; set; }
        public string Classifier { get; set; }

    }
}