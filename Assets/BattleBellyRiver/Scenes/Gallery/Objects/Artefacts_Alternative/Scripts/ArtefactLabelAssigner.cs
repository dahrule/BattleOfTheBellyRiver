using UnityEngine;

public class ArtefactLabelAssigner : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI Label;
    [SerializeField] Artefact artefact;
    
    void Start()
    {
        Label.text = artefact.artefactName;
    }

}
