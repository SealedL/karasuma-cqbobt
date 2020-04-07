namespace cqbot
{
    struct SharedText
    {
        public const string test = "";
        public const string HttpApiAdress = "http://127.0.0.1:5700/";
        public const string WebSocketEventAdress = "ws://127.0.0.1:6700/event";

        public const string Help = "用法：/[命令] [参数(可选)]\n" +
            "命令列表：\n" +
            "help       显示本帮助内容；\n\n" +
            "support    显示本项目GitHub仓库链接;\n\n" +
            "echo       复读命令后的内容。";
        public const string Support = "本项目GitHub仓库链接：" +
            "https://github.com/SealedL/karasuma-cqbot\n" +
            "欢迎帮助我改进程序\n";
    }
}