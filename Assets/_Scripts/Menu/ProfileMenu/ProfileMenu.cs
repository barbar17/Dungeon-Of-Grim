using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileMenu : MonoBehaviour
{
    public GameObject playerProfilePrefab;
    public Transform profileContainer;

    public void CreateNewProfile()
    {
        Instantiate(playerProfilePrefab, profileContainer.transform);
    }

    public void LoadProfile()
    {

    }

    public void DeleteProfile()
    {

    }
}
