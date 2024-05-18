using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject Option;
    public GameObject Pendant;

    public void ReverseOption()
    {
        if(Option.activeInHierarchy == true)
        {
            Option.SetActive(false);
        }
        else
        {
            Option.SetActive(true);
        }
    }

    public void ReversePendant()
    {
        if (Pendant.activeInHierarchy == true)
        {
            Pendant.SetActive(false);
        }
        else
        {
            Pendant.SetActive(true);
        }
    }
}
