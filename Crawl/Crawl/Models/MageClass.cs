using System;
using System.Collections.Generic;
using System.Text;

namespace Crawl.Models
{
    class MageClass : BaseClass
    {
        public override string name { get { return "Mage"; } }
        public override int baseHealth { get { return 4; } }
        public override int baseAttack { get { return 7; } }
        public override int baseDefense { get { return 3; } }
        public override int baseSpeed { get { return 4; } }
    }
}
