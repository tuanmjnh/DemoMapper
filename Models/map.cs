using System;
using System.Collections.Generic;
namespace DemoMapper.Extention {
    public static class Mapper {
        public static List<DemoMapper.Models.hocsinh> mapImage (this List<DemoMapper.Models.hocsinh> obj) {
            foreach (var item in obj) {
                if (string.IsNullOrEmpty (item.image))
                    item.image = "default";
            }
            return obj;
        }
    }
}