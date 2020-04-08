namespace cqbot
{
    internal struct SharedContent
    {
        //API related strings
        public const string HttpApiPath = "http://127.0.0.1:5700/";
        public const string WebSocketEventPath = "ws://127.0.0.1:6700/event";
        
        //Commands related strings
        public const string Help = "用法：/[命令] [参数(可选)]\n" +
                                   "命令列表：\n" +
                                   "help       显示本帮助内容；\n\n" +
                                   "support    显示本项目GitHub仓库链接;\n\n" +
                                   "echo       复读命令后的内容。";
        public const string Support = "本项目GitHub仓库链接：" +
                                      "https://github.com/SealedL/karasuma-cqbot\n" +
                                      "欢迎帮助我改进程序。";
        public const string Comment = "您已成功送出一条留言。";
        
        //User ID related
        public const long MasterID = 921228653;
    }
}