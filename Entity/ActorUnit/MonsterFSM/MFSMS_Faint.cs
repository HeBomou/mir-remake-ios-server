namespace MirRemake {
    struct MFSMS_Faint : IMFSMState {
        public MFSMStateType m_Type { get { return MFSMStateType.FAINT; } }
        public E_Monster m_Self { get; set; }
        public void OnEnter (MFSMStateType prevType) { }
        public void OnTick (float dT) { }
        public IMFSMState GetNextState () {
            return null;
        }
        public void OnExit (MFSMStateType nextType) { }
    }
}