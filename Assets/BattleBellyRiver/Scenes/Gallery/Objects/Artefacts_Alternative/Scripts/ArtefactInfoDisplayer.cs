using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;

public class ArtefactInfoDisplayer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI HeaderText;
    [SerializeField] TextMeshProUGUI Text;

    [SerializeField] string defaultHeaderText;
    [SerializeField, Multiline] string defaultText;

    private void Start()
    {
        DisplayDefaultInfo();
    }

    private void OnEnable()
    {
        ArtefactGrabInteractable.OnObjectSelected += DisplayArtefactInfo;
        ArtefactGrabInteractable.OnObjectReleased += (Artefact artefact)=>DisplayDefaultInfo();
    }

    private void OnDisable()
    {
        ArtefactGrabInteractable.OnObjectSelected -= DisplayArtefactInfo;
        ArtefactGrabInteractable.OnObjectReleased -= (Artefact artefact) => DisplayDefaultInfo();
    }

    public void DisplayArtefactInfo(Artefact artefact)
    {
        if (artefact == null) return;
        
        HeaderText.text = artefact.artefactName;
        Text.text = artefact.artefactInfo;
    }

    public void DisplayDefaultInfo()
    {
        HeaderText.text = defaultHeaderText;
        Text.text = defaultText;
    }

    void Test()
    {
        Debug.Log("TESTING");
    }
}
