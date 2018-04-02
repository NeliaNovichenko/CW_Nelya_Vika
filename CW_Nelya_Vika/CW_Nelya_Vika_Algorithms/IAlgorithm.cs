using CW_Nelya_Vika.Models;

namespace CW_Nelya_Vika_Algorithms
{
    /// <summary>
    /// Перелік реалізованих алгоритмів
    /// </summary>
    public enum Algorithm : byte
    {
        GirvanNewman,
        KernighanLin
    }

    /// <summary>
    /// Усі алгоритми приймають однаковій набір параметрів та !!!!!!! return type !!!!!!!!
    /// </summary>
    interface IAlgorithm
    {
        Result FindCommunityStructure(Graph pGraph);
    }
}
