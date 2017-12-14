/**
 * IPoolObject.cs
 * Created by: Joao Borks [joao.borks@gmail.com]
 * Created on: 12/07/17 (dd/mm/yy)
 */

/// <summary>
/// Interface for all objects that can be inserted into a <see cref="PoolSystem"/>
/// </summary>
public interface IPoolObject
{
    PoolSystem Pool { get; set; }
}