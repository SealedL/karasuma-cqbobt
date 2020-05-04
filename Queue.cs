using System.Collections.Generic;

namespace cqbot
{
    public static class Queue
    {
        private static List<long> userList = new List<long>();

        public static void AddUserToList(long userId)
        {
            userList.Add(userId);
        }

        public static void RemoveUserFromList(long userId)
        {
            userList.Remove(userId);
        }

        public static bool IsUserListed(long userId)
        {
            return userList.Contains(userId);
        }
    }
}