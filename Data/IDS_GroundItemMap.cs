using System;
using System.IO;
using System.Numerics;
using LitJson;
namespace MirRemakeBackend.Data {
    interface IDS_GroundItemMap {
        /// <summary>
        /// 获取所有地面物品的刷新位置  
        /// Key: ItemId  
        /// Value: 刷新位置  
        /// </summary>
        ValueTuple<short, Vector2>[] GetAllGroundItemRespawnPosition ();
    }
    class DS_GroundItemMapImpl : IDS_GroundItemMap{
        private JsonData s_itemPosDatas;
        public ValueTuple<short,Vector2>[] GetAllGroundItemRespawnPosition(){
            string jsonFile = File.ReadAllText("Data/D_ItemMap.json");
            Random ran=new Random();
            s_itemPosDatas = JsonMapper.ToObject(jsonFile);
            ValueTuple<short,Vector2>[] res=new ValueTuple<short,Vector2>[s_itemPosDatas.Count*5];
            for(int k=0;k<s_itemPosDatas.Count;k++){
                for (int j=0;j<5;j++){
                    int i=k*5+j;
                    res[i]=new ValueTuple<short,Vector2>(short.Parse(s_itemPosDatas[k]["ID"].ToString()),
                        new Vector2(float.Parse(s_itemPosDatas[k]["x"].ToString())+(float)ran.Next(-10000,10000)/10000,
                            float.Parse(s_itemPosDatas[k]["y"].ToString())+(float)ran.Next(-10000,10000)/10000));
                }
            }
            return res; 
        }
    }
}