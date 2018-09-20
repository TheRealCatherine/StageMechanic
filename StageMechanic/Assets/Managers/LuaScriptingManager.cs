using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Platforms;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuaScriptingManager : MonoBehaviour
{
	public static LuaScriptingManager Instance;
	public const string Keywords = @"\b(and|break|do|else|elseif|end|false|for|function|if|in|local|nil|not|or|repeat|return|then|true|until|while)\b";
	public const string Classes = @"\b(blockmanager|itemmanager|playermanager|audiomanager|visualeffectsmanager|skyboxmanager|alert|coroutine|string|utf8|table|math|io|file|os|debug)\b";
	public const string Functions = @"\b(assert|collectgarbage|error|_G|getmetatable|ipairs|next|pairs|pcall|print|rawequal|rawget|rawlen|rawset|select|setmetatable|ronumber|tostring|type|_VERSION|xpcall"
		+ @"|coroutine\.create|coroutine\.isyieldable|coroutine\.resume|coroutine\.running|coroutine\.status|coroutine\.wrap|coroutine\.yield"
		+ @"|package\.congig|package\.cpath|package\.loaded|package\.loadlib|package\.path|package\.preload|package\.searchers|package\.searchpath" 
		+ @"|string\.byte|string\.char|string\.dump|string\.find|string\.format|string\.gmatch|string\.gsub|string\.len|string\.lower|string\.match|string\.pack|string\.packsize|string\.rep|string\.reverse|string\.sub|string\.unpack|string\.upper"
		+ @"|utf8\.char|utf8\.charpattern|utf8\.codes|utf8\.codepoint|utf8\.len|utf8\.offset"
		+ @"|table\.concat|table\.insert|table\.move|table\.pack|table\.remove|table\.sort|table\.unpack"
		+ @"|math\.abs|math\.acos|math\.asin|math\.atan|math\.ceil|math\.cos|math\.deg|math\.exp|math\.floor|math\.fmod|math\.hugh|math\.log|math\.max|math\.maxinteger|math\.min|math\.mininteger|math\.modf|math\.pi|math\.rad|math\.random|math\.randomseed|math\.sin|math\.sqrt|math\.tan|math\.tointeger|math\.type|math\.ult"
		+ @"|os\.clock|os\.date|os\.difftime|os\.time"
		+ @")\b";

	public const string Blacklist = @"\b("
		+ @"load|loadsafe|loadfile|loadfilesafe|dofile|require"
		+ @"|os\.execute|os\.exit|os\.getenv|os\.remove|os\.rename|os\.setlocale|os\.tmpname"
		+ @"|io\.close|io\.flush|io\.input|io\.lines|io\.open|io\.output|io\.popen|io\.read|io\.tmpfile|io\.type|io\.write|file:close|file:flush|file:lines|file:read|file:seek|file:setvbuf|file:write"
		+ @"|debug\.debug|debug\.gethook|debug\.getinfo|debug\.getlocal|debug\.getmetatable|debug\.getregistry|debug\.getupvalue|debug\.getuservalue|debug\.sethook|debug\.setlocal|debug\.setmetatable|debug\.setupvalue|debug\.setuservalue|debug\.traceback|debug\.upvalueid|debug\.upvaluejoin"
		+ @"\b)";

	public const string Types = @"\b(nil|boolean|number|string|userdata|function|thread|table)\b";

	private void Awake()
	{
		Instance = this;
	}

	void Start()
    {
		Script.GlobalOptions.Platform = new LimitedPlatformAccessor();

		UserData.RegisterProxyType<LuaProxy_AbstractBlock, AbstractBlock>(r => new LuaProxy_AbstractBlock(r));
		UserData.RegisterProxyType<LuaProxy_AbstractItem, AbstractItem>(r => new LuaProxy_AbstractItem(r));
		UserData.RegisterProxyType<LuaProxy_AbstractPlayerCharacter, AbstractPlayerCharacter>(r => new LuaProxy_AbstractPlayerCharacter(r));
		UserData.RegisterProxyType<LuaProxy_BlockManager, BlockManager>(_ => new LuaProxy_BlockManager());
		UserData.RegisterProxyType<LuaProxy_ItemManager, ItemManager>(_ => new LuaProxy_ItemManager());
		UserData.RegisterProxyType<LuaProxy_PlayerManager, PlayerManager>(_ => new LuaProxy_PlayerManager());
		UserData.RegisterProxyType<LuaProxy_AudioEffectsManager, AudioEffectsManager>(_ => new LuaProxy_AudioEffectsManager());
		UserData.RegisterType<Sprite>();
		UserData.RegisterType<Alert>();
		LuaCustomConverters.RegisterAll();
	}

	public static Script BaseScript
	{
		get
		{
			Script script = new Script(CoreModules.Preset_SoftSandbox);
			DynValue blockManager = UserData.Create(BlockManager.Instance);
			DynValue itemManager = UserData.Create(ItemManager.Instance);
			DynValue playerManager = UserData.Create(PlayerManager.Instance);
			DynValue audioEffectsManager = UserData.Create(AudioEffectsManager.Instance);
			DynValue alert = UserData.Create(new Alert());
			script.Globals.Set("blockmanager", blockManager);
			script.Globals.Set("itemmanager", itemManager);
			script.Globals.Set("playermanager", playerManager);
			script.Globals.Set("soundeffectsmanager", audioEffectsManager);
			script.Globals.Set("alert", alert);
			return script;
		}
	}
}
