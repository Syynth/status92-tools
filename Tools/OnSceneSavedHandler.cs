namespace Status92.Tools
{
    public record SceneInfo(string Scene)
    {
        public string Scene { get; } = Scene;
    }

    public interface OnSceneSavedHandler
    {
        void OnSceneSaved(SceneInfo info);
    }
    
    public interface OnSceneSavedHandler<in T1>
    {
        void OnSceneSaved(SceneInfo info, T1 teleporters);
    }

    public interface OnSceneSavedHandler<in T1, in T2>
    {
        void OnSceneSaved(SceneInfo info, T1 arg1, T2 arg2);
    }

    public interface OnSceneSavedHandler<in T1, in T2, in T3>
    {
        void OnSceneSaved(SceneInfo info, T1 arg1, T2 arg2, T3 arg3);
    }

    public interface OnSceneSavedHandler<in T1, in T2, in T3, in T4>
    {
        void OnSceneSaved(SceneInfo info, T1 arg1, T2 arg2, T3 arg3, T4 arg4);
    }

    public interface OnSceneSavedHandler<in T1, in T2, in T3, in T4, in T5>
    {
        void OnSceneSaved(SceneInfo info, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);
    }

    public interface OnSceneSavedHandler<in T1, in T2, in T3, in T4, in T5, in T6>
    {
        void OnSceneSaved(SceneInfo info, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);
    }

    public interface OnSceneSavedHandler<in T1, in T2, in T3, in T4, in T5, in T6, in T7>
    {
        void OnSceneSaved(SceneInfo info, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);
    }

    public interface OnSceneSavedHandler<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8>
    {
        void OnSceneSaved(SceneInfo info, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8);
    }
}