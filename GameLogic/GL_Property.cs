using System.Collections.Generic;
using MirRemakeBackend.DynamicData;
using MirRemakeBackend.Entity;
using MirRemakeBackend.Network;

namespace MirRemakeBackend.GameLogic {
    class GL_Property : GameLogicBase {
        public static GL_Property s_instance;
        private IDDS_Item m_itemDds;
        private IDDS_Character m_charDds;
        public GL_Property (IDDS_Item itemDds, IDDS_Character charDds, INetworkService ns) : base (ns) {
            m_itemDds = itemDds;
            m_charDds = charDds;
        }
        public override void Tick (float dT) { }
        public override void NetworkTick () { }
        public void NotifyUpdateCurrency (E_Character charObj, CurrencyType type, long dC) {
            charObj.m_currencyDict[type] += dC;
            m_charDds.UpdateCharacter (charObj.GetDdo ());
            // client
            m_networkService.SendServerCommand (SC_ApplySelfCurrency.Instance (
                charObj.m_networkId, charObj.m_VirtualCurrency, charObj.m_ChargeCurrency));
        }
        public void NotifyLostItem (E_Character charObj, E_Item item, short num, short pos, E_RepositoryBase repo) {
            // 移除num个该物品
            bool runOut = item.RemoveNum (num);
            long realId = item.m_realId;
            short curNum = item.m_num;
            // 实例 与 数据
            if (runOut) {
                repo.RemoveItemByRealId (item.m_realId);
                m_itemDds.DeleteItemByRealId (item.m_realId);
                EM_Item.s_instance.RecycleItem (item);
            } else
                m_itemDds.UpdateItem (item.GetItemDdo (charObj.m_characterId, ItemPlace.BAG, pos));
            // Client
            m_networkService.SendServerCommand (SC_ApplySelfUpdateItemNum.Instance (
                charObj.m_networkId, new List<long> { realId }, new List<short> { curNum }));
        }
        public void NotifySwapItemPlace (E_Character charObj, E_RepositoryBase srcRepo, short srcPos, E_Item srcItem, E_RepositoryBase tarRepo, short tarPos, E_Item tarItem) {
            srcRepo.SetItem (tarItem, srcPos);
            tarRepo.SetItem (srcItem, tarPos);
            m_networkService.SendServerCommand (SC_ApplySelfMoveItem.Instance (
                new List<int> { charObj.m_networkId }, srcRepo.m_repositoryPlace, srcPos, tarRepo.m_repositoryPlace, tarPos));
        }
        public void NotifyGainItem (E_Character charObj, IReadOnlyList < (short, short) > itemIdAndNumList) {
            var bag = EM_Item.s_instance.GetBag (charObj.m_networkId);
            if (bag == null) return;
            // 分配RealId
            var realIdList = new List<long> (itemIdAndNumList.Count);
            for (int i = 0; i < itemIdAndNumList.Count; i++)
                realIdList.Add (m_itemDds.InsertItem (new DDO_Item ()));
            // 实例化
            var itemList = EM_Item.s_instance.InitItemList (itemIdAndNumList, realIdList);
            // 放入背包
            for (int i = 0; i < itemList.Count; i++) {
                List < (short, E_Item) > changedItemList;
                short storePos = bag.AutoStoreItem (itemList[i], out changedItemList);
                // 堆叠原有物品
                // 数据更新
                for (int j = 0; j < changedItemList.Count; j++)
                    m_itemDds.UpdateItem (
                        changedItemList[j].Item2.GetItemDdo (
                            charObj.m_characterId, ItemPlace.BAG, changedItemList[j].Item1));
                // client
                List<long> changedRealIdList = new List<long> (changedItemList.Count);
                List<short> changedPosList = new List<short> (changedItemList.Count);
                for (int j=0; j<changedItemList.Count; j++) {
                    changedRealIdList.Add (changedItemList[i].Item2.m_realId);
                    changedPosList.Add (changedItemList[i].Item1);
                }
                m_networkService.SendServerCommand (SC_ApplySelfUpdateItemNum.Instance (
                    charObj.m_networkId, changedRealIdList, changedPosList));

                // 若该物品消失 (堆叠或无法放入) 数据删除
                if (storePos == 0 || storePos == -1) {
                    m_itemDds.DeleteItemByRealId (itemList[i].m_realId);
                    continue;
                }
                // 该物品单独占有一格
                else {
                    // 基础数据更新
                    m_itemDds.UpdateItem (itemList[i].GetItemDdo (charObj.m_characterId, ItemPlace.BAG, storePos));
                    // 基础数据 client
                    m_networkService.SendServerCommand (SC_ApplySelfGainItem.Instance (
                        charObj.m_networkId,
                        new List<NO_Item> { itemList[i].GetItemNo () },
                        new List<ItemPlace> { ItemPlace.BAG },
                        new List<short> { storePos } ));
                    // 附加数据 (装备等) TODO: client
                    switch (itemList[i].m_Type) {
                        case ItemType.EQUIPMENT:
                            m_itemDds.UpdateEquipmentInfo (((E_EquipmentItem)itemList[i]).GetEquipmentInfoDdo (charObj.m_characterId));
                            break;
                    }
                }
            }
        }
    }
}