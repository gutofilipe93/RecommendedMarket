using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using RM.Domain.Entities;
using RM.Domain.Interfaces.Repositories;
using RM.Infrastructure.Data;

namespace RM.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IFirebaseConnection _firebaseConnection;
        public ProductRepository(IFirebaseConnection firebaseConnection)
        {
            _firebaseConnection = firebaseConnection;
        }

        public async Task AddAsync(List<Product> products, string market)
        {
            var db = _firebaseConnection.Open();
            DocumentReference docRef = db.Collection("products").Document(market);
            Dictionary<string, object> initialData = new Dictionary<string, object>();
            initialData.Add("Products", products);
            await docRef.SetAsync(initialData);
        }

        public async Task AddSearchableNamesAsync(List<string> names)
        {
            var db = _firebaseConnection.Open();
            DocumentReference docRef = db.Collection("products").Document("NomePesquisaveis");
            Dictionary<string, object> initialData = new Dictionary<string, object>();
            initialData.Add("NomePesquisaveis", names);
            await docRef.SetAsync(initialData);
        }

        public async Task DeleteAsync(string name)
        {
            var db = _firebaseConnection.Open();
            DocumentReference docRef = db.Collection("products").Document("NomePesquisaveis");            
            await docRef.DeleteAsync();
        }

        public async Task<ICollection<Product>> GetAsync(string market)
        {
            var db = _firebaseConnection.Open();
            var document = await db.Collection("products").Document(market).GetSnapshotAsync();
            if (!document.Exists)
                return new List<Product>();

            var products = document.ConvertTo<Dictionary<string, List<Product>>>();
            return products["Products"];
        }

        public async Task<ICollection<string>> GetMarkets()
        {
            var db = _firebaseConnection.Open();
            var collections = await db.Collection("products").GetSnapshotAsync();
            List<string> markets = new List<string>();
            foreach (var document in collections.Documents)
            {
                if (document.Id != "NomePesquisaveis")
                    markets.Add(document.Id);
            }

            return markets;
        }

        public async Task<ICollection<string>> GetSearchableNamesAsync()
        {
            var db = _firebaseConnection.Open();
            var document = await db.Collection("products").Document("NomePesquisaveis").GetSnapshotAsync();
            if (!document.Exists)
                return new List<string>();

            var names = document.ConvertTo<Dictionary<string, List<string>>>();
            return names["NomePesquisaveis"];
        }
    }
}