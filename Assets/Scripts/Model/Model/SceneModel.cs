

using System.Collections.Generic;
using UnityEngine;





[System.Serializable]
public class SceneModel : BaseModel
{
    [System.Serializable]
    public class UnitModel
    {
        public int id;
        public int cell;
        public int RX;
        public int RY;
        public int RZ;
        public UnitModel(int[] data)
        {
            id = data[0];
            cell = data[1];
            RX = data[2];
            RY = data[3];
            RZ = data[4];
        }
    }
    
    public List<UnitModel> ranks1;
    public List<UnitModel> ranks2;
    public bool skillEnenyOver;
    public bool deathOver;


    public void SetRanks1(List<int[]> data)
    {
        ranks1 = new List<UnitModel>();
        for (int i = 0; i < data.Count; i++)
        {
            ranks1.Add(new UnitModel(data[i]));
        }
    }
    public void SetRanks2(List<int[]> data)
    {
        ranks2 = new List<UnitModel>();
        for (int i = 0; i < data.Count; i++)
        {
            ranks2.Add(new UnitModel(data[i]));
        }
    }
}