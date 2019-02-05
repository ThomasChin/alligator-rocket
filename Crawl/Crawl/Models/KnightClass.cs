using System;
using System.Collections.Generic;
using System.Text;

namespace Crawl.Models
{
    class KnightClass : BaseClass
    {
        public override string name { get { return "Knight"; } }
        public override int baseHealth { get { return 6; } }
        public override int baseAttack { get { return 6; } }
        public override int baseDefense { get { return 6; } }
        public override int baseSpeed { get { return 2; } }
    }
}
