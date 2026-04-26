using System;
using System.Collections.Generic;
using Flow.Launcher.Plugin;

namespace Flow.Launcher.Plugin.Claude
{
    public class Claude : IPlugin
    {
        private PluginInitContext _context;

        private const string ClaudeUrl = "https://claude.ai/new?q=";
        private const string IncognitoArg = "&incognito";
        private const string QueryIconPath = "Icons/claude.png";

        private static readonly List<Result> EmptyResults = new();
    

        public void Init(PluginInitContext context)
        {
            _context = context;
        }

        public List<Result> Query(Query query)
        {
            if (string.IsNullOrWhiteSpace(query.Search))
            {
                return new List<Result>
                {
                    new() {
                        Title = "Ask Claude...",
                        SubTitle = "Type your prompt and press Enter",
                        IcoPath = QueryIconPath
                    }
                };
            }

            var results = new List<Result>
            {
                CreateNewResult(query.Search, QueryIconPath, CreateClaudeSearchUrl(ClaudeUrl, query)),
                CreateNewResult($"{query.Search} (Incognito Chat)", QueryIconPath, CreateClaudeSearchUrl(ClaudeUrl, query, IncognitoArg))
            };
            return results;
        }

        private Func<ActionContext, bool> CreateClaudeSearchUrl(string url, Query query, string args = "")
        {
            return _ =>
            {
                _context.API.OpenUrl($"{url}{Uri.EscapeDataString(query.Search)}{args}");
                return true;
            };
        }

        private Result CreateNewResult(string title, string iconPath, Func<ActionContext, bool> action)
        {
            var result = new Result
            {
                Title = title,
                IcoPath = iconPath,
                Action = action
            };
            return result;
        }
    }
}