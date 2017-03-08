﻿using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Networking;

public class GunSlot : GunSlot_Base {

    [SerializeField]
    Player player;

    private Action CB_AmmoChanged;

    [SerializeField]
    private Gun_Base primaryGun;
    
    [SerializeField]
    private Gun_Base secondaryGun;



    private Gun_Base equippedGun;


	// Use this for initialization
	void Start () {
        /*if(primaryGun != null){
            Debug.Log("Primary weapon is null");
            equippedGun = primaryGun;
            secondaryGun.gameObject.SetActive(false);
        }
        else{
            equippedGun = secondaryGun;
        }
        equippedGun.gameObject.SetActive(true);        
        equippedGun.AlignGun();

        CB_AmmoChanged();*/


    }

    void OnPlayerConnect(NetworkPlayer player){
       RpcSetupGunSlot(primaryGun.GetComponent<NetworkIdentity>().netId, secondaryGun.GetComponent<NetworkIdentity>().netId); 
    }

    [ClientRpc]
    void RpcSetupGunSlot(NetworkInstanceId primaryGun, NetworkInstanceId secondaryGun){
        //GameObject go = ClientScene.FindLocalObject(id);
        this.primaryGun = ClientScene.FindLocalObject(primaryGun).GetComponent<Gun_Base>();
        this.secondaryGun = ClientScene.FindLocalObject(secondaryGun).GetComponent<Gun_Base>();
    }

    public override void Drop(){
        if(primaryGun != null && equippedGun != secondaryGun)
        {
            equippedGun = secondaryGun;
            secondaryGun.gameObject.SetActive(true);
//            UpdateAmmoHUD();

            primaryGun.transform.parent = null;
            primaryGun.Drop();
            primaryGun = null;
        }

        CB_AmmoChanged();
    }

    public override bool TryPickup(Gun_Base gun){
        if(primaryGun == null){
            primaryGun = gun;
            equippedGun = primaryGun;
            secondaryGun.gameObject.SetActive(false);
            CB_AmmoChanged();
            return true;
        }
        return false;
    }

    public override void PreviousWeapon(){
        ToggleEquip();
    }

    public override void NextWeapon(){
        ToggleEquip();
    }

    private void ToggleEquip(){
        if(primaryGun == null){
            return;
        }

        equippedGun.gameObject.SetActive(false);
        if(equippedGun == primaryGun){
            equippedGun = secondaryGun;
        }
        else{
            equippedGun = primaryGun;
        }
        equippedGun.gameObject.SetActive(true);
        CB_AmmoChanged();
        equippedGun.AlignGun();
    }

    public override void SetStartingGun(Gun_Base gun){
        secondaryGun = gun;
        equippedGun = secondaryGun;
        equippedGun.AlignGun();
        CB_AmmoChanged();
    }


    public override void Reload(){
        equippedGun.Reload();
        CB_AmmoChanged();
    }


    public override void Shoot(bool firstDown){
        equippedGun.CmdShoot(firstDown);
        CB_AmmoChanged();
    }

    public override int BulletsInClip{
        get{ return equippedGun.BulletsInClip;}
    }

    public override int ClipSize{
        get{return equippedGun.ClipSize;}
    }


    public override void SetCB_AmmoChanged(Action action){
        CB_AmmoChanged = action;
    }


    public override Gun_Base EquippedGun{
        get{
            return equippedGun;
        }
    }

    public override Player Player{
        get{return player;}
    }
}
