using UnityEngine;

[CreateAssetMenu(fileName ="Artefact", menuName="Artefacts/Create new Artefact", order =0)]
public class Artefact : ScriptableObject
{
    [SerializeField] GameObject artefactPrefab=null;
    public string artefactName = null;
    [Multiline] public string artefactInfo = null;
    public AnimatorOverrideController leftHand_overrideController;
    public AnimatorOverrideController rightHand_overrideController;

}
