using System.Collections.Generic;
using UnityEngine;

namespace MirRemakeBackend {
    class EquipmentRegion {
        Dictionary<EquipmentPosition, E_EquipmentItem> m_equipPositionAndEquipmentDict = new Dictionary<EquipmentPosition, E_EquipmentItem> ();
        public List<E_Item> GetAllItem () {
            List<E_Item> res = new List<E_Item> ();
            var equipmentEn = m_equipPositionAndEquipmentDict.Values.GetEnumerator ();
            while (equipmentEn.MoveNext ())
                res.Add (equipmentEn.Current);
            return res;
        }
        public List<E_EquipmentItem> GetAllEquipment () {
            var res = CollectionUtils.DictionaryToValueList (m_equipPositionAndEquipmentDict);
            return res;
        }
        public E_EquipmentItem GetEquipmentByEquipPosition (EquipmentPosition ePos) {
            E_EquipmentItem res = null;
            m_equipPositionAndEquipmentDict.TryGetValue (ePos, out res);
            return res;
        }
    }
}