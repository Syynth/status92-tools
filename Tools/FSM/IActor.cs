namespace Status92.Tools.FSM
{
    public interface IActor<TActor, TState, out TMachine>
        where TActor : IActor<TActor, TState, TMachine>
        where TState : IState<TActor, TState, TMachine>
        where TMachine : IMachine<TActor, TState, TMachine>
    {
        public TMachine FSM { get; }
    }
}