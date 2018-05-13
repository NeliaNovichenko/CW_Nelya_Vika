using CW_Nelya_Vika.Models;

namespace CW_Nelya_Vika_Algorithms
{
    /// <summary>
    /// Усі алгоритми приймають однаковій набір параметрів та return type
    /// </summary>
    public interface IAlgorithm
    {
        Problem FindCommunityStructure(Graph pGraph);
    }
}
