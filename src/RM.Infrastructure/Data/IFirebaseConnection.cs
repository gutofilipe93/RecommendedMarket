using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Text;

namespace RM.Infrastructure.Data
{
    public interface IFirebaseConnection
    {
        FirestoreDb Open();
    }
}
