using System.Collections.Generic;

namespace RokonoDbManager.Models
{
    public class OutboundTable
    {
        public string Id { get; set; }
        public Shape Shape { get; set; }
        public int OffsetX {get; set;}
        public int OffsetY {get; set;}

    }
}