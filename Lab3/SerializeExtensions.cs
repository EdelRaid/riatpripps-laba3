using Newtonsoft.Json;

namespace Lab3
{
  public static class SerializeExtensions
  {
    public static string Serialize(this object objectInstance)
    {
      return JsonConvert.SerializeObject(objectInstance);
    }

    public static T Deserialize<T>(this string objectData)
    {
      return JsonConvert.DeserializeObject<T>(objectData);
    }
  }
}