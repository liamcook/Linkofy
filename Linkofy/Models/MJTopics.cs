using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Linkofy.Models
{
    public class MJTopics
    {
        public enum MJTopicTF
        {
            Adult,
            Arts,
            Business,
            Computers,
            General,
            Games,
            Green,
            Life,
            Fashion,
            Fitness,
            Health,
            Home,
            Jewellery,
            Marketing,
            News,
            Recreation,
            Reference,
            Regional,
            Science,
            Shopping,
            Society,
            Sports,
            Tech,
            Vehicles,
            Wedding,
            World
        }


        public int ID { get; set; }

        public MJTopicTF? topicalTF { get; set; }

        public virtual ICollection<Identifier> Identifiers { get; set; }

        public virtual ICollection<Client> Clients { get; set; }
    }
}