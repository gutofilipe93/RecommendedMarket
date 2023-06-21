using System.Collections;
using System.Collections.Generic;
using Google.Cloud.Firestore;

namespace RM.Domain.Entities
{
    [FirestoreData]
    public class RecommendsMarket
    {
        [FirestoreProperty]
        public string Market { get; set; }

        [FirestoreProperty]
        public string Message { get; set; }

        [FirestoreProperty]
        public ICollection<RecommendsMarketItem> Items { get; set; }

        [FirestoreProperty]
        public double TotalPrice {get; set;}

    }

    [FirestoreData]
    public class RecommendsMarketItem
    {
        [FirestoreProperty]
        public string Name { get; set; }
        
        [FirestoreProperty]
        public double Price { get; set; }

        [FirestoreProperty]
        public bool HaveOffer { get; set; }

        [FirestoreProperty]
        public string DateLastPurchase { get; set; }

        [FirestoreProperty]
        public string Market { get; set; }
        
        [FirestoreProperty]
        public string SearchableName { get; set; }
    }
}