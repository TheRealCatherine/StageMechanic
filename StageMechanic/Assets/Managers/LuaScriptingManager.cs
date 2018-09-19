using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Platforms;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuaScriptingManager : MonoBehaviour
{
	public static LuaScriptingManager Instance;
	public const string Keywords = @"\b(and|break|do|else|elseif|end|false|for|function|if|in|local|nil|not|or|repeat|return|then|true|until|while)\b";
	public const string Classes = @"\b(BlockManager|ItemManager|PlayerManager|AudioManager|VisualEffectsManager|SkyboxManager|Alert|coroutine|string|utf8|table|math|io|file|os|debug)\b";
	public const string Functions = @"\b(assert|collectgarbage|dofile|error|_G|getmetatable|ipairs|load|loadfile|next|pairs|pcall|print|rawequal|rawget|rawlen|rawset|select|setmetatable|ronumber|tostring|type|_VERSION|xpcall" +
		@"|coroutine.create|coroutine.isyieldable|coroutine.resume|coroutine.running|coroutine.status|coroutine.wrap|coroutine.yeld|"+
		@"|package.congig|package.cpath|package.loaded|package.loadlib|package.path|package.preload|package.searchers|package.searchpath" +
		@"|string.byte|string.char|string.dump|string.find|string.format|string.gmatch|string.gsub|string.len|string.lower|string.match|string.pack|string.packsize|string.rep|string.reverse|string.sub|string.unpack|string.upper" +
		@"|utf8.char|utf8.charpattern|utf8.codes|utf8.codepoint|utf8.len|utf8.offset" +
		@"|table.concat|table.insert|table.move|table.pack|table.remove|table.sort|table.unpack" +
		@"|math.abs|math.acos|math.asin|math.atan|math.ceil|math.cos|math.deg|math.exp|math.floor|math.fmod|math.hugh|math.log|math.max|math.maxinteger|math.min|math.mininteger|math.modf|math.pi|math.rad|math.random|math.randomseed|math.sin|math.sqrt|math.tan|math.tointeger|math.type|math.ult" +
		@"|io.cose|io.flush|io.input|io.lines|io.open|io.output|io.popen|io.read|io.tmpfile|io.type|io.write|file:close|file:flush|file:lines|file:read|file:seek|file:setvbuf|file:write" +
		@"|os.clock" + //TODO finish this list
		")\b";
	public const string Types = @"\b(nil|boolean|number|string|userdata|function|thread|table)\b";

	private void Awake()
	{
		Instance = this;
	}

	void Start()
    {
		Script.GlobalOptions.Platform = new LimitedPlatformAccessor();

		UserData.RegisterProxyType<LuaProxy_AbstractBlock, AbstractBlock>(r => new LuaProxy_AbstractBlock(r));
		UserData.RegisterProxyType<LuaProxy_BlockManager, BlockManager>(_ => new LuaProxy_BlockManager());
		UserData.RegisterType<AbstractPlayerCharacter>();
		UserData.RegisterType<Alert>();
		LuaCustomConverters.RegisterAll();
	}

	public static Script BaseScript
	{
		get
		{
			Script script = new Script(CoreModules.Preset_SoftSandbox);
			DynValue blockManager = UserData.Create(BlockManager.Instance);
			DynValue alert = UserData.Create(new Alert());
			script.Globals.Set("BlockManager", blockManager);
			script.Globals.Set("Alert", alert);
			return script;
		}
	}
}
