using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Loader;
using Microsoft.Extensions.DependencyModel;
namespace DemoMapper.Extention {
    public static class Mapper {
        // public static List<DemoMapper.Models.hocsinh> mapImage(this List<DemoMapper.Models.hocsinh> obj) {
        //     foreach (var item in obj) {
        //         if (string.IsNullOrEmpty(item.image))
        //             item.image = "default";
        //     }
        //     return obj;
        // }
        public static T DefaultProperties<T>(this T obj, Dictionary<string, string> Properties) where T : class {
            var type = typeof(T);
            var PropertyInfo = obj.GetType().GetProperties();
            foreach (var i in Properties) {
                var pty = PropertyInfo.FirstOrDefault(t => t.Name == i.Key);
                if (pty != null) {
                    var x = pty.GetValue(obj);
                    if (x == null)
                        pty.SetValue(obj, i.Value);
                    else {
                        if (string.IsNullOrEmpty(x.ToString()))
                            pty.SetValue(obj, i.Value);
                    }
                }
            }
            return obj;
        }
        public static List<T> DefaultProperties<T>(this List<T> obj, Dictionary<string, string> Properties) where T : class {
            foreach (var item in obj)
                item.DefaultProperties(Properties);
            return obj;
        }
        public static void ForEach<T>(this IList<T> enumeration, Action<T> action) {
            foreach (T item in enumeration) {
            action(item);
            }
        }
        // Get TypeProperties
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>> TypeProperties = new ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>>();
        private static List<PropertyInfo> TypePropertiesCache(Type type) {
            if (TypeProperties.TryGetValue(type.TypeHandle, out IEnumerable<PropertyInfo> pis)) {
                return pis.ToList();
            }

            var properties = type.GetProperties().ToArray();
            TypeProperties[type.TypeHandle] = properties;
            return properties.ToList();
        }
        // Get GetTableName
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, string> TypeTableName = new ConcurrentDictionary<RuntimeTypeHandle, string>();
        public delegate string TableNameMapperDelegate(Type type);
        public static TableNameMapperDelegate TableNameMapper;
        private static string GetTableName(Type type) {
            if (TypeTableName.TryGetValue(type.TypeHandle, out string name)) return name;

            if (TableNameMapper != null) {
                name = TableNameMapper(type);
            } else {
                //NOTE: This as dynamic trick should be able to handle both our own Table-attribute as well as the one in EntityFramework 
                var tableAttr = type
#if NETSTANDARD1_3
                    .GetTypeInfo()
#endif
                    .GetCustomAttributes(false).SingleOrDefault(attr => attr.GetType().Name == "TableAttribute") as dynamic;
                if (tableAttr != null) {
                    name = tableAttr.Name;
                } else {
                    name = type.Name + "s";
                    if (type.GetTypeInfo().IsInterface && name.StartsWith("I"))
                        name = name.Substring(1);
                }
            }
            TypeTableName[type.TypeHandle] = name;
            return name;
        }
    }
}