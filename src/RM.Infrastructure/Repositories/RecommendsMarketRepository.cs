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
        private readonly IFirebaseConnection _firebaseConnection;
        public RecommendsMarketRepository(IFirebaseConnection firebaseConnection)
        {
            _firebaseConnection = firebaseConnection;
        }

        public async Task DeleteAsync()
        {
            var db = _firebaseConnection.Open();
            var document = db.Collection("recommendsMarket").Document("recommendation");
            if (document != null)
                await document.DeleteAsync();
        }

        public async Task<RecommendsMarket> GetRecommendsMarket()
        {
            var db = _firebaseConnection.Open();
            var document = await db.Collection("recommendsMarket").Document("recommendation").GetSnapshotAsync();
            if (!document.Exists)
                return new RecommendsMarket { Items = new List<RecommendsMarketItem>()};

            var recommendsMarket = document.ConvertTo<RecommendsMarket>();                                    
            return recommendsMarket;
        }

        public async Task SaveAsync(RecommendsMarket recommendsMarket)
        {
            var db = _firebaseConnection.Open();
            DocumentReference docRef = db.Collection("recommendsMarket").Document("recommendation");
            await docRef.SetAsync(recommendsMarket);
        }
    }
}