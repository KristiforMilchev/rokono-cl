using System.Collections.Generic;

namespace RokonoDbManager.Models
{
    public class UmlBindingData
    {
        public List<OutboundTable> Tables { get; set; }
        public List<OutboundTableConnection> Connections { get; set; }
    }
}