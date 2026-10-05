using UnityEngine;

public class ElementMinigame : MonoBehaviour
{
    #region Fields
    [SerializeField] private string _id;
    #endregion

    #region public properties
    public string Id => _id;
    #endregion
}
