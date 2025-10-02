using Firebase.Firestore;

[FirestoreData]
public struct UserData
{
    [FirestoreProperty]
    public string UserName { get; set; }

    [FirestoreProperty]
    public string RealName { get; set; }

    [FirestoreProperty]
    public string FaceId { get; set; }

    [FirestoreProperty]
    public string ColorId { get; set; }
}
