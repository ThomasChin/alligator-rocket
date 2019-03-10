using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Crawl.Models.Enums
{

    /// <summary>
    /// Class to convert the hit enum to a string
    /// Used by the message builder to build good debug output as well as UX strings
    /// So that a hit can say " Hit "
    /// and a miss can say " misses "
    /// and a critical hit can say " wow, you knocked the snot out of your opponent"
    /// </summary>
    public class BattleMessageHitEnum
    {

        #region Singleton
        // Make this a singleton so it only exist one time because holds all the data records in memory
        private static BattleMessageHitEnum _instance;

        // Constructor for BMHitEnum
        public static BattleMessageHitEnum Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new BattleMessageHitEnum();
                }
                return _instance;
            }
        }

        #endregion Singleton

        // List for messages.
        private List<MessageDetailHitEnum> MessageList { get; set; }

        // Data for the Levels
        public BattleMessageHitEnum()
        {
            ClearAndLoadDatTable();
        }

        // Make new list and load data.
        public void ClearAndLoadDatTable()
        {
            MessageList = new List<MessageDetailHitEnum>();
            LoadData();
        }

        // Load Enum Data.
        public void LoadData()
        {
            MessageList.Add(new MessageDetailHitEnum { HitStatus = HitStatusEnum.Unknown, Message = "Unknown" });
            MessageList.Add(new MessageDetailHitEnum { HitStatus = HitStatusEnum.Hit, Message = " hits " });
            MessageList.Add(new MessageDetailHitEnum { HitStatus = HitStatusEnum.Miss, Message = " misses " });
            MessageList.Add(new MessageDetailHitEnum { HitStatus = HitStatusEnum.CriticalMiss, Message = " misses really badly" });
            MessageList.Add(new MessageDetailHitEnum { HitStatus = HitStatusEnum.CriticalHit, Message = " hits really hard " });
        }

        // Level details for each level
        private class MessageDetailHitEnum
        {
            public HitStatusEnum HitStatus;
            public string Message;
        }

        // Get the current message based on hit status.
        public string GetMessage(HitStatusEnum hitStatus)
        {
            var myReturn = MessageList.Where(a => a.HitStatus == hitStatus).FirstOrDefault().Message;
            return myReturn;
        }
    }
}
