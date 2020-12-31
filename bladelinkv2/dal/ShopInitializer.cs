using bladelinkv2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bladelinkv2.dal
{
    public class ShopInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<ShopContext>
    {
        protected override void Seed(ShopContext context)
        {
            var Clients = new List<Client>
            {
                new Client{ID_cli=1,Name="Godart", Fname="Guillaunme", Adress="Quelque part", Email="Guillaume.godart@hotmail.com", password="godart"},
                new Client{ID_cli=2,Name="Callens", Fname="Alexis", Adress="29,rue du parc", Email="alexis.callens@outlook.com", password="1234"},
            };

            Clients.ForEach(c => context.Client.Add(c));
            context.SaveChanges();


            var Produit = new List<Product>{
                new Product{ ID_prod=1, Name_prod="RTX 2070", Price=600, Stock=50, type="Carte graphique"},
                new Product{ ID_prod=2, Name_prod="RTX 2080", Price=1600, Stock=50, type="Carte graphique"},
            };

            Produit.ForEach(p => context.Produits.Add(p));
            context.SaveChanges();

            var commandes = new List<Order>{
                new Order { ID_comm1=1, Id_cli=1, lp=Produit},
            };

            commandes.ForEach(comm => context.Commande.Add(comm));
            context.SaveChanges();
        }
    }
}