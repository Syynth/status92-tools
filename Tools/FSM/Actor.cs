namespace Status92.Tools.FSM
{
    public abstract class Actor<TActor> : IActor<TActor, RecordState<TActor>, RecordMachine<TActor>>
        where TActor : IActor<TActor, RecordState<TActor>, RecordMachine<TActor>>
    {
        protected Actor(RecordMachine<TActor> fsm)
        {
            FSM = fsm;
        }

        public RecordMachine<TActor> FSM { get; }
    }
}