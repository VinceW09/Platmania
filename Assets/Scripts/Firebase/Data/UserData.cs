using Firebase.Firestore;

[FirestoreData]
public struct UserData
{
    [FirestoreProperty]
    public string Username { get; set; }

    [FirestoreProperty]
    public string Name { get; set; }

    [FirestoreProperty]
    public string FaceId { get; set; }

    [FirestoreProperty]
    public string ColorId { get; set; }

    [FirestoreProperty]
    public string UserId { get; set; }
}
