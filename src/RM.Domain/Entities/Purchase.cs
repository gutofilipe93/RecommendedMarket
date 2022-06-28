using System.Collections.Generic;
using Google.Cloud.Firestore;

namespace RM.Domain.Entities
{
    [FirestoreData]
    public class Purchase
    {        
        
        [FirestoreProperty]
        public string Market { get; set; }
        
        [FirestoreProperty]
        public bool Processed { get; set; }
                
        [FirestoreProperty]
        public List<Item> Items { get; set; }
    }
}