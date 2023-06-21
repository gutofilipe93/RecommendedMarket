using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using RM.Domain.Entities;
using RM.Domain.Interfaces.Repositories;
using RM.Infrastructure.Data;

namespace RM.Infrastructure.Repositories
{
    public class PurchaseRepository : IPurchaseRepository
    {
        private readonly IFirebaseConnection _firebaseConnection;
        public PurchaseRepository(IFirebaseConnection firebaseConnection)
        {
            _firebaseConnection = firebaseConnection;
        }
        public async Task AddAsync(List<Item> Items)
        {
            var db = _firebaseConnection.Open();
            var month = DateTime.Today.Month.ToString().Length == 1 ? $"0{DateTime.Today.Month}" : DateTime.Today.Month.ToString();
            var key = $"{month}-{DateTime.Today.Year}";
            DocumentReference docRef = db.Collection("shopping").Document(key);
            Dictionary<string, object> initialData = new Dictionary<string, object>();
            initialData.Add("Purcheses", Items);
            await docRef.SetAsync(initialData);            
        }

        public CollectionReference GetAll()
        {
            var db = _firebaseConnection.Open();
            return db.Collection("shopping");
        }

        public async Task<List<Item>> GetPurchasesAsync(string document)
        {
            var db = _firebaseConnection.Open();
            var data = await db.Collection("shopping").Document(document).GetSnapshotAsync();
            if (!data.Exists)
                return new List<Item>();
                                    
            var Items = data.ConvertTo<Dictionary<string, List<Item>>>();            
            return Items["Purcheses"];
        }
    }
}