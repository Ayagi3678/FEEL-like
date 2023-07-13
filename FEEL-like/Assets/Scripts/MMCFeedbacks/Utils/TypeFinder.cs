using System;
using System.Collections.Generic;
using System.Reflection;

namespace MMCFeedbacks.Utility
{
    public class TypeFinder
    {
        public static List<Type> FindClassesImplementingInterface<T>()
        {
            var implementingTypes = new List<Type>();

            // アセンブリ内のすべての型を取得
            var types = Assembly.GetExecutingAssembly().GetTypes();

            // 各型に対してインターフェースの継承をチェック
            foreach (var type in types)
                if (typeof(T).IsAssignableFrom(type) && type.IsClass)
                    implementingTypes.Add(type);

            return implementingTypes;
        }
    }
}