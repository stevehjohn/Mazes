namespace Mazes.Core.Extensions;

public static class ListExtensions
{
    extension<T>(List<T> list)
    {
        public void ForAll(Action<int, T> action)
        {
            for (var i = 0; i < list.Count; i++)
            {
                action(i, list[i]);
            }
        }
    }
}