using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Warframeaccountant.domain
{
    public class Item
    {
        public int itemId { get; set; }

        public string name { get; set; }

        public string imageString { get; set; }

        public int type { get; set; }

        public int quantity { get; set; }

        public int bprice {get; set;}

        public int eprice { get; set; }
    }
}
