using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class TestScript : MonoBehaviour
{
    public GameObject Ant;
    public int selectedCell;
    public GameObject morcellAmount;
    public int baseMorcells = 1000;

    private void OnEnable()
    {
        Ant = GameObject.FindGameObjectWithTag("Ant");
        if (GameManager.Instance.armoredUnlocked == true)
        {
            GameObject VLP1 = GameObject.FindGameObjectWithTag("VLP1");
            VLP1.GetComponent<Button>().interactable = true;
        }
    }

    public void CloseUI()
    {
        Ant.GetComponent<RTSController>().isDisabled = false;
        GameObject currentSlug = Ant.GetComponent<RTSController>().currentSlug;
        currentSlug.GetComponent<NavMeshAgent>().isStopped = false;
        this.gameObject.SetActive(false);
    }

    public void InjectSlug()
    {
        GameObject currentSlug = Ant.GetComponent<RTSController>().currentSlug;
        currentSlug.GetComponent<SlugWanderAI>().ChangeSlug(selectedCell);
        baseMorcells = baseMorcells - currentSlug.GetComponent<SlugWanderAI>().currentSlug.unitCost;
        morcellAmount.GetComponent<TMP_Text>().text = baseMorcells.ToString();
    }
    public void SelectCell(int cell)
    {
        selectedCell = cell;
        Debug.Log("Selected: " + selectedCell);
    }
}
