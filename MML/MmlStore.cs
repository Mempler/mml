﻿using System.Text;
using System.Threading.Tasks;
using osu.Framework.IO.Stores;

namespace MML
{
    public class MmlStore : ResourceStore<byte[]>
    {
        public new Task<MmlDisplayContainer> GetAsync(string name) => Task.Run(() => Get(name));

        public MmlStore(IResourceStore<byte[]> store = null) : base(store)
        {
            AddExtension(@"mml");
            AddExtension(@"xml");
            AddExtension(@"xaml");
        }
        
        public new virtual MmlDisplayContainer Get(string name)
        {
            if (string.IsNullOrEmpty(name)) return null;

            var bytes = base.Get(name);
            var data = Encoding.Default.GetString(bytes).Trim();
            
            var byteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
            if (data.StartsWith(byteOrderMarkUtf8))
            {
                data = data.Remove(0, byteOrderMarkUtf8.Length);
            }

            var parser = new MmlParser(data);
            return new MmlDisplayContainer(parser);
        }
    }
}