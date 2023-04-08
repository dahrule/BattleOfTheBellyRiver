using UnityEngine;
using TMPro;

public class ArtefactInfoDisplayer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI HeaderText;
    [SerializeField] TextMeshProUGUI Text;
    [SerializeField] GameObject scaleIntructionsPanel;

    [SerializeField] string defaultHeaderText;
    [SerializeField, Multiline] string defaultText;

    private void Start()
    {
        DisplayDefaultInfo();
    }

    private void OnEnable()
    {
        ArtefactGrabInteractable.OnObjectSelected += DisplayArtefactInfo;
        ArtefactGrabInteractable.OnObjectReleased += (Artefact artefact) => DisplayDefaultInfo();
    }

    private void OnDisable()
    {
        ArtefactGrabInteractable.OnObjectSelected -= DisplayArtefactInfo;
        ArtefactGrabInteractable.OnObjectReleased-= (Artefact artefact) => DisplayDefaultInfo();
    }

    private void DisplayArtefactInfo(Artefact artefact)
    {
        if (artefact == null) return;
        
        HeaderText.text = artefact.artefactName;
        Text.text = artefact.artefactInfo;
        if (scaleIntructionsPanel != null) scaleIntructionsPanel.SetActive(artefact.scalable);
    }

    private void DisplayDefaultInfo()
    {
        HeaderText.text = defaultHeaderText;
        Text.text = defaultText;
        if(scaleIntructionsPanel!=null) scaleIntructionsPanel.SetActive(false);
    }
}
