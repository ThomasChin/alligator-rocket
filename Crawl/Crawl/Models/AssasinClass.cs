using System;
using System.Collections.Generic;
using System.Text;

namespace Crawl.Models
{
    class AssasinClass : BaseClass
    {
        public override string name { get { return "Assasin"; } }
        public override int baseHealth { get { return 3; } }
        public override int baseAttack { get { return 5; } }
        public override int baseDefense { get { return 3; } }
        public override int baseSpeed { get { return 8; } }
    }
}
