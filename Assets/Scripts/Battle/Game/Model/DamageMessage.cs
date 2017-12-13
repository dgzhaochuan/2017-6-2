
using System.Collections.Generic;
using UnityEngine;


public class DamageMessage
{
    public Unit murderer;
    public int hit;
    public ResistanceEnum type;
    public List<BuffModel> buff;
    public List<bool> buffadd = new List<bool>();
    public DamageMessage(Unit murderer,int[] hit, ResistanceEnum type,List<BuffModel> buff,List<bool> buffadd)
    {
        this.type = type;
        this.murderer = murderer;
        this.hit = Random.Range(hit[0],hit[1]+1);
        this.buff = buff;
        this.buffadd = buffadd;
    }
}