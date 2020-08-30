using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using RM.Domain.Entities;
using RM.Domain.Interfaces.Repositories;
using RM.Infrastructure.Data;

namespace RM.Infrastructure.Repositories
{
    public class RecommendsMarketRepository : IRecommendsMarketRepository
    {
        public async Task DeleteAsync()
        {
            var db = new FirebaseConnection().Open();
            var document = db.Collection("recommendsMarket").Document("recommendation");
            await document.DeleteAsync();
        }

        public async Task<RecommendsMarket> GetRecommendsMarket()
        {
            var db = new FirebaseConnection().Open();
            var document = await db.Collection("recommendsMarket").Document("recommendation").GetSnapshotAsync();
            if (!document.Exists)
                return new RecommendsMarket { Items = new List<RecommendsMarketItem>()};

            var recommendsMarket = document.ConvertTo<Dictionary<string, RecommendsMarket>>();                                    
            return recommendsMarket[""];
        }

        public async Task SaveAsync(RecommendsMarket recommendsMarket)
        {
            var db = new FirebaseConnection().Open();
            DocumentReference docRef = db.Collection("recommendsMarket").Document();
            await docRef.SetAsync(recommendsMarket);
        }
    }
}