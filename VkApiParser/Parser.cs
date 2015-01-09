using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using HtmlAgilityPack;

namespace VkApiParser
{
    public sealed class Parser
    {
        public delegate void NotifyStateDelegate(string text);
        public event NotifyStateDelegate NotifyState;

        private HttpClient http = new HttpClient();

        public Parser()
        {
            http.BaseAddress = new Uri(Properties.Resources.BASE_DEV_URL);
            http.DefaultRequestHeaders.Add("cookie", "remixlang=1;");
        }

        public async Task Parse()
        {
            var groups = await ParseApiListByGroups();
            if (groups == null)
            {
                // Notify("");
                return;
            }
            foreach (var group in groups)
            {
                Notify("Process " + group.Title + " group");
                var tasks = group.Select(method => ParseApiMethod(method));
                Task.WaitAll(tasks.ToArray());
            }

        }

        /// <summary>
        /// Load api list
        /// </summary>
        private async Task<MethodGroupList> ParseApiListByGroups()
        {
            string response = "";
            try
            {
                response = await http.GetStringAsync("methods");
            }
            catch (Exception ex)
            {
                return null;
            }
            var doc = new HtmlDocument();
            doc.LoadHtml(response);

            var nodes = doc.DocumentNode.SelectNodes(Properties.Resources.API_GROUPS);
            if (nodes == null)
            {
                //TODO: Notify about parse error
                return null;
            }
            var groups = new MethodGroupList();
            foreach (var node in nodes)
            {
                var header = node.SelectSingleNode(Properties.Resources.METHOD_GROUP_TITLE);
                if (header == null)
                {
                    return null;
                }
                var group = new MethodGroup();
                group.Title = header.InnerText.Trim();
                var aNodes = node.SelectNodes(Properties.Resources.GROUP_METHODS);
                if (aNodes == null)
                {
                    return null;
                }
                foreach (var aNode in aNodes)
                {
                    var method = new Method();
                    if (aNode.Attributes.Contains("href"))
                        method.Url = aNode.Attributes["href"].Value.Trim();
                    else
                    {
                        return null;
                    }
                    header = aNode.SelectSingleNode(Properties.Resources.METHOD_TITLE);
                    if (header == null)
                    {
                        return null;
                    }
                    method.Name = ExtractMethodName(header.InnerText.Trim());
                    group.Add(method);
                }
                groups.Add(group);
            }
            return groups;
        }

        private async Task ParseApiMethod(Method method)
        {
            string response = "";
            try
            {
                response = await http.GetStringAsync(method.Url);
                Notify("Response received for " + method.Name);
            }
            catch (Exception ex)
            {
                Notify("Failed to receive data for " + method.Url.Trim('/'));
                return;
            }



            throw new NotImplementedException();
        }



        /// <summary>
        /// Extract api method name from '{group}.{name}' string
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns></returns>
        private string ExtractMethodName(string fullName)
        {
            var index = fullName.LastIndexOf('.');
            return (index == -1 || index + 1 >= fullName.Length) ? fullName : fullName.Substring(index + 1);
        }

        private void Notify(string text)
        {
            if (NotifyState != null)
                NotifyState(text);
        }
    }
}
