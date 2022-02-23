// Не инетрфейс, но в Instatiate нужно пихать UnityEngine.Object, ничего лучше не придумал :(
public abstract class Spawnable<DataRequiredForInitialization> : UnityEngine.MonoBehaviour {
    public abstract void Initialize(DataRequiredForInitialization data);
}
