using ParameterTransfer;

namespace Interfaces {
    public interface I_WaveRender {
        public void CleanDisplay();
        public void RefreshDisplay();
        public void UpdateDisplay();
        public void SyncRootParam(I_WaveRender srcWD);
    }
    public interface I_WaveLogic {
        public void CleanInteract();
        public void Interact();
        public void SyncRootParam(I_WaveLogic srcWI);
    }

    public interface I_ParameterTransfer {
        public void RegisterParametersCallback(ParameterInfoList ParameterInfos);
        //public bool ParameterSet<T>(string paramName, T value);
        //public T ParameterGet<T>(string paramName);
        //static public bool ParameterSetHelper<T>(object obj, string paramName, T value) {
        //    FieldInfo fieldInfo = obj.GetType().GetField(paramName, BindingFlags.Instance | BindingFlags.Public);
        //    if (fieldInfo != null) {
        //        fieldInfo.SetValue(obj, value);
        //        return true;
        //    }
            
        //    DebugLogger.Warning(
        //        obj.GetType().Name,
        //        "ParamUI tries to access parameter[" + paramName + "] not exist, return false!");
        //    return false;
        //}
        //static public T ParameterGetHelper<T>(object obj, string paramName) {
        //    FieldInfo fieldInfo = obj.GetType().GetField(paramName, BindingFlags.Instance | BindingFlags.Public);
        //    if (fieldInfo != null) {
        //        return (T)fieldInfo.GetValue(obj);
        //    }

        //    DebugLogger.Warning(
        //        obj.GetType().Name,
        //        "ParamUI tries to access parameter[" + paramName + "] not exist, return default(T)!");
        //    return default(T);
        //}
    }
}