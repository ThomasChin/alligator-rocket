using System;
using System.Collections.Generic;
using System.Text;


namespace Crawl.Models
{
    public abstract class Dice
    {
        public static int Roll(int faces, int count)
        {
            Random r = new Random();
            int sum = 0;

            for(int i = 0; i < count; i++)
                sum += r.Next(0, faces);

            return sum;
        }
    }
}
