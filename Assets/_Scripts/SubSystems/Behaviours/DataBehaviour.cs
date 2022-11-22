using Newtonsoft.Json;

namespace _Scripts.SubSystems.Behaviours
{
    public static class DataBehaviour
    {
        public static string Serialize<T>(T toSerialize)
        {
            return JsonConvert.SerializeObject(toSerialize, Formatting.Indented);
        }

        public static T DeSerialize<T>(string toDeSerialize)
        {
            return JsonConvert.DeserializeObject<T>(toDeSerialize);
        }
    }
}
