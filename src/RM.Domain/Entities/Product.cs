using Google.Cloud.Firestore;

namespace RM.Domain.Entities
{
    [FirestoreData]
    public class Product
    {
        [FirestoreProperty]
        public string Name { get; set; }
        
        [FirestoreProperty]
        public double Price { get; set; }
        
        [FirestoreProperty]
        public double PenultimatePrice { get; set; }
        
        [FirestoreProperty]
        public bool TemOferta { get; set; }
        
        [FirestoreProperty]
        public string DateOfLastPurchase { get; set; }
        
        [FirestoreProperty]
        public string DatePenultimatePurchase   { get; set; }
        
        [FirestoreProperty]
        public string Market { get; set; }

        [FirestoreProperty]
        public string SearchableName { get; set; }
    }
}