using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
 
 

#if UNITY_WINRT 
namespace System
{
#if NETFX_CORE 
  public class ApplicationException : Exception
  {
    public ApplicationException() : base() { }
    public ApplicationException(string s)
      : base(s)
    {

    }

    public ApplicationException(string s, Exception ex)
      : base(s, ex)
    { }
  }
#endif 



  public static class Extenders 
  {
    
    public static Type GetInterface ( this Type t, string stringtype )
    {

#if UNITY_METRO 
      Type target =  Type.GetType(stringtype);
      if ( t.IsAssignableFrom(target)) 
        return target; 
      else 
        return null ; 
#endif 

#if UNITY_WP8 
        return t.GetInterface ( stringtype, true ); 
#endif 

      throw new NotImplementedException(); 


    }

#if NETFX_CORE 
    public static PropertyInfo[] GetProperties(this Type t)
    {
        return t.GetRuntimeProperties().ToArray<PropertyInfo>(); 
    }

    public static FieldInfo[] GetFields (this Type t)
    {
      return t.GetRuntimeFields().ToArray<FieldInfo>();
    }

    public static bool IsAssignableFrom(this Type a, Type b)
    {
      return a.GetTypeInfo().IsAssignableFrom(b.GetTypeInfo());
    }

    public static MethodInfo GetMethod ( this Type t , string name, Type [] types ) 
    { 
       return t.GetRuntimeMethod( name , types  ) ;
    }

   

    public static bool GetIsEnum(this Type a)
    {
      return a.GetTypeInfo().IsEnum;
    }

    public static bool GetIsClass(this Type a)
    {
      return a.GetTypeInfo().IsClass;
    }
#endif 

  }
}
#endif 


namespace System.Collections.Specialized
{
  public interface IOrderedDictionary :
    System.Collections.ICollection,
    System.Collections.IEnumerable,
    System.Collections.IDictionary
  {
    object this[int idx] { get; set; }

    IDictionaryEnumerator GetEnumerator();
    void Insert(int idx, object key, object value);
    void RemoveAt(int idx);
  }

}

