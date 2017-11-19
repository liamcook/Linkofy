using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Linkofy.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;


namespace Linkofy.Models
{
    public class LinksDBInitialiser : System.Data.Entity.DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            var Clients = new List<Client>
            {
                new Client{ID=1,clientN="Orla James",monthlyQuota=10,MJTopicsID=23,UserTableID=1},
                new Client{ID=2,clientN="Float Spa",monthlyQuota=4,MJTopicsID=11,UserTableID=1},
                new Client{ID=3,clientN="Moto FX",monthlyQuota=6,MJTopicsID=22,UserTableID=2},
                  };

            Clients.ForEach(s => context.Clients.Add(s));
            context.SaveChanges();
            var Identifiers = new List<Identifier>
            {
                new Identifier{ID=1,domain="Vogue.com",contact="emma@gmail.com",contactname="emma",price=35,TF=34,CF=29,RI=389,MJTopicsID=9,type=Identifier.Ltype.ExistingLink,UserTableID=1},
                new Identifier{ID=2,domain="Fooyoh.com",contact="2emma@gmail.com",contactname="2emma",price=40,TF=37,CF=34,RI=2765,MJTopicsID=5,type=Identifier.Ltype.ExistingLink,UserTableID=1},
                new Identifier{ID=3,domain="Techwiz.com",contact="3emma@gmail.com",contactname="2emma",price=60,TF=52,CF=58,RI=5859,MJTopicsID=22,type=Identifier.Ltype.ExistingLink,UserTableID=2},
                    };

            Identifiers.ForEach(s => context.Identifiers.Add(s));
            context.SaveChanges();
            var Links = new List<Link>
            {
                new Link{IdentifierID=1,Obdomain="Vogue.com/findyourperfectring",Obpage="Orlajames.com/wedding-rings",ClientID=1,Anchor="Wedding Rings",BuildDate=DateTime.Parse("2017-11-11"),UserTableID=1},
                new Link{IdentifierID=1,Obdomain="Vogue.com/flotationtherapyoffer",Obpage="Orlajames.com/wedding-rings/platinum",ClientID=1,Anchor="Platinum Rings",BuildDate=DateTime.Parse("2017-07-27"),UserTableID=1},
                new Link{IdentifierID=2,Obdomain="Fooyoh.com/wrappingyourcar",Obpage="motoFX.com/wrapping", ClientID=3,Anchor="Car Wrapping",BuildDate=DateTime.Parse("2017-07-29"),UserTableID=1},
                  };

            Links.ForEach(s => context.Links.Add(s));
            context.SaveChanges();
            var Statuss = new List<Status>
            {
            new Status { IdentifierID = 1, ClientID = 1, status = Status.StatusC.OrderedArticle, Last = DateTime.Parse("2017-11-11"), UserTableID = 1 },
            new Status { IdentifierID = 1, ClientID = 1, status = Status.StatusC.SentArticle, Last = DateTime.Parse("2017-11-07"), UserTableID = 1 },
            new Status { IdentifierID = 2, ClientID = 3, status=Status.StatusC.NeedArticle, Last = DateTime.Parse("2017-07-30"), UserTableID = 2 },
                  };

       Statuss.ForEach(s => context.Statuss.Add(s));
            context.SaveChanges();
        }
    }
}
