using Data.Models;

namespace ECatalogueManager.Extensions
{
    public static class RankExtension
    {
        public static string RankToName(this Rank rank)
        {
            return (int)rank switch
            {
                1 => "Assistant Professor",
                2 => "Associate Professor",
                3 => "Professor",
                _ => "Instructor",
            };
        }
    }
}
