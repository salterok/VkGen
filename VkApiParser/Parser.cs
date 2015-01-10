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
            http.DefaultRequestHeaders.Add("accept-language", "en");
            http.DefaultRequestHeaders.Add("cookie", "remixlang=3;");
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

                //group.ForEach(method => ParseApiMethod(method).Wait());
            }


            string ttttttt = String.Join("\n", v.Distinct());

            var t = 5;

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
            var doc = new HtmlDocument();
            doc.LoadHtml(response);
            var main = doc.DocumentNode.SelectSingleNode(Properties.Resources.API_METHOD_PAGE);
            if (main == null)
            {

            }
            var node = main.SelectSingleNode(Properties.Resources.API_METHOD_DESC);
            if (node == null)
            {
                return;
            }
            method.Description = node.InnerText.Trim();
            node = main.SelectSingleNode(Properties.Resources.API_METHOD_DETAILS);
            if (node == null)
            {
                return;
            }
            //FillMethodDetails(method, node.InnerText);
            // TODO: parse method parameters
            var nodes = main.SelectNodes(Properties.Resources.API_METHOD_PARAMS);
            if (nodes == null)
            {
                return;
            }
            foreach (var tNode in nodes)
            {
                var parameter = ParseMethodParameter(tNode);
                if (parameter == null)
                {
                    // exit even if only one parameter can't be parsed
                    return;
                }
                method.Parameters.Add(parameter);
            }
            node = main.SelectSingleNode(Properties.Resources.API_METHOD_RESULT);
            if (node == null)
            {
                return;
            }
            //FillMethodReturnType(method, node.InnerText
        }

        private Parameter ParseMethodParameter(HtmlNode pNode)
        {
            var parameter = new Parameter();
            var node = pNode.SelectSingleNode("td[contains(@class, 'dev_param_name')]");
            if (node == null)
            {
                return null;
            }
            parameter.Name = node.InnerText.Trim();
            node = pNode.SelectSingleNode("td[contains(@class, 'dev_param_desc')]");
            if (node == null)
            {
                return null;
            }
            parameter.Description = node.InnerText;
            node = node.SelectSingleNode("div[contains(@class, 'dev_param_opts')]");
            if (node == null)
            {
                return null;
            }
            var opts = node.InnerText;
            parameter.Type = ParseResultType(opts);

            return parameter;
            //throw new NotImplementedException();
        }
        List<string> v = new List<string>();
        private ResultType ParseResultType(string opts_text)
        {
            string type = null;
            var opts = opts_text.Split(',');
            v.AddRange(opts);
            foreach (var opt in opts)
            {
                //CheckParameterType(opt.Trim(), ref type);

            }

            var isList = opts_text.Contains("list comma-separated strings");


            return new ResultType(null);
        }

        private bool CheckParameterType(string opt, ref string type)
        {
            switch (opt)
            {
                case "positive number": type = "uint"; break;
                case "list comma-separated strings": break;
            }

            return true;
        }
        /// <summary>
        /// Parse <paramref name="text"/> and extract info about access_token|access_rights required.
        /// </summary>
        /// <param name="method">method object to fill</param>
        /// <param name="text">text to parse</param>
        private void FillMethodDetails(Method method, string text)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Parse <paramref name="text"/> and extract info about data type being returned.
        /// </summary>
        /// <param name="method">method object to fill</param>
        /// <param name="text">text to parse</param>
        private void FillMethodReturnType(Method method, string text)
        {
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
