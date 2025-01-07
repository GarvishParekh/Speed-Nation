using UnityEngine;

[CreateAssetMenu(fileName = "Firebase Data", menuName = "Scriptable/FirebaseData")]
public class FirebaseData : ScriptableObject
{
    public SignInType signInType;
    public string webSeceretID; 
}
