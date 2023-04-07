using UnityEngine;
using TMPro;

public class ArtefactInfoDisplayer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI HeaderText;
    [SerializeField] TextMeshProUGUI Text;

    private void OnEnable()
    {
        ArtefactGrabInteractable.OnObjectSelected += OnObjectSelected;
    }

    private void OnDisable()
    {
        ArtefactGrabInteractable.OnObjectSelected -= OnObjectSelected;
    }

    private void OnObjectSelected(Artefact artefact)
    {
        if (artefact == null) return;
        
        HeaderText.text = artefact.artefactName;
        Text.text = artefact.artefactInfo;
    }
}
