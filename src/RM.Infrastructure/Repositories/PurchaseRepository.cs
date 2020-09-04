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
        public async Task AddAsync(Purchase purchase)
        {
            var db = _firebaseConnection.Open();
            DocumentReference docRef = db.Collection("shopping").Document();
            await docRef.SetAsync(purchase);
        }
    }
}