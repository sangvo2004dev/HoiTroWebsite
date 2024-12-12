using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Web.WebPages;

namespace HoiTroWebsite.HTLibraries
{
    public class HanhChinhVn
    {
        private static readonly string Folder = "Hanhchinh_VietNam";

        public static JToken GetTinh_TpJsonByKey(string key)
        {
            if (key.IsEmpty()) return null;

            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Folder, "tinh_tp.json");
            if (!File.Exists(filePath)) { return null; }

            JToken jobject = JToken.Parse(File.ReadAllText(filePath))[key];

            return jobject;
        }

        public static JToken GetTinh_TpJsonBySlug(string slug)
        {
            JToken jobject = null;

            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Folder, "tinh_tp.json");
            if (!File.Exists(filePath)) { return null; }

            var json = JToken.Parse(File.ReadAllText(filePath));

            foreach (var item in json.Values())
            {
                if (item["slug"].ToString() == slug) 
                { 
                    jobject = item; 
                    break; 
                }
            }

            return jobject;
        }

        public static JToken GetQuanHuyenJsonByKey(string parentCode, string key)
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Folder, "quan-huyen", parentCode + ".json");
            if (!File.Exists(filePath)) { return null; }

            JToken jobject = JToken.Parse(File.ReadAllText(filePath))[key];
            return jobject;
        }

        public static JToken GetQuanHuyenJsonBySlug(string parentCode, string slug)
        {
            if (slug.IsEmpty()) { return null; }
            JToken jobject = null;

            slug = slug.Substring(slug.IndexOf("-") + 1);

            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Folder, "quan-huyen", parentCode + ".json");
            if (!File.Exists(filePath)) { return null; }

            var json = JToken.Parse(File.ReadAllText(filePath));

            foreach (var item in json.Values())
            {
                if (item["slug"].ToString() == slug) { jobject = item; break; }
            }

            return jobject;
        }

        public static JToken GetPhuongXaJsonByKey(string parentCode, string key)
        {
            JToken jobject = null;

            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Folder, "xa-phuong", parentCode + ".json");
            if (!File.Exists(filePath)) { return null; }

            jobject = JToken.Parse(File.ReadAllText(filePath))[key];

            return jobject;
        }

        public static JToken GetPhuongXaJsonBySlug(string parentCode, string slug)
        {
            if (slug.IsEmpty()) { return null; }
            slug = slug.Substring(slug.IndexOf("-") + 1);
            JToken jobject = null;

            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Folder, "xa-phuong", parentCode + ".json");
            if (!File.Exists(filePath)) { return null; }

            var json = JToken.Parse(File.ReadAllText(filePath));

            foreach (var item in json.Values())
            {
                if (item["slug"].ToString() == slug) { jobject = item; break; }
            }

            return jobject;
        }


        //public static string GetNameOfTinh_thanh_pho(string id)
        //{
        //    string name = "";
        //    string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Folder, "tinh_tp.json");
        //    if (!File.Exists(filePath)) { return name; }

        //    JObject json = JObject.Parse(File.ReadAllText(filePath));

        //    name = json[id]["name"].ToString();

        //    return name;
        //}

        //public static string GetNameOfTinh_thanh_pho(int id, string slug)
        //{
        //    string name = "";
        //    string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Folder, "tinh_tp.json");
        //    if (!File.Exists(filePath)) { return name; }

        //    JObject json = JObject.Parse(File.ReadAllText(filePath));
        //    foreach (var item in json.Values())
        //    {
        //        if (item["slug"].ToString() == slug)
        //        {
        //            name = item["slug"].ToString(); break;
        //        }
        //    }

        //    return name;
        //}
    }
}