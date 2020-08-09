using System;
using Google.Cloud.Firestore;

namespace RM.Infrastructure.Data
{
    public class FirebaseConnection
    {
        public FirestoreDb Open()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"mercado-recomendado-fb-firebase-adminsdk-xin29-37cdcba084.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);            
            return FirestoreDb.Create("mercado-recomendado-fb");
        }
    }
}