/* WebHook 0.7 - https://github.com/willnode/WebViewHook/ - MIT */

using System;
using System.Collections.Generic;
using WebSocketSharp;
using WebSocketSharp.Server;

public class WebSocketHook : IDisposable
{

    public class Item
    {
        public readonly Func<string> Get;
        public readonly Action<string> Set;
        public readonly Action Bind;
        readonly Action<Item> _Update;
        
        public void Update() { _Update(this); }

        public Item(Func<string> get, Action<string> set, Action bind, Action<Item> update)
        {
            Get = get;
            Set = set;
            Bind = bind;
            _Update = update;
        }
    }

    public Dictionary<string, Item> sockets = new Dictionary<string, Item>();

    WebSocketServer server;

    WebViewHook webView;

    int port;

    public WebSocketHook(int port, WebViewHook webView)
    {
        // add WebSocketHook to webView too
        this.port = port;
        this.webView = webView;

        webView.LocationChanged += UpdateDefinitions;

        UpdateDefinitions("");

        server = new WebSocketServer("ws://127.0.0.1:" + port + "");
        server.AddWebSocketService<WSS>("/ws", (e) => { e.hook = this; });
        server.Start();
    }

    public void Dispose()
    {
        server.Stop();
        const string template = "window.socket{0}.close(); window.socket{0} = undefined";

        if (webView)
        {
            webView.LocationChanged -= UpdateDefinitions;
            webView.ExecuteJavascript(string.Format(template, port));
        }
    }

    void UpdateDefinitions(string url)
    {

        const string template = "window.socket{0} = window.socket{0} || new WebSocket('ws://127.0.0.1:{0}/ws');" +
            " function hook(){{ window.socket{0}.onopen = function(){{console.log('open');}}; window.socket{0}.onclose = function(e){{console.log('close' + e.code);}};  window.socket{0}.onerror = function(){{console.log('e');window.socket{0} = new WebSocket('ws://127.0.0.1:{0}/ws'); hook();}}; }};hook();";

        webView.ExecuteJavascript(string.Format(template, port));

        foreach (var item in sockets)
        {
            item.Value.Bind();
            item.Value.Update();
        }
    }

    public Item Add(string path, Func<string> get, Action<string> set)
    {

        const string template = "Object.defineProperty({0}, '{1}', {{ configurable: true, get: function(){{ return {0}.{1}___ }}, " +
            "set: function(v) {{ window.socket{2}.send('{0}.{1}=' + v) }} }})";

        const string template2 = "{0}.{1}___ = '{2}'";

        var dot = path.LastIndexOf('.');

        string first = dot < 0 ? "window" : path.Substring(0, dot);
        string last = dot < 0 ? path : path.Substring(dot + 1);
        string binding = string.Format(template, first, last, port);

        var hook = new Item(get, set, () => webView.ExecuteJavascript(binding), (x) => 
            webView.ExecuteJavascript(string.Format(template2, first, last, (x.Get() ?? "").Replace("'", "\'").Replace("\n", "\\n"))));

        sockets[first + "." + last] = hook;
        hook.Update();
        return hook;
    }
}

public class WSS : WebSocketBehavior
{
    protected override void OnMessage(MessageEventArgs e)
    {
        var n = e.Data;
        var i = n.IndexOf('=');
        var k = n.Substring(0, i);
        var v = n.Substring(i + 1);
        hook.sockets[k].Set(v);
    }

    public WebSocketHook hook;
}
