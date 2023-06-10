using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Zenject;

public class AuthService : IInitializable
{
    public int UserId = -1;
    public string UserContract = string.Empty;

    public void Initialize()
    {
        /*
        * This is sample commit code 
        */

        //DELETE ME!
        int a = 10;
        a+=15;

        Debug.Log($"Test message: a={a}");
        //DELETE ME!

        GetAuthInfo();
    }

#if !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern int GetUserId();

    [DllImport("__Internal")]
    private static extern string GetUserContract();
#endif

    private void GetAuthInfo()
    {
#if UNITY_EDITOR
        UserId = 64;
        UserContract = "0xaeae40b2Ea204e3Bc8e84aC2e8b26e2f75ab8391";
#else
        UserId = GetUserId();
        UserContract = GetUserContract();
#endif
    }

}
