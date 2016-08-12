/// <summary>
/// 深拷贝接口
/// </summary>
public interface IDeepCopy<T>
{
    Script_Base<T> DeepCopy();
}