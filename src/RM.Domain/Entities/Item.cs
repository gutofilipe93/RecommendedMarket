using Google.Cloud.Firestore;

namespace RM.Domain.Entities
{
    [FirestoreData]
    public class Item
    {
        [FirestoreProperty]
        public string Name { get; set; }
        
        [FirestoreProperty]
        public double Price { get; set; }
        
        [FirestoreProperty]
        public bool HaveOffer { get; set; }
        
        [FirestoreProperty]
        public string SearchableName { get; set; }
    }
}