using System;
using System.Collections.Generic;
using System.Text;

namespace Crawl.Models
{
    public abstract class Level
    {
        //Array represents the 20 character levels and relative stats
        //Format: EXP, Attack, Defense, Speed
        int[,] Levels =  {
               {0     ,1,1,1},
               {300   ,1,2,1},
               {900   ,2,3,1},
               {2700  ,2,3,1},
               {6500  ,2,4,2},
               {14000 ,3,4,2},
               {23000 ,3,5,2},
               {34000 ,3,5,2},
               {48000 ,3,5,2},
               {64000 ,4,6,3},
               {85000 ,4,6,3},
               {100000,4,6,3},
               {120000,4,7,3},
               {140000,5,7,3},
               {165000,5,7,4},
               {195000,5,8,4},
               {225000,5,8,4},
               {265000,6,8,4},
               {305000,7,9,4},
               {355000,8,10,5}
          };

    }
}
