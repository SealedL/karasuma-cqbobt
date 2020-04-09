using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace cqbot
{
    public class Auth
    {
        private static readonly JsonSerializerOptions Option = new JsonSerializerOptions()
        {
            WriteIndented = true
        };
        
        public static long ReadMasterId()
        {
            var idType = IdTypeDeserialize();
            return idType.MasterId;
        }

        public static List<long> ReadAdminIds()
        {
            var idType = IdTypeDeserialize();
            return idType.AdminIds;
        }

        public static void WriteAdminIds(long id)
        {
            var idType = IdTypeDeserialize();
            idType.AdminIds.Add(id);
            var newJsonString = JsonSerializer.Serialize(idType, Option);
            File.WriteAllText(SharedContent.JsonFilePath, newJsonString);
        }

        private static IdType IdTypeDeserialize()
        {
            var idType = new IdType();
            var jsonString = File.ReadAllText(SharedContent.JsonFilePath);
            idType = JsonSerializer.Deserialize<IdType>(jsonString);
            return idType;
        }
    }
}