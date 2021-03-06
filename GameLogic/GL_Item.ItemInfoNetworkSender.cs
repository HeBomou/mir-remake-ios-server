using MirRemakeBackend.Entity;
using MirRemakeBackend.Network;

namespace MirRemakeBackend.GameLogic {
    partial class GL_Item {
        private interface IItemInfoNetworkSender {
            ItemType m_Type { get; }
            void SendItemInfo (E_Item item, int netId, INetworkService ns);
            void SendMarketItemInfo (E_Item item, int netId, int holderNetId, INetworkService ns);
        }
        private class IINS_Consumable : IItemInfoNetworkSender {
            public ItemType m_Type { get { return ItemType.CONSUMABLE; } }
            public void SendItemInfo (E_Item item, int netId, INetworkService ns) { }
            public void SendMarketItemInfo (E_Item item, int netId, int holderNetId, INetworkService ns) { }
        }
        private class IINS_Empty : IItemInfoNetworkSender {
            public ItemType m_Type { get { return ItemType.EMPTY; } }
            public void SendItemInfo (E_Item item, int netId, INetworkService ns) { }
            public void SendMarketItemInfo (E_Item item, int netId, int holderNetId, INetworkService ns) { }
        }
        private class IINS_Enchantment : IItemInfoNetworkSender {
            public ItemType m_Type { get { return ItemType.ENCHANTMENT; } }
            public void SendItemInfo (E_Item item, int netId, INetworkService ns) {
                ns.SendServerCommand (
                    SC_ApplySelfUpdateEnchantment.Instance (
                        netId,
                        item.m_realId,
                        (item as E_EnchantmentItem).GetEnchantmentNoInfo ()));
            }
            public void SendMarketItemInfo (E_Item item, int netId, int holderNetId, INetworkService ns) {
                ns.SendServerCommand (
                    SC_ApplyOtherUpdateMarketEnchantment.Instance (
                        netId,
                        holderNetId,
                        (item as E_EnchantmentItem).GetEnchantmentNoInfo ()));
            }
        }
        private class IINS_Equipment : IItemInfoNetworkSender {
            public ItemType m_Type { get { return ItemType.EQUIPMENT; } }
            public void SendItemInfo (E_Item item, int netId, INetworkService ns) {
                ns.SendServerCommand (
                    SC_ApplySelfUpdateEquipment.Instance (
                        netId,
                        item.m_realId,
                        (item as E_EquipmentItem).GetEquipmentInfoNo ()));
            }
            public void SendMarketItemInfo (E_Item item, int netId, int holderNetId, INetworkService ns) {
                ns.SendServerCommand (
                    SC_ApplyOtherUpdateMarketEquipment.Instance (
                        netId,
                        holderNetId,
                        (item as E_EquipmentItem).GetEquipmentInfoNo ()));
            }
        }
        private class IINS_Gem : IItemInfoNetworkSender {
            public ItemType m_Type { get { return ItemType.GEM; } }
            public void SendItemInfo (E_Item item, int netId, INetworkService ns) { }
            public void SendMarketItemInfo (E_Item item, int netId, int holderNetId, INetworkService ns) { }
        }
        private class IINS_Material : IItemInfoNetworkSender {
            public ItemType m_Type { get { return ItemType.MATERIAL; } }
            public void SendItemInfo (E_Item item, int netId, INetworkService ns) { }
            public void SendMarketItemInfo (E_Item item, int netId, int holderNetId, INetworkService ns) { }
        }
    }
}