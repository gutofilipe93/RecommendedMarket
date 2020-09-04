using System;
using System.IO;
using System.Threading;
using Google.Cloud.Firestore;

namespace RM.Infrastructure.Data
{
    public class FirebaseConnection : IFirebaseConnection
    {
        private string _connection { get; set; } = "mercado-recomendado-fb-firebase-adminsdk-xin29-37cdcba084.json";               
        public FirestoreDb Open()
        {                       
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", _connection);            
            return FirestoreDb.Create("mercado-recomendado-fb");
        }
    }
}