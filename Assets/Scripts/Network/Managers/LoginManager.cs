using UnityEngine;

public class LoginManager
{
    private static int _id;

    public static int Id => _id;

    public void CreateId()
    {
        //TODO: Replace with guid type in future updates
        _id = Random.Range(1, 9999);
    }
}
