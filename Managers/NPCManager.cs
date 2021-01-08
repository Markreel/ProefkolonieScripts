using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    [SerializeField] GameObject npcPrefab;
    public NPC SelectedNPC;

    private List<NPC> npcList = new List<NPC>();

    private void Start()
    {
        SpawnNPCs();
    }

    public void SpawnNPCs()
    {
        foreach (var _house in GameManager.Instance.StructuresManager.Houses)
        {
            if (!_house.isActiveAndEnabled || _house.HasNPC) { continue; }

            _house.HasNPC = true;

            GameObject _obj = Instantiate(npcPrefab, _house.AccessPoint.position, _house.AccessPoint.rotation, transform);

            NPC _npc = _obj.GetComponent<NPC>();
            _npc.AssignedHouse = _house;
            _npc.AssignedFarm = _house.CorrespondingFarm;

            npcList.Add(_npc);
            _house.AddResident(_npc);
        }
    }

    public void ChangeProfessionOfSelectedNPC(int _profession)
    {
        if (SelectedNPC != null) {  SelectedNPC.Profession = (Profession)_profession; }
    }

    public void ChangeFarmOfSelectedNPC(bool _ownFarm)
    {
        if (SelectedNPC != null) { SelectedNPC.ChangeFarm(_ownFarm); }
    }

    public void MakeAllNPCsGoHome()
    {
        foreach (var _npc in npcList)
        {
            _npc.ResetNavigation();
            _npc.SetTargetDestination(_npc.AssignedHouse.AccessPoint.position);
        }
    }

    public void TeleportAllNPCsToTheirHome()
    {
        foreach (var _npc in npcList)
        {
            _npc.ResetAnimator();
            _npc.ResetNavigation();
            _npc.transform.position = _npc.AssignedHouse.AccessPoint.position;
        }
    }
}
