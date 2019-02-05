using System;
using System.Collections.Generic;
using System.Text;

namespace Crawl.Models
{
    public class BaseClass
    {
        public virtual string name { get { return "Base"; } }
        public virtual int baseHealth { get { return 5; } }
        public virtual int baseAttack { get { return 5; } }
        public virtual int baseDefense { get { return 3; } }
        public virtual int baseSpeed { get { return 2; } }
    }
}
