using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameHub
{
    //Multiton structure granting access to major game system via connected hub
    private static ObjectPooler _objectPooler;

    public static ObjectPooler ObjectPooler => ObjectReferenceUtility.FindReferenceIfNull(ref _objectPooler);


    private static GameManager _gameManager;

    public static GameManager GameManager => ObjectReferenceUtility.FindReferenceIfNull(ref _gameManager);
}
